using ClienteItaliaPizza.Servicio;
using System;
using System.Collections.Generic;
using System.IO;
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
    public partial class RegistroProductos : Window, IRegistrarProductoCallback, IObtenerRecetasCallback, IRegistrarIngredienteCallback, IModificarProductoCallback
    {
        CuentaUsuario1 CuentaUsuario;
        List<string> recetas = new List<string>();
        byte[] imagenProducto = null;

        public RegistroProductos(CuentaUsuario1 cuenta)
        {
            CuentaUsuario = cuenta;
            InitializeComponent();
            IniciarComboBoxes();

            UsuarioLbl.Content = cuenta.nombreUsuario;
            RecetaUbicacionLbl.Content = "Receta";
            tipoProductoCb.SelectedIndex = 0;
            EstadoCb.SelectedIndex = 1;
            CategoriaCb.SelectedIndex = 0;
            RecetaCb.SelectedIndex = 0;
            UnidadMedidaCb.SelectedIndex = 0;
            GuardarBtn.IsEnabled = false;
            VaciarBtn.IsEnabled = false;
            tipoProductoCb.IsEnabled = true;
            
            OcultarCamposProductoExterno();
            GenerarIdProducto();
            CargarRecetas();
        }

        private void GenerarIdProducto()
        {
            Random aleatorio = new Random();
            int PrimerPar = aleatorio.Next(10, 99);
            int SegundoPar = aleatorio.Next(10, 99);

            // CodigoTxt.Text = PrimerPar.ToString() + SegundoPar.ToString();
        }

        private void CargarRecetas()
        {
            try
            {
                InstanceContext context = new InstanceContext(this);
                ModificarProductoClient ServicioRecetas = new ModificarProductoClient(context);

                ServicioRecetas.ObtenerNombresDeRecetas();
            }
            catch (Exception exc)
            {
                FuncionesComunes.MostrarMensajeDeError(exc.Message);
            }
        }

        private void IniciarComboBoxes()
        {
            tipoProductoCb.Items.Add("Interno");
            tipoProductoCb.Items.Add("Externo");

            RecetaCb.Items.Insert(0, "Seleccionar");

            EstadoCb.Items.Insert(0, "Desactivado");
            EstadoCb.Items.Insert(1, "Activado");

            CategoriaCb.Items.Insert(0, "Seleccionar:");
            CategoriaCb.Items.Insert(1, "Ensaladas");
            CategoriaCb.Items.Insert(2, "Pizzas");
            CategoriaCb.Items.Insert(3, "Pastas");
            CategoriaCb.Items.Insert(4, "Postres");
            CategoriaCb.Items.Insert(5, "Bebidas");

            UnidadMedidaCb.Items.Insert(0, "Seleccionar:");
            UnidadMedidaCb.Items.Insert(1, "Kg");
            UnidadMedidaCb.Items.Insert(2, "Gr");
            UnidadMedidaCb.Items.Insert(3, "Oz");
            UnidadMedidaCb.Items.Insert(4, "Lt");
            UnidadMedidaCb.Items.Insert(5, "Pza");
        }

        private Boolean AlgunCampoLleno()
        {
            string tipoProducto = tipoProductoCb.SelectedItem.ToString();
            if(tipoProducto == "Interno")
            {
                if (NombreTxt.Text.Length > 0 || PrecioTxt.Text.Length > 0 
                    || CategoriaCb.SelectedIndex != 0 || RecetaCb.SelectedIndex != 0 
                || DescripcionTxt.Text.Length > 0 || RestriccionesTxt.Text.Length > 0)
                {
                    return true;
                }
            }
            else if(tipoProducto == "Externo")
            {
                if (NombreTxt.Text.Length > 0 || PrecioTxt.Text.Length > 0 || EstadoCb.SelectedIndex != 1
                || CategoriaCb.SelectedIndex != 0 || ExistenciasTxt.Text.Length > 0 || UbicacionTxt.Text.Length > 0 
                || stockMinTxt.Text.Length > 0 || UnidadMedidaCb.SelectedIndex != 0 || DescripcionTxt.Text.Length > 0
                || RestriccionesTxt.Text.Length > 0)
                {
                    return true;
                }
            }

            return false;
        }

        private Boolean CamposLlenos()
        {
            string tipoProducto = tipoProductoCb.SelectedItem.ToString();

            if (tipoProducto == "Interno")
            {
                if (NombreTxt.Text.Length > 0 && PrecioTxt.Text.Length > 0 && CategoriaCb.SelectedIndex != 0
                && DescripcionTxt.Text.Length > 0 && RestriccionesTxt.Text.Length > 0)
                {
                    return true;
                }
            }
            else if (tipoProducto == "Externo")
            {
                if (NombreTxt.Text.Length > 0 && PrecioTxt.Text.Length > 0 && CategoriaCb.SelectedIndex != 0
                && ExistenciasTxt.Text.Length > 0 && UbicacionTxt.Text.Length > 0 && stockMinTxt.Text.Length > 0
                && UnidadMedidaCb.SelectedIndex != 0 && DescripcionTxt.Text.Length > 0 && RestriccionesTxt.Text.Length > 0)
                {
                    return true;
                }
            }

            return false;
        }

        private void GuardarBtn_Click(object sender, RoutedEventArgs e)
        {
            if(imagenProducto != null)
            {
                RegistrarProducto();
            }
            else
            {
                FuncionesComunes.MostrarMensajeDeError("Falta seleccionar imagen para el producto.");
            }
        }

        private void RegistrarProducto()
        {
            string tipoProducto = tipoProductoCb.SelectedItem.ToString();

            if (tipoProducto == "Interno")
            {
                RegistrarProductoInterno();
            }
            else if (tipoProducto == "Externo")
            {
                RegistrarProductoExterno();
            }
        }

        private void RegistrarProductoInterno()
        {
            Producto producto = new Producto();
            Categoria categoria = new Categoria();
            string nombreReceta = RecetaCb.SelectedItem.ToString();

            try
            {
                InstanceContext context = new InstanceContext(this);
                RegistrarProductoClient ServicioRegistro = new RegistrarProductoClient(context);

                InicialzarProdcutoInterno(ref producto);
                InicializarCategoria(ref categoria);

                ServicioRegistro.RegistrarProducto(producto, categoria, nombreReceta, imagenProducto);
            }
            catch (EndpointNotFoundException)
            {
                FuncionesComunes.MostrarMensajeDeError("No se pudo establecer conexión con el servidor");
            }
            catch (TimeoutException)
            {
                FuncionesComunes.MostrarMensajeDeError("Se excedio el tiempo de espero y no hubo respuesta del servidor");
            }
            catch(Exception e)
            {
                FuncionesComunes.MostrarMensajeDeError(e.Message + " " + e.GetType());
            }
        }

        private void InicialzarProdcutoInterno(ref Producto producto)
        {
            try
            {
                producto.nombre = NombreTxt.Text.Trim();
                producto.precioUnitario = double.Parse(PrecioTxt.Text.Trim());
                producto.activado = ProductoActivado();
                producto.descripcion = DescripcionTxt.Text.Trim();
                producto.restricciones = RestriccionesTxt.Text.Trim();
            } 
            catch(ArgumentNullException)
            {
                FuncionesComunes.MostrarMensajeDeError("se necesita un precio para registrar");
            }
            catch(FormatException)
            {
                FuncionesComunes.MostrarMensajeDeError("El precio ingresado no es valido");
            } 
            catch (OverflowException)
            {
                FuncionesComunes.MostrarMensajeDeError("El precio ingresado es demasiado grande");
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

        private void InicializarCategoria(ref Categoria categoria)
        {
            categoria.Id = CategoriaCb.SelectedIndex;
            categoria.categoria = CategoriaCb.SelectedItem.ToString();
        }

        private void RegistrarProductoExterno()
        {
            InstanceContext context = new InstanceContext(this);
            RegistrarIngredienteClient servicioProvision = new RegistrarIngredienteClient(context);

            try
            {
                Provision provision = new Provision
                {
                    nombre = NombreTxt.Text,
                    costoUnitario = double.Parse(PrecioTxt.Text),
                    activado = ProductoActivado(),
                    noExistencias = int.Parse(ExistenciasTxt.Text),
                    ubicacion = UbicacionTxt.Text,
                    stockMinimo = int.Parse(stockMinTxt.Text),
                    unidadMedida = UnidadMedidaCb.SelectedItem.ToString(),
                };

                ProvisionDirecta provisionDirecta = new ProvisionDirecta
                {
                    descripcion = DescripcionTxt.Text,
                    activado = ProductoActivado(),
                    restricciones = RestriccionesTxt.Text,
                    Categoria = new Categoria
                    {
                        categoria = CategoriaCb.SelectedItem.ToString()
                    }
                };

                servicioProvision.RegistrarProvisionDirecta(provision, provisionDirecta, imagenProducto);
            }
            catch (FormatException)
            {
                FuncionesComunes.MostrarMensajeDeError("Los campos marcados con * solo permiten números.");
            }
            catch (OverflowException)
            {
                FuncionesComunes.MostrarMensajeDeError("Algunos de los campos marcados con * sobrepasa el valor 32767.");
            }
            catch (Exception e)
            {
                FuncionesComunes.MostrarMensajeDeError(e.GetType() + ": " + e.Message);
            }
        }

        private void VaciarCampos()
        {
            string tipoProducto = tipoProductoCb.SelectedItem.ToString();
            NombreTxt.Text = string.Empty;
            PrecioTxt.Text = string.Empty;
            EstadoCb.SelectedIndex = 1;
            if(tipoProducto == "Interno")
            {
                CategoriaCb.SelectedIndex = 0;
            }
            RecetaCb.SelectedIndex = 0;
            ExistenciasTxt.Text = string.Empty;
            UbicacionTxt.Text = string.Empty;
            stockMinTxt.Text = string.Empty;
            UnidadMedidaCb.SelectedIndex = 0;
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
            exploradorArchivos.Filter = "*.jpg | *.jpg";
            exploradorArchivos.Title = "Imagen del producto";
            exploradorArchivos.RestoreDirectory = true;

            DialogResult rutaImagen = exploradorArchivos.ShowDialog();

            if (rutaImagen == System.Windows.Forms.DialogResult.OK)
            {
                string imagePath = exploradorArchivos.FileName;
                Uri FilePath = new Uri(imagePath);
                ProductoImg.Source = new BitmapImage(FilePath);
            }
            try
            {
                Stream bytesImagen = exploradorArchivos.OpenFile();
                using (MemoryStream ms = new MemoryStream())
                {
                    bytesImagen.CopyTo(ms);
                    imagenProducto = ms.ToArray();
                }
            }
            catch(Exception error)
            {
                Console.WriteLine(error.GetType() + " | | " +error.Message);
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
                imagenProducto = null;
            }
            else
            {
                FuncionesComunes.MostrarMensajeDeError(mensaje);
            }
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
                
            });
        }

        private void tipoProductoCb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (tipoProductoCb.SelectedIndex == 0)
            {
                OcultarCamposProductoExterno();
                RecetaUbicacionLbl.Content = "Receta:";
                RecetaCb.Visibility = Visibility.Visible;
            }
            else
            {
                RecetaCb.Visibility = Visibility.Hidden;
                RecetaUbicacionLbl.Content = "Ubicación:";
                MostrarCamposProductoExterno();
            }
        }

        private void OcultarCamposProductoExterno()
        {
            CategoriaCb.IsEnabled = true;
            CategoriaCb.SelectedIndex = 0; 
            UbicacionTxt.Visibility = Visibility.Hidden;
            ExistenciasLbl.Visibility = Visibility.Hidden;
            ExistenciasTxt.Visibility = Visibility.Hidden;
            StockMinLbl.Visibility = Visibility.Hidden;
            stockMinTxt.Visibility = Visibility.Hidden;
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
            stockMinTxt.Visibility = Visibility.Visible;
            UnidadMedidaLbl.Visibility = Visibility.Visible;
            UnidadMedidaCb.Visibility = Visibility.Visible;
        }

        public void Respuesta(string mensajeError)
        {
            if(mensajeError == "Registro exitoso")
            {
                FuncionesComunes.MostrarMensajeExitoso(mensajeError);
                VaciarCampos();
                imagenProducto = null;
            }
            else
            {
                FuncionesComunes.MostrarMensajeDeError(mensajeError);
            }
        }

        public void ListaDeRecetas(string[] nombreDeRecetas)
        {
            foreach (var receta in nombreDeRecetas)
            {
                recetas.Add(receta);
                RecetaCb.Items.Add(receta);
            }
        }

        public void RespuestaModificarProducto(string mensajeError)
        {
            throw new NotImplementedException();
        }
    }
}
