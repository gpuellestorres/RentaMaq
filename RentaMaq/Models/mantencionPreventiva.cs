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
    public class mantencionPreventiva
    {
        public int mantencionPreventivaID { get; set; }
        [Display(Name="Equipo")]
        public int equipoID { get; set; }
        [Display(Name="Fecha")]
        public DateTime fecha  { get; set; }
        [Display(Name="Horómetro actual")]
        public int horometroActual { get; set; }
        [Display(Name="Kilometraje actual")]
        public int kilometrajeActual { get; set; }
        [Display(Name="Horómetro de próxima mantención")]
        public int horometroProximaMantencion { get; set; }
        [Display(Name="Kilometraje de próxima mantención")]
        public int kilometrajeProximaMantencion { get; set; }
        [Display(Name="Nota")]
        public string nota { get; set; }

        internal static mantencionPreventiva obtenerUltima(int ID)
        {
            mantencionPreventiva retorno = new mantencionPreventiva();
            SqlConnection con = conexion.crearConexion();
            con.Open();

            using (SqlCommand command = new SqlCommand("SELECT TOP 1 mantencionPreventivaID FROM mantencionPreventiva "
                + "WHERE equipoID=@equipoID ORDER BY fecha DESC", con))
            {
                command.Parameters.Add("@equipoID", SqlDbType.Int).Value = ID;
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        retorno = new Context().mantencionPreventivas.Find(int.Parse(reader[0].ToString()));
                    }
                }
            }

            con.Close();
            return retorno;
        }

        public static void reemplazar(mantencionPreventiva datos) 
        {
            Context db = new Context();

            db.mantencionPreventivas.RemoveRange(db.mantencionPreventivas.Where(s => s.equipoID == datos.equipoID && s.fecha == datos.fecha));
            db.SaveChanges();

            db.mantencionPreventivas.Add(datos);
            db.SaveChanges();
        }
    }
}