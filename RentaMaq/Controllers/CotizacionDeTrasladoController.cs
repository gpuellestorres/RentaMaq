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
    public class CotizacionDeTrasladoController : Controller
    {
        private Context db = new Context();
        private int numeroPermiso = 6;

        // GET: CotizacionDeTraslado
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
            return View(db.CotizacionDeTraslado.Where(s=>s.fecha>=Inicio && s.fecha<=Termino).ToList());
        }

        // GET: CotizacionDeTraslado/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CotizacionDeTraslado cotizacionDeTraslado = db.CotizacionDeTraslado.Find(id);
            if (cotizacionDeTraslado == null)
            {
                return HttpNotFound();
            }
            return View(cotizacionDeTraslado);
        }

        // GET: CotizacionDeTraslado/Create
        public ActionResult Create()
        {
            if (Session["ID"] == null || !roles.tienePermiso(numeroPermiso, int.Parse(Session["ID"].ToString())))
            {

                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        // POST: CotizacionDeTraslado/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CotizacionDeTrasladoID,NumeroDeCotizacion,año,fecha,cliente,rut,direccion,telefono,atencionA,referencia,moneda,tipoCambio,encabezado,nota,disponibilidad,valorIncluye,valorNoIncluye,condicionesGenerales,formasDePago,textoAdjuntarDocumentos")] CotizacionDeTraslado cotizacionDeTraslado,
            FormCollection post)
        {
            if (Session["ID"] == null || !roles.tienePermiso(numeroPermiso, int.Parse(Session["ID"].ToString())))
            {

                return RedirectToAction("Index", "Home");
            }
            string Fecha = post["fecha"].ToString();

            cotizacionDeTraslado.fecha = new DateTime(int.Parse(Fecha.Split('/')[2]),
                int.Parse(Fecha.Split('/')[1]), int.Parse(Fecha.Split('/')[0]));

            cotizacionDeTraslado.año = DateTime.Now.Year;

            List<CotizacionDeTraslado> listaCT =
            db.CotizacionDeTraslado.OrderByDescending(s => s.NumeroDeCotizacion).Take(1).ToList();
            if (listaCT.Count > 1)
            {
                cotizacionDeTraslado.NumeroDeCotizacion = listaCT[0].NumeroDeCotizacion + 1;
            }
            else
            {
                cotizacionDeTraslado.NumeroDeCotizacion = 1;
            }

            cotizacionDeTraslado.quitarNulos();

            db.CotizacionDeTraslado.Add(cotizacionDeTraslado);
            db.SaveChanges();

            //Se guarda el detalle:
            string[] codigoDetalle = Request.Form.GetValues("codigoDetalle");
            string[] cantidadDetalle = Request.Form.GetValues("cantidadDetalle");
            string[] descripcionDetalle = Request.Form.GetValues("descripcionDetalle");
            string[] precioUnitarioDetalle = Request.Form.GetValues("precioUnitarioDetalle");
            string[] totalDetalle = Request.Form.GetValues("totalDetalle");

            for (int i = 0; i < codigoDetalle.Length; i++)
            {
                detalleCotizacionDeTraslado nuevo = new detalleCotizacionDeTraslado();

                nuevo.codigo = codigoDetalle[i];
                nuevo.descripcion = descripcionDetalle[i];
                nuevo.cantidad = int.Parse(cantidadDetalle[i]);
                nuevo.precioUnitario = int.Parse(precioUnitarioDetalle[i]);
                nuevo.total = int.Parse(totalDetalle[i]);
                nuevo.IDCotizacionTraslado = cotizacionDeTraslado.CotizacionDeTrasladoID;

                db.detalleCotizacionTraslado.Add(nuevo);
            }

            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: CotizacionDeTraslado/Edit/5
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
            CotizacionDeTraslado cotizacionDeTraslado = db.CotizacionDeTraslado.Find(id);
            if (cotizacionDeTraslado == null)
            {
                return HttpNotFound();
            }
            return View(cotizacionDeTraslado);
        }

        // POST: CotizacionDeTraslado/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CotizacionDeTrasladoID,NumeroDeCotizacion,año,fecha,cliente,rut,direccion,telefono,atencionA,referencia,moneda,tipoCambio,encabezado,nota,disponibilidad,valorIncluye,valorNoIncluye,condicionesGenerales,formasDePago,textoAdjuntarDocumentos")] CotizacionDeTraslado cotizacionDeTraslado,
            FormCollection post)
        {

            if (Session["ID"] == null || !roles.tienePermiso(numeroPermiso, int.Parse(Session["ID"].ToString())))
            {

                return RedirectToAction("Index", "Home");
            }

            string Fecha = post["fecha"].ToString();

            cotizacionDeTraslado.fecha = new DateTime(int.Parse(Fecha.Split('/')[2]),
                int.Parse(Fecha.Split('/')[1]), int.Parse(Fecha.Split('/')[0]));

            cotizacionDeTraslado.quitarNulos();

            cotizacionDeTraslado.eliminarDetalle();

            //Se guarda el detalle:
            string[] codigoDetalle = Request.Form.GetValues("codigoDetalle");
            string[] cantidadDetalle = Request.Form.GetValues("cantidadDetalle");
            string[] descripcionDetalle = Request.Form.GetValues("descripcionDetalle");
            string[] precioUnitarioDetalle = Request.Form.GetValues("precioUnitarioDetalle");
            string[] totalDetalle = Request.Form.GetValues("totalDetalle");

            for (int i = 0; i < codigoDetalle.Length; i++)
            {
                detalleCotizacionDeTraslado nuevo = new detalleCotizacionDeTraslado();

                nuevo.codigo = codigoDetalle[i];
                nuevo.descripcion = descripcionDetalle[i];
                nuevo.cantidad = int.Parse(cantidadDetalle[i]);
                nuevo.precioUnitario = int.Parse(precioUnitarioDetalle[i]);
                nuevo.total = int.Parse(totalDetalle[i]);
                nuevo.IDCotizacionTraslado = cotizacionDeTraslado.CotizacionDeTrasladoID;

                db.detalleCotizacionTraslado.Add(nuevo);
            }

            db.Entry(cotizacionDeTraslado).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");

        }

        // GET: CotizacionDeTraslado/Delete/5
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
            CotizacionDeTraslado cotizacionDeTraslado = db.CotizacionDeTraslado.Find(id);
            if (cotizacionDeTraslado == null)
            {
                return HttpNotFound();
            }
            return View(cotizacionDeTraslado);
        }

        // POST: CotizacionDeTraslado/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["ID"] == null || !roles.tienePermiso(numeroPermiso, int.Parse(Session["ID"].ToString())))
            {

                return RedirectToAction("Index", "Home");
            }

            CotizacionDeTraslado cotizacionDeTraslado = db.CotizacionDeTraslado.Find(id);

            cotizacionDeTraslado.eliminarDetalle();

            db.CotizacionDeTraslado.Remove(cotizacionDeTraslado);
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
