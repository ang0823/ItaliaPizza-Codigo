using AccesoBD2;
using System.Collections.Generic;
using System.ServiceModel;

namespace ServidrorPizzaItaliana
{
    [ServiceContract(CallbackContract = typeof(IModificarProductoCallback))]
    public interface IModificarProducto
    {

        [OperationContract(IsOneWay = true)]
        void ObtenerNombresDeRecetas();

        [OperationContract(IsOneWay = true)]
        void ModificarProductoExterno(ProvisionVentaDirecta productoExterno, string antiguoNombreImagen);

        [OperationContract(IsOneWay = true)]
        void ModificarProductoInterno(AccesoBD2.Producto producto, string antiguoNombreImagen, string nombreReceta, byte[] imagen);
    }

    [ServiceContract]
    public interface IModificarProductoCallback
    {
        [OperationContract(IsOneWay = true)]
        void ListaDeRecetas(List<string> nombreDeRecetas);

        [OperationContract(IsOneWay = true)]
        void RespuestaModificarProducto(string mensajeError);
    }
}