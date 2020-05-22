using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using AccesoBD2;

namespace ServidrorPizzaItaliana
{
    [ServiceContract(CallbackContract = typeof(IEliminarCuentaUsuarioCallback))]
    interface IEliminarCuentaUsuario
    {
        [OperationContract(IsOneWay = true)]
        void EliminarCuentaUsuario(string nombreUsuario, int id);
    }

    [ServiceContract]
    public interface IEliminarCuentaUsuarioCallback
    {

        [OperationContract(IsOneWay = true)]
        void RespuestaEU(string mensaje);
    }
}
