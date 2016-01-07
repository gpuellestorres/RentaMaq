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
using System.Collections;

namespace RentaMaq.Controllers
{
    public class hojaRutaMantenedoresController : Controller
    {
        private Context db = new Context();
        int numeroPermiso = 14;

        // GET: hojaRutaMantenedores
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

            List<hojaRutaMantenedores> listaTodos = db.hojaRutaMantenedores.Where(s => s.fecha >= Inicio && s.fecha <= fin).ToList();

            Hashtable lista = new Hashtable();

            foreach (hojaRutaMantenedores hojaR in listaTodos) 
            {
                if (!lista.ContainsKey(hojaR.numero)) 
                {
                    lista.Add(hojaR.numero, hojaR);
                }
            }
            mantencionPreventiva nueva = new mantencionPreventiva();

            return View(lista.Values.OfType<hojaRutaMantenedores>().ToList());
        }

        // GET: hojaRutaMantenedores/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            List<hojaRutaMantenedores> hojaRutaMantenedores = db.hojaRutaMantenedores.Where(s => s.numero == id).ToList();
            if (hojaRutaMantenedores == null)
            {
                return HttpNotFound();
            }
            return View(hojaRutaMantenedores);
        }

        // GET: hojaRutaMantenedores/Create
        public ActionResult Create()
        {
            if (Session["ID"] == null || !roles.tienePermiso(numeroPermiso, int.Parse(Session["ID"].ToString())))
            {

                return RedirectToAction("Index", "Home");
            }

            return View(equipos.obtenerIDsLubricacionesPendientes());
        }

        // POST: hojaRutaMantenedores/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "hojaRutaMantenedoresID,fecha,nombreMantenedor")] hojaRutaMantenedores HojaRutaMantenedores,
            FormCollection post)
        {
            if (Session["ID"] == null || !roles.tienePermiso(numeroPermiso, int.Parse(Session["ID"].ToString())))
            {

                return RedirectToAction("Index", "Home");
            }

            HojaRutaMantenedores.fecha = Formateador.fechaFormatoGuardar(post["fecha"]);
            HojaRutaMantenedores.numero = hojaRutaMantenedores.obtenerNuevoNumero();

            //Se guarda el detalle:
            string[] equipoID = Request.Form.GetValues("equipoID");
            string[] lugar = Request.Form.GetValues("lugar");
            string[] horometro = Request.Form.GetValues("horometro");
            string[] trabajoRealizado = Request.Form.GetValues("trabajoRealizado");

            for (int i = 0; i < equipoID.Length; i++)
            {
                hojaRutaMantenedores nueva = new hojaRutaMantenedores();
                nueva.fecha = HojaRutaMantenedores.fecha;
                nueva.numero = HojaRutaMantenedores.numero;
                nueva.nombreMantenedor = HojaRutaMantenedores.nombreMantenedor;
                nueva.equipoID = int.Parse(equipoID[i]);
                nueva.lugar = lugar[i];
                nueva.horometro = int.Parse(horometro[i]);
                nueva.trabajoRealizado = trabajoRealizado[i];

                db.hojaRutaMantenedores.Add(nueva);

                registrokmhm nuevoRegistro = new registrokmhm();
                nuevoRegistro.equipoID = nueva.equipoID;
                nuevoRegistro.fecha = nueva.fecha;
                nuevoRegistro.horometro = nueva.horometro;
                nuevoRegistro.kilometraje = equipos.obtenerUltimoKilometraje(nueva.equipoID);
                //db.registrokmhms.Add(nuevoRegistro);
                registrokmhm.actualizarRegistroKmHm(nuevoRegistro);
            }

            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: hojaRutaMantenedores/Edit/5
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
            List<hojaRutaMantenedores> hojaRutaMantenedores = db.hojaRutaMantenedores.Where(s => s.numero == id).ToList();
            if (hojaRutaMantenedores == null)
            {
                return HttpNotFound();
            }
            return View(hojaRutaMantenedores);
        }

        // POST: hojaRutaMantenedores/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "hojaRutaMantenedoresID,fecha,nombreMantenedor, numero")] hojaRutaMantenedores HojaRutaMantenedores,
            FormCollection post)
        {
            if (Session["ID"] == null || !roles.tienePermiso(numeroPermiso, int.Parse(Session["ID"].ToString())))
            {

                return RedirectToAction("Index", "Home");
            }

            HojaRutaMantenedores.fecha = Formateador.fechaFormatoGuardar(post["fecha"]);
            
            //Se elimina el detalle:
            hojaRutaMantenedores.eliminar(HojaRutaMantenedores.numero);

            //Se guarda el detalle:
            string[] equipoID = Request.Form.GetValues("equipoID");
            string[] lugar = Request.Form.GetValues("lugar");
            string[] horometro = Request.Form.GetValues("horometro");
            string[] trabajoRealizado = Request.Form.GetValues("trabajoRealizado");

            for (int i = 0; i < equipoID.Length; i++)
            {
                hojaRutaMantenedores nueva = new hojaRutaMantenedores();
                nueva.fecha = HojaRutaMantenedores.fecha;
                nueva.numero = HojaRutaMantenedores.numero;
                nueva.nombreMantenedor = HojaRutaMantenedores.nombreMantenedor;
                nueva.equipoID = int.Parse(equipoID[i]);
                nueva.lugar = lugar[i];
                nueva.horometro = int.Parse(horometro[i]);
                nueva.trabajoRealizado = trabajoRealizado[i];

                db.hojaRutaMantenedores.Add(nueva);

                registrokmhm nuevoRegistro = new registrokmhm();
                nuevoRegistro.equipoID = nueva.equipoID;
                nuevoRegistro.fecha = nueva.fecha;
                nuevoRegistro.horometro = nueva.horometro;
                nuevoRegistro.kilometraje = equipos.obtenerUltimoKilometraje(nueva.equipoID);
                //db.registrokmhms.Add(nuevoRegistro);
                registrokmhm.actualizarRegistroKmHm(nuevoRegistro);
            }

            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: hojaRutaMantenedores/Delete/5
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
            List<hojaRutaMantenedores> hojaRutaMantenedores = db.hojaRutaMantenedores.Where(s => s.numero == id).ToList();
            if (hojaRutaMantenedores == null)
            {
                return HttpNotFound();
            }
            return View(hojaRutaMantenedores);
        }

        // POST: hojaRutaMantenedores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["ID"] == null || !roles.tienePermiso(numeroPermiso, int.Parse(Session["ID"].ToString())))
            {

                return RedirectToAction("Index", "Home");
            }

            hojaRutaMantenedores.eliminar(id);
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
        
        public int obtenerUltimosHorometro(string idEquipo) 
        {
            return hojaRutaMantenedores.obtenerUltimoHorometro(int.Parse(idEquipo));
        }
    }
}
