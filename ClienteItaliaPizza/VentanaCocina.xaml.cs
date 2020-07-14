using System;
using ClienteItaliaPizza.Servicio;
using System.Collections.Generic;
using System.Windows;
using ClienteItaliaPizza.Pantallas;
using System.ServiceModel;
using System.Linq;
using System.Collections.ObjectModel;
using ClienteItaliaPizza.Validacion;

namespace ClienteItaliaPizza
{
    /// <summary>
    /// Lógica de interacción para VentanaCocina.xaml
    /// </summary>
    [ServiceBehavior (ConcurrencyMode = ConcurrencyMode.Multiple)]
    public partial class VentanaCocina : Window, INotificarPedidoCallback, IBuscarPedidosCallback
    {
        InstanceContext context;
       // IBuscarPedidos serverBusquedaPedidos;
       // INotificarPedido server;
        NotificarPedidoClient server;
        List<PedidoLocal> pedidosLocales = new List<PedidoLocal>();
        List<PedidoADomicilio> pedidosADomicilio = new List<PedidoADomicilio>();
        int ejeY = 40; // eje Y de nuestra ventana
        int conteo = 0; //contador para nuestros controles dinámicos            

        ObservableCollection<CocinaPedidoLocal> cocinaPedidoLocals = new ObservableCollection<CocinaPedidoLocal>();
        ObservableCollection<CocinaPedidoDomicilio> cocinaPedidoDomicilios = new ObservableCollection<CocinaPedidoDomicilio>();
        public VentanaCocina()
        {
            InitializeComponent();
            try
            {
                context = new InstanceContext(this);
               // var canal = new DuplexChannelFactory<INotificarPedido>(context, "*");
                server = new NotificarPedidoClient(context);
                //server = new NotificarPedidoClient(context);
               /* ((ICommunicationObject)server).Faulted += delegate { MessageBox.Show(" Te desconectaste : Faulted COCINA"); 
                    var canal2 = new DuplexChannelFactory<INotificarPedido>(context, "*");
                    server = canal.CreateChannel(); server.AgregarUsuario("Cocinero"); MessageBox.Show("Nuevamente conectado");
                };
                ((ICommunicationObject)server).Closed += delegate { MessageBox.Show(" Te desconectaste : Closed COCINA");
                    var canal2 = new DuplexChannelFactory<INotificarPedido>(context, "*");
                    server = canal.CreateChannel(); server.AgregarUsuario("Cocinero"); MessageBox.Show("Nuevamente conectado");
                };*/
                server.AgregarUsuario("Cocinero");

             //   var canalBusquedas = new DuplexChannelFactory<IBuscarPedidos>(context, "*");
              //  serverBusquedaPedidos = canalBusquedas.CreateChannel();
               // ((ICommunicationObject)serverBusquedaPedidos).Faulted += delegate { MessageBox.Show( " Te desconectaste : Faulted"); };
               // ((ICommunicationObject)serverBusquedaPedidos).Closed += delegate { MessageBox.Show( " Te desconectaste : Closed"); };*/
               // serverBusquedaPedidos.BuscarPedidosCallCenter();
            }
            catch(CommunicationException e)
            {
                FuncionesComunes.MostrarMensajeDeError(e.Message);

            }catch(TimeoutException e)
            {
                FuncionesComunes.MostrarMensajeDeError(e.Message);
            }
        }                                

        private void ButtonRegresarClick(object sender, RoutedEventArgs e)
        {
            MainWindow ventana = new MainWindow();
            ventana.Show();
            this.Close();
        }

