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
    public class BodegasController : Controller
    {
        private Context db = new Context();

        // GET: Bodegas
        public ActionResult Index()
        {
            return View(db.bodegas.ToList());
        }

        // GET: Bodegas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bodegas bodegas = db.bodegas.Find(id);
            if (bodegas == null)
            {
                return HttpNotFound();
            }
            return View(bodegas);
        }

        // GET: Bodegas/Create
        public ActionResult Create()
        {
            if (Session["ID"] == null || !roles.tienePermiso(1, int.Parse(Session["ID"].ToString())))
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        // POST: Bodegas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "BodegasID,nombre,ubicacion")] Bodegas bodegas)
        {
            if (Session["ID"] == null || !roles.tienePermiso(1, int.Parse(Session["ID"].ToString())))
            {
                return RedirectToAction("Index", "Home");
            }
            if (ModelState.IsValid)
            {
                db.bodegas.Add(bodegas);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(bodegas);
        }

        // GET: Bodegas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Session["ID"] == null || !roles.tienePermiso(1, int.Parse(Session["ID"].ToString())))
            {
                return RedirectToAction("Index", "Home");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bodegas bodegas = db.bodegas.Find(id);
            if (bodegas == null)
            {
                return HttpNotFound();
            }
            return View(bodegas);
        }

        // POST: Bodegas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "BodegasID,nombre,ubicacion")] Bodegas bodegas)
        {
            if (Session["ID"] == null || !roles.tienePermiso(1, int.Parse(Session["ID"].ToString())))
            {
                return RedirectToAction("Index", "Home");
            }
            if (ModelState.IsValid)
            {
                db.Entry(bodegas).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(bodegas);
        }

        // GET: Bodegas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (Session["ID"] == null || !roles.tienePermiso(1, int.Parse(Session["ID"].ToString())))
            {
                return RedirectToAction("Index", "Home");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bodegas bodegas = db.bodegas.Find(id);
            if (bodegas == null)
            {
                return HttpNotFound();
            }
            return View(bodegas);
        }

        // POST: Bodegas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["ID"] == null || !roles.tienePermiso(1, int.Parse(Session["ID"].ToString())))
            {
                return RedirectToAction("Index", "Home");
            }
            Bodegas bodegas = db.bodegas.Find(id);
            db.bodegas.Remove(bodegas);
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
