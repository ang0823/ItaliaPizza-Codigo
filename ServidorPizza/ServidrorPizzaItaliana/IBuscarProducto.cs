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
        void BuscarPorNombre(string nombreProducto);

        [OperationContract(IsOneWay = true)]
        void BuscarPorID(int idProducto);
    }

    [ServiceContract]
    public interface IBuscarProductoCallback
    {
        [OperationContract(IsOneWay = true)]
        void ProvicionDirecta(ProvisionDirecta provDir);

        [OperationContract(IsOneWay = true)]
        void Provision(Provision prov);

        [OperationContract(IsOneWay = true)]
        void ErrorAlRecuperarProducto(string mensajeError);

    }
}
