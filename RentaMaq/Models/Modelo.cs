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
    public class Modelo
    {
        [Display(Name = "Marca")]
        public Marca MarcaID { get; set; }
        [Display]
        public string ModeloID { get; set; }
        [Display(Name="Modelo")]
        public string nombreModelo { get; set; }

        internal static List<Modelo> Todos()
        {
            Context db = new Context();
            SqlConnection con = conexion.crearConexion();
            con.Open();
            List<Modelo> retorno = new List<Modelo>();

            using (SqlCommand command = new SqlCommand("SELECT * FROM Modelo", con))
            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Modelo nuevo = new Modelo();
                    nuevo.ModeloID = reader["ModeloID"].ToString();
                    nuevo.MarcaID = db.Marcas.Find(int.Parse(reader["MarcaID_MarcaID"].ToString()));
                    nuevo.nombreModelo = reader["nombreModelo"].ToString();

                    retorno.Add(nuevo);
                }
            }

            con.Close();
            return retorno;
        }

        internal static Modelo Obtener(string modelo)
        {
            Modelo retorno = new Modelo();

            Context db = new Context();
            SqlConnection con = conexion.crearConexion();
            con.Open();

            using (SqlCommand command = new SqlCommand("SELECT * FROM Modelo WHERE ModeloID=@modelo", con))
            {
                command.Parameters.Add("@modelo", SqlDbType.NVarChar).Value = modelo;
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        retorno.ModeloID = reader["ModeloID"].ToString();
                        retorno.MarcaID = db.Marcas.Find(int.Parse(reader["MarcaID_MarcaID"].ToString()));
                        retorno.nombreModelo = reader["nombreModelo"].ToString();
                    }
                }
            }

            con.Close();
            return retorno;
        }

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

            using (SqlCommand command = new SqlCommand("UPDATE Modelo SET MarcaID_MarcaID=@MarcaID, nombreModelo=@nombreModelo WHERE ModeloID=@ModeloID", con))
            {
                command.Parameters.Add("@ModeloID", SqlDbType.NVarChar).Value = this.ModeloID;
                command.Parameters.Add("@MarcaID", SqlDbType.Int).Value = this.MarcaID.MarcaID;
                command.Parameters.Add("@nombreModelo", SqlDbType.NVarChar).Value = this.nombreModelo;

                command.ExecuteNonQuery();
            }

            con.Close();
        }

        private void insertar()
        {
            Context db = new Context();
            SqlConnection con = conexion.crearConexion();
            con.Open();

            using (SqlCommand command = new SqlCommand("INSERT INTO Modelo(ModeloID ,MarcaID_MarcaID, nombreModelo) "
                + "VALUES(@ModeloID, @NombreMarca, @nombreModelo)", con))
            {
                command.Parameters.Add("@ModeloID", SqlDbType.Int).Value = this.ModeloID;
                command.Parameters.Add("@MarcaID", SqlDbType.Int).Value = this.MarcaID.MarcaID;
                command.Parameters.Add("@nombreModelo", SqlDbType.NVarChar).Value = this.nombreModelo;

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

            using (SqlCommand command = new SqlCommand("SELECT * FROM Modelo WHERE ModeloID=@ModeloID", con))
            {
                command.Parameters.Add("@ModeloID", SqlDbType.VarChar).Value = this.ModeloID;
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    retorno = reader.HasRows;
                }
            }

            con.Close();
            return retorno;
        }

        public static bool SeUsa(string ModeloID)
        {
            bool retorno = false;
            Context db = new Context();
            SqlConnection con = conexion.crearConexion();
            con.Open();

            using (SqlCommand command = new SqlCommand("SELECT * FROM equipos WHERE ModeloID_ModeloID=@ModeloID", con))
            {
                command.Parameters.Add("@ModeloID", SqlDbType.VarChar).Value = ModeloID;
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