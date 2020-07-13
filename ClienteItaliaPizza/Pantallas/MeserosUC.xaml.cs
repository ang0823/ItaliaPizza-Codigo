using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using ClienteItaliaPizza.Servicio;

namespace ClienteItaliaPizza.Pantallas
{
    /// <summary>
    /// Lógica de interacción para MeserosUC.xaml
    /// </summary>
    public partial class MeserosUC : UserControl
    {
        public string tipoDeUsuario;
        public ObservableCollection<PedidoEnDataGrid> pedidosEnEspera = new ObservableCollection<PedidoEnDataGrid>();
        public ObservableCollection<PedidoEnDataGrid> pedidosPreparados = new ObservableCollection<PedidoEnDataGrid>();
        public ObservableCollection<PedidoEnDataGrid> pedidosEnviados = new ObservableCollection<PedidoEnDataGrid>();
        public ObservableCollection<PedidoEnDataGrid> pedidosEntregados = new ObservableCollection<PedidoEnDataGrid>();
        public ObservableCollection<PedidoEnDataGrid> pedidosCancelados = new ObservableCollection<PedidoEnDataGrid>();
        public EventHandler EventoAgregarNuevoPedidoALista;
        public EventHandler EventoAbrirVentanaLocal;
        public EventHandler EventoAbrirVentanaADomicilio;
        public EventHandler EventEditarPedido;
        public EventHandler EventQuitarPedido;

        public EventHandler EventCambiarEstado_Enviado;
        public EventHandler EventCambiarEstado_Entregado;

        public EventHandler EventTicketPDF;
        public EventHandler EventTicketImprimir;

        /// <summary>
        /// Constuctor de la vista general de Venta de Pedidos.
        /// </summary>
        /// <param name="tipoDeUsuario"> Rol del usuario para modificar las vistas si es "Mesero" o "Call Center". </param>
        public MeserosUC(string tipoDeUsuario)
        {
            InitializeComponent();
            comboBox3.Visibility = System.Windows.Visibility.Collapsed;
            
            this.tipoDeUsuario = tipoDeUsuario;            
            dataGridPedidosEnEspera.ItemsSource = pedidosEnEspera;
            dataGridPedidosEnviados.ItemsSource = pedidosPreparados;
            dataGridPedidosEnviados.ItemsSource = pedidosEnviados;
            dataGridPedidosEntregados.ItemsSource = pedidosEntregados;
            dataGridPedidosCancelados.ItemsSource = pedidosCancelados;

            if (tipoDeUsuario.Equals("Call Center"))
            {
                UCCallCenter.Visibility = Visibility.Visible;
                UCCallCenter.eventoAbrirPedidoADomicilio += UCCallCenter_AbrirPedidoDomicilio;
            }else
            {
                tabPedidosEnviados.Visibility = Visibility.Collapsed;
            }
        }


        // BOTONES Y ACCIONES DE BOTONES
        private void UCCallCenter_AbrirPedidoDomicilio(object sender, EventArgs e)
        {
            this.EventoAbrirVentanaADomicilio?.Invoke(this, e);
        }
        private void ButtonEditar_Click(object sender, RoutedEventArgs e)
        {
            if (buttonEditar.Content.Equals("Editar"))
                this.EventEditarPedido?.Invoke(this, e);
            else if (buttonEditar.Content.Equals("Enviado"))
                this.EventCambiarEstado_Enviado?.Invoke(this, e);
            else if (buttonEditar.Content.Equals("Entregado"))
                this.EventCambiarEstado_Entregado?.Invoke(this, e);
        }

        private void buttonQuitar_Click(object sender, RoutedEventArgs e)
        {
            this.EventQuitarPedido?.Invoke(this, e);
        }
        private void ButtonNuevoPedidoLocal_Click(object sender, RoutedEventArgs e)
        {
            EventoAbrirVentanaLocal?.Invoke(this, e);
        }

        private void ButtonPDF_Click(object sender, RoutedEventArgs e)
        {
            this.EventTicketPDF?.Invoke(this, e);
        }

