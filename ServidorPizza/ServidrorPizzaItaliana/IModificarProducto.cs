using AccesoBD2;
using System.ServiceModel;

namespace ServidrorPizzaItaliana
{
    [ServiceContract(CallbackContract = typeof(IModificarProductoCallback))]
    public interface IModificarProducto
    {
        [OperationContract(IsOneWay = true)]
        void Modificar(Provision provision, ProvisionDirecta provDirecta);
    }

    [ServiceContract]
    public interface IModificarProductoCallback
    {
        [OperationContract(IsOneWay = true)]
        void Respuesta(string mensajeError);

    }
}
