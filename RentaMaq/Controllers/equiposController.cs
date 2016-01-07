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
    public class equiposController : Controller
    {
        private int numeroPermiso = 11;
        private Context db = new Context();

        // GET: equipos
        public ActionResult Index()
        {
            return View(equipos.todos());
        }

        public ActionResult programaMantencion()
        {
            return View(equipos.todos());
        }

        public ActionResult equiposCertificacionVencida()
        {
            ViewData["detalleEquipo"] = detalleEquipo.detalleEquiposCertificacionVencida();
            ViewBag.titulo = "Certificaciones Vencidas";
            ViewBag.variable = "Fecha Termino Certificacion";
            return View("equiposVencidas",detalleEquipo.equiposCertificacionVencida());
        }

        public ActionResult equiposRevisionTecnicaVencida()
        {
            ViewData["detalleEquipo"] = detalleEquipo.detalleEquiposRevisionTecnicaVencida();
            ViewBag.titulo = "Revision Tecnica Vencidas";
            ViewBag.variable = "Revision Tecnica";
            return View("equiposVencidas", detalleEquipo.equiposRevisionTecnicaVencida());
        }

        public ActionResult equiposPermisoCirculacionVencida()
        {
            ViewData["detalleEquipo"] = detalleEquipo.detalleEquiposPermisoCirculacionVencida();
            ViewBag.titulo = "Permiso Circulacion Vencido";
            ViewBag.variable = "Permiso Circulacion";
            return View("equiposVencidas", detalleEquipo.equiposPermisoCirculacionVencida());
        }

        public ActionResult equiposSeguroVencida()
        {
            ViewData["detalleEquipo"] = detalleEquipo.detalleEquiposSeguroVencido();
            ViewBag.titulo = "Seguro Vencido";
            ViewBag.variable = "Seguro";
            return View("equiposVencidas", detalleEquipo.equiposSeguroVencido());
        }

        // GET: equipos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            equipos equipos = db.Equipos.Find(id);
            if (equipos == null)
            {
                return HttpNotFound();
            }

            int idEquipo = Convert.ToInt32(id);
            detalleEquipo detalleEquipo = db.detalleEquipos.SingleOrDefault(s => s.EquipoID == idEquipo);

            if (!Object.ReferenceEquals(detalleEquipo, null))
            {
                ViewBag.verificadorDetalle = "si";
            }
            else
            {
                ViewBag.verificadorDetalle = "no";
            }
            ViewData["detalleEquipo"] = detalleEquipo;
            return View(equipos);
        }

        // GET: equipos/Create
        public ActionResult Create()
        {
            if (Session["ID"] == null || !roles.tienePermiso(numeroPermiso, int.Parse(Session["ID"].ToString())))
            {

                return RedirectToAction("Index", "Home");
            }
            ViewBag.Modelos = Modelo.Todos();

            return View();
        }

        // POST: equipos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,tipoEquipo,patenteEquipo,año,numeroAFI, areaTrabajo, descripcion")] equipos equipos, FormCollection post)
        {
            if (Session["ID"] == null || !roles.tienePermiso(numeroPermiso, int.Parse(Session["ID"].ToString())))
            {

                return RedirectToAction("Index", "Home");
            }
            equipos.ModeloID = db.Modeloes.Find(post["ModeloID"].ToString());

            db.Equipos.Add(equipos);
            db.SaveChanges();

            detalleEquipo detalleEquipo = new detalleEquipo();
            detalleEquipo.EquipoID = equipos.ID;
            detalleEquipo.inicioCertificacion = Formateador.fechaFormatoGuardar(post["inicioCertificacion"].ToString());
            detalleEquipo.terminoCertificacion = Formateador.fechaFormatoGuardar(post["terminoCertificacion"].ToString());
            detalleEquipo.revisionTecnica = Formateador.fechaFormatoGuardar(post["revisionTecnica"].ToString());
            detalleEquipo.permisoCirculacion = Formateador.fechaFormatoGuardar(post["permisoCirculacion"].ToString());
            detalleEquipo.seguro = Formateador.fechaFormatoGuardar(post["seguro"].ToString());
            detalleEquipo.proveedor = post["proveedor"].ToString();

            db.detalleEquipos.Add(detalleEquipo);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        // GET: equipos/Edit/5
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
            equipos equipos = equipos.Obtener(id);
            
            if (equipos == null)
            {
                return HttpNotFound();
            }

            int idEquipo = Convert.ToInt32(id);
            detalleEquipo detalleEquipo = db.detalleEquipos.SingleOrDefault(s => s.EquipoID == idEquipo);

            if (!Object.ReferenceEquals(detalleEquipo, null))
            {
                ViewBag.verificadorDetalle = "si";
            }
            else {
                ViewBag.verificadorDetalle = "no";
            }

            ViewBag.Modelos = Modelo.Todos();
            ViewData["detalleEquipo"] = detalleEquipo;
            return View(equipos);
        }

        // POST: equipos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,tipoEquipo,patenteEquipo,año,numeroAFI, areaTrabajo, descripcion")] equipos equipos, FormCollection post)
        {
            if (Session["ID"] == null || !roles.tienePermiso(numeroPermiso, int.Parse(Session["ID"].ToString())))
            {

                return RedirectToAction("Index", "Home");
            }
            equipos.ModeloID = db.Modeloes.Find(post["ModeloID"]);

            detalleEquipo detalleEquipo = new detalleEquipo();
            if (!post["detalleEquipoID"].ToString().Equals("0")) {
                detalleEquipo.detalleEquipoID = Convert.ToInt32(post["detalleEquipoID"].ToString());
            }
            detalleEquipo.EquipoID = equipos.ID;
            detalleEquipo.inicioCertificacion = Formateador.fechaFormatoGuardar(post["inicioCertificacion"].ToString());
            detalleEquipo.terminoCertificacion = Formateador.fechaFormatoGuardar(post["terminoCertificacion"].ToString());
            detalleEquipo.revisionTecnica = Formateador.fechaFormatoGuardar(post["revisionTecnica"].ToString());
            detalleEquipo.permisoCirculacion = Formateador.fechaFormatoGuardar(post["permisoCirculacion"].ToString());
            detalleEquipo.seguro = Formateador.fechaFormatoGuardar(post["seguro"].ToString());
            detalleEquipo.proveedor = post["proveedor"].ToString();

            if (detalleEquipo.detalleEquipoID == 0)
            {
                db.detalleEquipos.Add(detalleEquipo);
            }
            else {
                db.Entry(detalleEquipo).State = EntityState.Modified;
            }
                
            db.SaveChanges();
            


            equipos.guardar();


            return RedirectToAction("Index");
        }

        // GET: equipos/Delete/5
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
            equipos equipos = db.Equipos.Find(id);
            if (equipos == null)
            {
                return HttpNotFound();
            }
            return View(equipos);
        }

        // POST: equipos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["ID"] == null || !roles.tienePermiso(numeroPermiso, int.Parse(Session["ID"].ToString())))
            {
                return RedirectToAction("Index", "Home");
            }
            equipos equipos = db.Equipos.Find(id);
            db.Equipos.Remove(equipos);
            db.SaveChanges();

            equipos.eliminarInformacionEquipo(id);

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
