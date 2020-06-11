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
        public string EditarComboBoxClienteNombre
        {
            get { return comboBoxClienteNombre.SelectedItem.ToString(); }
            set { comboBoxClienteNombre.Items.Add(value); }
        }

        public string EditarComboBoxDireccion
        {
            get { return comboBoxDireccion.SelectedItem.ToString(); }
            set { comboBoxDireccion.Items.Add(value); }
        }


        public string EditartextBoxTelefono
        {
            get { return textBoxTelefono.Text; }
            set { textBoxTelefono.Text = value; }
        }

        public event EventHandler eventLlenarComboBoxClienteNombre;
        public event EventHandler eventLlenarComboBoxDireccion;

        public event EventHandler eventSeleccionarCliente;
        public event EventHandler eventSeleccionarDirección;

        public event EventHandler eventEditarTelefono;





        public NuevoPedido_Domicilio()
        {
            InitializeComponent();
        }

        private void ComboBoxClienteNombre_Loaded(object sender, RoutedEventArgs e)
        {
            eventLlenarComboBoxClienteNombre?.Invoke(this, e);
        }

        private void ComboBoxClienteNombre_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            eventSeleccionarCliente?.Invoke(this, e);
        }

        private void ComboBoxDireccion_Loaded(object sender, RoutedEventArgs e)
        {
            eventLlenarComboBoxDireccion?.Invoke(this, e);
        }

        private void ComboBoxDireccion_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            eventSeleccionarDirección?.Invoke(this, e);
        }

        private void TextBoxTelefono_TextChanged(object sender, TextChangedEventArgs e)
        {
            eventEditarTelefono?.Invoke(this, e);
        }

        private void ButtonAgregarCliente_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ComboBoxClienteNombre_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (Validador.validarSoloLetrasConAcentos(e.Text) == false == false)
            {
                e.Handled = true;
            }
        }

        private void TextBoxTelefono_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {           
            if (Validador.validarSoloNumeros(e.Text) == false == false)
            {
                e.Handled = true;
            }
        }

        private void ComboBoxDireccion_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
           
        }
    }
}
