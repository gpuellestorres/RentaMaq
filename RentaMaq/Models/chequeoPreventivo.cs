using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace RentaMaq.Models
{
    public class chequeoPreventivo
    {
        public int chequeoPreventivoID { set; get; }

        [DisplayName("Codigo Equipo")]
        public string codigoEquipo{set;get;}

        [DisplayName("Numero:")]
        public int numeroChequeoPreventivo{set;get;}

        [DisplayName("Fecha")]
        public DateTime fecha{set;get;}

        [DisplayName("Hora")]
        public string hora { set; get; }

        [DisplayName("Observaciones Generales")]
        public string observacionesGenerales {set;get;}

        [DisplayName("Nivel Combustible")]
        public string nivelCombustible {set;get;}

        [DisplayName("Nombre Responsable Entrega")]
        public string nombreResponsableEntrega{set;get;}

        [DisplayName("Nombre Responsable Recepcion")]
        public string nombreResponsableRecepcion{set;get;}

        [DisplayName("Refrigerante")]
        public string refrigerante {set;get;}

        [DisplayName("Aceite de Motor")]
        public string aceiteMotor{set;get;}

        [DisplayName("Sistema de Refrigeracion")]
        public string sistemaRefrigeracion{set;get;}

        [DisplayName("Sistema Hidraulico")]
        public string sistemaHidraulico{set;get;}

        [DisplayName("Codigos o Testigos")]
        public string codigosTestigos{set;get;}

        [DisplayName("Freno de Servicio")]
        public string frenoServicio{set;get;}

        [DisplayName("Freno de Estacionamiento")]
        public string frenoEstacionamiento{set;get;}

        [DisplayName("Freno de Emergencia")]
        public string frenoEmergencia{set;get;}

        [DisplayName("Direccion")]
        public string direccion{set;get;}

        [DisplayName("Correas/Ventiladores")]
        public string correaVentiladores{set;get;}

        [DisplayName("Lubricacion General")]
        public string lubricacionGeneral{set;get;}

        [DisplayName("Fugas de Agua, Aceite o Comb.")]
        public string fugasAguaAceite{set;get;}

        [DisplayName("Elementos de Desgaste")]
        public string elementoDesgaste{set;get;}

        [DisplayName("Cadenas, Zapatas, Rodillos")]
        public string cadenaZapatillaRodillo{set;get;}

        [DisplayName("Balde o Pala")]
        public string baldePala{set;get;}

        [DisplayName("Pasadores")]
        public string pasadores{set;get;}

        [DisplayName("Neumaticas")]
        public string neumaticos{set;get;}

        [DisplayName("Pernos")]
        public string pernos{set;get;}

        [DisplayName("Mangueras y Orrings")]
        public string manguerasOrrings{set;get;}

        [DisplayName("Cilindros Hidraulicos")]
        public string cilindrosHidraulicos{set;get;}

        [DisplayName("Baterias")]
        public string baterias{set;get;}

        [DisplayName("Instalacion Electrica")]
        public string instalacionElectrica{set;get;}

        [DisplayName("Alzavidrios")]
        public string alzavidrios { set; get; }

        [DisplayName("Asientos")]
        public string asientos{set;get;}

        [DisplayName("Cinturon de Seguridad")]
        public string cinturonSeguridad{set;get;}

        [DisplayName("Aire Acondicionado")]
        public string aireAcondicionado{set;get;}

        [DisplayName("Limpia Parabrisas")]
        public string limpiaParabrisas{set;get;}

        [DisplayName("Vidrios")]
        public string vidrios{set;get;}

        [DisplayName("Balizas")]
        public string balizas{set;get;}

        [DisplayName("Cintas Reflextantes")]
        public string cintasReflextantes{set;get;}

        [DisplayName("Cunas")]
        public string cunas{set;get;}

        [DisplayName("Corta Corriente")]
        public string cortaCorriente{set;get;}

        [DisplayName("Bocina")]
        public string bocina{set;get;}

        [DisplayName("Luces Principales")]
        public string lucesPrincipales{set;get;}

        [DisplayName("Intermitentes")]
        public string intermitentes{set;get;}

        [DisplayName("Alarma de Retroceso")]
        public string alarmaDeRetroceso{set;get;}

        [DisplayName("Pertiga")]
        public string pertiga{set;get;}

        [DisplayName("Extintor")]
        public string extintor{set;get;}

        [DisplayName("Botiquin")]
        public string botiquin{set;get;}

        [DisplayName("Neumaticos de Respuesto")]
        public string neumaticosDeRespuesto { set; get; }


    }
}