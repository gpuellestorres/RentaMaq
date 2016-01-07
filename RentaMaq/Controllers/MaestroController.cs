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
    public class MaestroController : Controller
    {
        private Context db = new Context();

        // GET: Maestro
        public ActionResult Index()
        {
            DateTime fechaActual = DateTime.Now;
            DateTime fechaAnterior = DateTime.Now.AddDays(-7);
            

            Context context = new Context();
            var L2EQuery = context.Maestros.Where(s => s.fecha >= fechaAnterior && s.fecha <= fechaActual);
            var maestros = L2EQuery.ToList();

            ViewBag.fechaActual = Formateador.fechaCompletaToString(fechaActual);
            ViewBag.fechaAnterior = Formateador.fechaCompletaToString(fechaAnterior);

            //db.Maestros.ToList()
            return View(maestros);
        }

        // GET: Maestro/Details/5
        public ActionResult Details(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Maestro maestro = db.Maestros.Find(id);
            if (maestro == null)
            {
                return HttpNotFound();
            }
            return View(maestro);
        }

        // GET: Maestro/Create
        public ActionResult Create(string verificador)
        {
            if (Session["ID"] == null || !roles.tienePermiso(1, int.Parse(Session["ID"].ToString())))
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.verificador = verificador;

            ViewData["Productos"] = db.Productos.ToList();
            ViewData["Proveedores"] = db.Proveedores.ToList(); //Json(listaProveedores, JsonRequestBehavior.AllowGet);
            return View();
        }

        // POST: Maestro/Create        
        [HttpPost]
        [MultiButton(MatchFormKey = "action", MatchFormValue = "Registrar y Volver")]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "fecha,ProductoID,descripcionProducto,cantidadEntrante,cantidadSaliente,facturaDespacho,proveedor,valorUnitario,valorTotal,afiEquipo,entragadoA,motivo,numeroFormulario,observaciones")] Maestro maestro, FormCollection form) //, FormCollection form
        {
            if (Session["ID"] == null || !roles.tienePermiso(1, int.Parse(Session["ID"].ToString())))
            {
                return RedirectToAction("Index", "Home");
            }
            maestro.fecha = Formateador.fechaFormatoGuardar(form["fecha"].ToString());
            Producto producto = db.Productos.Find(int.Parse(maestro.ProductoID));
                                
            maestro.cantidadEntrante = ToDouble((string)form["cantidadEntrante"]);
            maestro.cantidadSaliente = ToDouble((string)form["cantidadSaliente"]);

            producto.stockActual = producto.stockActual + maestro.cantidadEntrante - maestro.cantidadSaliente;
            verificarStockCritico(producto);
            db.Entry(producto).State = EntityState.Modified;

            db.Maestros.Add(maestro);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        [MultiButton(MatchFormKey = "action", MatchFormValue = "Registrar y Agregar Entrada")]
        [ValidateAntiForgeryToken]
        public ActionResult CreateAddEntrada([Bind(Include = "fecha,ProductoID,descripcionProducto,cantidadEntrante,cantidadSaliente,facturaDespacho,proveedor,valorUnitario,valorTotal,afiEquipo,entragadoA,motivo,numeroFormulario,observaciones")] Maestro maestro, FormCollection form) //, FormCollection form
        {
            if (Session["ID"] == null || !roles.tienePermiso(1, int.Parse(Session["ID"].ToString())))
            {
                return RedirectToAction("Index", "Home");
            }
            maestro.fecha = Formateador.fechaFormatoGuardar(form["fecha"].ToString());
            Producto producto = db.Productos.Find(int.Parse(maestro.ProductoID));


            maestro.cantidadEntrante = ToDouble((string)form["cantidadEntrante"]);
            maestro.cantidadSaliente = ToDouble((string)form["cantidadSaliente"]);

            producto.stockActual = producto.stockActual + maestro.cantidadEntrante - maestro.cantidadSaliente;
            verificarStockCritico(producto);
            db.Entry(producto).State = EntityState.Modified;
            db.Maestros.Add(maestro);
            db.SaveChanges();
            return RedirectToAction("Create", new { verificador = "entrada" });
        }

        [HttpPost]
        [MultiButton(MatchFormKey = "action", MatchFormValue = "Registrar y Agregar Salida")]
        [ValidateAntiForgeryToken]
        public ActionResult CreateAddSalida([Bind(Include = "fecha,ProductoID,descripcionProducto,cantidadEntrante,cantidadSaliente,facturaDespacho,proveedor,valorUnitario,valorTotal,afiEquipo,entragadoA,motivo,numeroFormulario,observaciones")] Maestro maestro, FormCollection form) //, FormCollection form
        {
            if (Session["ID"] == null || !roles.tienePermiso(1, int.Parse(Session["ID"].ToString())))
            {
                return RedirectToAction("Index", "Home");
            }
            maestro.fecha = Formateador.fechaFormatoGuardar(form["fecha"].ToString());
            Producto producto = db.Productos.Find(int.Parse(maestro.ProductoID));

            maestro.cantidadEntrante = ToDouble((string)form["cantidadEntrante"]);
            maestro.cantidadSaliente = ToDouble((string)form["cantidadSaliente"]);

            producto.stockActual = producto.stockActual + maestro.cantidadEntrante - maestro.cantidadSaliente;
            verificarStockCritico(producto);
            db.Entry(producto).State = EntityState.Modified;
            db.Maestros.Add(maestro);
            db.SaveChanges();
            return RedirectToAction("Create", new { verificador = "salida" });
        }

        // GET: Maestro/Edit/5
        public ActionResult Edit(int id)
        {
            if (Session["ID"] == null || !roles.tienePermiso(1, int.Parse(Session["ID"].ToString())))
            {
                return RedirectToAction("Index", "Home");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Maestro maestro = db.Maestros.Find(id);

            if (maestro.cantidadEntrante > 0)
            {
                ViewBag.verificador = "entrada";
            }
            else {
                ViewBag.verificador = "salida";
            }


            ViewData["Productos"] = db.Productos.ToList();
            ViewData["Proveedores"] = db.Proveedores.ToList(); 
            ViewBag.Fecha = maestro.fecha;
            ViewBag.cantidadEntrante = maestro.cantidadEntrante;
            ViewBag.cantidadSaliente = maestro.cantidadSaliente;
            
            if (maestro == null)
            {
                return HttpNotFound();
            }
            return View(maestro);
        }

        // POST: Maestro/Edit/5      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaestroID,fecha,ProductoID,descripcionProducto,cantidadEntrante,cantidadSaliente,facturaDespacho,proveedor,valorUnitario,valorTotal,afiEquipo,entragadoA,motivo,numeroFormulario,observaciones")] Maestro maestro, FormCollection form)
        {
            if (Session["ID"] == null || !roles.tienePermiso(1, int.Parse(Session["ID"].ToString())))
            {
                return RedirectToAction("Index", "Home");
            }

            double cantidadEntranteAnterior = ToDouble((string)form["entrante"]);
            double cantidadSalienteAnterior = ToDouble((string)form["saliente"]);

            string fecha = form["fecha"].ToString();

            DateTime Fecha = new DateTime(int.Parse(fecha.Split('/')[2]), int.Parse(fecha.Split('/')[1]), int.Parse(fecha.Split('/')[0]));

            maestro.fecha = Formateador.fechaFormatoGuardar(fecha);

            //if (ModelState.IsValid)
            //{
                int ProductoID = int.Parse(maestro.ProductoID);
                Producto producto = db.Productos.SingleOrDefault(c => c.ProductoID == ProductoID);

                maestro.cantidadEntrante = ToDouble((string)form["cantidadEntrante"]);
                maestro.cantidadSaliente = ToDouble((string)form["cantidadSaliente"]);
            
                maestro.descripcionProducto = producto.descripcion;
                double total = 0;
                if (cantidadEntranteAnterior > 0)
                {
                    total = maestro.cantidadEntrante - cantidadEntranteAnterior;
                    producto.stockActual = producto.stockActual + total;
                }
                else
                {
                    total = maestro.cantidadSaliente - cantidadSalienteAnterior;
                    producto.stockActual = producto.stockActual - total;
                }
                               
                db.Entry(producto).State = EntityState.Modified;               
                db.Entry(maestro).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            //}
            //return View(maestro);
        }

        // GET: Maestro/Delete/5
        public ActionResult Delete(int id)
        {
            if (Session["ID"] == null || !roles.tienePermiso(1, int.Parse(Session["ID"].ToString())))
            {
                return RedirectToAction("Index", "Home");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Maestro maestro = db.Maestros.Find(id);
            if (maestro == null)
            {
                return HttpNotFound();
            }
            return View(maestro);
        }

        // POST: Maestro/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["ID"] == null || !roles.tienePermiso(1, int.Parse(Session["ID"].ToString())))
            {
                return RedirectToAction("Index", "Home");
            }

            Maestro maestro = db.Maestros.Find(id);

            Producto producto = db.Productos.Find(int.Parse(maestro.ProductoID));
            producto.stockActual = producto.stockActual - maestro.cantidadEntrante + maestro.cantidadSaliente;
            db.Entry(producto).State = EntityState.Modified;

            db.Maestros.Remove(maestro);
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

        public JsonResult obtenerNombreProducto(string id) {
            var producto = db.Productos.Find(int.Parse(id));
            
            if (producto == null)
            {
                return null;
            }
            else {
                return Json(producto,JsonRequestBehavior.AllowGet);
                
            }
        }


        private double ToDouble(string numero) {

            if (!Object.ReferenceEquals(null, numero)) 
            {
                numero=numero.Replace(".", ",");
                return Convert.ToDouble(numero);
            }
            else {
                return 0;
            }                    
        }

        public JsonResult Filtrar(string fecha1, string fecha2, string bodega, string producto)
        {
            DateTime fecha_1 = Formateador.formatearFechaCompleta(fecha1);
            DateTime fecha_2 = Formateador.formatearFechaCompleta(fecha2);

            using (var context = new Context())
            {
                //PRODUCTO
                if (bodega.Equals("Todas") && producto.Equals("Todos")) {
                    var L2EQuery = context.Maestros.Where(s => s.fecha >= fecha_1 && s.fecha <= fecha_2);
                    var student = L2EQuery.ToList();
                    return Json(student, JsonRequestBehavior.AllowGet);
                } 
                 //PROVEEDOR
                else if (bodega.Equals("Todas"))
                {
                    var L2EQuery = context.Maestros.Where(s => s.fecha >= fecha_1 && s.fecha <= fecha_2 && s.ProductoID == producto);
                    var student = L2EQuery.ToList();
                    return Json(student, JsonRequestBehavior.AllowGet);
                }
                //AFI EQUIPO
                else if (producto.Equals("Todos")) {
                    List<Maestro> lista = context.Maestros.Where(s => s.fecha >= fecha_1 && s.fecha <= fecha_2).ToList();
                    for(int i=lista.Count-1;i>=0;i--)
                    {
                       /* string idProd = lista[i].ProductoID;
                        List<Producto> productos = context.Productos.Where(s=>s.numeroDeParte==idProd).ToList();
                        if(productos.Count>0){
                        Producto elProducto = productos[0];
                        if (elProducto.idBodega != int.Parse(bodega))
                        {
                            lista.RemoveAt(i);
                        }
                        }//*/

                        if (lista[i].ProductoID.Equals("-1") 
                            || context.Productos.Find(int.Parse(lista[i].ProductoID)).idBodega != int.Parse(bodega))
                        {
                            lista.RemoveAt(i);
                        }
                        
                    }
                    var student = lista;
                    return Json(student, JsonRequestBehavior.AllowGet);
                }
                else
                {

                    List<Maestro> lista = context.Maestros.Where(s => s.fecha >= fecha_1 && s.fecha <= fecha_2 && s.ProductoID == producto).ToList();
                    for (int i = lista.Count - 1; i >= 0; i--)
                    {
                        if (lista[i].ProductoID.Equals("-1")
                            || context.Productos.Find(int.Parse(lista[i].ProductoID)).idBodega != int.Parse(bodega))
                        {
                            lista.RemoveAt(i);
                        }
                    }
                    var student = lista;
                    return Json(student, JsonRequestBehavior.AllowGet);
                }                
                
            }
        }

       
        public ActionResult listaCorreosStockCritico() {
            if (Session["ID"] == null || !roles.tienePermiso(9, int.Parse(Session["ID"].ToString())))
            {
                return RedirectToAction("Index", "Home");
            }   
            List<listaCorreosStockCritico> list = RentaMaq.Models.listaCorreosStockCritico.obtenerTodos();
            ViewBag.cont = list.Count +1;            
            return View(list);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult listaCorreosStockCritico(FormCollection form) 
        {    
            string[] nombres = Request.Form.GetValues("nombre");
            string[] correos = Request.Form.GetValues("correo");
            string verificador = "true";

            
            RentaMaq.Models.listaCorreosStockCritico.eliminarTodos();
            try
            {
                for (int i = 0; i < nombres.Length; i++)
                {
                    RentaMaq.Models.listaCorreosStockCritico.guardarPersona(nombres[i], correos[i]);
                }
            }
            catch (Exception) {
                verificador = "false";
            }
            ViewBag.verificador = verificador;
            List<listaCorreosStockCritico> list = RentaMaq.Models.listaCorreosStockCritico.obtenerTodos();
            ViewBag.cont = list.Count;
            return View("listaCorreosStockCritico",list);
        }
        

        private void verificarStockCritico(Producto producto){
            
             if (producto.stockActual <= producto.stockMinimo) {
                 string mensaje = "Estimados:<br>El producto " + producto.descripcion + " Numero de Parte:" + producto.numeroDeParte + " esta en un nivel de Stock Critico<br>" +
                             "<b>Stock Minimo</b>=" + producto.stockMinimo + "<br><b>Stock Actual=</b>" + producto.stockActual;                           
                    envioCorreos.enviarAlerta(RentaMaq.Models.listaCorreosStockCritico.obtenerTodosCorreos() ,"Producto Stock Critico",mensaje);
                
            }
                                 
        } 
        
    }





    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class MultiButtonAttribute : ActionNameSelectorAttribute
    {
        public string MatchFormKey { get; set; }
        public string MatchFormValue { get; set; }

        public override bool IsValidName(ControllerContext controllerContext, string actionName, System.Reflection.MethodInfo methodInfo)
        {
            return controllerContext.HttpContext.Request[MatchFormKey] != null &&
                controllerContext.HttpContext.Request[MatchFormKey] == MatchFormValue;
        }
    }
}