        public void MostrarPedidoLocal(PedidoLocal pedido)
        {
            CocinaPedidoLocal vistaPedidoLocal = cocinaPedidoLocals.FirstOrDefault(p => p.EditarLabelIDPedido == pedido.Id.ToString());
            List<platillo> platillos = new List<platillo>();
            
            if (vistaPedidoLocal == null)
            {
                CocinaPedidoLocal cocinaPedidoLocal = new CocinaPedidoLocal();
                cocinaPedidoLocal.Name = "local_" + conteo.ToString();
                cocinaPedidoLocal.Margin = new Thickness(100, ejeY, 0, 0);
                cocinaPedidoLocal.Visibility = Visibility.Visible;
                cocinaPedidoLocal.eventoNotificarPedidoPreparado += EnviarPedidoLocalPreparado;

                cocinaPedidoLocal.EditarLabelIDPedido = pedido.Id.ToString();
                cocinaPedidoLocal.EditarLabelTipo = "Local";
                cocinaPedidoLocal.EditarLabelInstrucciones = pedido.instruccionesEspeciales;                

                foreach (var producto in pedido.Producto)
                {
                    platillos.Add(new platillo(producto.nombre, false, producto.cantidad));
                }

                cocinaPedidoLocal.llenarDataGrid = platillos;

                ejeY += 300;
                conteo++;

                grid.Children.Add(cocinaPedidoLocal);
                cocinaPedidoLocals.Add(cocinaPedidoLocal);
                pedidosLocales.Add(pedido);
            }
            else
            {
                vistaPedidoLocal.EditarLabelInstrucciones = pedido.instruccionesEspeciales;
                vistaPedidoLocal.llenarDataGrid = null;
                foreach (var producto in pedido.Producto)
                {
                    platillos.Add(new platillo(producto.nombre, false, producto.cantidad));
                }
                vistaPedidoLocal.llenarDataGrid = platillos;
                pedidosLocales.Remove(pedidosLocales.FirstOrDefault(p => p.Id == pedido.Id));
                pedidosLocales.Add(pedido);
            }
        }

        public void MostrarPedidoDomicilio(PedidoADomicilio pedido)
        {
            CocinaPedidoDomicilio vistaPedidoDomicilio = cocinaPedidoDomicilios.FirstOrDefault(p => p.EditarLabelIDPedido == pedido.Id.ToString());
            if(vistaPedidoDomicilio == null)
            {
                CocinaPedidoDomicilio cocinaPedidoDomicilio = new CocinaPedidoDomicilio();
                cocinaPedidoDomicilio.Name = "domicilio_" + conteo.ToString();
                cocinaPedidoDomicilio.Margin = new Thickness(100, ejeY, 0, 0);
                cocinaPedidoDomicilio.Visibility = Visibility.Visible;
                cocinaPedidoDomicilio.EventoPedidoDomicilioListo += EnviarPedidoDomicilioPreparado;

                cocinaPedidoDomicilio.EditarLabelIDPedido = pedido.Id.ToString();
                cocinaPedidoDomicilio.EditarLabelTipo = "Domicilio";
                cocinaPedidoDomicilio.EditarLabelInstrucciones = pedido.instruccionesEspeciales;

                List<platillo> platillos = new List<platillo>();
                cocinaPedidoDomicilio.llenarDatagridDomicilio = pedido.Producto;
                ejeY += 300;
                conteo++;

                grid.Children.Add(cocinaPedidoDomicilio);
                cocinaPedidoDomicilios.Add(cocinaPedidoDomicilio);
                pedidosADomicilio.Add(pedido);
            }
            else
            {
                vistaPedidoDomicilio.EditarLabelInstrucciones = pedido.instruccionesEspeciales;
                vistaPedidoDomicilio.llenarDatagridDomicilio = null;
                vistaPedidoDomicilio.llenarDatagridDomicilio = pedido.Producto;
                pedidosADomicilio.Remove(pedidosADomicilio.FirstOrDefault(p => p.Id == pedido.Id));
                pedidosADomicilio.Add(pedido);
            }            
        }

        private void RemoverPedidoLocalCancelado(PedidoLocal pedidoCancelado)
        {
            CocinaPedidoLocal vistaPedidoLocal = cocinaPedidoLocals.FirstOrDefault(p => p.EditarLabelIDPedido == pedidoCancelado.Id.ToString());
            if (vistaPedidoLocal != null)
            {
                cocinaPedidoLocals.Remove(cocinaPedidoLocals.First(p => p.EditarLabelIDPedido == pedidoCancelado.Id.ToString()));
                grid.Children.Remove(vistaPedidoLocal);
                pedidosLocales.Remove(pedidosLocales.FirstOrDefault(p => p.Id == pedidoCancelado.Id));
                ejeY -= 300;
            }               
        }

