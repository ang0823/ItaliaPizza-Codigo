using System;

using System.ServiceModel;
using System.Windows;
using ClienteItaliaPizza.Servicio;

namespace ClienteItaliaPizza
{
    [CallbackBehavior(UseSynchronizationContext = false)]

    public partial class MainWindow : Window, IServicioPizzaItalianaCallback
    {
        private CuentaUsuario CuentaUsuario;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void IniciarSesion(object sender, RoutedEventArgs e)
        {
            string nombreUsuario = textBoxNombreUsuario.Text.Trim();
            string contraseña = passwordBoxContraseña.Password.Trim();

            try
            {
                InstanceContext instanceContext = new InstanceContext(this);
                ServicioPizzaItalianaClient cliente = new ServicioPizzaItalianaClient(instanceContext);

                if (ValidarDatosIngresados(nombreUsuario, contraseña))
                {
                    cliente.IniciarSesion(nombreUsuario, contraseña);

                }
                else
                {
                    MessageBox.Show("");
                }
            }
            catch (EndpointNotFoundException)
            {
                MessageBox.Show("");
            }
        }

        private bool ValidarDatosIngresados(string nombreUsuario, string contraseña)
        {
            bool datosValidos = false;

            if (nombreUsuario != "" && contraseña != "")
            {
                datosValidos = true;
                return datosValidos;
            }
            else
            {
                return datosValidos;
            }
        }

        public void DevuelveCuenta(CuentaUsuario cuenta)
        {
            Dispatcher.Invoke(() =>
            {
                CuentaUsuario = cuenta;
                Principal ventana = new Principal(cuenta);
                ventana.Show();
                this.Close();
            });
        }

        public void Respuesta(string mensaje)
        {
            throw new NotImplementedException();
        }
    }
}
