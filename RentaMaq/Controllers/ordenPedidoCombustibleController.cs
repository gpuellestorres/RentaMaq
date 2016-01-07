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
    public class ordenPedidoCombustibleController : Controller
    {
        private Context db = new Context();

        // GET: ordenPedidoCombustible
        public ActionResult Index(string inicio, string termino)
        {
            ViewBag.Fecha1 = DateTime.Now.AddMonths(-1);
            ViewBag.Fecha2 = DateTime.Now.AddDays(7);
            if (inicio != null && termino != null)
            {
                string[] inicioSeparado = inicio.Split('-');
                string[] terminoSeparado = termino.Split('-');

                ViewBag.Fecha1 = new DateTime(int.Parse(inicioSeparado[2]), int.Parse(inicioSeparado[1]), int.Parse(inicioSeparado[0]));
                ViewBag.Fecha2 = new DateTime(int.Parse(terminoSeparado[2]), int.Parse(terminoSeparado[1]), int.Parse(terminoSeparado[0]));
            }

            DateTime Inicio = ViewBag.Fecha1;
            DateTime Termino = ViewBag.Fecha2;
            Termino = Termino.AddDays(1);
            return View(db.ordenesPedidoCombustible.Where(s => s.fecha >= Inicio && s.fecha <= Termino).ToList());
        }

        // GET: ordenPedidoCombustible/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ordenPedidoCombustible ordenPedidoCombustible = db.ordenesPedidoCombustible.Find(id);
            List<detalleOrdenPedidoCombustible> detalleOrdenPedidoCombustible = new List<detalleOrdenPedidoCombustible>();

            int idOrden = Convert.ToInt32(id);
            var L2EQuery = db.detalleOrdenesPedidosCombustible.Where(s => s.ordenPedidoCombustibleID == idOrden);

            detalleOrdenPedidoCombustible = L2EQuery.ToList();

            ViewData["detalleOrdenPedidoCombustible"] = detalleOrdenPedidoCombustible;
            ViewBag.cantidadDetalle = detalleOrdenPedidoCombustible.Count;
            if (ordenPedidoCombustible == null)
            {
                return HttpNotFound();
            }
            return View(ordenPedidoCombustible);
        }

        // GET: ordenPedidoCombustible/Create
        public ActionResult Create()
        {
            if (Session["ID"] == null || !roles.tienePermiso(8, int.Parse(Session["ID"].ToString())))
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.fechaActual = Formateador.fechaCompletaToString(DateTime.Now);
            return View();
        }

        // POST: ordenPedidoCombustible/Create       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ordenPedidoCombustibleID,numeroOrdenPedido,destinatario,fecha,nombreQuienAutoriza,encabezado")] ordenPedidoCombustible ordenPedidoCombustible, FormCollection form)
        {
            if (Session["ID"] == null || !roles.tienePermiso(8, int.Parse(Session["ID"].ToString())))
            {
                return RedirectToAction("Index", "Home");
            }
            string[] trabajoRealizar = Request.Form.GetValues("trabajoRealizar");
            string[] personaResponsable = Request.Form.GetValues("personaResponsable");
            string[] detalle = Request.Form.GetValues("detalle");
            string[] cantidad = Request.Form.GetValues("cantidad");

            ordenPedidoCombustible.fecha = Formateador.fechaFormatoGuardar(form["fecha"].ToString());
            ordenPedidoCombustible.anio = ordenPedidoCombustible.fecha.Year;
            for (int i = 0; i < detalle.Length; i++)
            {
                detalleOrdenPedidoCombustible detalleOrden = new detalleOrdenPedidoCombustible();
                detalleOrden.cantidad = cantidad[i];
                detalleOrden.detalle = detalle[i];
                detalleOrden.personaResponsable = personaResponsable[i];
                detalleOrden.trabajoRealizar = trabajoRealizar[i];
                detalleOrden.ordenPedidoCombustibleID = ordenPedidoCombustible.ordenPedidoCombustibleID;
                db.detalleOrdenesPedidosCombustible.Add(detalleOrden);
            }
                                       
            if (ModelState.IsValid)
            {
                db.ordenesPedidoCombustible.Add(ordenPedidoCombustible);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(ordenPedidoCombustible);
        }

        // GET: ordenPedidoCombustible/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Session["ID"] == null || !roles.tienePermiso(8, int.Parse(Session["ID"].ToString())))
            {
                return RedirectToAction("Index", "Home");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ordenPedidoCombustible ordenPedidoCombustible = db.ordenesPedidoCombustible.Find(id);

            List<detalleOrdenPedidoCombustible> detalleOrdenPedidoCombustible = new List<detalleOrdenPedidoCombustible>();

            int idOrden = Convert.ToInt32(id);
            var L2EQuery = db.detalleOrdenesPedidosCombustible.Where(s => s.ordenPedidoCombustibleID == idOrden);

            detalleOrdenPedidoCombustible = L2EQuery.ToList();

            ViewData["detalleOrdenPedidoCombustible"] = detalleOrdenPedidoCombustible;
            ViewBag.cantidadDetalle = detalleOrdenPedidoCombustible.Count;
            if (ordenPedidoCombustible == null)
            {
                return HttpNotFound();
            }
            return View(ordenPedidoCombustible);
        }

        // POST: ordenPedidoCombustible/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ordenPedidoCombustibleID,numeroOrdenPedido,destinatario,fecha,nombreQuienAutoriza,encabezado,anio")] ordenPedidoCombustible ordenPedidoCombustible, FormCollection form)
        {
            if (Session["ID"] == null || !roles.tienePermiso(8, int.Parse(Session["ID"].ToString())))
            {
                return RedirectToAction("Index", "Home");
            }

            int ordenID = ordenPedidoCombustible.ordenPedidoCombustibleID;

            var detallesOrden = db.detalleOrdenesPedidosCombustible.Where(u => u.ordenPedidoCombustibleID == ordenID);

            foreach (var detalleOr in detallesOrden)
            {
                db.detalleOrdenesPedidosCombustible.Remove(detalleOr);
            }

            string[] trabajoRealizar = Request.Form.GetValues("trabajoRealizar");
            string[] personaResponsable = Request.Form.GetValues("personaResponsable");
            string[] detalle = Request.Form.GetValues("detalle");
            string[] cantidad = Request.Form.GetValues("cantidad");

            ordenPedidoCombustible.fecha = Formateador.fechaFormatoGuardar(form["fecha"].ToString());

            for (int i = 0; i < detalle.Length; i++)
            {
                detalleOrdenPedidoCombustible detalleOrden = new detalleOrdenPedidoCombustible();
                detalleOrden.cantidad = cantidad[i];
                detalleOrden.detalle = detalle[i];
                detalleOrden.personaResponsable = personaResponsable[i];
                detalleOrden.trabajoRealizar = trabajoRealizar[i];
                detalleOrden.ordenPedidoCombustibleID = ordenPedidoCombustible.ordenPedidoCombustibleID;
                db.detalleOrdenesPedidosCombustible.Add(detalleOrden);
            }

            if (ModelState.IsValid)
            {
                db.Entry(ordenPedidoCombustible).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(ordenPedidoCombustible);
        }

        // GET: ordenPedidoCombustible/Delete/5
        public ActionResult Delete(int? id)
        {
            if (Session["ID"] == null || !roles.tienePermiso(8, int.Parse(Session["ID"].ToString())))
            {
                return RedirectToAction("Index", "Home");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ordenPedidoCombustible ordenPedidoCombustible = db.ordenesPedidoCombustible.Find(id);
            if (ordenPedidoCombustible == null)
            {
                return HttpNotFound();
            }
            return View(ordenPedidoCombustible);
        }

        // POST: ordenPedidoCombustible/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["ID"] == null || !roles.tienePermiso(8, int.Parse(Session["ID"].ToString())))
            {
                return RedirectToAction("Index", "Home");
            }
            ordenPedidoCombustible ordenPedidoCombustible = db.ordenesPedidoCombustible.Find(id);
            
            var detallesOrden = db.detalleOrdenesPedidosCombustible.Where(u => u.ordenPedidoCombustibleID == id);

            foreach (var detalleOr in detallesOrden)
            {
                db.detalleOrdenesPedidosCombustible.Remove(detalleOr);
            }

            db.ordenesPedidoCombustible.Remove(ordenPedidoCombustible);
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
    }
}
