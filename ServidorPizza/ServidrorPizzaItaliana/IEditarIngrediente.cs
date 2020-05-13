using AccesoBD2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ServidrorPizzaItaliana
{
    [ServiceContract(CallbackContract = typeof(IEditarIngredienteCallback))]
    public interface IEditararIngrediente
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
