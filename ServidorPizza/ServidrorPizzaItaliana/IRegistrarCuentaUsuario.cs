using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using AccesoBD2;

namespace ServidrorPizzaItaliana
{
    [ServiceContract(CallbackContract = typeof(IRegistrarCuentaUsuarioCallback))]
    interface IRegistrarCuentaUsuario
    {
        [OperationContract(IsOneWay = true)]
        void RegistrarCuentaUsuario(CuentaUsuario cuenta, Empleado empleado, Direccion direccion, Rol rol);
    }

    [ServiceContract]
    public interface IRegistrarCuentaUsuarioCallback
    {

        [OperationContract(IsOneWay = true)]
        void RespuestaRCU(string mensaje);
    }
}

