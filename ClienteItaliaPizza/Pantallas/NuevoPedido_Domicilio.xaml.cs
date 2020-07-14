using ClienteItaliaPizza.Servicio;
using ClienteItaliaPizza.Validacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ClienteItaliaPizza.Pantallas
{
    /// <summary>
    /// Lógica de interacción para NuevoPedido_Domicilio.xaml
    /// </summary>
    public partial class NuevoPedido_Domicilio : UserControl
    {
        public List<Cliente1> clientes = new List<Cliente1>();

        public bool agregarCliente = false;
        
        public string EditarComboBoxClienteNombre
        {
            get { return comboBoxClienteNombre.SelectedItem.ToString(); }
            set { comboBoxClienteNombre.Items.Add(value); }
        }
        public string ObtenerTexto_ComboBoxClienteNombre
        {
            get { return comboBoxClienteNombre.Text; }
        }


        public string EditarComboBoxDireccion
        {
            get { return comboBoxDireccion.SelectedItem.ToString(); }
            set { comboBoxDireccion.Items.Add(value); }
        }
        public string ObtenerTexto_ComboBoxDirección
        {
            get { return comboBoxDireccion.Text; }
        }

        public string EditarComboBoxTelefono
        {
            get { return comboBoxTelefono.SelectedItem.ToString(); }
            set { comboBoxTelefono.Items.Add(value); }
        }
        public string ObtenerTexto_ComboBoxTelefono
        {
            get { return comboBoxTelefono.Text; }
        }

        public event EventHandler eventLlenarComboBoxClienteNombre;
        public event EventHandler eventLlenarComboBoxDireccion;

        public event EventHandler eventSeleccionarCliente;
        public event EventHandler eventSeleccionarDirección;
        public event EventHandler eventEditarTelefono;

        public event EventHandler eventAgregarNuevoCliente;

        public event EventHandler eventEditarTextBoxDireccion;


        public NuevoPedido_Domicilio()
        {
            InitializeComponent();
            comboBoxClienteNombre.IsEditable = true; comboBoxClienteNombre.IsReadOnly = true;
            comboBoxTelefono.IsEditable = false;
            comboBoxDireccion.IsEditable = false;
        }

        private void ComboBoxClienteNombre_Loaded(object sender, RoutedEventArgs e)
        {
            eventLlenarComboBoxClienteNombre?.Invoke(this, e);
        }

        private void ComboBoxClienteNombre_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            eventSeleccionarCliente?.Invoke(this, e);

                       
            if (comboBoxClienteNombre.SelectedItem != null)
            {
                comboBoxDireccion.Items.Clear();
                comboBoxTelefono.Items.Clear();
                comboBoxClienteNombre.IsReadOnly = false;
                comboBoxTelefono.IsEditable = true;
                comboBoxDireccion.IsEditable = true;

                var primerNombre = comboBoxClienteNombre.SelectedItem.ToString().Split(' ');

                var clienteEnLista = clientes.Find(cl => cl.nombre.Contains(primerNombre[0]));
                
                if (clienteEnLista.direcciones.Length != 0 || clienteEnLista.telefonos.Length != 0)
                {
                    foreach (var direccion in clienteEnLista.direcciones)
                    {
                        comboBoxDireccion.Items.Add(direccion.calle + ", " + direccion.colonia + ", " + "# "+
                             direccion.codigoPostal + ", " + "Ex." + direccion.numeroInterior + " " + "Int." + direccion.numeroExterior);
                        comboBoxDireccion.Text = (string)comboBoxDireccion.Items[0];
                    }
                    foreach (var telefono in clienteEnLista.telefonos)
                    {
                        comboBoxTelefono.Items.Add(telefono.telefono); comboBoxTelefono.Text = (string)comboBoxTelefono.Items[0];
                    }
                }
            }
        }


        private void ComboBoxDireccion_Loaded(object sender, RoutedEventArgs e)
        {
            eventLlenarComboBoxDireccion?.Invoke(this, e);
        }

        private void ComboBoxDireccion_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            eventSeleccionarDirección?.Invoke(this, e);
        }


        private void comboBoxTelefono_Loaded(object sender, RoutedEventArgs e)
        {
            eventEditarTelefono?.Invoke(this, e);
        }

        private void comboBoxTelefono_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            eventEditarTelefono?.Invoke(this, e);
        }



        private void ComboBoxClienteNombre_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (Validador.validarSoloLetrasConAcentos(e.Text) == false)
            {
                e.Handled = true;
            }
        }


        private void ComboBoxDireccion_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
           
        }       

        private void comboBoxTelefono_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (Validador.validarSoloNumeros(e.Text) == false)
            {
                e.Handled = true;
            }
        }

        private void ButtonAgregarCliente_Click(object sender, RoutedEventArgs e)
        {
            if (agregarCliente == false)
            {
                agregarCliente = true;
                comboBoxClienteNombre.Visibility = Visibility.Collapsed;
                comboBoxTelefono.IsEditable = true; comboBoxTelefono.Items.Clear(); comboBoxTelefono.Text = "";
                comboBoxDireccion.Visibility = Visibility.Collapsed;

                textBoxNombre.Visibility = Visibility.Visible;
                textBoxApellidoPaterno.Visibility = Visibility.Visible;
                textBoxApellidoMaterno.Visibility = Visibility.Visible;
                textBoxCalle.Visibility = Visibility.Visible;
                textBoxColonia.Visibility = Visibility.Visible;
                textBoxCodigoPostal.Visibility = Visibility.Visible;
                textBoxNoInterior.Visibility = Visibility.Visible;
                textBoxNoExterior.Visibility = Visibility.Visible;
                ButtonCancelAgregarCliente.Visibility = Visibility.Visible;
                imageAgregarCliente.Source = new BitmapImage(new Uri(System.IO.Path.GetFullPath(AppDomain.CurrentDomain.BaseDirectory + "../../RecursosGUI/check.png")));
               
            }
            else
            {
                ButtonCancelAgregarCliente.IsEnabled = false;
                ButtonAgregarCliente.IsEnabled = false;
                eventAgregarNuevoCliente?.Invoke(this, e);
                ButtonCancelAgregarCliente.IsEnabled = true;
                ButtonAgregarCliente.IsEnabled = true;
            }
        }

        private void TextChanged_Direccion(object sender, TextChangedEventArgs e)
        {
            var textBox =sender as TextBox;
            if (textBox.Text != null)
            {
                eventEditarTextBoxDireccion?.Invoke(this, e);
            }
        }

        private void ButtonCancelAgregarCliente_Click(object sender, RoutedEventArgs e)
        {
            MostrarSoloComboBox();
        }

        public void MostrarSoloComboBox()
        {
            agregarCliente = false;
            textBoxNombre.Visibility = Visibility.Collapsed;
            textBoxApellidoPaterno.Visibility = Visibility.Collapsed;
            textBoxApellidoMaterno.Visibility = Visibility.Collapsed;
            textBoxCalle.Visibility = Visibility.Collapsed;
            textBoxColonia.Visibility = Visibility.Collapsed;
            textBoxCodigoPostal.Visibility = Visibility.Collapsed;
            textBoxNoInterior.Visibility = Visibility.Collapsed;
            textBoxNoExterior.Visibility = Visibility.Collapsed;

            comboBoxClienteNombre.Visibility = Visibility.Visible; comboBoxClienteNombre.Text = "Seleccione un cliente"; comboBoxClienteNombre.SelectedIndex = -1; comboBoxClienteNombre.IsReadOnly = true;
            comboBoxTelefono.IsEditable = false; comboBoxTelefono.Items.Clear(); comboBoxTelefono.Text = "";
            comboBoxDireccion.IsEditable = false; comboBoxDireccion.Items.Clear(); comboBoxDireccion.Text = ""; comboBoxDireccion.Visibility = Visibility.Visible;
                      
            imageAgregarCliente.Source = new BitmapImage(new Uri(System.IO.Path.GetFullPath(AppDomain.CurrentDomain.BaseDirectory + "../../RecursosGUI/Plus_Icon.png")));

            ButtonCancelAgregarCliente.Visibility = Visibility.Collapsed;
        }
    }
}