using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RentaMaq.Models
{
    public class ordenPedidoCombustible
    {
        public ordenPedidoCombustible() {
            detalleOrdenPedidoCombustible = new List<detalleOrdenPedidoCombustible>();
        }

        public int ordenPedidoCombustibleID { set; get; }
        public string numeroOrdenPedido { set; get; }
        public string destinatario { set; get; }
        public DateTime fecha { set; get; }
        public string nombreQuienAutoriza { set; get; }
        public int anio { set; get; }
        public string encabezado { set; get; }
        public virtual ICollection<detalleOrdenPedidoCombustible> detalleOrdenPedidoCombustible { get; set; }
    }

    public class detalleOrdenPedidoCombustible {
        public int detalleOrdenPedidoCombustibleID { set; get; }
        public string trabajoRealizar { set; get; }
        public string personaResponsable { set; get; }
        public string detalle { set; get; }
        public string cantidad { set; get; }

        public int ordenPedidoCombustibleID { set; get; }
        [ForeignKey("ordenPedidoCombustibleID")]

        public virtual ordenPedidoCombustible ordenPedidoCombustible { get; set; }
    }


    public class ReporteOrdenPedidoCombustible{
        
        public string numeroOrdenPedido { set; get; }
        public string destinatario { set; get; }
        public string fecha { set; get; }
        public string nombreQuienAutoriza { set; get; }
        public int anio { set; get; }
        public string encabezado { set; get; }

        //DETALLE
        public string trabajoRealizar { set; get; }
        public string personaResponsable { set; get; }
        public string detalle { set; get; }
        public string cantidad { set; get; }
        public string pathImageFirma { set; get; }


        public ReporteOrdenPedidoCombustible(ordenPedidoCombustible ordenPedidoCombustible, detalleOrdenPedidoCombustible detalleOrdenPedidoCombustible, string imagePath)
        { 
            this.numeroOrdenPedido =ordenPedidoCombustible.numeroOrdenPedido;
            this.destinatario  = ordenPedidoCombustible.destinatario;
            this.fecha = ordenPedidoCombustible.fecha.ToString("D").Split(',')[1];
            this.nombreQuienAutoriza =ordenPedidoCombustible.nombreQuienAutoriza;    
    
            this.trabajoRealizar = detalleOrdenPedidoCombustible.trabajoRealizar;
            this.personaResponsable = detalleOrdenPedidoCombustible.personaResponsable;
            this.detalle = detalleOrdenPedidoCombustible.detalle;
            this.cantidad = detalleOrdenPedidoCombustible.cantidad;
            this.anio = ordenPedidoCombustible.anio;
            this.encabezado = ordenPedidoCombustible.encabezado;
            this.pathImageFirma = imagePath;


        }
        

    }
}