using RentaMaq.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentaMaq.Models
{
    public class IndicadoresCostosActivosOT
    {
        public ordenDeTrabajoGeneral ordenDeTrabajoGeneral{ get; set; }
        public List<detalleIndicadoresCostosActivosOT> detalleCostos  { get; set; }
        public List<materialesUtilizadosOT> materialesUtilizados { get; set; }
        public double totalCostos { get; set; }


        public IndicadoresCostosActivosOT( int idOrdenTrabajoGeneral)
        {
            totalCostos = 0;
            Context db = new Context();

            ordenDeTrabajoGeneral = db.ordenDeTrabajoGenerals.Find(idOrdenTrabajoGeneral);

            materialesUtilizados = db.materialesUtilizadosOTs.Where(s => s.ordenDeTrabajoGeneralID == idOrdenTrabajoGeneral).ToList();

            detalleCostos = new List<detalleIndicadoresCostosActivosOT>();

            foreach (materialesUtilizadosOT Material in materialesUtilizados) 
            {
                detalleCostos.Add(new detalleIndicadoresCostosActivosOT(Material));
                totalCostos += detalleCostos[detalleCostos.Count - 1].costos;
            }
        }

        public IndicadoresCostosActivosOT( ordenDeTrabajoGeneral OrdenTrabajoGeneral)
        {
            totalCostos = 0;
            Context db = new Context();

            ordenDeTrabajoGeneral = OrdenTrabajoGeneral;

            materialesUtilizados = db.materialesUtilizadosOTs.Where(s => s.ordenDeTrabajoGeneralID == OrdenTrabajoGeneral.ordenDeTrabajoGeneralID).ToList();

            detalleCostos = new List<detalleIndicadoresCostosActivosOT>();

            foreach (materialesUtilizadosOT Material in materialesUtilizados) 
            {
                detalleCostos.Add(new detalleIndicadoresCostosActivosOT(Material));
                totalCostos += detalleCostos[detalleCostos.Count - 1].costos;
            }
        }
    }

    public class detalleIndicadoresCostosActivosOT 
    {
        public materialesUtilizadosOT MaterialesUtilizados { get; set; }
        public double costos { get; set; }

        public detalleIndicadoresCostosActivosOT(materialesUtilizadosOT material) 
        {
            costos = 0;
            MaterialesUtilizados = material;
            costos = MaterialesUtilizados.cantidad * MaterialesUtilizados.precioActual;
        }
    }

    public class IndicadoresCostosOTEquipo 
    {
        public int equipoID { get; set; }
        public List<IndicadoresCostosActivosOT> listaIndicadores { get; set; }
        public double totalCostos { get; set; }

        public IndicadoresCostosOTEquipo(int equipoId) 
        {
            totalCostos = 0;
            Context db = new Context();

            equipoID = equipoId;

            List<ordenDeTrabajoGeneral> listaOrdenesTrabajo = 
                db.ordenDeTrabajoGenerals.Where(s => s.idEquipo == equipoId.ToString()).ToList();

            listaIndicadores =  new List<IndicadoresCostosActivosOT>();

            foreach (ordenDeTrabajoGeneral OT in listaOrdenesTrabajo) 
            {
                listaIndicadores.Add(new IndicadoresCostosActivosOT(OT));
                totalCostos += listaIndicadores[listaIndicadores.Count - 1].totalCostos;
            }
        }
    }

    public class IndicadoresCostosActivosOTReport {
        public string materialUtilizado { get; set; }
        public int cantidad { get; set; }
        public int precio { get; set; }
        public int total { get; set; }
        public string numeroOT { get; set; }
        public string fechaOT { get; set; }
        public string equipo { get; set; }

        public IndicadoresCostosActivosOTReport(detalleIndicadoresCostosActivosOT detalleInd)
        {
            Context db = new Context();

            materialUtilizado = detalleInd.MaterialesUtilizados.nombreMaterial + "/" + detalleInd.MaterialesUtilizados.numeroParte;
            cantidad = (int)detalleInd.MaterialesUtilizados.cantidad;
            precio = detalleInd.MaterialesUtilizados.precioActual;
            total = cantidad * precio;

            ordenDeTrabajoGeneral OT =
            db.ordenDeTrabajoGenerals.Find(detalleInd.MaterialesUtilizados.ordenDeTrabajoGeneralID);
            numeroOT = OT.numeroFolio.ToString();

            fechaOT = formatearString.fechaSinHoraDiaPrimero(OT.horasMantenimientoFecha);

            equipos Equipo = equipos.Obtener(int.Parse(OT.idEquipo));
            equipo = Equipo.numeroAFI + "/" + Equipo.patenteEquipo + "/" + Equipo.ModeloID.nombreModelo
                + " - " + Equipo.ModeloID.MarcaID.NombreMarca;
        }

        public static List<IndicadoresCostosActivosOTReport> convertirDatos(IndicadoresCostosActivosOT costosOT)
        {
            Context db = new Context();
            List<IndicadoresCostosActivosOTReport> lista =  new List<IndicadoresCostosActivosOTReport>();
            foreach (detalleIndicadoresCostosActivosOT indicadorCostos in costosOT.detalleCostos) 
            {
                indicadorCostos.MaterialesUtilizados.numeroParte = db.Productos.Find(indicadorCostos.MaterialesUtilizados.materialID).numeroDeParte;                
                lista.Add(new IndicadoresCostosActivosOTReport(indicadorCostos));
            }
            return lista;
        }

        public static List<IndicadoresCostosActivosOTReport> convertirDatos(List<IndicadoresCostosActivosOT> costosOT) 
        {
            Context db = new Context();
            List<IndicadoresCostosActivosOTReport> lista =  new List<IndicadoresCostosActivosOTReport>();
            foreach (IndicadoresCostosActivosOT indicadorCostos in costosOT)
            {                
                foreach (IndicadoresCostosActivosOTReport dato in convertirDatos(indicadorCostos))
                {
                   
                    lista.Add(dato);
                }
            }
            return lista;
        }

        public static List<IndicadoresCostosActivosOTReport> convertirDatos(List<IndicadoresCostosOTEquipo> costosOTEquipos)
        {
            List<IndicadoresCostosActivosOTReport> lista = new List<IndicadoresCostosActivosOTReport>();

            foreach (IndicadoresCostosOTEquipo indicador in costosOTEquipos)
            {
                foreach (IndicadoresCostosActivosOTReport dato in convertirDatos(indicador.listaIndicadores))
                {
                    lista.Add(dato);
                }
            }
            return lista;
        }
    }
}