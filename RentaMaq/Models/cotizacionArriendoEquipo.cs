using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RentaMaq.Models
{
    public class cotizacionArriendoEquipo
    {
        public cotizacionArriendoEquipo()
        {
            detalleCotizacionArriendoEquipos = new List<detalleCotizacionArriendoEquipo>();
        }
        public int cotizacionArriendoEquipoID { set; get; }
        public string numeroCotizacionArriendo { set; get; }
        public string tipoCotizacion { set; get; }
        public string referencia { set; get; }
        public DateTime fecha { set; get; }
        public string datosClienteEmpresa { set; get; }
        public string datosClienteRut { set; get; }
        public string datosClienteDireccion { set; get; }
        public DateTime datosClienteFecha { set; get; }
        public string datosClienteSolicitado { set; get; }
        public string datosClienteEmail { set; get; }
        public string encabezado { set; get; }
        public string incluye { set; get; }
        public string noIncluye { set; get; }
        public string tiempoArriendo { set; get; }
        public string faena { set; get; }
        public string tipoHorasMinimas { set; get; }

        public virtual ICollection<detalleCotizacionArriendoEquipo> detalleCotizacionArriendoEquipos { get; set; }

    }

    public class detalleCotizacionArriendoEquipo
    {

        public int detalleCotizacionArriendoEquipoID { get; set; }

        public string detalle { set; get; }
        public int horasMinimas { set; get; }
        public string valorHoraMaquina { set; get; }

        public int cotizacionArriendoEquipoID { set; get; }
        [ForeignKey("cotizacionArriendoEquipoID")]

        public virtual cotizacionArriendoEquipo cotizacionArriendoEquipo { get; set; }

    }




    public class ReportCotizacionArriendoEquipo {

        public string numeroCotizacionArriendo { set; get; }
        public string tipoCotizacion { set; get; }
        public string referencia { set; get; }
        public string fecha { set; get; }
        public string datosClienteEmpresa { set; get; }
        public string datosClienteRut { set; get; }
        public string datosClienteDireccion { set; get; }
        public string datosClienteFecha { set; get; }
        public string datosClienteSolicitado { set; get; }
        public string datosClienteEmail { set; get; }
        public string encabezado { set; get; }
        public string incluye { set; get; }
        public string noIncluye { set; get; }
        public string tiempoArriendo { set; get; }
        public string faena { set; get; }
        public string tipoHorasMinimas { set; get; }

        //DETALLE
        public string detalle { set; get; }
        public int horasMinimas { set; get; }
        public string valorHoraMaquina { set; get; }

        public ReportCotizacionArriendoEquipo(cotizacionArriendoEquipo cotizacionArriendoEquipo, detalleCotizacionArriendoEquipo detalleCotizacionArriendoEquipo)
        { 
             this.numeroCotizacionArriendo =cotizacionArriendoEquipo.numeroCotizacionArriendo;
             this.tipoCotizacion = cotizacionArriendoEquipo.tipoCotizacion;
             this.referencia = cotizacionArriendoEquipo.referencia;
             this.fecha = formatearString.fechaPalabras(cotizacionArriendoEquipo.fecha);
             this.datosClienteEmpresa = cotizacionArriendoEquipo.datosClienteEmpresa;
             this.datosClienteRut = cotizacionArriendoEquipo.datosClienteRut;
             this.datosClienteDireccion =cotizacionArriendoEquipo.datosClienteDireccion;
             this.datosClienteFecha = cotizacionArriendoEquipo.datosClienteFecha.ToString("D").Split(',')[1];
             this.datosClienteSolicitado =cotizacionArriendoEquipo.datosClienteSolicitado;
             this.datosClienteEmail = cotizacionArriendoEquipo.datosClienteEmail;
             this.encabezado = cotizacionArriendoEquipo.encabezado;
             this.incluye = cotizacionArriendoEquipo.incluye;
             this.noIncluye = cotizacionArriendoEquipo.noIncluye;
             this.tiempoArriendo = cotizacionArriendoEquipo.tiempoArriendo;
             this.faena = cotizacionArriendoEquipo.faena;
             this.tipoHorasMinimas = cotizacionArriendoEquipo.tipoHorasMinimas;

             this.detalle = detalleCotizacionArriendoEquipo.detalle;
             this.horasMinimas = detalleCotizacionArriendoEquipo.horasMinimas;
             this.valorHoraMaquina = detalleCotizacionArriendoEquipo.valorHoraMaquina;
        
        }
        
    }
}