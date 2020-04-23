using System;
using AccesoBD2;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Threading;

namespace ServidrorPizzaItaliana
{
    /// <summary>
    /// La clase ServicioCuentaUsuario se encarga de realizar las operaciones
    /// de los metodos implementados por la interfaz.
    /// Contiene todas las funcionalidades que requiere el cliente.
    /// </summary>
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Single, InstanceContextMode = InstanceContextMode.Single)]
    public class ServicioPizzaItaliana : IServicioPizzaItaliana
    {
        readonly Dictionary<ICalculatorServiceCallback, string> usuariosConectados = new Dictionary<ICalculatorServiceCallback, string>();
        private ICalculatorServiceCallback callbackChannel;

        /// <summary>
        /// Contructor que llama al cliente.

        public ServicioPizzaItaliana()
        {

        }
        /// <summary>
        /// Requiere de una operacion de la interfaz IServicioCuentaUsuario.
        /// </summary>
        /// <returns>
        /// The sum of two doubles.
        public ServicioPizzaItaliana(ICalculatorServiceCallback callbackCreator)
        {
            this.callbackChannel = callbackCreator ?? throw new ArgumentNullException("callbackCreator");
        }
        /// <summary>
        /// Requiere una tabla de cuenta usuario para agregar a la base de datos.
        /// </summary>
        /// <returns>
        /// Un objeto tipo CuentaSet al cliente para operaciones.
        /// </returns>
        /// <exception cref="System.InvalidOperationException">Thrown when one parameter is max 
        /// and the other is greater than zero.</exception>
        /// See <see cref="GuardarCuentaUsuario(CuentaSet)"/> to add integers.
        /// <param name="cuenta">un objecjt.</param>
        public void GuardarCuentaUsuario(CuentaUsuario cuentaUsuario)
        {
            try
            {
                Console.WriteLine("BDloteriaEntities");
                BDPizzaEntities db = new BDPizzaEntities();
                Console.WriteLine("BDloteriaEntities2");
                var c = (from per in db.CuentaUsuarioSet where per.nombreUsuario == cuentaUsuario.nombreUsuario select per).First();
                Console.WriteLine("Consulta");

                if (c != null)
                {

                    OperationContext.Current.GetCallbackChannel<ICalculatorServiceCallback>().Respuesta("El usuario ya ha sido registrado");
                }
            }
            catch (InvalidOperationException)
            {
                BDPizzaEntities db = new BDPizzaEntities();
                db.CuentaUsuarioSet.Add(cuentaUsuario);
                db.SaveChanges();
                Console.WriteLine(cuentaUsuario.nombreUsuario + ": Se ha registrado");
                db.Dispose();
            }
        }
        /// <summary>
        /// Requiere dos valores y regresa un objeto cuenta.
        /// </summary>
        /// <returns>
        /// objeto sitpo cuenta
        /// </returns>
        /// <exception cref="System.InvalidOperationException">Thrown when one parameter is max 
        /// and the other is greater than zero.</exception>
        /// See <see cref="IniciarSesion(string, string)"/> to add integers.
        /// <param name="nombreUsuario">un String.</param>
        /// <param name="contraseña">un String.</param>
        public void IniciarSesion(string nombreUsuario, string contraseña)
        {
            try
            {
                BDPizzaEntities db = new BDPizzaEntities();
                //db.CuentaSet.Where(d => d.nombreUsuario == nombreUsuario && d.contraseña == contraseña).First();
                db.Configuration.ProxyCreationEnabled = false;
                CuentaUsuario cuenta = (from per in db.CuentaUsuarioSet where per.nombreUsuario == nombreUsuario && per.contraseña == contraseña select per).First();
                OperationContext.Current.GetCallbackChannel<ICalculatorServiceCallback>().DevuelveCuenta(cuenta);
                var connection = OperationContext.Current.GetCallbackChannel<ICalculatorServiceCallback>();
                usuariosConectados[connection] = nombreUsuario;
                /*var connection = callbackChannel;
                    _users[connection] = nombreUsuario;
                Func<ICalculatorServiceCallback> channel = () => callbackChannel;
                callbackChannel.DevuelveCuenta(cuenta);*/
                Console.WriteLine(cuenta.nombreUsuario + ": Ha iniciado sesión");
                db.Dispose();
            }
            catch (InvalidOperationException)
            {
                OperationContext.Current.GetCallbackChannel<ICalculatorServiceCallback>().Respuesta("El usuario o contraseña son erroneos");
            }
        }

        /// <summary>
        /// Requiere un objeto tipo cuenta para modificación.
        /// </summary>
        /// <returns>
        /// nada
        /// </returns>
        /// <exception cref="System.InvalidOperationException">Thrown when one parameter is max 
        /// and the other is greater than zero.</exception>
        /// See <see cref="ModificarCuentaUsuario(CuentaSet)"/> to add integers.
        /// <param name=cuenta>un object.</param>
        public void ModificarCuentaUsuario(CuentaUsuario cuenta)
        {
            try
            {
                BDPizzaEntities po = new BDPizzaEntities();
                var c = (from per in po.CuentaUsuarioSet where per.nombreUsuario == cuenta.nombreUsuario select per).First();
                c.contraseña = cuenta.contraseña;
                po.SaveChanges();
                Console.WriteLine("Se ha modificado");
                po.Dispose();
            }
            catch (InvalidOperationException)
            {
                OperationContext.Current.GetCallbackChannel<ICalculatorServiceCallback>().Respuesta("Alguno de los datos introducidos no son correctos");
            }
        }

        /// <summary>
        /// requiere de una cadena de texto y un entero para registrar puntajes.
        /// </summary>
        /// <returns>
        /// The sum of two doubles.
        /// </returns>
        /// <exception cref="System.InvalidOperationException">Thrown when one parameter is max 
        /// and the other is greater than zero.</exception>
        /// See <see cref="RegistrarPuntajeMáximo(string, int)"/> to add integers.
        /// <param name="nombreUsuario">un String.</param>
        /// <param name="puntajeMaximo">un String.</param>

        public void CerrarSesion(string nombreUsuario)
        {
            var connection = OperationContext.Current.GetCallbackChannel<ICalculatorServiceCallback>();
            usuariosConectados[connection] = nombreUsuario;
        }
    }
}
