using RentaMaq.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace RentaMaq.Models
{
    public class DetalleOrdenCompra
    {
        public int DetalleOrdenCompraID { get; set; }
        public int IDOrdenCompra { get; set; }
        public string codigo { get; set; }
        public double cantidad { get; set; }
        public string descripcion { get; set; }
        public string tipoDeCompra { get; set; }
        public string codigoInternoRentamaq { get; set; }
        public int plazoEntrega { get; set; }
        public int precioUnitario { get; set; }
        public int descuento { get; set; }
        public double porcentajeDescuento { get; set; }
        public double valorTotal { get; set; }

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

            using (SqlCommand command = new SqlCommand("UPDATE DetalleOrdenCompra SET "
                + "IDOrdenCompra=@IDOrdenCompra, codigo=@codigo, cantidad=@cantidad, descripcion=@descripcion, "
                + "codigoInternoRentamaq=@codigoInternoRentamaq, plazoEntrega=@plazoEntrega, precioUnitario=@precioUnitario, descuento=@descuento, "
                + "porcentajeDescuento=@porcentajeDescuento, valorTotal=@valorTotal, tipoDeCompra=@tipoDeCompra "
                +" WHERE DetalleOrdenCompraID=@DetalleOrdenCompraID", con))
            {
                command.Parameters.Add("@IDOrdenCompra", SqlDbType.Int).Value = this.IDOrdenCompra;
                command.Parameters.Add("@codigo", SqlDbType.NVarChar).Value = this.codigo;
                command.Parameters.Add("@cantidad", SqlDbType.Float).Value = this.cantidad;
                command.Parameters.Add("@descripcion", SqlDbType.NVarChar).Value = this.descripcion;
                command.Parameters.Add("@codigoInternoRentamaq", SqlDbType.NVarChar).Value = this.codigoInternoRentamaq;
                command.Parameters.Add("@plazoEntrega", SqlDbType.Int).Value = this.plazoEntrega;
                command.Parameters.Add("@precioUnitario", SqlDbType.Int).Value = this.precioUnitario;
                command.Parameters.Add("@descuento", SqlDbType.Int).Value = this.descuento;
                command.Parameters.Add("@porcentajeDescuento", SqlDbType.Float).Value = this.porcentajeDescuento;
                command.Parameters.Add("@valorTotal", SqlDbType.Float).Value = this.valorTotal;
                command.Parameters.Add("@DetalleOrdenCompraID", SqlDbType.Int).Value = this.DetalleOrdenCompraID;
                command.Parameters.Add("@tipoDeCompra", SqlDbType.NVarChar).Value = this.tipoDeCompra;

                command.ExecuteNonQuery();
            }

            con.Close();
        }

        private void insertar()
        {
            Context db = new Context();
            SqlConnection con = conexion.crearConexion();
            con.Open();

            using (SqlCommand command = new SqlCommand("INSERT INTO DetalleOrdenCompra"
                + "(IDOrdenCompra, codigo, cantidad, descripcion, "
                + "codigoInternoRentamaq, plazoEntrega, precioUnitario, descuento, "
                + "porcentajeDescuento, valorTotal, DetalleOrdenCompraID, tipoDeCompra) "
                + "VALUES(@IDOrdenCompra, @codigo, @cantidad, @descripcion, "
                + "@codigoInternoRentamaq, @plazoEntrega, @precioUnitario, @descuento, "
                + "@porcentajeDescuento, @valorTotal, @DetalleOrdenCompraID, @tipoDeCompra)", con))
            {
                command.Parameters.Add("@IDOrdenCompra", SqlDbType.Int).Value = this.IDOrdenCompra;
                command.Parameters.Add("@codigo", SqlDbType.NVarChar).Value = this.codigo;
                command.Parameters.Add("@cantidad", SqlDbType.Float).Value = this.cantidad;
                command.Parameters.Add("@descripcion", SqlDbType.NVarChar).Value = this.descripcion;
                command.Parameters.Add("@codigoInternoRentamaq", SqlDbType.NVarChar).Value = this.codigoInternoRentamaq;
                command.Parameters.Add("@plazoEntrega", SqlDbType.Int).Value = this.plazoEntrega;
                command.Parameters.Add("@precioUnitario", SqlDbType.Int).Value = this.precioUnitario;
                command.Parameters.Add("@descuento", SqlDbType.Int).Value = this.descuento;
                command.Parameters.Add("@porcentajeDescuento", SqlDbType.Float).Value = this.porcentajeDescuento;
                command.Parameters.Add("@valorTotal", SqlDbType.Float).Value = this.valorTotal;
                command.Parameters.Add("@DetalleOrdenCompraID", SqlDbType.Int).Value = this.DetalleOrdenCompraID;
                command.Parameters.Add("@tipoDeCompra", SqlDbType.NVarChar).Value = this.tipoDeCompra;

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

            using (SqlCommand command = new SqlCommand("SELECT * FROM DetalleOrdenCompra WHERE DetalleOrdenCompraID=@DetalleOrdenCompraID", con))
            {
                command.Parameters.Add("@DetalleOrdenCompraID", SqlDbType.Int).Value = this.DetalleOrdenCompraID;                
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    retorno = reader.HasRows;
                }
            }

            con.Close();
            return retorno;
        }

        internal void obtenerID()
        {
            DAL.Context db = new DAL.Context();
            this.DetalleOrdenCompraID = 1;

            foreach (DetalleOrdenCompra OC in db.detalleOrdenCompra.ToList())
            {
                if (OC.DetalleOrdenCompraID >= this.DetalleOrdenCompraID)
                {
                    this.DetalleOrdenCompraID = OC.DetalleOrdenCompraID + 1;
                }
            }
        }
    }
}