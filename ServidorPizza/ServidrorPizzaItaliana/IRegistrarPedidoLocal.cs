using AccesoBD2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ServidrorPizzaItaliana
{
    [ServiceContract(CallbackContract = typeof(IRegistrarPedidoLocalCallback))]
    public interface IRegistrarPedidoLocal
    {
        [OperationContract(IsOneWay = true)]
        void ObtenerInformacionDeProductosYEstados();

        [OperationContract(IsOneWay = true)]
        void RegistrarPedidoLocal(PedidoLocal pedido, Cuenta cuenta, int idEstado, int idEmpleado);
    }

    [ServiceContract]
    public interface IRegistrarPedidoLocalCallback
    {
        [OperationContract(IsOneWay = true)]
        void DatosRecuperados(List<ProductoDePedido> productos, List<ProvisionVentaDirecta> provisiones, List<EstadoDePedido> estados, List<MesaLocal> mesas);
        [OperationContract(IsOneWay = true)]
        void MensajeRegistrarPedidoLocal(string mensaje);
    }

    [DataContract]
    public class MesaLocal
    {
        [DataMember]
        private int idMesa;
        [DataMember]
        private short numeroMesa;

        public MesaLocal(int idMesa, short numeroMesa)
        {
            this.IdMesa = idMesa;
            this.NumeroMesa = numeroMesa;
        }

        public int IdMesa { get => idMesa; set => idMesa = value; }
        public short NumeroMesa { get => numeroMesa; set => numeroMesa = value; }
    }
}
