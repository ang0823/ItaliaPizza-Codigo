using ClienteItaliaPizza.Servicio;
using System;
using System.Collections.Generic;
using System.Linq;
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
    public partial class RegistroProductos : Window, IRegistrarProductoCallback, IObtenerRecetasCallback
    {
        CuentaUsuario CuentaUsuario;
        Producto producto;
        List<Receta1> recetas = new List<Receta1>();
        public RegistroProductos(CuentaUsuario cuenta)
        {
            CuentaUsuario = cuenta;
            InitializeComponent();
            GenerarIdProducto();
            CargarRecetas();
            IniciarComboBoxes();

            UsuarioLbl.Content = cuenta.nombreUsuario;
            EstadoCb.SelectedIndex = 1;
            CategoriaCb.SelectedIndex = 0;
            RecetaCb.SelectedIndex = 0;
            GuardarBtn.IsEnabled = false;
            VaciarBtn.IsEnabled = false;
            CodigoTxt.IsEnabled = false;
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

        private void CargarRecetas()
        {
            try
            {
                InstanceContext context = new InstanceContext(this);
                ObtenerRecetasClient ServicioRecetas = new ObtenerRecetasClient(context);

                ServicioRecetas.ObtenerRecetas();
            }
            catch(Exception exc)
            {
                FuncionesComunes.MostrarMensajeDeError(exc.Message);
            }
        }

        private byte ConvertirImagenABytes()
        {
            Uri ImageUri = new Uri(ProductoImg.Source.ToString());
            BitmapImage Imagen = new BitmapImage(ImageUri);
            byte ImagenConvertida = Convert.ToByte(Imagen);

            return ImagenConvertida;
        }

        public bool EstaActivado()
        {
            bool EstaActivado = false;

            if (EstadoCb.SelectedIndex == 1)
            {
                EstaActivado = true;
            }

            return EstaActivado;
        }

        private void GenerarIdProducto()
        {
            Random aleatorio = new Random();
            int PrimerPar = aleatorio.Next(10, 99);
            int SegundoPar = aleatorio.Next(10, 99);

            CodigoTxt.Text = PrimerPar.ToString() + SegundoPar.ToString();
        }

        private void IniciarComboBoxes()
        {
            EstadoCb.Items.Insert(0, "Desactivado");
            EstadoCb.Items.Insert(1, "Activado");

            CategoriaCb.Items.Insert(0, "Seleccionar:");
            CategoriaCb.Items.Insert(1, "Ensaladas");
            CategoriaCb.Items.Insert(2, "Pizzas");
            CategoriaCb.Items.Insert(3, "Pastas");
            CategoriaCb.Items.Insert(4, "Postres");
            CategoriaCb.Items.Insert(5, "Bebidas");
        }

        private void RegistrarProductoClient()
        {
            Producto producto;
            Categoria categoria;
            try
            {
                InstanceContext context = new InstanceContext(this);
                RegistrarProductoClient ServicioRegistro = new RegistrarProductoClient(context);
                producto = new Producto();
                producto.nombre = NombreTxt.Text;
                producto.precioUnitario = double.Parse(PrecioTxt.Text.Trim());
                producto.imagen = ConvertirImagenABytes();
                producto.activado = EstaActivado();
                producto.descripcion = DescripcionTxt.Text;
                producto.restricciones = RestriccionesTxt.Text;

                categoria = new Categoria();
                categoria.Id = CategoriaCb.SelectedIndex;
                categoria.categoria = CategoriaCb.SelectedItem.ToString();

                ServicioRegistro.RegistrarProducto(producto, categoria);
            }
            catch (Exception e)
            {
                FuncionesComunes.MostrarMensajeDeError(e.Message);
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

        public void RespuestaRP(string mensaje)
        {
            FuncionesComunes.MostrarMensajeExitoso(mensaje);
        }

        private void GuardarBtn_Click(object sender, RoutedEventArgs e)
        {
            RegistrarProductoClient();
        }

        public void DatosRecuperados(ProductoDePedido[] productos, ProvisionVentaDirecta[] provisiones, EstadoDePedido[] estados, MesaLocal[] mesas)
        {
            throw new NotImplementedException();
        }

        public void MensajeRegistrarPedidoLocal(string mensaje)
        {
            throw new NotImplementedException();
        }

        public void DevuelveRecetas(Receta1[] receta, Ingrediente1[] ingredientes)
        {
            Dispatcher.Invoke(() => {
            for (int i = 0; i < receta.Length; i++)
                {
                    recetas.Add(receta[i]);
                    RecetaCb.Items.Add(receta[i].nombreReceta);
                }
            });
        }

        public void RespuestaIOR(string mensaje)
        {
            throw new NotImplementedException();
        }
    }
}
