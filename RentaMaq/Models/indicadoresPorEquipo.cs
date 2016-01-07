using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RentaMaq.Models;

using System.Data;
using System.Data.SqlClient;


namespace RentaMaq.Models
{
    public class indicadoresPorEquipo
    {
        public string nombreEquipo { set; get; }
        public string nombreProducto { set; get; }
        public string codigoProducto { set; get; }
        public double cantidadProducto { set; get; }
        public DateTime fecha { set; get; }        

        public static List<indicadoresPorEquipo> obtenerProductosPorEquipo(string nombreEquipo,DateTime fechaInicial, DateTime fechaFinal){
            List<indicadoresPorEquipo> retorno = new List<indicadoresPorEquipo>();
            SqlConnection con = conexion.crearConexion();
            con.Open();

            using (SqlCommand command = new SqlCommand("SELECT afiEquipo,ProductoID,descripcionProducto, SUM(cantidadSaliente) AS sumaProductos FROM Maestro WHERE cantidadSaliente>0 AND afiEquipo=@afiEquipo AND fecha>=@fechaInicial AND fecha<=@fechaFinal GROUP BY afiEquipo,ProductoID,descripcionProducto", con))
            {
                command.Parameters.Add("@afiEquipo", SqlDbType.VarChar).Value = nombreEquipo;
                command.Parameters.Add("@fechaInicial", SqlDbType.DateTime).Value = fechaInicial;
                command.Parameters.Add("@fechaFinal", SqlDbType.DateTime).Value = fechaFinal;
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        indicadoresPorEquipo nuevo = new indicadoresPorEquipo();
                        nuevo.nombreEquipo = reader["afiEquipo"].ToString();
                        nuevo.nombreProducto = reader["descripcionProducto"].ToString();
                        nuevo.codigoProducto = reader["ProductoID"].ToString();
                        nuevo.cantidadProducto = Convert.ToDouble(reader["sumaProductos"].ToString());

                        retorno.Add(nuevo);
                    }
                }
            }
            con.Close();
            return retorno;          
        }


        public static List<indicadoresPorEquipo> obtenerProductosPorEquipoSinIntervalorFecha(string nombreEquipo)
        {
            List<indicadoresPorEquipo> retorno = new List<indicadoresPorEquipo>();
            SqlConnection con = conexion.crearConexion();
            con.Open();

            using (SqlCommand command = new SqlCommand("SELECT * FROM Maestro WHERE cantidadSaliente>0 AND afiEquipo=@afiEquipo", con))
            {
                command.Parameters.Add("@afiEquipo", SqlDbType.VarChar).Value = nombreEquipo;

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {

                        //formatearFechaCompleta
                        indicadoresPorEquipo nuevo = new indicadoresPorEquipo();
                        nuevo.nombreEquipo = reader["afiEquipo"].ToString();
                        nuevo.nombreProducto = reader["descripcionProducto"].ToString();
                        nuevo.codigoProducto = reader["ProductoID"].ToString();
                        nuevo.cantidadProducto = Convert.ToDouble(reader["cantidadSaliente"].ToString());
                        nuevo.fecha = (DateTime)reader["fecha"];                        
                        retorno.Add(nuevo);
                    }
                }
            }
            con.Close();
            return retorno;
        }


        public static List<indicadoresPorEquipo> obtenerProductosPorEquipoConIntervalorFecha(string nombreEquipo, DateTime fechaInicial, DateTime fechaFinal)
        {
            List<indicadoresPorEquipo> retorno = new List<indicadoresPorEquipo>();
            SqlConnection con = conexion.crearConexion();
            con.Open();

            using (SqlCommand command = new SqlCommand("SELECT * FROM Maestro WHERE cantidadSaliente>0 AND afiEquipo=@afiEquipo AND fecha>=@fechaInicial AND fecha<=@fechaFinal", con))
            {
                command.Parameters.Add("@afiEquipo", SqlDbType.VarChar).Value = nombreEquipo;
                command.Parameters.Add("@fechaInicial", SqlDbType.DateTime).Value = fechaInicial;
                command.Parameters.Add("@fechaFinal", SqlDbType.DateTime).Value = fechaFinal;

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {

                        //formatearFechaCompleta
                        indicadoresPorEquipo nuevo = new indicadoresPorEquipo();
                        nuevo.nombreEquipo = reader["afiEquipo"].ToString();
                        nuevo.nombreProducto = reader["descripcionProducto"].ToString();
                        nuevo.codigoProducto = reader["ProductoID"].ToString();
                        nuevo.cantidadProducto = Convert.ToDouble(reader["cantidadSaliente"].ToString());
                        nuevo.fecha = (DateTime)reader["fecha"];
                        retorno.Add(nuevo);
                    }
                }
            }
            con.Close();
            return retorno;
        }

         public static List<indicadoresPorEquipo> obtenerProductosTodosEquiposConIntervalorFecha(DateTime fechaInicial, DateTime fechaFinal)
        {
            List<indicadoresPorEquipo> retorno = new List<indicadoresPorEquipo>();
            SqlConnection con = conexion.crearConexion();
            con.Open();
            using (SqlCommand command = new SqlCommand("SELECT afiEquipo,ProductoID,descripcionProducto, SUM(cantidadSaliente) AS sumaProductos FROM Maestro WHERE cantidadSaliente>0 AND fecha>=@fechaInicial AND fecha<=@fechaFinal GROUP BY afiEquipo,ProductoID,descripcionProducto", con))
           // using (SqlCommand command = new SqlCommand("SELECT * FROM Maestro WHERE cantidadSaliente>0 AND fecha>=@fechaInicial AND fecha<=@fechaFinal", con))
            {                
                command.Parameters.Add("@fechaInicial", SqlDbType.DateTime).Value = fechaInicial;
                command.Parameters.Add("@fechaFinal", SqlDbType.DateTime).Value = fechaFinal;

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {

                        //formatearFechaCompleta
                        indicadoresPorEquipo nuevo = new indicadoresPorEquipo();
                        nuevo.nombreEquipo = reader["afiEquipo"].ToString();
                        nuevo.nombreProducto = reader["descripcionProducto"].ToString();
                        nuevo.codigoProducto = reader["ProductoID"].ToString();
                        nuevo.cantidadProducto = Convert.ToDouble(reader["sumaProductos"].ToString());
                        //nuevo.fecha = Formateador.fechaStringToDateTime(reader["fecha"].ToString());
                        retorno.Add(nuevo);
                    }
                }
            }
            con.Close();
            return retorno;
        }


        public static List<indicadoresPorEquipo> obtenerTodosConsumos(){
            List<indicadoresPorEquipo> retorno = new List<indicadoresPorEquipo>();

            SqlConnection con = conexion.crearConexion();
            con.Open();

            using (SqlCommand command = new SqlCommand("SELECT afiEquipo,ProductoID,descripcionProducto, SUM(cantidadSaliente) AS sumaProductos FROM Maestro WHERE cantidadSaliente>0 GROUP BY afiEquipo,ProductoID,descripcionProducto", con))

            //command.Parameters.Add("@ID", SqlDbType.Int).Value = ID;
            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {    
                    indicadoresPorEquipo nuevo = new indicadoresPorEquipo();
                    nuevo.nombreEquipo = reader["afiEquipo"].ToString();                    
                    nuevo.nombreProducto = reader["descripcionProducto"].ToString();
                    nuevo.codigoProducto = reader["ProductoID"].ToString();
                    nuevo.cantidadProducto = Convert.ToDouble(reader["sumaProductos"].ToString());
                    if (!nuevo.nombreEquipo.Equals("")) 
                        retorno.Add(nuevo);
                }
            }

            con.Close();           

            return retorno;
        }       
    }
}