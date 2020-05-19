using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using AccesoBD2;

namespace ServidrorPizzaItaliana
{
    [ServiceContract(CallbackContract = typeof(IModificarCuentaUsuarioCallback))]
    interface IModificarCuentaUsuario
    {
        [OperationContract(IsOneWay = true)]
        void ModificarCuentaUsuario(CuentaUsuario cuenta, Empleado empleado, Direccion direccion, Rol rol);

    }

    [ServiceContract]
    public interface IModificarCuentaUsuarioCallback
    {

        [OperationContract(IsOneWay = true)]
        void ModificarCuentaUsuarioRespuesta(string mensaje);
    }
}
