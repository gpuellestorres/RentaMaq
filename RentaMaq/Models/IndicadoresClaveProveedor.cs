using RentaMaq.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace RentaMaq.Models
{
    public class IndicadoresClaveProveedor
    {
        public int ProveedorID { get;set; }
        public double tiempoMedioRespuesta { get; set; }
        public double montoTotalCompras { get; set; }
        public double promedioComprasPorOC{ get; set; }
        public double promedioOCArriendoEquipo { get; set; }

        public List<tiempoRespuestaEquipo> tiemposRespuestaEquipo { set; get; }
        public List<tiempoRespuestaProducto> tiemposRespuestaProducto {get;set;}
        public List<MontoCompraOrdenCompra> montosCompra { get; set; }

        public IndicadoresClaveProveedor(int proveedorID) 
        {
            this.ProveedorID = proveedorID;
            this.promedioOCArriendoEquipo = 0;
            /*1.- Obtenemos el tiempo medio de respuesta:*/
            
            //Se obtienen los códigos de todos los productos comprados:
            List<int> IDsProductos = this.obtenerProductos();

            //Se obtienen los datos de tiempo de respuesta para cada producto:
            tiemposRespuestaProducto = new List<tiempoRespuestaProducto>();
            foreach (int idProducto in IDsProductos) 
            {
                tiemposRespuestaProducto.Add(new tiempoRespuestaProducto(this.ProveedorID, idProducto));
            }

            //Se obtiene el tiempo promedio de respuesta
            foreach (tiempoRespuestaProducto TR in tiemposRespuestaProducto) 
            {
                this.tiempoMedioRespuesta = TR.tiempoPromedioRespuesta;
            }

            if (tiemposRespuestaProducto.Count > 0) this.tiempoMedioRespuesta /= tiemposRespuestaProducto.Count;

            /*2.- Obtenemos el monto total de compras y el promedio de compras por orden de compra*/
            
            //Obtenemos los ID de ordenes de compra en que ha participado el proveedor y que han sido entregadas
            List<int> IDsOrdenesCompra = obtenerIDsOrdenesCompra();
            this.montosCompra = new List<MontoCompraOrdenCompra>();

            foreach (int IDOrdenCompra in IDsOrdenesCompra) 
            {
                this.montosCompra.Add(new MontoCompraOrdenCompra(this.ProveedorID,IDOrdenCompra));
            }

            //Se calcula el monto total de compras
            foreach (MontoCompraOrdenCompra montoOC in this.montosCompra) 
            {
                this.montoTotalCompras += montoOC.montoCompra;
            }

            //Se obtiene el monto promedio de compras
            this.promedioComprasPorOC = this.montoTotalCompras / montosCompra.Count;



            Context db = new Context();
            List<ordenDeCompraArriendoEquipo> listaOCArriendoEquipo = db.ordenDeCompraArriendoEquipoes.Where(s=>s.ProveedorID==ProveedorID & s.estado=="ENTREGADA").ToList();
            tiemposRespuestaEquipo = new List<tiempoRespuestaEquipo>();
            int cantidadOCArriendoEquipo=0;
           
            foreach(ordenDeCompraArriendoEquipo OC in listaOCArriendoEquipo){

                tiempoRespuestaEquipo tiempoRespuestaEquipo = new tiempoRespuestaEquipo(ProveedorID,OC);
                promedioOCArriendoEquipo += tiempoRespuestaEquipo.tiempoRespuesta;                    
                tiemposRespuestaEquipo.Add(tiempoRespuestaEquipo);
                cantidadOCArriendoEquipo++;
            }
            
            if(cantidadOCArriendoEquipo>0){
                promedioOCArriendoEquipo =   promedioOCArriendoEquipo/cantidadOCArriendoEquipo;
            }


        }

        private List<int> obtenerIDsOrdenesCompra()
        {
            List<int> retorno = new List<int>();

            Context db = new Context();

            SqlConnection con = conexion.crearConexion();
            con.Open();

            using (SqlCommand command = new SqlCommand("SELECT OrdenDeCompraGeneralID "
                + "FROM OrdenDeCompraGeneral "
                + "WHERE ProveedorID_ProveedorID=@ProveedorID "
                + "AND estado='ENTREGADA'", con))
            {
                command.Parameters.Add("@ProveedorID", SqlDbType.Int).Value = this.ProveedorID;
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        retorno.Add(int.Parse(reader[0].ToString()));
                    }
                }
            }
            con.Close();
            return retorno;
        }

        private List<int> obtenerProductos()
        {
            List<int> retorno = new List<int>();

            Context db = new Context();

            SqlConnection con = conexion.crearConexion();
            con.Open();

            using (SqlCommand command = new SqlCommand("SELECT DISTINCT(DetalleOrdenCompra.codigoInternoRentamaq) "
                + "FROM OrdenDeCompraGeneral, DetalleOrdenCompra, datosEntregaOrdenCompraGeneral "
                + "WHERE OrdenDeCompraGeneral.OrdenDeCompraGeneralID=datosEntregaOrdenCompraGeneral.OrdenDeCompraGeneralID AND "
                + "OrdenDeCompraGeneral.OrdenDeCompraGeneralID=DetalleOrdenCompra.IDOrdenCompra AND "
                + "OrdenDeCompraGeneral.ProveedorID_ProveedorID=@ProveedorID", con))
            {
                command.Parameters.Add("@ProveedorID", SqlDbType.Int).Value = this.ProveedorID;
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string numParte = reader[0].ToString();
                        retorno.Add(db.Productos.Where(s=>s.numeroDeParte==numParte).ToList()[0].ProductoID);
                    }
                }
            }
            con.Close();
            return retorno;
        }

        internal static List<IndicadoresClaveProveedor> obtenerTodos()
        {
            List<IndicadoresClaveProveedor> retorno = new List<IndicadoresClaveProveedor>();

            List<Proveedor> proveedores = new Context().Proveedores.ToList();

            foreach (Proveedor pro in proveedores) 
            {
                IndicadoresClaveProveedor temp = new IndicadoresClaveProveedor(pro.ProveedorID);
                if (temp.tiemposRespuestaProducto.Count > 0)
                    retorno.Add(temp);
            }

            return retorno;
        }
    }

    public class tiempoRespuestaProducto
    {
        public int ProveedorID { get;set; }
        public int ProductoID { get;set; }
        public double tiempoPromedioRespuesta {get;set;}

        public tiempoRespuestaProducto(int proveedorID, int productoID)
        {
            this.ProveedorID = proveedorID;
            this.ProductoID = productoID;
            tiempoPromedioRespuesta = calcularTiempoRespuesta();
        }

        private double calcularTiempoRespuesta() 
        {
            double retorno = 0;
            int cantidadResultados=0;

            Context db = new Context();

            SqlConnection con = conexion.crearConexion();
            con.Open();

            using (SqlCommand command = new SqlCommand("SELECT datosEntregaOrdenCompraGeneral.fechaEntregaReal, "
                +"OrdenDeCompraGeneral.fechaEntrega "
                +" FROM OrdenDeCompraGeneral, DetalleEntregaOrdenCompraGeneral, DetalleOrdenCompra, datosEntregaOrdenCompraGeneral "
                + "WHERE OrdenDeCompraGeneral.OrdenDeCompraGeneralID=datosEntregaOrdenCompraGeneral.OrdenDeCompraGeneralID AND "
                + "OrdenDeCompraGeneral.OrdenDeCompraGeneralID=DetalleOrdenCompra.IDOrdenCompra AND "
                + "DetalleOrdenCompra.DetalleOrdenCompraID=DetalleEntregaOrdenCompraGeneral.DetalleOrdenCompraID AND "
                + "DetalleOrdenCompra.codigoInternoRentamaq=@numeroParteProducto AND "
                + "OrdenDeCompraGeneral.ProveedorID_ProveedorID=@ProveedorID", con))
            {
                command.Parameters.Add("@ProveedorID", SqlDbType.Int).Value = this.ProveedorID;
                command.Parameters.Add("@numeroParteProducto", SqlDbType.Int).Value = db.Productos.Find(this.ProductoID).numeroDeParte;
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        retorno += (((DateTime)reader[0]) - ((DateTime)reader[1])).TotalDays;
                        cantidadResultados++;
                    }
                }
            }

            con.Close();

            if (cantidadResultados > 0) retorno /= cantidadResultados;
            return retorno;
        }
    }

    public class MontoCompraOrdenCompra
    {
        public int ProveedorID { get; set; }
        public int IDOrdenCompra { get; set; }
        public double montoCompra { get; set; }

        public MontoCompraOrdenCompra(int idProveedor,int IDOrdenCompra)
        {
            this.ProveedorID = idProveedor;
            this.IDOrdenCompra = IDOrdenCompra;
            this.montoCompra = calcularMontoCompra();
        }

        private double calcularMontoCompra()
        {
            double retorno = 0;
            Context db = new Context();

            retorno = db.ordenesDeCompra.Find(this.IDOrdenCompra).total;

            return retorno;
        }
    }
    
    public class tiempoRespuestaEquipo { 
    
        public int ProveedorID { get;set; }
        public int ordenDeCompraArriendoEquipoID { get;set; }
        public double tiempoRespuesta {get;set;}
        public string fecha { set; get; }

        public tiempoRespuestaEquipo(int proveedorID,ordenDeCompraArriendoEquipo OC )
        {
            this.ProveedorID = proveedorID;
            this.ordenDeCompraArriendoEquipoID = OC.ordenDeCompraArriendoEquipoID;
            tiempoRespuesta = (OC.fechaLlegadaReal - OC.fecha).TotalDays;
            this.fecha = Formateador.fechaCompletaToString(OC.fecha);
        }        
    }

    public class IndicadoresProdProvReporte 
    {
        public string nombreProveedor { get; set; }
        public string nombreProducto { get; set; }
        public string tiempoMedioRespuesta { get; set; }
        public string montoTotalCompras { get; set; }
        public string montoPromedioCompras { get; set; }
        public string numeroOrdenDeCompras { get; set; }
        public string fechaOrdenDeCompras { get; set; }
        public string tiempoRespuestaEquipos { get; set; }

        public static List<IndicadoresProdProvReporte> convertirIndicadoresProveedoresEnReporte(List<IndicadoresClaveProveedor> datosEntrada)
        {
            Context db = new Context();
            List<IndicadoresProdProvReporte> retorno = new List<IndicadoresProdProvReporte>();

            foreach (IndicadoresClaveProveedor dato in datosEntrada)
            {
                IndicadoresProdProvReporte temp = new IndicadoresProdProvReporte();
                temp.nombreProveedor = db.Proveedores.Find(dato.ProveedorID).nombreProveedor;
                temp.nombreProducto = "General";
                temp.tiempoMedioRespuesta = dato.tiempoMedioRespuesta.ToString();
                temp.montoTotalCompras = dato.montoTotalCompras.ToString();
                temp.montoPromedioCompras = dato.promedioComprasPorOC.ToString();
                temp.numeroOrdenDeCompras = "SIN DATOS";
                temp.fechaOrdenDeCompras = "SIN DATOS";
                temp.tiempoRespuestaEquipos = dato.promedioOCArriendoEquipo.ToString();

                retorno.Add(temp);
            }
            return retorno;
        }

        public static List<IndicadoresProdProvReporte> convertirIndicadoresProductoEnReporte(List<IndicadoresClaveProducto> datosEntrada)
        {
            Context db = new Context();
            List<IndicadoresProdProvReporte> retorno = new List<IndicadoresProdProvReporte>();

            foreach (IndicadoresClaveProducto dato in datosEntrada)
            {
                IndicadoresProdProvReporte temp = new IndicadoresProdProvReporte();
                temp.nombreProveedor = "General";
                temp.nombreProducto = db.Productos.Find(dato.ProductoID).descripcion;
                temp.tiempoMedioRespuesta = dato.tiempoRespuestaPromedio.ToString();
                temp.montoTotalCompras = "SIN DATOS";
                temp.montoPromedioCompras = "SIN DATOS";
                temp.numeroOrdenDeCompras = "SIN DATOS";
                temp.fechaOrdenDeCompras = "SIN DATOS";

                retorno.Add(temp);
            }
            return retorno;
        }

        public static List<IndicadoresProdProvReporte> convertirTiemposRespuestaEnReporte(List<tiempoRespuestaProducto> datosEntrada)
        {
            Context db = new Context();
            List<IndicadoresProdProvReporte> retorno = new List<IndicadoresProdProvReporte>();

            foreach (tiempoRespuestaProducto dato in datosEntrada)
            {
                IndicadoresProdProvReporte temp = new IndicadoresProdProvReporte();
                temp.nombreProveedor = db.Proveedores.Find(dato.ProveedorID).nombreProveedor;
                temp.nombreProducto = db.Productos.Find(dato.ProductoID).descripcion;
                temp.tiempoMedioRespuesta = dato.tiempoPromedioRespuesta.ToString();
                temp.montoTotalCompras = "SIN DATOS";
                temp.montoPromedioCompras = "SIN DATOS";
                temp.numeroOrdenDeCompras = "SIN DATOS";
                temp.fechaOrdenDeCompras = "SIN DATOS";

                retorno.Add(temp);
            }
            return retorno;
        }

        public static List<IndicadoresProdProvReporte> convertirIndicadoresMontoEnReporte(List<MontoCompraOrdenCompra> datosEntrada)
        {
            Context db = new Context();
            List<IndicadoresProdProvReporte> retorno = new List<IndicadoresProdProvReporte>();

            foreach (MontoCompraOrdenCompra dato in datosEntrada)
            {
                IndicadoresProdProvReporte temp = new IndicadoresProdProvReporte();
                temp.nombreProveedor = db.Proveedores.Find(dato.ProveedorID).nombreProveedor;
                temp.nombreProducto = "SIN DATOS";
                temp.tiempoMedioRespuesta = "SIN DATOS";
                temp.montoTotalCompras = dato.montoCompra.ToString();
                temp.montoPromedioCompras = "SIN DATOS";

                OrdenDeCompraGeneral OC = OrdenDeCompraGeneral.obtener(dato.IDOrdenCompra);

                temp.numeroOrdenDeCompras = OC.numeroOC + "/" + OC.añoOC;
                temp.fechaOrdenDeCompras = formatearString.fechaSinHoraDiaPrimero(OC.Fecha);

                retorno.Add(temp);
            }
            return retorno;
        }

        public static List<IndicadoresProdProvReporte> convertirIndicadoresTiempoOrdenArriendoEquipoEnReporte(List<tiempoRespuestaEquipo> datosEntrada)
        {
            Context db = new Context();
            List<IndicadoresProdProvReporte> retorno = new List<IndicadoresProdProvReporte>();

            foreach (tiempoRespuestaEquipo dato in datosEntrada)
            {
                IndicadoresProdProvReporte temp = new IndicadoresProdProvReporte();
                temp.nombreProveedor = db.Proveedores.Find(dato.ProveedorID).nombreProveedor;
                temp.nombreProducto = "SIN DATOS";

                if (dato.tiempoRespuesta > 0)
                {
                    temp.tiempoMedioRespuesta = dato.tiempoRespuesta.ToString();
                }
                else 
                {
                    temp.tiempoMedioRespuesta = "-";
                }

                temp.montoTotalCompras = "SIN DATOS";
                temp.montoPromedioCompras = "SIN DATOS";

                ordenDeCompraArriendoEquipo OC = db.ordenDeCompraArriendoEquipoes.Find(dato.ordenDeCompraArriendoEquipoID);

                temp.numeroOrdenDeCompras = OC.numeroOrdenCompraArriendoEquipo + "/" + OC.anio;
                temp.fechaOrdenDeCompras = formatearString.fechaSinHoraDiaPrimero(OC.fecha);

                retorno.Add(temp);
            }
            return retorno;
        }
    }

}