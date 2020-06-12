using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using AccesoBD2;

namespace ServidrorPizzaItaliana
{
    [ServiceContract(CallbackContract = typeof(IConsultarInventarioCallback))]
    interface IConsultarInventario
    {
        [OperationContract(IsOneWay = true)]
        void ConsultarInventario();
    }

    [ServiceContract]
    public interface IConsultarInventarioCallback
    {
        [OperationContract(IsOneWay = true)]
        void DevuelveInventario(List<Provision> cuentas);

        [OperationContract(IsOneWay = true)]
        void RespuestaCI(string mensaje);
    }


    [DataContract]
    public class Provision1
    {
        [DataMember]
        int id;
        [DataMember]
        string nombre;
        [DataMember]
        int noExistencias;
        [DataMember]
        string ubicacion;
        [DataMember]
        int stockMinimo;
        [DataMember]
        double costoUnitario;
        [DataMember]
        string unidadMedida;

        public Provision1(int id, string nombre, int noExistencias, string ubicacion, int stockMinimo, double costoUnitario, string unidadMedida)
        {
            this.id = id;
            this.nombre = nombre;
            this.noExistencias = noExistencias;
            this.ubicacion = ubicacion;
            this.stockMinimo = stockMinimo;
            this.costoUnitario = costoUnitario;
            this.unidadMedida = unidadMedida;
        }
    }

    [DataContract]
    public class ProvisionDirecta1
    {
        [DataMember]
        int id;
        [DataMember]
        string descripcion;

        [DataMember]
        Boolean activado;
        [DataMember]
        string restricciones;

        public ProvisionDirecta1(int id, string descripcion, Boolean activado, string restricciones)
        {
            this.id = id;
            this.descripcion = descripcion;
            this.activado = activado;
            this.restricciones = restricciones;
        }
    }
}
