using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace RentaMaq.Models
{
    public class listaCorreosStockCritico
    {
        public int ID { set; get; }
        public string nombre { set; get; }
        public string correo { set;get;}
        public static void eliminarTodos()
        {

            SqlConnection con = conexion.crearConexion();
            con.Open();

            using (SqlCommand command = new SqlCommand("DELETE FROM listaCorreosStockCritico", con))
            {                
                command.ExecuteNonQuery();
            }

            con.Close();

        }



        public static List<listaCorreosStockCritico> obtenerTodos()
        {
            SqlConnection con = conexion.crearConexion();
            con.Open();
            List<listaCorreosStockCritico> lista = new List<listaCorreosStockCritico>();
            using (SqlCommand command = new SqlCommand("SELECT * FROM listaCorreosStockCritico", con))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        listaCorreosStockCritico Persona = new listaCorreosStockCritico();
                        Persona.nombre = reader[1].ToString();
                        Persona.correo = reader[2].ToString();
                        lista.Add(Persona);
                    }
                }
            }

            con.Close();
            return lista;
        }

        public static List<string> obtenerTodosCorreos()
        {
            SqlConnection con = conexion.crearConexion();
            con.Open();
            List<string> lista = new List<string>();
            using (SqlCommand command = new SqlCommand("SELECT * FROM listaCorreosStockCritico", con))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                                                                      
                        lista.Add(reader[2].ToString());
                    }
                }
            }

            con.Close();
            return lista;
        }

        public static void guardarPersona(string nombre, string correo)
        {

            SqlConnection con = conexion.crearConexion();
            con.Open();

            using (SqlCommand command = new SqlCommand("INSERT INTO listaCorreosStockCritico"
                + "(nombre, correo) "
                + "VALUES(@nombre, @correo)", con))
            {
                command.Parameters.Add("@nombre", SqlDbType.VarChar).Value = nombre;
                command.Parameters.Add("@correo", SqlDbType.VarChar).Value = correo;                
               command.ExecuteNonQuery();
            }

            con.Close();
        }

    }
}