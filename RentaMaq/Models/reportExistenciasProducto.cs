using RentaMaq.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace RentaMaq.Models
{
    public class reportExistenciasProductos
    {
        public DateTime Inicio { get; set; }
        public DateTime Termino { get; set; }
        public Producto Producto { get; set; }
        public double stockDisponible { get; set; }
        public double stockDisponibleValorizado { get; set; }
        public double stockProductosIngresadosPeriodo { get; set; }
        public double stockProductosSalientesPeriodo { get; set; }

        public reportExistenciasProductos(Producto Prod, DateTime inicio, DateTime termino) 
        {
            if (Prod == null) return;

            this.Producto = Prod;
            this.Inicio = inicio;
            this.Termino = termino;
            this.stockDisponible = Prod.stockActual;
            this.stockDisponibleValorizado = Prod.precioUnitario * stockDisponible;
            this.stockProductosIngresadosPeriodo = this.obtenerProductosIngresadosPeriodo();
            this.stockProductosSalientesPeriodo = this.obtenerProductosSalientesPeriodo();
            
        }

        private double obtenerProductosSalientesPeriodo()
        {
            double retorno = 0;
            Context db = new Context();
            SqlConnection con = conexion.crearConexion();
            con.Open();

            List<Maestro> listaMaestros = db.Maestros.Where(s => s.ProductoID == this.Producto.numeroDeParte && s.fecha <= this.Termino && s.fecha >= this.Inicio).ToList();

            /*using (SqlCommand command = new SqlCommand("SELECT cantidadSaliente FROM Maestro WHERE ProductoID=@ProductoID "
                + "AND fecha<=@Termino AND fecha>=@Inicio", con))
            {
                command.Parameters.Add("@ProductoID", SqlDbType.NVarChar).Value = Producto.ProductoID;
                command.Parameters.Add("@Inicio", SqlDbType.DateTime).Value = this.Inicio;
                command.Parameters.Add("@Termino", SqlDbType.DateTime).Value = this.Termino;
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                        retorno += int.Parse(reader[0].ToString());
                }
            }
            //*/

            foreach (Maestro Ms in listaMaestros) 
            {
                retorno += Ms.cantidadSaliente;
            }

            con.Close();

            return retorno;
        }

        private int obtenerProductosIngresadosPeriodo()
        {
            int retorno = 0;
            Context db = new Context();
            SqlConnection con = conexion.crearConexion();
            con.Open();

            using (SqlCommand command = new SqlCommand("SELECT cantidadEntrante FROM Maestro WHERE ProductoID=@ProductoID "
                + "AND fecha<=@Termino AND fecha>=@Inicio", con))
            {
                command.Parameters.Add("@ProductoID", SqlDbType.NVarChar).Value = Producto.numeroDeParte;
                command.Parameters.Add("@Inicio", SqlDbType.DateTime).Value = this.Inicio;
                command.Parameters.Add("@Termino", SqlDbType.DateTime).Value = this.Termino;
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                        retorno += int.Parse(reader[0].ToString());
                }
            }

            con.Close();

            return retorno;
        }
    }

    public class reportExistenciasReporte {
        public string NumeroDeParte { get; set; }
        public string descripcionProducto { get; set; }
        public double StockDisponible { get; set; }
        public DateTime Inicio { get; set; }
        public DateTime Termino { get; set; }
        public double stockDisponibleValorizado { get; set; }
        public double stockProductosIngresadosPeriodo { get; set; }
        public double stockProductosSalientesPeriodo { get; set; }

        public reportExistenciasReporte(reportExistenciasProductos Rep) 
        {
            this.NumeroDeParte = Rep.Producto.numeroDeParte;
            this.descripcionProducto = Rep.Producto.descripcion;
            this.StockDisponible = Rep.stockDisponible;
            this.Inicio = Rep.Inicio;
            this.Termino = Rep.Termino;
            this.stockDisponibleValorizado = Rep.stockDisponibleValorizado;
            this.stockProductosIngresadosPeriodo = Rep.stockProductosIngresadosPeriodo;
            this.stockProductosSalientesPeriodo = Rep.stockProductosSalientesPeriodo;
        }
    }
}