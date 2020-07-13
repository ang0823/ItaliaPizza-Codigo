using AccesoBD2;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace ServidrorPizzaItaliana
{
    [ServiceContract(CallbackContract = typeof(IAdministrarPedidosMeserosCallback))]
    public interface IAdministrarPedidosMeseros
    {
        [OperationContract(IsOneWay = true)]
        void ObtenerProductos();

        [OperationContract]
        List<EmpleadoPizzeria> ObtenerMeseros();

        [OperationContract]
        bool RegistrarPedidoLocal(PedidoLocal pedido);

        [OperationContract]
        bool ModificarDatosPedidoLocal(PedidoLocal pedido);      
    }

    [ServiceContract]
    public interface IAdministrarPedidosMeserosCallback
    {
        [OperationContract(IsOneWay = true)]
        void DatosRecuperados(List<ProductoDePedido> productos, List<ProvisionVentaDirecta> provisiones);

        [OperationContract(IsOneWay = true)]
        void MensajeAdministrarPedidosMeseros(string mensaje);
    }

    [DataContract]
    public class EmpleadoPizzeria
    {
        [DataMember]
        private long id;
        [DataMember]
        private string idGenerado;

        public EmpleadoPizzeria(long id, string idGenerado)
        {
            this.id = id;
            this.idGenerado = idGenerado;
        }
    }
}
