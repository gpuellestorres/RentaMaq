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
using System.IO;

namespace RentaMaq.Controllers
{
    public class ordenDeTrabajoGeneralController : Controller
    {
        private Context db = new Context();
        private int numeroPermiso = 16;

        public string verificarnumeroFolio(string numeroFolio) 
        {
            //int temp = 0;
            if (Object.ReferenceEquals(null,numeroFolio) || numeroFolio.Equals("") ) return "false"; //!int.TryParse(numeroFolio, out temp)

            //int numeroFolioInt = int.Parse(numeroFolio);

            if (db.ordenDeTrabajoGenerals.Where(s => s.numeroFolio == numeroFolio).ToList().Count > 0) return "true";

            return "false";
        }

        // GET: ordenDeTrabajoGeneral
        public ActionResult Index(string inicio, string termino,string IDEquipo)
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
            if (IDEquipo == null || IDEquipo.Equals(""))
            {
                List<ordenDeTrabajoGeneral> lista =
                    db.ordenDeTrabajoGenerals.Where(s => s.fechaOTAbierta >= Inicio && s.fechaOTAbierta <= fin).OrderByDescending(s => s.ordenDeTrabajoGeneralID).ToList();
                return View(lista);
            }
            else {
                int idEq = int.Parse(IDEquipo);
                equipos Equipo = equipos.Obtener(idEq);
                IDEquipo=Equipo.ID.ToString();
                List<ordenDeTrabajoGeneral> lista =
                        db.ordenDeTrabajoGenerals.Where(s => s.fechaOTAbierta >= Inicio && s.fechaOTAbierta <= fin && s.idEquipo == IDEquipo).OrderByDescending(s => s.ordenDeTrabajoGeneralID).ToList();
                return View(lista);
            }                        
        }

        // GET: ordenDeTrabajoGeneral/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ordenDeTrabajoGeneral ordenDeTrabajoGeneral = db.ordenDeTrabajoGenerals.Find(id);
            if (ordenDeTrabajoGeneral == null)
            {
                return HttpNotFound();
            }
            int idOT = Convert.ToInt32(id);
            List<ejecutanteTrabajoOT> ejecutantesTrabajoOT = db.ejecutanteTrabajoOTs.Where(s => s.ordenDeTrabajoGeneralID == idOT).ToList();
            List<materialesRequeridosOT> materialesRequeridosOT = db.materialesRequeridosOTs.Where(s => s.ordenDeTrabajoGeneralID == idOT).ToList();
            List<materialesUtilizadosOT> materialesUtilizadosOT = db.materialesUtilizadosOTs.Where(s => s.ordenDeTrabajoGeneralID == idOT).ToList();

            foreach (RentaMaq.Models.materialesRequeridosOT matReq in materialesRequeridosOT) {
                Producto product = db.Productos.Find(matReq.materialID);
                matReq.numeroParte = product.numeroDeParte;
            }

            foreach (RentaMaq.Models.materialesUtilizadosOT matUt in materialesUtilizadosOT)
            {
                Producto product = db.Productos.Find(matUt.materialID);
                matUt.numeroParte = product.numeroDeParte;
            }

