using System;
using System.Windows;
using ClienteItaliaPizza.Pantallas;
using ClienteItaliaPizza.Servicio;


namespace ClienteItaliaPizza
{
    /// <summary>
    /// Lógica de interacción para Principal.xaml
    /// </summary>
    public partial class Principal : Window
    {
        CuentaUsuario CuentaUsuario;
        public Principal(CuentaUsuario cuenta)
        {
            InitializeComponent();
            CuentaUsuario = cuenta;
            nombreUs.Content = CuentaUsuario.nombreUsuario;
        }

        private void CerrarSesion()
        {
            Dispatcher.Invoke(() =>
            {
                MainWindow ventana = new MainWindow();
                ventana.Show();
                this.Close();
            });
        }

        private void MostrarBuscarEmpledosGui()
        {
            BuscarEmpleados ventana = new BuscarEmpleados(CuentaUsuario);
            ventana.Show();
            this.Close();
        }

        private void MostrarBuscarProductosGui()
        {
            BuscarProductos ventana = new BuscarProductos(CuentaUsuario);
            ventana.Show();
            this.Close();
        }

        private void MostrarRegistroEmpleadosGui()
        {
            RegistroEmpleados RegistroEmpleadosGui = new RegistroEmpleados(CuentaUsuario);
            RegistroEmpleadosGui.Show();
            this.Close();
        }

        private void MostrarRegistroProductosGui(object sender, RoutedEventArgs e)
        {
            RegistroProductos RegistroProdcutosGui = new RegistroProductos(CuentaUsuario);
            RegistroProdcutosGui.Show();
            this.Close();
        }

        private void registrarEmpBtn_Click(object sender, RoutedEventArgs e)
        {
            MostrarRegistroEmpleadosGui();
        }

        private void buscarEmpBtn_Click(object sender, RoutedEventArgs e)
        {
            MostrarBuscarEmpledosGui();
        }

        private void CerrarSesionBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult opcion;

            opcion = MessageBox.Show("¿Seguro que deseas cerrar la sesión?", "Cerrar sesión",
                    MessageBoxButton.OKCancel, MessageBoxImage.Question);

            if (opcion == MessageBoxResult.OK)
            {
                CerrarSesion();
            }
        }

        private void BuscarProductosBtn_Click(object sender, RoutedEventArgs e)
        {
            MostrarBuscarProductosGui();
        }
    }
}
