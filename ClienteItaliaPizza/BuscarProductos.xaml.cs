using ClienteItaliaPizza.Servicio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ClienteItaliaPizza
{
    /// <summary>
    /// Lógica de interacción para BuscarEmpleado.xaml
    /// </summary>
    public partial class BuscarProductos : Window
    {
        CuentaUsuario CuentaUsuario;

        public BuscarProductos(CuentaUsuario cuenta)
        {
            CuentaUsuario = cuenta;
            InitializeComponent();

            EditarBtn.IsEnabled = false;
            ImagenBtn.Visibility = Visibility.Hidden;
        }

        private void MostrarVentanaPrincipal()
        {
            Dispatcher.Invoke(() =>
            {
                Principal ventana = new Principal(CuentaUsuario);
                ventana.Show();
                this.Close();
            });
        }

        private void CancelarBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult opcion;

            opcion = MessageBox.Show("¿Seguro que deseeas volver a la pantalla principal?", "Cancelar",
                MessageBoxButton.OKCancel, MessageBoxImage.Question);

            if (opcion == MessageBoxResult.OK)
            {
                MostrarVentanaPrincipal();
            }
        }
    }
}
