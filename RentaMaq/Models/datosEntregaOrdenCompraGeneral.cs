using RentaMaq.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace RentaMaq.Models
{
    public class datosEntregaOrdenCompraGeneral
    {
        public int datosEntregaOrdenCompraGeneralID { get; set; }
        public int OrdenDeCompraGeneralID { get; set; }
        public DateTime fechaEntregaReal { get; set; }
        public string notaRecibo { get; set; }

        internal void obtenerID()
        {
            DAL.Context db = new DAL.Context();
            this.datosEntregaOrdenCompraGeneralID = 0;

            foreach (datosEntregaOrdenCompraGeneral OC in db.datosEntega.ToList())
            {
                if (OC.datosEntregaOrdenCompraGeneralID >= this.datosEntregaOrdenCompraGeneralID)
                {
                    this.datosEntregaOrdenCompraGeneralID = OC.datosEntregaOrdenCompraGeneralID + 1;
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

            using (SqlCommand command = new SqlCommand("UPDATE datosEntregaOrdenCompraGeneral SET "
                + "fechaEntregaReal=@fechaEntregaReal, notaRecibo=@notaRecibo, OrdenDeCompraGeneralID=@OrdenDeCompraGeneralID "
                + "WHERE datosEntregaOrdenCompraGeneralID=@datosEntregaOrdenCompraGeneralID", con))
            {
                command.Parameters.Add("@fechaEntregaReal", SqlDbType.DateTime).Value = this.fechaEntregaReal;
                command.Parameters.Add("@notaRecibo", SqlDbType.NVarChar).Value = this.notaRecibo;
                command.Parameters.Add("@OrdenDeCompraGeneralID", SqlDbType.Int).Value = this.OrdenDeCompraGeneralID;
                command.Parameters.Add("@datosEntregaOrdenCompraGeneralID", SqlDbType.Int).Value = this.datosEntregaOrdenCompraGeneralID;

                command.ExecuteNonQuery();
            }

            con.Close();
        }

        private void insertar()
        {
            Context db = new Context();
            SqlConnection con = conexion.crearConexion();
            con.Open();

            using (SqlCommand command = new SqlCommand("INSERT INTO datosEntregaOrdenCompraGeneral"
                + "(fechaEntregaReal, notaRecibo, OrdenDeCompraGeneralID, datosEntregaOrdenCompraGeneralID) "
                + "VALUES(@fechaEntregaReal, @notaRecibo, @OrdenDeCompraGeneralID, @datosEntregaOrdenCompraGeneralID)", con))
            {
                command.Parameters.Add("@fechaEntregaReal", SqlDbType.DateTime).Value = this.fechaEntregaReal;
                command.Parameters.Add("@notaRecibo", SqlDbType.NVarChar).Value = this.notaRecibo;
                command.Parameters.Add("@OrdenDeCompraGeneralID", SqlDbType.Int).Value = this.OrdenDeCompraGeneralID;
                command.Parameters.Add("@datosEntregaOrdenCompraGeneralID", SqlDbType.Int).Value = this.datosEntregaOrdenCompraGeneralID;

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

            using (SqlCommand command = new SqlCommand("SELECT * FROM datosEntregaOrdenCompraGeneral"
                + " WHERE datosEntregaOrdenCompraGeneralID=@datosEntregaOrdenCompraGeneralID", con))
            {
                command.Parameters.Add("@datosEntregaOrdenCompraGeneralID", SqlDbType.Int).Value = this.datosEntregaOrdenCompraGeneralID;
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    retorno = reader.HasRows;
                }
            }

            con.Close();
            return retorno;
        }

        internal void eliminarDetalle()
        {
            List<DetalleOrdenCompra> detalle =
            new RentaMaq.DAL.Context().detalleOrdenCompra.Where(s => s.IDOrdenCompra == this.OrdenDeCompraGeneralID).ToList();

            foreach (DetalleOrdenCompra Det in detalle) 
            {
                SqlConnection con = conexion.crearConexion();
                con.Open();

                using (SqlCommand command = new SqlCommand("DELETE FROM DetalleEntregaOrdenCompraGeneral "
                    + "WHERE DetalleOrdenCompraID=@DetalleOrdenCompraID", con))
                {
                    command.Parameters.Add("@DetalleOrdenCompraID", SqlDbType.Int).Value = Det.DetalleOrdenCompraID;

                    command.ExecuteNonQuery();
                }

                con.Close();
            }
        }
    }
}