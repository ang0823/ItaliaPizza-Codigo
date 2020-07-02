using ClienteItaliaPizza.Validacion;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Lógica de interacción para CallCenterUC.xaml
    /// </summary>
    public partial class CallCenterUC : UserControl
    {
        /// <summary>
        /// /Método que obtiene la opción que selecciona el usuario
        /// </summary>
        public string RegresarSeleccionComboBox2
        {
            get { return comboBox2.SelectedItem.ToString(); }            
        }

        /// <summary>
        /// /Método que obtiene el texto que ingresa el usuario
        /// </summary>
        public string EditarNombreClienteBusqueda
        {
            get { return textBoxNombreCliente.Text; }
            set { textBoxNombreCliente.Text = value; }
        }

        /// <summary>
        /// Evento que podrá invocarse desde otro UserControl para acceder la opcion seleccionada del comboBox2
        /// </summary>
        public event EventHandler eventoComboBox2Seleccionado;

        /// <summary>
        /// Evento que podrá invocarse desde otro UserControl para aceder al texto del textBox
        /// </summary>
        public event EventHandler eventoEditarNombreClienteBusqueda;

        public event EventHandler eventoAbrirPedidoADomicilio;
        public CallCenterUC()
        {
            InitializeComponent();
            textBoxNombreCliente.Visibility = Visibility.Collapsed;
            comboBox2.Visibility = Visibility.Collapsed;
            comboBox2.Items.Add("Locales");
            comboBox2.Items.Add("Domicilio");
        }      

        /// <summary>
        /// Método que cuando se seleccione una opción del comboBox invocará al evento manejador implementado en otro UserControl
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComboBox2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            textBoxNombreCliente.Visibility = Visibility.Visible;
            eventoComboBox2Seleccionado?.Invoke(this,e);
        }

        /// <summary>
        /// Método que cuando se escriba en el textBox invocará al evento manejador que estará implementado en otro userControl
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBoxNombreCliente_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (textBoxNombreCliente.Text != null)
            {
                eventoEditarNombreClienteBusqueda?.Invoke(this, e);
            }
        }

        private void TextBoxNombreCliente_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (Validador.validarSoloLetrasConAcentos(e.Text) == false)
            {
                e.Handled = true;
            }
        }
        private void ButtonNuevoPedidoDomicilio_Click(object sender, RoutedEventArgs e)
        {
            this.eventoAbrirPedidoADomicilio?.Invoke(this, e);          
        }
    }
}
