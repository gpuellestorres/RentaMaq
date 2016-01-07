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
    public class cotizacionArriendoEquiposController : Controller
    {
        private Context db = new Context();
        private int numeroPermiso = 6;
        // GET: cotizacionArriendoEquipos
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

            return View(db.cotizacionArriendoEquipos.Where(s => s.fecha >= Inicio && s.fecha <= Termino).ToList());
        }

        // GET: cotizacionArriendoEquipos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            cotizacionArriendoEquipo cotizacionArriendoEquipo = db.cotizacionArriendoEquipos.Find(id);
            List<detalleCotizacionArriendoEquipo> detalleCotizacionArriendoEquipo = new List<detalleCotizacionArriendoEquipo>();

            int idCotizacion = Convert.ToInt32(id);
            var L2EQuery = db.detalleCotizacionArriendoEquipo.Where(s => s.cotizacionArriendoEquipoID == idCotizacion);

            detalleCotizacionArriendoEquipo = L2EQuery.ToList();

            ViewData["detalleCotizacionArriendoEquipo"] = detalleCotizacionArriendoEquipo;
            if (cotizacionArriendoEquipo == null)
            {
                return HttpNotFound();
            }
            return View(cotizacionArriendoEquipo);
        }

        // GET: cotizacionArriendoEquipos/Create
        public ActionResult Create()
        {
            if (Session["ID"] == null || !roles.tienePermiso(numeroPermiso, int.Parse(Session["ID"].ToString())))
            {

                return RedirectToAction("Index", "Home");
            }
            ViewBag.fechaActual = Formateador.fechaCompletaToString(DateTime.Now);
            return View();
        }

        // POST: cotizacionArriendoEquipos/Create      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "cotizacionArriendoEquipoID,numeroCotizacionArriendo,tipoCotizacion,referencia,fecha,datosClienteEmpresa,datosClienteRut,datosClienteDireccion,datosClienteFecha,datosClienteSolicitado,datosClienteEmail,encabezado,incluye,noIncluye,tiempoArriendo,faena,tipoHorasMinimas")] cotizacionArriendoEquipo cotizacionArriendoEquipo, FormCollection form)
        {
            if (Session["ID"] == null || !roles.tienePermiso(numeroPermiso, int.Parse(Session["ID"].ToString())))
            {

                return RedirectToAction("Index", "Home");
            }
            string[] detalle = Request.Form.GetValues("detalle");
            string[] horasMinimas = Request.Form.GetValues("horasMinimas");
            string[] valorHoraMaquina = Request.Form.GetValues("valorHoraMaquina");           

            cotizacionArriendoEquipo.fecha = Formateador.fechaFormatoGuardar(form["fecha"].ToString());
            cotizacionArriendoEquipo.datosClienteFecha = Formateador.fechaFormatoGuardar(form["datosClienteFecha"].ToString());

            for (int i = 0; i < detalle.Length; i++)
            {
                detalleCotizacionArriendoEquipo detalleCotizacion = new detalleCotizacionArriendoEquipo();
                detalleCotizacion.horasMinimas = Convert.ToInt32(horasMinimas[i]);
                detalleCotizacion.detalle = detalle[i];
                detalleCotizacion.valorHoraMaquina = valorHoraMaquina[i];               
                db.detalleCotizacionArriendoEquipo.Add(detalleCotizacion);
            }
          
            if (ModelState.IsValid)
            {
                db.cotizacionArriendoEquipos.Add(cotizacionArriendoEquipo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(cotizacionArriendoEquipo);
        }

        // GET: cotizacionArriendoEquipos/Edit/5
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
            cotizacionArriendoEquipo cotizacionArriendoEquipo = db.cotizacionArriendoEquipos.Find(id);


            List<detalleCotizacionArriendoEquipo> detalleCotizacionArriendoEquipo = new List<detalleCotizacionArriendoEquipo>();

            int idCotizacion = Convert.ToInt32(id);
            var L2EQuery = db.detalleCotizacionArriendoEquipo.Where(s => s.cotizacionArriendoEquipoID == idCotizacion);

            detalleCotizacionArriendoEquipo = L2EQuery.ToList();

            ViewData["detalleCotizacionArriendoEquipo"] = detalleCotizacionArriendoEquipo;
            ViewBag.cantidadDetalle = detalleCotizacionArriendoEquipo.Count;

            if (cotizacionArriendoEquipo == null)
            {
                return HttpNotFound();
            }
            return View(cotizacionArriendoEquipo);
        }

        // POST: cotizacionArriendoEquipos/Edit/5      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "cotizacionArriendoEquipoID,numeroCotizacionArriendo,tipoCotizacion,referencia,fecha,datosClienteEmpresa,datosClienteRut,datosClienteDireccion,datosClienteFecha,datosClienteSolicitado,datosClienteEmail,encabezado,incluye,noIncluye,tiempoArriendo,faena,tipoHorasMinimas")] cotizacionArriendoEquipo cotizacionArriendoEquipo, FormCollection form)
        {

            if (Session["ID"] == null || !roles.tienePermiso(numeroPermiso, int.Parse(Session["ID"].ToString())))
            {

                return RedirectToAction("Index", "Home");
            }
            int cotizacionID= cotizacionArriendoEquipo.cotizacionArriendoEquipoID;

            var detallesCotizacion = db.detalleCotizacionArriendoEquipo.Where(u => u.cotizacionArriendoEquipoID == cotizacionID);

            foreach (var detalleCot in detallesCotizacion)
            {
                db.detalleCotizacionArriendoEquipo.Remove(detalleCot);
            }

            string[] detalle = Request.Form.GetValues("detalle");
            string[] horasMinimas = Request.Form.GetValues("horasMinimas");
            string[] valorHoraMaquina = Request.Form.GetValues("valorHoraMaquina");

            cotizacionArriendoEquipo.fecha = Formateador.fechaFormatoGuardar(form["fecha"].ToString());
            cotizacionArriendoEquipo.datosClienteFecha = Formateador.fechaFormatoGuardar(form["datosClienteFecha"].ToString());

            for (int i = 0; i < detalle.Length; i++)
            {
                detalleCotizacionArriendoEquipo detalleCotizacion = new detalleCotizacionArriendoEquipo();
                detalleCotizacion.horasMinimas = Convert.ToInt32(horasMinimas[i]);
                detalleCotizacion.detalle = detalle[i];
                detalleCotizacion.valorHoraMaquina = valorHoraMaquina[i];
                detalleCotizacion.cotizacionArriendoEquipoID = cotizacionID;
                db.detalleCotizacionArriendoEquipo.Add(detalleCotizacion);
            }
          


            cotizacionArriendoEquipo.fecha = Formateador.fechaFormatoGuardar(form["fecha"].ToString());
            cotizacionArriendoEquipo.datosClienteFecha = Formateador.fechaFormatoGuardar(form["datosClienteFecha"].ToString());

            if (ModelState.IsValid)
            {
                db.Entry(cotizacionArriendoEquipo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cotizacionArriendoEquipo);
        }

        // GET: cotizacionArriendoEquipos/Delete/5
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
            cotizacionArriendoEquipo cotizacionArriendoEquipo = db.cotizacionArriendoEquipos.Find(id);
            if (cotizacionArriendoEquipo == null)
            {
                return HttpNotFound();
            }
            return View(cotizacionArriendoEquipo);
        }

        // POST: cotizacionArriendoEquipos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["ID"] == null || !roles.tienePermiso(numeroPermiso, int.Parse(Session["ID"].ToString())))
            {

                return RedirectToAction("Index", "Home");
            }
            cotizacionArriendoEquipo cotizacionArriendoEquipo = db.cotizacionArriendoEquipos.Find(id);

            var detallesCotizacion = db.detalleCotizacionArriendoEquipo.Where(u => u.cotizacionArriendoEquipoID == id);

            foreach (var detalleCot in detallesCotizacion)
            {
                db.detalleCotizacionArriendoEquipo.Remove(detalleCot);
            }
            db.cotizacionArriendoEquipos.Remove(cotizacionArriendoEquipo);
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
