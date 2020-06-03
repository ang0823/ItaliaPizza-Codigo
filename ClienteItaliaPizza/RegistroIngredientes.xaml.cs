using ClienteItaliaPizza.Servicio;
using System;
using System.ServiceModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MessageBox = System.Windows.MessageBox;

namespace ClienteItaliaPizza
{

    [CallbackBehavior(UseSynchronizationContext = false)]
    /// <summary>
    /// Lógica de interacción para RegistrarIngrediente.xaml
    /// </summary>
    public partial class RegistroIngredientes : Window, Servicio.IRegistrarIngredienteCallback
    {
        CuentaUsuario1 CuentaUsuario;
        Provision ingrediente = new Provision();

        public RegistroIngredientes(CuentaUsuario1 cuenta)
        {
           // this.CuentaUsuario = cuenta;
            InitializeComponent();
            IniciarComboBox();
            CuentaUsuario = cuenta;
            UserLbl.Content = cuenta.nombreUsuario;
            VaciarBtn.IsEnabled = false;
            GuardarBtn.IsEnabled = false;
        }

        private Boolean AlgunCampoLleno()
        {
            if (IngredienteNombre.Text.Length > 0 || IngredientePrecio.Text.Length > 0
                || IngredienteUbicacion.Text.Length > 0 || UnidadMedidaCb.SelectedIndex != 0
                || IngredienteExistencias.Text.Length > 0 || StockMinimo.Text.Length > 0)
            {
                return true;
            }

            return false;
        }

        private Boolean CamposLlenos()
        {
            if (IngredienteNombre.Text.Length > 0 && IngredientePrecio.Text.Length > 0
                && IngredienteUbicacion.Text.Length > 0 && UnidadMedidaCb.SelectedIndex != 0
                && IngredienteExistencias.Text.Length > 0 && StockMinimo.Text.Length > 0)
            {
                return true;
            }

            return false;
        }

