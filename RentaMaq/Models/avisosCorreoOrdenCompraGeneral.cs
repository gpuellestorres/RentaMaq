using RentaMaq.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace RentaMaq.Models
{
    public class avisosCorreoOrdenCompraGeneral
    {
        public int avisosCorreoOrdenCompraGeneralID { get; set; }
        public string nombreContacto { get; set; }
        public string correo { get; set; }

        internal static int obtenerID()
        {
            Context db = new Context();

            List<avisosCorreoOrdenCompraGeneral> lista = 
                db.avisosCorreoOrdenCompraGeneral.OrderByDescending(s => s.avisosCorreoOrdenCompraGeneralID).Take(1).ToList();
            if (lista.Count > 0) return lista[0].avisosCorreoOrdenCompraGeneralID + 1;
            else return 0;
        }

        internal void guardar()
        {
            if (existe()) {
                actualizar();
            }
            else
            {
                agregar();
            }
        }

        private void agregar()
        {
            SqlConnection con = conexion.crearConexion();
            con.Open();

            using (SqlCommand command = new SqlCommand("INSERT INTO avisosCorreoOrdenCompraGeneral("+
                "avisosCorreoOrdenCompraGeneralID,correo,nombreContacto) VALUES("
                + "@avisosCorreoOrdenCompraGeneralID, @correo, @nombreContacto)", con))
            {
                command.Parameters.Add("@avisosCorreoOrdenCompraGeneralID", SqlDbType.Int).Value = this.avisosCorreoOrdenCompraGeneralID;
                command.Parameters.Add("@correo", SqlDbType.NVarChar).Value = this.correo;
                command.Parameters.Add("@nombreContacto", SqlDbType.NVarChar).Value = this.nombreContacto;

                command.ExecuteNonQuery();
            }
            con.Close();
        }

        private void actualizar()
        {
            SqlConnection con = conexion.crearConexion();
            con.Open();

            using (SqlCommand command = new SqlCommand("UPDATE avisosCorreoOrdenCompraGeneral SET "
                + "correo=@correo, nombreContacto=@nombreContacto"
                + " WHERE avisosCorreoOrdenCompraGeneralID=@avisosCorreoOrdenCompraGeneralID", con))
            {
                command.Parameters.Add("@avisosCorreoOrdenCompraGeneralID", SqlDbType.Int).Value = this.avisosCorreoOrdenCompraGeneralID;
                command.Parameters.Add("@correo", SqlDbType.NVarChar).Value = this.correo;
                command.Parameters.Add("@nombreContacto", SqlDbType.NVarChar).Value = this.nombreContacto;

                command.ExecuteNonQuery();
            }
            con.Close();
        }

        private bool existe()
        {
            Context db = new Context();
            if (db.avisosCorreoOrdenCompraGeneral.Find(this.avisosCorreoOrdenCompraGeneralID) != null) return true;
            return false;
        }

        internal void eliminar()
        {
            Context db = new Context();
            db.avisosCorreoOrdenCompraGeneral.Remove(db.avisosCorreoOrdenCompraGeneral.Find(this.avisosCorreoOrdenCompraGeneralID));
            db.SaveChanges();
        }
    }
}