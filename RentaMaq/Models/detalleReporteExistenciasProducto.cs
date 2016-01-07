using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentaMaq.Models
{
    public class detalleReporteExistenciasProducto
    {
        

        public detalleReporteExistenciasProducto(Maestro Ms, DateTime inicio, DateTime termino)
        {
            // TODO: Complete member initialization
            this.ProductoID = Ms.ProductoID;
            this.descripcionProducto = Ms.descripcionProducto;
            this.MaestroID = Ms.MaestroID.ToString();
            this.fechaMaestro = Ms.fecha;
            this.Inicio = inicio;
            this.Termino = termino;
            this.stockIngresado = Ms.cantidadEntrante;
            this.stockSaliente = Ms.cantidadSaliente;
        }
        public string ProductoID { get; set; }
        public string descripcionProducto { get; set; }
        public string MaestroID { get; set; }
        public DateTime fechaMaestro { get; set; }
        public DateTime Inicio { get; set; }
        public DateTime Termino { get; set; }
        public double stockIngresado { get; set; }
        public double stockSaliente { get; set; }


    }
}