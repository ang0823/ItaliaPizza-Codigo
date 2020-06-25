using AccesoBD2;
using System.ServiceModel;
using System.Collections.Generic;
using System.Runtime.Serialization;


namespace ServidrorPizzaItaliana
{

    [ServiceContract(CallbackContract = typeof(IBuscarProductoCallback))]
    public interface IBuscarProducto
    {
        [OperationContract(IsOneWay = true)]
        void BuscarProductoInternoPorNombre(string nombreProducto);

        [OperationContract(IsOneWay = true)]
        void BuscarProductoExternoPorNombre(string nombreProducto);
    }

    [ServiceContract]
    public interface IBuscarProductoCallback
    {
        [OperationContract(IsOneWay = true)]
        void ProductoInterno(AccesoBD2.Producto productoInterno, byte[] imagen);

        [OperationContract(IsOneWay = true)]
        void ProductoExterno(Provision1 provision, ProvisionDirecta1 provisionDirecta, byte[] imagen);

        [OperationContract(IsOneWay = true)]
        void ErrorAlRecuperarProducto(string mensajeError);

    }
}
