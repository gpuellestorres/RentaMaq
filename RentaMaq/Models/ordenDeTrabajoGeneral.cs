using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RentaMaq.DAL;

namespace RentaMaq.Models
{
    public class ordenDeTrabajoGeneral
    {
        public ordenDeTrabajoGeneral(){
            ejecutanteTrabajoOTs = new List<ejecutanteTrabajoOT>();
            materialesUtilizadosOTs= new List<materialesUtilizadosOT>();
            materialesRequeridosOTs = new List<materialesRequeridosOT>();
        }
        public int ordenDeTrabajoGeneralID { set; get; }
        public string numeroFolio { set; get; }
        public DateTime fechaOTAbierta { set; get; }
        public DateTime fechaOTCerrada { set; get; }
        public string operador { set; get; }
        public string faena { set; get; }
        public string turno { set; get; }
        public string area { set; get; }
        public string idEquipo { set; get; }
        public string tipoEquipo { set; get; }
        public string patenteEquipo { set; get; }
        public int horometro { set; get; }
        public int kilometraje { set; get; }
        public string tipoMantenimientoARealizar { set; get; }
        public string horasMantenimientoNivelCombustible { set; get; }
        public DateTime horasMantenimientoFecha { set; get; }
        public string horasMantenimientoHRInicio { set; get; }
        public string horasMantenimientoHRTermino { set; get; }
        public double horasMantenimientoHRSDetenido { set; get; }
        public string trabajoRealizar { set; get; }
        public string conclusionesTrabajoRealizado { set; get; }
        public string estadoEquipo { set; get; }
        public string trabajosPendientesPorRealizar { set; get; }
        public DateTime fechaTrabajosPendientesPorRealizar { set; get; }
        public string rutaImagen { set; get; }
        public string nombreMantenedor { set; get; }
        public string nombreOperador { set; get; }
        public string nombreSupervisor { set; get; }
        public string verificarTrabajoPendiente {set;get; }
        public string tipoOTSegunMantenimiento { set; get; }
        public int IDOTAnterior { set; get; }

        public virtual ICollection<ejecutanteTrabajoOT> ejecutanteTrabajoOTs { get; set; }
        public virtual ICollection<materialesUtilizadosOT> materialesUtilizadosOTs { get; set; }
        public virtual ICollection<materialesRequeridosOT> materialesRequeridosOTs { get; set; }


        public static int cantidadTrabajosPendientes() {
            Context db = new Context();
            DateTime fechaActual = DateTime.Now.AddDays(7);
            DateTime fechaIgnorar = new DateTime(2000, 1, 1);
            var L2EQueryCant = db.ordenDeTrabajoGenerals.Where(s => s.fechaTrabajosPendientesPorRealizar <= fechaActual & s.fechaTrabajosPendientesPorRealizar!=fechaIgnorar & s.verificarTrabajoPendiente!="TRUE").Count();
            int retorno = Convert.ToInt32(L2EQueryCant);
            return retorno;            
        }
        public static List<ordenDeTrabajoGeneral> obtenerOTconTrabajosPendientes() {
            Context db = new Context();
            DateTime fechaActual = DateTime.Now.AddDays(7);
            DateTime fechaIgnorar = new DateTime(2000, 1, 1);
            var L2EQuery = db.ordenDeTrabajoGenerals.Where(s => s.fechaTrabajosPendientesPorRealizar <= fechaActual & s.fechaTrabajosPendientesPorRealizar != fechaIgnorar & s.verificarTrabajoPendiente != "TRUE");
            List<ordenDeTrabajoGeneral> ordenTrabajo = L2EQuery.ToList();
            return ordenTrabajo;
        }
       

    }

    public class ejecutanteTrabajoOT {

        public int ejecutanteTrabajoOTID { set; get; }       

        public string nombreTrabajador { set; get; }
        public string cargo { set; get; }
        public double HH { set; get; }

        public int ordenDeTrabajoGeneralID { set; get; }
        [ForeignKey("ordenDeTrabajoGeneralID")]
        public virtual ordenDeTrabajoGeneral ordenDeTrabajoGeneral { get; set; }
    }

    public class materialesUtilizadosOT {
        public int materialesUtilizadosOTID { set; get; }
        public string nombreMaterial { set; get; }
        public double cantidad { set; get; }
        public int materialID { set; get; }
        public string numeroParte { set; get; }
        public int precioActual { set; get; }

        public int ordenDeTrabajoGeneralID { set; get; }
        [ForeignKey("ordenDeTrabajoGeneralID")]
        public virtual ordenDeTrabajoGeneral ordenDeTrabajoGeneral { get; set; }
    }

    public class materialesRequeridosOT
    {
        public int materialesRequeridosOTID { set; get; }
        public string nombreMaterial { set; get; }
        public double cantidad { set; get; }
        public int materialID { set; get; }
        public string numeroParte { set; get; }
        public int precioActual { set; get; }

