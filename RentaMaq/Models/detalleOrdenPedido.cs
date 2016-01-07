using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentaMaq.Models
{
    public class detalleOrdenPedido
    {
        public int detalleOrdenPedidoID { get; set; }
        public string detalle { get; set; }
        public int cantidad { get; set; }
        public int OrdenDePedidoID { get; set; }
    }
}