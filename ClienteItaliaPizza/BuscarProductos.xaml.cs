using ClienteItaliaPizza.Servicio;
using System;
using System.Drawing;
using System.IO;
using System.ServiceModel;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;
using MessageBox = System.Windows.MessageBox;

namespace ClienteItaliaPizza
{
    /// <summary>
    /// Lógica de interacción para BuscarEmpleado.xaml
    /// </summary>
    public partial class BuscarProductos : Window, IBuscarProductoCallback, IModificarProductoCallback
    {
        CuentaUsuario1 CuentaUsuario;
        Producto producto = new Producto();
        ProvisionVentaDirecta productoExterno = new ProvisionVentaDirecta();
        Categoria categoria = new Categoria();
        byte[] imagenProducto;
        string nombreReceta;
        readonly bool imagenEditada = true;
        bool enEdicion = false;
        
        public BuscarProductos(CuentaUsuario1 cuenta)
        {
            CuentaUsuario = cuenta;
            InitializeComponent();
            InicilizarComboBoxes();
            DeshabilitarModoEdicion();

            SearchBox.Focus();
            UserLbl.Content = cuenta.nombreUsuario;
            recetaUbicacionLbl.Content = "Receta:";
            criterioCb.SelectedIndex = 0;
            ImagenBtn.Visibility = Visibility.Hidden;
            EditSaveBtn.IsEnabled = false;
        }

        private void InicilizarComboBoxes()
        {
            criterioCb.Items.Insert(0, "Producto interno");
            criterioCb.Items.Insert(1, "Producto externo");

            tipoProductoCb.Items.Insert(0, "Interno");
            tipoProductoCb.Items.Insert(1, "Externo");

            estadoCb.Items.Insert(0, "Desactivado");
            estadoCb.Items.Insert(1, "Activado");

            CategoriaCb.Items.Insert(0, string.Empty);
            CategoriaCb.Items.Insert(1, "Ensaladas");
            CategoriaCb.Items.Insert(2, "Pizzas");
            CategoriaCb.Items.Insert(3, "Pastas");
            CategoriaCb.Items.Insert(4, "Postres");
            CategoriaCb.Items.Insert(5, "Bebidas");

            recetaCb.Items.Insert(0, string.Empty);

            UnidadMedidaCb.Items.Insert(0, string.Empty);
            UnidadMedidaCb.Items.Insert(1, "Kg");
            UnidadMedidaCb.Items.Insert(2, "Gr");
            UnidadMedidaCb.Items.Insert(3, "Oz");
            UnidadMedidaCb.Items.Insert(4, "Lt");
            UnidadMedidaCb.Items.Insert(5, "Pza");
        }

        private void DeshabilitarModoEdicion()
        {
            ImagenBtn.Visibility = Visibility.Hidden;
            tipoProductoCb.IsEnabled = false;
            nombreTxt.IsEnabled = false;
            precioTxt.IsEnabled = false;
            estadoCb.IsEnabled = false;
            CategoriaCb.IsEnabled = false;
            recetaCb.IsEnabled = false;
            UbicacionTxt.IsEnabled = false;
            ExistenciasTxt.IsEnabled = false;
            StockMinTxt.IsEnabled = false;
            UnidadMedidaCb.IsEnabled = false;
            DescripcionTxt.IsEnabled = false;
            RestriccionesTxt.IsEnabled = false;
            enEdicion = false;
        }

        private void LogoutBtn_Click(object sender, RoutedEventArgs e)
        {
            string titulo = "Cerrar sesión";
            string pregunta = "¿Seguro que deseas cerrar la sesión?";
            bool salir = FuncionesComunes.ConfirmarOperacion(titulo, pregunta);

            if (salir)
            {
                FuncionesComunes.CerrarSesion();
                this.Close();
            }
        }

