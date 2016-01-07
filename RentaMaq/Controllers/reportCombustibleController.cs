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
    public class reportCombustibleController : Controller
    {
        private Context db = new Context();
        private int numeroPermiso = 2;

        public string verificarNumero(string numero) 
        {
            int numeroReport = int.Parse(numero);
            if (db.ReportsCombustible.Where(s=>s.numeroReport==numeroReport).ToList().Count>0)
            {
                return "true";
            }

            return "false";
        }

        // GET: reportCombustible
        public ActionResult Index(string inicio, string termino, string equipoID)
        {
            DateTime Inicio = DateTime.Now.AddMonths(-1);
            DateTime Termino = DateTime.Now;

            String EquipoID = "todos";

            if (inicio != null || termino != null)
            {
                string[] inicioSeparado = inicio.Split('-');
                string[] terminoSeparado = termino.Split('-');

                Inicio = new DateTime(int.Parse(inicioSeparado[2]), int.Parse(inicioSeparado[1]), int.Parse(inicioSeparado[0]));
                Termino = new DateTime(int.Parse(terminoSeparado[2]), int.Parse(terminoSeparado[1]), int.Parse(terminoSeparado[0]));
            }

            if (equipoID != null && !equipoID.Equals("todos")) 
            {
                EquipoID = equipoID;
            }

            List<reportCombustible> lista = reportCombustible.Todos(Inicio, Termino, EquipoID);

            ViewBag.Inicio = Inicio;
            ViewBag.Termino = Termino;
            ViewBag.EquipoID = EquipoID;

            return View(lista);
        }

        // GET: reportCombustible/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            reportCombustible reportCombustible = reportCombustible.Obtener(id);
            if (reportCombustible == null)
            {
                return HttpNotFound();
            }
            return View(reportCombustible);
        }

        // GET: reportCombustible/Create
        public ActionResult Create()
        {
            if (Session["ID"] == null || !roles.tienePermiso(numeroPermiso, int.Parse(Session["ID"].ToString())))
            {
                return RedirectToAction("Index", "reportCombustible");
            }
            ViewBag.Equipos = equipos.todos();
            return View();
        }

        // POST: reportCombustible/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,numeroReport,usuario,denominacionEquipo,horometro,kilometraje,fechaHora,litros,ubicacion,operador,quienCarga,responsable,comentario")] reportCombustible reportCombustible, FormCollection post)
        {
            if (Session["ID"] == null || !roles.tienePermiso(numeroPermiso, int.Parse(Session["ID"].ToString())))
            {

                return RedirectToAction("Index", "reportCombustible");
            }

            reportCombustible.equiposID = equipos.Obtener(int.Parse(post["equipoID"].ToString()));

            reportCombustible.guardar();


            registrokmhm nuevo = new registrokmhm();
            nuevo.equipoID = reportCombustible.equiposID.ID;
            nuevo.fecha = new DateTime(reportCombustible.fechaHora.Year, reportCombustible.fechaHora.Month, reportCombustible.fechaHora.Day);
            nuevo.horometro = reportCombustible.horometro;
            nuevo.kilometraje = reportCombustible.kilometraje;

            //db.registrokmhms.Add(nuevo);
            registrokmhm.actualizarRegistroKmHm(nuevo.equipoID,nuevo.fecha, nuevo.horometro, nuevo.kilometraje);

            db.SaveChanges();

            return RedirectToAction("Index");
        }

        // GET: reportCombustible/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Session["ID"] == null || !roles.tienePermiso(numeroPermiso, int.Parse(Session["ID"].ToString())))
            {

                return RedirectToAction("Index", "reportCombustible");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            reportCombustible reportCombustible = reportCombustible.Obtener(id);
            if (reportCombustible == null)
            {
                return HttpNotFound();
            }
            ViewBag.Equipos = equipos.todos();
            return View(reportCombustible);
        }

        // POST: reportCombustible/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,numeroReport,usuario,denominacionEquipo,horometro,kilometraje,fechaHora,litros,ubicacion,operador,quienCarga,responsable,comentario")] reportCombustible reportCombustible, FormCollection post)
        {
            if (Session["ID"] == null || !roles.tienePermiso(numeroPermiso, int.Parse(Session["ID"].ToString())))
            {

                return RedirectToAction("Index", "reportCombustible");
            }
            reportCombustible.equiposID = equipos.Obtener(int.Parse(post["equipoID"].ToString()));
            reportCombustible.guardar();


            registrokmhm nuevo = new registrokmhm();
            nuevo.equipoID = reportCombustible.equiposID.ID;
            nuevo.fecha = new DateTime(reportCombustible.fechaHora.Year, reportCombustible.fechaHora.Month, reportCombustible.fechaHora.Day);
            nuevo.horometro = reportCombustible.horometro;
            nuevo.kilometraje = reportCombustible.kilometraje;

            //db.registrokmhms.Add(nuevo);
            registrokmhm.actualizarRegistroKmHm(nuevo.equipoID, nuevo.fecha, nuevo.horometro, nuevo.kilometraje);

            db.SaveChanges();

            return RedirectToAction("Index");
        }

        // GET: reportCombustible/Delete/5
        public ActionResult Delete(int? id)
        {
            if (Session["ID"] == null || !roles.tienePermiso(numeroPermiso, int.Parse(Session["ID"].ToString())))
            {

                return RedirectToAction("Index", "reportCombustible");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            reportCombustible reportCombustible = db.ReportsCombustible.Find(id);
            if (reportCombustible == null)
            {
                return HttpNotFound();
            }
            return View(reportCombustible);
        }

        // POST: reportCombustible/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["ID"] == null || !roles.tienePermiso(numeroPermiso, int.Parse(Session["ID"].ToString())))
            {

                return RedirectToAction("Index", "reportCombustible");
            }
            reportCombustible reportCombustible = db.ReportsCombustible.Find(id);
            db.ReportsCombustible.Remove(reportCombustible);
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

        //Se debe enviar el ultimo horometro + ";" + el ultimo kilometraje
        public string obtenerUltimosDatos(string idEquipo) 
        {
            string retorno = "0;0";

            int horometroAnterior = equipos.obtenerUltimoHorometro(int.Parse(idEquipo));
            int kilometrajeAnterior = equipos.obtenerUltimoKilometraje(int.Parse(idEquipo));

            retorno = horometroAnterior + ";" + kilometrajeAnterior;

            return retorno;
        }
    }
}