            ViewData["ejecutantesTrabajoOT"] = ejecutantesTrabajoOT;
            ViewData["materialesRequeridosOT"] = materialesRequeridosOT;
            ViewData["materialesUtilizadosOT"] = materialesUtilizadosOT;
            return View(ordenDeTrabajoGeneral);
        }

        // GET: ordenDeTrabajoGeneral/Create
        public ActionResult Create()
        {
            if (Session["ID"] == null || !roles.tienePermiso(numeroPermiso, int.Parse(Session["ID"].ToString())))
            {
                return RedirectToAction("Index", "Home");
            }
            ViewData["equipos"] = equipos.todosConTipo();
            ViewData["materiales"] = db.Productos.ToList();
            return View();
        }

        // POST: ordenDeTrabajoGeneral/Create       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ordenDeTrabajoGeneralID,fechaOTAbierta,fechaOTCerrada,operador,faena,turno,idEquipo,horometro,kilometraje,tipoMantenimientoARealizar,horasMantenimientoNivelCombustible,horasMantenimientoFecha,horasMantenimientoHRInicio,horasMantenimientoHRTermino,horasMantenimientoHRSDetenido,trabajoRealizar,conclusionesTrabajoRealizado,estadoEquipo,trabajosPendientesPorRealizar,fechaTrabajosPendientesPorRealizar,numeroFolio,area,nombreMantenedor,nombreOperador,nombreSupervisor,tipoOTSegunMantenimiento")] ordenDeTrabajoGeneral ordenDeTrabajoGeneral, FormCollection form, HttpPostedFileBase file)
        {
            if (Session["ID"] == null || !roles.tienePermiso(numeroPermiso, int.Parse(Session["ID"].ToString())))
            {
                return RedirectToAction("Index", "Home");
            }
                ordenDeTrabajoGeneral.fechaOTAbierta=Formateador.fechaFormatoGuardar((string)form["fechaOTAbierta"]);
                ordenDeTrabajoGeneral.fechaOTCerrada =Formateador.fechaFormatoGuardar((string)form["fechaOTCerrada"]);
                ordenDeTrabajoGeneral.horasMantenimientoFecha =Formateador.fechaFormatoGuardar((string)form["horasMantenimientoFecha"]);
                ordenDeTrabajoGeneral.fechaTrabajosPendientesPorRealizar = Formateador.fechaFormatoGuardar((string)form["fechaTrabajosPendientesPorRealizar"]);
                
                equipos equipo = equipos.ObtenerConTipo(Convert.ToInt32(ordenDeTrabajoGeneral.idEquipo));
                ordenDeTrabajoGeneral.patenteEquipo = equipo.patenteEquipo;
                ordenDeTrabajoGeneral.tipoEquipo = equipo.tipoEquipo;
                ordenDeTrabajoGeneral.verificarTrabajoPendiente = "FALSE";
                ordenDeTrabajoGeneral.horasMantenimientoHRSDetenido = double.Parse((string)form["horasMantenimientoHRSDetenido"].Replace(',', '.'));
                db.ordenDeTrabajoGenerals.Add(ordenDeTrabajoGeneral);
                if (db.ordenDeTrabajoGenerals.Where(s => s.numeroFolio == ordenDeTrabajoGeneral.numeroFolio).ToList().Count > 0) return RedirectToAction("Index");
                db.SaveChanges();

                int idOT = ordenDeTrabajoGeneral.ordenDeTrabajoGeneralID;
                string[] ejecutantesDelTrabajo = Request.Form.GetValues("ejecutanteDelTrabajo");
                string[] cargo = Request.Form.GetValues("cargo");
                string[] HH = Request.Form.GetValues("HH");
                for (int i = 0; i < ejecutantesDelTrabajo.Length; i++) {
                    ejecutanteTrabajoOT ejecutanteTrabajoOT = new ejecutanteTrabajoOT();
                    ejecutanteTrabajoOT.ordenDeTrabajoGeneralID = idOT;
                    ejecutanteTrabajoOT.nombreTrabajador = ejecutantesDelTrabajo[i];
                    ejecutanteTrabajoOT.cargo = cargo[i];
                    if (HH[i].Equals(""))
                    {
                        ejecutanteTrabajoOT.HH = 0;
                    }
                    else {
                        ejecutanteTrabajoOT.HH = Convert.ToDouble(HH[i]);
                    }    
                    
                    db.ejecutanteTrabajoOTs.Add(ejecutanteTrabajoOT);
                }


                           


                

                string[] materialUtilizado = Request.Form.GetValues("materialUtilizado");
                string[] matUtcantidad = Request.Form.GetValues("matUtcantidad");
                string[] matUtNumeroParte = Request.Form.GetValues("matUtNumeroParte");
                if (!materialUtilizado[0].Equals(""))
                {
                    

                    for (int i = 0; i < materialUtilizado.Length; i++)
                    {
                        materialesUtilizadosOT materialesUtilizadosOT = new materialesUtilizadosOT();
                        materialesUtilizadosOT.ordenDeTrabajoGeneralID = idOT;
                        materialesUtilizadosOT.nombreMaterial = materialUtilizado[i];
                        materialesUtilizadosOT.cantidad = Convert.ToDouble(matUtcantidad[i]);
                        materialesUtilizadosOT.materialID = Convert.ToInt32(matUtNumeroParte[i]);
                        materialesUtilizadosOT.precioActual = db.Productos.Find(Convert.ToInt32(matUtNumeroParte[i])).precioUnitario;
                        db.materialesUtilizadosOTs.Add(materialesUtilizadosOT);


                        Maestro maestro = new Maestro();
                        maestro.afiEquipo = equipo.numeroAFI;
                        maestro.fecha = ordenDeTrabajoGeneral.horasMantenimientoFecha;
                        maestro.descripcionProducto = materialUtilizado[i];
                        maestro.cantidadEntrante = 0;
                        maestro.cantidadSaliente = Convert.ToDouble(matUtcantidad[i]);
                        maestro.idOT = idOT;
                        maestro.ProductoID = matUtNumeroParte[i];
                        maestro.observaciones="Agregada Automaticamente de OT:"+ordenDeTrabajoGeneral.numeroFolio;

                        Producto producto = db.Productos.Find(int.Parse(maestro.ProductoID));

                        producto.stockActual = producto.stockActual - maestro.cantidadSaliente;                      
                        db.Entry(producto).State = EntityState.Modified;
                        db.Maestros.Add(maestro);

                    }
                }
                //save

                string[] materialRequerido = Request.Form.GetValues("materialRequerido");
                string[] matReqCantidad = Request.Form.GetValues("matReqCantidad");
                string[] matReqNumeroParte = Request.Form.GetValues("matReqNumeroParte");

               

                if (!materialRequerido[0].Equals(""))
                {
                    pedidos pedido = new pedidos();
                    pedido.fecha = Formateador.formatearFechaCompleta(DateTime.Now);
                    pedido.estado = "NUEVA";
                    pedido.nota = "Agregado Automaticamente desde OT:" + ordenDeTrabajoGeneral.numeroFolio;
                    pedido.idOT = idOT;
                    db.pedidos.Add(pedido);
                    db.SaveChanges();

                    for (int i = 0; i < materialRequerido.Length; i++)
                    {
                        materialesRequeridosOT materialesRequeridosOT = new materialesRequeridosOT();
                        materialesRequeridosOT.ordenDeTrabajoGeneralID = idOT;
                        materialesRequeridosOT.nombreMaterial = materialRequerido[i];
                        materialesRequeridosOT.cantidad = Convert.ToDouble(matReqCantidad[i]);
                        materialesRequeridosOT.materialID = Convert.ToInt32(matReqNumeroParte[i]);
                        materialesRequeridosOT.precioActual = db.Productos.Find(Convert.ToInt32(matReqNumeroParte[i])).precioUnitario;
                        db.materialesRequeridosOTs.Add(materialesRequeridosOT);

                        detallePedido detallePedido = new detallePedido();
                        detallePedido.cantidad = Convert.ToDouble(materialesRequeridosOT.cantidad);
                        detallePedido.descripcion = materialesRequeridosOT.nombreMaterial;
                        detallePedido.numeroParte = db.Productos.Find(materialesRequeridosOT.materialID).numeroDeParte;
                        detallePedido.pedidosID = pedido.pedidosID;
                        detallePedido.tipoPedido = "DIRECTA";
                        detallePedido.detalleTipoPedido = db.Equipos.Find(Convert.ToInt32(ordenDeTrabajoGeneral.idEquipo)).numeroAFI.ToString();
                        db.detallePedidos.Add(detallePedido);

                    }
                }
                



                crearCarpetaSiNoExiste();
                string extImage = Convert.ToString(Request.Files["file"].ContentType);
                string[] infoImage = extImage.Split('/');
                string fileExtension = System.IO.Path.GetExtension(Request.Files["file"].FileName);
                string fileLocation = Server.MapPath("~/Images/OrdenTrabajo/") + ordenDeTrabajoGeneral.numeroFolio + "." + infoImage[1];
                if (!fileExtension.Equals(""))
                {
                    Request.Files["file"].SaveAs(fileLocation);
                    ordenDeTrabajoGeneral.rutaImagen = "Images/OrdenTrabajo/" + ordenDeTrabajoGeneral.numeroFolio + "." + infoImage[1];
                }

                registrokmhm registro = new registrokmhm();
                registro.equipoID = Convert.ToInt32(ordenDeTrabajoGeneral.idEquipo);
                registro.fecha = Formateador.fechaFormatoGuardar((string)form["fechaOTAbierta"]);
                registro.horometro = ordenDeTrabajoGeneral.horometro;
                registro.kilometraje=ordenDeTrabajoGeneral.kilometraje;
                //db.registrokmhms.Add(registro);
                registrokmhm.actualizarRegistroKmHm(registro);

                mantencionPreventiva mantecionPreventiva = new mantencionPreventiva();
                mantecionPreventiva.equipoID = Convert.ToInt32(ordenDeTrabajoGeneral.idEquipo);
                mantecionPreventiva.fecha = ordenDeTrabajoGeneral.horasMantenimientoFecha;
                mantecionPreventiva.horometroActual = ordenDeTrabajoGeneral.horometro;
                mantecionPreventiva.kilometrajeActual=ordenDeTrabajoGeneral.kilometraje;

                if(ordenDeTrabajoGeneral.horometro==0){
                    mantecionPreventiva.horometroProximaMantencion = 0;
                }else{
                    mantecionPreventiva.horometroProximaMantencion = ordenDeTrabajoGeneral.horometro + 400;
                }

                if (ordenDeTrabajoGeneral.kilometraje == 0)
                {
                    mantecionPreventiva.kilometrajeProximaMantencion = 0;
                }
                else {
                    mantecionPreventiva.kilometrajeProximaMantencion = ordenDeTrabajoGeneral.kilometraje + 10000;
                }

                mantecionPreventiva.nota = "Agregado Automaticamente desde OT N°:" + ordenDeTrabajoGeneral.numeroFolio;
                RentaMaq.Models.mantencionPreventiva.reemplazar(mantecionPreventiva);


                               
                db.SaveChanges();
                                                
                return RedirectToAction("Index");
         

         
        }

        // GET: ordenDeTrabajoGeneral/Edit/5
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
            ordenDeTrabajoGeneral ordenDeTrabajoGeneral = db.ordenDeTrabajoGenerals.Find(id);
            if (ordenDeTrabajoGeneral == null)
            {
                return HttpNotFound();
            }

            int idOT = Convert.ToInt32(id);
            List<ejecutanteTrabajoOT> ejecutantesTrabajoOT = db.ejecutanteTrabajoOTs.Where(s => s.ordenDeTrabajoGeneralID == idOT).ToList();
            List<materialesRequeridosOT> materialesRequeridosOT = db.materialesRequeridosOTs.Where(s => s.ordenDeTrabajoGeneralID == idOT).ToList();
            List<materialesUtilizadosOT> materialesUtilizadosOT = db.materialesUtilizadosOTs.Where(s=> s.ordenDeTrabajoGeneralID==idOT).ToList();

            ViewData["ejecutantesTrabajoOT"] = ejecutantesTrabajoOT;
            ViewData["materialesRequeridosOT"] = materialesRequeridosOT;
            ViewData["materialesUtilizadosOT"] = materialesUtilizadosOT;
            ViewData["equipos"] = equipos.todosConTipo();
            ViewData["materiales"] = db.Productos.ToList();

            return View(ordenDeTrabajoGeneral);
        }

        // POST: ordenDeTrabajoGeneral/Edit/5        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ordenDeTrabajoGeneralID,fechaOTAbierta,fechaOTCerrada,operador,faena,turno,idEquipo,horometro,kilometraje,tipoMantenimientoARealizar,horasMantenimientoNivelCombustible,horasMantenimientoFecha,horasMantenimientoHRInicio,horasMantenimientoHRTermino,horasMantenimientoHRSDetenido,trabajoRealizar,conclusionesTrabajoRealizado,estadoEquipo,trabajosPendientesPorRealizar,fechaTrabajosPendientesPorRealizar,numeroFolio,area,nombreMantenedor,nombreOperador,nombreSupervisor,tipoOTSegunMantenimiento, IDOTAnterior, verificarTrabajoPendiente")] ordenDeTrabajoGeneral ordenDeTrabajoGeneral, 
            FormCollection form, HttpPostedFileBase file)
        {
            if (Session["ID"] == null || !roles.tienePermiso(numeroPermiso, int.Parse(Session["ID"].ToString())))
            {
                return RedirectToAction("Index", "Home");
            }
            int idOT = ordenDeTrabajoGeneral.ordenDeTrabajoGeneralID;

            var ejecutantesTrabajo = db.ejecutanteTrabajoOTs.Where(s => s.ordenDeTrabajoGeneralID == idOT).ToList();
            var materialesRequeridos = db.materialesRequeridosOTs.Where(s => s.ordenDeTrabajoGeneralID == idOT).ToList();
            var materialesUtilizados = db.materialesUtilizadosOTs.Where(s => s.ordenDeTrabajoGeneralID == idOT).ToList();
            pedidos pedido = db.pedidos.SingleOrDefault(s => s.idOT==idOT);
            var detallesPedidos = db.detallePedidos.Where(s => s.pedidosID == pedido.pedidosID);


            ordenDeTrabajoGeneral.fechaOTAbierta = Formateador.fechaFormatoGuardar((string)form["fechaOTAbierta"]);
            ordenDeTrabajoGeneral.fechaOTCerrada = Formateador.fechaFormatoGuardar((string)form["fechaOTCerrada"]);
            ordenDeTrabajoGeneral.horasMantenimientoFecha = Formateador.fechaFormatoGuardar((string)form["horasMantenimientoFecha"]);
            ordenDeTrabajoGeneral.fechaTrabajosPendientesPorRealizar = Formateador.fechaFormatoGuardar((string)form["fechaTrabajosPendientesPorRealizar"]);
            ordenDeTrabajoGeneral.horasMantenimientoHRSDetenido = double.Parse((string)form["horasMantenimientoHRSDetenido"].Replace(',', '.'));
               
                

            if (!Object.ReferenceEquals(null, pedido))
            {
                foreach (var detallePed in detallesPedidos)
                {
                    db.detallePedidos.Remove(detallePed);
                }
            }
            foreach (var ejecutante in ejecutantesTrabajo)
            {
                db.ejecutanteTrabajoOTs.Remove(ejecutante);
            }

            foreach (var matReq in materialesRequeridos)
            {
                db.materialesRequeridosOTs.Remove(matReq);
            }

            foreach (var matUT in materialesUtilizados)
            {
                db.materialesUtilizadosOTs.Remove(matUT);
            }


            string[] ejecutantesDelTrabajo = Request.Form.GetValues("ejecutanteDelTrabajo");
            string[] cargo = Request.Form.GetValues("cargo");
            string[] HH = Request.Form.GetValues("HH");
            for (int i = 0; i < ejecutantesDelTrabajo.Length; i++)
            {
                ejecutanteTrabajoOT ejecutanteTrabajoOT = new ejecutanteTrabajoOT();
                ejecutanteTrabajoOT.ordenDeTrabajoGeneralID = idOT;
                ejecutanteTrabajoOT.nombreTrabajador = ejecutantesDelTrabajo[i];
                ejecutanteTrabajoOT.cargo = cargo[i];
                if (HH[i].Equals(""))
                {
                    ejecutanteTrabajoOT.HH = 0;
                }
                else
                {
                    ejecutanteTrabajoOT.HH = Convert.ToDouble(HH[i]);
                } 
                
                db.ejecutanteTrabajoOTs.Add(ejecutanteTrabajoOT);
            }


            string[] materialUtilizado = Request.Form.GetValues("materialUtilizado");
            string[] matUtcantidad = Request.Form.GetValues("matUtcantidad");
            string[] matUtNumeroParte = Request.Form.GetValues("matUtNumeroParte");


            var maestrosExistentes = db.Maestros.Where(s => s.idOT == idOT).ToList();
            foreach (Maestro maestroExist in maestrosExistentes) {
                bool encontrado = false;
                for (int i = 0; i < materialUtilizado.Length; i++)
                {
                    if (!materialUtilizado[i].Equals(""))
                    {
                        materialesUtilizadosOT materialesUtilizadosOT = new materialesUtilizadosOT();
                        materialesUtilizadosOT.ordenDeTrabajoGeneralID = idOT;
                        materialesUtilizadosOT.nombreMaterial = materialUtilizado[i];
                        materialesUtilizadosOT.cantidad = Convert.ToDouble(matUtcantidad[i]);
                        materialesUtilizadosOT.materialID = Convert.ToInt32(matUtNumeroParte[i]);
                        materialesUtilizadosOT.precioActual = db.Productos.Find(Convert.ToInt32(matUtNumeroParte[i])).precioUnitario;


                        if (int.Parse(maestroExist.ProductoID) == Convert.ToInt32(matUtNumeroParte[i]))
                        {
                            encontrado = true;
                            double diferencia = maestroExist.cantidadSaliente - materialesUtilizadosOT.cantidad;
                            if (diferencia != 0)
                            {
                                maestroExist.cantidadSaliente = materialesUtilizadosOT.cantidad;
                                Producto producto = db.Productos.Find(materialesUtilizadosOT.materialID);
                                producto.stockActual = producto.stockActual + diferencia;
                                db.Entry(producto).State = EntityState.Modified;
                            }


                        }
                    }

                }
                if (encontrado == false) {
                    Producto producto = db.Productos.Find(Convert.ToInt32(maestroExist.ProductoID));
                    producto.stockActual = producto.stockActual + maestroExist.cantidadSaliente;
                    db.Entry(producto).State = EntityState.Modified;
                    db.Maestros.Remove(maestroExist);
                    
                }
            }

            if (!materialUtilizado[0].Equals(""))
            {
                for (int i = 0; i < materialUtilizado.Length; i++)
                {
                    materialesUtilizadosOT materialesUtilizadosOT = new materialesUtilizadosOT();
                    materialesUtilizadosOT.ordenDeTrabajoGeneralID = idOT;
                    materialesUtilizadosOT.nombreMaterial = materialUtilizado[i];
                    materialesUtilizadosOT.cantidad = Convert.ToDouble(matUtcantidad[i]);
                    materialesUtilizadosOT.materialID = Convert.ToInt32(matUtNumeroParte[i]);
                    materialesUtilizadosOT.precioActual = db.Productos.Find(Convert.ToInt32(matUtNumeroParte[i])).precioUnitario;
                    db.materialesUtilizadosOTs.Add(materialesUtilizadosOT);
                    bool encontrado=false;
                    foreach (Maestro maestroExist in maestrosExistentes) {
                        if (int.Parse(maestroExist.ProductoID) == materialesUtilizadosOT.materialID) {
                            encontrado = true;
                        }
                    }
                    if(encontrado==false){
                        Maestro maestro = new Maestro();
                        maestro.afiEquipo = db.Equipos.Find(int.Parse(ordenDeTrabajoGeneral.idEquipo)).numeroAFI;
                        maestro.fecha = ordenDeTrabajoGeneral.horasMantenimientoFecha;
                        maestro.descripcionProducto = materialUtilizado[i];
                        maestro.cantidadEntrante = 0;
                        maestro.cantidadSaliente = Convert.ToDouble(matUtcantidad[i]);
                        maestro.idOT = idOT;
                        maestro.ProductoID = matUtNumeroParte[i];
                        maestro.observaciones="Agregada Automaticamente de OT:"+ordenDeTrabajoGeneral.numeroFolio;

                        Producto prod = db.Productos.Find(Convert.ToInt32(maestro.ProductoID));
                        prod.stockActual = prod.stockActual - maestro.cantidadSaliente;
                        db.Entry(prod).State = EntityState.Modified;
                        db.Maestros.Add(maestro);
                        db.SaveChanges();
                    }
                    


                }
            }
            //save

            string[] materialRequerido = Request.Form.GetValues("materialRequerido");
            string[] matReqCantidad = Request.Form.GetValues("matReqCantidad");
            string[] matReqNumeroParte = Request.Form.GetValues("matReqNumeroParte");
            if (!materialRequerido[0].Equals(""))
            {
                if (Object.ReferenceEquals(null, pedido))
                {
                    pedido = new pedidos();                    
                    pedido.fecha = Formateador.formatearFechaCompleta(DateTime.Now);
                    pedido.estado = "NUEVA";
                    pedido.nota = "Agregado Automaticamente desde OT:" + ordenDeTrabajoGeneral.numeroFolio;
                    pedido.idOT = idOT;
                    db.pedidos.Add(pedido);
                    db.SaveChanges();
                    
                }
                for (int i = 0; i < materialRequerido.Length; i++)
                {
                    materialesRequeridosOT materialesRequeridosOT = new materialesRequeridosOT();
                    materialesRequeridosOT.ordenDeTrabajoGeneralID = idOT;
                    materialesRequeridosOT.nombreMaterial = materialRequerido[i];
                    materialesRequeridosOT.cantidad = Convert.ToDouble(matReqCantidad[i]);
                    materialesRequeridosOT.materialID = Convert.ToInt32(matReqNumeroParte[i]);
                    materialesRequeridosOT.precioActual = db.Productos.Find(Convert.ToInt32(matReqNumeroParte[i])).precioUnitario;
                    db.materialesRequeridosOTs.Add(materialesRequeridosOT);


                    detallePedido detallePedido = new detallePedido();
                    detallePedido.cantidad = Convert.ToDouble(materialesRequeridosOT.cantidad);
                    detallePedido.descripcion = materialesRequeridosOT.nombreMaterial;
                    detallePedido.numeroParte = db.Productos.Find(materialesRequeridosOT.materialID).numeroDeParte;
                    detallePedido.pedidosID = pedido.pedidosID;
                    detallePedido.tipoPedido = "DIRECTA";
                    detallePedido.detalleTipoPedido = db.Equipos.Find(Convert.ToInt32(ordenDeTrabajoGeneral.idEquipo)).numeroAFI.ToString();
                    db.detallePedidos.Add(detallePedido);
                }
            }
            //if (ModelState.IsValid)
            //{
            equipos equipo = equipos.ObtenerConTipo(Convert.ToInt32(ordenDeTrabajoGeneral.idEquipo));
            ordenDeTrabajoGeneral.patenteEquipo = equipo.patenteEquipo;
            ordenDeTrabajoGeneral.tipoEquipo = equipo.tipoEquipo;                  
            
            db.Entry(ordenDeTrabajoGeneral).State = EntityState.Modified;
               

                crearCarpetaSiNoExiste();
                string extImage = Convert.ToString(Request.Files["file"].ContentType);
                string[] infoImage = extImage.Split('/');
                string fileExtension = System.IO.Path.GetExtension(Request.Files["file"].FileName);
                string fileLocation = Server.MapPath("~/Images/OrdenTrabajo/") + ordenDeTrabajoGeneral.numeroFolio + "." + infoImage[1];
                if (!fileExtension.Equals(""))
                {
                    Request.Files["file"].SaveAs(fileLocation);
                    ordenDeTrabajoGeneral.rutaImagen = "Images/OrdenTrabajo/" + ordenDeTrabajoGeneral.numeroFolio + "." + infoImage[1];

                }
                registrokmhm registro = new registrokmhm();
                registro.equipoID = Convert.ToInt32(ordenDeTrabajoGeneral.idEquipo);
                registro.fecha = Formateador.fechaFormatoGuardar((string)form["fechaOTAbierta"]);
                registro.horometro = ordenDeTrabajoGeneral.horometro;
                registro.kilometraje = ordenDeTrabajoGeneral.kilometraje;
                //db.registrokmhms.Add(registro);
                registrokmhm.actualizarRegistroKmHm(registro);


                mantencionPreventiva mantecionPreventiva = new mantencionPreventiva();
                mantecionPreventiva.equipoID = Convert.ToInt32(ordenDeTrabajoGeneral.idEquipo);
                mantecionPreventiva.fecha = ordenDeTrabajoGeneral.horasMantenimientoFecha;
                mantecionPreventiva.horometroActual = ordenDeTrabajoGeneral.horometro;
                mantecionPreventiva.kilometrajeActual = ordenDeTrabajoGeneral.kilometraje;

                if (ordenDeTrabajoGeneral.horometro == 0)
                {
                    mantecionPreventiva.horometroProximaMantencion = 0;
                }
                else
                {
                    mantecionPreventiva.horometroProximaMantencion = ordenDeTrabajoGeneral.horometro + 400;
                }

                if (ordenDeTrabajoGeneral.kilometraje == 0)
                {
                    mantecionPreventiva.kilometrajeProximaMantencion = 0;
                }
                else
                {
                    mantecionPreventiva.kilometrajeProximaMantencion = ordenDeTrabajoGeneral.kilometraje + 10000;
                }

                mantecionPreventiva.nota = "Agregado Automaticamente desde OT N°:" + ordenDeTrabajoGeneral.numeroFolio;
                RentaMaq.Models.mantencionPreventiva.reemplazar(mantecionPreventiva);
                db.SaveChanges();
                return RedirectToAction("Index");          
        }

        // GET: ordenDeTrabajoGeneral/Delete/5
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
            ordenDeTrabajoGeneral ordenDeTrabajoGeneral = db.ordenDeTrabajoGenerals.Find(id);
            if (ordenDeTrabajoGeneral == null)
            {
                return HttpNotFound();
            }

            int idOT = Convert.ToInt32(id);
            List<ejecutanteTrabajoOT> ejecutantesTrabajoOT = db.ejecutanteTrabajoOTs.Where(s => s.ordenDeTrabajoGeneralID == idOT).ToList();
            List<materialesRequeridosOT> materialesRequeridosOT = db.materialesRequeridosOTs.Where(s => s.ordenDeTrabajoGeneralID == idOT).ToList();
            List<materialesUtilizadosOT> materialesUtilizadosOT = db.materialesUtilizadosOTs.Where(s => s.ordenDeTrabajoGeneralID == idOT).ToList();

            ViewData["ejecutantesTrabajoOT"] = ejecutantesTrabajoOT;
            ViewData["materialesRequeridosOT"] = materialesRequeridosOT;
            ViewData["materialesUtilizadosOT"] = materialesUtilizadosOT;
            return View(ordenDeTrabajoGeneral);
        }

        // POST: ordenDeTrabajoGeneral/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["ID"] == null || !roles.tienePermiso(numeroPermiso, int.Parse(Session["ID"].ToString())))
            {
                return RedirectToAction("Index", "Home");
            }
            ordenDeTrabajoGeneral ordenDeTrabajoGeneral = db.ordenDeTrabajoGenerals.Find(id);
            db.ordenDeTrabajoGenerals.Remove(ordenDeTrabajoGeneral);            

            var ejecutantesTrabajo = db.ejecutanteTrabajoOTs.Where(s => s.ordenDeTrabajoGeneralID == id).ToList();
            var materialesRequeridos = db.materialesRequeridosOTs.Where(s => s.ordenDeTrabajoGeneralID == id).ToList();
            var materialesUtilizados = db.materialesUtilizadosOTs.Where(s => s.ordenDeTrabajoGeneralID == id).ToList();

            foreach (var ejecutante in ejecutantesTrabajo)
            {
                db.ejecutanteTrabajoOTs.Remove(ejecutante);
            }

            foreach (var matReq in materialesRequeridos)
            {
                db.materialesRequeridosOTs.Remove(matReq);
            }

            foreach (var matUT in materialesUtilizados)
            {
                db.materialesUtilizadosOTs.Remove(matUT);
            }


            if (!Object.ReferenceEquals(ordenDeTrabajoGeneral.rutaImagen,null)) {
                string rutaImagen = Server.MapPath("~/") + ordenDeTrabajoGeneral.rutaImagen;
                System.IO.File.Delete(rutaImagen);
            }
            
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

        private void crearCarpetaSiNoExiste(){
            var path = Server.MapPath("~/Images/OrdenTrabajo");
            if (!Directory.Exists(path))
            {
                DirectoryInfo ruta = Directory.CreateDirectory(path);
            }

        }

        public JsonResult Filtrar(string numeroFolio)
        {
            //int numFol = Convert.ToInt32(numeroFolio);
            using (var db = new Context())
            {

                var L2EQuery = db.ordenDeTrabajoGenerals.Where(s => s.numeroFolio == numeroFolio).Select(s => new { s.ordenDeTrabajoGeneralID, s.numeroFolio, s.fechaOTAbierta, s.fechaOTCerrada, s.patenteEquipo, s.tipoEquipo, s.fechaTrabajosPendientesPorRealizar });
                var solicitud = L2EQuery.ToList();
                return Json(solicitud, JsonRequestBehavior.AllowGet);

            }

        }

        public ActionResult trabajosPendientes() {
            return View(ordenDeTrabajoGeneral.obtenerOTconTrabajosPendientes());
        }

        public ActionResult marcarComoRealizado(int id) {
            if (Session["ID"] == null || !roles.tienePermiso(numeroPermiso, int.Parse(Session["ID"].ToString())))
            {
                return RedirectToAction("Index", "Home");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ordenDeTrabajoGeneral ordenDeTrabajoGeneral = db.ordenDeTrabajoGenerals.Find(id);
            if (ordenDeTrabajoGeneral == null)
            {
                return HttpNotFound();
            }
            equipos equipo = equipos.ObtenerConTipo(Convert.ToInt32(ordenDeTrabajoGeneral.idEquipo));
            ViewBag.Equipo=equipo.patenteEquipo+"/"+equipo.tipoEquipo +"/"+equipo.numeroAFI;
            return View(ordenDeTrabajoGeneral);
        
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult marcarComoRealizado(FormCollection form)
        {
            if (Session["ID"] == null || !roles.tienePermiso(numeroPermiso, int.Parse(Session["ID"].ToString())))
            {
                return RedirectToAction("Index", "Home");
            }
            int id = Convert.ToInt32((string)form["ordenDeTrabajoGeneralID"]);
            ordenDeTrabajoGeneral ordenDeTrabajoGeneral = db.ordenDeTrabajoGenerals.Find(id);
            ordenDeTrabajoGeneral.verificarTrabajoPendiente = "TRUE";
            db.Entry(ordenDeTrabajoGeneral).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("trabajosPendientes");
        }

        public ActionResult Costos(string inicio, string termino)
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

            List<ordenDeTrabajoGeneral> listaOrdenes =
                db.ordenDeTrabajoGenerals.Where(s => s.horasMantenimientoFecha >= Inicio && s.horasMantenimientoFecha <= fin).ToList();
            List<IndicadoresCostosActivosOT> lista = new List<IndicadoresCostosActivosOT>();

            foreach (ordenDeTrabajoGeneral OT in listaOrdenes) 
            {
                lista.Add(new IndicadoresCostosActivosOT(OT));
            }

            return View(lista);
        }

        public ActionResult CostosDetalle(int id) 
        {
            return View(new IndicadoresCostosActivosOT(id));
        }

        public ActionResult CostosPorEquipo()
        {
            List<equipos> Equipos = new Context().Equipos.ToList();

            List<IndicadoresCostosOTEquipo> lista = new List<IndicadoresCostosOTEquipo>();
            foreach (equipos Equipo in Equipos)
            {
                lista.Add(new IndicadoresCostosOTEquipo(Equipo.ID));
            }

            return View(lista);
        }

        public ActionResult DetalleCostosPorEquipo(int id)
        {
            string ID =id.ToString();
            List<ordenDeTrabajoGeneral> listaOrdenes =
                db.ordenDeTrabajoGenerals.Where(s => s.idEquipo == ID).ToList();
            List<IndicadoresCostosActivosOT> lista = new List<IndicadoresCostosActivosOT>();

            foreach (ordenDeTrabajoGeneral OT in listaOrdenes)
            {
                lista.Add(new IndicadoresCostosActivosOT(OT));
            }

            return View(lista);
        }

        public ActionResult generarOTTrabajoPendiente(int id) {
            if (Session["ID"] == null || !roles.tienePermiso(numeroPermiso, int.Parse(Session["ID"].ToString())))
            {
                return RedirectToAction("Index", "Home");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ordenDeTrabajoGeneral ordenDeTrabajoGeneral = db.ordenDeTrabajoGenerals.Find(id);
            if (ordenDeTrabajoGeneral == null)
            {
                return HttpNotFound();
            }

            ViewData["equipos"] = equipos.todosConTipo();
            ViewData["materiales"] = db.Productos.ToList();
            ViewData["OTAnterior"] = ordenDeTrabajoGeneral;
            return View(new ordenDeTrabajoGeneral());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult generarOTTrabajoPendiente([Bind(Include = "ordenDeTrabajoGeneralID,fechaOTAbierta,fechaOTCerrada,operador,faena,turno,idEquipo,horometro,kilometraje,tipoMantenimientoARealizar,horasMantenimientoNivelCombustible,horasMantenimientoFecha,horasMantenimientoHRInicio,horasMantenimientoHRTermino,horasMantenimientoHRSDetenido,trabajoRealizar,conclusionesTrabajoRealizado,estadoEquipo,trabajosPendientesPorRealizar,fechaTrabajosPendientesPorRealizar,numeroFolio,area,nombreMantenedor,nombreOperador,nombreSupervisor,tipoOTSegunMantenimiento, IDOTAnterior")] ordenDeTrabajoGeneral ordenDeTrabajoGeneral, 
            FormCollection form, HttpPostedFileBase file)
        {
            if (Session["ID"] == null || !roles.tienePermiso(numeroPermiso, int.Parse(Session["ID"].ToString())))
            {
                return RedirectToAction("Index", "Home");
            }
            ordenDeTrabajoGeneral.fechaOTAbierta = Formateador.fechaFormatoGuardar((string)form["fechaOTAbierta"]);
            ordenDeTrabajoGeneral.fechaOTCerrada = Formateador.fechaFormatoGuardar((string)form["fechaOTCerrada"]);
            ordenDeTrabajoGeneral.horasMantenimientoFecha = Formateador.fechaFormatoGuardar((string)form["horasMantenimientoFecha"]);
            ordenDeTrabajoGeneral.fechaTrabajosPendientesPorRealizar = Formateador.fechaFormatoGuardar((string)form["fechaTrabajosPendientesPorRealizar"]);

            equipos equipo = equipos.ObtenerConTipo(Convert.ToInt32(ordenDeTrabajoGeneral.idEquipo));
            ordenDeTrabajoGeneral.patenteEquipo = equipo.patenteEquipo;
            ordenDeTrabajoGeneral.tipoEquipo = equipo.tipoEquipo;
            ordenDeTrabajoGeneral.verificarTrabajoPendiente = "FALSE";

            ordenDeTrabajoGeneral anterior = db.ordenDeTrabajoGenerals.Find(ordenDeTrabajoGeneral.IDOTAnterior);
            anterior.verificarTrabajoPendiente = "TRUE";

            db.Entry(anterior).State = EntityState.Modified;

            db.ordenDeTrabajoGenerals.Add(ordenDeTrabajoGeneral);
            db.SaveChanges();

            int idOT = ordenDeTrabajoGeneral.ordenDeTrabajoGeneralID;
            string[] ejecutantesDelTrabajo = Request.Form.GetValues("ejecutanteDelTrabajo");
            string[] cargo = Request.Form.GetValues("cargo");
            string[] HH = Request.Form.GetValues("HH");
            for (int i = 0; i < ejecutantesDelTrabajo.Length; i++)
            {
                ejecutanteTrabajoOT ejecutanteTrabajoOT = new ejecutanteTrabajoOT();
                ejecutanteTrabajoOT.ordenDeTrabajoGeneralID = idOT;
                ejecutanteTrabajoOT.nombreTrabajador = ejecutantesDelTrabajo[i];
                ejecutanteTrabajoOT.cargo = cargo[i];
                if (HH[i].Equals(""))
                {
                    ejecutanteTrabajoOT.HH = 0;
                }
                else
                {
                    ejecutanteTrabajoOT.HH = Convert.ToInt32(HH[i]);
                }

                db.ejecutanteTrabajoOTs.Add(ejecutanteTrabajoOT);
            }







            string[] materialUtilizado = Request.Form.GetValues("materialUtilizado");
            string[] matUtcantidad = Request.Form.GetValues("matUtcantidad");
            string[] matUtNumeroParte = Request.Form.GetValues("matUtNumeroParte");
            if (!materialUtilizado[0].Equals(""))
            {


                for (int i = 0; i < materialUtilizado.Length; i++)
                {
                    materialesUtilizadosOT materialesUtilizadosOT = new materialesUtilizadosOT();
                    materialesUtilizadosOT.ordenDeTrabajoGeneralID = idOT;
                    materialesUtilizadosOT.nombreMaterial = materialUtilizado[i];
                    materialesUtilizadosOT.cantidad = Convert.ToDouble(matUtcantidad[i]);
                    materialesUtilizadosOT.materialID = Convert.ToInt32(matUtNumeroParte[i]);
                    materialesUtilizadosOT.precioActual = db.Productos.Find(Convert.ToInt32(matUtNumeroParte[i])).precioUnitario;
                    db.materialesUtilizadosOTs.Add(materialesUtilizadosOT);


                    Maestro maestro = new Maestro();
                    maestro.afiEquipo = equipo.numeroAFI;
                    maestro.fecha = ordenDeTrabajoGeneral.horasMantenimientoFecha;
                    maestro.descripcionProducto = materialUtilizado[i];
                    maestro.cantidadEntrante = 0;
                    maestro.cantidadSaliente = Convert.ToDouble(matUtcantidad[i]);
                    maestro.idOT = idOT;
                    maestro.ProductoID = matUtNumeroParte[i];
                    maestro.observaciones = "Agregada Automaticamente de OT:" + ordenDeTrabajoGeneral.numeroFolio;

                    Producto producto = db.Productos.Find(int.Parse(maestro.ProductoID));
                    producto.stockActual = producto.stockActual - maestro.cantidadSaliente;
                    db.Entry(producto).State = EntityState.Modified;
                    db.Maestros.Add(maestro);

                }
            }
            //save

            string[] materialRequerido = Request.Form.GetValues("materialRequerido");
            string[] matReqCantidad = Request.Form.GetValues("matReqCantidad");
            string[] matReqNumeroParte = Request.Form.GetValues("matReqNumeroParte");



            if (!materialRequerido[0].Equals(""))
            {
                pedidos pedido = new pedidos();
                pedido.fecha = Formateador.formatearFechaCompleta(DateTime.Now);
                pedido.estado = "NUEVA";
                pedido.nota = "Agregado Automaticamente desde OT:" + ordenDeTrabajoGeneral.numeroFolio;
                pedido.idOT = idOT;
                db.pedidos.Add(pedido);
                db.SaveChanges();

                for (int i = 0; i < materialRequerido.Length; i++)
                {
                    materialesRequeridosOT materialesRequeridosOT = new materialesRequeridosOT();
                    materialesRequeridosOT.ordenDeTrabajoGeneralID = idOT;
                    materialesRequeridosOT.nombreMaterial = materialRequerido[i];
                    materialesRequeridosOT.cantidad = Convert.ToDouble(matReqCantidad[i]);
                    materialesRequeridosOT.materialID = Convert.ToInt32(matReqNumeroParte[i]);
                    materialesRequeridosOT.precioActual = db.Productos.Find(Convert.ToInt32(matReqNumeroParte[i])).precioUnitario;
                    db.materialesRequeridosOTs.Add(materialesRequeridosOT);

                    detallePedido detallePedido = new detallePedido();
                    detallePedido.cantidad = Convert.ToInt32(materialesRequeridosOT.cantidad);
                    detallePedido.descripcion = materialesRequeridosOT.nombreMaterial;
                    detallePedido.numeroParte = db.Productos.Find(materialesRequeridosOT.materialID).numeroDeParte;
                    detallePedido.pedidosID = pedido.pedidosID;
                    detallePedido.tipoPedido = "DIRECTA";
                    detallePedido.detalleTipoPedido = db.Equipos.Find(Convert.ToInt32(ordenDeTrabajoGeneral.idEquipo)).numeroAFI.ToString();
                    db.detallePedidos.Add(detallePedido);

                }
            }




            crearCarpetaSiNoExiste();
            string extImage = Convert.ToString(Request.Files["file"].ContentType);
            string[] infoImage = extImage.Split('/');
            string fileExtension = System.IO.Path.GetExtension(Request.Files["file"].FileName);
            string fileLocation = Server.MapPath("~/Images/OrdenTrabajo/") + ordenDeTrabajoGeneral.numeroFolio + "." + infoImage[1];
            if (!fileExtension.Equals(""))
            {
                Request.Files["file"].SaveAs(fileLocation);
                ordenDeTrabajoGeneral.rutaImagen = "Images/OrdenTrabajo/" + ordenDeTrabajoGeneral.numeroFolio + "." + infoImage[1];
            }

            registrokmhm registro = new registrokmhm();
            registro.equipoID = Convert.ToInt32(ordenDeTrabajoGeneral.idEquipo);
            registro.fecha = Formateador.fechaFormatoGuardar((string)form["fechaOTAbierta"]);
            registro.horometro = ordenDeTrabajoGeneral.horometro;
            registro.kilometraje = ordenDeTrabajoGeneral.kilometraje;
            //db.registrokmhms.Add(registro);
            registrokmhm.actualizarRegistroKmHm(registro);

            mantencionPreventiva mantecionPreventiva = new mantencionPreventiva();
            mantecionPreventiva.equipoID = Convert.ToInt32(ordenDeTrabajoGeneral.idEquipo);
            mantecionPreventiva.fecha = ordenDeTrabajoGeneral.horasMantenimientoFecha;
            mantecionPreventiva.horometroActual = ordenDeTrabajoGeneral.horometro;
            mantecionPreventiva.kilometrajeActual = ordenDeTrabajoGeneral.kilometraje;

            if (ordenDeTrabajoGeneral.horometro == 0)
            {
                mantecionPreventiva.horometroProximaMantencion = 0;
            }
            else
            {
                mantecionPreventiva.horometroProximaMantencion = ordenDeTrabajoGeneral.horometro + 400;
            }

            if (ordenDeTrabajoGeneral.kilometraje == 0)
            {
                mantecionPreventiva.kilometrajeProximaMantencion = 0;
            }
            else
            {
                mantecionPreventiva.kilometrajeProximaMantencion = ordenDeTrabajoGeneral.kilometraje + 10000;
            }

            mantecionPreventiva.nota = "Agregado Automaticamente desde OT N°:" + ordenDeTrabajoGeneral.numeroFolio;
            RentaMaq.Models.mantencionPreventiva.reemplazar(mantecionPreventiva);



            db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
