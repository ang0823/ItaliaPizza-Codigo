using System;

using System.ServiceModel;
using System.Windows;
using System.Windows.Media.Imaging;
using ClienteItaliaPizza.Servicio;
using ClienteItaliaPizza.Pantallas;
using System.Windows.Input;

namespace ClienteItaliaPizza
{
    [CallbackBehavior(UseSynchronizationContext = false)]

    public partial class MainWindow : Window //, IServicioPizzaItalianaCallback
    {
        private CuentaUsuario CuentaUsuario;


        public MainWindow()
        {
            InitializeComponent();
            textBoxNombreUsuario.Focus();
        }

        private void IniciarSesion()
        {

            try
            {
                InstanceContext instanceContext = new InstanceContext(this);
                //ServicioPizzaItalianaClient cliente = new ServicioPizzaItalianaClient(instanceContext);

                if (DatosCompletos())
                {
                    string nombreUsuario = textBoxNombreUsuario.Text.Trim();
                    string contraseña = passwordBoxContraseña.Password.Trim();
                    //cliente.IniciarSesion(nombreUsuario, contraseña);
                }
                else
                {
                    MessageBox.Show("Se requiere usuario y contraseña", "Campos vacios", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (EndpointNotFoundException)
            {
                MessageBox.Show("Falló la conexión con el servidor", "Error de comunicación", MessageBoxButton.OK, MessageBoxImage.Information);
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
                this.CuentaUsuario = cuenta;
                Principal ventana = new Principal(cuenta);
                ventana.Show();
                this.Close();
            });
        }

        public void Respuesta(string mensaje)
        {
            throw new NotImplementedException();
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
    }
}
