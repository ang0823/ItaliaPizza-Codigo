using System;
using System.IO;
using System.ServiceModel;
using System.Windows;
using ClienteItaliaPizza.Servicio;


namespace ClienteItaliaPizza
{
    /// <summary>
    /// Lógica de interacción para Principal.xaml
    /// </summary>
    public partial class Principal : Window, IGenerarRespaldoCallback
    {
        CuentaUsuario1 CuentaUsuario;

        public Principal (CuentaUsuario1 cuenta)
        {
            InitializeComponent();
            CuentaUsuario = new CuentaUsuario1();
            CuentaUsuario.nombreUsuario = cuenta.nombreUsuario;
            nombreUs.Content = CuentaUsuario.nombreUsuario;
        }

        public Principal(CuentaUsuario1 cuentaUsuario)
        {
            this.cuentaUsuario = cuentaUsuario;
        }

        private void MostrarBuscarEmpledosGui()
        {
            Dispatcher.Invoke(() =>
            {
                BuscarEmpleados ventana = new BuscarEmpleados(CuentaUsuario);
                ventana.Show();
                this.Close();
            });
        }

        private void MostrarBuscarIngredienteGui()
        {
            Dispatcher.Invoke(() =>
            {
                BuscarIngrediente ventana = new BuscarIngrediente(CuentaUsuario);
                ventana.Show();
                this.Close();
            });
        }

        private void MostrarBuscarProductosGui()
        {
            Dispatcher.Invoke(() =>
            {
                BuscarProductos ventana = new BuscarProductos(CuentaUsuario);
                ventana.Show();
                this.Close();
            });
        }

        private void MostrarRegistroEmpleadosGui()
        {
            Dispatcher.Invoke(() =>
            {
                RegistroEmpleados RegistroEmpleadosGui = new RegistroEmpleados(CuentaUsuario);
                RegistroEmpleadosGui.Show();
                this.Close();
            });
        }
        private void MostrarRegistroIngredientesGui()
        {
            Dispatcher.Invoke(() =>
            {
                RegistroIngredientes RegistroIngredientesGui = new RegistroIngredientes(CuentaUsuario);
                RegistroIngredientesGui.Show();
                this.Close();
            });
        }

        private void MostrarRegistroProductosGui(object sender, RoutedEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                RegistroProductos RegistroProdcutosGui = new RegistroProductos(CuentaUsuario);
                RegistroProdcutosGui.Show();
                this.Close();
            });
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
                FuncionesComunes.CerrarSesion();
                this.Close();
            }
        }

        private void BuscarProductosBtn_Click(object sender, RoutedEventArgs e)
        {
            MostrarBuscarProductosGui();
        }

        private void ButtonRegistrarReceta_Click(object sender, RoutedEventArgs e)
        {
            Receta ventanaRegistroReceta = new Receta(CuentaUsuario);
            ventanaRegistroReceta.Show();
            this.Close();
        }

        private void ButtonGenerarInventario_Click(object sender, RoutedEventArgs e)
        {
            Inventario ventanaInvventario = new Inventario();
            ventanaInvventario.Show();
            this.Close();
        }

        private void ButtonBuscarReceta_Click(object sender, RoutedEventArgs e)
        {
            BuscarReceta ventanaBuscarReceta = new BuscarReceta(CuentaUsuario);
            ventanaBuscarReceta.Show();
            this.Close();
        }
        
        private void BuscarIngedienteBtn_Click(object sender, RoutedEventArgs e)
        {
            MostrarBuscarIngredienteGui();
        }

        private void RegistrarIngredienteBtn_Click(object sender, RoutedEventArgs e)
        {
            MostrarRegistroIngredientesGui();
        }

        private void ButtonRespaldoManual_Click(object sender, RoutedEventArgs e)
        {
            string nombreArchivo = GenerarNombreArchivoRespaldo();

            if(nombreArchivo!= null)
            {
                InstanceContext context = new InstanceContext(this);
                GenerarRespaldoClient ServidorRespaldo = new GenerarRespaldoClient(context);
                ServidorRespaldo.GenerarRespaldo(nombreArchivo);
            }
        }

        public void RespuestaGR(string mensaje)
        {
            MessageBox.Show(mensaje);
        }

        public string GenerarNombreArchivoRespaldo()
        {
            string nombreRespaldoFechaActual = DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss");
            return nombreRespaldoFechaActual;
        }
    }
}
