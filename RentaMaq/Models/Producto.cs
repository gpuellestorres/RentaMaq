using RentaMaq.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentaMaq.Models
{
    public class Producto
    {
        public int ProductoID { get; set; }
        public string numeroDeParte { set; get; }
        public int precioUnitario { get; set; }
        public string descripcion { get; set; }
        public string descripcion2 { get; set; }
        public double stockActual { get; set; }
        public double stockMinimo { get; set; }
        public string unidadDeMedida { get; set; }
        public string ubicacion { get; set; }
        public string codigo { get; set; }
        public int idBodega { set; get; }
        
        public Producto() { 
        
        }

        public string verificarStockCritico()
        {   
            
            double diferencia = this.stockActual - this.stockMinimo;
            if (diferencia > 0)
                return "false";
            else
                return "true";
        }
        public static int cantidadProductosStockCritico() {
            Context db = new Context();
            var cantidad = db.Productos.Where(s => s.stockActual <= s.stockMinimo).Count();
            return Convert.ToInt32(cantidad);
        }
        public static List<Producto> listaProductosStockCritico()
        {
            Context db = new Context();
            List<Producto> lista = db.Productos.Where(s => s.stockActual <= s.stockMinimo).ToList();
            return lista;
        }
    }


    public class ProductoReport{
        public int ProductoID { get; set; }
        public string numeroDeParte { set; get; }
        public string precioUnitario { get; set; }
        public string descripcion { get; set; }
        public string descripcion2 { get; set; }
        public double stockActual { get; set; }
        public double stockMinimo { get; set; }
        public string unidadDeMedida { get; set; }
        public string ubicacion { get; set; }
        public string codigo { get; set; }
        public string fechaActual { get; set; }

        public ProductoReport(Producto Prod) {
            this.ProductoID = Prod.ProductoID;
            this.numeroDeParte = Prod.numeroDeParte;
            this.precioUnitario = Formateador.valoresPesos(Prod.precioUnitario);
            this.descripcion = Prod.descripcion;
            this.descripcion2 = Prod.descripcion2;
            this.stockActual = Prod.stockActual;
            this.stockMinimo = Prod.stockMinimo;
            this.unidadDeMedida = Prod.unidadDeMedida;
            this.ubicacion = Prod.ubicacion;
            this.codigo = Prod.codigo;
            this.fechaActual = DateTime.Now.ToString("d");

        }
    
    }

    
}