using RentaMaq.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace RentaMaq.Models
{
    public class reporteConsumoCombustible
    {
        reportCombustible Report { get; set; }
        public double horasTrabajo { get; set; }
        public double kilometrosTrabajo { get; set; }
        public double rendimientoLitrosHora { get; set; }
        public double rendimientoKilometrosLitro { get; set; }

        public reporteConsumoCombustible(reportCombustible Report) 
        {
            this.Report = Report;
            this.horasTrabajo = this.obtenerHorasTrabajo();
            this.kilometrosTrabajo = this.obtenerKilometrosTrabajo();
            if (this.horasTrabajo == 0)
            {
                this.rendimientoLitrosHora = 0;
            }
            else
            {
                this.rendimientoLitrosHora = this.Report.litros / this.horasTrabajo;
            }
            this.rendimientoKilometrosLitro = this.kilometrosTrabajo / this.Report.litros;
        }

        private double obtenerKilometrosTrabajo()
        {
            double retorno = this.Report.kilometraje;
            Context db = new Context();
            SqlConnection con = conexion.crearConexion();
            con.Open();

            using (SqlCommand command = new SqlCommand("SELECT TOP 1 kilometraje FROM reportCombustible "
                + "WHERE fechaHora<@fechaHora AND equiposID_ID=@equiposID ORDER BY fechaHora DESC", con))
            {
                command.Parameters.Add("@fechaHora", SqlDbType.DateTime).Value = this.Report.fechaHora;
                command.Parameters.Add("@equiposID", SqlDbType.Int).Value = this.Report.equiposID.ID;
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (!reader.HasRows) 
                    {
                        retorno = 0; 
                    }
                    while (reader.Read())
                    {
                        retorno -= double.Parse(reader[0].ToString());
                    }
                }
            }

            con.Close();
            return retorno;
        }

        private double obtenerHorasTrabajo()
        {
            double retorno = this.Report.horometro;
            Context db = new Context();
            SqlConnection con = conexion.crearConexion();
            con.Open();

            using (SqlCommand command = new SqlCommand("SELECT TOP 1 horometro FROM reportCombustible "
                + "WHERE fechaHora<@fechaHora AND equiposID_ID=@equiposID ORDER BY fechaHora DESC", con))
            {
                command.Parameters.Add("@fechaHora", SqlDbType.DateTime).Value = this.Report.fechaHora;
                command.Parameters.Add("@equiposID", SqlDbType.Int).Value = this.Report.equiposID.ID;
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (!reader.HasRows)
                    {
                        retorno = 0;
                    }
                    while (reader.Read())
                    {
                        retorno -= double.Parse(reader[0].ToString());
                    }
                }
            }

            con.Close();
            return retorno;
        }
    }
}