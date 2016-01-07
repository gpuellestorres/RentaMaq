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
    public class pedidosController : Controller
    {
        private Context db = new Context();

        // GET: pedidos
        public ActionResult Index()
        {
            DateTime fechaActual = DateTime.Now;
            DateTime fechaAnterior = DateTime.Now.AddMonths(-1);
            Context context = new Context();
            var L2EQuery = context.pedidos.Where(s => s.fecha >= fechaAnterior && s.fecha <= fechaActual).OrderByDescending(s=>s.pedidosID);
            var pedidos = L2EQuery.ToList();
            ViewBag.fechaActual = Formateador.fechaCompletaToString(fechaActual);
            ViewBag.fechaAnterior = Formateador.fechaCompletaToString(fechaAnterior);
            return View(pedidos);
        }

        // GET: pedidos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            pedidos pedidos = db.pedidos.Find(id);
            List<detallePedido> detallePedido = new List<detallePedido>();
            int idPedido = Convert.ToInt32(id);
            var L2EQuery = db.detallePedidos.Where(s => s.pedidosID == idPedido);
            detallePedido = L2EQuery.ToList();

            ViewData["detallePedido"] = detallePedido;
            ViewBag.cantidadDetalle = detallePedido.Count;
            if (pedidos == null)
            {
                return HttpNotFound();
            }
            return View(pedidos);
        }

        public ActionResult pasarAPedido(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            /*pedidos pedidos = db.pedidos.Find(id);
            List<detallePedido> detallePedido = new List<detallePedido>();
            int idPedido = Convert.ToInt32(id);
            var L2EQuery = db.detallePedidos.Where(s => s.pedidosID == idPedido);
            detallePedido = L2EQuery.ToList();

            ViewData["detallePedido"] = detallePedido;
            ViewBag.cantidadDetalle = detallePedido.Count;
            if (pedidos == null)
            {
                return HttpNotFound();
            }

            ViewBag.fechaActual = Formateador.fechaCompletaToString(DateTime.Now);
            ViewData["Proveedores"] = db.Proveedores.ToList();            
            ViewData["Equipos"] = equipos.todos();
            ViewBag.escritoPor = "DANN BARAHONA TORRES";
            ViewBag.escritoPorCargo = "JEFE DEPTO. DE COMPRAS";*/

            return RedirectToAction("Create", "solicitudDeCotizacion", new { id = id });            
        }

        // GET: pedidos/Details/5
        public ActionResult Autorizar(int? id, string login)
        {
            if (Session["ID"]!=null && roles.tienePermiso(10,int.Parse(Session["ID"].ToString())))
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                pedidos pedidos = db.pedidos.Find(id);
                List<detallePedido> detallePedido = new List<detallePedido>();
                int idPedido = Convert.ToInt32(id);
                var L2EQuery = db.detallePedidos.Where(s => s.pedidosID == idPedido);
                detallePedido = L2EQuery.ToList();

                ViewData["detallePedido"] = detallePedido;
                ViewBag.cantidadDetalle = detallePedido.Count;
                if (pedidos == null)
                {
                    return HttpNotFound();
                }
                return View(pedidos);
            }
            else {
                if (!login.Equals(null) && login.Equals("true")) {
                    return RedirectToAction("Login", "Usuarios");
                }
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult Completada(int? id)
        {
            if (Session["ID"] != null && roles.tienePermiso(3, int.Parse(Session["ID"].ToString())))
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                pedidos pedidos = db.pedidos.Find(id);
                List<detallePedido> detallePedido = new List<detallePedido>();
                int idPedido = Convert.ToInt32(id);
                var L2EQuery = db.detallePedidos.Where(s => s.pedidosID == idPedido);
                detallePedido = L2EQuery.ToList();

                ViewData["detallePedido"] = detallePedido;
                ViewBag.cantidadDetalle = detallePedido.Count;
                if (pedidos == null)
                {
                    return HttpNotFound();
                }
                return View(pedidos);
            }
            else {
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult OrdenDeCompra(int? id)
        {
            if (Session["ID"] != null && roles.tienePermiso(3, int.Parse(Session["ID"].ToString())))
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                pedidos pedidos = db.pedidos.Find(id);
                List<detallePedido> detallePedido = new List<detallePedido>();
                int idPedido = Convert.ToInt32(id);
                var L2EQuery = db.detallePedidos.Where(s => s.pedidosID == idPedido);
                detallePedido = L2EQuery.ToList();

                ViewData["detallePedido"] = detallePedido;
                ViewBag.cantidadDetalle = detallePedido.Count;
                if (pedidos == null)
                {
                    return HttpNotFound();
                }
                return View(pedidos);
            }
            else {
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult Cotizacion(int? id)
        {
            if (Session["ID"] != null && roles.tienePermiso(3, int.Parse(Session["ID"].ToString())))
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                pedidos pedidos = db.pedidos.Find(id);
                List<detallePedido> detallePedido = new List<detallePedido>();
                int idPedido = Convert.ToInt32(id);
                var L2EQuery = db.detallePedidos.Where(s => s.pedidosID == idPedido);
                detallePedido = L2EQuery.ToList();

                ViewData["detallePedido"] = detallePedido;
                ViewBag.cantidadDetalle = detallePedido.Count;
                if (pedidos == null)
                {
                    return HttpNotFound();
                }
                return View(pedidos);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // GET: pedidos/Create
        public ActionResult Create()
        {
            if (Session["ID"] == null || !roles.tienePermiso(7, int.Parse(Session["ID"].ToString())))
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.fechaActual = Formateador.fechaCompletaToString(DateTime.Now);
            return View();
        }

        // POST: pedidos/Create       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "pedidosID,fecha,nota")] pedidos pedidos, FormCollection form)
        {
            if (Session["ID"] == null || !roles.tienePermiso(7, int.Parse(Session["ID"].ToString())))
            {
                return RedirectToAction("Index", "Home");
            }
            string[] numeroDeParte = Request.Form.GetValues("numeroDeParte");
            string[] cantidad = Request.Form.GetValues("cantidad");
            string[] tipoPedido = Request.Form.GetValues("tipoPedido");
            string[] detalleTipoPedido = Request.Form.GetValues("detalleTipoPedido");
            string[] prioridad = Request.Form.GetValues("prioridad");
            string[] descripcion = Request.Form.GetValues("descripcion");

            int pedidosID = pedidos.pedidosID;
            string verificadorUrgencia = "";
            for (int i = 0; i < numeroDeParte.Length; i++)
            {
                detallePedido detallePedido = new detallePedido();
                detallePedido.cantidad = Convert.ToDouble(cantidad[i]);
                detallePedido.detalleTipoPedido = detalleTipoPedido[i];
                detallePedido.numeroParte = numeroDeParte[i];
                detallePedido.pedidosID = pedidosID;
                detallePedido.prioridad = prioridad[i];
                detallePedido.tipoPedido = tipoPedido[i];
                detallePedido.descripcion = descripcion[i];
                if (prioridad[i].Equals("URGENTE"))
                {
                    verificadorUrgencia = "URGENTE";
                }
                db.detallePedidos.Add(detallePedido);

            }



            pedidos.fecha = Formateador.fechaFormatoGuardar(form["fecha"].ToString());
            pedidos.estado = "NUEVA";
            //if (ModelState.IsValid)
            //{
            db.pedidos.Add(pedidos);
            db.SaveChanges();

            enviarMail(pedidos.pedidosID);

            return RedirectToAction("Index");
            //}

            //return View(pedidos);
        }

        // GET: pedidos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Session["ID"] == null || !roles.tienePermiso(7, int.Parse(Session["ID"].ToString())))
            {
                return RedirectToAction("Index", "Home");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            pedidos pedidos = db.pedidos.Find(id);


            List<detallePedido> detallePedido = new List<detallePedido>();
            int idPedido = Convert.ToInt32(id);
            var L2EQuery = db.detallePedidos.Where(s => s.pedidosID == idPedido);
            detallePedido = L2EQuery.ToList();

            ViewData["detallePedido"] = detallePedido;
            ViewBag.cantidadDetalle = detallePedido.Count;

            if (pedidos == null)
            {
                return HttpNotFound();
            }
            return View(pedidos);
        }

        // POST: pedidos/Edit/5        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "pedidosID,fecha,nota,estado")] pedidos pedidos, FormCollection form)
        {
            if (Session["ID"] == null || !roles.tienePermiso(7, int.Parse(Session["ID"].ToString())))
            {
                return RedirectToAction("Index", "Home");
            }
            int pedidosID = pedidos.pedidosID;
            pedidos.fecha = Formateador.fechaFormatoGuardar(form["fecha"].ToString());

            var detalles = db.detallePedidos.Where(u => u.pedidosID == pedidosID);

            foreach (var detalle in detalles)
            {
                db.detallePedidos.Remove(detalle);
            }


            string[] numeroDeParte = Request.Form.GetValues("numeroDeParte");
            string[] cantidad = Request.Form.GetValues("cantidad");
            string[] tipoPedido = Request.Form.GetValues("tipoPedido");
            string[] detalleTipoPedido = Request.Form.GetValues("detalleTipoPedido");
            string[] prioridad = Request.Form.GetValues("prioridad");
            string[] descripcion = Request.Form.GetValues("descripcion");

            for (int i = 0; i < numeroDeParte.Length; i++)
            {
                detallePedido detallePedido = new detallePedido();
                detallePedido.cantidad = Convert.ToDouble(cantidad[i]);
                detallePedido.detalleTipoPedido = detalleTipoPedido[i];
                detallePedido.numeroParte = numeroDeParte[i];
                detallePedido.pedidosID = pedidosID;
                detallePedido.prioridad = prioridad[i];
                detallePedido.tipoPedido = tipoPedido[i];
                detallePedido.descripcion = descripcion[i];
                db.detallePedidos.Add(detallePedido);

            }

            db.Entry(pedidos).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");

        }

        // GET: pedidos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (Session["ID"] == null || !roles.tienePermiso(7, int.Parse(Session["ID"].ToString())))
            {
                return RedirectToAction("Index", "Home");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            pedidos pedidos = db.pedidos.Find(id);
            if (pedidos == null)
            {
                return HttpNotFound();
            }
            return View(pedidos);
        }

        // POST: pedidos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["ID"] == null || !roles.tienePermiso(7, int.Parse(Session["ID"].ToString())))
            {
                return RedirectToAction("Index", "Home");
            }
            pedidos pedidos = db.pedidos.Find(id);
            var detalles = db.detallePedidos.Where(u => u.pedidosID == id);

            foreach (var detalle in detalles)
            {
                db.detallePedidos.Remove(detalle);
            }
            db.pedidos.Remove(pedidos);
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult autorizarPedido(FormCollection form) {
            if (Session["ID"] == null || !roles.tienePermiso(9, int.Parse(Session["ID"].ToString())))
            {
                return RedirectToAction("Index", "Home");
            }

            int idPedido = Convert.ToInt32(form["pedidosID"].ToString());

            pedidos pedidos = db.pedidos.Find(idPedido);
            pedidos.estado = "AUTORIZADA";
            pedidos.nota = form["nota"].ToString();
            db.Entry(pedidos).State = EntityState.Modified;

            string verificador = "true";
            try
            {
                db.SaveChanges();
            }
            catch (Exception ex) {
                verificador = "false";
            }
            ViewBag.verificador = verificador;            
            return RedirectToAction("Index");
        }

        public ActionResult cambiarOrdenCompra(FormCollection form)
        {

            int idPedido = Convert.ToInt32(form["pedidosID"].ToString());

            pedidos pedidos = db.pedidos.Find(idPedido);
            pedidos.nota = form["nota"].ToString();
            pedidos.estado = "EN ORDEN DE COMPRA";
            db.Entry(pedidos).State = EntityState.Modified;

            string verificador = "true";
            try
            {
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                verificador = "false";
            }
            ViewBag.verificador = verificador;
            return RedirectToAction("Index");
        }

        public ActionResult cambiarCompletada(FormCollection form)
        {

            int idPedido = Convert.ToInt32(form["pedidosID"].ToString());

            pedidos pedidos = db.pedidos.Find(idPedido);
            pedidos.nota = form["nota"].ToString();
            pedidos.estado = "COMPLETADA";
            db.Entry(pedidos).State = EntityState.Modified;

            string verificador = "true";
            try
            {
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                verificador = "false";
            }
            ViewBag.verificador = verificador;
            return RedirectToAction("Index");
        }


        public ActionResult cambiarCotizacion(FormCollection form)
        {

            int idPedido = Convert.ToInt32(form["pedidosID"].ToString());

            pedidos pedidos = db.pedidos.Find(idPedido);
            pedidos.nota = form["nota"].ToString();
            pedidos.estado = "EN COTIZACION";
            db.Entry(pedidos).State = EntityState.Modified;

            string verificador = "true";
            try
            {
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                verificador = "false";
            }
            ViewBag.verificador = verificador;
            return RedirectToAction("Index");
        }

        private void enviarMail(int idPedido)
        {


            string texto = "Estimado/a usuario/a:"
                + ":<br><br>Le notificamos que acaba de registrarse un pedido de productos en el sistema Renta-Maq"

                + "<br><br>El pedido contiene los siguientes productos en su detalle:<br><br>";

            List<detallePedido> DetallePedido = db.detallePedidos.Where(s => s.pedidosID == idPedido).ToList();

            foreach (detallePedido Detalle in DetallePedido)
            {
                texto += Detalle.numeroParte + " - " + Detalle.cantidad + "<br />" + " - " + Detalle.descripcion + "<br />";
            }

            texto += "<br><br>Para ver el detalle del pedido visite el "
            + "siguiente enlace:<br><br>http://rentamaq.azurewebsites.net"
            + Url.Action("Details", "pedidos", new { id = idPedido, login = "true" });

            texto += "<br><br>Atentamente,<br>puntodesarrollo ltda.";

            List<string> correos =  roles.correosAlertaPedido();

            RentaMaq.Models.envioCorreos.enviarAlerta(correos, "Nuevo Pedido en Sistema", texto);
        }

        public JsonResult Filtrar(string fecha1, string fecha2, string estadoPedido)
        {
            DateTime fecha_1 = Formateador.formatearFechaCompleta(fecha1);
            DateTime fecha_2 = Formateador.formatearFechaCompleta(fecha2);

            if (estadoPedido.Equals("TODAS"))
            {
                using (var db = new Context())
                {

                    var L2EQuery = db.pedidos.Where(s => s.fecha >= fecha_1 && s.fecha <= fecha_2).Select(s => new { s.pedidosID,s.fecha, s.nota, s.estado});
                    var pedidos = L2EQuery.ToList();
                    return Json(pedidos, JsonRequestBehavior.AllowGet);

                }
            }
            else {
                using (var db = new Context())
                {

                    var L2EQuery = db.pedidos.Where(s => s.fecha >= fecha_1 && s.fecha <= fecha_2 && s.estado == estadoPedido).Select(s => new { s.pedidosID, s.fecha, s.nota, s.estado });
                    var pedidos = L2EQuery.ToList();
                    return Json(pedidos, JsonRequestBehavior.AllowGet);

                }
            }            
        }
    }
}
