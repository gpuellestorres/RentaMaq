using RentaMaq.DAL;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace RentaMaq.Models
{
    public class IndicadoresClaveProducto
    {
        public int ProductoID { get; set; }
        public double tiempoRespuestaPromedio { get; set; }
        public List<tiempoRespuestaProducto> tiemposRespuestaPorProveedor { get; set; }

        public IndicadoresClaveProducto(int productoID) 
        {
            this.ProductoID = productoID;

            //Se obtienen los proveedores que han traído el producto
            List<int> idsProveedores = obtenerProveedores();

            //Obtenemos los datos de cada proveedor:
            this.tiemposRespuestaPorProveedor = new List<tiempoRespuestaProducto>();
            foreach (int idProveedor in idsProveedores) 
            {
                this.tiemposRespuestaPorProveedor.Add(new tiempoRespuestaProducto(idProveedor, this.ProductoID));
                this.tiempoRespuestaPromedio+=this.tiemposRespuestaPorProveedor[this.tiemposRespuestaPorProveedor.Count-1].tiempoPromedioRespuesta;
            }
            
            //Obtenemos el tiempo de respuesta promedio:
            this.tiempoRespuestaPromedio /= this.tiemposRespuestaPorProveedor.Count;
        }

        private List<int> obtenerProveedores()
        {
            List<int> retorno = new List<int>();

            Context db = new Context();
            string numeroParte = db.Productos.Find(this.ProductoID).numeroDeParte;

            List<DetalleOrdenCompra> detalleOC =
                db.detalleOrdenCompra.Where(s => s.codigoInternoRentamaq == numeroParte).ToList();

            foreach (DetalleOrdenCompra det in detalleOC) 
            {
                OrdenDeCompraGeneral OC = OrdenDeCompraGeneral.obtener(det.IDOrdenCompra);
                if (OC.estado == "ENTREGADA")
                {
                    int idProveedor = OC.ProveedorID.ProveedorID;
                    if (!retorno.Contains(idProveedor)) retorno.Add(idProveedor);
                }
            }

            return retorno;
        }

        internal static List<IndicadoresClaveProducto> obtenerDatos()
        {
            List<IndicadoresClaveProducto> retorno = new List<IndicadoresClaveProducto>();
            List<Producto> productos = new RentaMaq.DAL.Context().Productos.ToList();

            foreach (Producto prod in productos) 
            {
                IndicadoresClaveProducto ICP =
                new IndicadoresClaveProducto(prod.ProductoID);

                if (ICP.tiemposRespuestaPorProveedor.Count > 0)
                    retorno.Add(ICP);
            }

            return retorno;
        }
    }
}