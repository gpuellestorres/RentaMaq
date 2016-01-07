using RentaMaq.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RentaMaq.Models
{
    public class OrdenDePedido
    {
        public int OrdenDePedidoID { get; set; }
        [Display(Name = "Número de Orden")]
        public int numeroOrden { get; set; }
        [Display(Name = "Año")]
        public int año { get; set; }
        [Display(Name = "Cliente")]
        public string señores { get; set; }
        [Display(Name = "Fecha")]
        public DateTime fecha { get; set; }
        [Display(Name = "Encabezado")]
        public string encabezado { get; set; }
        [Display(Name = "Trabajo a realizar")]
        public string trabajoARealizar { get; set; }
        [Display(Name = "Persona Responsable")]
        public string personaResponsable { get; set; }
        [Display(Name = "Nombre de Solicitante")]
        public string nombreSolicitante { get; set; }
        [Display(Name = "Fecha de Solicitud")]
        public DateTime fechaSolicitud { get; set; }


        public void quitarNulos()
        {
            if (señores == null)
            {
                señores = "";
            }
            if (fecha == null)
            {
                fecha = DateTime.Now;
            }
            if (encabezado == null)
            {
                encabezado = "";
            }
            if (trabajoARealizar == null)
            {
                trabajoARealizar = "";
            }
            if (nombreSolicitante == null)
            {
                nombreSolicitante = "";
            }
            if (trabajoARealizar == null)
            {
                trabajoARealizar = "";
            }
            if (fechaSolicitud == null)
            {
                fechaSolicitud = DateTime.Now;
            }
        }

        public int obtenerNuevoNumero()
        {
            List<OrdenDePedido> OP = new Context().OrdenesPedido.OrderByDescending(s => s.numeroOrden).Take(1).ToList();
            if (OP.Count == 0) return 1;
            else return OP[0].numeroOrden + 1;
        }

        internal void eliminarDetalle()
        {
            Context db = new Context();

            List<detalleOrdenPedido> detalle = db.DetalleOrdenesPedido.Where(s => s.OrdenDePedidoID == this.OrdenDePedidoID).ToList();
            foreach (detalleOrdenPedido det in detalle)             
            {
                db.DetalleOrdenesPedido.Remove(det);
            }
            db.SaveChanges();
        }
    }

    public class OrdenDePedidoReport 
    {
        public int numeroOrden { get; set; }
        public int año { get; set; }
        public string señores { get; set; }
        public string fecha { get; set; }
        public string encabezado { get; set; }
        public string trabajoARealizar { get; set; }
        public string personaResponsable { get; set; }
        public string nombreSolicitante { get; set; }
        public string fechaSolicitud { get; set; }
        public string detalle { get; set; }
        public int cantidad { get; set; }

        public static List<OrdenDePedidoReport> convertirDatos(OrdenDePedido OrdenPedido) 
        {
            List<OrdenDePedidoReport> retorno = new List<OrdenDePedidoReport>();

            foreach (detalleOrdenPedido Detalle in new Context().DetalleOrdenesPedido.Where(s => s.OrdenDePedidoID == OrdenPedido.OrdenDePedidoID).ToList())
            {
                OrdenDePedidoReport nuevo = new OrdenDePedidoReport();

                nuevo.numeroOrden = OrdenPedido.numeroOrden;
                nuevo.año = OrdenPedido.año;
                nuevo.señores = OrdenPedido.señores;

                string dia = OrdenPedido.fecha.Day.ToString();
                if (dia.Length == 1) dia = "0" + dia;
                string mes = OrdenPedido.fecha.Month.ToString();
                if (mes.Length == 1) mes = "0" + mes;

                nuevo.fecha = dia + "/" + mes + "/" + OrdenPedido.fecha.Year;

                nuevo.encabezado = OrdenPedido.encabezado;
                nuevo.trabajoARealizar = OrdenPedido.trabajoARealizar;
                nuevo.personaResponsable = OrdenPedido.personaResponsable;
                nuevo.nombreSolicitante = OrdenPedido.nombreSolicitante;

                dia = OrdenPedido.fechaSolicitud.Day.ToString();
                if (dia.Length == 1) dia = "0" + dia;
                mes = OrdenPedido.fechaSolicitud.Month.ToString();
                if (mes.Length == 1) mes = "0" + mes;

                nuevo.fechaSolicitud = dia + "/" + mes + "/" + OrdenPedido.fechaSolicitud.Year;
                nuevo.detalle = Detalle.detalle;
                nuevo.cantidad = Detalle.cantidad;

                retorno.Add(nuevo);
            }
            return retorno;

        }
    }
}