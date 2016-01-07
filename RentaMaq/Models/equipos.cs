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
    public class equipos
    {
        public int ID { get; set; }
        [Display(Name="Tipo de Equipo")]
        public string tipoEquipo { get; set; }
        [Display(Name = "Patente de Equipo")]
        public string patenteEquipo { get; set; }
        [Display(Name = "Modelo")]
        [Required]
        public Modelo ModeloID { get; set; }
        [Display(Name = "Año")]
        public int año { get; set; }
        [Display(Name = "Número AFI")]
        public string numeroAFI { get; set; }
        [Display(Name = "Área de Trabajo")]
        public string areaTrabajo { get; set; }
        [Display(Name = "Descripción")]
        public string descripcion { get; set; }

        public static List<equipos> todos()
        {
            List<equipos> retorno = new List<equipos>();

            Context db = new Context();
            SqlConnection con = conexion.crearConexion();
            con.Open();

            using (SqlCommand command = new SqlCommand("SELECT * FROM Equipos", con))
            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    equipos nuevo = new equipos();
                    nuevo.ID = int.Parse(reader["ID"].ToString());
                    nuevo.tipoEquipo = reader["tipoEquipo"].ToString();
                    nuevo.año = int.Parse(reader["año"].ToString());
                    nuevo.numeroAFI = reader["numeroAFI"].ToString();
                    nuevo.patenteEquipo = reader["patenteEquipo"].ToString();
                    nuevo.ModeloID = Modelo.Obtener(reader["ModeloID_ModeloID"].ToString());
                    nuevo.descripcion = reader["descripcion"].ToString();
                    nuevo.areaTrabajo = reader["areaTrabajo"].ToString();

                    retorno.Add(nuevo);
                }
            }

            con.Close();
            return retorno;
        }

        public static List<equipos> todosConTipo()
        {
            List<equipos> retorno = new List<equipos>();

            Context db = new Context();
            SqlConnection con = conexion.crearConexion();
            con.Open();

            using (SqlCommand command = new SqlCommand("SELECT * FROM Equipos", con))
            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    equipos nuevo = new equipos();
                    nuevo.ID = int.Parse(reader["ID"].ToString());                    
                    nuevo.año = int.Parse(reader["año"].ToString());
                    nuevo.numeroAFI = reader["numeroAFI"].ToString();
                    nuevo.patenteEquipo = reader["patenteEquipo"].ToString();
                    nuevo.tipoEquipo = db.tipoEquipoes.Find(Convert.ToInt32(reader["tipoEquipo"].ToString())).nombre;
                    nuevo.ModeloID = Modelo.Obtener(reader["ModeloID_ModeloID"].ToString());
                    nuevo.descripcion = reader["descripcion"].ToString();
                    nuevo.areaTrabajo = reader["areaTrabajo"].ToString();

                    retorno.Add(nuevo);
                }
            }

            con.Close();
            return retorno;
        }

        public static List<equipos> todosDeMaestros()
        {
            List<equipos> retorno = new List<equipos>();

            Context db = new Context();
            SqlConnection con = conexion.crearConexion();
            con.Open();

            using (SqlCommand command = new SqlCommand("SELECT DISTINCT afiEquipo FROM Maestro WHERE afiEquipo<>''", con))
            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    equipos nuevo = new equipos();
                    nuevo.numeroAFI = reader["afiEquipo"].ToString();
                    
                        retorno.Add(nuevo);
                }
            }

            con.Close();
            return retorno;
        }

        public static equipos Obtener(int? id)
        {
            equipos retorno = new equipos();

            Context db = new Context();
            SqlConnection con = conexion.crearConexion();
            con.Open();

            using (SqlCommand command = new SqlCommand("SELECT * FROM Equipos WHERE ID=@ID", con))
            {
                command.Parameters.Add("@ID", SqlDbType.Int).Value = id;
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        retorno.ID = int.Parse(reader["ID"].ToString());
                        retorno.tipoEquipo = reader["tipoEquipo"].ToString();
                        retorno.año = int.Parse(reader["año"].ToString());
                        retorno.numeroAFI = reader["numeroAFI"].ToString();
                        retorno.patenteEquipo = reader["patenteEquipo"].ToString();
                        retorno.ModeloID = Modelo.Obtener(reader["ModeloID_ModeloID"].ToString());
                        retorno.descripcion = reader["descripcion"].ToString();
                        retorno.areaTrabajo = reader["areaTrabajo"].ToString();
                    }
                }
            }

            con.Close();
            return retorno;
        }

        public static equipos ObtenerConTipo(int? id)
        {
            equipos retorno = new equipos();

            Context db = new Context();
            SqlConnection con = conexion.crearConexion();
            con.Open();

            using (SqlCommand command = new SqlCommand("SELECT * FROM Equipos WHERE ID=@ID", con))
            {
                command.Parameters.Add("@ID", SqlDbType.Int).Value = id;
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        retorno.ID = int.Parse(reader["ID"].ToString());
                        retorno.tipoEquipo = db.tipoEquipoes.Find(Convert.ToInt32(reader["tipoEquipo"].ToString())).nombre;                        
                        retorno.año = int.Parse(reader["año"].ToString());
                        retorno.numeroAFI = reader["numeroAFI"].ToString();
                        retorno.patenteEquipo = reader["patenteEquipo"].ToString();
                        retorno.ModeloID = Modelo.Obtener(reader["ModeloID_ModeloID"].ToString());
                        retorno.descripcion = reader["descripcion"].ToString();
                        retorno.areaTrabajo = reader["areaTrabajo"].ToString();
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

            using (SqlCommand command = new SqlCommand("UPDATE equipos SET tipoEquipo=@tipoEquipo, patenteEquipo=@patenteEquipo, "
                + "año=@año, numeroAFI=@numeroAFI, ModeloID_ModeloID=@ModeloID_ModeloID, "
                + "areaTrabajo=@areaTrabajo, descripcion=@descripcion WHERE ID=@ID", con))
            {
                command.Parameters.Add("@tipoEquipo", SqlDbType.NVarChar).Value = this.tipoEquipo;
                command.Parameters.Add("@patenteEquipo", SqlDbType.NVarChar).Value = this.patenteEquipo;
                command.Parameters.Add("@año", SqlDbType.Int).Value = this.año;
                command.Parameters.Add("@numeroAFI", SqlDbType.NVarChar).Value = this.numeroAFI;
                command.Parameters.Add("@ModeloID_ModeloID", SqlDbType.NVarChar).Value = this.ModeloID.ModeloID;
                command.Parameters.Add("@ID", SqlDbType.Int).Value = this.ID;
                command.Parameters.Add("@areaTrabajo", SqlDbType.NVarChar).Value = this.areaTrabajo;
                command.Parameters.Add("@descripcion", SqlDbType.NVarChar).Value = this.descripcion;

                command.ExecuteNonQuery();
            }

            con.Close();
        }

        private void insertar()
        {
            Context db = new Context();
            SqlConnection con = conexion.crearConexion();
            con.Open();

            using (SqlCommand command = new SqlCommand("INSERT INTO equipos(tipoEquipo, patenteEquipo, año, numeroAFI, ModeloID_ModeloID, areaTrabajo, descripcion) "
                + "VALUES(@tipoEquipo, @patenteEquipo, @año, @numeroAFI, @ModeloID_ModeloID, @areaTrabajo, @descripcion)", con))
            {
                command.Parameters.Add("@tipoEquipo", SqlDbType.NVarChar).Value = this.tipoEquipo;
                command.Parameters.Add("@patenteEquipo", SqlDbType.NVarChar).Value = this.patenteEquipo;
                command.Parameters.Add("@año", SqlDbType.Int).Value = this.año;
                command.Parameters.Add("@numeroAFI", SqlDbType.NVarChar).Value = this.numeroAFI;
                command.Parameters.Add("@ModeloID_ModeloID", SqlDbType.NVarChar).Value = this.ModeloID.ModeloID;
                command.Parameters.Add("@areaTrabajo", SqlDbType.NVarChar).Value = this.areaTrabajo;
                command.Parameters.Add("@descripcion", SqlDbType.NVarChar).Value = this.descripcion;

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

            using (SqlCommand command = new SqlCommand("SELECT * FROM equipos WHERE ID=@ID", con))
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

        public static bool SeUsa(int ID)
        {
            bool retorno = false;
            Context db = new Context();
            SqlConnection con = conexion.crearConexion();
            con.Open();

            using (SqlCommand command = new SqlCommand("SELECT * FROM reportCombustible WHERE EquiposID_ID=@ID", con))
            {
                command.Parameters.Add("@ID", SqlDbType.Int).Value = ID;
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    retorno = reader.HasRows;
                }
            }

            con.Close();
            return retorno;
        }

        public static int obtenerUltimoHorometro(int ID)
        {
            //reportCombustible ultimoReporteCombustible = reportCombustible.obtenerUltimo(ID);
            registrokmhm ultimoRegistroKMHM = registrokmhm.obtenerUltimo(ID);
            //mantencionPreventiva ultimaMantencionPreventiva = mantencionPreventiva.obtenerUltima(ID);
            return ultimoRegistroKMHM.horometro;
        }

        public static int obtenerUltimoKilometraje(int ID)
        {
            //reportCombustible ultimoReporteCombustible = reportCombustible.obtenerUltimo(ID);
            registrokmhm ultimoRegistroKMHM = registrokmhm.obtenerUltimo(ID);

            //mantencionPreventiva ultimaMantencionPreventiva = mantencionPreventiva.obtenerUltima(ID);

            return ultimoRegistroKMHM.kilometraje;
        }

        //Nos entrega una lista con IDs de los equipos con mantenciones preventivas pendientes
        public static List<int> mantencionesPreventivasPendientes() 
        {
            Context db = new Context();
            //List<int> lista = db.Equipos.Select(s => s.ID).ToList();
            List<int> lista = obtenerIDsMantencionesPendientes();
            return lista;
        }

        private static List<int> obtenerIDsMantencionesPendientes()
        {
            List<int> retorno = new List<int>();
            string consulta = "Select * FROM equipos "
                + "WHERE equipos.ID IN("
                + "select equipos.ID "
                + "from equipos, registrokmhm, mantencionPreventiva "
                + "WHERE equipos.ID=registrokmhm.equipoID "
                + "AND equipos.ID=mantencionPreventiva.equipoID "
                + "AND ((mantencionPreventiva.kilometrajeProximaMantencion<=(registrokmhm.kilometraje + 500) "
                + "AND mantencionPreventiva.kilometrajeProximaMantencion<>'0')OR ("
                + "mantencionPreventiva.horometroProximaMantencion<=(registrokmhm.horometro + 50)"
                + "AND mantencionPreventiva.horometroProximaMantencion<>'0'))) ";
            SqlConnection con = conexion.crearConexion();
            con.Open();

            using (SqlCommand command = new SqlCommand(consulta, con))
            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    retorno.Add(int.Parse(reader["ID"].ToString()));
                }
            }

            con.Close();
            return retorno;
        }

        public static int obtenerCantidadMantencionesPendientes()
        {
            int retorno = 0;
            string consulta = "Select count(*) FROM equipos "
                + "WHERE equipos.ID IN("
                + "select equipos.ID "
                + "from equipos, registrokmhm, mantencionPreventiva "
                + "WHERE equipos.ID=registrokmhm.equipoID "
                + "AND equipos.ID=mantencionPreventiva.equipoID "
                + "AND ((mantencionPreventiva.kilometrajeProximaMantencion<=(registrokmhm.kilometraje + 500) "
                + "AND mantencionPreventiva.kilometrajeProximaMantencion<>'0')OR ("
                + "mantencionPreventiva.horometroProximaMantencion<=(registrokmhm.horometro + 50)"
                + "AND mantencionPreventiva.horometroProximaMantencion<>'0'))) ";
            SqlConnection con = conexion.crearConexion();
            con.Open();

            using (SqlCommand command = new SqlCommand(consulta, con))
            {
                retorno = (Int32)command.ExecuteScalar();
            }

            con.Close();
            return retorno;
        }

        public static int obtenerCantidadMantencionesUrgentes()
        {
            int retorno = 0;
            string consulta = "Select count(*) FROM equipos "
	            +"WHERE equipos.ID IN("
		        +"select equipos.ID "
		        +"from equipos, registrokmhm, mantencionPreventiva "
		        +"WHERE equipos.ID=registrokmhm.equipoID "
		        +"AND equipos.ID=mantencionPreventiva.equipoID "
                +"AND ((mantencionPreventiva.kilometrajeProximaMantencion<=(registrokmhm.kilometraje + 200) "
                + "AND mantencionPreventiva.kilometrajeProximaMantencion<>'0')OR ("
				+"mantencionPreventiva.horometroProximaMantencion<=(registrokmhm.horometro + 20)"
                + "AND mantencionPreventiva.horometroProximaMantencion<>'0'))) ";
            SqlConnection con = conexion.crearConexion();
            con.Open();

            using (SqlCommand command = new SqlCommand(consulta, con))
            {
                retorno = (Int32)command.ExecuteScalar();
            }

            con.Close();
            return retorno;
        }

        private static bool mantencionPreventivaPendiente(int ID)
        {
            mantencionPreventiva ultima = mantencionPreventiva.obtenerUltima(ID);

            int ultimoHorometro = obtenerUltimoHorometro(ID);
            int ultimoKilometraje = obtenerUltimoKilometraje(ID);

            if (!ultima.Equals(new mantencionPreventiva())
                && ((ultima.horometroProximaMantencion < ultimoHorometro && ultima.horometroProximaMantencion > 0)
                || (ultima.kilometrajeProximaMantencion < ultimoKilometraje && ultima.kilometrajeProximaMantencion > 0)))
            {
                return true;
            }
            return false;
        }

        public static List<int> obtenerIDsLubricacionesPendientes() 
        {
            List<int> retorno = new List<int>();
            string consulta = "SELECT ID FROM equipos "
                        +"WHERE equipos.ID in "
                        +"(SELECT equipos.ID FROM equipos, tipoEquipo "
                        +"WHERE equipos.tipoEquipo=tipoEquipo.tipoEquipoID "
                        +"AND "
	                        +"( "
	                        +"tipoEquipo.nombre='BULLDOZER' "
	                        +"OR tipoEquipo.nombre='EXCAVADORA' "
	                        +"OR tipoEquipo.nombre='RETROEXCAVADORA' "
	                        +"OR tipoEquipo.nombre='CARGADOR FRONTAL' "
	                        +"OR tipoEquipo.nombre='CARGADOR FRONTAL' "
	                        +"OR tipoEquipo.nombre='MOTONIVELADORA' "
	                        +"OR tipoEquipo.nombre='RODILLO' "
	                        +") "
                        +") "
                        +"AND  "
                        +"( "
	                        +"equipos.ID not in ( "
		                        +"SELECT equipos.ID FROM equipos, hojaRutaMantenedores "
		                        +"WHERE equipos.ID=hojaRutaMantenedores.equipoID "
	                        +") "
	                        +"OR "
	                        +"equipos.ID in "
	                        +"( "
		                        +"SELECT equipos.ID "
		                        +"FROM equipos, hojaRutaMantenedores, "
		                        +"(select distinct(equipoID), max(fecha)as fecha from hojaRutaMantenedores group by equipoID) as fechas "
		                        +"WHERE fechas.equipoID=equipos.ID "
		                        +"AND fechas.fecha=hojaRutaMantenedores.fecha "
		                        +"AND equipos.ID=hojaRutaMantenedores.equipoID "
		                        +"AND ( "
				                        +"DATEADD(day,2,hojaRutaMantenedores.fecha) < GETDATE() "
			                        +") "
	                        +") "
	                        +"OR "
	                        +"equipos.ID in "
	                        +"( "
		                        +"SELECT equipos.ID "
		                        +"FROM equipos, hojaRutaMantenedores, reportCombustible, "
		                        +"(select distinct(equipoID), max(fecha)as fecha from hojaRutaMantenedores group by equipoID) as fechas "
		                        +"WHERE fechas.equipoID=equipos.ID "
		                        +"AND fechas.fecha=hojaRutaMantenedores.fecha "
		                        +"AND equipos.ID=hojaRutaMantenedores.equipoID "
		                        +"AND equipos.ID=reportCombustible.equiposID_ID "
		                        +"AND ( "
				                        +"reportCombustible.horometro > (hojaRutaMantenedores.horometro+20) "
			                        +") "
	                        +") "
	                        +"OR "
	                        +"equipos.ID in "
	                        +"( "
		                        +"SELECT equipos.ID "
		                        +"FROM equipos, hojaRutaMantenedores, registrokmhm, "
		                        +"(select distinct(equipoID), max(fecha)as fecha from hojaRutaMantenedores group by equipoID) as fechas "
		                        +"WHERE fechas.equipoID=equipos.ID "
		                        +"AND fechas.fecha=hojaRutaMantenedores.fecha "
		                        +"AND equipos.ID=hojaRutaMantenedores.equipoID "
		                        +"AND equipos.ID=registrokmhm.equipoID "
		                        +"AND ( "
				                        +"registrokmhm.horometro > (hojaRutaMantenedores.horometro+20) "
			                        +") "
	                        +") "
	                        +"OR "
	                        +"equipos.ID in "
	                        +"( "
		                        +"SELECT equipos.ID "
		                        +"FROM equipos, hojaRutaMantenedores, mantencionPreventiva, "
		                        +"(select distinct(equipoID), max(fecha)as fecha from hojaRutaMantenedores group by equipoID) as fechas "
		                        +"WHERE fechas.equipoID=equipos.ID "
		                        +"AND fechas.fecha=hojaRutaMantenedores.fecha "
		                        +"AND equipos.ID=hojaRutaMantenedores.equipoID "
                                +"AND equipos.ID=mantencionPreventiva.equipoID "
		                        +"AND ( "
				                        +"mantencionPreventiva.horometroActual > (hojaRutaMantenedores.horometro+20) "
			                        +") "
	                        +") "
                        +")";
            SqlConnection con = conexion.crearConexion();
            con.Open();

            using (SqlCommand command = new SqlCommand(consulta, con))
            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    retorno.Add(int.Parse(reader["ID"].ToString()));
                }
            }

            con.Close();
            return retorno;
        }

        public static int obtenerCantidadLubricacionesPendientes()
        {
            int retorno = 0;
            string consulta = "SELECT count(ID) FROM equipos "
                        + "WHERE equipos.ID in "
                        + "(SELECT equipos.ID FROM equipos, tipoEquipo "
                        + "WHERE equipos.tipoEquipo=tipoEquipo.tipoEquipoID "
                        + "AND "
                            + "( "
                            + "tipoEquipo.nombre='BULLDOZER' "
                            + "OR tipoEquipo.nombre='EXCAVADORA' "
                            + "OR tipoEquipo.nombre='RETROEXCAVADORA' "
                            + "OR tipoEquipo.nombre='CARGADOR FRONTAL' "
                            + "OR tipoEquipo.nombre='CARGADOR FRONTAL' "
                            + "OR tipoEquipo.nombre='MOTONIVELADORA' "
                            + "OR tipoEquipo.nombre='RODILLO' "
                            + ") "
                        + ") "
                        + "AND  "
                        + "( "
                            + "equipos.ID not in ( "
                                + "SELECT equipos.ID FROM equipos, hojaRutaMantenedores "
                                + "WHERE equipos.ID=hojaRutaMantenedores.equipoID "
                            + ") "
                            + "OR "
                            + "equipos.ID in "
                            + "( "
                                + "SELECT equipos.ID "
                                + "FROM equipos, hojaRutaMantenedores, "
                                + "(select distinct(equipoID), max(fecha)as fecha from hojaRutaMantenedores group by equipoID) as fechas "
                                + "WHERE fechas.equipoID=equipos.ID "
                                + "AND fechas.fecha=hojaRutaMantenedores.fecha "
                                + "AND equipos.ID=hojaRutaMantenedores.equipoID "
                                + "AND ( "
                                        + "DATEADD(day,2,hojaRutaMantenedores.fecha) < GETDATE() "
                                    + ") "
                            + ") "
                            + "OR "
                            + "equipos.ID in "
                            + "( "
                                + "SELECT equipos.ID "
                                + "FROM equipos, hojaRutaMantenedores, reportCombustible, "
                                + "(select distinct(equipoID), max(fecha)as fecha from hojaRutaMantenedores group by equipoID) as fechas "
                                + "WHERE fechas.equipoID=equipos.ID "
                                + "AND fechas.fecha=hojaRutaMantenedores.fecha "
                                + "AND equipos.ID=hojaRutaMantenedores.equipoID "
                                + "AND equipos.ID=reportCombustible.equiposID_ID "
                                + "AND ( "
                                        + "reportCombustible.horometro > (hojaRutaMantenedores.horometro+20) "
                                    + ") "
                            + ") "
                            + "OR "
                            + "equipos.ID in "
                            + "( "
                                + "SELECT equipos.ID "
                                + "FROM equipos, hojaRutaMantenedores, registrokmhm, "
                                + "(select distinct(equipoID), max(fecha)as fecha from hojaRutaMantenedores group by equipoID) as fechas "
                                + "WHERE fechas.equipoID=equipos.ID "
                                + "AND fechas.fecha=hojaRutaMantenedores.fecha "
                                + "AND equipos.ID=hojaRutaMantenedores.equipoID "
                                + "AND equipos.ID=registrokmhm.equipoID "
                                + "AND ( "
                                        + "registrokmhm.horometro > (hojaRutaMantenedores.horometro+20) "
                                    + ") "
                            + ") "
                            + "OR "
                            + "equipos.ID in "
                            + "( "
                                + "SELECT equipos.ID "
                                + "FROM equipos, hojaRutaMantenedores, mantencionPreventiva, "
                                + "(select distinct(equipoID), max(fecha)as fecha from hojaRutaMantenedores group by equipoID) as fechas "
                                + "WHERE fechas.equipoID=equipos.ID "
                                + "AND fechas.fecha=hojaRutaMantenedores.fecha "
                                + "AND equipos.ID=hojaRutaMantenedores.equipoID "
                                + "AND equipos.ID=mantencionPreventiva.equipoID "
                                + "AND ( "
                                        + "mantencionPreventiva.horometroActual > (hojaRutaMantenedores.horometro+20) "
                                    + ") "
                            + ") "
                        + ")";
            SqlConnection con = conexion.crearConexion();
            con.Open();

            using (SqlCommand command = new SqlCommand(consulta, con))
            {
                retorno = (Int32)command.ExecuteScalar();
            }

            con.Close();
            return retorno;
        }

        public static int obtenerCantidadLubricacionesUrgentes()
        {
            int retorno = 0;
            string consulta = "SELECT count(ID) FROM equipos "
                        + "WHERE equipos.ID in "
                        + "(SELECT equipos.ID FROM equipos, tipoEquipo "
                        + "WHERE equipos.tipoEquipo=tipoEquipo.tipoEquipoID "
                        + "AND "
                            + "( "
                            + "tipoEquipo.nombre='BULLDOZER' "
                            + "OR tipoEquipo.nombre='EXCAVADORA' "
                            + "OR tipoEquipo.nombre='RETROEXCAVADORA' "
                            + "OR tipoEquipo.nombre='CARGADOR FRONTAL' "
                            + "OR tipoEquipo.nombre='CARGADOR FRONTAL' "
                            + "OR tipoEquipo.nombre='MOTONIVELADORA' "
                            + "OR tipoEquipo.nombre='RODILLO' "
                            + ") "
                        + ") "
                        + "AND  "
                        + "( "
                            + "equipos.ID not in ( "
                                + "SELECT equipos.ID FROM equipos, hojaRutaMantenedores "
                                + "WHERE equipos.ID=hojaRutaMantenedores.equipoID "
                            + ") "
                            + "OR "
                            + "equipos.ID in "
                            + "( "
                                + "SELECT equipos.ID "
                                + "FROM equipos, hojaRutaMantenedores, "
                                + "(select distinct(equipoID), max(fecha)as fecha from hojaRutaMantenedores group by equipoID) as fechas "
                                + "WHERE fechas.equipoID=equipos.ID "
                                + "AND fechas.fecha=hojaRutaMantenedores.fecha "
                                + "AND equipos.ID=hojaRutaMantenedores.equipoID "
                                + "AND ( "
                                        + "DATEADD(day,3,hojaRutaMantenedores.fecha) < GETDATE() "
                                    + ") "
                            + ") "
                            + "OR "
                            + "equipos.ID in "
                            + "( "
                                + "SELECT equipos.ID "
                                + "FROM equipos, hojaRutaMantenedores, reportCombustible, "
                                + "(select distinct(equipoID), max(fecha)as fecha from hojaRutaMantenedores group by equipoID) as fechas "
                                + "WHERE fechas.equipoID=equipos.ID "
                                + "AND fechas.fecha=hojaRutaMantenedores.fecha "
                                + "AND equipos.ID=hojaRutaMantenedores.equipoID "
                                + "AND equipos.ID=reportCombustible.equiposID_ID "
                                + "AND ( "
                                        + "reportCombustible.horometro > (hojaRutaMantenedores.horometro+30) "
                                    + ") "
                            + ") "
                            + "OR "
                            + "equipos.ID in "
                            + "( "
                                + "SELECT equipos.ID "
                                + "FROM equipos, hojaRutaMantenedores, registrokmhm, "
                                + "(select distinct(equipoID), max(fecha)as fecha from hojaRutaMantenedores group by equipoID) as fechas "
                                + "WHERE fechas.equipoID=equipos.ID "
                                + "AND fechas.fecha=hojaRutaMantenedores.fecha "
                                + "AND equipos.ID=hojaRutaMantenedores.equipoID "
                                + "AND equipos.ID=registrokmhm.equipoID "
                                + "AND ( "
                                        + "registrokmhm.horometro > (hojaRutaMantenedores.horometro+30) "
                                    + ") "
                            + ") "
                            + "OR "
                            + "equipos.ID in "
                            + "( "
                                + "SELECT equipos.ID "
                                + "FROM equipos, hojaRutaMantenedores, mantencionPreventiva, "
                                + "(select distinct(equipoID), max(fecha)as fecha from hojaRutaMantenedores group by equipoID) as fechas "
                                + "WHERE fechas.equipoID=equipos.ID "
                                + "AND fechas.fecha=hojaRutaMantenedores.fecha "
                                + "AND equipos.ID=hojaRutaMantenedores.equipoID "
                                + "AND equipos.ID=mantencionPreventiva.equipoID "
                                + "AND ( "
                                        + "mantencionPreventiva.horometroActual > (hojaRutaMantenedores.horometro+30) "
                                    + ") "
                            + ") "
                        + ")";
            SqlConnection con = conexion.crearConexion();
            con.Open();

            using (SqlCommand command = new SqlCommand(consulta, con))
            {
                retorno = (Int32)command.ExecuteScalar();
            }

            con.Close();
            return retorno;
        }

        public static void enviarAlertasMantenimiento()
        {
            int cantidadMantencionesPreventivasPendientes = RentaMaq.Models.equipos.obtenerCantidadMantencionesPendientes();
            int cantidadLubricacionesPendientes = RentaMaq.Models.equipos.obtenerCantidadLubricacionesPendientes();

            int cantidadCertificacionVencida = RentaMaq.Models.detalleEquipo.cantidadEquiposCertificacionVencida();
            int cantidadRevisionTecnicaVencida = RentaMaq.Models.detalleEquipo.cantidadEquiposRevisionTecnicaVencida();
            int cantidadPermisoCirculacionVencida = RentaMaq.Models.detalleEquipo.cantidadEquiposPermisoCirculacionVencida();
            int cantidadSeguroVencido = RentaMaq.Models.detalleEquipo.cantidadSeguroVencido();

            if (cantidadMantencionesPreventivasPendientes > 0 || cantidadLubricacionesPendientes > 0
                || cantidadCertificacionVencida > 0 || cantidadRevisionTecnicaVencida > 0
                || cantidadPermisoCirculacionVencida > 0 || cantidadSeguroVencido > 0)
            {

                string mensaje = "Estimado Usuario:<br><br>" +
                    "Le informamos que existen las siguientes alertas de mantenimiento en el sistema de gestión de equipos de Renta-Maq:<br><br>";

                if (cantidadMantencionesPreventivasPendientes > 0)
                {
                    mensaje += "Mantenciones Preventivas pendientes: " + cantidadMantencionesPreventivasPendientes + "<br>";
                }
                if (cantidadLubricacionesPendientes > 0)
                {
                    mensaje += "Lubricaciones pendientes: " + cantidadLubricacionesPendientes + "<br>";
                }
                if (cantidadCertificacionVencida > 0)
                {
                    mensaje += "Certificaciones vencidas o próximas a vencer: " + cantidadCertificacionVencida + "<br>";
                }
                if (cantidadRevisionTecnicaVencida > 0)
                {
                    mensaje += "Revisiones Técnicas vencidas o próximas a vencer: " + cantidadRevisionTecnicaVencida + "<br>";
                }
                if (cantidadPermisoCirculacionVencida > 0)
                {
                    mensaje += "Permisos de Circulación vencidos o próximos a vencer: " + cantidadPermisoCirculacionVencida + "<br>";
                }
                if (cantidadSeguroVencido > 0)
                {
                    mensaje += "Seguro Automotriz vencido o próximo a vencer: " + cantidadSeguroVencido + "<br>";
                }

                mensaje += "<br><br>Para ingresar al sistema presione el siguiente enlace <a href='http://rentamaq.azurewebsites.net' target='_blank'>Ir al sitio</a><br><br>Atentamente,<br>puntodesarrollo ltda";

                Context db = new Context();

                List<alertasMantenimiento> Alertas = db.alertasMantenimientoes.ToList();
                List<string> correos = new List<string>();
                foreach (alertasMantenimiento Alerta in Alertas)
                {
                    correos.Add(Alerta.correo);
                }

                if (alertasMantenimiento.diasDesdeUltimaAlerta() > 1)
                {
                    envioCorreos.enviarAlerta(correos, "Alertas de Mantenimiento Rentamaq", mensaje);
                    alertasMantenimiento.registrarEnvio();
                }
            }
        }

        public static void enviarAlertasUrgenteMantenimiento()
        {
            if (alertasMantenimiento.diasDesdeUltimaAlerta() < 1)
            {
                return;
            }
            int cantidadMantencionesPreventivasUrgentes = RentaMaq.Models.equipos.obtenerCantidadMantencionesUrgentes();
            int cantidadLubricacionesUrgentes = RentaMaq.Models.equipos.obtenerCantidadLubricacionesUrgentes();

            int cantidadCertificacionVencidasUrgentes = RentaMaq.Models.detalleEquipo.cantidadEquiposCertificacionVencidaUrgente();
            int cantidadRevisionTecnicaVencidasUrgente = RentaMaq.Models.detalleEquipo.cantidadEquiposRevisionTecnicaVencidaUrgente();
            int cantidadPermisoCirculacionVencidasUrgentes = RentaMaq.Models.detalleEquipo.cantidadEquiposPermisoCirculacionVencidaUrgente();
            int cantidadSeguroVencidoUrgente = RentaMaq.Models.detalleEquipo.cantidadSeguroVencidoUrgente();

            if (cantidadMantencionesPreventivasUrgentes > 0 || cantidadLubricacionesUrgentes > 0
                || cantidadCertificacionVencidasUrgentes > 0 || cantidadRevisionTecnicaVencidasUrgente > 0
                || cantidadPermisoCirculacionVencidasUrgentes > 0 || cantidadSeguroVencidoUrgente > 0)
            {

                string mensaje = "Estimado Usuario:<br><br>" +
                    "Le informamos que existen las siguientes alertas de mantenimiento urgentes en el sistema de gestión de equipos de Renta-Maq:<br><br>";

                if (cantidadMantencionesPreventivasUrgentes > 0)
                {
                    mensaje += "Mantenciones Preventivas urgentes (20 horas o 200 KMs hasta mantención o menos): " + cantidadMantencionesPreventivasUrgentes + "<br>";
                }
                if (cantidadLubricacionesUrgentes > 0)
                {
                    mensaje += "Lubricaciones urgentes (3 días o 30 horas sin lubricar): " + cantidadLubricacionesUrgentes + "<br>";
                }
                if (cantidadCertificacionVencidasUrgentes > 0)
                {
                    mensaje += "Certificaciones vencidas o muy próximas a vencer (5 días o menos): " + cantidadCertificacionVencidasUrgentes + "<br>";
                }
                if (cantidadRevisionTecnicaVencidasUrgente > 0)
                {
                    mensaje += "Revisiones Técnicas vencidas o muy próximas a vencer (5 días o menos): " + cantidadRevisionTecnicaVencidasUrgente + "<br>";
                }
                if (cantidadPermisoCirculacionVencidasUrgentes > 0)
                {
                    mensaje += "Permisos de Circulación vencidos o muy próximos a vencer (5 días o menos): " + cantidadPermisoCirculacionVencidasUrgentes + "<br>";
                }
                if (cantidadSeguroVencidoUrgente > 0)
                {
                    mensaje += "Seguro Automotriz vencido o muy próximo a vencer (5 días o menos): " + cantidadSeguroVencidoUrgente + "<br>";
                }

                mensaje += "<br><br>Para ingresar al sistema presione el siguiente enlace <a href='http://rentamaq.azurewebsites.net' target='_blank'>Ir al sitio</a><br><br>Atentamente,<br>puntodesarrollo ltda";

                Context db = new Context();

                List<alertasMantenimiento> Alertas = db.alertasMantenimientoes.ToList();
                List<string> correos = new List<string>();
                foreach (alertasMantenimiento Alerta in Alertas)
                {
                    correos.Add(Alerta.correo);
                }
                envioCorreos.enviarAlerta(correos, "Alertas Urgentes de Mantenimiento Rentamaq", mensaje);
                alertasMantenimiento.registrarEnvio();
            }
        }

        internal static void eliminarInformacionEquipo(int id)
        {
            Context db = new Context();

            string idString = id.ToString();

            db.detalleEquipos.RemoveRange(db.detalleEquipos.Where(s => s.EquipoID == id));
            db.hojaRutaMantenedores.RemoveRange(db.hojaRutaMantenedores.Where(s => s.equipoID == id));
            db.registrokmhms.RemoveRange(db.registrokmhms.Where(s => s.equipoID == id));
            db.mantencionPreventivas.RemoveRange(db.mantencionPreventivas.Where(s => s.equipoID == id));
            db.ordenDeTrabajoGenerals.RemoveRange(db.ordenDeTrabajoGenerals.Where(s => s.idEquipo == idString));

            reportCombustible.eliminarPorIDEquipo(id);

            db.SaveChanges();
        }
    }
}