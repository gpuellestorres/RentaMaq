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
    public class tipoEquipoController : Controller
    {
        private Context db = new Context();

        // GET: tipoEquipo
        public ActionResult Index()
        {
            return View(db.tipoEquipoes.ToList());
        }

        // GET: tipoEquipo/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tipoEquipo tipoEquipo = db.tipoEquipoes.Find(id);
            if (tipoEquipo == null)
            {
                return HttpNotFound();
            }
            return View(tipoEquipo);
        }

        // GET: tipoEquipo/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: tipoEquipo/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "tipoEquipoID,nombre")] tipoEquipo tipoEquipo)
        {
            if (ModelState.IsValid)
            {
                db.tipoEquipoes.Add(tipoEquipo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tipoEquipo);
        }

        // GET: tipoEquipo/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tipoEquipo tipoEquipo = db.tipoEquipoes.Find(id);
            if (tipoEquipo == null)
            {
                return HttpNotFound();
            }
            return View(tipoEquipo);
        }

        // POST: tipoEquipo/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "tipoEquipoID,nombre")] tipoEquipo tipoEquipo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tipoEquipo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tipoEquipo);
        }

        // GET: tipoEquipo/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tipoEquipo tipoEquipo = db.tipoEquipoes.Find(id);
            if (tipoEquipo == null)
            {
                return HttpNotFound();
            }
            return View(tipoEquipo);
        }

        // POST: tipoEquipo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tipoEquipo tipoEquipo = db.tipoEquipoes.Find(id);
            db.tipoEquipoes.Remove(tipoEquipo);
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
