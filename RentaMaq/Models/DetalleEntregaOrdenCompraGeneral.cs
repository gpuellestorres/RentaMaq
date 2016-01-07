using RentaMaq.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace RentaMaq.Models
{
    public class DetalleEntregaOrdenCompraGeneral
    {
        public int DetalleEntregaOrdenCompraGeneralID { get; set; }
        public int DetalleOrdenCompraID { get; set; }
        public int CantidadEntregada { get; set; }

        internal void obtenerID()
        {
            DAL.Context db = new DAL.Context();
            this.DetalleEntregaOrdenCompraGeneralID = 0;

            foreach (DetalleEntregaOrdenCompraGeneral OC in db.detalleEntregaOCG.ToList())
            {
                if (OC.DetalleEntregaOrdenCompraGeneralID >= this.DetalleEntregaOrdenCompraGeneralID)
                {
                    this.DetalleEntregaOrdenCompraGeneralID = OC.DetalleEntregaOrdenCompraGeneralID + 1;
                }
            }
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

            using (SqlCommand command = new SqlCommand("UPDATE DetalleEntregaOrdenCompraGeneral SET "
                + "DetalleOrdenCompraID=@DetalleOrdenCompraID, CantidadEntregada=@CantidadEntregada "
                + " WHERE DetalleEntregaOrdenCompraGeneralID=@DetalleEntregaOrdenCompraGeneralID", con))
            {
                command.Parameters.Add("@DetalleOrdenCompraID", SqlDbType.Int).Value = this.DetalleOrdenCompraID;
                command.Parameters.Add("@CantidadEntregada", SqlDbType.Int).Value = this.CantidadEntregada;
                command.Parameters.Add("@DetalleEntregaOrdenCompraGeneralID", SqlDbType.Int).Value = this.DetalleEntregaOrdenCompraGeneralID;

                command.ExecuteNonQuery();
            }

            con.Close();
        }

        private void insertar()
        {
            Context db = new Context();
            SqlConnection con = conexion.crearConexion();
            con.Open();

            using (SqlCommand command = new SqlCommand("INSERT INTO DetalleEntregaOrdenCompraGeneral"
                + "(DetalleOrdenCompraID, CantidadEntregada, DetalleEntregaOrdenCompraGeneralID) "
                + "VALUES(@DetalleOrdenCompraID, @CantidadEntregada, @DetalleEntregaOrdenCompraGeneralID)", con))
            {
                command.Parameters.Add("@DetalleOrdenCompraID", SqlDbType.Int).Value = this.DetalleOrdenCompraID;
                command.Parameters.Add("@CantidadEntregada", SqlDbType.Int).Value = this.CantidadEntregada;
                command.Parameters.Add("@DetalleEntregaOrdenCompraGeneralID", SqlDbType.Int).Value = this.DetalleEntregaOrdenCompraGeneralID;

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

            using (SqlCommand command = new SqlCommand("SELECT * FROM DetalleEntregaOrdenCompraGeneral"
                + " WHERE DetalleEntregaOrdenCompraGeneralID=@DetalleEntregaOrdenCompraGeneralID", con))
            {
                command.Parameters.Add("@DetalleEntregaOrdenCompraGeneralID", SqlDbType.Int).Value = this.DetalleEntregaOrdenCompraGeneralID;
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