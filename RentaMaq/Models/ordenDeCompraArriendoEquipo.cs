using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RentaMaq.DAL;
using System.Data;
using System.Data.SqlClient;
namespace RentaMaq.Models
{
    public class ordenDeCompraArriendoEquipo
    {

        public int ordenDeCompraArriendoEquipoID { get; set; }
             
        [Display(Name = "Código")]
        public string Codigo { get; set; }
        [Display(Name = "Orden de Compra Teck CDA N°")]
        public int numeroOrdenCompraArriendoEquipo { get; set; }


        public DateTime fecha { set; get; }
        public int anio { get; set; }

        [Display(Name = "Proveedor")]
        public int ProveedorID { get; set; }
        public string texto1 { set; get; }
        public string texto2 { set; get; }

        [Display(Name = "Plazo de Entrega")]
        public DateTime plazoEntrega { get; set; }
        [Display(Name = "Persona que retira")]
        public string personaRetira { get; set; }
        [Display(Name = "Forma de Pago")]        
        public string formaPago { get; set; }
        public string estado { get; set; }

        [Display(Name = "Atencion A:")]
        public string dirigidoA { set; get; }


        [Display(Name = "Nota:")]
        public string nota { set; get; }

        [Display(Name = "Fecha Llegada Equipo:")]
        public DateTime fechaLlegadaReal { set; get; }

        
        public string noIncluye { set; get; }
        public string tipoHorasMinimas { set; get; }


        public static void ingresoOrdenCompraArriendoEquipo(int ordenDeCompraArriendoEquipoID, int numeroItem, DateTime fechaIngresoEquipo)
        {
          
            SqlConnection con = conexion.crearConexion();
            con.Open();

            using (SqlCommand command = new SqlCommand("INSERT INTO ingresoOrdenCompraArriendoEquipo"
                + "(ordenDeCompraArriendoEquipoID, numeroItem, fechaIngresoEquipo) "
                + "VALUES(@ordenDeCompraArriendoEquipoID, @numeroItem, @fechaIngresoEquipo)", con))
            {
                command.Parameters.Add("@ordenDeCompraArriendoEquipoID", SqlDbType.Int).Value = ordenDeCompraArriendoEquipoID;
                command.Parameters.Add("@numeroItem", SqlDbType.Int).Value = numeroItem;
                command.Parameters.Add("@fechaIngresoEquipo", SqlDbType.DateTime).Value = fechaIngresoEquipo;               

                command.ExecuteNonQuery();
            }

            con.Close();
        }

        public static bool itemFueIngresado(int ordenDeCompraArriendoEquipoID, int numeroItem)
        {
            bool retorno = false;            
            SqlConnection con = conexion.crearConexion();
            con.Open();

            using (SqlCommand command = new SqlCommand("SELECT * FROM ingresoOrdenCompraArriendoEquipo WHERE ordenDeCompraArriendoEquipoID=@ordenDeCompraArriendoEquipoID AND numeroItem=@numeroItem", con))
            {
                command.Parameters.Add("@ordenDeCompraArriendoEquipoID", SqlDbType.Int).Value = ordenDeCompraArriendoEquipoID;
                command.Parameters.Add("@numeroItem", SqlDbType.Int).Value = numeroItem;
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    retorno = reader.HasRows;
                }
            }