        private void SearchBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                BuscarProducto();
            }
        }

        private void SearchBtn_Click(object sender, RoutedEventArgs e)
        {
            BuscarProducto();
        }

        private void BuscarProducto()
        {
            if (SearchBox.Text.Length > 0)
            {
                InstanceContext context = new InstanceContext(this);
                BuscarProductoClient ServicioBuscar = new BuscarProductoClient(context);

                try
                {
                    string tipoProducto = criterioCb.SelectedItem.ToString();
                    string nombreProducto = SearchBox.Text;
                    SearchBox.Text = "";
                    SearchBox.Focus();
                    if (tipoProducto == "Producto interno")
                    {
                        ServicioBuscar.BuscarProductoInternoPorNombre(nombreProducto);
                    }
                    else if (tipoProducto == "Producto externo")
                    {
                        ServicioBuscar.BuscarProductoExternoPorNombre(nombreProducto);
                    }
                }
                catch (Exception any)
                {
                    FuncionesComunes.MostrarMensajeDeError(any.Message + " " + any.GetType());
                }
            }
        }

        public void ProductoInterno([MessageParameter(Name = "productoInterno")] Producto productoInterno, byte[] imagen, string nombreReceta, string categoria)
        {
            Dispatcher.Invoke(() =>
            {
                imagenProducto = imagen;
                producto = productoInterno;
                this.categoria.categoria = categoria;
                this.nombreReceta = nombreReceta;
                EstablecerDatosProductoInterno();
            });
        }

        private void EstablecerDatosProductoInterno()
        {
            tipoProductoCb.SelectedIndex = 0;
            nombreTxt.Text = producto.nombre;
            precioTxt.Text = producto.precioUnitario.ToString();
            estadoCb.SelectedIndex = EstaActivado(producto.activado);
            CategoriaCb.SelectedIndex = CategoriaProducto(categoria.categoria);
            EstablecerReceta(nombreReceta);
            DescripcionTxt.Text = producto.descripcion;
            RestriccionesTxt.Text = producto.restricciones;
            EstablecerImagenProducto(imagenProducto);
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

        private int CategoriaProducto(string nombreCategoria)
        {
            int categoriaIndex = 0;

            switch (nombreCategoria)
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

        private void EstablecerReceta(string nombreReceta)
        {
               recetaCb.Items.Insert(0, nombreReceta);
               recetaCb.SelectedIndex = 0;
        }

        private void EstablecerImagenProducto(byte[] arrayImagen)
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

            ProductoImg.Source = image;
        }

        public void ProductoExterno([MessageParameter(Name = "productoExterno")] ProvisionVentaDirecta productoExterno1)
        {
            Dispatcher.Invoke(() =>
            {
                productoExterno = productoExterno1;
                imagenProducto = productoExterno1.imagen;
                EstablecerDatosProductoExterno();
            });
        }

        private void EstablecerDatosProductoExterno()
        {
            tipoProductoCb.SelectedIndex = 1;
            nombreTxt.Text = productoExterno.nombre;
            precioTxt.Text = productoExterno.precioUnitario.ToString();
            estadoCb.SelectedIndex = EstaActivado(productoExterno.activado);
            CategoriaCb.SelectedIndex = CategoriaProducto(productoExterno.categoria);
            UbicacionTxt.Text = productoExterno.ubicacion;
            ExistenciasTxt.Text = productoExterno.cantidadExistencias.ToString();
            StockMinTxt.Text = productoExterno.stock.ToString();
            UnidadMedidaCb.SelectedItem = productoExterno.unidadDeMedida;
            DescripcionTxt.Text = productoExterno.descripcion;
            RestriccionesTxt.Text = productoExterno.restricciones;
            EstablecerImagenProducto(productoExterno.imagen);
        }

        public void ErrorAlRecuperarProducto(string mensajeError)
        {
            FuncionesComunes.MostrarMensajeDeError(mensajeError);
        }

        private void EditSaveBtn_Click(object sender, RoutedEventArgs e)
        {
            if (enEdicion)
            {
                EditSaveBtn.Content = "Editar";
                DeshabilitarModoEdicion();
                ActualizarInfoProducto();
            }
            else
            {
                if (tipoProductoCb.SelectedIndex == 0)
                {
                    ObtenerRecetasDeServidor();
                }
                EditSaveBtn.Content = "Guardar";
                HabilitarCampos();
            }
        }

        private void ActualizarInfoProducto()
        {
            string titulo = "Guardar";
            string pregunta = "¿Guardar cambios en la información del producto?";
            bool guardar = FuncionesComunes.ConfirmarOperacion(titulo, pregunta);

            if (guardar)
            {
                ActualizarInformacion();
            }
        }

        private void ActualizarInformacion()
        {
            try
            {
                InstanceContext context = new InstanceContext(this);
                ModificarProductoClient servicioProduto = new ModificarProductoClient(context);
                string tipoProducto = tipoProductoCb.SelectedItem.ToString();

                if (tipoProducto == "Interno")
                {
                    string nombreReceta = recetaCb.SelectedItem.ToString();
                    string nombreProducoAntes = producto.nombre;
                    IniciarProductoInternoAEnviar();

                    servicioProduto.ModificarProductoInterno(producto, nombreReceta, nombreProducoAntes, imagenProducto);
                }
                else if (tipoProducto == "Externo")
                {
                    string nombreProducoAntes = productoExterno.nombre;
                    IniciarProductoExternoAEnviar();

                    servicioProduto.ModificarProductoExterno(productoExterno, nombreProducoAntes);
                }
            }
            catch (FormatException)
            {
                FuncionesComunes.MostrarMensajeDeError("Se encontraron caracteres invalidos." +
                    "\nLos campos marcados con asterisco (*) solo aceptan números.");
                HabilitarCampos();
            }
            catch (OverflowException)
            {
                FuncionesComunes.MostrarMensajeDeError("Alguno de los valores núméricos ingresados es demasiado grande");
                HabilitarCampos();
            }
        }

        private void IniciarProductoInternoAEnviar()
        {
            try
            {
                producto.nombre = nombreTxt.Text;
                producto.precioUnitario = double.Parse(precioTxt.Text);
                producto.activado = Convert.ToBoolean(estadoCb.SelectedIndex);
                categoria.categoria = CategoriaCb.SelectedItem.ToString();
                producto.Categoria = categoria;
                producto.descripcion = DescripcionTxt.Text;
                producto.restricciones = RestriccionesTxt.Text;
            }
            catch (FormatException)
            {
                throw new FormatException();
            }
            catch (OverflowException)
            {
                throw new OverflowException();
            }
        }

        private void IniciarProductoExternoAEnviar()
        {
            try
            {
                productoExterno.nombre = nombreTxt.Text;
                productoExterno.precioUnitario = double.Parse(precioTxt.Text);
                producto.activado = Convert.ToBoolean(estadoCb.SelectedIndex);
                productoExterno.categoria = CategoriaCb.SelectedItem.ToString();
                productoExterno.ubicacion = UbicacionTxt.Text;
                productoExterno.cantidadExistencias = int.Parse(ExistenciasTxt.Text);
                productoExterno.stock = int.Parse(StockMinTxt.Text);
                productoExterno.unidadDeMedida = UnidadMedidaCb.SelectedItem.ToString();
                productoExterno.descripcion = DescripcionTxt.Text;
                productoExterno.restricciones = RestriccionesTxt.Text;
                productoExterno.imagen = imagenProducto;
            }
            catch (FormatException)
            {
                throw new FormatException();
            }
            catch (OverflowException)
            {
                throw new OverflowException();
            }
        }

        private void ObtenerRecetasDeServidor()
        {
            InstanceContext context = new InstanceContext(this);
            ModificarProductoClient servicioModificar = new ModificarProductoClient(context);

            try
            {
                servicioModificar.ObtenerNombresDeRecetas();
            }
            catch (Exception e)
            {
                FuncionesComunes.MostrarMensajeDeError("Ocurrio la excepcion: " + e.GetType() + " al recuperar recetas " +
                    "\nMensaje: " + e.Message );
            }
        }

        public void ListaDeRecetas(string[] nombreDeRecetas)
        {
            Array.Sort(nombreDeRecetas);
            foreach(var receta in nombreDeRecetas)
            {
                recetaCb.Items.Add(receta);
            }
        }

        private void HabilitarCampos()
        {
            enEdicion = true;
            ImagenBtn.Visibility = Visibility.Visible;
            tipoProductoCb.IsEnabled = true;
            nombreTxt.IsEnabled = true;
            precioTxt.IsEnabled = true;
            estadoCb.IsEnabled = true;
            if (tipoProductoCb.SelectedIndex == 0)
            {
                CategoriaCb.IsEnabled = true;
            }
            UbicacionTxt.IsEnabled = true;
            recetaCb.IsEnabled = true;
            ExistenciasTxt.IsEnabled = true;
            StockMinTxt.IsEnabled = true;
            UnidadMedidaCb.IsEnabled = true;
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

        private void tipoProductoCb_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (tipoProductoCb.SelectedIndex == 0)
            {
                recetaUbicacionLbl.Content = "Receta:";
                OcultarCamposProductoExterno();
                CategoriaCb.IsEnabled = true;
                recetaCb.Visibility = Visibility.Visible;
                ExistenciasTxt.Visibility = Visibility.Hidden;
            }
            else
            {
                recetaUbicacionLbl.Content = "Ubicación:";
                MostrarCamposProductoExterno();
                recetaCb.Visibility = Visibility.Hidden;
                ExistenciasTxt.Visibility = Visibility.Visible;
            }
        }

        private void OcultarCamposProductoExterno()
        {
            CategoriaCb.SelectedIndex = CategoriaProducto(categoria.categoria);
            UbicacionTxt.Visibility = Visibility.Hidden;
            ExistenciasLbl.Visibility = Visibility.Hidden;
            ExistenciasTxt.Visibility = Visibility.Hidden;
            StockMinLbl.Visibility = Visibility.Hidden;
            StockMinTxt.Visibility = Visibility.Hidden;
            UnidadMedidaLbl.Visibility = Visibility.Hidden;
            UnidadMedidaCb.Visibility = Visibility.Hidden;
        }

        private void MostrarCamposProductoExterno()
        {
            CategoriaCb.IsEnabled = false;
            CategoriaCb.SelectedIndex = 5;
            UbicacionTxt.Visibility = Visibility.Visible;
            ExistenciasLbl.Visibility = Visibility.Visible;
            ExistenciasTxt.Visibility = Visibility.Visible;
            StockMinLbl.Visibility = Visibility.Visible;
            StockMinTxt.Visibility = Visibility.Visible;
            UnidadMedidaLbl.Visibility = Visibility.Visible;
            UnidadMedidaCb.Visibility = Visibility.Visible;
        }

        private void CategoriaCb_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (CamposLlenos())
            {
                EditSaveBtn.IsEnabled = true;
            }
            else
            {
                EditSaveBtn.IsEnabled = false;
            }
        }

        private void ActivarDesactivarEditSaveBtn(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (CamposLlenos())
            {
                EditSaveBtn.IsEnabled = true;
            }
            else
            {
                EditSaveBtn.IsEnabled = false;
            }
        }

        private bool CamposLlenos()
        {
            string tipoProducto = tipoProductoCb.SelectedItem.ToString();

            if(tipoProducto == "Interno")
            {
                if (nombreTxt.Text.Length > 0 && precioTxt.Text.Length > 0 && CategoriaCb.SelectedIndex != 0
                && (recetaCb.SelectedIndex == 0 || recetaCb.SelectedIndex != 0)
                && DescripcionTxt.Text.Length > 0 && RestriccionesTxt.Text.Length > 0)
                {
                    return true;
                }
            }
            else
            {
                if (nombreTxt.Text.Length > 0 && precioTxt.Text.Length > 0 && CategoriaCb.SelectedIndex != 0
                && UbicacionTxt.Text.Length > 0 && ExistenciasTxt.Text.Length > 0 && StockMinTxt.Text.Length > 0
                && UnidadMedidaCb.SelectedIndex != 0 && DescripcionTxt.Text.Length > 0 && RestriccionesTxt.Text.Length > 0)
                {
                    return true;
                }
            }

            return false;
        }

        private void SeleccionarImagen(object sender, RoutedEventArgs e)
        {
            OpenFileDialog exploradorArchivos = new OpenFileDialog();
            exploradorArchivos.Filter = "*.jpg | *.jpg";
            exploradorArchivos.Title = "Imagen del producto";
            exploradorArchivos.RestoreDirectory = true;

            DialogResult rutaImagen = exploradorArchivos.ShowDialog();

            if (rutaImagen == System.Windows.Forms.DialogResult.OK)
            {
                string imagePath = exploradorArchivos.FileName;
                Uri FilePath = new Uri(imagePath);
                ProductoImg.Source = new BitmapImage(FilePath);

                Stream bytesImagen = exploradorArchivos.OpenFile();
                using (MemoryStream ms = new MemoryStream())
                {
                    bytesImagen.CopyTo(ms);
                    imagenProducto = ms.ToArray();
                }
            }
        }

        public void RespuestaModificarProducto(string mensajeError)
        {
            if(mensajeError == "Cambios Guardados")
            {
                FuncionesComunes.MostrarMensajeExitoso(mensajeError);
                ActualizarInfoProducto();
                recetaCb.Items.Clear();
                recetaCb.Items.Add(nombreReceta);
            }
            else
            {
                FuncionesComunes.MostrarMensajeDeError(mensajeError);
            }
        }
    }
}
