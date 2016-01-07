using RentaMaq.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RentaMaq.Models
{
    public class OrdenDeCompraGeneral
    {
        public int OrdenDeCompraGeneralID { get; set; }
        /*
        [Display(Name = "Fecha de Vigencia")]
        public DateTime FechaVigencia { get; set; }
        //*/
        [Display(Name = "Fecha")]
        public DateTime Fecha { get; set; }
        /*
        [Display(Name = "Número de Edición")]
        public int NumeroEdicion { get; set; }
        [Display(Name = "Código")]
        public string Codigo { get; set; }
        //*/
        [Display(Name = "Orden de Compra Teck CDA N°")]
        public int numeroOC { get; set; }
        public int añoOC { get; set; }
        [Display(Name = "Proveedor")]
        public Proveedor ProveedorID { get; set; }
        [Display(Name = "Atención a:")]
        public string atencionA { get; set; }
        [Display(Name = "Texto de Orden de Compra:")]
        public string texto { get; set; }
        [Display(Name = "Fecha de Entrega")]
        public DateTime fechaEntrega { get; set; }
        [Display(Name = "Forma de Retiro")]
        public string formaRetiro { get; set; }
        [Display(Name = "Forma de Pago")]
        public string formaPago { get; set; }

        [Display(Name = "Subtotal")]
        public int subtotal { get; set; }
        [Display(Name = "Miscelaneos")]
        public int miscelaneos { get; set; }
        [Display(Name = "Total Neto")]
        public int totalNeto { get; set; }
        [Display(Name = "IVA")]
        public int IVA { get; set; }
        [Display(Name = "Total")]
        public int total { get; set; }
        [Display(Name = "Estado")]
        public string estado { get; set; }
        [Display(Name = "Tipo de Orden de Compra")]
        public string tipo { get; set; }

        internal void obtenerNumeroOC()
        {
            DAL.Context db = new DAL.Context();
            this.numeroOC = 1;

            foreach (OrdenDeCompraGeneral OC in db.ordenesDeCompra.Where(s => s.añoOC == this.añoOC && s.tipo == this.tipo).ToList())
            {
                if (OC.numeroOC >= this.numeroOC)
                {
                    this.numeroOC = OC.numeroOC + 1;
                }
            }
        }

        internal void guardar()
        {
            if (!existe())
            {
                insertar();
            }
            else
            {
                actualizar();
            }
        }

        private void actualizar()
        {
            Context db = new Context();
            SqlConnection con = conexion.crearConexion();
            con.Open();

            using (SqlCommand command = new SqlCommand("UPDATE OrdenDeCompraGeneral SET "
                + "numeroOC=@numeroOC, añoOC=@añoOC, ProveedorID_ProveedorID=@ProveedorID, atencionA=@atencionA, "
                + "texto=@texto, fechaEntrega=@fechaEntrega, formaRetiro=@formaRetiro, formaPago=@formaPago, "
                + "subtotal=@subtotal, miscelaneos=@miscelaneos, totalNeto=@totalNeto, IVA=@IVA, total=@total, "
                + "estado=@estado, Fecha=@Fecha, tipo=@tipo WHERE OrdenDeCompraGeneralID=@OrdenDeCompraGeneralID", con))
            {
                /*
                command.Parameters.Add("@FechaVigencia", SqlDbType.DateTime).Value = this.FechaVigencia;
                command.Parameters.Add("@NumeroEdicion", SqlDbType.Int).Value = this.NumeroEdicion;
                command.Parameters.Add("@Codigo", SqlDbType.NVarChar).Value = this.Codigo;//*/
                command.Parameters.Add("@numeroOC", SqlDbType.Int).Value = this.numeroOC;
                command.Parameters.Add("@añoOC", SqlDbType.Int).Value = this.añoOC;
                command.Parameters.Add("@ProveedorID", SqlDbType.Int).Value = this.ProveedorID.ProveedorID;
                command.Parameters.Add("@atencionA", SqlDbType.NVarChar).Value = this.atencionA;
                command.Parameters.Add("@texto", SqlDbType.NVarChar).Value = this.texto;
                command.Parameters.Add("@fechaEntrega", SqlDbType.DateTime).Value = this.fechaEntrega;
                command.Parameters.Add("@formaRetiro", SqlDbType.NVarChar).Value = this.formaRetiro;
                command.Parameters.Add("@formaPago", SqlDbType.NVarChar).Value = this.formaPago;
                command.Parameters.Add("@subtotal", SqlDbType.Int).Value = this.subtotal;
                command.Parameters.Add("@miscelaneos", SqlDbType.Int).Value = this.miscelaneos;
                command.Parameters.Add("@totalNeto", SqlDbType.Int).Value = this.totalNeto;
                command.Parameters.Add("@IVA", SqlDbType.Int).Value = this.IVA;
                command.Parameters.Add("@total", SqlDbType.Int).Value = this.total;
                command.Parameters.Add("@OrdenDeCompraGeneralID", SqlDbType.Int).Value = this.OrdenDeCompraGeneralID;
                command.Parameters.Add("@estado", SqlDbType.NVarChar).Value = this.estado;
                command.Parameters.Add("@Fecha", SqlDbType.DateTime).Value = this.Fecha;
                command.Parameters.Add("@tipo", SqlDbType.NVarChar).Value = this.tipo;

                command.ExecuteNonQuery();
            }

            con.Close();
        }

        private void insertar()
        {
            Context db = new Context();
            SqlConnection con = conexion.crearConexion();
            con.Open();

            using (SqlCommand command = new SqlCommand("INSERT INTO OrdenDeCompraGeneral"
                + "(numeroOC, añoOC, ProveedorID_ProveedorID, texto, atencionA, fechaEntrega,"
                + "formaRetiro, formaPago, subtotal, miscelaneos,"
                + "totalNeto, IVA, total, OrdenDeCompraGeneralID, estado, Fecha, tipo) "
                + "VALUES(@numeroOC, @añoOC, @ProveedorID_ProveedorID, @texto, @atencionA, @fechaEntrega,"
                + "@formaRetiro, @formaPago, @subtotal, @miscelaneos,"
                + "@totalNeto, @IVA, @total, @OrdenDeCompraGeneralID, @estado ,@Fecha, @tipo)", con))
            {
                /*
                command.Parameters.Add("@FechaVigencia", SqlDbType.DateTime).Value = this.FechaVigencia;
                command.Parameters.Add("@NumeroEdicion", SqlDbType.Int).Value = this.NumeroEdicion;
                command.Parameters.Add("@Codigo", SqlDbType.NVarChar).Value = this.Codigo;
                //*/
                command.Parameters.Add("@numeroOC", SqlDbType.Int).Value = this.numeroOC;
                command.Parameters.Add("@añoOC", SqlDbType.Int).Value = this.añoOC;
                command.Parameters.Add("@ProveedorID_ProveedorID", SqlDbType.Int).Value = this.ProveedorID.ProveedorID;
                command.Parameters.Add("@atencionA", SqlDbType.NVarChar).Value = this.atencionA;
                command.Parameters.Add("@texto", SqlDbType.NVarChar).Value = this.texto;
                command.Parameters.Add("@fechaEntrega", SqlDbType.DateTime).Value = this.fechaEntrega;
                command.Parameters.Add("@formaRetiro", SqlDbType.NVarChar).Value = this.formaRetiro;
                command.Parameters.Add("@formaPago", SqlDbType.NVarChar).Value = this.formaPago;
                command.Parameters.Add("@subtotal", SqlDbType.Int).Value = this.subtotal;
                command.Parameters.Add("@miscelaneos", SqlDbType.Int).Value = this.miscelaneos;
                command.Parameters.Add("@totalNeto", SqlDbType.Int).Value = this.totalNeto;
                command.Parameters.Add("@IVA", SqlDbType.Int).Value = this.IVA;
                command.Parameters.Add("@total", SqlDbType.Int).Value = this.total;
                command.Parameters.Add("@OrdenDeCompraGeneralID", SqlDbType.Int).Value = this.OrdenDeCompraGeneralID;
                command.Parameters.Add("@estado", SqlDbType.NVarChar).Value = this.estado;
                command.Parameters.Add("@Fecha", SqlDbType.DateTime).Value = this.Fecha;
                command.Parameters.Add("@tipo", SqlDbType.NVarChar).Value = this.tipo;

                command.ExecuteNonQuery();
            }

            con.Close();
        }

        private bool existe()
        {
            bool retorno = false;
            Context db = new Context();
            SqlConnection con = conexion.crearConexion();
            con.Open();

            using (SqlCommand command = new SqlCommand("SELECT * FROM OrdenDeCompraGeneral WHERE OrdenDeCompraGeneralID=@OrdenDeCompraGeneralID", con))
            {
                command.Parameters.Add("@OrdenDeCompraGeneralID", SqlDbType.Int).Value = this.OrdenDeCompraGeneralID;
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    retorno = reader.HasRows;
                }
            }

            con.Close();
            return retorno;
        }

        internal void obtenerID()
        {
            DAL.Context db = new DAL.Context();
            this.OrdenDeCompraGeneralID = 0;

            foreach (OrdenDeCompraGeneral OC in db.ordenesDeCompra.ToList())
            {
                if (OC.OrdenDeCompraGeneralID >= this.OrdenDeCompraGeneralID)
                {
                    this.OrdenDeCompraGeneralID = OC.OrdenDeCompraGeneralID + 1;
                }
            }
        }

        internal static OrdenDeCompraGeneral obtener(int? id)
        {
            RentaMaq.DAL.Context db = new RentaMaq.DAL.Context();
            OrdenDeCompraGeneral retorno = db.ordenesDeCompra.Find(id);
            retorno.ProveedorID = obtenerProveedor(retorno.OrdenDeCompraGeneralID);
            return retorno;
        }

        private static solicitudDeCotizacion obtenerSolicitud(int OCID)
        {
            solicitudDeCotizacion solicitud = new solicitudDeCotizacion();
            Context db = new Context();
            SqlConnection con = conexion.crearConexion();
            con.Open();

            using (SqlCommand command = new SqlCommand("SELECT solicitudDeCotizacionID_solicitudDeCotizacionID FROM OrdenDeCompraGeneral "
                + "WHERE OrdenDeCompraGeneralID=@OrdenDeCompraGeneralID", con))
            {
                command.Parameters.Add("@OrdenDeCompraGeneralID", SqlDbType.Int).Value = OCID;
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        solicitud = db.solicitudesDeCotizaciones.Find(int.Parse(reader[0].ToString()));
                    }
                }
            }

            con.Close();

            return solicitud;
        }

        private static Proveedor obtenerProveedor(int OCID)
        {
            Proveedor proveedor = new Proveedor();
            Context db = new Context();
            SqlConnection con = conexion.crearConexion();
            con.Open();

            using (SqlCommand command = new SqlCommand("SELECT ProveedorID_ProveedorID FROM OrdenDeCompraGeneral "
                + "WHERE OrdenDeCompraGeneralID=@OrdenDeCompraGeneralID", con))
            {
                command.Parameters.Add("@OrdenDeCompraGeneralID", SqlDbType.Int).Value = OCID;
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        proveedor = db.Proveedores.Find(int.Parse(reader[0].ToString()));
                    }
                }
            }

            con.Close();

            return proveedor;
        }

        internal void eliminarDetalleBD()
        {
            Context db = new Context();
            SqlConnection con = conexion.crearConexion();
            con.Open();

            using (SqlCommand command = new SqlCommand("DELETE FROM DetalleOrdenCompra "
                + "WHERE IDOrdenCompra=@IDOrdenCompra", con))
            {
                command.Parameters.Add("@IDOrdenCompra", SqlDbType.Int).Value = this.OrdenDeCompraGeneralID;
                command.ExecuteNonQuery();
            }

            con.Close();
        }

        internal static List<OrdenDeCompraGeneral> Todos(DateTime inicio, DateTime termino, string estado)
        {
            DateTime Termino = termino;
            DateTime temp = termino;

            while (temp.Day == termino.Day)
            {
                Termino = temp;
                temp = temp.AddMinutes(1);
            }

            List<OrdenDeCompraGeneral> retorno = new List<OrdenDeCompraGeneral>();
            if (estado.Equals("TODOS"))
            {
                retorno = new RentaMaq.DAL.Context().ordenesDeCompra.Where(s => s.Fecha >= inicio && s.Fecha <= Termino).ToList();
            }
            else if (estado.Equals("PENDIENTES"))
            {
                retorno = OrdenDeCompraGeneral.obtenerPendientes().Where(s => s.Fecha >= inicio && s.Fecha <= Termino).ToList();
            }
            else
            {
                retorno = new RentaMaq.DAL.Context().ordenesDeCompra.Where(s => s.Fecha >= inicio && s.Fecha <= Termino && s.estado == estado).ToList();
            }

            foreach (OrdenDeCompraGeneral OC in retorno)
            {
                OC.ProveedorID = OrdenDeCompraGeneral.obtenerProveedor(OC.OrdenDeCompraGeneralID);
            }

            return retorno;
        }

        public static List<OrdenDeCompraGeneral> obtenerPendientes()
        {
            List<OrdenDeCompraGeneral> retorno = new List<OrdenDeCompraGeneral>();

            SqlConnection con = conexion.crearConexion();
            con.Open();

            using (SqlCommand command = new SqlCommand("SELECT DISTINCT(IDOrdenCompra) "
                + "FROM DetalleOrdenCompra, DetalleEntregaOrdenCompraGeneral "
                + "WHERE DetalleEntregaOrdenCompraGeneral.DetalleOrdenCompraID=DetalleOrdenCompra.DetalleOrdenCompraID "
                + "AND CantidadEntregada<cantidad", con))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        retorno.Add(OrdenDeCompraGeneral.obtener(int.Parse(reader[0].ToString())));
                    }
                }
            }
            con.Close();
            return retorno;
        }

        public static bool existenPendientes()
        {
            bool retorno = false;

            SqlConnection con = conexion.crearConexion();
            con.Open();

            using (SqlCommand command = new SqlCommand("SELECT DISTINCT(IDOrdenCompra) "
                + "FROM DetalleOrdenCompra, DetalleEntregaOrdenCompraGeneral "
                + "WHERE DetalleEntregaOrdenCompraGeneral.DetalleOrdenCompraID=DetalleOrdenCompra.DetalleOrdenCompraID "
                + "AND CantidadEntregada<cantidad", con))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    retorno = reader.HasRows;
                }
            }
            con.Close();
            return retorno;
        }

        internal static void enviarCorreo(int idOrdenCompra)
        {
            Context db = new Context();

            List<avisosCorreoOrdenCompraGeneral> listaCorreos = db.avisosCorreoOrdenCompraGeneral.ToList();

            foreach (avisosCorreoOrdenCompraGeneral correo in listaCorreos)
            {
                UrlHelper url = new UrlHelper(HttpContext.Current.Request.RequestContext);

                string texto = "Estimado/a " + correo.nombreContacto
                    + ":<br><br>Le notificamos que acaba de registrarse una Orden de Compra con productos por "
                    + "ser entregados."
                    + "<br><br>Para ver todas las órdenes de compra con productos pendientes visite el "
                    + "siguiente enlace:<br><br>http://rentamaq.azurewebsites.net"
                    + url.Action("Index", "OrdenDeCompraGeneral", new { estado = "PENDIENTES" });
                envioCorreos.enviarAlerta(correo.correo, "Alerta por productos pendientes en Orden de Compra", texto);
            }
        }
    }

    public class OrdenDeCompraGeneralReporte
    {
        /*
        public string FechaVigencia { get; set; }
        public string NumeroEdicion { get; set; }
        public string Codigo { get; set; }
        //*/
        public string NumeroOC { get; set; }
        public string AñoOC { get; set; }
        public string NombreProveedor { get; set; }
        public string RutProveedor { get; set; }
        public string DomicilioProveedor { get; set; }
        public string Fecha { get; set; }
        public string AtencionA { get; set; }
        public string texto { get; set; }
        public string NumeroItem { get; set; }
        public string CodigoItem { get; set; }
        public string CantidadItem { get; set; }
        public string DescripcionItem { get; set; }
        public string CodigoInternoItem { get; set; }
        public string PlazoEntregaItem { get; set; }
        public string PrecioUnitarioItem { get; set; }
        public string DescuentoItem { get; set; }
        public string ValorTotalItem { get; set; }
        public string FechaEntrega { get; set; }
        public string FormaRetiro { get; set; }
        public string FormaPago { get; set; }
        public string Subtotal { get; set; }
        public string Miscelaneos { get; set; }
        public string TotalNeto { get; set; }
        public string IVA { get; set; }
        public string Total { get; set; }

        public static List<OrdenDeCompraGeneralReporte> pasarADatosReporte(OrdenDeCompraGeneral OC)
        {
            List<DetalleOrdenCompra> detalle = new RentaMaq.DAL.Context().detalleOrdenCompra.Where(s => s.IDOrdenCompra == OC.OrdenDeCompraGeneralID).ToList();

            List<OrdenDeCompraGeneralReporte> retorno = new List<OrdenDeCompraGeneralReporte>();

            for (int i = 0; i < detalle.Count; i++)
            {
                OrdenDeCompraGeneralReporte nueva = new OrdenDeCompraGeneralReporte();
                /*
                string dia = OC.FechaVigencia.Day.ToString();
                if (dia.Length == 1) dia = "0" + dia;
                string mes = OC.FechaVigencia.Month.ToString();
                if (mes.Length == 1) mes = "0" + mes;
                nueva.FechaVigencia = OC.FechaVigencia.Day + "/" + OC.FechaVigencia.Month + "/" + OC.FechaVigencia.Year;
                nueva.NumeroEdicion = OC.NumeroEdicion.ToString();
                nueva.Codigo = OC.Codigo;
                //*/
                nueva.NumeroOC = OC.numeroOC.ToString();
                nueva.AñoOC = OC.añoOC.ToString();
                nueva.NombreProveedor = OC.ProveedorID.nombreProveedor;
                if (string.IsNullOrEmpty(OC.ProveedorID.rut))
                {
                    nueva.RutProveedor = "";
                }
                else
                {
                    nueva.RutProveedor = formatearString.formatoRut(OC.ProveedorID.rut);
                }

                if (string.IsNullOrEmpty(OC.ProveedorID.domicilio))
                {
                    nueva.DomicilioProveedor = OC.ProveedorID.domicilio;
                }
                else
                {
                    nueva.DomicilioProveedor = OC.ProveedorID.domicilio;
                }

                string dia = OC.Fecha.Day.ToString();
                if (dia.Length == 1) dia = "0" + dia;
                string mes = OC.Fecha.Month.ToString();
                if (mes.Length == 1) mes = "0" + mes;
                nueva.Fecha = dia + "/" + mes + "/" + OC.Fecha.Year;
                nueva.AtencionA = OC.atencionA;
                nueva.texto = OC.texto;
                nueva.NumeroItem = (i + 1).ToString();
                nueva.CodigoItem = detalle[i].codigo;
                nueva.CantidadItem = detalle[i].cantidad.ToString();
                nueva.DescripcionItem = detalle[i].descripcion;

                if (detalle[i].tipoDeCompra.Equals("Compra Directa"))
                {
                    nueva.CodigoInternoItem = equipos.Obtener(int.Parse(detalle[i].codigoInternoRentamaq)).numeroAFI;
                }
                else
                {
                    nueva.CodigoInternoItem = detalle[i].codigoInternoRentamaq;
                }


                nueva.PlazoEntregaItem = detalle[i].plazoEntrega.ToString();
                nueva.PrecioUnitarioItem = detalle[i].precioUnitario.ToString();
                nueva.DescuentoItem = detalle[i].descuento.ToString();
                nueva.ValorTotalItem = detalle[i].valorTotal.ToString();
                dia = OC.fechaEntrega.Day.ToString();
                if (dia.Length == 1) dia = "0" + dia;
                mes = OC.fechaEntrega.Month.ToString();
                if (mes.Length == 1) mes = "0" + mes;
                nueva.FechaEntrega = dia + "/" + mes + "/" + OC.fechaEntrega.Year;
                nueva.FormaRetiro = OC.formaRetiro;
                nueva.FormaPago = OC.formaPago.ToString();
                nueva.Subtotal = formatearString.valores_Pesos(OC.subtotal.ToString());
                nueva.Miscelaneos = formatearString.valores_Pesos(OC.miscelaneos.ToString());
                nueva.TotalNeto = formatearString.valores_Pesos(OC.totalNeto.ToString());
                nueva.IVA = formatearString.valores_Pesos(OC.IVA.ToString());
                nueva.Total = formatearString.valores_Pesos(OC.total.ToString());

                retorno.Add(nueva);
            }
            return retorno;
        }
    }
}