        public void GenerarTicketPDF(string ruta, Pedido pedido)
        {
            Document doc = new Document();
            string pdfName = @ruta + "/Ticket" + pedido.Id + ".pdf";
            if (!Directory.Exists(@pdfName))
            {
                PdfWriter.GetInstance(doc, new FileStream(pdfName, FileMode.OpenOrCreate));

                doc.Open();
                Paragraph title = new Paragraph();
                title.Font = FontFactory.GetFont(FontFactory.TIMES, 18f, BaseColor.BLUE);
                title.Add("Ticket: " + pedido.Cuenta.Id);
                doc.Add(title);

                doc.Add(new Paragraph("Formato de Ticket - Italia Pizza"));
                doc.Add(new Paragraph("IdPedido: " + pedido.Cuenta.Id));
                doc.Add(new Paragraph("Subtotal: " + pedido.Cuenta.subTotal));
                doc.Add(new Paragraph("IVA: " + pedido.Cuenta.iva));
                doc.Add(new Paragraph("Descuento: " + pedido.Cuenta.descuento));
                doc.Add(new Paragraph("Total: " + pedido.Cuenta.precioTotal));
                doc.Close();
            }
            else
                PdfWriter.GetInstance(doc, new FileStream(pdfName, FileMode.Open));

            VisorPDF visorPDF = new VisorPDF(@ruta + "/Ticket" + pedido.Id + ".pdf");
            visorPDF.Show();  
        }

        
        // BOTONES Y ACCIONES DE BOTONES


        // COMPONENETES DE BÚSQUEDAS
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
        // COMPONENETES DE BÚSQUEDAS


        private void CargarPedidosEnEspera(object sender, RoutedEventArgs e)
        {//  EventoAgregarNuevoPedidoALista?.Invoke(this, e); 
        }


        // TABS DE ESTADOS DE PEDIDOS
        private void tabPedidosEnEspera_GotFocus(object sender, RoutedEventArgs e)
        {
            dataGridPedidosPreparados.UnselectAllCells(); 
            dataGridPedidosEnviados.UnselectAllCells();
            dataGridPedidosEntregados.UnselectAllCells();
            dataGridPedidosCancelados.UnselectAllCells();

            buttonQuitar.Visibility = Visibility.Collapsed;
            buttonEditar.Visibility = Visibility.Collapsed;
            OcultarComponentesDeTicket();

            buttonEditar.Content = "Editar";
        }

        private void tabPedidosPreparados_GotFocus(object sender, RoutedEventArgs e)
        {
            dataGridPedidosEnEspera.UnselectAllCells();
            dataGridPedidosEnviados.UnselectAllCells();
            dataGridPedidosEntregados.UnselectAllCells();
            dataGridPedidosCancelados.UnselectAllCells();

            buttonEditar.Visibility = Visibility.Collapsed;
            buttonQuitar.Visibility = Visibility.Collapsed;
            OcultarComponentesDeTicket();

            if (tipoDeUsuario == "Call Center")
                buttonEditar.Content = "Enviado";
            else
            {
                buttonEditar.Content = "Entregado";
            }
        }

        private void tabPedidosEnviados_GotFocus(object sender, RoutedEventArgs e)
        {
            dataGridPedidosEnEspera.UnselectAllCells();
            dataGridPedidosPreparados.UnselectAllCells();
            dataGridPedidosEntregados.UnselectAllCells();
            dataGridPedidosCancelados.UnselectAllCells();

            buttonEditar.Visibility = Visibility.Collapsed;
            buttonQuitar.Visibility = Visibility.Collapsed;
            OcultarComponentesDeTicket();

            buttonEditar.Content = "Entregado";
        }

        private void tabPedidosEntregados_GotFocus(object sender, RoutedEventArgs e)
        {
            dataGridPedidosPreparados.UnselectAllCells();
            dataGridPedidosEnEspera.UnselectAllCells();
            dataGridPedidosEnviados.UnselectAllCells();
            dataGridPedidosCancelados.UnselectAllCells();

            buttonEditar.Visibility = Visibility.Collapsed;
            buttonQuitar.Visibility = Visibility.Collapsed;
            OcultarComponentesDeTicket();
        }
        private void tabPedidosCancelados_GotFocus(object sender, RoutedEventArgs e)
        {
            dataGridPedidosPreparados.UnselectAllCells();
            dataGridPedidosEnEspera.UnselectAllCells();
            dataGridPedidosEnviados.UnselectAllCells();
            dataGridPedidosEntregados.UnselectAllCells();

            buttonEditar.Visibility = Visibility.Collapsed;
            buttonQuitar.Visibility = Visibility.Collapsed;
            OcultarComponentesDeTicket();
        }
        // TABS DE ESTADOS DE PEDIDOS


