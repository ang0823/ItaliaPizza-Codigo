using ClienteItaliaPizza.Servicio;
using System;
using System.ServiceModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ClienteItaliaPizza
{

    [CallbackBehavior(UseSynchronizationContext = false)]

    /// <summary>
    /// Interfaz gráfica del cliente para buscar y ediar un ingrediente guardado en el servidor.
    /// </summary>
    public partial class BuscarIngrediente : Window, IBuscarIngredienteCallback, IEditarIngredienteCallback 
    {
        CuentaUsuario1 CuentaUsuario;
        Provision ingrediente = new Provision();

        public BuscarIngrediente(CuentaUsuario1 cuenta)
        {
            this.CuentaUsuario = cuenta;
            InitializeComponent();
            DeshabilitarCampos();
            IniciarUnidadesMedida();
            UserLbl.Content = cuenta.nombreUsuario;
            EditarGuardarBtn.IsEnabled = false;
        }

        /// <summary>
        /// Envia al servidor la nueva información del ingtediente para que se edite en la base de datos.
        /// </summary>
        private void ActualizarDatosDeIngrediente()
        {
            try
            {
                if (InformacionEditada())
                {
                    InstanceContext context = new InstanceContext(this);
                    EditarIngredienteClient ServicioIngrediente = new EditarIngredienteClient(context);
                
                    float precio = FuncionesComunes.ParsearAFloat(IngredientePrecio.Text.Trim());
                    short noExistencias = FuncionesComunes.ParsearAShort(IngredienteExistencias.Text.Trim());
                    int minimoPermitido = FuncionesComunes.ParsearAEntero(StockMinimo.Text.Trim());

                    ingrediente.nombre = IngredienteNombre.Text.Trim();
                    ingrediente.noExistencias = noExistencias;
                    ingrediente.ubicacion = IngredienteUbicacion.Text.Trim();
                    ingrediente.stockMinimo = int.Parse(StockMinimo.Text.Trim());
                    ingrediente.costoUnitario = precio;
                    ingrediente.unidadMedida = UnidadMedidaCb.SelectedItem.ToString();
                    ServicioIngrediente.Editar(ingrediente);
                    DeshabilitarCampos();
                    EstablecerInformacion(ingrediente);
                }
                else
                {
                    EstablecerInformacion(ingrediente);
                    DeshabilitarCampos();
                }
            }
            catch (FormatException error)
            {
                throw new FormatException(error.Message);
            }
            catch (OverflowException error)
            {
                throw new OverflowException(error.Message);
            }
        }

        /// <summary>
        /// Se conecta con el servidor para recuperar la información del producto ingresado por el usuario, mostrando los datos en la interfaz gráfica.
        /// </summary>
        private void BuscarInformacionDeProducto()
        {
            InstanceContext context = new InstanceContext(this);
            BuscarIngredienteClient ServicioIngrediente = new BuscarIngredienteClient(context);
            string NombreIngrediente = SearchBox.Text;
            DeshabilitarCampos();
            UnidadMedidaCb.SelectedIndex = 0;

            try
            {
                ServicioIngrediente.BuscarIngredientePorNombre(NombreIngrediente);
            }
            catch (FormatException error)
            {
                FuncionesComunes.MostrarMensajeDeError(error.Message);
            }
            catch (OverflowException error)
            {
                FuncionesComunes.MostrarMensajeDeError(error.Message);
            }
            catch(Exception error)
            {
                FuncionesComunes.MostrarMensajeDeError(error.Message);
            }
        }

        /// <summary>
        /// Verfica que todos los campos del formulario tengan contenido.
        /// </summary>
        /// <returns>True si todos lo campos están llenos o false en caso contrario.</returns>
        private Boolean CamposLlenos()
        {
            if (IngredienteNombre.Text.Length > 0 && IngredientePrecio.Text.Length > 0
                && IngredienteUbicacion.Text.Length > 0 && IngredienteExistencias.Text.Length > 0
                && StockMinimo.Text.Length > 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Establece todos los campos del formulario a deshabilitados para evirar que el usuario ingrese o edite información de los mismos.
        /// </summary>
        private void DeshabilitarCampos()
        {
            IngredienteNombre.IsEnabled = false;
            IngredientePrecio.IsEnabled = false;
            IngredienteUbicacion.IsEnabled = false;
            IngredienteExistencias.IsEnabled = false;
            StockMinimo.IsEnabled = false;
            UnidadMedidaCb.IsEnabled = false;
        }

        private void EstablecerInformacion(Provision provision)
        {
            IngredienteNombre.Text = provision.nombre;
            IngredientePrecio.Text = provision.costoUnitario.ToString();
            IngredienteUbicacion.Text = provision.ubicacion;
            IngredienteExistencias.Text = provision.noExistencias.ToString();
            StockMinimo.Text = provision.stockMinimo.ToString();
            UnidadMedidaCb.SelectedItem = provision.unidadMedida;
        }

        /// <summary>
        /// Establece todos los campos del formulario a habilitados para permitir que el usuario edite información de los mismos.
        /// </summary>
        private void HabilitarCampos()
        {
            IngredienteNombre.IsEnabled = true;
            IngredientePrecio.IsEnabled = true;
            IngredienteUbicacion.IsEnabled = true;
            IngredienteExistencias.IsEnabled = true;
            StockMinimo.IsEnabled = true;
            UnidadMedidaCb.IsEnabled = true;
        }

        /// <summary>
        /// Valida que la información del producto encontrado se haya editado cuado estaban habilitados los campos.
        /// </summary>
        /// <returns>True si se encuetra álgún campo con información editada o false en caso contrario.</returns>
        private Boolean InformacionEditada()
        {
            Boolean InformacionEditada = false;
            string NuevoNombre = IngredienteNombre.Text;
            string NuevoPrecio = IngredientePrecio.Text;
            string NuevaUbicacion = IngredienteUbicacion.Text;
            string NuevasExistencias = IngredienteExistencias.Text;
            string NuevoStockMinimo = StockMinimo.Text;
            string NuevaMedida = UnidadMedidaCb.SelectedItem.ToString();

            if (NuevoNombre != ingrediente.nombre || NuevoPrecio != ingrediente.costoUnitario.ToString()
                || NuevaUbicacion != ingrediente.ubicacion || NuevasExistencias != ingrediente.noExistencias.ToString()
                || NuevoStockMinimo != ingrediente.stockMinimo.ToString() || NuevaMedida != ingrediente.unidadMedida)
            {
                InformacionEditada = true;
            }

            return InformacionEditada;
        }

        private void IniciarUnidadesMedida()
        {
            UnidadMedidaCb.Items.Insert(0, "");
            UnidadMedidaCb.Items.Insert(1, "Kg");
            UnidadMedidaCb.Items.Insert(2, "Lt");
            UnidadMedidaCb.Items.Insert(3, "Pza");
            UnidadMedidaCb.SelectedIndex = 0;
        }

        private void LogoutBtn_Click(object sender, RoutedEventArgs e)
        {
            FuncionesComunes.CerrarSesion();
            this.Close();
        }

        private void IngredienteNombre_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (CamposLlenos())
            {
                if (CamposLlenos())
                {
                    EditarGuardarBtn.IsEnabled = true;
                }
                else
                {
                    EditarGuardarBtn.IsEnabled = false;
                }
            }
        }

        private void IngredientePrecio_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (CamposLlenos())
            {
                EditarGuardarBtn.IsEnabled = true;
            }
            else
            {
                EditarGuardarBtn.IsEnabled = false;
            }
        }

        private void IngredienteUbicacion_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (CamposLlenos())
            {
                EditarGuardarBtn.IsEnabled = true;
            }
            else
            {
                EditarGuardarBtn.IsEnabled = false;
            }
        }

        private void UnidadMedidaCb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CamposLlenos())
            {
                EditarGuardarBtn.IsEnabled = true;
            }
            else
            {
                EditarGuardarBtn.IsEnabled = false;
            }
        }

        private void IngredienteExistencias_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (CamposLlenos())
            {
                EditarGuardarBtn.IsEnabled = true;
            }
            else
            {
                EditarGuardarBtn.IsEnabled = false;
            }
        }

        private void StockMinimo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CamposLlenos())
            {
                EditarGuardarBtn.IsEnabled = true;
            }
            else
            {
                EditarGuardarBtn.IsEnabled = false;
            }
        }

        private void StockMinimo_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (CamposLlenos())
            {
                EditarGuardarBtn.IsEnabled = true;
            }
            else
            {
                EditarGuardarBtn.IsEnabled = false;
            }
        }

        private void SearchBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (SearchBox.Text.Length > 0 && e.Key == Key.Return)
            {
                BuscarInformacionDeProducto();
            }
        }

        private void EditarGuardarBtn_Click(object sender, RoutedEventArgs e)
        {
            if(EditarGuardarBtn.Content.ToString() == "Editar")
            {
                EditarGuardarBtn.Content = "Guardar";
                HabilitarCampos();
            }
            else if(EditarGuardarBtn.Content.ToString() == "Guardar")
            {
                ActualizarDatosDeIngrediente();
                EditarGuardarBtn.Content = "Editar";
            }

        }

        private void SearchBtn_Click(object sender, RoutedEventArgs e)
        {
            BuscarInformacionDeProducto();
        }

        public void ProvicionDirecta(ProvisionDirecta provDir)
        {
            throw new NotImplementedException();
        }

        public void Ingrediente(Provision prov)
        {
            Dispatcher.Invoke(() =>
            {
                ingrediente = prov;
                EstablecerInformacion(ingrediente);
                EditarGuardarBtn.IsEnabled = true;
            });
        }

        public void ErrorAlRecuperarIngrediente(string mensajeError)
        {
            FuncionesComunes.MostrarMensajeDeError(mensajeError);
        }

        private void CancelarBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult opcion;

            if (CamposLlenos())
            {
                opcion = MessageBox.Show(
                    "¿Seguro que deseea volver a la pantalla principal?", 
                    "Descartar cambios",
                    MessageBoxButton.OKCancel, MessageBoxImage.Question
                    );

                if (opcion == MessageBoxResult.OK)
                {
                   FuncionesComunes.MostrarVentanaPrincipal(CuentaUsuario);
                   Close();
                }
            }
            else
            {
                FuncionesComunes.MostrarVentanaPrincipal(CuentaUsuario);
                Close();
            }
        }

        public void RespuestaEditarIngrediente(string mensajeError)
        {
            if (mensajeError == "Cambios Guardados")
            {
                FuncionesComunes.MostrarMensajeExitoso(mensajeError);
            }
            else
            {
                FuncionesComunes.MostrarMensajeDeError(mensajeError);
            }
        }
    }
}
