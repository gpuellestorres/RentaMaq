using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentaMaq.Models
{
    public class detalleEquiposCotizacionServicios
    {
        public int detalleEquiposCotizacionServiciosID { get; set; }
        public string equipo { get; set; }
        public int cantidad { get; set; }
        public int CotizacionServiciosID { get; set; }
    }
}