        public int ordenDeTrabajoGeneralID { set; get; }
        [ForeignKey("ordenDeTrabajoGeneralID")]
        public virtual ordenDeTrabajoGeneral ordenDeTrabajoGeneral { get; set; }
    }


    public class ReportOTGeneral {       
        public string numeroFolio { set; get; }
        public string fechaOTAbierta { set; get; }
        public string fechaOTCerrada { set; get; }
        public string operador { set; get; }
        public string faena { set; get; }
        public string turno { set; get; }
        public string area { set; get; }        
        public string tipoEquipo { set; get; }
        public string patenteEquipo { set; get; }
        public string AFIEquipo { set; get; }
        public string modeloEquipo {set;get;}
        public int horometro { set; get; }
        public int kilometraje { set; get; }
        public string tipoMantenimientoARealizar { set; get; }
        public string horasMantenimientoNivelCombustible { set; get; }
        public string horasMantenimientoFecha { set; get; }
        public string horasMantenimientoHRInicio { set; get; }
        public string horasMantenimientoHRTermino { set; get; }
        public string horasMantenimientoHRSDetenido { set; get; }
        public string trabajoRealizar { set; get; }
        public string conclusionesTrabajoRealizado { set; get; }
        public string estadoEquipo { set; get; }
        public string trabajosPendientesPorRealizar { set; get; }
        public string fechaTrabajosPendientesPorRealizar { set; get; }        
        public string nombreMantenedor { set; get; }
        public string nombreOperador { set; get; }
        public string nombreSupervisor { set; get; }        
        public string tipoOTSegunMantenimiento { set; get; }

        public ReportOTGeneral(ordenDeTrabajoGeneral OT) {             
            this.numeroFolio =OT.numeroFolio;
            this.fechaOTAbierta =Formateador.fechaCompletaToString(OT.fechaOTAbierta);
            this.fechaOTCerrada =Formateador.fechaCompletaToString(OT.fechaOTCerrada);
            this.operador =OT.operador;
            this.faena=OT.faena;
            this.turno =OT.turno;
            this.area = OT.area;
            equipos equipo = equipos.ObtenerConTipo(Convert.ToInt32(OT.idEquipo));
            this.modeloEquipo= equipo.ModeloID.nombreModelo;
            this.tipoEquipo=equipo.tipoEquipo;
            this.patenteEquipo=OT.patenteEquipo;
            this.horometro=OT.horometro;
            this.kilometraje=OT.kilometraje;
            this.tipoMantenimientoARealizar=OT.tipoMantenimientoARealizar;
            this.horasMantenimientoNivelCombustible =OT.horasMantenimientoNivelCombustible;
            this.horasMantenimientoFecha =Formateador.fechaCompletaToString(OT.horasMantenimientoFecha);
            this.horasMantenimientoHRInicio =OT.horasMantenimientoHRInicio;
            this.horasMantenimientoHRTermino =OT.horasMantenimientoHRTermino;
            this.horasMantenimientoHRSDetenido =OT.horasMantenimientoHRSDetenido.ToString();
            this.trabajoRealizar=OT.trabajoRealizar;
            this.conclusionesTrabajoRealizado = OT.conclusionesTrabajoRealizado;
            this.estadoEquipo = OT.estadoEquipo;
            this.trabajosPendientesPorRealizar=OT.trabajosPendientesPorRealizar;
            this.fechaTrabajosPendientesPorRealizar =Formateador.fechaCompletaToString(OT.fechaTrabajosPendientesPorRealizar);            
            this.nombreMantenedor=OT.nombreMantenedor;
            this.nombreOperador =OT.nombreOperador;
            this.nombreSupervisor=OT.nombreSupervisor;
            this.tipoOTSegunMantenimiento = OT.tipoOTSegunMantenimiento;
            this.AFIEquipo = new Context().Equipos.Find(int.Parse(OT.idEquipo)).numeroAFI;
        
        }    
    }

    public class materialesUtilizadosReportOTGeneral {
        public string nombreMaterial { set; get; }
        public double cantidad { set; get; }        
        public string numeroParte { set; get; }
        public materialesUtilizadosReportOTGeneral(materialesUtilizadosOT matOT) {
            Context db = new Context();
            this.nombreMaterial = matOT.nombreMaterial;
            this.cantidad = matOT.cantidad;
            Producto producto = db.Productos.Find(matOT.materialID);
            this.numeroParte = producto.numeroDeParte;
        }
    }
    public class materialesRequeridoReportOTGeneral
    {
        public string nombreMaterial { set; get; }
        public double cantidad { set; get; }        
        public string numeroParte { set; get; }
        public materialesRequeridoReportOTGeneral(materialesRequeridosOT matReq)
        {
            Context db = new Context();
            this.nombreMaterial = matReq.nombreMaterial;
            this.cantidad = matReq.cantidad;
            Producto producto = db.Productos.Find(matReq.materialID);
            this.numeroParte = producto.numeroDeParte;
        }
    }
}