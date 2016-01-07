using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace RentaMaq.Models
{
    public class pedidos
    {
        public pedidos() {
            detallePedidos = new List<detallePedido>();
        }
        public int pedidosID { get; set; }
        public DateTime fecha { set; get; }
        public string nota { set; get; }
        public string estado { set; get; }
        public int idOT { set; get; }

        public virtual ICollection<detallePedido> detallePedidos { get; set; }
    }

    public class detallePedido {

        public int detallePedidoID { get; set; }
        public string numeroParte { set; get; }
        public double cantidad { set; get; }
        
        public string tipoPedido { set; get; } //stock o directa
        public string detalleTipoPedido { set; get; } ///codigo del equipo si es directa ...
        public string prioridad { set; get; }
        public string descripcion { set; get; }


        public int pedidosID { set; get; }
        [ForeignKey("pedidosID")]

        public virtual pedidos pedidos { get; set; }
    
    }
}