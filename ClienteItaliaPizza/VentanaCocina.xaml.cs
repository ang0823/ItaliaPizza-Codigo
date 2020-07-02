using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;
using ClienteItaliaPizza.Servicio;
using System.Collections.Generic;
using System.Windows;
using ClienteItaliaPizza.Pantallas;
using System.ServiceModel;

namespace ClienteItaliaPizza
{
    /// <summary>
    /// Lógica de interacción para VentanaCocina.xaml
    /// </summary>
    public partial class VentanaCocina : Window, INotificarPedidoCallback
    {
        InstanceContext context;
        NotificarPedidoClient server;
        List<PedidoLocal> pedidosLocales = new List<PedidoLocal>();
        int ejeY = 40; // eje Y de nuestra ventana
        int conteo = 0; //contador para nuestros controles dinámicos            

        public VentanaCocina()
        {
            InitializeComponent();
            try
            {
                context = new InstanceContext(this);
                server = new NotificarPedidoClient(context);
                server.AgregarUsuario("Cocinero");
            }
            catch(CommunicationException e)
            {
                FuncionesComunes.MostrarMensajeDeError(e.Message);
            }
        }
    

        private void ButtonPedidoDomicilioListo_Click(object sender, RoutedEventArgs e)
        {
            /// llamo al servicio para mandar el pedido a domicilio preparado

            /* estoy probando agregar controles dinámicos para cada que llegue un nuevo pedido en espera 
            StackPanel panel = new StackPanel();
            Label labelIDPedido = new Label();
            Label labelTipoPedido = new Label();
            DataGrid dataGrid = new DataGrid();

            //agrego propiedades           
            labelIDPedido.FontFamily = new FontFamily("Palatino Linotype");
            labelIDPedido.Foreground = Brushes.Black;
            labelIDPedido.Margin = new Thickness(450, 6,0,0);
            labelIDPedido.Name = "labelIDPedido" + conteo.ToString();
            labelIDPedido.FontSize = 14;
            labelIDPedido.Content = "P" + conteo.ToString();

            labelTipoPedido.FontFamily = new FontFamily("Palatino Linotype");
            labelTipoPedido.Foreground = Brushes.Black;
            labelTipoPedido.Margin = new Thickness(647, -30, 0, 0);
            labelTipoPedido.Name = "labelTipoPedido" + conteo.ToString();
            labelTipoPedido.FontSize = 14;
            labelTipoPedido.Content = "Tipo";

            panel.Height = 147;
            panel.Width = 915;
            panel.Opacity = 0.75;
            panel.Background = Brushes.Gray;
            panel.Margin = new Thickness(30, ejeY, 0, 0);
            panel.Name = "panel_" + conteo.ToString();
            
            dataGrid.Name = "dataGridPlatillos" + conteo.ToString();
            dataGrid.Margin = new Thickness(24, 42, 524, 9);
            dataGrid.Background = Brushes.FloralWhite;

            panel.Children.Add(labelIDPedido);
            panel.Children.Add(labelTipoPedido);
            panel.Children.Add(dataGrid);
            ejeY += 320;       
            conteo++;

          grid.Children.Add(panel); */
            // panelPrincipal.Children.Add(panel);

            /*CocinaPedidoLocal cocinaPedidoLocal = new CocinaPedidoLocal();
            cocinaPedidoLocal.Name = "local_" + conteo.ToString();
            cocinaPedidoLocal.Margin = new Thickness(50, ejeY, 0, 0);
            cocinaPedidoLocal.Visibility = Visibility.Visible;

            List<platillo> platillos = new List<platillo>();
            platillo pla = new platillo("dodo", true);
            platillo pla1 = new platillo("camote", false);
            platillos.Add(pla);
            platillos.Add(pla1);

            cocinaPedidoLocal.llenarDataGrid = platillos;

            ejeY += 300;
            conteo++;

            grid.Children.Add(cocinaPedidoLocal);*/
        }

        private void ButtonRegresarClick(object sender, RoutedEventArgs e)
        {
            MainWindow ventana = new MainWindow();
            ventana.Show();
            this.Close();
        }

