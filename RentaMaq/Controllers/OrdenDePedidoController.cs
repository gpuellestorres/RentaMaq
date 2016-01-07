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
    public class OrdenDePedidoController : Controller
    {
        private Context db = new Context();

        // GET: OrdenDePedido
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

            List<OrdenDePedido> lista = db.OrdenesPedido.Where(s => s.fecha >= Inicio && s.fecha <= Termino).ToList();

            return View(lista);
        }

        // GET: OrdenDePedido/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrdenDePedido ordenDePedido = db.OrdenesPedido.Find(id);
            if (ordenDePedido == null)
            {
                return HttpNotFound();
            }
            return View(ordenDePedido);
        }

        // GET: OrdenDePedido/Create
        public ActionResult Create()
        {
            if (Session["ID"] == null || !roles.tienePermiso(8, int.Parse(Session["ID"].ToString())))
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        // POST: OrdenDePedido/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "OrdenDePedidoID,numeroOrden,año,señores,fecha,encabezado,trabajoARealizar,personaResponsable,nombreSolicitante,fechaSolicitud")] OrdenDePedido ordenDePedido,
            FormCollection post)
        {
            if (Session["ID"] == null || !roles.tienePermiso(8, int.Parse(Session["ID"].ToString())))
            {
                return RedirectToAction("Index", "Home");
            }
            ordenDePedido.numeroOrden = ordenDePedido.obtenerNuevoNumero();
            ordenDePedido.año = DateTime.Now.Year;

            string Fecha = post["fecha"].ToString();

            ordenDePedido.fecha = new DateTime(int.Parse(Fecha.Split('/')[2]),
                int.Parse(Fecha.Split('/')[1]), int.Parse(Fecha.Split('/')[0]));

            Fecha = post["fechaSolicitud"].ToString();

            ordenDePedido.fechaSolicitud = new DateTime(int.Parse(Fecha.Split('/')[2]),
                int.Parse(Fecha.Split('/')[1]), int.Parse(Fecha.Split('/')[0]));

            ordenDePedido.quitarNulos();

            db.OrdenesPedido.Add(ordenDePedido);

            db.SaveChanges();

            //Se guarda el detalle:
            string[] detalleDetalle = Request.Form.GetValues("detalleDetalle");
            string[] cantidadDetalle = Request.Form.GetValues("cantidadDetalle");
            
            for (int i = 0; i < detalleDetalle.Length; i++)
            {
                detalleOrdenPedido nuevo = new detalleOrdenPedido();

                nuevo.detalle = detalleDetalle[i];
                nuevo.cantidad = int.Parse(cantidadDetalle[i]);
                nuevo.OrdenDePedidoID = ordenDePedido.OrdenDePedidoID;

                db.DetalleOrdenesPedido.Add(nuevo);
            }

            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: OrdenDePedido/Edit/5
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
            OrdenDePedido ordenDePedido = db.OrdenesPedido.Find(id);
            if (ordenDePedido == null)
            {
                return HttpNotFound();
            }
            return View(ordenDePedido);
        }

        // POST: OrdenDePedido/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OrdenDePedidoID,numeroOrden,año,señores,fecha,encabezado,trabajoARealizar,personaResponsable,nombreSolicitante,fechaSolicitud,firmaAutorizada")] OrdenDePedido ordenDePedido,
            FormCollection post)
        {
            if (Session["ID"] == null || !roles.tienePermiso(8, int.Parse(Session["ID"].ToString())))
            {
                return RedirectToAction("Index", "Home");
            }
            string Fecha = post["fecha"].ToString();

            ordenDePedido.fecha = new DateTime(int.Parse(Fecha.Split('/')[2]),
                int.Parse(Fecha.Split('/')[1]), int.Parse(Fecha.Split('/')[0]));

            Fecha = post["fechaSolicitud"].ToString();

            ordenDePedido.fechaSolicitud = new DateTime(int.Parse(Fecha.Split('/')[2]),
                int.Parse(Fecha.Split('/')[1]), int.Parse(Fecha.Split('/')[0]));

            ordenDePedido.eliminarDetalle();

            //Se guarda el detalle:
            string[] detalleDetalle = Request.Form.GetValues("detalleDetalle");
            string[] cantidadDetalle = Request.Form.GetValues("cantidadDetalle");

            for (int i = 0; i < detalleDetalle.Length; i++)
            {
                detalleOrdenPedido nuevo = new detalleOrdenPedido();

                nuevo.detalle = detalleDetalle[i];
                nuevo.cantidad = int.Parse(cantidadDetalle[i]);
                nuevo.OrdenDePedidoID = ordenDePedido.OrdenDePedidoID;

                db.DetalleOrdenesPedido.Add(nuevo);
            }

            db.Entry(ordenDePedido).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: OrdenDePedido/Delete/5
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
            OrdenDePedido ordenDePedido = db.OrdenesPedido.Find(id);
            if (ordenDePedido == null)
            {
                return HttpNotFound();
            }
            return View(ordenDePedido);
        }

        // POST: OrdenDePedido/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["ID"] == null || !roles.tienePermiso(8, int.Parse(Session["ID"].ToString())))
            {
                return RedirectToAction("Index", "Home");
            }
            OrdenDePedido ordenDePedido = db.OrdenesPedido.Find(id);
            ordenDePedido.eliminarDetalle();
            db.OrdenesPedido.Remove(ordenDePedido);
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
