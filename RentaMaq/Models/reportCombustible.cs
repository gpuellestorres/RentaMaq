using RentaMaq.DAL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace RentaMaq.Models
{
    public class reportCombustible
    {
        public int ID { get; set; }
        [Display(Name = "Número de Report")]
        [Required]
        public int numeroReport { get; set; }
        [Required]
        [Display(Name = "Usuario")]
        public string usuario { get; set; }
        [Required]
        [Display(Name = "Equipo")]
        public equipos equiposID { get; set; }
        [Required]
        [Display(Name = "Denominación de Equipo")]
        public string denominacionEquipo { get; set; }
        [Required]
        [Display(Name = "Horómetro")]
        public int horometro { get; set; }
        [Required]
        [Display(Name = "Kilometraje")]
        public int kilometraje { get; set; }
        [Required]
        [Display(Name="Fecha y Hora")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm}", ApplyFormatInEditMode = true)]
        public DateTime fechaHora{ get; set; }
        [Required]
        [Display(Name = "Litros")]
        public int litros { get; set; }
        [Required]
        [Display(Name = "Ubicación")]
        public string ubicacion { get; set; }
        [Required]
        [Display(Name = "Operador")]
        public string operador { get; set; }
        [Required]
        [Display(Name = "Quien Carga")]
        public string quienCarga { get; set; }
        [Required]
        [Display(Name = "Responsable")]
        public string responsable { get; set; }
        [Required]
        [Display(Name = "Comentario")]
        public string comentario { get; set; }

        internal static List<reportCombustible> Todos(DateTime inicio, DateTime termino, string EquipoID)
        {

            //List<equipos> Equipos = equipos.todos();
            Hashtable tablaEquipos = new Hashtable();

            /*foreach (equipos Equipo in Equipos) 
            {
                tablaEquipos.Add(Equipo.ID, Equipo);
            }/*/


            DateTime fin = termino;
            DateTime temp=fin;
            while (temp.Day == termino.Day) 
            {
                fin =temp;
                temp = temp.AddMinutes(1);
            }
            
            List<reportCombustible> retorno = new RentaMaq.DAL.Context().ReportsCombustible.Where(m => m.fechaHora >= inicio && m.fechaHora <= fin).ToList();

            foreach (reportCombustible Report in retorno) 
            {
                int idEquipo = obtenerIDEquipo(Report.ID);
                if (tablaEquipos.ContainsKey(idEquipo))
                {
                    Report.equiposID = (equipos)tablaEquipos[idEquipo];
                }
                else 
                {
                    tablaEquipos.Add(idEquipo, equipos.Obtener(idEquipo));
                    Report.equiposID = (equipos)tablaEquipos[idEquipo];
                }
            }

            if (!EquipoID.Equals("todos"))
            {
                int equipoID = int.Parse(EquipoID);
                retorno = retorno.Where(s=>s.equiposID.ID==equipoID).ToList();
            }
            //retorno = retorno.OrderBy(s => s.equiposID.ID).OrderByDescending(s=>s.fechaHora).ToList();

            retorno.Sort(
                delegate(reportCombustible p1, reportCombustible p2)
                {
                    int compareDate = p1.equiposID.ID.CompareTo(p2.equiposID.ID);
                    if (compareDate == 0)
                    {
                        return p1.fechaHora.CompareTo(p2.fechaHora) * -1;
                    }
                    return compareDate;
                });//*/
            return retorno;
        }

        private static equipos obtenerEquipo(int ID)
        {
            equipos retorno = new equipos();
            Context db = new Context();
            SqlConnection con = conexion.crearConexion();
            con.Open();

            using (SqlCommand command = new SqlCommand("SELECT equiposID_ID FROM reportCombustible WHERE ID=@ID", con))
            {
                command.Parameters.Add("@ID", SqlDbType.Int).Value=ID;
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        retorno = equipos.Obtener(int.Parse(reader[0].ToString()));
                    }
                }
            }

            con.Close();
            return retorno;
        }

        private static int obtenerIDEquipo(int ID)
        {
            int retorno = -1;
            Context db = new Context();
            SqlConnection con = conexion.crearConexion();
            con.Open();

            using (SqlCommand command = new SqlCommand("SELECT equiposID_ID FROM reportCombustible WHERE ID=@ID", con))
            {
                command.Parameters.Add("@ID", SqlDbType.Int).Value = ID;
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        retorno = int.Parse(reader[0].ToString());
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

            using (SqlCommand command = new SqlCommand("UPDATE reportCombustible SET numeroReport=@numeroReport, usuario=@usuario, "
                + "denominacionEquipo=@denominacionEquipo, horometro=@horometro, kilometraje=@kilometraje, "
                + "fechaHora=@fechaHora, litros=@litros, ubicacion=@ubicacion, operador=@operador, "
                + "quienCarga=@quienCarga, responsable=@responsable, comentario=@comentario, equiposID_ID=@equiposID_ID "
                + "WHERE ID=@ID", con))
            {
                command.Parameters.Add("@numeroReport", SqlDbType.Int).Value = this.numeroReport;
                command.Parameters.Add("@usuario", SqlDbType.NVarChar).Value = this.usuario;
                command.Parameters.Add("@denominacionEquipo", SqlDbType.NVarChar).Value = this.denominacionEquipo;
                command.Parameters.Add("@horometro", SqlDbType.Int).Value = this.horometro;
                command.Parameters.Add("@kilometraje", SqlDbType.Int).Value = this.kilometraje;
                command.Parameters.Add("@fechaHora", SqlDbType.DateTime).Value = this.fechaHora;
                command.Parameters.Add("@litros", SqlDbType.Int).Value = this.litros;
                command.Parameters.Add("@ubicacion", SqlDbType.NVarChar).Value = this.ubicacion;
                command.Parameters.Add("@operador", SqlDbType.NVarChar).Value = this.operador;
                command.Parameters.Add("@quienCarga", SqlDbType.NVarChar).Value = this.quienCarga;
                command.Parameters.Add("@responsable", SqlDbType.NVarChar).Value = this.responsable;
                command.Parameters.Add("@comentario", SqlDbType.NVarChar).Value = this.comentario;
                command.Parameters.Add("@equiposID_ID", SqlDbType.Int).Value = this.equiposID.ID;
                command.Parameters.Add("@ID", SqlDbType.Int).Value = this.ID;

                command.ExecuteNonQuery();
            }

            con.Close();
        }

        private void insertar()
        {
            Context db = new Context();
            SqlConnection con = conexion.crearConexion();
            con.Open();

            using (SqlCommand command = new SqlCommand("INSERT INTO reportCombustible"
                +"(numeroReport, usuario, denominacionEquipo, horometro, kilometraje, fechaHora, litros, "
                + "ubicacion, operador, quienCarga, responsable, comentario, equiposID_ID) "
                + "VALUES(@numeroReport, @usuario, @denominacionEquipo, @horometro, @kilometraje, @fechaHora, @litros, "
                + "@ubicacion, @operador, @quienCarga, @responsable, @comentario, @equiposID_ID)", con))
            {
                command.Parameters.Add("@numeroReport", SqlDbType.Int).Value = this.numeroReport;
                command.Parameters.Add("@usuario", SqlDbType.NVarChar).Value = this.usuario;
                command.Parameters.Add("@denominacionEquipo", SqlDbType.NVarChar).Value = this.denominacionEquipo;
                command.Parameters.Add("@horometro", SqlDbType.Int).Value = this.horometro;
                command.Parameters.Add("@kilometraje", SqlDbType.Int).Value = this.kilometraje;
                command.Parameters.Add("@fechaHora", SqlDbType.DateTime).Value = this.fechaHora;
                command.Parameters.Add("@litros", SqlDbType.Int).Value = this.litros;
                command.Parameters.Add("@ubicacion", SqlDbType.NVarChar).Value = this.ubicacion;
                command.Parameters.Add("@operador", SqlDbType.NVarChar).Value = this.operador;
                command.Parameters.Add("@quienCarga", SqlDbType.NVarChar).Value = this.quienCarga;
                command.Parameters.Add("@responsable", SqlDbType.NVarChar).Value = this.responsable;
                command.Parameters.Add("@comentario", SqlDbType.NVarChar).Value = this.comentario;
                command.Parameters.Add("@equiposID_ID", SqlDbType.Int).Value = this.equiposID.ID;

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

            using (SqlCommand command = new SqlCommand("SELECT * FROM reportCombustible WHERE ID=@ID", con))
            {
                command.Parameters.Add("@ID", SqlDbType.Int).Value = this.ID;
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    retorno = reader.HasRows;
                }
            }

            con.Close();
            return retorno;
        }

        internal static reportCombustible Obtener(int? id)
        {
            reportCombustible Report = new RentaMaq.DAL.Context().ReportsCombustible.Find(id);
            Report.equiposID = obtenerEquipo(Report.ID);

            return Report;
        }

        public static int nuevoReport()
        {
            int retorno = 1;

            Context db = new Context();
            SqlConnection con = conexion.crearConexion();
            con.Open();

            using (SqlCommand command = new SqlCommand("SELECT TOP 1 numeroReport FROM reportCombustible ORDER BY numeroReport DESC", con))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while(reader.Read())
                        retorno = int.Parse(reader[0].ToString());
                }
            }

            con.Close();
            return retorno + 1;
        }

        public static reportCombustible obtenerUltimo(int idEquipo)
        {
            reportCombustible retorno = new reportCombustible();
            SqlConnection con = conexion.crearConexion();
            con.Open();

            using (SqlCommand command = new SqlCommand("SELECT TOP 1 ID FROM reportCombustible "
                + "WHERE equiposID_ID=@equiposID ORDER BY fechaHora DESC", con))
            {
                command.Parameters.Add("@equiposID", SqlDbType.Int).Value = idEquipo;
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        retorno = new Context().ReportsCombustible.Find(int.Parse(reader[0].ToString()));
                    }
                }
            }

            con.Close();
            return retorno;
        }

        internal static void eliminarPorIDEquipo(int id)
        {
            SqlConnection con = conexion.crearConexion();
            con.Open();

            using (SqlCommand command = new SqlCommand("DELETE FROM reportCombustible "
                + "WHERE equiposID_ID=@equiposID", con))
            {
                command.Parameters.Add("@equiposID", SqlDbType.Int).Value = id;
                command.ExecuteNonQuery();
            }

            con.Close();
        }
    }
}