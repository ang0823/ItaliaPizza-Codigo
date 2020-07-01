using ClienteItaliaPizza.Servicio;
using System;
using System.IO;
using System.ServiceModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace ClienteItaliaPizza
{
    /// <summary>
    /// Lógica de interacción para BuscarEmpleado.xaml
    /// </summary>
    public partial class BuscarProductos : Window, IBuscarProductoCallback
    {
        CuentaUsuario1 CuentaUsuario;
        bool enEdicion = false;

        public BuscarProductos(CuentaUsuario1 cuenta)
        {
            CuentaUsuario = cuenta;
            InitializeComponent();
            InicilizarComboBoxes();
            DeshabilitarCampos();

            UserLbl.Content = cuenta.nombreUsuario;
            criterioCb.SelectedIndex = 0;
            ImagenBtn.Visibility = Visibility.Hidden;
            //EditSaveBtn.IsEnabled = false;
        }

        private void InicilizarComboBoxes()
        {
            criterioCb.Items.Insert(0, "Producto interno");
            criterioCb.Items.Insert(1, "Producto externo");

            tipoProductoCb.Items.Insert(0, "Interno");
            tipoProductoCb.Items.Insert(1, "Externo");

            estadoCb.Items.Insert(0, "Desactivado");
            estadoCb.Items.Insert(1, "Activado");

            CategoriaCb.Items.Insert(0, "Seleccionar");
            CategoriaCb.Items.Insert(1, "Ensaladas");
            CategoriaCb.Items.Insert(2, "Pizzas");
            CategoriaCb.Items.Insert(3, "Pastas");
            CategoriaCb.Items.Insert(4, "Postres");
            CategoriaCb.Items.Insert(5, "Bebidas");
        }

        private void DeshabilitarCampos()
        {
            ImagenBtn.Visibility = Visibility.Hidden;
            tipoProductoCb.IsEnabled = false;
            nombreTxt.IsEnabled = false;
            precioTxt.IsEnabled = false;
            estadoCb.IsEnabled = false;
            CategoriaCb.IsEnabled = false;
            recetaCb.IsEnabled = false;
            DescripcionTxt.IsEnabled = false;
            RestriccionesTxt.IsEnabled = false;
        }

        // FALTA LA CORRECCIÓN DEL SERVIDOR PARA TERMINAR ESTO
        private void BuscarProducto(object sender, KeyEventArgs e)
        {
            if (SearchBox.Text.Length > 0 && e.Key == Key.Return)
            {
                InstanceContext context = new InstanceContext(this);
                BuscarProductoClient ServicioBuscar = new BuscarProductoClient(context);

                try
                {
                    int idProducto = int.Parse(SearchBox.Text);
                    // ServicioBuscar(idProducto);
                }
                catch (FormatException)
                {
                    string nombreProducto = SearchBox.Text;
                    ServicioBuscar.BuscarProductoInternoPorNombre(nombreProducto);
                }
                catch (Exception any)
                {
                    FuncionesComunes.MostrarMensajeDeError(any.Message + " " + any.GetType());
                }
            }
        }

        private Boolean CamposVacios()
        {
            if (nombreTxt.Text.Length > 0 && precioTxt.Text.Length > 0
                && CategoriaCb.SelectedIndex != 0 && DescripcionTxt.Text.Length > 0 && RestriccionesTxt.Text.Length > 0)
            {
                return false;
            }

            return true;
        }

        private void EditarInfoProducto()
        {
            MessageBoxResult opcion;

            if (CamposVacios())
            {
                opcion = MessageBox.Show("No se puede dejar campos vacíos", "Información",
                MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                opcion = MessageBox.Show("¿Guardar cambios en la información del producto?", "Guardar",
                MessageBoxButton.OKCancel, MessageBoxImage.Information);

                if (opcion == MessageBoxResult.OK)
                {
                    EditSaveBtn.Content = "Editar";
                    DeshabilitarCampos();
                }
            }
        }

        /*private int ProductoExternoAcitvado(ProvisionDirecta1 provisionDirecta)
        {
            int EstaActivo = 0;

            if (provisionDirecta.activado)
            {
                EstaActivo = 1;
            }

            return EstaActivo;
        }*/

        private void HabilitarCampos()
        {
            ImagenBtn.Visibility = Visibility.Visible;
            tipoProductoCb.IsEnabled = true;
            nombreTxt.IsEnabled = true;
            precioTxt.IsEnabled = true;
            estadoCb.IsEnabled = true;
            CategoriaCb.IsEnabled = true;
            recetaCb.IsEnabled = true;
            DescripcionTxt.IsEnabled = true;
            RestriccionesTxt.IsEnabled = true;
        }

        private void CancelarBtn_Click(object sender, RoutedEventArgs e)
        {
            if (enEdicion)
            {
                MessageBoxResult opcion;

                opcion = MessageBox.Show("¿Seguro que deseeas volver a la pantalla principal?", "Cancelar",
                    MessageBoxButton.OKCancel, MessageBoxImage.Question);

                if (opcion == MessageBoxResult.OK)
                {
                    FuncionesComunes.MostrarVentanaPrincipal(CuentaUsuario);
                    this.Close();
                }
            }
            else
            {
                FuncionesComunes.MostrarVentanaPrincipal(CuentaUsuario);
                this.Close();
            }

        }

        private void EditSaveBtn_Click(object sender, RoutedEventArgs e)
        {
            if(EditSaveBtn.Content.ToString() == "Editar")
            {
                EditSaveBtn.Content = "Guardar";
                HabilitarCampos();
                enEdicion = true;
            }
            else if (EditSaveBtn.Content.ToString() == "Guardar")
            {
                EditarInfoProducto();
                EditSaveBtn.Content = "Editar";
                enEdicion = true;
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

        /*public void ProductoExterno(Provision1 provision, ProvisionDirecta1 provisionDirecta, byte[] imagen)
        {
            throw new NotImplementedException();
        }*/

        public void ErrorAlRecuperarProducto(string mensajeError)
        {
            FuncionesComunes.MostrarMensajeDeError(mensajeError);
        }

        private void tipoProductoCb_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (tipoProductoCb.SelectedIndex == 0)
            {
                recetaExistenciasLbl.Content = "Receta:";
                recetaCb.Visibility = Visibility.Visible;
                existenciasTxt.Visibility = Visibility.Hidden;
            }
            else
            {
                recetaExistenciasLbl.Content = "Existencias actuales:";
                recetaCb.Visibility = Visibility.Hidden;
                existenciasTxt.Visibility = Visibility.Visible;
            }
        }

        private void SearchBtn_Click(object sender, RoutedEventArgs e)
        {
            if (criterioCb.SelectedIndex == 0)
            {
                BuscarProductoInterno();
            }
            else
            {
                BuscarProductoExterno();
            }
        }

        private void BuscarProductoInterno()
        {
            InstanceContext context = new InstanceContext(this);
            BuscarProductoClient servicioProducto = new BuscarProductoClient(context);

            try
            {
                string nombreProducto = SearchBox.Text;
                servicioProducto.BuscarProductoInternoPorNombre(nombreProducto);
            } 
            catch(Exception e)
            {
                FuncionesComunes.MostrarMensajeDeError(e.Message + " " + e.GetType());
            }
        }

        public void ProductoInterno([MessageParameter(Name = "productoInterno")] Producto productoInterno1, byte[] imagen)
        {
            tipoProductoCb.SelectedIndex = 0;
            nombreTxt.Text = productoInterno1.nombre;
            precioTxt.Text = productoInterno1.precioUnitario.ToString();
            estadoCb.SelectedIndex = EstaActivado(productoInterno1.activado);
            CategoriaCb.SelectedIndex = CategoriaProducto(productoInterno1);
            recetaCb.SelectedItem = productoInterno1.Receta;
            DescripcionTxt.Text = productoInterno1.descripcion;
            RestriccionesTxt.Text = productoInterno1.restricciones;
            ProductoImg.Source = ObtenerImagenDeArray(imagen);
        }

        private int EstaActivado(bool productoEstaActivado)
        {
            int indexComboBox = 0;

            if (productoEstaActivado)
            {
                indexComboBox = 1;
            }

            return indexComboBox;
        }

        private int CategoriaProducto(Producto producto)
        {
            int categoriaIndex = 0;

            switch (producto.Categoria.categoria)
            {
                case "Ensaladas":
                    categoriaIndex = 1;
                    break;
                case "Pizzas":
                    categoriaIndex = 2;
                    break;
                case "Pastas":
                    categoriaIndex = 3;
                    break;
                case "Postres":
                    categoriaIndex = 4;
                    break;
                case "Bebidas":
                    categoriaIndex = 5;
                    break;
                default:
                    categoriaIndex = 0;
                    break;
            }

            return categoriaIndex;
        }

        private BitmapImage ObtenerImagenDeArray(byte[] arrayImagen)
        {
            var image = new BitmapImage();
            using (var mem = new MemoryStream(arrayImagen))
            {
                mem.Position = 0;
                image.BeginInit();
                image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.UriSource = null;
                image.StreamSource = mem;
                image.EndInit();
            }

            return image;
        }

        private void BuscarProductoExterno()
        {
            //existenciasTxt.Text = productoInterno1.Id.ToString();

        }

        public void ProductoInterno([MessageParameter(Name = "productoInterno")] Producto productoInterno1, byte[] imagen, string nombreReceta, string categoria)
        {
            throw new NotImplementedException();
        }

        public void ProductoExterno([MessageParameter(Name = "productoExterno")] ProvisionVentaDirecta productoExterno1)
        {
            throw new NotImplementedException();
        }

        /*
        public void ProductoExterno(Provision1 provision, ProvisionDirecta1 provisionDirecta)
        {
            codigoTxt.Text = provision.id.ToString();
            nombreTxt.Text = provision.nombre;
            precioTxt.Text = provision.costoUnitario.ToString();
            DescripcionTxt.Text = provisionDirecta.descripcion.ToString();
            RestriccionesTxt.Text = provisionDirecta.restricciones;
            estadoCb.SelectedIndex = EstaActivadoProductoExterno(provisionDirecta);
            recetaCb.IsEnabled = false;          
        }*/
    }
}
