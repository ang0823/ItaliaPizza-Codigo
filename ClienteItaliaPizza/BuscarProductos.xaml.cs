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

            ImagenBtn.Visibility = Visibility.Hidden;
            //EditSaveBtn.IsEnabled = false;
            DeshabilitarCampos();
        }

        private Boolean CamposVacios()
        {
            if (nombreTxt.Text.Length > 0 && PrecioTxt.Text.Length > 0 && MinimoTxt.Text.Length > 0
                && UnidadCb.SelectedIndex != 0 && ActualTxt.Text.Length > 0 && UbicacionTxt.Text.Length > 0
                && CodigoTxt.Text.Length > 0 && DescripcionTxt.Text.Length > 0 && RestriccionesTxt.Text.Length > 0)
            {
                return false;
            }

            return true;
        }

        private void DeshabilitarCampos()
        {
            ImagenBtn.Visibility = Visibility.Hidden;
            nombreTxt.IsEnabled = false;
            PrecioTxt.IsEnabled = false;
            MinimoTxt.IsEnabled = false;
            UnidadCb.IsEnabled = false;
            ActualTxt.IsEnabled = false;
            UbicacionTxt.IsEnabled = false;
            CodigoTxt.IsEnabled = false;
            DescripcionTxt.IsEnabled = false;
            RestriccionesTxt.IsEnabled = false;
        }

        private void HabilitarEdicion()
        {
            ImagenBtn.Visibility = Visibility.Visible;
            nombreTxt.IsEnabled = true;
            PrecioTxt.IsEnabled = true;
            MinimoTxt.IsEnabled = true;
            UnidadCb.IsEnabled = true;
            ActualTxt.IsEnabled = true;
            UbicacionTxt.IsEnabled = true;
            CodigoTxt.IsEnabled = true;
            DescripcionTxt.IsEnabled = true;
            RestriccionesTxt.IsEnabled = true;
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

        private void EditSaveBtn_Click(object sender, RoutedEventArgs e)
        {
            if(EditSaveBtn.Content.ToString() == "Editar")
            {
                EditSaveBtn.Content = "Guardar";
                HabilitarEdicion();
            }
            else
            {
                if(CamposVacios())
                {
                    MessageBoxResult opcion;
                    opcion = MessageBox.Show("No se puede dejar campos vacíos", "Información",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                } 
                else
                {
                    EditSaveBtn.Content = "Editar";
                    DeshabilitarCampos();
                }
            }
        }
    }
}
