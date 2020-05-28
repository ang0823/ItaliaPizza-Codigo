using System;

using System.ServiceModel;
using System.Windows;
using ClienteItaliaPizza.Servicio;
using ClienteItaliaPizza.Pantallas;
using System.Windows.Input;
using System.Windows.Controls;

namespace ClienteItaliaPizza
{
    [CallbackBehavior(UseSynchronizationContext = false)]

    public partial class MainWindow : Window, ILoginCallback
    {
        CuentaUsuario CuentaUsuario;

        public MainWindow()
        {
            InitializeComponent();
            textBoxNombreUsuario.Focus();
        }

        private void IniciarSesion()
        {
            string Mensaje;
            string nombreUsuario = textBoxNombreUsuario.Text.Trim();
            string contraseña = passwordBoxContraseña.Password.Trim();

            try
            {
                InstanceContext instanceContext = new InstanceContext(this);
                LoginClient cliente = new LoginClient(instanceContext);

                if (DatosCompletos(nombreUsuario, contraseña))
                {
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

        private void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            IniciarSesion();
        }

        private bool DatosCompletos(string nombreUsuario, string contrasena)
        {
            bool datosValidos = false;

            if (nombreUsuario != "" && contrasena != "")
            {
                datosValidos = true;
            }
                
            return datosValidos;
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            VentanaCocina ventanaCocina = new VentanaCocina();
            ventanaCocina.Show();

            this.Close();
        }

        private void ButtonVentanaMeseros_Click_1(object sender, RoutedEventArgs e)
        {
            VentanaPedidos ventanaPedidos = new VentanaPedidos("Mesero");
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

        public void RespuestaLogin(string mensaje)
        {
            Dispatcher.Invoke(() =>
            {
                FuncionesComunes.MostrarMensajeDeError(mensaje);
            });
        }

        private void passwordBoxContraseña_PasswordChanged(object sender, RoutedEventArgs e)
        {
            PasswordBox pb = sender as PasswordBox;
            pb.Tag = (!string.IsNullOrEmpty(pb.Password)).ToString();
        }
    }
}
