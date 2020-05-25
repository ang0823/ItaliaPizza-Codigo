using ClienteItaliaPizza.Servicio;
using System;
using System.Linq.Expressions;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using MessageBox = System.Windows.MessageBox;

namespace ClienteItaliaPizza
{
    /// <summary>
    /// Lógica de interacción para RegistroProductos.xaml
    /// </summary>
    public partial class RegistroProductos : Window, IRegistrarProductoCallback
    {
        CuentaUsuario CuentaUsuario;
        Producto producto;
        public RegistroProductos(CuentaUsuario cuenta)
        {
            CuentaUsuario = cuenta;
            InitializeComponent();
            IniciarComboBoxes();

            UsuarioLbl.Content = cuenta.nombreUsuario;
            EstadoCb.SelectedIndex = 0;
            CategoriaCb.SelectedIndex = 0;
            RecetaCb.SelectedIndex = 0;
            GuardarBtn.IsEnabled = false;
            VaciarBtn.IsEnabled = false;
        }

        private Boolean AlgunCampoLleno()
        {
            if (NombreTxt.Text.Length > 0 || PrecioTxt.Text.Length > 0
                || CategoriaCb.SelectedIndex != 0 || RecetaCb.SelectedIndex != 0
                || DescripcionTxt.Text.Length > 0 || RestriccionesTxt.Text.Length > 0)
            {
                return true;
            }

            return false;
        }

        private Boolean CamposLlenos()
        {
            if (NombreTxt.Text.Length > 0 && PrecioTxt.Text.Length > 0 
                && CategoriaCb.SelectedIndex != 0 && RecetaCb.SelectedIndex != 0
                && DescripcionTxt.Text.Length > 0 && RestriccionesTxt.Text.Length > 0)
            {
                return true;
            }

            return false;
        }

        private void IniciarComboBoxes()
        {
            EstadoCb.Items.Insert(0, "Activado");
            EstadoCb.Items.Insert(1, "Desactivado");

            CategoriaCb.Items.Insert(0, "Seleccionar:");
            CategoriaCb.Items.Insert(1, "Perecederos");
            CategoriaCb.Items.Insert(2, "Lacteos");
            CategoriaCb.Items.Insert(3, "Embutidos");
            CategoriaCb.Items.Insert(4, "Carnes");

            RecetaCb.Items.Insert(0, "Seleccionar");
            RecetaCb.Items.Insert(1, "Pizza Salami");
            RecetaCb.Items.Insert(2, "Pizza hawaiana");
        }

        //Implementado, falta el servidor
        private void IniciarRegistro()
        {
            try
            {
                InstanceContext context = new InstanceContext(this);
                
            }
            catch
            {

            }
        }

        private void VaciarCampos()
        {
            NombreTxt.Text = "";
            PrecioTxt.Text = "";
            EstadoCb.SelectedIndex = 0;
            CategoriaCb.SelectedIndex = 0;
            RecetaCb.SelectedIndex = 0;
            DescripcionTxt.Text = "";
            RestriccionesTxt.Text = "";
            ProductoImg.Source = null;
        }

        private void AceptarBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void VaciarBtn_Click(object sender, RoutedEventArgs e)
        {
            VaciarCampos();
        }

        private void nombreTxt_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (AlgunCampoLleno())
            {
                VaciarBtn.IsEnabled = true;
            } 
            else
            {
                VaciarBtn.IsEnabled = false;
            }

            if (CamposLlenos())
            {
                GuardarBtn.IsEnabled = true;
            }
            else
            {
                GuardarBtn.IsEnabled = false;
            }
        }

        private void PrecioTxt_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (AlgunCampoLleno())
            {
                VaciarBtn.IsEnabled = true;
            }
            else
            {
                VaciarBtn.IsEnabled = false;
            }