            con.Close();
            return retorno;
        }
        private static int cantidadItemIngresados(int ordenDeCompraArriendoEquipoID)
        {           
            SqlConnection con = conexion.crearConexion();
            con.Open();
            int retorno = 0;
            using (SqlCommand command = new SqlCommand("SELECT * FROM ingresoOrdenCompraArriendoEquipo WHERE ordenDeCompraArriendoEquipoID=@ordenDeCompraArriendoEquipoID", con))
            {
                command.Parameters.Add("@ordenDeCompraArriendoEquipoID", SqlDbType.Int).Value = ordenDeCompraArriendoEquipoID;               
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        retorno++;                       
                    }
                }
            }

            con.Close();
            return retorno;
        }


        public static int cantidadOrdenCompraEquiposPendientes() {
            Context db = new Context();
            List<ordenDeCompraArriendoEquipo> listaOrdenCompraArriendoEquipo = db.ordenDeCompraArriendoEquipoes.Where(s => s.estado == "ENTREGADA").ToList();
            List<ordenDeCompraArriendoEquipo> listaFinalOC = new List<ordenDeCompraArriendoEquipo>();
            foreach (ordenDeCompraArriendoEquipo OC in listaOrdenCompraArriendoEquipo)
            {
                if (ordenDeCompraArriendoEquipo.cantidadEquiposFaltantes(OC.ordenDeCompraArriendoEquipoID) > 0)
                {
                    listaFinalOC.Add(OC);
                }
            }
            return listaFinalOC.Count;                
        
        }



        public static int cantidadEquiposFaltantes(int ordenDeCompraArriendoEquipoID)
        {
            int cantidadEquiposIngresados = ordenDeCompraArriendoEquipo.cantidadItemIngresados(ordenDeCompraArriendoEquipoID);
            
            Context db = new Context();
            var L2EQuery = db.detalleOrdenCompraArriendoEquipos.Where(s => s.ordenDeCompraArriendoEquipoID == ordenDeCompraArriendoEquipoID);
            int cantidadTotalEquiposOrden = L2EQuery.ToList().Count;
            return cantidadTotalEquiposOrden-cantidadEquiposIngresados;
        }




        internal void obtenerNumeroOC()
        {
            List<ordenDeCompraArriendoEquipo>lista = new Context().ordenDeCompraArriendoEquipoes.Where(s=>s.anio == this.anio)
                .OrderByDescending(u => u.numeroOrdenCompraArriendoEquipo).Take(1).ToList();
            if (lista.Count == 0) this.numeroOrdenCompraArriendoEquipo = 1;
            else
            {
                this.numeroOrdenCompraArriendoEquipo = lista[0].numeroOrdenCompraArriendoEquipo + 1;
            }
        }


        public static void eliminarDetalle(int ordenCompraArriendoEquipoID)
        {
            
                SqlConnection con = conexion.crearConexion();
                con.Open();

                using (SqlCommand command = new SqlCommand("DELETE FROM ingresoOrdenCompraArriendoEquipo "
                    + "WHERE ordenDeCompraArriendoEquipoID=@ordenDeCompraArriendoEquipoID", con))
                {
                    command.Parameters.Add("@ordenDeCompraArriendoEquipoID", SqlDbType.Int).Value = ordenCompraArriendoEquipoID;

                    command.ExecuteNonQuery();
                }

                con.Close();
            
        }
    }

    public class detalleOrdenDeCompraArriendoEquipo {

        public int detalleOrdenDeCompraArriendoEquipoID { set; get; }


       
        public int ordenDeCompraArriendoEquipoID { set; get; }
         [ForeignKey("ordenDeCompraArriendoEquipoID")]

        public virtual ordenDeCompraArriendoEquipo ordenDeCompraArriendoEquipo { get; set; }
       

        [Display(Name = "Item")]
        public int numeroItem { set; get; }

        [Display(Name = "Descripcion Equipo")]
        public string descripcionEquipo { set; get; }

        [Display(Name = "Valor Hora")]
        public string valorHora { set; get; }

        [Display(Name = "Horas Minimas")]
        public int horasMinimasMensuales { set; get; }

        [Display(Name = "Duracion Arriendo")]
        public string duracionArriendo { set; get; }

        [Display(Name = "Lugar De Faena")]
        public string lugarDeFaena { set; get; }

        [Display(Name = "Condiciones de Pago")]
        public string condicionesDePago { set; get; }        
    
    
    }


    public class ReportOrdenDeCompraArriendoEquipo {
       
        public int numeroOrdenCompraArriendoEquipo { get; set; }              
        public string Codigo { get; set; }                
        public string fecha { set; get; }
        public int anio { get; set; }   
        public string nombreProveedor { get; set; }
        public string rutProveedor { set; get; }
        public string direccionProveedor { set; get; }
        public string texto1 { set; get; }
        public string texto2 { set; get; }        
        public string plazoEntrega { get; set; }       
        public string personaRetira { get; set; }        
        public string formaPago { get; set; }
        public string dirigidoA { set; get; }
        public string noIncluye { set; get; }
        public string tipoHorasMinimas { set; get; }
                  
        //DETALLE
        public int numeroItem { set; get; }   
        public string descripcionEquipo { set; get; }        
        public string valorHora { set; get; }        
        public int horasMinimasMensuales { set; get; }
        public string duracionArriendo { set; get; }        
        public string lugarDeFaena { set; get; }      
        public string condicionesDePago { set; get; }

        public ReportOrdenDeCompraArriendoEquipo(ordenDeCompraArriendoEquipo ordenDeCompraArriendoEquipo, detalleOrdenDeCompraArriendoEquipo detalleOrdenDeCompraArriendoEquipo)
        {  

             Context db = new Context();
             Proveedor Proveedor = db.Proveedores.Find(ordenDeCompraArriendoEquipo.ProveedorID);
             
             this.numeroOrdenCompraArriendoEquipo=ordenDeCompraArriendoEquipo.numeroOrdenCompraArriendoEquipo; 
             //this.FechaVigencia = Formateador.fechaCompletaToString(ordenDeCompraArriendoEquipo.FechaVigencia);
             this.Codigo = ordenDeCompraArriendoEquipo.Codigo;               
             this.fecha = Formateador.fechaCompletaToString(ordenDeCompraArriendoEquipo.fecha);
             this.anio = ordenDeCompraArriendoEquipo.anio;    
             this.nombreProveedor = Proveedor.nombreProveedor;
             this.rutProveedor = Proveedor.rut;
             this.direccionProveedor = Proveedor.domicilio;
             this.texto1 = ordenDeCompraArriendoEquipo.texto1;
             this.texto2 = ordenDeCompraArriendoEquipo.texto2;
             this.noIncluye=ordenDeCompraArriendoEquipo.noIncluye;
             this.tipoHorasMinimas =ordenDeCompraArriendoEquipo.tipoHorasMinimas.ToUpper();


             if(ordenDeCompraArriendoEquipo.plazoEntrega.Year==2000){
                this.plazoEntrega ="POR CONFIRMAR";
             }else{
                 this.plazoEntrega = formatearString.fechaPalabras(ordenDeCompraArriendoEquipo.plazoEntrega);
             }             

             this.personaRetira = ordenDeCompraArriendoEquipo.personaRetira;       
             this.formaPago = ordenDeCompraArriendoEquipo.formaPago;
             this.dirigidoA = ordenDeCompraArriendoEquipo.dirigidoA;
                  
             //DETALLE
             this.numeroItem =detalleOrdenDeCompraArriendoEquipo.numeroItem;  
             this.descripcionEquipo = detalleOrdenDeCompraArriendoEquipo.descripcionEquipo;       
             this.valorHora = detalleOrdenDeCompraArriendoEquipo.valorHora;       
             this.horasMinimasMensuales= detalleOrdenDeCompraArriendoEquipo.horasMinimasMensuales;
             this.duracionArriendo = detalleOrdenDeCompraArriendoEquipo.duracionArriendo;         
             this.lugarDeFaena = detalleOrdenDeCompraArriendoEquipo.lugarDeFaena;
             this.condicionesDePago = detalleOrdenDeCompraArriendoEquipo.condicionesDePago;              
        }
    }
}