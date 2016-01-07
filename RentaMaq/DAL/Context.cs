using RentaMaq.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace RentaMaq.DAL
{
    public class Context : DbContext
    {

        public Context()
            : base("Context")
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }

        public DbSet<Producto> Productos { get; set; }
        public DbSet<Maestro> Maestros { get;set; }
        public DbSet<equipos> Equipos { get; set; }
        public DbSet<Proveedor> Proveedores { set; get; }
        public DbSet<reportCombustible> ReportsCombustible { set; get; }

        public DbSet<solicitudDeCotizacion> solicitudesDeCotizaciones { set; get; }
        public DbSet<detalleSolicitudDeCotizacion> detalleSolicitudDeCotizaciones { set; get; }

        public DbSet<OrdenDeCompraGeneral> ordenesDeCompra { set; get; }
        public DbSet<DetalleOrdenCompra> detalleOrdenCompra { set; get; }
        
        public DbSet<detalleOrdenDeCompraArriendoEquipo> detalleOrdenCompraArriendoEquipos { set; get; }

        public DbSet<pedidos> pedidos { set; get; }
        public DbSet<detallePedido> detallePedidos { set; get; }

        public DbSet<datosEntregaOrdenCompraGeneral> datosEntega { get;set; }
        public DbSet<DetalleEntregaOrdenCompraGeneral> detalleEntregaOCG { get; set; }

        public DbSet<avisosCorreoOrdenCompraGeneral> avisosCorreoOrdenCompraGeneral { get; set; }

        public DbSet<cotizacionArriendoEquipo> cotizacionArriendoEquipos { set; get; }
        public DbSet<detalleCotizacionArriendoEquipo> detalleCotizacionArriendoEquipo { set; get; }

        public DbSet<CotizacionDeTraslado> CotizacionDeTraslado { set; get; }
        public DbSet<detalleCotizacionDeTraslado> detalleCotizacionTraslado { set; get; }

        public DbSet<ordenPedidoCombustible> ordenesPedidoCombustible { set; get; }
        public DbSet<detalleOrdenPedidoCombustible> detalleOrdenesPedidosCombustible { set; get; }

        public DbSet<OrdenDePedido> OrdenesPedido { set; get; }
        public DbSet<detalleOrdenPedido> DetalleOrdenesPedido { set; get; }


        public DbSet<cotizacionServicios> cotizacionesServicios { set; get; }
        public DbSet<detalleServicioCotizacionServicios> detalleServiciosCotizacionServicios { set; get; }
        public DbSet<detalleEquiposCotizacionServicios> detalleEquiposCotizacionServicios { set; get; }

        public DbSet<Bodegas> bodegas { set; get; }


        public DbSet<detalleEquipo> detalleEquipos { set ; get; }


        public DbSet<ordenDeTrabajoGeneral> ordenDeTrabajoGenerals { set; get; }
        public DbSet<ejecutanteTrabajoOT> ejecutanteTrabajoOTs { set; get; }
        public DbSet<materialesUtilizadosOT> materialesUtilizadosOTs { set; get; }
        public DbSet<materialesRequeridosOT> materialesRequeridosOTs { set; get; }

        public DbSet<chequeoPreventivo> chequeoPreventivos { set; get; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

        public System.Data.Entity.DbSet<RentaMaq.Models.Modelo> Modeloes { get; set; }

        public System.Data.Entity.DbSet<RentaMaq.Models.Marca> Marcas { get; set; }

        public System.Data.Entity.DbSet<RentaMaq.Models.ordenDeCompraArriendoEquipo> ordenDeCompraArriendoEquipoes { get; set; }

        public System.Data.Entity.DbSet<RentaMaq.Models.registrokmhm> registrokmhms { get; set; }

        public System.Data.Entity.DbSet<RentaMaq.Models.mantencionPreventiva> mantencionPreventivas { get; set; }

        public System.Data.Entity.DbSet<RentaMaq.Models.hojaRutaMantenedores> hojaRutaMantenedores { get; set; }

        public System.Data.Entity.DbSet<RentaMaq.Models.tipoEquipo> tipoEquipoes { get; set; }

        public System.Data.Entity.DbSet<RentaMaq.Models.alertasMantenimiento> alertasMantenimientoes { get; set; }

        public System.Data.Entity.DbSet<RentaMaq.Models.indicadoresDeMantencion> indicadoresDeMantencions { get; set; }

        //public System.Data.Entity.DbSet<RentaMaq.Models.CotizacionDeTraslado> CotizacionDeTrasladoes { get; set; }
    }
}