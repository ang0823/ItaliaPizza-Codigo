using ClienteItaliaPizza.Servicio;
using System;
using System.Windows;
using System.Windows.Input;

namespace ClienteItaliaPizza
{
    /// <summary>
    /// Lógica de interacción para BuscarEmpleado.xaml
    /// </summary>
    public partial class BuscarProductos : Window
    {
        CuentaUsuario1 CuentaUsuario;

        public BuscarProductos(CuentaUsuario1 cuenta)
        {
            CuentaUsuario = cuenta;
            InitializeComponent();

            UserLbl.Content = cuenta.nombreUsuario;
            ImagenBtn.Visibility = Visibility.Hidden;
            DeshabilitarCampos();
            //EditSaveBtn.IsEnabled = false;
        }

        private void BuscarProducto()
        {

        }

        private Boolean CamposVacios()
        {
            if (nombreTxt.Text.Length > 0 && PrecioTxt.Text.Length > 0 && MinimoTxt.Text.Length > 0
                && ActualTxt.Text.Length > 0 && UbicacionTxt.Text.Length > 0
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
            ActualTxt.IsEnabled = true;
            UbicacionTxt.IsEnabled = true;
            CodigoTxt.IsEnabled = true;
            DescripcionTxt.IsEnabled = true;
            RestriccionesTxt.IsEnabled = true;
        }

        private void CancelarBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult opcion;

            opcion = MessageBox.Show("¿Seguro que deseeas volver a la pantalla principal?", "Cancelar",
                MessageBoxButton.OKCancel, MessageBoxImage.Question);

            if (opcion == MessageBoxResult.OK)
            {
                CamposVacios();
                FuncionesComunes.MostrarVentanaPrincipal(CuentaUsuario);
                this.Close();
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
                MessageBoxResult opcion;

                if(CamposVacios())
                {
                    opcion = MessageBox.Show("No se puede dejar campos vacíos", "Información",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                } 
                else
                {
                    opcion = MessageBox.Show("¿Guardar cambios en la información del producto?", "Guardar",
                    MessageBoxButton.OKCancel, MessageBoxImage.Information);

                    if(opcion == MessageBoxResult.OK)
                    {
                        EditSaveBtn.Content = "Editar";
                        DeshabilitarCampos();
                    }
                }
            }
        }

        private void SearchBox_KeyDown(object sender, KeyEventArgs e)
        {
            if(SearchBox.Text.Length > 0 && e.Key == Key.Return)
            {
                BuscarProducto();
            }
        }

        private void LogoutBtn_Click(object sender, RoutedEventArgs e)
        {
            FuncionesComunes.CerrarSesion();
        }
    }
}
