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
    [ServiceContract(CallbackContract = typeof(IAdministrarPedidosMeserosCallback))]
    public interface IAdministrarPedidosMeseros
    {
        [OperationContract(IsOneWay = true)]
        void ObtenerProductos();

        [OperationContract(IsOneWay = true)]
        void RegistrarPedidoLocal(PedidoLocal pedido);

        [OperationContract(IsOneWay = true)]
        void ModificarPedidoLocal(PedidoLocal pedido);
    }

    [ServiceContract]
    public interface IAdministrarPedidosMeserosCallback
    {
        [OperationContract(IsOneWay = true)]
        void DatosRecuperados(List<ProductoDePedido> productos, List<ProvisionVentaDirecta> provisiones);

        [OperationContract(IsOneWay = true)]
        void MensajeAdministrarPedidosMeseros(string mensaje);
    }
}