            if (CamposLlenos())
            {
                GuardarBtn.IsEnabled = true;
            }
            else
            {
                GuardarBtn.IsEnabled = false;
            }
        }

        private void MinimoTxt_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (AlgunCampoLleno())
            {
                VaciarBtn.IsEnabled = true;
            }
            else
            {
                VaciarBtn.IsEnabled = false;
            }

            if (CamposLlenos())
            {
                GuardarBtn.IsEnabled = true;
            }
            else
            {
                GuardarBtn.IsEnabled = false;
            }
        }

        private void UnidadCb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (AlgunCampoLleno())
            {
                VaciarBtn.IsEnabled = true;
            }
            else
            {
                VaciarBtn.IsEnabled = false;
            }

            if (CamposLlenos())
            {
                GuardarBtn.IsEnabled = true;
            }
            else
            {
                GuardarBtn.IsEnabled = false;
            }
        }

        private void ActualTxt_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (AlgunCampoLleno())
            {
                VaciarBtn.IsEnabled = true;
            }
            else
            {
                VaciarBtn.IsEnabled = false;
            }

            if (CamposLlenos())
            {
                GuardarBtn.IsEnabled = true;
            }
            else
            {
                GuardarBtn.IsEnabled = false;
            }
        }

        private void CodigoTxt_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (AlgunCampoLleno())
            {
                VaciarBtn.IsEnabled = true;
            }
            else
            {
                VaciarBtn.IsEnabled = false;
            }

            if (CamposLlenos())
            {
                GuardarBtn.IsEnabled = true;
            }
            else
            {
                GuardarBtn.IsEnabled = false;
            }
        }

        private void DescripcionTxt_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (AlgunCampoLleno())
            {
                VaciarBtn.IsEnabled = true;
            }
            else
            {
                VaciarBtn.IsEnabled = false;
            }

            if (CamposLlenos())
            {
                GuardarBtn.IsEnabled = true;
            }
            else
            {
                GuardarBtn.IsEnabled = false;
            }
        }

        private void RestriccionesTxt_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (AlgunCampoLleno())
            {
                VaciarBtn.IsEnabled = true;
            }
            else
            {
                VaciarBtn.IsEnabled = false;
            }

            if (CamposLlenos())
            {
                GuardarBtn.IsEnabled = true;
            }
            else
            {
                GuardarBtn.IsEnabled = false;
            }
        }

        private void ImagenBtn_Click(object sender, RoutedEventArgs e)
        {
            var ExploradorArchivos = new OpenFileDialog();

            DialogResult RutaImagen = ExploradorArchivos.ShowDialog();

            if(RutaImagen == System.Windows.Forms.DialogResult.OK)
            {
                string path = ExploradorArchivos.FileName;
                Uri FilePath = new Uri(path);
                ProductoImg.Source = new BitmapImage(FilePath);
            }
        }

        private void CancelarBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult opcion;

            if (AlgunCampoLleno())
            {
                opcion = MessageBox.Show("¿Seguro que deseea descartar los cambios?", "Descartar cambios",
                    MessageBoxButton.OKCancel, MessageBoxImage.Question);

                if (opcion == MessageBoxResult.OK)
                {
                    VaciarCampos();
                    FuncionesComunes.MostrarVentanaPrincipal(this.CuentaUsuario);
                    this.Close();
                }
            }
            else
            {
                FuncionesComunes.MostrarVentanaPrincipal(this.CuentaUsuario);
                this.Close();
            }
        }

        private void LogoutBtn_Click(object sender, RoutedEventArgs e)
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

        public void RegistrarIngrediente(Provision provision)
        {
            throw new NotImplementedException();
        }

        public Task RegistrarIngredienteAsync(Provision provision)
        {
            throw new NotImplementedException();
        }

        private void CategoriaTxt_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (AlgunCampoLleno())
            {
                VaciarBtn.IsEnabled = true;
            }
            else
            {
                VaciarBtn.IsEnabled = false;
            }

            if (CamposLlenos())
            {
                GuardarBtn.IsEnabled = true;
            }
            else
            {
                GuardarBtn.IsEnabled = false;
            }
        }

        public void RegistroProductooRespuesta(string mensaje)
        {
            throw new NotImplementedException();
        }
    }
}
