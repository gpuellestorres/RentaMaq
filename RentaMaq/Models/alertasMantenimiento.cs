using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace RentaMaq.Models
{
    public class alertasMantenimiento
    {
        public int alertasMantenimientoID { get; set; }
        [Display(Name="Nombre")]
        public string nombre { get; set; }
        [Display(Name = "Correo")]
        public string correo { get; set; }

        internal static int diasDesdeUltimaAlerta()
        {
            DateTime fecha = DateTime.Now.AddMonths(-12);

            SqlConnection con = conexion.crearConexion();
            con.Open();

            using (SqlCommand command = new SqlCommand("SELECT TOP 1 fecha FROM controlAlertasMantenimiento ORDER BY fecha DESC", con))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {                        
                        fecha = (DateTime)reader[0];
                    }
                }
            }

            con.Close();

            return int.Parse(Math.Truncate((DateTime.Now - fecha).TotalDays).ToString());
        }

        internal static void registrarEnvio()
        {
            SqlConnection con = conexion.crearConexion();
            con.Open();

            using (SqlCommand command = new SqlCommand(
                "INSERT INTO controlAlertasMantenimiento VALUES(@fecha)", con))
            {
                command.Parameters.Add("@fecha", SqlDbType.DateTime).Value = DateTime.Now;
                command.ExecuteNonQuery();                
            }

            con.Close();
        }
    }
}