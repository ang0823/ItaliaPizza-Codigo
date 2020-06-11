using ClienteItaliaPizza.Servicio;
using System;
using System.ServiceModel;
using System.Windows;
using System.Windows.Input;

namespace ClienteItaliaPizza
{
    /// <summary>
    /// Lógica de interacción para BuscarEmpleado.xaml
    /// </summary>
    public partial class BuscarProductos : Window, IBuscarProductoCallback
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
                    ServicioBuscar.BuscarPorID(idProducto);
                }
                catch (FormatException)
                {
                    string nombreProducto = SearchBox.Text;
                    ServicioBuscar.BuscarPorNombre(nombreProducto);
                }
                catch (Exception any)
                {
                    FuncionesComunes.MostrarMensajeDeError(any.Message + " " + any.GetType());
                }
            }
        }

        private void BuscarProducto(object sender, RoutedEventArgs e)
        {
            InstanceContext context = new InstanceContext(this);
            BuscarProductoClient ServicioBuscar = new BuscarProductoClient(context);

            try
            {
                int idProducto = int.Parse(SearchBox.Text);
                ServicioBuscar.BuscarPorID(idProducto);
            }
            catch (FormatException)
            {
                string nombreProducto = SearchBox.Text;
                ServicioBuscar.BuscarPorNombre(nombreProducto);
            }
            catch (Exception any)
            {
                FuncionesComunes.MostrarMensajeDeError(any.Message + " " + any.GetType());
            }
        }

        private Boolean CamposVacios()
        {
            if (nombreTxt.Text.Length > 0 && precioTxt.Text.Length > 0
                && categoriaCb.SelectedIndex != 0 && DescripcionTxt.Text.Length > 0 && RestriccionesTxt.Text.Length > 0)
            {
                return false;
            }

            return true;
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
            }

            return categoriaIndex;
        }

        private void DeshabilitarCampos()
        {
            ImagenBtn.Visibility = Visibility.Hidden;
            nombreTxt.IsEnabled = false;
            precioTxt.IsEnabled = false;
            estadoCb.IsEnabled = false;
            categoriaCb.IsEnabled = false;
            recetaCb.IsEditable = false;
            DescripcionTxt.IsEnabled = false;
            RestriccionesTxt.IsEnabled = false;
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

        private int EstaActivado(Producto producto)
        {
            int EstaActivo = 0;

            if (producto.activado)
            {
                EstaActivo = 1;
            }

            return EstaActivo;
        }

        private void HabilitarCampos()
        {
            ImagenBtn.Visibility = Visibility.Visible;
            nombreTxt.IsEnabled = true;
            precioTxt.IsEnabled = true;
            estadoCb.IsEnabled = true;
            categoriaCb.IsEnabled = true;
            recetaCb.IsEditable = true;
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
                HabilitarCampos();
            }
            else if (EditSaveBtn.Content.ToString() == "Guardar")
            {
                EditarInfoProducto();
                EditSaveBtn.Content = "Editar";
            }
        }

        private void LogoutBtn_Click(object sender, RoutedEventArgs e)
        {
            FuncionesComunes.CerrarSesion();
        }

        public void ProvicionDirecta(ProvisionDirecta provDir)
        {
            throw new NotImplementedException();
        }

        public void Provision(Producto prov)
        {
            codigoTxt.Text = prov.Id.ToString();
            nombreTxt.Text = prov.nombre;
            precioTxt.Text = prov.precioUnitario.ToString();
            estadoCb.SelectedIndex = EstaActivado(prov);
            categoriaCb.SelectedIndex = CategoriaProducto(prov);
            recetaCb.SelectedItem = prov.Receta;
            DescripcionTxt.Text = prov.descripcion;
        }

        public void ErrorAlRecuperarProducto(string mensajeError)
        {
            throw new NotImplementedException();
        }

        public void Provision(Provision prov)
        {
            throw new NotImplementedException();
        }
    }
}
