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
    public class ProductoController : Controller
    {
        private Context db = new Context();

        // GET: Producto
        public ActionResult Index()
        {
            ViewData["Bodegas"] = db.bodegas.ToList();
            List<Producto> productos = (List<Producto>)TempData["productos"];
            TempData["productos"] = null;
            if (!Object.ReferenceEquals(null,productos)) {
                return View(productos);
            }
            return View(db.Productos.ToList());
        }

        public ActionResult productosStockCritico() {

            return View(Producto.listaProductosStockCritico());
        }
        public ActionResult reporteExistencias(string inicio, string termino)
            //inicio y termino deben tener el siguiente formato dia-mes-año
        {
            if (Session["ID"] == null || !roles.tienePermiso(1, int.Parse(Session["ID"].ToString())))
            {
                return RedirectToAction("Index", "Home");
            }

            DateTime Inicio = DateTime.Now.AddMonths(-1);
            DateTime Termino = DateTime.Now;
            if (inicio != null && termino != null)
            {
                string[] inicioSeparado = inicio.Split('-');
                string[] terminoSeparado = termino.Split('-');

                Inicio = new DateTime(int.Parse(inicioSeparado[2]), int.Parse(inicioSeparado[1]), int.Parse(inicioSeparado[0]));
                Termino = new DateTime(int.Parse(terminoSeparado[2]), int.Parse(terminoSeparado[1]), int.Parse(terminoSeparado[0]));
            }

            List<reportExistenciasProductos> lista = new List<reportExistenciasProductos>();

            List<string> idsProductos = Maestro.ObtenerIdsProductosPeriodo(Inicio, Termino);

            reportExistenciasProductos masIngresos = new reportExistenciasProductos(
                db.Productos.Find(int.Parse(idsProductos[0])), Inicio, Termino);
            reportExistenciasProductos masEgresos = new reportExistenciasProductos(
                db.Productos.Find(int.Parse(idsProductos[0])), Inicio, Termino);

            foreach (string IDProducto in idsProductos) 
            {
                lista.Add(new reportExistenciasProductos(db.Productos.Find(int.Parse(IDProducto)), Inicio, Termino));

                if (lista[lista.Count - 1].stockProductosIngresadosPeriodo > masIngresos.stockProductosIngresadosPeriodo)
                {
                    masIngresos = lista[lista.Count - 1];
                }
                if (lista[lista.Count - 1].stockProductosIngresadosPeriodo > masIngresos.stockProductosIngresadosPeriodo)
                {
                    masEgresos = lista[lista.Count - 1];
                }
            }

            ViewBag.ProductoMasIngresos = masIngresos;
            ViewBag.ProductoMasEgresos = masEgresos;
            return View(lista);
        }

        public ActionResult reporteExistenciasIndividual(string id, string inicio, string termino)
        //inicio y termino deben tener el siguiente formato dia-mes-año
        {
            if (Session["ID"] == null || !roles.tienePermiso(1, int.Parse(Session["ID"].ToString())))
            {
                return RedirectToAction("Index", "Home");
            }
            if (id == null || inicio==null || termino==null) {
                return RedirectToAction("reporteExistencias");
            }

             RentaMaq.DAL.Context db=new RentaMaq.DAL.Context();

            DateTime Inicio = DateTime.Now.AddMonths(-1);
            DateTime Termino = DateTime.Now;
            if (inicio != null || termino != null)
            {
                string[] inicioSeparado = inicio.Replace('/','-').Split('-');
                string[] terminoSeparado = termino.Replace('/', '-').Split('-');

                Inicio = new DateTime(int.Parse(inicioSeparado[2]), int.Parse(inicioSeparado[1]), int.Parse(inicioSeparado[0]));
                Termino = new DateTime(int.Parse(terminoSeparado[2]), int.Parse(terminoSeparado[1]), int.Parse(terminoSeparado[0]));
            }

            List<detalleReporteExistenciasProducto> lista = new List<detalleReporteExistenciasProducto>();

            Producto Producto = db.Productos.Find(int.Parse(id));

            List<Maestro> listaMaestros = db.Maestros.Where(s=>s.ProductoID==Producto.numeroDeParte && s.fecha<=Termino && s.fecha>=Inicio).OrderBy(s=>s.fecha).ToList();

            foreach (Maestro Ms in listaMaestros) 
            {
                lista.Add(new detalleReporteExistenciasProducto(Ms, Inicio, Termino));
            }

            ViewBag.IDPRODUCTO = id;
            ViewBag.ReporteGeneral = new reportExistenciasProductos(db.Productos.Find(int.Parse(id)), Inicio, Termino);

            return View(lista);
        }

        public ActionResult indicadoresClave() 
        {
            if (Session["ID"] == null || !roles.tienePermiso(1, int.Parse(Session["ID"].ToString())))
            {
                return RedirectToAction("Index", "Home");
            }
            List<IndicadoresClaveProducto> datos = IndicadoresClaveProducto.obtenerDatos();
            return View(datos.OrderBy(s => s.tiempoRespuestaPromedio).ToList());
        }


        public ActionResult indicadoresClaveProducto(int id)
        {
            if (Session["ID"] == null || !roles.tienePermiso(1, int.Parse(Session["ID"].ToString())))
            {
                return RedirectToAction("Index", "Home");
            }
            return View(new IndicadoresClaveProducto(id));
        }

        // GET: Producto/Details/5
        public ActionResult Details(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Producto producto = db.Productos.Find(id);
            if (producto == null)
            {
                return HttpNotFound();
            }
            return View(producto);
        }

        // GET: Producto/Create
        public ActionResult Create()
        {
            if (Session["ID"] == null || !roles.tienePermiso(1, int.Parse(Session["ID"].ToString())))
            {
                return RedirectToAction("Index", "Home");
            }
            ViewData["Bodegas"] = db.bodegas.ToList();
            return View();
        }

        // POST: Producto/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductoID,numeroDeParte,precioUnitario,descripcion,descripcion2,stockMinimo,unidadDeMedida,ubicacion,codigo,idBodega")] Producto producto)
        {
            if (Session["ID"] == null || !roles.tienePermiso(1, int.Parse(Session["ID"].ToString())))
            {
                return RedirectToAction("Index", "Home");
            }
            if (ModelState.IsValid)
            {
                producto.stockActual = 0;
                db.Productos.Add(producto);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(producto);
        }

        // GET: Producto/Edit/5
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
            ViewData["Bodegas"] = db.bodegas.ToList();
            Producto producto = db.Productos.Find(id);
            if (producto == null)
            {
                return HttpNotFound();
            }
            return View(producto);
        }

        // POST: Producto/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductoID,numeroDeParte,precioUnitario,descripcion,descripcion2,stockMinimo,stockActual,unidadDeMedida,ubicacion,codigo,idBodega")] Producto producto, FormCollection form)
        {
            if (Session["ID"] == null || !roles.tienePermiso(1, int.Parse(Session["ID"].ToString())))
            {
                return RedirectToAction("Index", "Home");
            }           
            
            double stockActual = Convert.ToDouble((string)form["stockActual"].Replace(".",","));
            producto.stockActual = stockActual;
            //if (ModelState.IsValid)
            //{
                db.Entry(producto).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            //}
            //return View(producto);
        }

        // GET: Producto/Delete/5
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
            Producto producto = db.Productos.Find(id);
            if (producto == null)
            {
                return HttpNotFound();
            }
            return View(producto);
        }

        // POST: Producto/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["ID"] == null || !roles.tienePermiso(1, int.Parse(Session["ID"].ToString())))
            {
                return RedirectToAction("Index", "Home");
            }
            Producto producto = db.Productos.Find(id);            

            string productoID = producto.ProductoID.ToString();

            db.Maestros.RemoveRange(db.Maestros.Where(s => s.ProductoID == productoID));

            db.Productos.Remove(producto);
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


        public ActionResult BuscarProducto(FormCollection form)
        {
            string codigoProducto = form["codigoProducto"];
            List<Producto> Listaproductos = db.Productos.Where(c => c.numeroDeParte == codigoProducto).ToList();
            if (codigoProducto == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (Listaproductos == null)
            {
                return HttpNotFound();
            }
            TempData["productos"] = Listaproductos;
            return RedirectToAction("Index");
            //return View("Index", new { productos = productos});
        }
        public string verificarExistenciaCodigo(string numeroDeParte) {
            
            //Producto producto = db.Productos.Find(id);
            var producto = db.Productos.SingleOrDefault(c => c.numeroDeParte == numeroDeParte);            
            if (producto == null)
            {
                return "false";
            }
            else {
                return "true";
            }
        
        }

        public JsonResult FiltrarProductosStockCriticos()
        {            
            var L2EQuery = db.Productos.Where(s => s.stockActual<=s.stockMinimo);
            var productos = L2EQuery.ToList();
            return Json(productos, JsonRequestBehavior.AllowGet);
        }


        public JsonResult Filtrar(string idBodega, string stockCritico, string descripcion)
        {            

            using (var context = new Context())
            {
                //PRODUCTO
                if (idBodega.Equals("TODAS") && stockCritico.Equals("TODOS"))
                {
                    var L2EQuery = context.Productos.ToList();
                    var PRODUCTOS = L2EQuery.ToList();
                    PRODUCTOS = filtrarDescripcionNParte(PRODUCTOS, descripcion);
                    return Json(PRODUCTOS, JsonRequestBehavior.AllowGet);
                }
                else if (idBodega.Equals("TODAS") && stockCritico.Equals("CRITICO"))
                {
                    var L2EQuery = context.Productos.Where(s => s.stockActual <= s.stockMinimo);
                    var PRODUCTOS = L2EQuery.ToList();
                    PRODUCTOS = filtrarDescripcionNParte(PRODUCTOS, descripcion);
                    return Json(PRODUCTOS, JsonRequestBehavior.AllowGet);
                }
                else {

                    int IDBODEGA = Convert.ToInt32(idBodega);
                    if (stockCritico.Equals("CRITICO"))
                    {
                        var L2EQuery = context.Productos.Where(s => s.stockActual <= s.stockMinimo & s.idBodega == IDBODEGA);
                        var PRODUCTOS = L2EQuery.ToList();
                        PRODUCTOS = filtrarDescripcionNParte(PRODUCTOS, descripcion);
                        return Json(PRODUCTOS, JsonRequestBehavior.AllowGet);
                    }
                    else {
                        var L2EQuery = context.Productos.Where(s => s.idBodega == IDBODEGA);
                        var PRODUCTOS = L2EQuery.ToList();
                        PRODUCTOS = filtrarDescripcionNParte(PRODUCTOS, descripcion);
                        return Json(PRODUCTOS, JsonRequestBehavior.AllowGet);
                    }
                    
                }

            }

            return null;
        }

        private List<Producto> filtrarDescripcionNParte(List<Producto> PRODUCTOS, string filtro)
        {
            string FILTRO = filtro.ToUpper();
            if (filtro.Equals("")) return PRODUCTOS;
            else
            {
                List<Producto> retorno = PRODUCTOS;

                for (int i = retorno.Count - 1; i >= 0; i--)
                {
                    string descripcion = retorno[i].descripcion.ToUpper();
                    string nParte = retorno[i].numeroDeParte.ToUpper();

                    if (!descripcion.Contains(FILTRO) && !nParte.Contains(FILTRO)) 
                    {
                        retorno.RemoveAt(i);
                    }
                }

                return retorno;
            }
        }
    }
}
