using RentaMaq.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentaMaq.Models
{
    public class cotizacionServicios
    {
        public int cotizacionServiciosID { set; get; }
        public string numeroCotizacion { set; get; }
        public string datosClienteEmpresa { set; get; }
        public string datosClienteRut { set; get; }
        public string datosClienteDomicilio { set; get; }
        public string datosClienteSolicitadoPor { set; get; }
        public DateTime fecha { set; get; }
        public string fechaEscrita { set; get; }
        public string encabezado { set; get; }
        public string descripcionServicio { set; get; }
        public string valorTotal { set;get; }
        public string nota { set; get; }
        public string faena { set; get; }
        public string tiempo { set; get; }
        public int anio { set; get; }


        public List<string> ObtenerCategorias()
        {
            List<string> lista = new List<string>();

            Context db = new Context();

            foreach (detalleServicioCotizacionServicios detalle in db.detalleServiciosCotizacionServicios.Where(s => s.CotizacionServiciosID == this.cotizacionServiciosID).ToList()) 
            {
                if (!lista.Contains(detalle.categoria)) 
                {
                    lista.Add(detalle.categoria);
                }
            }

            return lista;
        }

        internal void eliminarDetalleServiciosYEquipos()
        {
            Context db = new Context();
            foreach (detalleServicioCotizacionServicios detalle in db.detalleServiciosCotizacionServicios.Where(s => s.CotizacionServiciosID == this.cotizacionServiciosID).ToList()) 
            {
                db.detalleServiciosCotizacionServicios.Remove(detalle);
            }

            foreach (detalleEquiposCotizacionServicios detalle in db.detalleEquiposCotizacionServicios.Where(s => s.CotizacionServiciosID == this.cotizacionServiciosID).ToList())
            {
                db.detalleEquiposCotizacionServicios.Remove(detalle);
            }

            db.SaveChanges();
        }
    }
}