using RentaMaq.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace RentaMaq.Models
{
    public class Maestro
    {
        public int MaestroID { get; set; }
        public DateTime fecha { get; set; }
        public string ProductoID { set; get; }
        public string descripcionProducto { set; get; }
        public double cantidadEntrante { set; get; }
        public double cantidadSaliente { set; get; }
        public string facturaDespacho { set; get; }
        public string proveedor { set; get; }
        public int valorUnitario { set; get; }
        public int valorTotal { set; get; }
        public string afiEquipo { set; get; }
        public string entragadoA { set; get; }
        public string motivo { set; get; }
        public string numeroFormulario { set; get; }
        public string observaciones { set; get; }
        public int idOT { set; get; }
       
        internal static List<string> ObtenerIdsProductosPeriodo(DateTime Inicio, DateTime Termino)
        {
            List<string> retorno = new List<string>();

            Context db = new Context();
            SqlConnection con = conexion.crearConexion();
            con.Open();

            using (SqlCommand command = new SqlCommand("SELECT DISTINCT ProductoID FROM Maestro WHERE fecha<=@Termino AND fecha>=@Inicio", con))
            {
                command.Parameters.Add("@Inicio", SqlDbType.DateTime).Value = Inicio;
                command.Parameters.Add("@Termino", SqlDbType.DateTime).Value = Termino;
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                        retorno.Add(reader[0].ToString());
                }
            }

            con.Close();
            return retorno;
        }
    }

    public class MaestroJSon
    {
        public int MaestroID { get; set; }
        public string fecha { get; set; }
        public string ProductoID { set; get; }
        public string descripcionProducto { set; get; }
        public double cantidadEntrante { set; get; }
        public double cantidadSaliente { set; get; }
        public string facturaDespacho { set; get; }
        public string proveedor { set; get; }
        public int valorUnitario { set; get; }
        public int valorTotal { set; get; }
        public string afiEquipo { set; get; }
        public string entragadoA { set; get; }
        public string motivo { set; get; }
        public string numeroFormulario { set; get; }
        public string observaciones { set; get; }

        public static List<MaestroJSon> convertirLista(List<Maestro> lista) {
            List<MaestroJSon> retorno = new List<MaestroJSon>();

            foreach(Maestro dato in lista)
            {
                MaestroJSon nuevo = new MaestroJSon();
                retorno.Add(nuevo);
            }

            return retorno;
        }
    }
}