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
    public class ordenDeCompraArriendoEquipoController : Controller
    {

        //nota entrega, editar entrega, cancelar
        private Context db = new Context();
        private int numeroPermiso = 4;

        // GET: ordenDeCompraArriendoEquipo
        public ActionResult Index(string inicio, string termino, string estado)
        {
            ViewBag.Fecha1 = DateTime.Now.AddMonths(-1);
            ViewBag.Fecha2 = DateTime.Now.AddDays(7);
            ViewBag.Estado = "TODOS";
            if (inicio != null && termino != null)
            {
                string[] inicioSeparado = inicio.Split('-');
                string[] terminoSeparado = termino.Split('-');

                ViewBag.Fecha1 = new DateTime(int.Parse(inicioSeparado[2]), int.Parse(inicioSeparado[1]), int.Parse(inicioSeparado[0]));
                ViewBag.Fecha2 = new DateTime(int.Parse(terminoSeparado[2]), int.Parse(terminoSeparado[1]), int.Parse(terminoSeparado[0]));
            }
            if (estado != null)
            {
                ViewBag.Estado = estado;
            }
            DateTime Inicio = ViewBag.Fecha1;
            DateTime Termino = ViewBag.Fecha2;
            string Estado = ViewBag.Estado;
            DateTime temp = ViewBag.Fecha2;

            while (temp.Day == ViewBag.Fecha2.Day)
            {
                Termino = temp;
                temp = temp.AddMinutes(1);
            }
            if (Estado.Equals("TODOS"))
            {
                return View(db.ordenDeCompraArriendoEquipoes.Where(s => s.fecha >= Inicio && s.fecha <= Termino).ToList());
            }
            else if (Estado.Equals("PENDIENTE")) { 
                List<ordenDeCompraArriendoEquipo> listaOrdenCompraArriendoEquipo =db.ordenDeCompraArriendoEquipoes.Where(s => s.fecha >= Inicio && s.fecha <= Termino && s.estado=="ENTREGADA").ToList();
                List<ordenDeCompraArriendoEquipo> listaFinalOC = new List<ordenDeCompraArriendoEquipo>();
                foreach (ordenDeCompraArriendoEquipo OC in listaOrdenCompraArriendoEquipo) {

                    if (ordenDeCompraArriendoEquipo.cantidadEquiposFaltantes(OC.ordenDeCompraArriendoEquipoID) > 0) {
                        listaFinalOC.Add(OC);
                    }
                }
                return View(listaFinalOC);
                //db.detalleOrdenCompraArriendoEquipos.Where(s => s.)    
            }
            else
            {
                return View(db.ordenDeCompraArriendoEquipoes.Where(s => s.fecha >= Inicio && s.fecha <= Termino && s.estado == Estado).ToList());
            }
        }

        // GET: ordenDeCompraArriendoEquipo/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            List<detalleOrdenDeCompraArriendoEquipo> detalleOrdenDeCompraArriendoEquipo = new List<detalleOrdenDeCompraArriendoEquipo>();

            int idOC = Convert.ToInt32(id);
            var L2EQuery = db.detalleOrdenCompraArriendoEquipos.Where(s => s.ordenDeCompraArriendoEquipoID == idOC);

            detalleOrdenDeCompraArriendoEquipo = L2EQuery.ToList();

            ViewData["detalleOrdenDeCompraArriendoEquipo"] = detalleOrdenDeCompraArriendoEquipo;

            ordenDeCompraArriendoEquipo ordenDeCompraArriendoEquipo = db.ordenDeCompraArriendoEquipoes.Find(id);

            int ProveedorID = ordenDeCompraArriendoEquipo.ProveedorID;
            Proveedor Proveedor = db.Proveedores.Find(ProveedorID);
            ViewBag.nombreProveedor = Proveedor.nombreProveedor;

            if (ordenDeCompraArriendoEquipo == null)
            {
                return HttpNotFound();
            }
            return View(ordenDeCompraArriendoEquipo);
        }

        // GET: ordenDeCompraArriendoEquipo/Create
        public ActionResult Create()
        {
            if (Session["ID"] == null || !roles.tienePermiso(numeroPermiso, int.Parse(Session["ID"].ToString())))
            {

                return RedirectToAction("Index", "Home");
            }

            ViewBag.fechaActual = Formateador.fechaCompletaToString(DateTime.Now);
            ViewData["Proveedores"] = db.Proveedores.ToList();
            return View();
        }

        // POST: ordenDeCompraArriendoEquipo/Create       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ordenDeCompraArriendoEquipoID,numeroOrdenCompraArriendoEquipo,ProveedorID,texto1,texto2,plazoEntrega,personaRetira,formaPago,fecha,tipoHorasMinimas,noIncluye")] ordenDeCompraArriendoEquipo ordenDeCompraArriendoEquipo,
            FormCollection form)
        {

            if (Session["ID"] == null || !roles.tienePermiso(numeroPermiso, int.Parse(Session["ID"].ToString())))
            {

                return RedirectToAction("Index", "Home");
            }
            int ordenDeCompraArriendoEquipoID = ordenDeCompraArriendoEquipo.ordenDeCompraArriendoEquipoID;
            ordenDeCompraArriendoEquipo.anio = DateTime.Now.Year;

            //DateTime PlazoEntrega = Formateador.fechaFormatoGuardar(form["plazoEntrega"]);
            if (!string.IsNullOrEmpty(form["plazoEntrega"]))
            {
                ordenDeCompraArriendoEquipo.plazoEntrega = Formateador.fechaFormatoGuardar(form["plazoEntrega"]);
            }
            else 
            {
                ordenDeCompraArriendoEquipo.plazoEntrega = new DateTime(2000, 1, 1);
            }

            ordenDeCompraArriendoEquipo.fechaLlegadaReal = new DateTime(2000,1,1);
            ordenDeCompraArriendoEquipo.fecha = Formateador.fechaFormatoGuardar(form["fecha"].ToString());

            ordenDeCompraArriendoEquipo.dirigidoA = db.Proveedores.Find(ordenDeCompraArriendoEquipo.ProveedorID).personaContacto1;

            ordenDeCompraArriendoEquipo.estado = "NUEVA";
            

            string[] descripcionProducto = Request.Form.GetValues("descripcionProducto");
            string[] valorHora = Request.Form.GetValues("valorHora");
            string[] horasMinimasMensuales = Request.Form.GetValues("horasMinimasMensuales");
            string[] duracionDelArriendo = Request.Form.GetValues("duracionDelArriendo");
            string[] lugarFaena = Request.Form.GetValues("lugarFaena");
            string[] condicionesPago = Request.Form.GetValues("condicionesPago");



            for (int i = 0; i < descripcionProducto.Length; i++)
            {
                detalleOrdenDeCompraArriendoEquipo detalle = new detalleOrdenDeCompraArriendoEquipo();

                detalle.ordenDeCompraArriendoEquipoID = ordenDeCompraArriendoEquipoID;
                detalle.horasMinimasMensuales = Convert.ToInt32(horasMinimasMensuales[i]);
                detalle.condicionesDePago = condicionesPago[i];
                detalle.descripcionEquipo = descripcionProducto[i];
                detalle.duracionArriendo = duracionDelArriendo[i];
                detalle.lugarDeFaena = lugarFaena[i];                
                detalle.numeroItem = i + 1;
                detalle.valorHora = valorHora[i];

                db.detalleOrdenCompraArriendoEquipos.Add(detalle);

            }


           // if (ModelState.IsValid)
            //{
                db.ordenDeCompraArriendoEquipoes.Add(ordenDeCompraArriendoEquipo);
                db.SaveChanges();
                return RedirectToAction("Index");
            //}

            //return View(ordenDeCompraArriendoEquipo);
        }

        // GET: ordenDeCompraArriendoEquipo/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Session["ID"] == null || !roles.tienePermiso(numeroPermiso, int.Parse(Session["ID"].ToString())))
            {

                return RedirectToAction("Index", "Home");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewData["Proveedores"] = db.Proveedores.ToList();

            List<detalleOrdenDeCompraArriendoEquipo> detalleOrdenDeCompraArriendoEquipo = new List<detalleOrdenDeCompraArriendoEquipo>();

            int idOC = Convert.ToInt32(id);
            var L2EQuery = db.detalleOrdenCompraArriendoEquipos.Where(s => s.ordenDeCompraArriendoEquipoID == idOC);

            detalleOrdenDeCompraArriendoEquipo = L2EQuery.ToList();

            ViewData["detalleOrdenDeCompraArriendoEquipo"] = detalleOrdenDeCompraArriendoEquipo;

            ordenDeCompraArriendoEquipo ordenDeCompraArriendoEquipo = db.ordenDeCompraArriendoEquipoes.Find(id);
            if (ordenDeCompraArriendoEquipo == null)
            {
                return HttpNotFound();
            }
            return View(ordenDeCompraArriendoEquipo);
        }

        // POST: ordenDeCompraArriendoEquipo/Edit/5       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ordenDeCompraArriendoEquipoID,numeroOrdenCompraArriendoEquipo,ProveedorID,estado,anio,texto1,texto2,plazoEntrega,personaRetira,formaPago,fecha,tipoHorasMinimas,noIncluye")] ordenDeCompraArriendoEquipo ordenDeCompraArriendoEquipo, FormCollection form)
        {

            if (Session["ID"] == null || !roles.tienePermiso(numeroPermiso, int.Parse(Session["ID"].ToString())))
            {

                return RedirectToAction("Index", "Home");
            }

            int ordenDeCompraArriendoEquipoID = ordenDeCompraArriendoEquipo.ordenDeCompraArriendoEquipoID;
            var detalles = db.detalleOrdenCompraArriendoEquipos.Where(u => u.ordenDeCompraArriendoEquipoID == ordenDeCompraArriendoEquipoID);

            foreach (var detalle in detalles)
            {
                db.detalleOrdenCompraArriendoEquipos.Remove(detalle);
            }
            //db.SaveChanges();

            string[] descripcionProducto = Request.Form.GetValues("descripcionProducto");
            string[] valorHora = Request.Form.GetValues("valorHora");
            string[] horasMinimasMensuales = Request.Form.GetValues("horasMinimasMensuales");
            string[] duracionDelArriendo = Request.Form.GetValues("duracionDelArriendo");
            string[] lugarFaena = Request.Form.GetValues("lugarFaena");
            string[] condicionesPago = Request.Form.GetValues("condicionesPago");


            for (int i = 0; i < descripcionProducto.Length; i++)
            {
                detalleOrdenDeCompraArriendoEquipo detalle = new detalleOrdenDeCompraArriendoEquipo();

                detalle.ordenDeCompraArriendoEquipoID = ordenDeCompraArriendoEquipoID;
                detalle.horasMinimasMensuales = Convert.ToInt32(horasMinimasMensuales[i]);
                detalle.condicionesDePago = condicionesPago[i];
                detalle.descripcionEquipo = descripcionProducto[i];
                detalle.duracionArriendo = duracionDelArriendo[i];
                detalle.lugarDeFaena = lugarFaena[i];
                detalle.numeroItem = i + 1;
                detalle.valorHora = valorHora[i];

                db.detalleOrdenCompraArriendoEquipos.Add(detalle);

            }

            //EDITAR FECHAS
            if (!string.IsNullOrEmpty(form["plazoEntrega"]))
            {
                ordenDeCompraArriendoEquipo.plazoEntrega = Formateador.fechaFormatoGuardar(form["plazoEntrega"]);
            }
            else
            {
                ordenDeCompraArriendoEquipo.plazoEntrega = new DateTime(2000, 1, 1);
            }
            ordenDeCompraArriendoEquipo.fecha = Formateador.fechaFormatoGuardar(form["fecha"].ToString());
            ordenDeCompraArriendoEquipo.fechaLlegadaReal = new DateTime(2000, 1, 1);

            ordenDeCompraArriendoEquipo.dirigidoA = 
                db.Proveedores.Find(ordenDeCompraArriendoEquipo.ProveedorID).personaContacto1;

            //if (ModelState.IsValid)
            //{
                db.Entry(ordenDeCompraArriendoEquipo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            //}
            //return View(ordenDeCompraArriendoEquipo);
        }

        // GET: ordenDeCompraArriendoEquipo/Delete/5
        public ActionResult Delete(int? id)
        {
            if (Session["ID"] == null || !roles.tienePermiso(numeroPermiso, int.Parse(Session["ID"].ToString())))
            {

                return RedirectToAction("Index", "Home");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ordenDeCompraArriendoEquipo ordenDeCompraArriendoEquipo = db.ordenDeCompraArriendoEquipoes.Find(id);

            List<detalleOrdenDeCompraArriendoEquipo> detalleOrdenDeCompraArriendoEquipo = new List<detalleOrdenDeCompraArriendoEquipo>();

            int idOC = Convert.ToInt32(id);
            var L2EQuery = db.detalleOrdenCompraArriendoEquipos.Where(s => s.ordenDeCompraArriendoEquipoID == idOC);

            detalleOrdenDeCompraArriendoEquipo = L2EQuery.ToList();

            ViewData["detalleOrdenDeCompraArriendoEquipo"] = detalleOrdenDeCompraArriendoEquipo;

            int ProveedorID = ordenDeCompraArriendoEquipo.ProveedorID;
            Proveedor Proveedor = db.Proveedores.Find(ProveedorID);
            ViewBag.nombreProveedor = Proveedor.nombreProveedor;

            if (ordenDeCompraArriendoEquipo == null)
            {
                return HttpNotFound();
            }
            return View(ordenDeCompraArriendoEquipo);
        }

        // POST: ordenDeCompraArriendoEquipo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {

            if (Session["ID"] == null || !roles.tienePermiso(numeroPermiso, int.Parse(Session["ID"].ToString())))
            {

                return RedirectToAction("Index", "Home");
            }

            var detalles = db.detalleOrdenCompraArriendoEquipos.Where(u => u.ordenDeCompraArriendoEquipoID == id);

            foreach (var detalle in detalles)
            {
                db.detalleOrdenCompraArriendoEquipos.Remove(detalle);
            }
            db.SaveChanges();

            ordenDeCompraArriendoEquipo ordenDeCompraArriendoEquipo = db.ordenDeCompraArriendoEquipoes.Find(id);
            db.ordenDeCompraArriendoEquipoes.Remove(ordenDeCompraArriendoEquipo);
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







        public ActionResult registroIngresoOrdenCompraArriendoEquipo(int id)
        {
            if (Session["ID"] == null || !roles.tienePermiso(numeroPermiso, int.Parse(Session["ID"].ToString())))
            {

                return RedirectToAction("Index", "Home");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            List<detalleOrdenDeCompraArriendoEquipo> detalleOrdenDeCompraArriendoEquipo = new List<detalleOrdenDeCompraArriendoEquipo>();

            int idOC = Convert.ToInt32(id);
            var L2EQuery = db.detalleOrdenCompraArriendoEquipos.Where(s => s.ordenDeCompraArriendoEquipoID == idOC);

            detalleOrdenDeCompraArriendoEquipo = L2EQuery.ToList();

            ViewData["detalleOrdenDeCompraArriendoEquipo"] = detalleOrdenDeCompraArriendoEquipo;

            ordenDeCompraArriendoEquipo ordenDeCompraArriendoEquipo = db.ordenDeCompraArriendoEquipoes.Find(id);

            int ProveedorID = ordenDeCompraArriendoEquipo.ProveedorID;
            Proveedor Proveedor = db.Proveedores.Find(ProveedorID);
            ViewBag.nombreProveedor = Proveedor.nombreProveedor;

            if (ordenDeCompraArriendoEquipo == null)
            {
                return HttpNotFound();
            }
            return View(ordenDeCompraArriendoEquipo);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult registroIngresoOrdenCompraArriendoEquipo(FormCollection form)
        {
            if (Session["ID"] == null || !roles.tienePermiso(numeroPermiso, int.Parse(Session["ID"].ToString())))
            {

                return RedirectToAction("Index", "Home");
            }

             string[] ingresoEquipo = Request.Form.GetValues("ingresoEquipo");
             string[] numeroItem = Request.Form.GetValues("numeroItem");
             DateTime fechaIngresoEquipo = Formateador.fechaFormatoGuardar(form["fechaIngresoEquipo"].ToString());
             int ordenDeCompraArriendoEquipoID = int.Parse(form["ordenDeCompraArriendoEquipoID"].ToString());
            
             string nota = (string)form["nota"];

             ordenDeCompraArriendoEquipo ordenDeCompraArriendoEquipo = db.ordenDeCompraArriendoEquipoes.Find(ordenDeCompraArriendoEquipoID);
             ordenDeCompraArriendoEquipo.nota = nota;
             ordenDeCompraArriendoEquipo.estado = "ENTREGADA";
             ordenDeCompraArriendoEquipo.fechaLlegadaReal = fechaIngresoEquipo;

             db.Entry(ordenDeCompraArriendoEquipo).State = EntityState.Modified;
             db.SaveChanges();
            
             if (!Object.ReferenceEquals(null, ingresoEquipo)) 
             {
                 ordenDeCompraArriendoEquipo.eliminarDetalle(ordenDeCompraArriendoEquipoID);
                 for (int i = 0; i < ingresoEquipo.Length; i++)
                 {
                     if (ingresoEquipo[i].Equals("SI"))
                         ordenDeCompraArriendoEquipo.ingresoOrdenCompraArriendoEquipo(ordenDeCompraArriendoEquipoID, int.Parse(numeroItem[i].ToString()), fechaIngresoEquipo);
                 }
             }
                         
            return RedirectToAction("Index");
        }


        public JsonResult Filtrar(string fecha1, string fecha2)
        {
            DateTime fecha_1 = Formateador.formatearFechaCompleta(fecha1);
            DateTime fecha_2 = Formateador.formatearFechaCompleta(fecha2);

            using (var context = new Context())
            {
               
                    var L2EQuery = context.ordenDeCompraArriendoEquipoes.Where(s => s.fecha >= fecha_1 && s.fecha <= fecha_2);
                    var student = L2EQuery.ToList();
                    return Json(student, JsonRequestBehavior.AllowGet);                

            }

            return null;
        }


        public ActionResult Cancelar(int id) {
            if (Session["ID"] == null || !roles.tienePermiso(numeroPermiso, int.Parse(Session["ID"].ToString())))
            {

                return RedirectToAction("Index", "Home");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }            

            List<detalleOrdenDeCompraArriendoEquipo> detalleOrdenDeCompraArriendoEquipo = new List<detalleOrdenDeCompraArriendoEquipo>();

            int idOC = Convert.ToInt32(id);
            var L2EQuery = db.detalleOrdenCompraArriendoEquipos.Where(s => s.ordenDeCompraArriendoEquipoID == idOC);

            detalleOrdenDeCompraArriendoEquipo = L2EQuery.ToList();

            ViewData["detalleOrdenDeCompraArriendoEquipo"] = detalleOrdenDeCompraArriendoEquipo;

            ordenDeCompraArriendoEquipo ordenDeCompraArriendoEquipo = db.ordenDeCompraArriendoEquipoes.Find(id);
            if (ordenDeCompraArriendoEquipo == null)
            {
                return HttpNotFound();
            }
            return View(ordenDeCompraArriendoEquipo);            
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Cancelar(FormCollection form)
        {
            if (Session["ID"] == null || !roles.tienePermiso(numeroPermiso, int.Parse(Session["ID"].ToString())))
            {

                return RedirectToAction("Index", "Home");
            }

            int id = int.Parse(form["ordenDeCompraArriendoEquipoID"].ToString());
            ordenDeCompraArriendoEquipo ordenCompraArriendoEquipo = db.ordenDeCompraArriendoEquipoes.Find(id);
            ordenCompraArriendoEquipo.estado = "CANCELAR";
            db.Entry(ordenCompraArriendoEquipo).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Index");
        }




    }
}
