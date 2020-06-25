using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using AccesoBD2;

namespace ServidrorPizzaItaliana
{
    [ServiceContract(CallbackContract = typeof(INotificarPedidoCallback))]
    public interface INotificarPedido
    {
        [OperationContract(IsOneWay = true)]
        void AgregarUsuario(string tipoUsuario);

        [OperationContract(IsOneWay = true)]
        void NotificarPedidoLocalPreparado(PedidoLocal pedido, string usuario);

        [OperationContract(IsOneWay = true)]
        void NotificarPedidoADomicilioPreparado(PedidoADomicilio pedido, string usuario);
    }

    [ServiceContract]
    public interface INotificarPedidoCallback
    {
        [OperationContract(IsOneWay = true)]
        void RecibirPedidoLocal(PedidoLocal pedido);

        [OperationContract(IsOneWay = true)]
        void RecibirPedidoDomicilio(PedidoADomicilio pedido);

        [OperationContract(IsOneWay = true)]
        void MensajeNotificarPedido(string mensaje);
    }
}
