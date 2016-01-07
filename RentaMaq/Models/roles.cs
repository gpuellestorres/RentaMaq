using RentaMaq.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace RentaMaq.Models
{
    public class roles
    {
        public int ID {get;set;}
        public string nombre{get;set;}
        public string descripcion{get;set;}

        public static List<roles> obtenerListaRoles()
        {
            List<roles> lista = new List<roles>();
            SqlConnection con = conexion.crearConexion();
            con.Open();

            using (SqlCommand command = new SqlCommand("SELECT * FROM roles ORDER BY ID ASC", con))
            {
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read()) 
                {
                    roles rol = new roles();

                    rol.ID = int.Parse(reader["ID"].ToString());
                    rol.nombre = reader["nombre"].ToString();
                    rol.descripcion = reader["descripcion"].ToString();

                    lista.Add(rol);
                }
            }
            con.Close();

            return lista;
        }

        public static bool tienePermiso(int IDRol, int IDUsuario)
        {
            bool retorno = false;

            SqlConnection con = conexion.crearConexion();
            con.Open();

            using (SqlCommand command = new SqlCommand("SELECT * FROM roles_usuarios "
                + " WHERE usuarioID=@usuarioID AND rolID=@rolID", con))
            {
                command.Parameters.Add("@usuarioID", SqlDbType.Int).Value = IDUsuario;
                command.Parameters.Add("@rolID", SqlDbType.Int).Value = IDRol;

                SqlDataReader reader = command.ExecuteReader();

                retorno = reader.HasRows;
            }
            con.Close();

            return retorno;
        }

        internal static void agregarPermiso(int IDRol, int idUsuario)
        {
            SqlConnection con = conexion.crearConexion();
            con.Open();

            using (SqlCommand command = new SqlCommand("INSERT INTO roles_usuarios "
                + " VALUES(@usuarioID,@rolID)", con))
            {
                command.Parameters.Add("@usuarioID", SqlDbType.Int).Value = idUsuario;
                command.Parameters.Add("@rolID", SqlDbType.Int).Value = IDRol;

                command.ExecuteNonQuery();
            }
            con.Close();
        }

        internal static void eliminarPermisos(int idUsuario)
        {
            SqlConnection con = conexion.crearConexion();
            con.Open();

            using (SqlCommand command = new SqlCommand("DELETE FROM roles_usuarios "
                + " WHERE usuarioID=@usuarioID", con))
            {
                command.Parameters.Add("@usuarioID", SqlDbType.Int).Value = idUsuario;

                command.ExecuteNonQuery();
            }
            con.Close();
        }

        public static bool tienePermiso(int IDUsuario, string tipoPermiso) 
        {
            int idRol = obtenerIDPermiso(tipoPermiso);

            return tienePermiso(idRol, IDUsuario);
        }

        private static int obtenerIDPermiso(string nombrePermiso)
        {
            int retorno = -1;
            SqlConnection con = conexion.crearConexion();
            con.Open();

            using (SqlCommand command = new SqlCommand("SELECT ID FROM roles WHERE nombre=@nombre", con))
            {
                command.Parameters.Add("@nombre", SqlDbType.NVarChar).Value = nombrePermiso;
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    retorno = int.Parse(reader["ID"].ToString());
                }
            }
            con.Close();

            return retorno;
        }

        internal static List<string> correosAlertaPedido()
        {
            List<string> retorno = new List<string>();
            List<int> lista = new List<int>();

            SqlConnection con = conexion.crearConexion();
            con.Open();

            using (SqlCommand command = new SqlCommand("SELECT usuarioID FROM roles_usuarios WHERE rolID='17'", con))
            {
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    roles rol = new roles();

                    lista.Add(int.Parse(reader["usuarioID"].ToString()));
                }
            }
            con.Close();

            Context db = new Context();

            foreach (int IDUsuario in lista) 
            {
                retorno.Add(db.Usuarios.Find(IDUsuario).correoElectronico);
                if (string.IsNullOrEmpty(retorno[retorno.Count - 1])) retorno.RemoveAt(retorno.Count - 1);
            }
            return retorno;
        }
    }
}