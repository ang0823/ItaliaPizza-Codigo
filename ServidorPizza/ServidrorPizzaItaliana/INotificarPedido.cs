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
        void EnviarPedidoLocal(PedidoLocal pedido, string usuario);

        [OperationContract(IsOneWay = true)]
        void EnviarPedidoADomicilio(PedidoADomicilio pedido, string usuario);
    }

    [ServiceContract]
    public interface INotificarPedidoCallback
    {
        [OperationContract(IsOneWay = true)]
        void RecibirPedidoLocal(PedidoLocal pedido);

        [OperationContract(IsOneWay = true)]
        void RecibirPedidoDomicilio(PedidoADomicilio pedido);
    }
}
