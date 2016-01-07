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
    public class cotizacionServiciosController : Controller
    {
        private Context db = new Context();
        private int numeroPermiso = 6;

        // GET: cotizacionServicios
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
            return View(db.cotizacionesServicios.Where(s=>s.fecha>=Inicio && s.fecha<=Termino).ToList());
        }

        // GET: cotizacionServicios/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            cotizacionServicios cotizacionServicios = db.cotizacionesServicios.Find(id);
            if (cotizacionServicios == null)
            {
                return HttpNotFound();
            }
            return View(cotizacionServicios);
        }

        // GET: cotizacionServicios/Create
        public ActionResult Create()
        {
            if (Session["ID"] == null || !roles.tienePermiso(numeroPermiso, int.Parse(Session["ID"].ToString())))
            {

                return RedirectToAction("Index", "Home");
            }
            ViewBag.fechaActual = Formateador.fechaCompletaToString(DateTime.Now);
            return View();
        }

        // POST: cotizacionServicios/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "cotizacionServiciosID,numeroCotizacion,datosClienteEmpresa,datosClienteRut,datosClienteDomicilio,datosClienteSolicitadoPor,fecha,encabezado,descripcionServicio,valorTotal,nota,faena,tiempo")] cotizacionServicios cotizacionServicios, FormCollection form)
        {
            if (Session["ID"] == null || !roles.tienePermiso(numeroPermiso, int.Parse(Session["ID"].ToString())))
            {

                return RedirectToAction("Index", "Home");
            }
            cotizacionServicios.fecha = Formateador.fechaFormatoGuardar(form["fecha"].ToString());
            //cotizacionServicios.fechaEscrita = cotizacionServicios.fecha.ToString("D").Split(',')[1];
            cotizacionServicios.fechaEscrita = formatearString.fechaPalabras(cotizacionServicios.fecha);
            
            cotizacionServicios.anio = cotizacionServicios.fecha.Year;
            db.cotizacionesServicios.Add(cotizacionServicios);
            db.SaveChanges();

            //Se obtiene el detalle de categorias

            int cantidadCategorias = int.Parse(form["cantidadCategorias"].ToString());

            for (int i = 1; i <= cantidadCategorias; i++)
            {
                if (Request.Form.GetValues("categoria" + i) != null)
                {
                    string[] categoria = Request.Form.GetValues("categoria" + i);
                    string[] cargo = Request.Form.GetValues("cargoGeneral" + i);
                    string[] turno = Request.Form.GetValues("turnoGeneral" + i);
                    string[] cantidad = Request.Form.GetValues("cantidadGeneral" + i);

                    for (int j = 0; j < cargo.Length; j++)
                    {
                        detalleServicioCotizacionServicios nuevo = new detalleServicioCotizacionServicios();

                        nuevo.categoria = categoria[0];
                        nuevo.cargo = cargo[j];
                        nuevo.turno = turno[j];
                        nuevo.numeroPersonas = int.Parse(cantidad[j]);
                        nuevo.CotizacionServiciosID = cotizacionServicios.cotizacionServiciosID;

                        db.detalleServiciosCotizacionServicios.Add(nuevo);
                    }
                }
            }

            //Se obtiene el detalle de equipos

            string[] equipo = Request.Form.GetValues("equipoEquipo");
            string[] cantidadEquipo = Request.Form.GetValues("cantidadEquipo");

            for (int i = 0; i < equipo.Length; i++)
            {
                detalleEquiposCotizacionServicios nuevo = new detalleEquiposCotizacionServicios();

                nuevo.equipo=equipo[i];
                nuevo.cantidad = int.Parse(cantidadEquipo[i]);
                nuevo.CotizacionServiciosID = cotizacionServicios.cotizacionServiciosID;

                db.detalleEquiposCotizacionServicios.Add(nuevo);
            }

            db.SaveChanges();

            return RedirectToAction("Index");
        }

        // GET: cotizacionServicios/Edit/5
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
            cotizacionServicios cotizacionServicios = db.cotizacionesServicios.Find(id);
            if (cotizacionServicios == null)
            {
                return HttpNotFound();
            }
            return View(cotizacionServicios);
        }

        // POST: cotizacionServicios/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "cotizacionServiciosID,numeroCotizacion,datosClienteEmpresa,datosClienteRut,datosClienteDomicilio,datosClienteSolicitadoPor,fecha,encabezado,descripcionServicio,valorTotal,nota,faena,tiempo")] cotizacionServicios cotizacionServicios,
            FormCollection form)
        {
            if (Session["ID"] == null || !roles.tienePermiso(numeroPermiso, int.Parse(Session["ID"].ToString())))
            {

                return RedirectToAction("Index", "Home");
            }
            cotizacionServicios.fecha = Formateador.fechaFormatoGuardar(form["fecha"].ToString());
            cotizacionServicios.fechaEscrita = cotizacionServicios.fecha.ToString("D").Split(',')[1];
            cotizacionServicios.anio = cotizacionServicios.fecha.Year;
            db.cotizacionesServicios.Add(cotizacionServicios);

            db.Entry(cotizacionServicios).State = EntityState.Modified;
            db.SaveChanges();

            //Se obtiene el detalle de categorias

            cotizacionServicios.eliminarDetalleServiciosYEquipos();

            int cantidadCategorias = int.Parse(form["cantidadCategorias"].ToString());

            for (int i = 1; i <= cantidadCategorias; i++)
            {
                if (Request.Form.GetValues("categoria" + i) != null)
                {
                    string[] categoria = Request.Form.GetValues("categoria" + i);
                    string[] cargo = Request.Form.GetValues("cargoGeneral" + i);
                    string[] turno = Request.Form.GetValues("turnoGeneral" + i);
                    string[] cantidad = Request.Form.GetValues("cantidadGeneral" + i);

                    for (int j = 0; j < cargo.Length; j++)
                    {
                        detalleServicioCotizacionServicios nuevo = new detalleServicioCotizacionServicios();

                        nuevo.categoria = categoria[0];
                        nuevo.cargo = cargo[j];
                        nuevo.turno = turno[j];
                        nuevo.numeroPersonas = int.Parse(cantidad[j]);
                        nuevo.CotizacionServiciosID = cotizacionServicios.cotizacionServiciosID;

                        db.detalleServiciosCotizacionServicios.Add(nuevo);
                    }
                }
            }

            //Se obtiene el detalle de equipos

            string[] equipo = Request.Form.GetValues("equipoEquipo");
            string[] cantidadEquipo = Request.Form.GetValues("cantidadEquipo");

            for (int i = 0; i < equipo.Length; i++)
            {
                detalleEquiposCotizacionServicios nuevo = new detalleEquiposCotizacionServicios();

                nuevo.equipo = equipo[i];
                nuevo.cantidad = int.Parse(cantidadEquipo[i]);
                nuevo.CotizacionServiciosID = cotizacionServicios.cotizacionServiciosID;

                db.detalleEquiposCotizacionServicios.Add(nuevo);
            }

            db.SaveChanges();
            return RedirectToAction("Index");

        }

        // GET: cotizacionServicios/Delete/5
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
            cotizacionServicios cotizacionServicios = db.cotizacionesServicios.Find(id);
            if (cotizacionServicios == null)
            {
                return HttpNotFound();
            }
            return View(cotizacionServicios);
        }

        // POST: cotizacionServicios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["ID"] == null || !roles.tienePermiso(numeroPermiso, int.Parse(Session["ID"].ToString())))
            {

                return RedirectToAction("Index", "Home");
            }
            cotizacionServicios cotizacionServicios = db.cotizacionesServicios.Find(id);
            db.cotizacionesServicios.Remove(cotizacionServicios);
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
