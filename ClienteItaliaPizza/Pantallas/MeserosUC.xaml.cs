using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;

namespace ClienteItaliaPizza.Pantallas
{
    /// <summary>
    /// Lógica de interacción para MeserosUC.xaml
    /// </summary>
    public partial class MeserosUC : UserControl
    {
        public string tipoDeUsuario;
        public MeserosUC(string tipoDeUsuario)
        {
            InitializeComponent();
            this.tipoDeUsuario = tipoDeUsuario;         
            comboBox3.Visibility = System.Windows.Visibility.Collapsed;

            if (tipoDeUsuario.Equals("CallCenter"))
            {
                UCCallCenter.Visibility = Visibility.Visible;
            }
        }

        private void ComboBox1_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            comboBox1.Items.Add("Pedidos");
            comboBox1.Items.Add("Cuentas");         
        }

        private void ComboBox3_CargarOpcionesLocales()
        {
            comboBox3.Items.Clear();
            comboBox3.Text = "No.Mesa";
            
            for (int i = 1; i < 6; i++)
                {
                    comboBox3.Items.Add(i);
                }      
        }

        private void ComboBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            comboBox3.Items.Clear();

            if (comboBox1.SelectedItem.Equals("Pedidos"))
            {
                ComboBox3_CargarOpcionesLocales();
                comboBox3.Visibility = System.Windows.Visibility.Collapsed;

                if (tipoDeUsuario.Equals("Mesero"))
                {                    
                    comboBox3.Visibility = System.Windows.Visibility.Visible;                   
                }
                else
                {
                   UCCallCenter.comboBox2.Visibility = Visibility.Visible; // lo visivilizo temporalmente para probar la vista del call center
                }
            }
            if(comboBox1.SelectedItem.Equals("Cuentas"))
            {
                comboBox3.Visibility = System.Windows.Visibility.Visible;
                comboBox3.Text = "Tipo de Cuenta";
                comboBox3.Items.Add("Abiertas");
                comboBox3.Items.Add("Cerrada");

                UCCallCenter.comboBox2.Visibility = Visibility.Collapsed;
                UCCallCenter.textBoxNombreCliente.Visibility = Visibility.Collapsed;
            }
        }

        private void ButtonEditar_Click(object sender, RoutedEventArgs e)
        {
           
        }

        /// <summary>
        /// Método que responde a la invocación del eventoComboBox2Seleccionado declarado en CallCenterUC 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UCCallCenter_eventoComboBox2Seleccionado(object sender, EventArgs e)
        {
            var seleccionComboBox2= UCCallCenter.RegresarSeleccionComboBox2;

            if (seleccionComboBox2.Equals("Locales"))
            {
                comboBox3.Visibility = Visibility.Visible;
                ComboBox3_CargarOpcionesLocales();
            }
            else
            {
                comboBox3.Visibility = Visibility.Collapsed;
            }
        }

        private void UCCallCenter_eventoEditarNombreClienteBusqueda(object sender, EventArgs e)
        {
            var nombreClienteABuscar = UCCallCenter.EditarNombreClienteBusqueda;
           // labelGenerarTicket.Content = nombreClienteABuscar;  //lo imprimo temporalmente en el label para comprobar su funcionamiento
        }

        private void ButtonNuevoPedidoLocal_Click(object sender, RoutedEventArgs e)
        {
            NuevoPedido ventanaNuevoPedido = new NuevoPedido();
            ventanaNuevoPedido.Show();                        
        }

        private void ButtonImprimir_Click(object sender, RoutedEventArgs e)
        {
           
        }

        private void ButtonPDF_Click(object sender, RoutedEventArgs e)
        {
            Document doc = new Document();
            PdfWriter.GetInstance(doc, new FileStream("hola.pdf", FileMode.Create));
            doc.Open();

            Paragraph title = new Paragraph();
            title.Font = FontFactory.GetFont(FontFactory.TIMES, 18f, BaseColor.BLUE);
            title.Add("Hola Mundo!!");
            doc.Add(title);

            doc.Add(new Paragraph("Hola Mundo!!"));
            doc.Add(new Paragraph("Parrafo 1"));
            doc.Add(new Paragraph("Parrafo 2"));
            doc.Close();
        }
    }
}
