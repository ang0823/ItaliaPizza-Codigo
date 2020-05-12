using System;

using System.ServiceModel;
using System.Windows;
using System.Windows.Media.Imaging;
using ClienteItaliaPizza.Servicio;
using ClienteItaliaPizza.Pantallas;

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
                    MessageBox.Show("Se requiere usuario y contraseña", "Campos vacios", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (EndpointNotFoundException)
            {
                MessageBox.Show("Falló la conexión con el servidor", "Error de comunicación", MessageBoxButton.OK, MessageBoxImage.Information);
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
    }
}
