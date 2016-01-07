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
using sarey_erp.Models;

namespace RentaMaq.Controllers
{
    public class UsuariosController : Controller
    {
        static int flag = 0;

        private Context db = new Context();

        // GET: Usuarios
        public ActionResult Index()
        {
            if (Session["ID"]==null || !roles.tienePermiso(9, int.Parse(Session["ID"].ToString())))
            {
                return RedirectToAction("Index", "Home");
            }
            return View(db.Usuarios.ToList());
        }

        // GET: Usuarios/Details/5
        public ActionResult Details(int? id)
        {
            if (Session["ID"] == null || !roles.tienePermiso(9, int.Parse(Session["ID"].ToString())))
            {
                return RedirectToAction("Index", "Home");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Usuario usuario = db.Usuarios.Find(id);
            if (usuario == null)
            {
                return HttpNotFound();
            }
            return View(usuario);
        }

        // GET: Usuarios/Create
        public ActionResult Create()
        {
            if (Session["ID"] == null || !roles.tienePermiso(9, int.Parse(Session["ID"].ToString())))
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        // POST: Usuarios/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "usuarioID,nombreUsuario,nombreCompleto,correoElectronico")] Usuario usuario, FormCollection post)
        {
            if (Session["ID"] == null || !roles.tienePermiso(9, int.Parse(Session["ID"].ToString())))
            {
                return RedirectToAction("Index", "Home");
            }
            usuario.password = encriptacion.GetMD5(post["password"].ToString());

            if (ModelState.IsValid)
            {
                db.Usuarios.Add(usuario);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(usuario);
        }

        // GET: Usuarios/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Session["ID"] == null || !roles.tienePermiso(9, int.Parse(Session["ID"].ToString())))
            {
                return RedirectToAction("Index", "Home");
            }
            Usuario usuario = db.Usuarios.Find(id);
            if (usuario.nombreUsuario.Equals("gpuelles"))
            {
                if ((Session["ID"] == null || !(db.Usuarios.Find(int.Parse(Session["ID"].ToString())).nombreUsuario.Equals("gpuelles")))) return RedirectToAction("Index", "Home");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (usuario == null)
            {
                return HttpNotFound();
            }
            return View(usuario);
        }

        // POST: Usuarios/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "usuarioID,nombreUsuario,nombreCompleto,correoElectronico")] Usuario usuario)
        {
            if (Session["ID"] == null || !roles.tienePermiso(9, int.Parse(Session["ID"].ToString())))
            {
                return RedirectToAction("Index", "Home");
            }
            if (usuario.nombreUsuario.Equals("gpuelles"))
            {
                if ((Session["ID"] == null || !(db.Usuarios.Find(int.Parse(Session["ID"].ToString())).nombreUsuario.Equals("gpuelles")))) return RedirectToAction("Index", "Home");
            }
            usuario.password = db.Usuarios.Find(usuario.usuarioID).password;

            usuario.actualizar();

            return RedirectToAction("Index");
        }

        // GET: Usuarios/Delete/5
        public ActionResult Delete(int? id)
        {
            if (Session["ID"] == null || !roles.tienePermiso(9, int.Parse(Session["ID"].ToString())))
            {
                return RedirectToAction("Index", "Home");
            }
            Usuario usuario = db.Usuarios.Find(id);
            if (usuario.nombreUsuario.Equals("gpuelles"))
            {
                return RedirectToAction("Index", "Home");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (usuario == null)
            {
                return HttpNotFound();
            }
            return View(usuario);
        }

        // POST: Usuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["ID"] == null || !roles.tienePermiso(9, int.Parse(Session["ID"].ToString())))
            {
                return RedirectToAction("Index", "Home");
            }
            Usuario usuario = db.Usuarios.Find(id);
            if (usuario.nombreUsuario.Equals("gpuelles"))
            {
                if ((Session["ID"] == null || !(db.Usuarios.Find(int.Parse(Session["ID"].ToString())).nombreUsuario.Equals("gpuelles")))) return RedirectToAction("Index", "Home");
            }
            db.Usuarios.Remove(usuario);
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

        public string existeUsuario(string nombreUsuario)
        {
            List<Usuario> usuarios =
            db.Usuarios.Where(s => s.nombreUsuario == nombreUsuario).ToList();

            if (usuarios.Count > 0) return "true";
            return "false";
        }

        public ActionResult ModificarContraseña(int id)         
        {
            if (!roles.tienePermiso(9, int.Parse(Session["ID"].ToString())))
            {
                return RedirectToAction("Index", "Home");
            }
            Usuario usuario = db.Usuarios.Find(id);
            if (usuario.nombreUsuario.Equals("gpuelles"))
            {
                if ((Session["ID"] == null || !(db.Usuarios.Find(int.Parse(Session["ID"].ToString())).nombreUsuario.Equals("gpuelles")))) return RedirectToAction("Index", "Home");
            }
            return View(db.Usuarios.Find(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ModificarContraseña(FormCollection post)
        {
            if (Session["ID"] == null || !roles.tienePermiso(9, int.Parse(Session["ID"].ToString())))
            {
                return RedirectToAction("Index", "Home");
            }
            Usuario usuario = db.Usuarios.Find(int.Parse(post["usuarioID"].ToString()));
            if (usuario.nombreUsuario.Equals("gpuelles"))
            {
                if ((Session["ID"] == null || !(db.Usuarios.Find(int.Parse(Session["ID"].ToString())).nombreUsuario.Equals("gpuelles")))) return RedirectToAction("Index", "Home");
            }
            usuario.password = encriptacion.GetMD5(post["password"].ToString());

            usuario.actualizar();

            return RedirectToAction("Index");
        }

        public ActionResult gestionarPermisos(int id) 
        {
            if (Session["ID"] == null || !roles.tienePermiso(9, int.Parse(Session["ID"].ToString())))
            {
                return RedirectToAction("Index", "Home");
            }
            Usuario usuario = db.Usuarios.Find(id);
            if (usuario.nombreUsuario.Equals("gpuelles"))
            {
                if ((Session["ID"] == null || !(db.Usuarios.Find(int.Parse(Session["ID"].ToString())).nombreUsuario.Equals("gpuelles")))) return RedirectToAction("Index", "Home");
            }
            return View(db.Usuarios.Find(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult gestionarPermisos(FormCollection post)
        {
            if (Session["ID"] == null || !roles.tienePermiso(9, int.Parse(Session["ID"].ToString())))
            {
                return RedirectToAction("Index","Home");
            }

            int idUsuario = int.Parse(post["usuarioID"].ToString());

            Usuario usuario = db.Usuarios.Find(int.Parse(post["usuarioID"].ToString()));
            if (usuario.nombreUsuario.Equals("gpuelles"))
            {
                if ((Session["ID"] == null || !(db.Usuarios.Find(int.Parse(Session["ID"].ToString())).nombreUsuario.Equals("gpuelles")))) return RedirectToAction("Index", "Home");
            }

            string[] IDPermiso = Request.Form.GetValues("IDPermiso");
            string[] tienePermiso = Request.Form.GetValues("tienePermiso");

            roles.eliminarPermisos(idUsuario);

            for (int i = 0; i < IDPermiso.Length; i++) 
            {
                if (tienePermiso[i].Equals("si")) 
                {
                    roles.agregarPermiso(int.Parse(IDPermiso[i]), idUsuario);
                }
            }

            return RedirectToAction("Index");
        }

        public ActionResult Login()
        {
            if (Session["rol"] != null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                if (flag == 2) ViewBag.mensaje = "Contraseña cambiada";
                if (flag == 4) ViewBag.mensaje = "login erroneo";
                flag = 0;
                return View();
            }
        }

        public ActionResult ModificarMiContraseña()
        {
            if (Session["ID"] != null)
            {
                if (flag == 1) ViewBag.error = "contraseñaIncorrecta";
                flag = 0;
                Usuario usuarioActual = db.Usuarios.Find(int.Parse(Session["ID"].ToString()));
                return View(usuarioActual);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ModificarMiContraseña(FormCollection post)
        {
            Usuario usuario = db.Usuarios.Find(int.Parse(post["ID"].ToString()));
            if (usuario.password.Equals(encriptacion.GetMD5(post["passwordActual"].ToString())))
            {
                usuario.password = encriptacion.GetMD5(post["password"].ToString());

                usuario.actualizar();
                flag = 2;
                return RedirectToAction("Login");
            }
            else {
                flag = 1;
                return RedirectToAction("ModificarMiContraseña", "Usuarios");
            }
        }

        [HttpPost]
        public ActionResult verificarLogin(FormCollection post)
        {
            if (post["nombre"] == null || post["nombre"].ToString().Equals(""))
            {
                Session.RemoveAll();
                return RedirectToAction("login", "Home");
            }
            else if (Usuario.revisarUsuarioPassword(post["nombre"], encriptacion.GetMD5(post["password"])))
            {
                Session["ID"] = Usuario.obtenerIDPorNombre(post["nombre"]);
                //La sesión dura 1.5 horas por defecto
                Session.Timeout = 120;

                return RedirectToAction("Index", "Home");
            }
            flag = 4; //usuario y/o contraseña erróneos
            return RedirectToAction("login");
        }

        public ActionResult CerrarSesion()
        {
            Session.RemoveAll();
            return RedirectToAction("Index", "Home");
        }
    }
}
