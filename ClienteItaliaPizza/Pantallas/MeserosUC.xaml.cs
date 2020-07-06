using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Collections.Generic;

namespace ClienteItaliaPizza.Pantallas
{
    /// <summary>
    /// Lógica de interacción para MeserosUC.xaml
    /// </summary>
    public partial class MeserosUC : UserControl
    {
        public string tipoDeUsuario;
        public ObservableCollection<PedidoEnDataGrid> pedidosEnEspera = new ObservableCollection<PedidoEnDataGrid>();
        public EventHandler eventoAgregarNuevoPedidoALista;
        public EventHandler eventoAbrirVentanaLocal;
        public EventHandler eventoAbrirVentanaADomicilio;
        public MeserosUC(string tipoDeUsuario)
        {
            InitializeComponent();
            this.tipoDeUsuario = tipoDeUsuario;
            comboBox3.Visibility = System.Windows.Visibility.Collapsed;
            dataGridPedidosEnEspera.ItemsSource = pedidosEnEspera;

            if (tipoDeUsuario.Equals("CallCenter"))
            {
                UCCallCenter.Visibility = Visibility.Visible;
                UCCallCenter.eventoAbrirPedidoADomicilio += UCCallCenter_AbrirPedidoDomicilio;
            }
        }

        private void UCCallCenter_AbrirPedidoDomicilio(object sender, EventArgs e)
        {
            this.eventoAbrirVentanaADomicilio?.Invoke(this, e);
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
            if (comboBox1.SelectedItem.Equals("Cuentas"))
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
            var seleccionComboBox2 = UCCallCenter.RegresarSeleccionComboBox2;

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
            eventoAbrirVentanaLocal?.Invoke(this, e);
        }

        private void ButtonImprimir_Click(object sender, RoutedEventArgs e)
        {
        }

        private void ButtonPDF_Click(object sender, RoutedEventArgs e)
        {
            string rutaTicketsPDF = "TicketsPDF";
            var rootDirectory = System.IO.Path.GetFullPath(AppDomain.CurrentDomain.BaseDirectory + "../../" + rutaTicketsPDF);
            if (Directory.Exists(@rootDirectory) == true)
            {
                GenerarTicketPDF(@rootDirectory);
            }
            else
            {
                DirectoryInfo di = Directory.CreateDirectory(@rootDirectory);
                GenerarTicketPDF(@rootDirectory);
            }
        }

        public void GenerarTicketPDF(string ruta)
        {
            int contadorTicketid = 0;
            contadorTicketid++;

            Document doc = new Document();
            PdfWriter.GetInstance(doc, new FileStream(@ruta + "/Ticket" + contadorTicketid + ".pdf", FileMode.OpenOrCreate));

            doc.Open();
            Paragraph title = new Paragraph();
            title.Font = FontFactory.GetFont(FontFactory.TIMES, 18f, BaseColor.BLUE);
            title.Add("Ticket" + contadorTicketid);
            doc.Add(title);

            doc.Add(new Paragraph("Formato de Ticket - Italia Pizza"));
            doc.Add(new Paragraph("IdPedido: " + contadorTicketid));
            doc.Add(new Paragraph("Costo total: "));
            doc.Close();

            VisorPDF visorPDF = new VisorPDF(@ruta + "/Ticket" + contadorTicketid + ".pdf");
            visorPDF.Show();
        }

        private void CargarPedidosEnEspera(object sender, RoutedEventArgs e)
        {
            eventoAgregarNuevoPedidoALista?.Invoke(this, e);
        }

        public PedidoEnDataGrid AgregarOSeleccionarNuevoPedido
        {
            get { return dataGridPedidosEnEspera.SelectedItem as PedidoEnDataGrid; }
            set { pedidosEnEspera.Add(value); }
        }

        public List<PedidoEnDataGrid> ListaEnEspera_DataGrid
        {
            get { return dataGridPedidosEnEspera.ItemsSource as List<PedidoEnDataGrid>;  }
            set { dataGridPedidosEnEspera.ItemsSource = value; }
        }
            
    }
}
