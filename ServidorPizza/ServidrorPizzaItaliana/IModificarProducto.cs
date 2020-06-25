using AccesoBD2;
using System.ServiceModel;

namespace ServidrorPizzaItaliana
{
    [ServiceContract(CallbackContract = typeof(IModificarProductoCallback))]
    public interface IModificarProducto
    {

        [OperationContract(IsOneWay = true)]
        void ModificarProductoExterno(ProvisionDirecta producto, Provision provision, byte[] imagen, bool modificarImagen);

        [OperationContract(IsOneWay = true)]
        void ModificarProductoInterno(AccesoBD2.Producto producto, byte[] imagen, bool modificarImagen);
    }

    [ServiceContract]
    public interface IModificarProductoCallback
    {
        [OperationContract(IsOneWay = true)]
        void RespuestaModificarProducto(string mensajeError);

    }
}