        // SELECCIÓN DE DATAGRID´S DE PEDIDOS
        private void dataGridPedidosEnEspera_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {           
                buttonEditar.Visibility = Visibility.Visible;
                buttonQuitar.Visibility = Visibility.Visible;      
        }

        private void dataGridPedidosPreparados_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var pedidoPreparado = dataGridPedidosPreparados.SelectedItem as PedidoEnDataGrid;
            if (dataGridPedidosPreparados.SelectedItem as PedidoEnDataGrid != null)
            {               
                if (pedidoPreparado.Tipo == "Local")
                {
                    buttonEditar.Content = "Entregado";
                    buttonQuitar.Visibility = Visibility.Visible;
                }
                else if(pedidoPreparado.Tipo == "Domicilio")
                {
                    buttonEditar.Content = "Enviado";
                }
            }
            buttonEditar.Visibility = Visibility.Visible; 
        }

        private void dataGridPedidosEntregados_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            borderGenerarTicket.Visibility = Visibility.Visible;
            buttonImprimir.Visibility = Visibility.Visible;
            buttonPDF.Visibility = Visibility.Visible;
        }

        private void OcultarComponentesDeTicket()
        {
            borderGenerarTicket.Visibility = Visibility.Hidden;
            buttonImprimir.Visibility = Visibility.Hidden;
            buttonPDF.Visibility = Visibility.Hidden;
        }
        // SELECCIÓN DE DATAGRID´S DE PEDIDOS



        // ACCESO AL DATAGRID EN ESPERA
        public PedidoEnDataGrid AgregarOSeleccionar_PedidoEnEspera
        {
            get { return dataGridPedidosEnEspera.SelectedItem as PedidoEnDataGrid; }
            set { pedidosEnEspera.Add(value); }
        }      

        public ObservableCollection<PedidoEnDataGrid> ListaEnEspera_DataGrid
        {
            get { return dataGridPedidosEnEspera.ItemsSource as ObservableCollection<PedidoEnDataGrid>;  }
            set { dataGridPedidosEnEspera.ItemsSource = value; }
        }

        // ACCESO AL DATAGRID PREPARADOS
        public PedidoEnDataGrid AgregarOSeleccionar_PedidoPreparado
        {
            get { return dataGridPedidosPreparados.SelectedItem as PedidoEnDataGrid; }
            set { pedidosPreparados.Add(value); }
        }

        public ObservableCollection<PedidoEnDataGrid> ListaPreparados_DataGrid
        {
            get { return dataGridPedidosPreparados.ItemsSource as ObservableCollection<PedidoEnDataGrid>; }
            set { dataGridPedidosPreparados.ItemsSource = value; }
        }

        //ACCESO AL DATAGRID ENVIADOS
        public PedidoEnDataGrid AgregarOSeleccionar_PedidoEnviado
        {
            get { return dataGridPedidosEnviados.SelectedItem as PedidoEnDataGrid; }
            set { pedidosEnviados.Add(value); }
        }

        public ObservableCollection<PedidoEnDataGrid> ListaEnviados_DataGrid
        {
            get { return dataGridPedidosEnviados.ItemsSource as ObservableCollection<PedidoEnDataGrid>; }
            set { dataGridPedidosEnviados.ItemsSource = value; }
        }

        //ACCESO AL DATAGRID ENTREGADOS
        public PedidoEnDataGrid AgregarOSeleccionar_PedidoEntregado
        {
            get { return dataGridPedidosEntregados.SelectedItem as PedidoEnDataGrid; }
            set { pedidosEntregados.Add(value); }
        }
        public ObservableCollection<PedidoEnDataGrid> ListaEntregados_DataGrid
        {
            get { return dataGridPedidosEntregados.ItemsSource as ObservableCollection<PedidoEnDataGrid>; }
            set { dataGridPedidosEntregados.ItemsSource = value; }
        }

        //ACCESO AL DATAGRID CANCELADOS
        public PedidoEnDataGrid AgregarOSeleccionar_PedidoCancelado
        {
            get { return dataGridPedidosCancelados.SelectedItem as PedidoEnDataGrid; }
            set { pedidosCancelados.Add(value); }
        }
        public ObservableCollection<PedidoEnDataGrid> ListaCancelados_DataGrid
        {
            get { return dataGridPedidosCancelados.ItemsSource as ObservableCollection<PedidoEnDataGrid>; }
            set { dataGridPedidosCancelados.ItemsSource = value; }
        }        
    }    
}
