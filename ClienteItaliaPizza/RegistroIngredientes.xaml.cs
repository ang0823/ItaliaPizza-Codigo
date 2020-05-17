using ClienteItaliaPizza.Servicio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ClienteItaliaPizza
{
    /// <summary>
    /// Lógica de interacción para RegistrarIngrediente.xaml
    /// </summary>
    public partial class RegistroIngredientes : Window, Servicio.IRegistrarIngrediente
    {
        CuentaUsuario CuentaUsuario;
        
        /*public RegistroIngredientes(CuentaUsuario cuenta)
        {
            this.CuentaUsuario = cuenta;
            InitializeComponent();
            IniciarComboBox();

            VaciarBtn.IsEnabled = false;
            GuardarBtn.IsEnabled = false;
        }*/

        public RegistroIngredientes()
        {
            InitializeComponent();
            IniciarComboBox();

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

        private void IniciarComboBox()
        {
            UnidadMedidaCb.Items.Insert(0, "Seleccionar");
            UnidadMedidaCb.Items.Insert(1, "Lt");
            UnidadMedidaCb.Items.Insert(2, "Kg");
            UnidadMedidaCb.SelectedIndex = 0;
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

        private void IngredienteExistencias_TextChanged_1(object sender, TextChangedEventArgs e)
        {
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

        private void IngredienteExistencias_TextChanged_2(object sender, TextChangedEventArgs e)
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

        private void LogoutBtn_Click(object sender, RoutedEventArgs e)
        {
            FuncionesComunes.CerrarSesion();
            this.Close();
        }

        private void GuardarBtn_Click(object sender, RoutedEventArgs e)
        {
            InstanceContext context = new InstanceContext(this);
            RegistrarIngredienteClient ServicioIngrediente = new RegistrarIngredienteClient(context);

            try
            {
                Provision ingrediente = new Provision();
                ingrediente.nombre = IngredienteNombre.Text;
                short noExistencias = short.Parse(IngredienteExistencias.Text);
                ingrediente.noExistencias = noExistencias;
                ingrediente.ubicacion = IngredienteUbicacion.Text;
                ingrediente.stockMinimo = StockMinimo.Text;
                float precio = float.Parse(IngredientePrecio.Text);
                ingrediente.costoUnitario = precio;
                ingrediente.unidadMedida = UnidadMedidaCb.SelectedItem.ToString();
                Console.Write($"1. { ingrediente.nombre} 2. {ingrediente.noExistencias} 3. {ingrediente.ubicacion} 4. {ingrediente.stockMinimo} 5. {ingrediente.costoUnitario} 6. {ingrediente.unidadMedida}");
                ServicioIngrediente.RegistrarIngrediente(ingrediente);
                VaciarCampos();
            }
            catch (CommunicationException)
            {
                
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
    }
}
