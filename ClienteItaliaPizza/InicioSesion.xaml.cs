using System;

using System.ServiceModel;
using System.Windows;
using ClienteItaliaPizza.Servicio;
using ClienteItaliaPizza.Pantallas;
using System.Windows.Input;

namespace ClienteItaliaPizza
{
    [CallbackBehavior(UseSynchronizationContext = false)]

    public partial class MainWindow : Window, ILoginCallback
    {
        public MainWindow()
        {
            InitializeComponent();
            textBoxNombreUsuario.Focus();
        }

        private void IniciarSesion()
        {
            string Mensaje;

            try
            {
                InstanceContext instanceContext = new InstanceContext(this);
                LoginClient cliente = new LoginClient(instanceContext);

                if (DatosCompletos())
                {
                    string nombreUsuario = textBoxNombreUsuario.Text.Trim();
                    string contraseña = passwordBoxContraseña.Password.Trim();
                    cliente.IniciarSesion(nombreUsuario, contraseña);
                }
                else
                {
                    Mensaje = "Se requiere usuario y contraseña";
                    FuncionesComunes.MostrarMensajeDeError(Mensaje);
                }
            }
            catch (EndpointNotFoundException)
            {
                Mensaje = "Falló la conexión con el servidor";
                FuncionesComunes.MostrarMensajeDeError(Mensaje);
            } catch (InvalidOperationException error)
            {
                Mensaje = error.Message;
                FuncionesComunes.MostrarMensajeDeError(Mensaje);
            }
        }

        private void IniciarSesion(object sender, RoutedEventArgs e)
        {
            IniciarSesion();
        }

        private bool DatosCompletos()
        {
            string nombreUsuario = textBoxNombreUsuario.Text.Trim();
            string contraseña = passwordBoxContraseña.Password.Trim();
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
                FuncionesComunes.MostrarVentanaPrincipal(cuenta);
                this.Close();
            });
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            VentanaCocina ventanaCocina = new VentanaCocina();
            ventanaCocina.Show();

            this.Close();
        }

        private void ButtonVentanaMeseros_Click_1(object sender, RoutedEventArgs e)
        {
            VentanaPedidos ventanaPedidos = new VentanaPedidos();
            ventanaPedidos.Show();

            this.Close();
        }

        private void textBoxNombreUsuario_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                IniciarSesion();
            }
        }

        private void passwordBoxContraseña_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                IniciarSesion();
            }
        }

        public void LoginRespuesta(string mensaje)
        {
            Dispatcher.Invoke(() =>
            {
                FuncionesComunes.MostrarMensajeDeError(mensaje);
            });
        }
    }
}
