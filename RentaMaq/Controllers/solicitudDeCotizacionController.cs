using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using RentaMaq.DAL;
using RentaMaq.Models;

namespace RentaMaq.Controllers
{
    public class solicitudDeCotizacionController : Controller
    {
        private Context db = new Context();
        // GET: solicitudDeCotizacion
        public ActionResult Index()
        {

            DateTime fechaActual = DateTime.Now;
            DateTime fechaAnterior = DateTime.Now.AddMonths(-1);
            Context context = new Context();
            var L2EQuery = context.solicitudesDeCotizaciones.Where(s => s.fecha >= fechaAnterior && s.fecha <= fechaActual).OrderByDescending(s=>s.solicitudDeCotizacionID);
            var solicitudes = L2EQuery.ToList();
            ViewBag.fechaActual = Formateador.fechaCompletaToString(fechaActual);
            ViewBag.fechaAnterior = Formateador.fechaCompletaToString(fechaAnterior);
            //db.solicitudesDeCotizaciones.ToList()

            return View(solicitudes);
        }

        // GET: solicitudDeCotizacion/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            solicitudDeCotizacion solicitudDeCotizacion = db.solicitudesDeCotizaciones.Find(id);

            List<detalleSolicitudDeCotizacion> detalleSolicitudDeCotizacion = new List<detalleSolicitudDeCotizacion>();
            int idCotizacion = Convert.ToInt32(id);
            var L2EQuery = db.detalleSolicitudDeCotizaciones.Where(s => s.solicitudDeCotizacionID == idCotizacion);
            detalleSolicitudDeCotizacion = L2EQuery.ToList();

            ViewData["detalleSolicitudDeCotizacion"] = detalleSolicitudDeCotizacion;
            ViewBag.cantidadDetalle = detalleSolicitudDeCotizacion.Count;

            if (solicitudDeCotizacion == null)
            {
                return HttpNotFound();
            }
            return View(solicitudDeCotizacion);
        }

        // GET: solicitudDeCotizacion/Create
        public ActionResult Create(int? id)
        {
            if (Session["ID"] == null || !roles.tienePermiso(5, int.Parse(Session["ID"].ToString())))
            {
                return RedirectToAction("Index", "Home");
            }
            if (id != null) {
                id = Convert.ToInt32(id);
                pedidos pedidos = db.pedidos.Find(id);
                List<detallePedido> detallePedido = new List<detallePedido>();
                int idPedido = Convert.ToInt32(id);
                var L2EQuery = db.detallePedidos.Where(s => s.pedidosID == idPedido);
                detallePedido = L2EQuery.ToList();
                ViewData["detallePedido"] = detallePedido;                                                
            }


            ViewBag.fechaActual = Formateador.fechaCompletaToString(DateTime.Now);
            ViewData["Proveedores"] = db.Proveedores.ToList();
            //ViewData["Productos"] = db.Productos.ToList();
            ViewData["Equipos"] = equipos.todos();
            ViewBag.escritoPor = "DANNY BARAHONA TORRES";
            ViewBag.escritoPorCargo = "JEFE DEPTO. DE COMPRAS";             
            return View();
        }

        // POST: solicitudDeCotizacion/Create       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "solicitudDeCotizacionID,numeroSolicitudDeCotizacion,fecha,proveedor,emitidoPor,escritoPor,escritoPorCargo")] solicitudDeCotizacion solicitudDeCotizacion, FormCollection form)
        {

            if (Session["ID"] == null || !roles.tienePermiso(5, int.Parse(Session["ID"].ToString())))
            {
                return RedirectToAction("Index", "Home");
            }

            solicitudDeCotizacion.fecha = Formateador.fechaFormatoGuardar(form["fecha"].ToString());
            int solicitudDeCotizacionID = solicitudDeCotizacion.solicitudDeCotizacionID;

            string[] descripcionProducto = Request.Form.GetValues("descripcionProducto");
            string[] productos = Request.Form.GetValues("productos");
            string[] cantidad = Request.Form.GetValues("cantidad");
            string[] codigoInternoRentamaq = Request.Form.GetValues("codigoInternoRentamaq");
            string[] lugarDeFaena = Request.Form.GetValues("lugarDeFaena");
            string[] tipoCompra = Request.Form.GetValues("tipoCompra");


            for (int i = 0; i < descripcionProducto.Length; i++)
            {
                detalleSolicitudDeCotizacion detalle = new detalleSolicitudDeCotizacion();

                detalle.cantidad = Convert.ToDouble(cantidad[i]);
                detalle.codigoInterno = codigoInternoRentamaq[i];
                detalle.codigoProducto = productos[i];
                detalle.descripcionItem = descripcionProducto[i];
                detalle.solicitudDeCotizacionID = solicitudDeCotizacionID;
                detalle.lugarDeFaena = lugarDeFaena[i];
                detalle.tipoCompra = tipoCompra[i];
                detalle.numeroItem = i + 1;

                db.detalleSolicitudDeCotizaciones.Add(detalle);

            }
            db.solicitudesDeCotizaciones.Add(solicitudDeCotizacion);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: solicitudDeCotizacion/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Session["ID"] == null || !roles.tienePermiso(5, int.Parse(Session["ID"].ToString())))
            {
                return RedirectToAction("Index", "Home");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            solicitudDeCotizacion solicitudDeCotizacion = db.solicitudesDeCotizaciones.Find(id);

            List<detalleSolicitudDeCotizacion> detalleSolicitudDeCotizacion = new List<detalleSolicitudDeCotizacion>();

            int idCotizacion = Convert.ToInt32(id);
            var L2EQuery = db.detalleSolicitudDeCotizaciones.Where(s => s.solicitudDeCotizacionID == idCotizacion);

            detalleSolicitudDeCotizacion = L2EQuery.ToList();

            ViewData["detalleSolicitudDeCotizacion"] = detalleSolicitudDeCotizacion;
            ViewData["Proveedores"] = db.Proveedores.ToList();
            ViewData["Productos"] = db.Productos.ToList();
            ViewData["Equipos"] = equipos.todos();
            ViewBag.cantidadDetalle = detalleSolicitudDeCotizacion.Count;

            if (solicitudDeCotizacion == null || detalleSolicitudDeCotizacion==null)
            {
                return HttpNotFound();
            }
            return View(solicitudDeCotizacion);
        }
        
