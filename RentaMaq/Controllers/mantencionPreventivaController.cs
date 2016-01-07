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
    public class mantencionPreventivaController : Controller
    {
        private Context db = new Context();
        private int numeroPermiso = 13;

        // GET: mantencionPreventiva
        public ActionResult Index(string inicio, string termino)
        {
            DateTime Inicio = DateTime.Now.AddMonths(-1);
            DateTime Termino = DateTime.Now;
            if (inicio != null || termino != null)
            {
                string[] inicioSeparado = inicio.Split('-');
                string[] terminoSeparado = termino.Split('-');

                Inicio = new DateTime(int.Parse(inicioSeparado[2]), int.Parse(inicioSeparado[1]), int.Parse(inicioSeparado[0]));
                Termino = new DateTime(int.Parse(terminoSeparado[2]), int.Parse(terminoSeparado[1]), int.Parse(terminoSeparado[0]));
            }

            DateTime fin = Termino;
            DateTime temp = fin;
            while (temp.Day == Termino.Day)
            {
                fin = temp;
                temp = temp.AddMinutes(1);
            }

            ViewBag.Inicio = Inicio;
            ViewBag.Termino = Termino;

            return View(db.mantencionPreventivas.Where(s => s.fecha >= Inicio && s.fecha <= fin).ToList());
        }

        public ActionResult Pendientes(string inicio, string termino)
        {
            List<int> IDsPendientes = equipos.mantencionesPreventivasPendientes();

            List<mantencionPreventiva> lista = new List<mantencionPreventiva>();

            foreach (int ID in IDsPendientes)
            {
                lista.Add(mantencionPreventiva.obtenerUltima(ID));
            }

            return View(lista);
        }

        // GET: mantencionPreventiva/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            mantencionPreventiva mantencionPreventiva = db.mantencionPreventivas.Find(id);
            if (mantencionPreventiva == null)
            {
                return HttpNotFound();
            }
            return View(mantencionPreventiva);
        }

        // GET: mantencionPreventiva/Create
        public ActionResult Create()
        {
            if (Session["ID"] == null || !roles.tienePermiso(numeroPermiso, int.Parse(Session["ID"].ToString())))
            {
                return RedirectToAction("Index", "mantencionPreventiva");
            }
            return View();
        }

        // POST: mantencionPreventiva/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "mantencionPreventivaID,equipoID,fecha,horometroActual,kilometrajeActual,horometroProximaMantencion,kilometrajeProximaMantencion,nota")] mantencionPreventiva mantencionPreventiva,
            FormCollection post)
        {
            if (Session["ID"] == null || !roles.tienePermiso(numeroPermiso, int.Parse(Session["ID"].ToString())))
            {
                return RedirectToAction("Index", "mantencionPreventiva");
            }

            mantencionPreventiva.fecha = Formateador.fechaFormatoGuardar(post["fecha"]);

            db.mantencionPreventivas.Add(mantencionPreventiva);

            registrokmhm nuevo = new registrokmhm();
            nuevo.equipoID = mantencionPreventiva.equipoID;
            nuevo.fecha = mantencionPreventiva.fecha;
            nuevo.horometro = mantencionPreventiva.horometroActual;
            nuevo.kilometraje = mantencionPreventiva.kilometrajeActual;

            //db.registrokmhms.Add(nuevo);
            registrokmhm.actualizarRegistroKmHm(nuevo.equipoID, nuevo.fecha, nuevo.horometro, nuevo.kilometraje);
            
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: mantencionPreventiva/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Session["ID"] == null || !roles.tienePermiso(numeroPermiso, int.Parse(Session["ID"].ToString())))
            {
                return RedirectToAction("Index", "mantencionPreventiva");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            mantencionPreventiva mantencionPreventiva = db.mantencionPreventivas.Find(id);
            if (mantencionPreventiva == null)
            {
                return HttpNotFound();
            }
            return View(mantencionPreventiva);
        }

        // POST: mantencionPreventiva/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "mantencionPreventivaID,equipoID,fecha,horometroActual,kilometrajeActual,horometroProximaMantencion,kilometrajeProximaMantencion,nota")] mantencionPreventiva mantencionPreventiva,
            FormCollection post)
        {
            if (Session["ID"] == null || !roles.tienePermiso(numeroPermiso, int.Parse(Session["ID"].ToString())))
            {
                return RedirectToAction("Index", "mantencionPreventiva");
            }

            mantencionPreventiva.fecha = Formateador.fechaFormatoGuardar(post["fecha"]);

            db.Entry(mantencionPreventiva).State = EntityState.Modified;

            registrokmhm nuevo = new registrokmhm();
            nuevo.equipoID = mantencionPreventiva.equipoID;
            nuevo.fecha = mantencionPreventiva.fecha;
            nuevo.horometro = mantencionPreventiva.horometroActual;
            nuevo.kilometraje = mantencionPreventiva.kilometrajeActual;

            //db.registrokmhms.Add(nuevo);
            registrokmhm.actualizarRegistroKmHm(nuevo.equipoID, nuevo.fecha, nuevo.horometro, nuevo.kilometraje);

            db.SaveChanges();
            return RedirectToAction("Index");

        }

        // GET: mantencionPreventiva/Delete/5
        public ActionResult Delete(int? id)
        {
            if (Session["ID"] == null || !roles.tienePermiso(numeroPermiso, int.Parse(Session["ID"].ToString())))
            {
                return RedirectToAction("Index", "mantencionPreventiva");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            mantencionPreventiva mantencionPreventiva = db.mantencionPreventivas.Find(id);
            if (mantencionPreventiva == null)
            {
                return HttpNotFound();
            }
            return View(mantencionPreventiva);
        }

        // POST: mantencionPreventiva/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["ID"] == null || !roles.tienePermiso(numeroPermiso, int.Parse(Session["ID"].ToString())))
            {
                return RedirectToAction("Index", "mantencionPreventiva");
            }

            mantencionPreventiva mantencionPreventiva = db.mantencionPreventivas.Find(id);
            db.mantencionPreventivas.Remove(mantencionPreventiva);
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
