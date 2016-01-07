using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlClient;
using RentaMaq.DAL;

namespace RentaMaq.Models
{
    public class solicitudDeCotizacion
    {
        public solicitudDeCotizacion()
        {
            detalleSolicitudDeCotizacions = new List<detalleSolicitudDeCotizacion>();
        }

        public int solicitudDeCotizacionID { get; set; }
        public string numeroSolicitudDeCotizacion { set; get; }       
        public int numeroEdicion { set; get; }
        public string codigoNumero { set; get; }
        public DateTime fecha { set; get; }
        public string proveedor { set; get; }
        public string emitidoPor { set; get; }
        public string escritoPor { set; get; }
        public string escritoPorCargo { set; get; }
        public virtual ICollection<detalleSolicitudDeCotizacion> detalleSolicitudDeCotizacions { get; set; }

        public List<solicitudDeCotizacion> Pendientes()
        {
            List<solicitudDeCotizacion> retorno = new List<solicitudDeCotizacion>();

            Context db = new Context();
            SqlConnection con = conexion.crearConexion();
            con.Open();

            using (SqlCommand command = new SqlCommand("SELECT solicitudDeCotizacionID FROM OrdenDeCompraGeneral,solicitudDeCotizacion WHERE OrdenDeCompraGeneral.solicitudDeCotizacionID_solicitudDeCotizacionID<>solicitudDeCotizacion.solicitudDeCotizacionID ", con))
            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    retorno.Add(db.solicitudesDeCotizaciones.Find(int.Parse(reader[0].ToString())));
                }
            }

            con.Close();

            return retorno;
        }
    }
    public class detalleSolicitudDeCotizacion {

        public int detalleSolicitudDeCotizacionID { get; set; }
        public int numeroItem { set; get; }
        public string descripcionItem {set;get;}
        public string codigoProducto { set; get; }
        public double cantidad { set; get; }
        public string codigoInterno { set; get; }
        public string lugarDeFaena { set; get; }
        public string tipoCompra { set; get; }
       
        public int solicitudDeCotizacionID { set; get; }
        [ForeignKey("solicitudDeCotizacionID")]
      
        public virtual solicitudDeCotizacion solicitudDeCotizacion { get; set; }
    
    }
    public class ReportSolicitudDeCotizacion{

        public int solicitudDeCotizacionID { get; set; }
        public string numeroSolicitudDeCotizacion { set; get; }       
        public int numeroEdicion { set; get; }
        public string codigoNumero { set; get; }
        public string fecha { set; get; }
        public string proveedor { set; get; }
        public string emitidoPor { set; get; }
        public string escritoPor { set; get; }
        public string escritoPorCargo { set; get; }
       
        public int numeroItem { set; get; }
        public string descripcionItem { set; get; }
        public string codigoProducto { set; get; }
        public double cantidad { set; get; }
        public string codigoInterno { set; get; }
        public string lugarDeFaena { set; get; }
        //public string tipoCompra { set; get; }

        public ReportSolicitudDeCotizacion(solicitudDeCotizacion solicitudDeCotizacion, detalleSolicitudDeCotizacion detalleSolicitudDeCotizacion) { 
        
            this.solicitudDeCotizacionID =solicitudDeCotizacion.solicitudDeCotizacionID;
            this.numeroSolicitudDeCotizacion =solicitudDeCotizacion.numeroSolicitudDeCotizacion;            
            this.numeroEdicion  = solicitudDeCotizacion.numeroEdicion;
            this.codigoNumero = solicitudDeCotizacion.codigoNumero;
            this.fecha = formatearString.fechaPalabras(solicitudDeCotizacion.fecha);
            this.proveedor = solicitudDeCotizacion.proveedor;
            this.emitidoPor = solicitudDeCotizacion.emitidoPor;
            this.escritoPor = solicitudDeCotizacion.escritoPor;
            this.escritoPorCargo = solicitudDeCotizacion.escritoPorCargo;
            
            this.numeroItem = detalleSolicitudDeCotizacion.numeroItem;
            this.descripcionItem  = detalleSolicitudDeCotizacion.descripcionItem;
            this.codigoProducto = detalleSolicitudDeCotizacion.codigoProducto;
            this.cantidad  = detalleSolicitudDeCotizacion.cantidad;
           
            this.lugarDeFaena = detalleSolicitudDeCotizacion.lugarDeFaena;
            //this.tipoCompra = detalleSolicitudDeCotizacion.tipoCompra;

            if (detalleSolicitudDeCotizacion.tipoCompra.Equals("DIRECTA"))
            {
                int codigo =Convert.ToInt32(detalleSolicitudDeCotizacion.codigoInterno);
                this.codigoInterno = equipos.Obtener(codigo).numeroAFI;
            }
            else {
                this.codigoInterno = detalleSolicitudDeCotizacion.codigoInterno;
            }
            

        }
    
    }
}