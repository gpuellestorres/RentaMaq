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
    public class chequeoPreventivoController : Controller
    {
        private Context db = new Context();
        int numeroPermiso = 15;

        // GET: chequeoPreventivo
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
            DateTime temp=fin;
            while (temp.Day == Termino.Day) 
            {
                fin =temp;
                temp = temp.AddMinutes(1);
            }
            List<chequeoPreventivo> lista = db.chequeoPreventivos.Where(s=>s.fecha>=Inicio && s.fecha<=fin).ToList();

            ViewBag.Inicio = Inicio;
            ViewBag.Termino = Termino;
            return View(lista);
        }

        // GET: chequeoPreventivo/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            chequeoPreventivo chequeoPreventivo = db.chequeoPreventivos.Find(id);
            if (chequeoPreventivo == null)
            {
                return HttpNotFound();
            }
            return View(chequeoPreventivo);
        }

        // GET: chequeoPreventivo/Create
        public ActionResult Create()
        {
            if (Session["ID"] == null || !roles.tienePermiso(numeroPermiso, int.Parse(Session["ID"].ToString())))
            {

                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        // POST: chequeoPreventivo/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "chequeoPreventivoID,codigoEquipo,numeroChequeoPreventivo,fecha,hora,observacionesGenerales,nivelCombustible,nombreResponsableEntrega,nombreResponsableRecepcion,refrigerante,aceiteMotor,sistemaRefrigeracion,sistemaHidraulico,codigosTestigos,frenoServicio,frenoEstacionamiento,frenoEmergencia,direccion,correaVentiladores,lubricacionGeneral,fugasAguaAceite,elementoDesgaste,cadenaZapatillaRodillo,baldePala,pasadores,neumaticos,pernos,manguerasOrrings,cilindrosHidraulicos,baterias,instalacionElectrica,alzavidrios,asientos,cinturonSeguridad,aireAcondicionado,limpiaParabrisas,vidrios,balizas,cintasReflextantes,cunas,cortaCorriente,bocina,lucesPrincipales,intermitentes,alarmaDeRetroceso,pertiga,extintor,botiquin,neumaticosDeRespuesto")] chequeoPreventivo chequeoPreventivo, FormCollection form)
        {
            if (Session["ID"] == null || !roles.tienePermiso(numeroPermiso, int.Parse(Session["ID"].ToString())))
            {

                return RedirectToAction("Index", "Home");
            }
            chequeoPreventivo.fecha = Formateador.fechaFormatoGuardar((string)form["fecha"]);
            //if (ModelState.IsValid)
            //{   
                db.chequeoPreventivos.Add(chequeoPreventivo);
                db.SaveChanges();
                return RedirectToAction("Index");
            //}

            //return View(chequeoPreventivo);
        }

        // GET: chequeoPreventivo/Edit/5
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
            chequeoPreventivo chequeoPreventivo = db.chequeoPreventivos.Find(id);
            if (chequeoPreventivo == null)
            {
                return HttpNotFound();
            }
            return View(chequeoPreventivo);
        }

        // POST: chequeoPreventivo/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "chequeoPreventivoID,codigoEquipo,numeroChequeoPreventivo,fecha,hora,observacionesGenerales,nivelCombustible,nombreResponsableEntrega,nombreResponsableRecepcion,refrigerante,aceiteMotor,sistemaRefrigeracion,sistemaHidraulico,codigosTestigos,frenoServicio,frenoEstacionamiento,frenoEmergencia,direccion,correaVentiladores,lubricacionGeneral,fugasAguaAceite,elementoDesgaste,cadenaZapatillaRodillo,baldePala,pasadores,neumaticos,pernos,manguerasOrrings,cilindrosHidraulicos,baterias,instalacionElectrica,alzavidrios,asientos,cinturonSeguridad,aireAcondicionado,limpiaParabrisas,vidrios,balizas,cintasReflextantes,cunas,cortaCorriente,bocina,lucesPrincipales,intermitentes,alarmaDeRetroceso,pertiga,extintor,botiquin,neumaticosDeRespuesto")] chequeoPreventivo chequeoPreventivo, FormCollection form)
        {
            if (Session["ID"] == null || !roles.tienePermiso(numeroPermiso, int.Parse(Session["ID"].ToString())))
            {

                return RedirectToAction("Index", "Home");
            }
            //if (ModelState.IsValid)
            //{
            chequeoPreventivo.fecha = Formateador.fechaFormatoGuardar((string)form["fecha"]);
                db.Entry(chequeoPreventivo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            //}
            //return View(chequeoPreventivo);
        }

        // GET: chequeoPreventivo/Delete/5
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
            chequeoPreventivo chequeoPreventivo = db.chequeoPreventivos.Find(id);
            if (chequeoPreventivo == null)
            {
                return HttpNotFound();
            }
            return View(chequeoPreventivo);
        }

        // POST: chequeoPreventivo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["ID"] == null || !roles.tienePermiso(numeroPermiso, int.Parse(Session["ID"].ToString())))
            {

                return RedirectToAction("Index", "Home");
            }
            chequeoPreventivo chequeoPreventivo = db.chequeoPreventivos.Find(id);
            db.chequeoPreventivos.Remove(chequeoPreventivo);
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
