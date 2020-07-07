using ClienteItaliaPizza.Servicio;
using DevExpress.Utils.Serializing.Helpers;
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
        bool imageneditada = false;
        bool enEdicion = false;


        public BuscarProductos(CuentaUsuario1 cuenta)
        {
            CuentaUsuario = cuenta;
            InitializeComponent();
            InicilizarComboBoxes();
            DeshabilitarCampos();
            producto.Id = 2; // BORRAR
            UserLbl.Content = cuenta.nombreUsuario;
            recetaExistenciasLbl.Content = "Receta:";
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
            existenciasTxt.IsEnabled = false;
            DescripcionTxt.IsEnabled = false;
            RestriccionesTxt.IsEnabled = false;
            enEdicion = false;
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

        private void SearchBox_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Return)
            {
                SearchBox.IsEnabled = false;
                SearchBtn.IsEnabled = false;
                BuscarProducto();
            }
        }

        private void SearchBtn_Click(object sender, RoutedEventArgs e)
        {
            SearchBox.IsEnabled = false;
            SearchBtn.IsEnabled = false;
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
                    SearchBox.IsEnabled = true;
                    SearchBtn.IsEnabled = true;
                    FuncionesComunes.MostrarMensajeDeError(any.Message + " " + any.GetType());
                }
            }
        }

        public void ProductoInterno([MessageParameter(Name = "productoInterno")] Producto productoInterno1, byte[] imagen, string nombreReceta, string categoria)
        {
            SearchBox.IsEnabled = true;
            SearchBtn.IsEnabled = true;

            Dispatcher.Invoke(() =>
            {
                imagenProducto = imagen;
                producto = productoInterno1;
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
            SearchBox.IsEnabled = true;
            SearchBtn.IsEnabled = true;
            Dispatcher.Invoke(() =>
            {
                productoExterno = productoExterno1;
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
            existenciasTxt.Text = productoExterno.cantidadExistencias.ToString();
            DescripcionTxt.Text = productoExterno.descripcion;
            RestriccionesTxt.Text = productoExterno.restricciones;
            EstablecerImagenProducto(productoExterno.imagen);
        }

        public void ErrorAlRecuperarProducto(string mensajeError)
        {
            SearchBox.IsEnabled = true;
            SearchBtn.IsEnabled = true;
            FuncionesComunes.MostrarMensajeDeError(mensajeError);
        }

        private void EditSaveBtn_Click(object sender, RoutedEventArgs e)
        {
            if (enEdicion)
            {
                EditarInfoProducto();
                EditSaveBtn.Content = "Editar";
                DeshabilitarCampos();
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

        private void CargarRecetasEnComboBox()
        {
            // FALTA IMPLEMETAR
        }

        private void HabilitarCampos()
        {
            enEdicion = true;
            ImagenBtn.Visibility = Visibility.Visible;
            tipoProductoCb.IsEnabled = true;
            nombreTxt.IsEnabled = true;
            precioTxt.IsEnabled = true;
            estadoCb.IsEnabled = true;
            CategoriaCb.IsEnabled = true;
            recetaCb.IsEnabled = true;
            existenciasTxt.IsEnabled = true;
            DescripcionTxt.IsEnabled = true;
            RestriccionesTxt.IsEnabled = true;
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
                        DeterminarMetodoALlamar();
                    }
                }
        }

        private void DeterminarMetodoALlamar()
        {
            try
            {
                InstanceContext context = new InstanceContext(this);
                ModificarProductoClient servicioProduto = new ModificarProductoClient(context);
                string tipoProducto = tipoProductoCb.SelectedItem.ToString();

                if (tipoProducto == "Interno")
                {
                    IniciarProductoInternoAEnviar();
                    bool imagenEitada = true;
                    string nombreReceta = recetaCb.SelectedItem.ToString();
                    byte[] imagen = imagenProducto;

                    servicioProduto.ModificarProductoInterno(producto, imagenEitada, nombreReceta, imagen);
                } 
                else if(tipoProducto == "Externo")
                {
                    IniciarProductoExternoAEnviar();

                    servicioProduto.ModificarProductoExterno(this.productoExterno, this.imageneditada);
                }
            }
            catch (FormatException)
            {
                FuncionesComunes.MostrarMensajeDeError("Se encontraron caracteres invalidos, favor de corregir e intentar nuevamente");
            }
            catch (OverflowException)
            {
                FuncionesComunes.MostrarMensajeDeError("Alguno de los valores núméricos ingresados es demasiado grande");
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
                productoExterno.cantidadExistencias = int.Parse(existenciasTxt.Text);
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

        private bool CamposVacios()
        {
            if (nombreTxt.Text.Length > 0 && precioTxt.Text.Length > 0
                && CategoriaCb.SelectedIndex != 0 && DescripcionTxt.Text.Length > 0 && RestriccionesTxt.Text.Length > 0)
            {
                return false;
            }

            return true;
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
            if (nombreTxt.Text.Length > 0 && precioTxt.Text.Length > 0 && CategoriaCb.SelectedIndex != 0
                && (recetaCb.SelectedIndex == 0 || recetaCb.SelectedIndex != 0 || existenciasTxt.Text.Length > 0) 
                && DescripcionTxt.Text.Length > 0 && RestriccionesTxt.Text.Length > 0)
            {
                return true;
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
                imageneditada = true;
                string imagePath = exploradorArchivos.FileName;
                Uri FilePath = new Uri(imagePath);
                ProductoImg.Source = new BitmapImage(FilePath);
            }

            Stream bytesImagen = exploradorArchivos.OpenFile();
            using (MemoryStream ms = new MemoryStream())
            {
                bytesImagen.CopyTo(ms);
                imagenProducto = ms.ToArray();
            }
        }

        public void RespuestaModificarProducto(string mensajeError)
        {
            if (imageneditada)
            {   
                imageneditada = false;
            }

            if(mensajeError == "Cambios Guardados")
            {
                FuncionesComunes.MostrarMensajeExitoso(mensajeError);
                DeshabilitarCampos();
            }
            else
            {
                FuncionesComunes.MostrarMensajeDeError(mensajeError);
            }
        }

        private byte[] ObtenerByteArrayDeImagen()
        {
            using (var ms = new MemoryStream())
            {
                ImageConverter _imageConverter = new ImageConverter();
                byte[] xByte = (byte[])_imageConverter.ConvertTo(ProductoImg, typeof(byte[]));
                return xByte;
            }
        }
    }
}
