using AccesoBD2;
using System.ServiceModel;

namespace ServidrorPizzaItaliana
{
    [ServiceContract(CallbackContract = typeof(IEditarIngredienteCallback))]
    public interface IEditarIngrediente
    {
        [OperationContract(IsOneWay = true)]
        void Editar(Provision provision);
    }

    [ServiceContract]
    public interface IEditarIngredienteCallback
    {
        [OperationContract(IsOneWay = true)]
        void RespuestaEditarIngrediente(string mensajeError);

    }
}
