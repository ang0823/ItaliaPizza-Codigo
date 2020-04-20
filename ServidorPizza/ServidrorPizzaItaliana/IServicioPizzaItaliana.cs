using AccesoBD2;
using System.ServiceModel;
using System.Collections.Generic;
using System.Runtime.Serialization;


namespace ServidrorPizzaItaliana
{
   
    [ServiceContract(CallbackContract = typeof(ICalculatorServiceCallback))]
    public interface IServicioPizzaItaliana
    {
        [OperationContract(IsOneWay = true)]
        void IniciarSesion(string nombreUsuario, string contraseña);
        [OperationContract(IsOneWay = true)]
        void CerrarSesion(string nombreUsuario);
        [OperationContract(IsOneWay = true)]
        void GuardarCuentaUsuario(CuentaUsuario cuentaUsuario);
        [OperationContract(IsOneWay = true)]
        void ModificarCuentaUsuario(CuentaUsuario cuentaUsuario);

    }

    /// <summary>
    /// La interfazICalculatorServiceCallback se encarga de especificar los
    /// metodos que un cliente puede utilizar del servidor.
    /// Contiene todas las funcionalidades que requiere el cliente.
    /// </summary>
    [ServiceContract]
    public interface ICalculatorServiceCallback
    {
        [OperationContract(IsOneWay = true)]
        void Respuesta(string mensaje);
        [OperationContract(IsOneWay = true)]
        void DevuelveCuenta(CuentaUsuario cuenta);

    }
}
