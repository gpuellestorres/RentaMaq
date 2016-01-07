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
    public class OrdenDeCompraGeneralController : Controller
    {
        private Context db = new Context();
        private int numeroPermiso = 3;
        public string obtenerNombreProducto(string numeroDeParte) 
        {
            if (numeroDeParte == null) return "";
            return db.Productos.Where(s => s.numeroDeParte == numeroDeParte).ToList()[0].descripcion;
        }

        // GET: OrdenDeCompraGeneral
        public ActionResult Index(string inicio, string termino, string estado)
        {
            ViewBag.Fecha1 = DateTime.Now.AddMonths(-1);
            ViewBag.Fecha2 = DateTime.Now.AddDays(7);
            ViewBag.Estado = "TODOS";
            if (inicio != null && termino != null) 
            {
                string[] inicioSeparado = inicio.Split('-');
                string[] terminoSeparado = termino.Split('-');

                ViewBag.Fecha1 = new DateTime(int.Parse(inicioSeparado[2]), int.Parse(inicioSeparado[1]), int.Parse(inicioSeparado[0]));
                ViewBag.Fecha2 = new DateTime(int.Parse(terminoSeparado[2]), int.Parse(terminoSeparado[1]), int.Parse(terminoSeparado[0]));
            }
            if (estado != null) 
            {
                ViewBag.Estado = estado;
            }
            List<OrdenDeCompraGeneral> datos = OrdenDeCompraGeneral.Todos(ViewBag.Fecha1, ViewBag.Fecha2, ViewBag.Estado);
            return View(datos.OrderByDescending(s => s.Fecha).OrderByDescending(s=>s.OrdenDeCompraGeneralID).ToList());
        }

        // GET: OrdenDeCompraGeneral/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrdenDeCompraGeneral ordenDeCompraGeneral = OrdenDeCompraGeneral.obtener(id);
            if (ordenDeCompraGeneral == null)
            {
                return HttpNotFound();
            }
            return View(ordenDeCompraGeneral);
        }

        // GET: OrdenDeCompraGeneral/Create
        public ActionResult Create()
        {
            if (Session["ID"] == null || !roles.tienePermiso(numeroPermiso, int.Parse(Session["ID"].ToString())))
            {

                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        // POST: OrdenDeCompraGeneral/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "OrdenDeCompraGeneralID,fechaEntrega,formaRetiro,formaPago,subtotal,miscelaneos,totalNeto,IVA,total, texto, tipo")] OrdenDeCompraGeneral ordenDeCompraGeneral,
            FormCollection post)
        {
            if (Session["ID"] == null || !roles.tienePermiso(numeroPermiso, int.Parse(Session["ID"].ToString())))
            {

                return RedirectToAction("Index", "Home");
            }

            ordenDeCompraGeneral.estado = "NUEVA";
            ordenDeCompraGeneral.añoOC = DateTime.Now.Year;
            ordenDeCompraGeneral.obtenerNumeroOC();
            ordenDeCompraGeneral.ProveedorID = db.Proveedores.Find(int.Parse(post["ProveedorID"].ToString()));
            if (string.IsNullOrEmpty(ordenDeCompraGeneral.ProveedorID.personaContacto1))
            {
                ordenDeCompraGeneral.atencionA = "--";
            }
            else 
            {
                ordenDeCompraGeneral.atencionA = ordenDeCompraGeneral.ProveedorID.personaContacto1;
            }            
            
            string fechaEntrega = post["fechaEntrega"].ToString();

            ordenDeCompraGeneral.fechaEntrega = new DateTime(int.Parse(fechaEntrega.Split('/')[2]),
                int.Parse(fechaEntrega.Split('/')[1]), int.Parse(fechaEntrega.Split('/')[0]));

            string Fecha = post["Fecha"].ToString();

            ordenDeCompraGeneral.Fecha = new DateTime(int.Parse(Fecha.Split('/')[2]),
                int.Parse(Fecha.Split('/')[1]), int.Parse(Fecha.Split('/')[0]));

            /*
            string FechaVigencia = post["FechaVigencia"].ToString();

            ordenDeCompraGeneral.FechaVigencia = new DateTime(int.Parse(FechaVigencia.Split('/')[2]),
             int.Parse(FechaVigencia.Split('/')[1]), int.Parse(FechaVigencia.Split('/')[0]));
            //*/

            ordenDeCompraGeneral.obtenerID();
            
            ordenDeCompraGeneral.guardar();

            //Se guarda el detalle:
            string[] codigoDetalle = Request.Form.GetValues("codigoDetalle");
            string[] cantidadDetalle = Request.Form.GetValues("cantidadDetalle");
            string[] descripcionDetalle = Request.Form.GetValues("descripcionDetalle");
            string[] tipoDeCompraDetalle = Request.Form.GetValues("tipoDeCompraDetalle");
            string[] codigoInternoDetalleStock = Request.Form.GetValues("codigoInternoDetalleStock");
            string[] codigoInternoDetalle = Request.Form.GetValues("codigoInternoDetalle");
            string[] plazoEntregaDetalle = Request.Form.GetValues("plazoEntregaDetalle");
            string[] precioUnitarioDetalle = Request.Form.GetValues("precioUnitarioDetalle");
            string[] descuentoDetalle = Request.Form.GetValues("descuentoDetalle");
            string[] totalDetalle = Request.Form.GetValues("totalDetalle");

            for (int i = 0; i < codigoDetalle.Length; i++) 
            {
                DetalleOrdenCompra detalle = new DetalleOrdenCompra();

                //detalle.DetalleOrdenCompraID =
                detalle.IDOrdenCompra= ordenDeCompraGeneral.OrdenDeCompraGeneralID;
                detalle.codigo = codigoDetalle[i];
                detalle.cantidad = double.Parse(cantidadDetalle[i].Replace(',', '.'));
                detalle.descripcion =descripcionDetalle[i];
                detalle.tipoDeCompra = tipoDeCompraDetalle[i];
                
                if (detalle.tipoDeCompra.Equals("Compra Directa"))
                {
                    detalle.codigoInternoRentamaq = codigoInternoDetalle[i];
                }
                else 
                {
                    detalle.codigoInternoRentamaq = codigoInternoDetalleStock[i];
                }
                
                detalle.plazoEntrega =int.Parse(plazoEntregaDetalle[i].Split('.')[0]);
                detalle.precioUnitario = int.Parse(precioUnitarioDetalle[i].Split('.')[0]);
                detalle.descuento = int.Parse(descuentoDetalle[i].Split('.')[0]);
                detalle.porcentajeDescuento=0;
                detalle.valorTotal = detalle.cantidad * (detalle.precioUnitario - detalle.descuento);
                detalle.obtenerID();
                detalle.guardar();
            }

                return RedirectToAction("Index");
        }

        // GET: OrdenDeCompraGeneral/Edit/5
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
            OrdenDeCompraGeneral ordenDeCompraGeneral = OrdenDeCompraGeneral.obtener(id);
            if (ordenDeCompraGeneral == null)
            {
                return HttpNotFound();
            }
            return View(ordenDeCompraGeneral);
        }

        // POST: OrdenDeCompraGeneral/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OrdenDeCompraGeneralID,fechaEntrega,formaRetiro,formaPago,subtotal,miscelaneos,totalNeto,IVA,total, añoOC, numeroOC, texto, tipo")] OrdenDeCompraGeneral ordenDeCompraGeneral,
            FormCollection post)
        {
            if (Session["ID"] == null || !roles.tienePermiso(numeroPermiso, int.Parse(Session["ID"].ToString())))
            {

                return RedirectToAction("Index", "Home");
            }
            //ordenDeCompraGeneral.añoOC = DateTime.Now.Year;
            //ordenDeCompraGeneral.obtenerNumeroOC();
            ordenDeCompraGeneral.estado = db.ordenesDeCompra.Find(ordenDeCompraGeneral.OrdenDeCompraGeneralID).estado;
            ordenDeCompraGeneral.ProveedorID = db.Proveedores.Find(int.Parse(post["ProveedorID"].ToString()));

            if (string.IsNullOrEmpty(ordenDeCompraGeneral.ProveedorID.personaContacto1))
            {
                ordenDeCompraGeneral.atencionA = "--";
            }
            else
            {
                ordenDeCompraGeneral.atencionA = ordenDeCompraGeneral.ProveedorID.personaContacto1;
            }

            string fechaEntrega = post["fechaEntrega"].ToString();

            ordenDeCompraGeneral.fechaEntrega = new DateTime(int.Parse(fechaEntrega.Split('/')[2]),
                int.Parse(fechaEntrega.Split('/')[1]), int.Parse(fechaEntrega.Split('/')[0]));

            string Fecha = post["Fecha"].ToString();

            ordenDeCompraGeneral.Fecha = new DateTime(int.Parse(Fecha.Split('/')[2]),
                int.Parse(Fecha.Split('/')[1]), int.Parse(Fecha.Split('/')[0]));

            /*
            string FechaVigencia = post["FechaVigencia"].ToString();

            ordenDeCompraGeneral.FechaVigencia = new DateTime(int.Parse(FechaVigencia.Split('/')[2]),
                int.Parse(FechaVigencia.Split('/')[1]), int.Parse(FechaVigencia.Split('/')[0]));
            //*/
            //ordenDeCompraGeneral.obtenerID();

            ordenDeCompraGeneral.guardar();

            //Se guarda el detalle:
            string[] codigoDetalle = Request.Form.GetValues("codigoDetalle");
            string[] cantidadDetalle = Request.Form.GetValues("cantidadDetalle");
            string[] descripcionDetalle = Request.Form.GetValues("descripcionDetalle");
            string[] tipoDeCompraDetalle = Request.Form.GetValues("tipoDeCompraDetalle");
            string[] codigoInternoDetalleStock = Request.Form.GetValues("codigoInternoDetalleStock");
            string[] codigoInternoDetalle = Request.Form.GetValues("codigoInternoDetalle");
            string[] plazoEntregaDetalle = Request.Form.GetValues("plazoEntregaDetalle");
            string[] precioUnitarioDetalle = Request.Form.GetValues("precioUnitarioDetalle");
            string[] descuentoDetalle = Request.Form.GetValues("descuentoDetalle");
            string[] totalDetalle = Request.Form.GetValues("totalDetalle");
            
            ordenDeCompraGeneral.eliminarDetalleBD();

            for (int i = 0; i < codigoDetalle.Length; i++)
            {
                DetalleOrdenCompra detalle = new DetalleOrdenCompra();

                //detalle.DetalleOrdenCompraID =
                detalle.IDOrdenCompra = ordenDeCompraGeneral.OrdenDeCompraGeneralID;
                detalle.codigo = codigoDetalle[i];
                detalle.cantidad = double.Parse(cantidadDetalle[i].Replace(',', '.'));
                detalle.descripcion = descripcionDetalle[i];
                detalle.tipoDeCompra = tipoDeCompraDetalle[i];

                if (detalle.tipoDeCompra.Equals("Compra Directa"))
                {
                    detalle.codigoInternoRentamaq = codigoInternoDetalle[i];
                }
                else
                {
                    detalle.codigoInternoRentamaq = codigoInternoDetalleStock[i];
                }

                detalle.plazoEntrega = int.Parse(plazoEntregaDetalle[i]);
                detalle.precioUnitario = int.Parse(precioUnitarioDetalle[i]);
                detalle.descuento = int.Parse(descuentoDetalle[i]);
                detalle.porcentajeDescuento = 0;
                detalle.valorTotal = detalle.cantidad * (detalle.precioUnitario - detalle.descuento);
                detalle.obtenerID();
                detalle.guardar();
            }
            return RedirectToAction("Index");
        }

        // GET: OrdenDeCompraGeneral/Delete/5
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
            OrdenDeCompraGeneral ordenDeCompraGeneral = db.ordenesDeCompra.Find(id);
            if (ordenDeCompraGeneral == null)
            {
                return HttpNotFound();
            }
            return View(ordenDeCompraGeneral);
        }

        // POST: OrdenDeCompraGeneral/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["ID"] == null || !roles.tienePermiso(numeroPermiso, int.Parse(Session["ID"].ToString())))
            {

                return RedirectToAction("Index", "Home");
            }
            OrdenDeCompraGeneral ordenDeCompraGeneral = db.ordenesDeCompra.Find(id);
            db.ordenesDeCompra.Remove(ordenDeCompraGeneral);
            ordenDeCompraGeneral.eliminarDetalleBD();
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

        public ActionResult Cancelar(int? id)
        {
            if (Session["ID"] == null || !roles.tienePermiso(numeroPermiso, int.Parse(Session["ID"].ToString())))
            {

                return RedirectToAction("Index", "Home");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrdenDeCompraGeneral ordenDeCompraGeneral = OrdenDeCompraGeneral.obtener(id);
            if (ordenDeCompraGeneral == null)
            {
                return HttpNotFound();
            }
            return View(ordenDeCompraGeneral);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Cancelar([Bind(Include = "OrdenDeCompraGeneralID,FechaVigencia,NumeroEdicion,Codigo,fechaEntrega,formaRetiro,formaPago,subtotal,miscelaneos,totalNeto,IVA,total, añoOC, numeroOC, texto, atencionA")] OrdenDeCompraGeneral ordenDeCompraGeneral,
            FormCollection post)
        {
            if (Session["ID"] == null || !roles.tienePermiso(numeroPermiso, int.Parse(Session["ID"].ToString())))
            {

                return RedirectToAction("Index", "Home");
            }

            ordenDeCompraGeneral = OrdenDeCompraGeneral.obtener(int.Parse(post["OrdenDeCompraGeneralID"]));
            ordenDeCompraGeneral.estado = "CANCELADA";

            ordenDeCompraGeneral.guardar();

            return RedirectToAction("Index");
        }

        public ActionResult RegistrarEntrega(int? id)
        {
            if (Session["ID"] == null || !roles.tienePermiso(numeroPermiso, int.Parse(Session["ID"].ToString())))
            {

                return RedirectToAction("Index", "Home");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrdenDeCompraGeneral ordenDeCompraGeneral = OrdenDeCompraGeneral.obtener(id);
            if (ordenDeCompraGeneral == null)
            {
                return HttpNotFound();
            }
            return View(ordenDeCompraGeneral);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RegistrarEntrega([Bind(Include = "OrdenDeCompraGeneralID")] OrdenDeCompraGeneral ordenDeCompraGeneral,
            FormCollection post)
        {

            if (Session["ID"] == null || !roles.tienePermiso(numeroPermiso, int.Parse(Session["ID"].ToString())))
            {

                return RedirectToAction("Index", "Home");
            }


            //Se marca como ENTEGADA la OC
            ordenDeCompraGeneral = OrdenDeCompraGeneral.obtener(int.Parse(post["OrdenDeCompraGeneralID"]));
            ordenDeCompraGeneral.estado = "ENTREGADA";
            ordenDeCompraGeneral.guardar();

            //Se guardan los datos de entrega
            datosEntregaOrdenCompraGeneral datosEntrega = new datosEntregaOrdenCompraGeneral();

            datosEntrega.obtenerID();

            string fechaEntregaReal = post["fechaEntregaReal"].ToString();

            datosEntrega.fechaEntregaReal = new DateTime(int.Parse(fechaEntregaReal.Split('/')[2]),
                int.Parse(fechaEntregaReal.Split('/')[1]), int.Parse(fechaEntregaReal.Split('/')[0]));

            datosEntrega.notaRecibo = post["NotaRecibo"].ToString();
            datosEntrega.OrdenDeCompraGeneralID = ordenDeCompraGeneral.OrdenDeCompraGeneralID;

            datosEntrega.guardar();


            //Se guarda el detalle:
            string[] IDDetalle = Request.Form.GetValues("IDDetalle");
            string[] cantidadDetalle = Request.Form.GetValues("cantidadDetalle");
            string[] cantidadEntregada = Request.Form.GetValues("cantidadEntregadaDetalle");

            bool enviarCorreo = false;


            for (int i = 0; i < IDDetalle.Length; i++)
            {
                DetalleEntregaOrdenCompraGeneral detalle = new DetalleEntregaOrdenCompraGeneral();

                detalle.obtenerID();
                detalle.DetalleOrdenCompraID = int.Parse(IDDetalle[i]);
                detalle.CantidadEntregada = int.Parse(cantidadEntregada[i]);

                detalle.guardar();

                if (detalle.CantidadEntregada < int.Parse(cantidadDetalle[i])) 
                {
                    enviarCorreo = true;
                }
            }

            if (enviarCorreo) OrdenDeCompraGeneral.enviarCorreo(ordenDeCompraGeneral.OrdenDeCompraGeneralID);

            return RedirectToAction("Index");
        }

        public ActionResult ActualizarEntrega(int? id)
        {

            if (Session["ID"] == null || !roles.tienePermiso(numeroPermiso, int.Parse(Session["ID"].ToString())))
            {

                return RedirectToAction("Index", "Home");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrdenDeCompraGeneral ordenDeCompraGeneral = OrdenDeCompraGeneral.obtener(id);
            if (ordenDeCompraGeneral == null)
            {
                return HttpNotFound();
            }
            return View(ordenDeCompraGeneral);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ActualizarEntrega([Bind(Include = "OrdenDeCompraGeneralID")] OrdenDeCompraGeneral ordenDeCompraGeneral,
            FormCollection post)
        {
            if (Session["ID"] == null || !roles.tienePermiso(numeroPermiso, int.Parse(Session["ID"].ToString())))
            {

                return RedirectToAction("Index", "Home");
            }

            //Se marca como ENTEGADA la OC
            ordenDeCompraGeneral = OrdenDeCompraGeneral.obtener(int.Parse(post["OrdenDeCompraGeneralID"]));
            ordenDeCompraGeneral.estado = "ENTREGADA";
            ordenDeCompraGeneral.guardar();

            //Se guardan los datos de entrega
            datosEntregaOrdenCompraGeneral datosEntrega = new datosEntregaOrdenCompraGeneral();

            datosEntrega.datosEntregaOrdenCompraGeneralID = int.Parse(post["IDEntrega"].ToString());

            string fechaEntregaReal = post["fechaEntregaReal"].ToString();

            datosEntrega.fechaEntregaReal = new DateTime(int.Parse(fechaEntregaReal.Split('/')[2]),
                int.Parse(fechaEntregaReal.Split('/')[1]), int.Parse(fechaEntregaReal.Split('/')[0]));

            datosEntrega.notaRecibo = post["NotaRecibo"].ToString();
            datosEntrega.OrdenDeCompraGeneralID = ordenDeCompraGeneral.OrdenDeCompraGeneralID;

            datosEntrega.guardar();
            
            //Se guarda el detalle:

            datosEntrega.eliminarDetalle();

            string[] IDDetalle = Request.Form.GetValues("IDDetalle");
            string[] cantidadEntregada = Request.Form.GetValues("cantidadEntregadaDetalle");
            string[] cantidadDetalle = Request.Form.GetValues("cantidadDetalle");

            bool enviarCorreo = false;

            for (int i = 0; i < IDDetalle.Length; i++)
            {
                DetalleEntregaOrdenCompraGeneral detalle = new DetalleEntregaOrdenCompraGeneral();

                detalle.obtenerID();
                detalle.DetalleOrdenCompraID = int.Parse(IDDetalle[i]);
                detalle.CantidadEntregada = int.Parse(cantidadEntregada[i]);

                detalle.guardar();

                if (detalle.CantidadEntregada < int.Parse(cantidadDetalle[i]))
                {
                    enviarCorreo = true;
                }
            }

            if (enviarCorreo) OrdenDeCompraGeneral.enviarCorreo(ordenDeCompraGeneral.OrdenDeCompraGeneralID);

            return RedirectToAction("Index");
        }

        public ActionResult avisosCorreo()
        {
            List<avisosCorreoOrdenCompraGeneral> datos = db.avisosCorreoOrdenCompraGeneral.ToList();

            return View(datos);
        }

        public ActionResult AgregarCorreo(string id)
        {
            avisosCorreoOrdenCompraGeneral nuevo = new avisosCorreoOrdenCompraGeneral();
            if (id != null)
            {
                nuevo = db.avisosCorreoOrdenCompraGeneral.Find(int.Parse(id));
            }            
            return View(nuevo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AgregarCorreo(FormCollection post)
        {
            string id=post["id"].ToString();
            string correo = post["correo"].ToString();
            string contacto = post["contacto"].ToString();

            if (id.Equals("0")) 
            {
                avisosCorreoOrdenCompraGeneral nuevo = new avisosCorreoOrdenCompraGeneral();
                nuevo.avisosCorreoOrdenCompraGeneralID = avisosCorreoOrdenCompraGeneral.obtenerID();
                nuevo.nombreContacto = contacto;
                nuevo.correo = correo;

                nuevo.guardar();
            }
            else 
            {
                avisosCorreoOrdenCompraGeneral editar = db.avisosCorreoOrdenCompraGeneral.Find(int.Parse(id));
                editar.correo = correo;
                editar.nombreContacto = contacto;

                editar.guardar();
            }

            return RedirectToAction("avisosCorreo");
        }

        public ActionResult EliminarCorreo(string id) 
        {
            avisosCorreoOrdenCompraGeneral eliminar = db.avisosCorreoOrdenCompraGeneral.Find(int.Parse(id));
            return View(eliminar);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EliminarCorreo(FormCollection post)
        {
            string id = post["id"].ToString();
            string correo = post["correo"].ToString();
            string contacto = post["contacto"].ToString();


            avisosCorreoOrdenCompraGeneral eliminar = db.avisosCorreoOrdenCompraGeneral.Find(int.Parse(id));

            if (correo.Equals(eliminar.correo) && contacto.Equals(eliminar.nombreContacto)) 
            {
                eliminar.eliminar();
            }

            return RedirectToAction("avisosCorreo");
        }
    }
}