        private void InicializarObjetoIngrediente()
        {
            try
            {
            float precio = ParsearPrecio(IngredientePrecio.Text.Trim());
            short noExistencias = ParsearAShort(IngredienteExistencias.Text.Trim());
            int minimoPermitido = ParsearAEntero(StockMinimo.Text.Trim());

            ingrediente.nombre = IngredienteNombre.Text.Trim();
            ingrediente.noExistencias = noExistencias;
            ingrediente.ubicacion = IngredienteUbicacion.Text.Trim();
            ingrediente.stockMinimo = int.Parse(StockMinimo.Text.Trim());
            ingrediente.costoUnitario = precio;
            ingrediente.unidadMedida = UnidadMedidaCb.SelectedItem.ToString();
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

        private void IniciarComboBox()
        {
            UnidadMedidaCb.Items.Insert(0, "Seleccionar");
            UnidadMedidaCb.Items.Insert(1, "Lt");
            UnidadMedidaCb.Items.Insert(2, "Kg");
            UnidadMedidaCb.Items.Insert(3, "Pza");
            UnidadMedidaCb.SelectedIndex = 0;
        }

        private void MostrarCuadroError(string mensaje)
        {
            string titulo = "Información";
            MessageBoxResult opcion;
            opcion = MessageBox.Show(mensaje, titulo,
                MessageBoxButton.OK, MessageBoxImage. Error);
        }

        private void MostrarMensajeExitoso(string Mensaje)
        {
            MessageBoxResult opcion;
            string titulo = "Operación exitósa";
            opcion = MessageBox.Show(Mensaje, titulo,
                MessageBoxButton.OK, MessageBoxImage.Information);
            VaciarCampos();
        }

        private void MostrarVentanaPrincipal()
        {
            Dispatcher.Invoke(() =>
            {
                Principal ventana = new Principal(CuentaUsuario);
                ventana.Show();
                this.Close();
            });
        }

        private int ParsearAEntero(string EntradaUsuario)
        {
            int entero;

            try
            {
                entero = int.Parse(EntradaUsuario);
            }
            catch (FormatException)
            {
                throw new FormatException("El stock mínimo debe ser numérico");
            }
            catch (OverflowException)
            {
                throw new OverflowException("El stock mínimo ingresado provoco un desbordamiento");
            }

            return entero;
        }

        private short ParsearAShort(string EntradaUsuario)
        {
            short shortNumber;

            try
            {
                shortNumber = short.Parse(EntradaUsuario);
            }
            catch (FormatException)
            {
                throw new FormatException("El número de existencias no pueden ser letras");
            }
            catch (OverflowException)
            {
                throw new OverflowException("El númweo de existencias ingresado provocó un desbordamiento");
            }

            return shortNumber;
        }
        
        private float ParsearPrecio(string precio) 
        {
            float costo = 0;

            try
            {
                costo = float.Parse(precio);
            } 
            catch (FormatException)
            {
                throw new FormatException("El precio del producto debe ser numérico (puede incluir punto decimal).");
            } catch (OverflowException)
            {
                throw new OverflowException("El precio ingresado provoco un desbordamiento");
            }

            return costo;
        }

        private void RegistrarIngrediente()
        {
            InstanceContext context = new InstanceContext(this);
            RegistrarIngredienteClient ServicioIngrediente = new RegistrarIngredienteClient(context);
            string Mensaje;

            try
            {
                if (CamposLlenos())
                {
                    InicializarObjetoIngrediente();
                    ServicioIngrediente.RegistrarIngrediente(ingrediente);
                    Mensaje = "El ingrediente se guardó exitosamente";
                    MostrarMensajeExitoso(Mensaje);
                    VaciarCampos();
                }
                else
                {
                    Mensaje = "Es necesario llenar todos los campos";
                    MostrarCuadroError(Mensaje);
                }
            }
            catch (CommunicationException)
            {
                Mensaje = "Ocurrió un error al comunicarse con el servidor";
                MostrarCuadroError(Mensaje);
            }
            catch (FormatException error)
            {
                MostrarCuadroError(error.Message);
            }
            catch (OverflowException error)
            {
                MostrarCuadroError(error.Message);
            }
            catch (InvalidOperationException)
            {

            }
        }

        private void VaciarCampos()
        {
            IngredienteNombre.Text = "";
            IngredientePrecio.Text = "";
            IngredienteUbicacion.Text = "";
            IngredienteExistencias.Text = "";
            StockMinimo.Text = "";
            UnidadMedidaCb.SelectedIndex = 0;
        }

        private void IngredienteNombre_TextChanged(object sender, TextChangedEventArgs e)
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

        private void IngredientePrecio_TextChanged(object sender, TextChangedEventArgs e)
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

        private void IngredienteUbicacion_TextChanged(object sender, TextChangedEventArgs e)
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

        private void UnidadMedidaCb_SelectionChanged(object sender, SelectionChangedEventArgs e)
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

        private void IngredienteExistencias_TextChanged(object sender, TextChangedEventArgs e)
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

        private void StockMinimo_SelectionChanged(object sender, SelectionChangedEventArgs e)
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

        private void StockMinimo_TextChanged(object sender, TextChangedEventArgs e)
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

        private void VaciarBtn_Click(object sender, RoutedEventArgs e)
        {
            VaciarCampos();
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
                    MostrarVentanaPrincipal();
                }
            }
            else
            {
                MostrarVentanaPrincipal();
            }
        }

        private void LogoutBtn_Click(object sender, RoutedEventArgs e)
        {
            FuncionesComunes.CerrarSesion();
            this.Close();
        }

        private void GuardarBtn_Click(object sender, RoutedEventArgs e)
        {
            RegistrarIngrediente();
        }

        private void IngredienteNombre_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (CamposLlenos() && e.Key == Key.Return)
            {
                RegistrarIngrediente();
            }
        }

        public void Respuesta(string mensajeError)
        {
            throw new NotImplementedException();
        }
    }
}