        private void BorderPedido_Loaded(object sender, RoutedEventArgs e)
        {
            List<platillo> platillos = new List<platillo>();
            platillo pla = new platillo("dodo", true);
            platillo pla1 = new platillo("camote", false);
            platillos.Add(pla);
            platillos.Add(pla1);

           /* foreach (var platillo in platillos)
            {
                DataGridPlatillos.Items.Add(platillo);
            }
            DataGridPlatillos.ItemsSource = platillos;
            var n = DataGridPlatillos.ItemsSource;
            */
        }

        public void RecibirPedidoLocal(PedidoLocal pedido)
        {
            CocinaPedidoLocal cocinaPedidoLocal = new CocinaPedidoLocal();
            cocinaPedidoLocal.Name = "local_" + conteo.ToString();
            cocinaPedidoLocal.Margin = new Thickness(50, ejeY, 0, 0);
            cocinaPedidoLocal.Visibility = Visibility.Visible;
            cocinaPedidoLocal.eventoNotificarPedidoPreparado += EnviarPedidoLocalPreparado;

            cocinaPedidoLocal.EditarLabelIDPedido = pedido.Id.ToString();
            cocinaPedidoLocal.EditarLabelTipo = "Local";
            cocinaPedidoLocal.EditarLabelInstrucciones = pedido.instruccionesEspeciales;

            List<platillo> platillos = new List<platillo>();

            foreach (var producto in pedido.Producto)
            {
                platillos.Add(new platillo (producto.nombre, false));
            }
                       
            cocinaPedidoLocal.llenarDataGrid = platillos;

            ejeY += 300;
            conteo++;

            grid.Children.Add(cocinaPedidoLocal);
            pedidosLocales.Add(pedido);
        }

        private void EnviarPedidoLocalPreparado(object sender, EventArgs e)
        {
            var local = sender as CocinaPedidoLocal;
            var idPedido = local.EditarLabelIDPedido;
            var pedidoEncontrado = pedidosLocales.Find(p => p.Id == FuncionesComunes.ParsearAEntero(idPedido));
            pedidoEncontrado.Estado = new Estado
            {
                estadoPedido = "Preparado"
            };

            if (pedidoEncontrado!= null){
                server.NotificarPedidoLocalPreparado(pedidoEncontrado, "Cocinero");
            }            
        }

        public void RecibirPedidoDomicilio(PedidoADomicilio pedido)
        {
            CocinaPedidoDomicilio cocinaPedidoDomicilio = new CocinaPedidoDomicilio();
            cocinaPedidoDomicilio.Name = "domicilio_" + conteo.ToString();
            cocinaPedidoDomicilio.Margin = new Thickness(50, ejeY, 0, 0);
            cocinaPedidoDomicilio.Visibility = Visibility.Visible;

            cocinaPedidoDomicilio.EditarLabelIDPedido = pedido.Id.ToString();
            cocinaPedidoDomicilio.EditarLabelTipo = "Domicilio";
            cocinaPedidoDomicilio.EditarLabelInstrucciones = pedido.instruccionesEspeciales;

            List<platillo> platillos = new List<platillo>();

            cocinaPedidoDomicilio.llenarDatagridDomicilio = pedido.Producto;

            ejeY += 300;
            conteo++;

            grid.Children.Add(cocinaPedidoDomicilio);
        }

        public void MensajeNotificarPedido(string mensaje)
        {
            MessageBox.Show(mensaje);
        }
    }

    /**
     * estas son clases de prueba que simula los platillos para el DataGrid
     */
    public class platillo
    {
        public string nombreplatillo { get; set; }
        public bool preparado { get; set; }

        public platillo(string nombre, bool preparado)
        {
            this.nombreplatillo = nombre;
            this.preparado = preparado;
        }
        
    }   

    /**
     * metodo callback qe recibira para obtener el nuevo pedido a preparar
     * 1. creara un nuevvo border, etc para poner los datos. 
     * 2. en cda label pondra los respectivs cmpos
     * 3. list  platillos = pedido.platillos; 
     * 3.1  foreach (var platillo in platillos)
             {
                 DataGridPlatillos.Items.Add(platillo, false); 
              }
     */
}