        // POST: solicitudDeCotizacion/Edit/5       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "solicitudDeCotizacionID,numeroSolicitudDeCotizacion,fecha,proveedor,emitidoPor,escritoPor,escritoPorCargo")] solicitudDeCotizacion solicitudDeCotizacion, FormCollection form)
        {
            if (Session["ID"] == null || !roles.tienePermiso(5, int.Parse(Session["ID"].ToString())))
            {
                return RedirectToAction("Index", "Home");
            }
            int solicitudDeCotizacionID = solicitudDeCotizacion.solicitudDeCotizacionID;

            solicitudDeCotizacion.fecha = Formateador.fechaFormatoGuardar(form["fecha"].ToString());

            var detalles = db.detalleSolicitudDeCotizaciones.Where(u => u.solicitudDeCotizacionID == solicitudDeCotizacionID);

            foreach (var detalle in detalles)
            {
                db.detalleSolicitudDeCotizaciones.Remove(detalle);
            }
            db.SaveChanges();

            string[] descripcionProducto = Request.Form.GetValues("descripcionProducto");
            string[] productos = Request.Form.GetValues("productos");
            string[] cantidad = Request.Form.GetValues("cantidad");
            string[] codigoInternoRentamaq = Request.Form.GetValues("codigoInternoRentamaq");
            string[] lugarDeFaena = Request.Form.GetValues("lugarDeFaena");
            string[] tipoCompra = Request.Form.GetValues("tipoCompra");


            for (int i = 0; i < descripcionProducto.Length; i++)
            {
                detalleSolicitudDeCotizacion detalle = new detalleSolicitudDeCotizacion();

                detalle.cantidad = Convert.ToDouble(cantidad[i]);
                detalle.codigoInterno = codigoInternoRentamaq[i];
                detalle.codigoProducto = productos[i];
                detalle.descripcionItem = descripcionProducto[i];
                detalle.solicitudDeCotizacionID = solicitudDeCotizacionID;
                detalle.lugarDeFaena = lugarDeFaena[i];
                detalle.tipoCompra = tipoCompra[i];
                detalle.numeroItem = i + 1;

                db.detalleSolicitudDeCotizaciones.Add(detalle);

            }

            db.Entry(solicitudDeCotizacion).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: solicitudDeCotizacion/Delete/5
        public ActionResult Delete(int? id)
        {
            if (Session["ID"] == null || !roles.tienePermiso(5, int.Parse(Session["ID"].ToString())))
            {
                return RedirectToAction("Index", "Home");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            solicitudDeCotizacion solicitudDeCotizacion = db.solicitudesDeCotizaciones.Find(id);

            List<detalleSolicitudDeCotizacion> detalleSolicitudDeCotizacion = new List<detalleSolicitudDeCotizacion>();
            int idCotizacion = Convert.ToInt32(id);
            var L2EQuery = db.detalleSolicitudDeCotizaciones.Where(s => s.solicitudDeCotizacionID == idCotizacion);
            detalleSolicitudDeCotizacion = L2EQuery.ToList();

            ViewData["detalleSolicitudDeCotizacion"] = detalleSolicitudDeCotizacion;           


            if (solicitudDeCotizacion == null)
            {
                return HttpNotFound();
            }
            return View(solicitudDeCotizacion);
        }

        // POST: solicitudDeCotizacion/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["ID"] == null || !roles.tienePermiso(5, int.Parse(Session["ID"].ToString())))
            {
                return RedirectToAction("Index", "Home");
            }
            solicitudDeCotizacion solicitudDeCotizacion = db.solicitudesDeCotizaciones.Find(id);
            db.solicitudesDeCotizaciones.Remove(solicitudDeCotizacion);
            
            var detalles = db.detalleSolicitudDeCotizaciones.Where(u => u.solicitudDeCotizacionID == id);
            foreach (var detalle in detalles)
            {
                db.detalleSolicitudDeCotizaciones.Remove(detalle);
            }

            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public string obtenerNombreProducto(string numeroDeParte)
        {
            var producto = db.Productos.SingleOrDefault(c => c.numeroDeParte == numeroDeParte);

            if (producto == null)
            {
                return "";
            }
            else
            {
                return producto.descripcion;

            }
        }
      
        public JsonResult Filtrar(string fecha1, string fecha2)
        {
            DateTime fecha_1 = Formateador.formatearFechaCompleta(fecha1);
            DateTime fecha_2 = Formateador.formatearFechaCompleta(fecha2);

            using (var db = new Context())
            {

                var L2EQuery = db.solicitudesDeCotizaciones.Where(s => s.fecha >= fecha_1 && s.fecha <= fecha_2).Select(s => new { s.solicitudDeCotizacionID, s.numeroSolicitudDeCotizacion,s.fecha, s.proveedor, s.escritoPor });
                var solicitud = L2EQuery.ToList();
                return Json(solicitud, JsonRequestBehavior.AllowGet);

            }
            
        }


    }
}