        private void RemoverPedidoDomicilioCancelado(PedidoADomicilio pedidoCancelado)
        {
            CocinaPedidoDomicilio vistaPedidoDomicilio = cocinaPedidoDomicilios.FirstOrDefault(p => p.EditarLabelIDPedido == pedidoCancelado.Id.ToString());
            if(vistaPedidoDomicilio != null)
            {
                cocinaPedidoDomicilios.Remove(cocinaPedidoDomicilios.First(p => p.EditarLabelIDPedido == pedidoCancelado.Id.ToString()));
                grid.Children.Remove(vistaPedidoDomicilio);
                pedidosADomicilio.Remove(pedidosADomicilio.FirstOrDefault(p => p.Id == pedidoCancelado.Id));
                ejeY -= 300;
            }
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
            if (pedidoEncontrado != null)
            {
                try
                {
                    server.NotificarPedidoLocalPreparado(pedidoEncontrado, "Cocinero");
                    grid.Children.Remove(local);
                    ejeY -= 300;
                }
                catch (CommunicationException ex)
                {
                    FuncionesComunes.MostrarMensajeDeError("No se ha podido establecer comunicación con el servidor\n" + ex.Message);
                }
            }
        }

        private void EnviarPedidoDomicilioPreparado(object sender, EventArgs e)
        {
            var domicilio = sender as CocinaPedidoDomicilio;
            var idPedido = domicilio.EditarLabelIDPedido;
            var pedidoEncontrado = pedidosADomicilio.Find(p => p.Id == FuncionesComunes.ParsearAEntero(idPedido));
            pedidoEncontrado.Estado = new Estado
            {
                estadoPedido = "Preparado"
            };
            if (pedidoEncontrado != null)
            {
                try
                {
                    server.NotificarPedidoADomicilioPreparado(pedidoEncontrado, "Cocinero");
                    grid.Children.Remove(domicilio);
                    ejeY -= 300;
                }
                catch (CommunicationException ex)
                {
                    FuncionesComunes.MostrarMensajeDeError("No se ha podido establecer comunicación con el servidor\n" + ex.Message);
                }
            }
        }

        // CALLBACKS NOTIFICAR PEDIDOS ******************************************************
        public void RecibirPedidoLocal(PedidoLocal pedido)
        {
            if (pedido.Estado.estadoPedido == "Cancelado")
            {
                FuncionesComunes.MostrarMensajeExitoso("Pedido Cancelado: " + pedido.Id);
                RemoverPedidoLocalCancelado(pedido);
            }               
            else
            MostrarPedidoLocal(pedido);                       
        }

        public void RecibirPedidoDomicilio(PedidoADomicilio pedido)
        {
            if(pedido.Estado.estadoPedido == "Cancelado")
            {
                FuncionesComunes.MostrarMensajeExitoso("Pedido Cancelado: " + pedido.Id);
                RemoverPedidoDomicilioCancelado(pedido);
            }else
            MostrarPedidoDomicilio(pedido);
        }

        public void MensajeNotificarPedido(string mensaje)
        {
            FuncionesComunes.MostrarMensajeDeError(mensaje);
        }
        // CALLBACKS NOTIFICAR PEDIDOS ******************************************************

        
        public void ObtenerTodosPedidos(PedidoADomicilioDeServidor[] pedidosADomicilio, PedidoLocalDeServidor[] pedidosLocales)
        {
            Dispatcher.Invoke(() =>
            {
                foreach (var pedidoDomicilio in pedidosADomicilio.Where(p => p.estado == "En Espera"))
                {
                    PedidoADomicilio pedidoDomicilioConvertido = ConvertidorDeObjetos.PedidoADomicilioDeServidor_A_PedidoADomicilio(pedidoDomicilio);
                    MostrarPedidoDomicilio(pedidoDomicilioConvertido);
                }

                foreach (var pedidoLocal in pedidosLocales.Where(p => p.estado == "En Espera"))
                {
                    PedidoLocal pedidoLocalConvertido = ConvertidorDeObjetos.PedidoLocalDeServidor_A_PedidoLocal(pedidoLocal);
                    MostrarPedidoLocal(pedidoLocalConvertido);
                }
            });            
        }

        public void ObtenerPedidosLocales(PedidoLocalDeServidor[] pedidosLocales)
        {
            throw new NotImplementedException();
        }

        public void MensajeErrorBuscarPedidos(string mensaje)
        {
            FuncionesComunes.MostrarMensajeDeError(mensaje);
        }
    }

    /**
     * estas son clases de prueba que simula los platillos para el DataGrid
     */
    public class platillo
    {
        public string nombreplatillo { get; set; }
        public bool preparado { get; set; }
        public int cantidad { get; set; }

        public platillo(string nombre, bool preparado, int cantidad)
        {
            this.nombreplatillo = nombre;
            this.preparado = preparado;
            this.cantidad = cantidad;
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

