using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;
using ClienteItaliaPizza.Servicio;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Controls;
using ClienteItaliaPizza.Pantallas;

namespace ClienteItaliaPizza
{
    /// <summary>
    /// Lógica de interacción para VentanaCocina.xaml
    /// </summary>
    public partial class VentanaCocina : Window
    {
        int ejeY = 40; // eje Y de nuestra ventana
        int conteo = 0; //contador para nuestros controles dinámicos
      

        public Pedido pedido
        {
            get { return (Pedido)GetValue(MyPropertyProperty); }
            set { SetValue(MyPropertyProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MyPropertyProperty =
            DependencyProperty.Register("MyProperty", typeof(int), typeof(VentanaCocina), new PropertyMetadata(0));


        public VentanaCocina()
        {
            InitializeComponent();
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

            CocinaPedidoLocal cocinaPedidoLocal = new CocinaPedidoLocal();
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

            grid.Children.Add(cocinaPedidoLocal);
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
            }*/
            DataGridPlatillos.ItemsSource = platillos;
            var n = DataGridPlatillos.ItemsSource;
            
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
    public class Pedido
    {
        private int idPedido { get; set; }
        public string tipo { get; set; }
        public string instruccionesEspeciles { get; set;}
        public List<platillo> platillosEnEspera;
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


