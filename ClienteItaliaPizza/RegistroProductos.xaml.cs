using ClienteItaliaPizza.Servicio;
using System;
using System.Collections.Generic;
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
        CuentaUsuario1 CuentaUsuario;
        List<Receta1> recetas = new List<Receta1>();
        byte[] imagen;
        string imagePath;

        public RegistroProductos(CuentaUsuario1 cuenta)
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

        public bool ProductoActivado()
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
            RecetaCb.Items.Insert(0, "Seleccionar");

            EstadoCb.Items.Insert(0, "Desactivado");
            EstadoCb.Items.Insert(1, "Activado");

            CategoriaCb.Items.Insert(0, "Seleccionar:");
            CategoriaCb.Items.Insert(1, "Ensaladas");
            CategoriaCb.Items.Insert(2, "Pizzas");
            CategoriaCb.Items.Insert(3, "Pastas");
            CategoriaCb.Items.Insert(4, "Postres");
            CategoriaCb.Items.Insert(5, "Bebidas");
        }

        private void RegistrarProducto()
        {
            // AQUI HAY ERROR AL ASIGNAR IMAGEN AL PRODUCTO
            Producto producto = new Producto();
            Categoria categoria = new Categoria();
            int indiceReceta = RecetaCb.SelectedIndex;

            try
            {
                InstanceContext context = new InstanceContext(this);
                RegistrarProductoClient ServicioRegistro = new RegistrarProductoClient(context);

                producto.nombre = NombreTxt.Text.Trim(); //
                producto.precioUnitario = double.Parse(PrecioTxt.Text.Trim()); //
                // producto.imagen = byte.Parse(imagen.ToString());
                producto.activado = ProductoActivado(); //
                producto.descripcion = DescripcionTxt.Text; //
                producto.restricciones = RestriccionesTxt.Text; //
                
                categoria.Id = CategoriaCb.SelectedIndex;
                categoria.categoria = CategoriaCb.SelectedItem.ToString();

                ServicioRegistro.RegistrarProducto(producto, categoria, indiceReceta);
            }
            catch (Exception e)
            {
                FuncionesComunes.MostrarMensajeDeError(e.Message + " " + e.GetType());
            }

        }

        private void VaciarCampos()
        {
            NombreTxt.Text = string.Empty;
            PrecioTxt.Text = string.Empty;
            EstadoCb.SelectedIndex = 1;
            CategoriaCb.SelectedIndex = 0;
            RecetaCb.SelectedIndex = 0;
            DescripcionTxt.Text = string.Empty;
            RestriccionesTxt.Text = string.Empty;
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
            OpenFileDialog exploradorArchivos = new OpenFileDialog();
            exploradorArchivos.Filter = "*.jpg; *.png; *.bmp | *.jpg; *.png; *.bmp";
            exploradorArchivos.Title = "Imagen del producto";
            exploradorArchivos.RestoreDirectory = true;

            DialogResult rutaImagen = exploradorArchivos.ShowDialog();

            if(rutaImagen == System.Windows.Forms.DialogResult.OK)
            {
                string imagePath = exploradorArchivos.FileName;
                 Uri FilePath = new Uri(imagePath);
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
            if (mensaje == "Guardado")
            {
                FuncionesComunes.MostrarMensajeExitoso("El producto se guardó exitosamente");
                VaciarCampos();
            }
            else
            {
                FuncionesComunes.MostrarMensajeDeError(mensaje);
            }
        }

        private void GuardarBtn_Click(object sender, RoutedEventArgs e)
        {
            RegistrarProducto();
        }

        public void RespuestaIOR(string mensaje)
        {
            throw new NotImplementedException();
        }

        public void DevuelveReceta(Receta1 receta, Ingrediente1[] ingredientes)
        {
            throw new NotImplementedException();
        }

        public void DevuelveRecetas(Receta1[] recetas)
        {
            Dispatcher.Invoke(() =>
            {
                foreach (var receta in recetas)
                {
                    this.recetas.Add(receta);
                    RecetaCb.Items.Add(receta.nombreReceta);
                }
            });
        }
    }
}
