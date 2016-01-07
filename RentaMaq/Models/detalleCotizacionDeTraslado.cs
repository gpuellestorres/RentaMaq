using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentaMaq.Models
{
    public class detalleCotizacionDeTraslado
    {
        public int detalleCotizacionDeTrasladoID { get; set; }
        public string codigo { get; set; }
        public string descripcion { get; set; }
        public int cantidad { get; set; }
        public int precioUnitario { get; set; }
        public int total { get; set; }
        public int IDCotizacionTraslado { get; set; }
    }
}