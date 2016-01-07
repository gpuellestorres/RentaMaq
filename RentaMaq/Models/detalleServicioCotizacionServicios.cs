using RentaMaq.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentaMaq.Models
{
    public class detalleServicioCotizacionServicios
    {
        public int detalleServicioCotizacionServiciosID {get;set;}
        public string categoria { get; set; }
        public string cargo { get; set; }
        public string turno { get; set; }
        public int numeroPersonas { get; set; }
        public int CotizacionServiciosID { get; set; }

        public static List<detalleServicioCotizacionServicios> obtenerDetalle(int IDCotizacion, string categoria)
        {
            Context db = new Context();

            return db.detalleServiciosCotizacionServicios.Where(s => s.CotizacionServiciosID == IDCotizacion & s.categoria == categoria).ToList();            
        }
    }
}