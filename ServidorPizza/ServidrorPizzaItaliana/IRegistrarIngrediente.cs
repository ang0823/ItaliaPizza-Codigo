using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AccesoBD2;
using System.ServiceModel;

namespace ServidrorPizzaItaliana
{
    [ServiceContract(CallbackContract = typeof(IRegistrarIngredienteCallback))]
    public interface IRegistrarIngrediente
    {
        [OperationContract(IsOneWay = true)]
        void RegistrarProducto(Provision provision);
    }

    [ServiceContract]
    public interface IRegistrarIngredienteCallback
    {
        [OperationContract(IsOneWay = true)]
        void Respuesta(string mensajeError);

    }
}
