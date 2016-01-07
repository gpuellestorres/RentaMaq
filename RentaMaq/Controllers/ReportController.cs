using Microsoft.Reporting.WebForms;
using RentaMaq.DAL;
using RentaMaq.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RentaMaq.Controllers
{
    public class ReportController : Controller
    {
        // GET: Report
        public ActionResult Index()
        {
            return View();
        }

        public FileContentResult Existencias(string inicio, string termino)
        {
            // Nota los datos creados en el dataset deben ser con el mismo nombre que tengan los Datos del Modelo
            LocalReport reporte_local = new LocalReport();
            // pasa la ruta donde se encuentra el reporte
            reporte_local.ReportPath = Server.MapPath("~/Report/Existencias.rdlc");
            // creamos un recurso de datos del tipo report
            ReportDataSource conjunto_datos = new ReportDataSource();
            // le asginamos al conjuto de datos el nombre del datasource del reporte
            conjunto_datos.Name = "DataSet1";
            List<reportExistenciasReporte> datos = new List<reportExistenciasReporte>();

            DateTime Inicio = DateTime.Now.AddMonths(-1);
            DateTime Termino = DateTime.Now;

            if (inicio != null || termino != null)
            {
                string[] inicioSeparado = inicio.Split('-');
                string[] terminoSeparado = termino.Split('-');

                Inicio = new DateTime(int.Parse(inicioSeparado[2]), int.Parse(inicioSeparado[1]), int.Parse(inicioSeparado[0]));
                Termino = new DateTime(int.Parse(terminoSeparado[2]), int.Parse(terminoSeparado[1]), int.Parse(terminoSeparado[0]));
            }

            List<reportExistenciasProductos> lista = new List<reportExistenciasProductos>();
            List<string> idsProductos = Maestro.ObtenerIdsProductosPeriodo(Inicio, Termino);

            foreach (string IDProducto in idsProductos)
            {
                lista.Add(new reportExistenciasProductos(new RentaMaq.DAL.Context().Productos.Find(int.Parse(IDProducto)), Inicio, Termino));
            }

            foreach (reportExistenciasProductos Rep in lista) 
            {
                datos.Add(new reportExistenciasReporte(Rep));
            }
                
            // se le asigna el datasource el conjunto de datos desde el modelo
            conjunto_datos.Value = datos;
            // se agrega el conjunto de datos del tipo report al reporte local
            reporte_local.DataSources.Add(conjunto_datos);
            // datos para renderizar como se mostrara el reporte
            string reportType = "PDF";
            string mimeType;
            string encoding;
            string fileNameExtension;
            string deviceInfo = "<DeviceInfo>" +
                 "  <OutputFormat>jpeg</OutputFormat>" +
                 "  <PageWidth>10in</PageWidth>" +
                 "  <PageHeight>13in</PageHeight>" +
                 "  <MarginTop>0.5in</MarginTop>" +
                 "  <MarginLeft>1in</MarginLeft>" +
                 "  <MarginRight>1in</MarginRight>" +
                 "  <MarginBottom>0.5in</MarginBottom>" +
                 "</DeviceInfo>";
            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes;
            //Se renderiza el reporte            
            renderedBytes = reporte_local.Render(reportType, deviceInfo, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);
            // el reporte es mostrado como una imagen
            return File(renderedBytes, mimeType);
        }

        public FileContentResult ExistenciasProducto(string idProducto, string inicio, string termino)
        {
            // Nota los datos creados en el dataset deben ser con el mismo nombre que tengan los Datos del Modelo
            LocalReport reporte_local = new LocalReport();
            // pasa la ruta donde se encuentra el reporte
            reporte_local.ReportPath = Server.MapPath("~/Report/DetalleExistencias.rdlc");
            // creamos un recurso de datos del tipo report
            ReportDataSource conjunto_datos = new ReportDataSource();
            // le asginamos al conjuto de datos el nombre del datasource del reporte
            conjunto_datos.Name = "DataSet1";

            DateTime Inicio = DateTime.Now.AddMonths(-1);
            DateTime Termino = DateTime.Now;
            RentaMaq.DAL.Context db = new RentaMaq.DAL.Context();

            if (inicio != null || termino != null)
            {
                string[] inicioSeparado = inicio.Replace('/', '-').Split('-');
                string[] terminoSeparado = termino.Replace('/', '-').Split('-');

                Inicio = new DateTime(int.Parse(inicioSeparado[2]), int.Parse(inicioSeparado[1]), int.Parse(inicioSeparado[0]));
                Termino = new DateTime(int.Parse(terminoSeparado[2]), int.Parse(terminoSeparado[1]), int.Parse(terminoSeparado[0]));
            }

            List<detalleReporteExistenciasProducto> lista = new List<detalleReporteExistenciasProducto>();

            Producto Producto = db.Productos.Find(int.Parse(idProducto));

            List<Maestro> listaMaestros = db.Maestros.Where(s => s.ProductoID == Producto.numeroDeParte && s.fecha <= Termino && s.fecha >= Inicio).OrderBy(s => s.fecha).ToList();

            foreach (Maestro Ms in listaMaestros)
            {
                lista.Add(new detalleReporteExistenciasProducto(Ms, Inicio, Termino));
            }

            // se le asigna el datasource el conjunto de datos desde el modelo
            conjunto_datos.Value = lista;
            // se agrega el conjunto de datos del tipo report al reporte local
            reporte_local.DataSources.Add(conjunto_datos);
            // datos para renderizar como se mostrara el reporte
            string reportType = "PDF";
            string mimeType;
            string encoding;
            string fileNameExtension;
            string deviceInfo = "<DeviceInfo>" +
                 "  <OutputFormat>jpeg</OutputFormat>" +
                 "  <PageWidth>10.3in</PageWidth>" +
                 "  <PageHeight>13in</PageHeight>" +
                 "  <MarginTop>0.5in</MarginTop>" +
                 "  <MarginLeft>1in</MarginLeft>" +
                 "  <MarginRight>1in</MarginRight>" +
                 "  <MarginBottom>0.5in</MarginBottom>" +
                 "</DeviceInfo>";
            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes;
            //Se renderiza el reporte            
            renderedBytes = reporte_local.Render(reportType, deviceInfo, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);
            // el reporte es mostrado como una imagen
            return File(renderedBytes, mimeType);
        }

        public FileContentResult stockCritico()
        {
           
            LocalReport reporte_local = new LocalReport();
          
            reporte_local.ReportPath = Server.MapPath("~/Report/reporteStockCritico.rdlc");            
            ReportDataSource conjunto_datos = new ReportDataSource();       
            conjunto_datos.Name = "DataSet1";

            List<ProductoReport> datos = new List<ProductoReport>();
            List<Producto> listaProductosStockCritico = Producto.listaProductosStockCritico();
            foreach (Producto Prod in listaProductosStockCritico)
            {
                datos.Add(new ProductoReport(Prod));
                    
            }
                        
            conjunto_datos.Value = datos;            
            reporte_local.DataSources.Add(conjunto_datos);            
            string reportType = "PDF";
            string mimeType;
            string encoding;
            string fileNameExtension;
            string deviceInfo = "<DeviceInfo>" +
                 "  <OutputFormat>jpeg</OutputFormat>" +
                 "  <PageWidth>10.3in</PageWidth>" +
                 "  <PageHeight>13in</PageHeight>" +
                 "  <MarginTop>0.5in</MarginTop>" +
                 "  <MarginLeft>1in</MarginLeft>" +
                 "  <MarginRight>1in</MarginRight>" +
                 "  <MarginBottom>1in</MarginBottom>" +
                 "</DeviceInfo>";
            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes;
                      
            renderedBytes = reporte_local.Render(reportType, deviceInfo, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);        
            return File(renderedBytes, mimeType);
        }

        public FileContentResult solicitudDeCotizacion(int id)
        {

            LocalReport reporte_local = new LocalReport();
            RentaMaq.DAL.Context db = new RentaMaq.DAL.Context();
            reporte_local.ReportPath = Server.MapPath("~/Report/reporteSolicitudDeCotizacion.rdlc");
            ReportDataSource conjunto_datos = new ReportDataSource();
            conjunto_datos.Name = "DataSet1";
            List<ReportSolicitudDeCotizacion> ReportSolicitudDeCotizacion = new List<ReportSolicitudDeCotizacion>();     

            solicitudDeCotizacion solicitudDeCotizacion = db.solicitudesDeCotizaciones.Find(id);
            List<detalleSolicitudDeCotizacion> detalleSolicitudDeCotizacion = new List<detalleSolicitudDeCotizacion>();
            
            var L2EQuery = db.detalleSolicitudDeCotizaciones.Where(s => s.solicitudDeCotizacionID == id);
            detalleSolicitudDeCotizacion = L2EQuery.ToList();


            foreach (detalleSolicitudDeCotizacion detalleSolicitud in detalleSolicitudDeCotizacion)
            {
                ReportSolicitudDeCotizacion.Add(new ReportSolicitudDeCotizacion(solicitudDeCotizacion, detalleSolicitud));

            }

            conjunto_datos.Value = ReportSolicitudDeCotizacion;
            reporte_local.DataSources.Add(conjunto_datos);
            string reportType = "PDF";
            string mimeType;
            string encoding;
            string fileNameExtension;
            string deviceInfo = "<DeviceInfo>" +
                 "  <OutputFormat>jpeg</OutputFormat>" +
                 "  <PageWidth>10.3in</PageWidth>" +
                 "  <PageHeight>13in</PageHeight>" +
                 "  <MarginTop>0.5in</MarginTop>" +
                 "  <MarginLeft>1in</MarginLeft>" +
                 "  <MarginRight>1in</MarginRight>" +
                 "  <MarginBottom>1in</MarginBottom>" +
                 "</DeviceInfo>";
            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes;

            renderedBytes = reporte_local.Render(reportType, deviceInfo, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);
            return File(renderedBytes, mimeType);
        }

        public FileContentResult ordenCompraArriendoEquipo(int id)
        {

            LocalReport reporte_local = new LocalReport();
            RentaMaq.DAL.Context db = new RentaMaq.DAL.Context();
            reporte_local.ReportPath = Server.MapPath("~/Report/reporteOrdenCompraArriendoEquipo.rdlc");
            ReportDataSource conjunto_datos = new ReportDataSource();
            conjunto_datos.Name = "DataSet1";
            List<ReportOrdenDeCompraArriendoEquipo> ReportOrdenDeCompraArriendoEquipo = new List<ReportOrdenDeCompraArriendoEquipo>();

            ordenDeCompraArriendoEquipo ordenDeCompraArriendoEquipo = db.ordenDeCompraArriendoEquipoes.Find(id);
            List<detalleOrdenDeCompraArriendoEquipo> detalleOrdenDeCompraArriendoEquipo = new List<detalleOrdenDeCompraArriendoEquipo>();

            var L2EQuery = db.detalleOrdenCompraArriendoEquipos.Where(s => s.ordenDeCompraArriendoEquipoID == id);
            detalleOrdenDeCompraArriendoEquipo = L2EQuery.ToList();


            foreach (detalleOrdenDeCompraArriendoEquipo detalleOrdeCompraArriendEquipo in detalleOrdenDeCompraArriendoEquipo)
            {
                ReportOrdenDeCompraArriendoEquipo.Add(new ReportOrdenDeCompraArriendoEquipo(ordenDeCompraArriendoEquipo, detalleOrdeCompraArriendEquipo));

            }

            conjunto_datos.Value = ReportOrdenDeCompraArriendoEquipo;
            reporte_local.DataSources.Add(conjunto_datos);
            string reportType = "PDF";
            string mimeType;
            string encoding;
            string fileNameExtension;
            string deviceInfo = "<DeviceInfo>" +
                 "  <OutputFormat>jpeg</OutputFormat>" +
                 "  <PageWidth>10.2in</PageWidth>" +
                 "  <PageHeight>13in</PageHeight>" +
                 "  <MarginTop>0.5in</MarginTop>" +
                 "  <MarginLeft>0.5in</MarginLeft>" +
                 "  <MarginRight>0.1in</MarginRight>" +
                 "  <MarginBottom>1in</MarginBottom>" +
                 "</DeviceInfo>";
            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes;

            renderedBytes = reporte_local.Render(reportType, deviceInfo, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);
            return File(renderedBytes, mimeType);
        }
        
        public FileContentResult cotizacionArriendoEquipo(int id)
        {

            LocalReport reporte_local = new LocalReport();
            RentaMaq.DAL.Context db = new RentaMaq.DAL.Context();
            reporte_local.ReportPath = Server.MapPath("~/Report/reporteCotizacionArriendoEquipo.rdlc");
            ReportDataSource conjunto_datos = new ReportDataSource();
            conjunto_datos.Name = "DataSet1";
            List<ReportCotizacionArriendoEquipo> ReportCotizacionArriendoEquipo = new List<ReportCotizacionArriendoEquipo>();

            cotizacionArriendoEquipo cotizacionArriendoEquipo = db.cotizacionArriendoEquipos.Find(id);
            List<detalleCotizacionArriendoEquipo> detalleCotizacionArriendoEquipo = new List<detalleCotizacionArriendoEquipo>();

            var L2EQuery = db.detalleCotizacionArriendoEquipo.Where(s => s.cotizacionArriendoEquipoID == id);
            detalleCotizacionArriendoEquipo = L2EQuery.ToList();


            foreach (detalleCotizacionArriendoEquipo detalle in detalleCotizacionArriendoEquipo)
            {
                ReportCotizacionArriendoEquipo.Add(new ReportCotizacionArriendoEquipo(cotizacionArriendoEquipo, detalle));

            }

            conjunto_datos.Value = ReportCotizacionArriendoEquipo;
            reporte_local.DataSources.Add(conjunto_datos);
            string reportType = "PDF";
            string mimeType;
            string encoding;
            string fileNameExtension;
            string deviceInfo = "<DeviceInfo>" +
                 "  <OutputFormat>jpeg</OutputFormat>" +
                 "  <PageWidth>10.3in</PageWidth>" +
                 "  <PageHeight>13in</PageHeight>" +
                 "  <MarginTop>0.5in</MarginTop>" +
                 "  <MarginLeft>0.5in</MarginLeft>" +
                 "  <MarginRight>0.1in</MarginRight>" +
                 "  <MarginBottom>1in</MarginBottom>" +
                 "</DeviceInfo>";
            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes;

            renderedBytes = reporte_local.Render(reportType, deviceInfo, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);
            return File(renderedBytes, mimeType);
        }

        public FileContentResult ordenPedidoCombustible(int id)
        {

            LocalReport reporte_local = new LocalReport();
            reporte_local.EnableExternalImages = true;
            RentaMaq.DAL.Context db = new RentaMaq.DAL.Context();
            reporte_local.ReportPath = Server.MapPath("~/Report/reporteOrdenPedidoCombustible.rdlc");
            ReportDataSource conjunto_datos = new ReportDataSource();
            conjunto_datos.Name = "DataSet1";
            List<ReporteOrdenPedidoCombustible> ReporteOrdenPedidoCombustible = new List<ReporteOrdenPedidoCombustible>();

            ordenPedidoCombustible ordenPedidoCombustible = db.ordenesPedidoCombustible.Find(id);
            List<detalleOrdenPedidoCombustible> detalleOrdenPedidoCombustible = new List<detalleOrdenPedidoCombustible>();

            var L2EQuery = db.detalleOrdenesPedidosCombustible.Where(s => s.ordenPedidoCombustibleID == id);
            detalleOrdenPedidoCombustible = L2EQuery.ToList();

            foreach (detalleOrdenPedidoCombustible detalle in detalleOrdenPedidoCombustible)
            {
                string rutaImagen = "";
                if (ordenPedidoCombustible.nombreQuienAutoriza.Equals("SRA. ORIETTA ARAYA PANGUE"))
                {
                    rutaImagen = new Uri(Server.MapPath("~/Images/firmaM.png")).AbsoluteUri;
                }
                else {
                    rutaImagen = new Uri(Server.MapPath("~/Images/firmaH.png")).AbsoluteUri;
                }
                ReporteOrdenPedidoCombustible.Add(new ReporteOrdenPedidoCombustible(ordenPedidoCombustible, detalle, rutaImagen));

            }

            conjunto_datos.Value = ReporteOrdenPedidoCombustible;
            reporte_local.DataSources.Add(conjunto_datos);
            string reportType = "PDF";
            string mimeType;
            string encoding;
            string fileNameExtension;
            string deviceInfo = "<DeviceInfo>" +
                 "  <OutputFormat>jpeg</OutputFormat>" +
                 "  <PageWidth>10.2in</PageWidth>" +
                 "  <PageHeight>13in</PageHeight>" +
                 "  <MarginTop>0.5in</MarginTop>" +
                 "  <MarginLeft>0.5in</MarginLeft>" +
                 "  <MarginRight>0.1in</MarginRight>" +
                 "  <MarginBottom>0in</MarginBottom>" +
                 "</DeviceInfo>";
            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes;

            renderedBytes = reporte_local.Render(reportType, deviceInfo, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);
            return File(renderedBytes, mimeType);
        }

        public FileContentResult OrdenCompraGeneral(int id)
        {

            LocalReport reporte_local = new LocalReport();
            RentaMaq.DAL.Context db = new RentaMaq.DAL.Context();
            reporte_local.ReportPath = Server.MapPath("~/Report/OrdenCompraGeneral.rdlc");
            ReportDataSource conjunto_datos = new ReportDataSource();
            conjunto_datos.Name = "DataSet1";

            OrdenDeCompraGeneral OC = OrdenDeCompraGeneral.obtener(id);

            List<OrdenDeCompraGeneralReporte> datos = OrdenDeCompraGeneralReporte.pasarADatosReporte(OC);

            if (datos.Count > 0 && OC.tipo.Equals("taltal")) 
            {
                reporte_local.ReportPath = Server.MapPath("~/Report/OrdenCompraGeneralTaltal.rdlc");
            }

            conjunto_datos.Value = datos;
            reporte_local.DataSources.Add(conjunto_datos);
            string reportType = "PDF";
            string mimeType;
            string encoding;
            string fileNameExtension;
            string deviceInfo = "<DeviceInfo>" +
                 "  <OutputFormat>jpeg</OutputFormat>" +
                 "  <PageWidth>8.5in</PageWidth>" +
                 "  <PageHeight>11in</PageHeight>" +
                 "  <MarginTop>0.5in</MarginTop>" +
                 "  <MarginLeft>0.5in</MarginLeft>" +
                 "  <MarginRight>0.5in</MarginRight>" +
                 "  <MarginBottom>0.5in</MarginBottom>" +
                 "</DeviceInfo>";
            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes;

            renderedBytes = reporte_local.Render(reportType, deviceInfo, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);
            return File(renderedBytes, mimeType);
        }

        public FileContentResult IndicadoresProveedor()
        {

            LocalReport reporte_local = new LocalReport();
            RentaMaq.DAL.Context db = new RentaMaq.DAL.Context();
            reporte_local.ReportPath = Server.MapPath("~/Report/IndicadoresClaveProveedores.rdlc");
            ReportDataSource conjunto_datos = new ReportDataSource();
            conjunto_datos.Name = "DataSet1";

            List<IndicadoresClaveProveedor> IndicadoresProveedor = IndicadoresClaveProveedor.obtenerTodos();

            List<IndicadoresProdProvReporte> datos =
                IndicadoresProdProvReporte.convertirIndicadoresProveedoresEnReporte(IndicadoresProveedor.OrderBy(s => s.tiempoMedioRespuesta).ToList());

            conjunto_datos.Value = datos;
            reporte_local.DataSources.Add(conjunto_datos);
            string reportType = "PDF";
            string mimeType;
            string encoding;
            string fileNameExtension;
            string deviceInfo = "<DeviceInfo>" +
                 "  <OutputFormat>jpeg</OutputFormat>" +
                 "  <PageWidth>10in</PageWidth>" +
                 "  <PageHeight>13in</PageHeight>" +
                 "  <MarginTop>0.5in</MarginTop>" +
                 "  <MarginLeft>1in</MarginLeft>" +
                 "  <MarginRight>1in</MarginRight>" +
                 "  <MarginBottom>0in</MarginBottom>" +
                 "</DeviceInfo>";
            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes;

            renderedBytes = reporte_local.Render(reportType, deviceInfo, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);
            return File(renderedBytes, mimeType);
        }

        public FileContentResult IndicadoresProductos()
        {

            LocalReport reporte_local = new LocalReport();
            RentaMaq.DAL.Context db = new RentaMaq.DAL.Context();
            reporte_local.ReportPath = Server.MapPath("~/Report/IndicadoresClaveProductos.rdlc");
            ReportDataSource conjunto_datos = new ReportDataSource();
            conjunto_datos.Name = "DataSet1";

            List<IndicadoresClaveProducto> IndicadoresProducto = IndicadoresClaveProducto.obtenerDatos();

            List<IndicadoresProdProvReporte> datos =
                IndicadoresProdProvReporte.convertirIndicadoresProductoEnReporte(
                IndicadoresProducto.OrderBy(s => s.tiempoRespuestaPromedio).ToList());

            conjunto_datos.Value = datos;
            reporte_local.DataSources.Add(conjunto_datos);
            string reportType = "PDF";
            string mimeType;
            string encoding;
            string fileNameExtension;
            string deviceInfo = "<DeviceInfo>" +
                 "  <OutputFormat>jpeg</OutputFormat>" +
                 "  <PageWidth>10in</PageWidth>" +
                 "  <PageHeight>13in</PageHeight>" +
                 "  <MarginTop>0.5in</MarginTop>" +
                 "  <MarginLeft>1in</MarginLeft>" +
                 "  <MarginRight>1in</MarginRight>" +
                 "  <MarginBottom>0.5in</MarginBottom>" +
                 "</DeviceInfo>";
            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes;

            renderedBytes = reporte_local.Render(reportType, deviceInfo, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);
            return File(renderedBytes, mimeType);
        }

        public FileContentResult IndicadoresProductosIndividual(int id)
        {
            LocalReport reporte_local = new LocalReport();
            RentaMaq.DAL.Context db = new RentaMaq.DAL.Context();
            reporte_local.ReportPath = Server.MapPath("~/Report/IndicadoresProductoIndividual.rdlc");
            ReportDataSource conjunto_datos = new ReportDataSource();
            conjunto_datos.Name = "DataSet1";

            IndicadoresClaveProducto datosProducto = new IndicadoresClaveProducto(id);

            List<IndicadoresProdProvReporte> datos =
                IndicadoresProdProvReporte.convertirTiemposRespuestaEnReporte(
                datosProducto.tiemposRespuestaPorProveedor.OrderBy(s => s.tiempoPromedioRespuesta).ToList());

            conjunto_datos.Value = datos;
            reporte_local.DataSources.Add(conjunto_datos);
            string reportType = "PDF";
            string mimeType;
            string encoding;
            string fileNameExtension;
            string deviceInfo = "<DeviceInfo>" +
                 "  <OutputFormat>jpeg</OutputFormat>" +
                 "  <PageWidth>10in</PageWidth>" +
                 "  <PageHeight>13in</PageHeight>" +
                 "  <MarginTop>0.5in</MarginTop>" +
                 "  <MarginLeft>1in</MarginLeft>" +
                 "  <MarginRight>1in</MarginRight>" +
                 "  <MarginBottom>0.5in</MarginBottom>" +
                 "</DeviceInfo>";
            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes;

            renderedBytes = reporte_local.Render(reportType, deviceInfo, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);
            return File(renderedBytes, mimeType);
        }

        public FileContentResult IndicadoresTiempoProvedorIndividual(int id)
        {
            LocalReport reporte_local = new LocalReport();
            RentaMaq.DAL.Context db = new RentaMaq.DAL.Context();
            reporte_local.ReportPath = Server.MapPath("~/Report/IndicadoresTiempoProveedorIndividual.rdlc");
            ReportDataSource conjunto_datos = new ReportDataSource();
            conjunto_datos.Name = "DataSet1";

            IndicadoresClaveProveedor datosProveedor = new IndicadoresClaveProveedor(id);

            List<IndicadoresProdProvReporte> datos =
                IndicadoresProdProvReporte.convertirTiemposRespuestaEnReporte(
                datosProveedor.tiemposRespuestaProducto.OrderBy(s => s.tiempoPromedioRespuesta).ToList());

            conjunto_datos.Value = datos;
            reporte_local.DataSources.Add(conjunto_datos);
            string reportType = "PDF";
            string mimeType;
            string encoding;
            string fileNameExtension;
            string deviceInfo = "<DeviceInfo>" +
                 "  <OutputFormat>jpeg</OutputFormat>" +
                 "  <PageWidth>10in</PageWidth>" +
                 "  <PageHeight>13in</PageHeight>" +
                 "  <MarginTop>0.5in</MarginTop>" +
                 "  <MarginLeft>1in</MarginLeft>" +
                 "  <MarginRight>1in</MarginRight>" +
                 "  <MarginBottom>0.5in</MarginBottom>" +
                 "</DeviceInfo>";
            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes;

            renderedBytes = reporte_local.Render(reportType, deviceInfo, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);
            return File(renderedBytes, mimeType);
        }

        public FileContentResult IndicadoresMontosProvedorIndividual(int id)
        {
            LocalReport reporte_local = new LocalReport();
            RentaMaq.DAL.Context db = new RentaMaq.DAL.Context();
            reporte_local.ReportPath = Server.MapPath("~/Report/IndicadoresMontosProveedorIndividual.rdlc");
            ReportDataSource conjunto_datos = new ReportDataSource();
            conjunto_datos.Name = "DataSet1";

            IndicadoresClaveProveedor datosProveedor = new IndicadoresClaveProveedor(id);

            List<IndicadoresProdProvReporte> datos =
                IndicadoresProdProvReporte.convertirIndicadoresMontoEnReporte(
                datosProveedor.montosCompra.OrderBy(s => s.montoCompra).ToList());

            conjunto_datos.Value = datos;
            reporte_local.DataSources.Add(conjunto_datos);
            string reportType = "PDF";
            string mimeType;
            string encoding;
            string fileNameExtension;
            string deviceInfo = "<DeviceInfo>" +
                 "  <OutputFormat>jpeg</OutputFormat>" +
                 "  <PageWidth>10in</PageWidth>" +
                 "  <PageHeight>13in</PageHeight>" +
                 "  <MarginTop>0.5in</MarginTop>" +
                 "  <MarginLeft>1in</MarginLeft>" +
                 "  <MarginRight>1in</MarginRight>" +
                 "  <MarginBottom>0.5in</MarginBottom>" +
                 "</DeviceInfo>";
            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes;

            renderedBytes = reporte_local.Render(reportType, deviceInfo, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);
            return File(renderedBytes, mimeType);
        }

        public FileContentResult IndicadoresTiempoOrdenArriendoProveedorIndividual(int id)
        {
            LocalReport reporte_local = new LocalReport();
            RentaMaq.DAL.Context db = new RentaMaq.DAL.Context();
            reporte_local.ReportPath = Server.MapPath("~/Report/IndicadoresTiempoEquiposProveedorIndividual.rdlc");
            ReportDataSource conjunto_datos = new ReportDataSource();
            conjunto_datos.Name = "DataSet1";

            IndicadoresClaveProveedor datosProveedor = new IndicadoresClaveProveedor(id);

            List<IndicadoresProdProvReporte> datos =
                IndicadoresProdProvReporte.convertirIndicadoresTiempoOrdenArriendoEquipoEnReporte(
                datosProveedor.tiemposRespuestaEquipo.OrderBy(s => s.tiempoRespuesta).ToList());

            conjunto_datos.Value = datos;
            reporte_local.DataSources.Add(conjunto_datos);
            string reportType = "PDF";
            string mimeType;
            string encoding;
            string fileNameExtension;
            string deviceInfo = "<DeviceInfo>" +
                 "  <OutputFormat>jpeg</OutputFormat>" +
                 "  <PageWidth>10in</PageWidth>" +
                 "  <PageHeight>13in</PageHeight>" +
                 "  <MarginTop>0.5in</MarginTop>" +
                 "  <MarginLeft>1in</MarginLeft>" +
                 "  <MarginRight>1in</MarginRight>" +
                 "  <MarginBottom>0.5in</MarginBottom>" +
                 "</DeviceInfo>";
            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes;

            renderedBytes = reporte_local.Render(reportType, deviceInfo, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);
            return File(renderedBytes, mimeType);
        }

        public FileContentResult CotizacionDeTraslado(int id)
        {
            LocalReport reporte_local = new LocalReport();
            RentaMaq.DAL.Context db = new RentaMaq.DAL.Context();
            reporte_local.ReportPath = Server.MapPath("~/Report/CotizacionDeTraslado.rdlc");
            ReportDataSource conjunto_datos = new ReportDataSource();
            conjunto_datos.Name = "DataSet1";

            List<cotizacionDeTrasladoReporte> datos = cotizacionDeTrasladoReporte.convertirDatos(db.CotizacionDeTraslado.Find(id));

            conjunto_datos.Value = datos;
            reporte_local.DataSources.Add(conjunto_datos);
            string reportType = "PDF";
            string mimeType;
            string encoding;
            string fileNameExtension;
            string deviceInfo = "<DeviceInfo>" +
                 "  <OutputFormat>jpeg</OutputFormat>" +
                 "  <PageWidth>10in</PageWidth>" +
                 "  <PageHeight>13in</PageHeight>" +
                 "  <MarginTop>0.5in</MarginTop>" +
                 "  <MarginLeft>1in</MarginLeft>" +
                 "  <MarginRight>1in</MarginRight>" +
                 "  <MarginBottom>0.5in</MarginBottom>" +
                 "</DeviceInfo>";
            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes;

            renderedBytes = reporte_local.Render(reportType, deviceInfo, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);
            return File(renderedBytes, mimeType);
        }

        public FileContentResult OrdenDePedido(int id)
        {
            LocalReport reporte_local = new LocalReport();
            RentaMaq.DAL.Context db = new RentaMaq.DAL.Context();
            reporte_local.ReportPath = Server.MapPath("~/Report/OrdenDePedido.rdlc");
            ReportDataSource conjunto_datos = new ReportDataSource();
            conjunto_datos.Name = "DataSet1";

            List<OrdenDePedidoReport> datos = OrdenDePedidoReport.convertirDatos(db.OrdenesPedido.Find(id));

            conjunto_datos.Value = datos;
            reporte_local.DataSources.Add(conjunto_datos);
            string reportType = "PDF";
            string mimeType;
            string encoding;
            string fileNameExtension;
            string deviceInfo = "<DeviceInfo>" +
                 "  <OutputFormat>jpeg</OutputFormat>" +
                 "  <PageWidth>10in</PageWidth>" +
                 "  <PageHeight>13in</PageHeight>" +
                 "  <MarginTop>0.5in</MarginTop>" +
                 "  <MarginLeft>1in</MarginLeft>" +
                 "  <MarginRight>1in</MarginRight>" +
                 "  <MarginBottom>0.5in</MarginBottom>" +
                 "</DeviceInfo>";
            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes;

            renderedBytes = reporte_local.Render(reportType, deviceInfo, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);
            return File(renderedBytes, mimeType);
        }

        public FileContentResult CotizacionServicio(int id)
        {
            LocalReport reporte_local = new LocalReport();
            RentaMaq.DAL.Context db = new RentaMaq.DAL.Context();
            reporte_local.ReportPath = Server.MapPath("~/Report/CotizacionServicios.rdlc");
            ReportDataSource conjunto_datos1 = new ReportDataSource();
            ReportDataSource conjunto_datos2 = new ReportDataSource();
            ReportDataSource conjunto_datos3 = new ReportDataSource();

            conjunto_datos1.Name = "DataSet1";
            conjunto_datos2.Name = "DataSet2";
            conjunto_datos3.Name = "DataSet3";

            List<cotizacionServicios> datos1  = new List<cotizacionServicios>();
            datos1.Add(db.cotizacionesServicios.Find(id));

            List<detalleServicioCotizacionServicios> datos2 = 
                db.detalleServiciosCotizacionServicios.Where(s => s.CotizacionServiciosID == id).OrderBy(s=>s.detalleServicioCotizacionServiciosID).ToList();

            List<detalleEquiposCotizacionServicios> datos3 =
                db.detalleEquiposCotizacionServicios.Where(s => s.CotizacionServiciosID == id).OrderBy(s => s.detalleEquiposCotizacionServiciosID).ToList();
            
            conjunto_datos1.Value = datos1;
            conjunto_datos2.Value = datos2;
            conjunto_datos3.Value = datos3;

            reporte_local.DataSources.Add(conjunto_datos1);
            reporte_local.DataSources.Add(conjunto_datos2);
            reporte_local.DataSources.Add(conjunto_datos3);
            string reportType = "PDF";
            string mimeType;
            string encoding;
            string fileNameExtension;
            string deviceInfo = "<DeviceInfo>" +
                 "  <OutputFormat>jpeg</OutputFormat>" +
                 "  <PageWidth>10in</PageWidth>" +
                 "  <PageHeight>13in</PageHeight>" +
                 "  <MarginTop>0.5in</MarginTop>" +
                 "  <MarginLeft>1in</MarginLeft>" +
                 "  <MarginRight>1in</MarginRight>" +
                 "  <MarginBottom>0.5in</MarginBottom>" +
                 "</DeviceInfo>";
            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes;

            renderedBytes = reporte_local.Render(reportType, deviceInfo, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);
            return File(renderedBytes, mimeType);
        }

        public FileContentResult HojaRutaMantenedoresPrevia()
        {

            LocalReport reporte_local = new LocalReport();
            RentaMaq.DAL.Context db = new RentaMaq.DAL.Context();
            reporte_local.ReportPath = Server.MapPath("~/Report/hojaRutaMantenedoresPrevia.rdlc");
            ReportDataSource conjunto_datos = new ReportDataSource();
            conjunto_datos.Name = "DataSet1";

            List<int> idsPendientes = equipos.obtenerIDsLubricacionesPendientes();

            List<hojaRutaMantenedores> pendientes = new List<hojaRutaMantenedores>();

            foreach (int idPendiente in idsPendientes) 
            {
                hojaRutaMantenedores nueva = new hojaRutaMantenedores();

                nueva.equipoID = idPendiente;

                pendientes.Add(nueva);
            }

            List<hojaRutaMantenedoresReporte> datos = hojaRutaMantenedoresReporte.convertirDatosAntesDeRegistro(pendientes);

            conjunto_datos.Value = datos;
            reporte_local.DataSources.Add(conjunto_datos);
            string reportType = "PDF";
            string mimeType;
            string encoding;
            string fileNameExtension;
            string deviceInfo = "<DeviceInfo>" +
                 "  <OutputFormat>jpeg</OutputFormat>" +
                 "  <PageWidth>10in</PageWidth>" +
                 "  <PageHeight>13in</PageHeight>" +
                 "  <MarginTop>0.5in</MarginTop>" +
                 "  <MarginLeft>1in</MarginLeft>" +
                 "  <MarginRight>1in</MarginRight>" +
                 "  <MarginBottom>0.5in</MarginBottom>" +
                 "</DeviceInfo>";
            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes;

            renderedBytes = reporte_local.Render(reportType, deviceInfo, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);
            return File(renderedBytes, mimeType);
        }

        public FileContentResult HojaRutaMantenedores(int id)
        {

            LocalReport reporte_local = new LocalReport();
            RentaMaq.DAL.Context db = new RentaMaq.DAL.Context();
            reporte_local.ReportPath = Server.MapPath("~/Report/hojaRutaMantenedores.rdlc");
            ReportDataSource conjunto_datos = new ReportDataSource();
            conjunto_datos.Name = "DataSet1";

            List<hojaRutaMantenedoresReporte> datos = hojaRutaMantenedoresReporte.convertirDatos(db.hojaRutaMantenedores.Where(s => s.numero == id).ToList());

            conjunto_datos.Value = datos;
            reporte_local.DataSources.Add(conjunto_datos);
            string reportType = "PDF";
            string mimeType;
            string encoding;
            string fileNameExtension;
            string deviceInfo = "<DeviceInfo>" +
                 "  <OutputFormat>jpeg</OutputFormat>" +
                 "  <PageWidth>10in</PageWidth>" +
                 "  <PageHeight>13in</PageHeight>" +
                 "  <MarginTop>0.5in</MarginTop>" +
                 "  <MarginLeft>1in</MarginLeft>" +
                 "  <MarginRight>1in</MarginRight>" +
                 "  <MarginBottom>0.5in</MarginBottom>" +
                 "</DeviceInfo>";
            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes;

            renderedBytes = reporte_local.Render(reportType, deviceInfo, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);
            return File(renderedBytes, mimeType);
        }



        public FileContentResult ordenDeTrabajoGeneral(int id)
        {
            LocalReport reporte_local = new LocalReport();
            RentaMaq.DAL.Context db = new RentaMaq.DAL.Context();
            reporte_local.ReportPath = Server.MapPath("~/Report/ordenDeTrabajoGeneral.rdlc");
            ReportDataSource conjunto_datosOT = new ReportDataSource();
            ReportDataSource conjunto_datosEjecutantes = new ReportDataSource();
            ReportDataSource conjunto_datosMatUt = new ReportDataSource();
            ReportDataSource conjunto_datosMatReq = new ReportDataSource();

            conjunto_datosOT.Name = "DataSetOT";
            conjunto_datosEjecutantes.Name = "DataSetEject";
            conjunto_datosMatUt.Name = "DataSetMatUt";
            conjunto_datosMatReq.Name = "DataSetMatReq";

            List<ReportOTGeneral> reportOT = new List<ReportOTGeneral>();
            reportOT.Add( new ReportOTGeneral(db.ordenDeTrabajoGenerals.Find(id)));
            List<ejecutanteTrabajoOT> ejecutantesTrabajoOT = db.ejecutanteTrabajoOTs.Where(s => s.ordenDeTrabajoGeneralID == id).ToList();
            
            List<materialesRequeridosOT> materialesRequeridosOT = db.materialesRequeridosOTs.Where(s => s.ordenDeTrabajoGeneralID == id).ToList();
            List<materialesUtilizadosOT> materialesUtilizadosOT = db.materialesUtilizadosOTs.Where(s => s.ordenDeTrabajoGeneralID == id).ToList();

            List<materialesRequeridoReportOTGeneral> matReq = new List<materialesRequeridoReportOTGeneral>();
            List<materialesUtilizadosReportOTGeneral> matUt = new List<materialesUtilizadosReportOTGeneral>();

            foreach (RentaMaq.Models.materialesRequeridosOT matR in materialesRequeridosOT)
            {
                materialesRequeridoReportOTGeneral materialR = new materialesRequeridoReportOTGeneral(matR);
                matReq.Add(materialR);
            }

            foreach (RentaMaq.Models.materialesUtilizadosOT matU in materialesUtilizadosOT)
            {
                materialesUtilizadosReportOTGeneral materialU = new materialesUtilizadosReportOTGeneral(matU);
                matUt.Add(materialU);

            }

            conjunto_datosOT.Value = reportOT;
            conjunto_datosEjecutantes.Value = ejecutantesTrabajoOT;
            conjunto_datosMatUt.Value = matUt;
            conjunto_datosMatReq.Value = matReq;

            reporte_local.DataSources.Add(conjunto_datosOT);
            reporte_local.DataSources.Add(conjunto_datosEjecutantes);
            reporte_local.DataSources.Add(conjunto_datosMatUt);
            reporte_local.DataSources.Add(conjunto_datosMatReq);
            string reportType = "PDF";
            string mimeType;
            string encoding;
            string fileNameExtension;
            string deviceInfo = "<DeviceInfo>" +
                 "  <OutputFormat>jpeg</OutputFormat>" +
                 "  <PageWidth>10in</PageWidth>" +
                 "  <PageHeight>13in</PageHeight>" +
                 "  <MarginTop>0.5in</MarginTop>" +
                 "  <MarginLeft>1in</MarginLeft>" +
                 "  <MarginRight>1in</MarginRight>" +
                 "  <MarginBottom>0.5in</MarginBottom>" +
                 "</DeviceInfo>";
            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes;

            renderedBytes = reporte_local.Render(reportType, deviceInfo, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);
            return File(renderedBytes, mimeType);
        }

        public FileContentResult costosOrdenesTrabajoGenerales(string inicio, string termino)
        {
            LocalReport reporte_local = new LocalReport();
            RentaMaq.DAL.Context db = new RentaMaq.DAL.Context();
            reporte_local.ReportPath = Server.MapPath("~/Report/indicadorOT.rdlc");
            ReportDataSource conjunto_datosOT = new ReportDataSource();           

            conjunto_datosOT.Name = "DataSet1";

            DateTime Inicio = DateTime.Now.AddMonths(-1);
            DateTime Termino = DateTime.Now;
            if (inicio != null || termino != null)
            {
                string[] inicioSeparado = inicio.Split('/');
                string[] terminoSeparado = termino.Split('/');

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

            
            conjunto_datosOT.Value = IndicadoresCostosActivosOTReport.convertirDatos(lista);
           

            reporte_local.DataSources.Add(conjunto_datosOT);
            
            string reportType = "PDF";
            string mimeType;
            string encoding;
            string fileNameExtension;
            string deviceInfo = "<DeviceInfo>" +
                 "  <OutputFormat>jpeg</OutputFormat>" +
                 "  <PageWidth>10in</PageWidth>" +
                 "  <PageHeight>13in</PageHeight>" +
                 "  <MarginTop>0.5in</MarginTop>" +
                 "  <MarginLeft>1in</MarginLeft>" +
                 "  <MarginRight>1in</MarginRight>" +
                 "  <MarginBottom>0.5in</MarginBottom>" +
                 "</DeviceInfo>";
            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes;

            renderedBytes = reporte_local.Render(reportType, deviceInfo, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);
            return File(renderedBytes, mimeType);
        }
        public FileContentResult costosDetalleOrdenesTrabajoGenerales(int id)
        {
            LocalReport reporte_local = new LocalReport();
            RentaMaq.DAL.Context db = new RentaMaq.DAL.Context();
            reporte_local.ReportPath = Server.MapPath("~/Report/indicadorDetalleOT.rdlc");
            ReportDataSource conjunto_datosOT = new ReportDataSource();

            conjunto_datosOT.Name = "DataSet1";

            conjunto_datosOT.Value = IndicadoresCostosActivosOTReport.convertirDatos(new IndicadoresCostosActivosOT(id));


            reporte_local.DataSources.Add(conjunto_datosOT);

            string reportType = "PDF";
            string mimeType;
            string encoding;
            string fileNameExtension;
            string deviceInfo = "<DeviceInfo>" +
                 "  <OutputFormat>jpeg</OutputFormat>" +
                 "  <PageWidth>10in</PageWidth>" +
                 "  <PageHeight>13in</PageHeight>" +
                 "  <MarginTop>0.5in</MarginTop>" +
                 "  <MarginLeft>1in</MarginLeft>" +
                 "  <MarginRight>1in</MarginRight>" +
                 "  <MarginBottom>0.5in</MarginBottom>" +
                 "</DeviceInfo>";
            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes;

            renderedBytes = reporte_local.Render(reportType, deviceInfo, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);
            return File(renderedBytes, mimeType);
        }
        
        public FileContentResult costosOrdenesTrabajoGeneralesEquipo()
        {
            LocalReport reporte_local = new LocalReport();
            RentaMaq.DAL.Context db = new RentaMaq.DAL.Context();
            reporte_local.ReportPath = Server.MapPath("~/Report/indicadorOTEquipo.rdlc");
            ReportDataSource conjunto_datosOT = new ReportDataSource();

            conjunto_datosOT.Name = "DataSet1";

            List<equipos> Equipos = new Context().Equipos.ToList();

            List<IndicadoresCostosOTEquipo> lista = new List<IndicadoresCostosOTEquipo>();
            foreach (equipos Equipo in Equipos)
            {
                lista.Add(new IndicadoresCostosOTEquipo(Equipo.ID));
            }

            List<IndicadoresCostosActivosOTReport> datos = IndicadoresCostosActivosOTReport.convertirDatos(lista);

            conjunto_datosOT.Value = datos;

            reporte_local.DataSources.Add(conjunto_datosOT);

            string reportType = "PDF";
            string mimeType;
            string encoding;
            string fileNameExtension;
            string deviceInfo = "<DeviceInfo>" +
                 "  <OutputFormat>jpeg</OutputFormat>" +
                 "  <PageWidth>10in</PageWidth>" +
                 "  <PageHeight>13in</PageHeight>" +
                 "  <MarginTop>0.5in</MarginTop>" +
                 "  <MarginLeft>1in</MarginLeft>" +
                 "  <MarginRight>1in</MarginRight>" +
                 "  <MarginBottom>0.5in</MarginBottom>" +
                 "</DeviceInfo>";
            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes;

            renderedBytes = reporte_local.Render(reportType, deviceInfo, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);
            return File(renderedBytes, mimeType);
        }
        public FileContentResult costosDetalleEquipo(int id)
        {
            LocalReport reporte_local = new LocalReport();
            RentaMaq.DAL.Context db = new RentaMaq.DAL.Context();
            reporte_local.ReportPath = Server.MapPath("~/Report/indicadorOTEquipoDetalle.rdlc");
            ReportDataSource conjunto_datosOT = new ReportDataSource();

            conjunto_datosOT.Name = "DataSet1";

            string ID = id.ToString();
            List<ordenDeTrabajoGeneral> listaOrdenes =
                db.ordenDeTrabajoGenerals.Where(s => s.idEquipo == ID).ToList();
            List<IndicadoresCostosActivosOT> lista = new List<IndicadoresCostosActivosOT>();

            foreach (ordenDeTrabajoGeneral OT in listaOrdenes)
            {
                lista.Add(new IndicadoresCostosActivosOT(OT));
            }

            conjunto_datosOT.Value = IndicadoresCostosActivosOTReport.convertirDatos(lista);

            reporte_local.DataSources.Add(conjunto_datosOT);

            string reportType = "PDF";
            string mimeType;
            string encoding;
            string fileNameExtension;
            string deviceInfo = "<DeviceInfo>" +
                 "  <OutputFormat>jpeg</OutputFormat>" +
                 "  <PageWidth>10in</PageWidth>" +
                 "  <PageHeight>13in</PageHeight>" +
                 "  <MarginTop>0.5in</MarginTop>" +
                 "  <MarginLeft>1in</MarginLeft>" +
                 "  <MarginRight>1in</MarginRight>" +
                 "  <MarginBottom>0.5in</MarginBottom>" +
                 "</DeviceInfo>";
            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes;

            renderedBytes = reporte_local.Render(reportType, deviceInfo, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);
            return File(renderedBytes, mimeType);
        }

        public FileContentResult indicadoresEquiposGeneral(string fechaInicio, string fechaFinal)
        {
            LocalReport reporte_local = new LocalReport();
            RentaMaq.DAL.Context db = new RentaMaq.DAL.Context();
            reporte_local.ReportPath = Server.MapPath("~/Report/indicadoresEquiposGeneral.rdlc");
            ReportDataSource conjunto_datos = new ReportDataSource();

            conjunto_datos.Name = "DataSet1";

            conjunto_datos.Value = indicadoresReporte.obtenerDatosEquipos(fechaInicio, fechaFinal);

            reporte_local.DataSources.Add(conjunto_datos);

            string reportType = "PDF";
            string mimeType;
            string encoding;
            string fileNameExtension;
            string deviceInfo = "<DeviceInfo>" +
                 "  <OutputFormat>jpeg</OutputFormat>" +
                 "  <PageWidth>10in</PageWidth>" +
                 "  <PageHeight>13in</PageHeight>" +
                 "  <MarginTop>0.5in</MarginTop>" +
                 "  <MarginLeft>1in</MarginLeft>" +
                 "  <MarginRight>1in</MarginRight>" +
                 "  <MarginBottom>0.5in</MarginBottom>" +
                 "</DeviceInfo>";
            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes;

            renderedBytes = reporte_local.Render(reportType, deviceInfo, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);
            return File(renderedBytes, mimeType);
        }

        public FileContentResult indicadoresEquiposIndividual(int id,string fechaInicio, string fechaFinal, string tipoAgrupacion)
        {
            LocalReport reporte_local = new LocalReport();
            RentaMaq.DAL.Context db = new RentaMaq.DAL.Context();
            reporte_local.ReportPath = Server.MapPath("~/Report/indicadoresEquipoIndividual.rdlc");
            ReportDataSource conjunto_datos = new ReportDataSource();

            conjunto_datos.Name = "DataSet1";

            conjunto_datos.Value = indicadoresReporte.obtenerDatosEquipo(id,fechaInicio,fechaFinal,tipoAgrupacion);

            reporte_local.DataSources.Add(conjunto_datos);

            string reportType = "PDF";
            string mimeType;
            string encoding;
            string fileNameExtension;
            string deviceInfo = "<DeviceInfo>" +
                 "  <OutputFormat>jpeg</OutputFormat>" +
                 "  <PageWidth>10in</PageWidth>" +
                 "  <PageHeight>13in</PageHeight>" +
                 "  <MarginTop>0.5in</MarginTop>" +
                 "  <MarginLeft>1in</MarginLeft>" +
                 "  <MarginRight>1in</MarginRight>" +
                 "  <MarginBottom>0.5in</MarginBottom>" +
                 "</DeviceInfo>";
            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes;

            renderedBytes = reporte_local.Render(reportType, deviceInfo, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);
            return File(renderedBytes, mimeType);
        }
    }
}