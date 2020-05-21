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
using System.Windows.Shapes;

namespace ClienteItaliaPizza
{
    /// <summary>
    /// Lógica de interacción para PedidoADomicilio.xaml
    /// </summary>
    public partial class NuevoPedido : Window
    {
        public NuevoPedido()
        {
            InitializeComponent();
        }

        private void TextBoxInstruccionesEspeciales_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {

        }

        private void ButtonAceptar_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void TextBoxDescuento_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {

        }
    }
}
