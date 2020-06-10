using AccesoBD2;
using System.ServiceModel;

namespace ServidrorPizzaItaliana
{
    [ServiceContract(CallbackContract = typeof(IModificarProductoCallback))]
    public interface IModificarProducto
    {
        [OperationContract(IsOneWay = true)]
        void ModificarProvisionDirectaDeProductoExterno(ProvisionDirecta provisionDirecta);

        [OperationContract(IsOneWay = true)]
        void ModificarProvisionDeProductoExterno(Provision provision);

        [OperationContract(IsOneWay = true)]
        void ModificarProductoInterno(AccesoBD2.Producto producto);
    }

    [ServiceContract]
    public interface IModificarProductoCallback
    {
        [OperationContract(IsOneWay = true)]
        void RespuestaModificarProducto(string mensajeError);

    }
}