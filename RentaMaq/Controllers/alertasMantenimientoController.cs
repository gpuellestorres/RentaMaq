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
    public class alertasMantenimientoController : Controller
    {
        private Context db = new Context();
        int numeroPermiso = 9;

        // GET: alertasMantenimiento
        public ActionResult Index()
        {
            return View(db.alertasMantenimientoes.ToList());
        }

        // GET: alertasMantenimiento/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            alertasMantenimiento alertasMantenimiento = db.alertasMantenimientoes.Find(id);
            if (alertasMantenimiento == null)
            {
                return HttpNotFound();
            }
            return View(alertasMantenimiento);
        }

        // GET: alertasMantenimiento/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: alertasMantenimiento/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "alertasMantenimientoID,nombre,correo")] alertasMantenimiento alertasMantenimiento)
        {
            if (ModelState.IsValid)
            {
                db.alertasMantenimientoes.Add(alertasMantenimiento);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(alertasMantenimiento);
        }

        // GET: alertasMantenimiento/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            alertasMantenimiento alertasMantenimiento = db.alertasMantenimientoes.Find(id);
            if (alertasMantenimiento == null)
            {
                return HttpNotFound();
            }
            return View(alertasMantenimiento);
        }

        // POST: alertasMantenimiento/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "alertasMantenimientoID,nombre,correo")] alertasMantenimiento alertasMantenimiento)
        {
            if (ModelState.IsValid)
            {
                db.Entry(alertasMantenimiento).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(alertasMantenimiento);
        }

        // GET: alertasMantenimiento/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            alertasMantenimiento alertasMantenimiento = db.alertasMantenimientoes.Find(id);
            if (alertasMantenimiento == null)
            {
                return HttpNotFound();
            }
            return View(alertasMantenimiento);
        }

        // POST: alertasMantenimiento/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            alertasMantenimiento alertasMantenimiento = db.alertasMantenimientoes.Find(id);
            db.alertasMantenimientoes.Remove(alertasMantenimiento);
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
