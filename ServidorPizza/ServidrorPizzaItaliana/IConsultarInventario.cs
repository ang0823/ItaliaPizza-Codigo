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
            this.Id = id;
            this.Nombre = nombre;
            this.NoExistencias = noExistencias;
            this.Ubicacion = ubicacion;
            this.StockMinimo = stockMinimo;
            this.CostoUnitario = costoUnitario;
            this.UnidadMedida = unidadMedida;
        }

        public int Id { get => id; set => id = value; }
        public string Nombre { get => nombre; set => nombre = value; }
        public int NoExistencias { get => noExistencias; set => noExistencias = value; }
        public string Ubicacion { get => ubicacion; set => ubicacion = value; }
        public int StockMinimo { get => stockMinimo; set => stockMinimo = value; }
        public double CostoUnitario { get => costoUnitario; set => costoUnitario = value; }
        public string UnidadMedida { get => unidadMedida; set => unidadMedida = value; }
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
            this.Id = id;
            this.Descripcion = descripcion;
            this.Activado = activado;
            this.Restricciones = restricciones;
        }

        public int Id { get => id; set => id = value; }
        public string Descripcion { get => descripcion; set => descripcion = value; }
        public bool Activado { get => activado; set => activado = value; }
        public string Restricciones { get => restricciones; set => restricciones = value; }
    }
}
