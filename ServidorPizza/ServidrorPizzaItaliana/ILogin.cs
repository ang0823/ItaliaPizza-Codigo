using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using AccesoBD2;

namespace ServidrorPizzaItaliana
{
    [ServiceContract(CallbackContract = typeof(ILoginCallback))]
    public interface ILogin
    {
        [OperationContract(IsOneWay = true)]
        void IniciarSesion(string nombreUsuario, string contraseña);
    }

    [ServiceContract]
    public interface ILoginCallback
    {
        [OperationContract(IsOneWay = true)]
        void DevuelveCuenta(CuentaUsuario1 cuenta,Empleado1 empleado, Direccion1 direccion,Rol1 rol);

        [OperationContract(IsOneWay = true)]
        void RespuestaLogin(string mensaje);
    }
}
