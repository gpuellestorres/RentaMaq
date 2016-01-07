using RentaMaq.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace RentaMaq.Models
{
    public class Marca
    {
        public int MarcaID { get; set; }
        [Display(Name="Marca")]
        public string NombreMarca { get; set; }

        internal void guardar()
        {
            if (!existe())
            {
                insertar();
            }
            else 
            {
                actualizar();
            }
        }

        private void actualizar()
        {
            Context db = new Context();
            SqlConnection con = conexion.crearConexion();
            con.Open();

            using (SqlCommand command = new SqlCommand("UPDATE Marca SET NombreMarca=@NombreMarca WHERE MarcaID=@MarcaID", con))
            {
                command.Parameters.Add("@MarcaID", SqlDbType.Int).Value = this.MarcaID;
                command.Parameters.Add("@NombreMarca", SqlDbType.NVarChar).Value = this.NombreMarca;

                command.ExecuteNonQuery();
            }

            con.Close();
        }

        private void insertar()
        {
            Context db = new Context();
            SqlConnection con = conexion.crearConexion();
            con.Open();

            using (SqlCommand command = new SqlCommand("INSERT INTO Marca(NombreMarca) VALUES(@NombreMarca)", con))
            {
                //command.Parameters.Add("@MarcaID", SqlDbType.Int).Value = this.MarcaID;
                command.Parameters.Add("@NombreMarca", SqlDbType.NVarChar).Value = this.NombreMarca;

                command.ExecuteNonQuery();
            }

            con.Close();
        }

        private bool existe()
        {
            bool retorno = false;
            Context db = new Context();
            SqlConnection con = conexion.crearConexion();
            con.Open();

            using (SqlCommand command = new SqlCommand("SELECT * FROM Marca WHERE MarcaID=@MarcaID", con))
            {
                command.Parameters.Add("@MarcaID", SqlDbType.Int).Value = this.MarcaID;
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    retorno = reader.HasRows;
                }
            }

            con.Close();
            return retorno;        
        }


        public static bool SeUsa(int MarcaID)
        {
            bool retorno = false;
            Context db = new Context();
            SqlConnection con = conexion.crearConexion();
            con.Open();

            using (SqlCommand command = new SqlCommand("SELECT * FROM Modelo WHERE MarcaID_MarcaID=@MarcaID", con))
            {
                command.Parameters.Add("@MarcaID", SqlDbType.Int).Value = MarcaID;
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    retorno = reader.HasRows;
                }
            }

            con.Close();
            return retorno;
        }
    }
}