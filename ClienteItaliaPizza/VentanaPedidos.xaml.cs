using ClienteItaliaPizza.Servicio;
using System;
using System.ServiceModel;
using System.Windows;

namespace ClienteItaliaPizza.Pantallas
{
    /// <summary>
    /// Lógica de interacción para VentanaPedidos.xaml
    /// </summary>
    public partial class VentanaPedidos : Window, INotificarPedidoCallback
    {
        NotificarPedidoClient server;
        MeserosUC meserosUC;
        public static long idEmpleadoCallCenter; public static string idEmpleadoGeneradoCallCenter;

        /// <summary>
        /// Constructor Específico para el Mesero
        /// </summary>
        /// <param name="tipoUsuario"></param>
        public VentanaPedidos()
        {
            InitializeComponent();
            InstanceContext context = new InstanceContext(this);
            
            meserosUC = new MeserosUC("Mesero");
            gridpedidos.Children.Add(meserosUC);           
            meserosUC.Visibility = Visibility.Visible;
            try
            {
            server = new NotificarPedidoClient(context);
            server.AgregarUsuario("Mesero");
            meserosUC.eventoAgregarNuevoPedidoALista += UC_AgregandoNuevoPedido;
            meserosUC.eventoAbrirVentanaLocal += UC_AbrirVentanaPedidoLocal;
            }
            catch (CommunicationException e)
            {
            FuncionesComunes.MostrarMensajeDeError("Error de conexión con el servidor, intente más tarde."); 
                //poner un anuncio a la GUI que no esta conectado
            }                                       
        }

        /// <summary>
        /// Constructor Específico para el CallCenter
        /// </summary>
        /// <param name="idEmpleado"></param>
        public VentanaPedidos(long idEmpleado, string idEmpleadoGenerado)
        {
            InitializeComponent();
            InstanceContext context = new InstanceContext(this);
            idEmpleadoCallCenter = idEmpleado; idEmpleadoGeneradoCallCenter = idEmpleadoGenerado;

            meserosUC = new MeserosUC("CallCenter");
            gridpedidos.Children.Add(meserosUC);
            meserosUC.Visibility = Visibility.Visible;
            try
            {
                server = new NotificarPedidoClient(context);
                server.AgregarUsuario("Call Center");
                meserosUC.eventoAgregarNuevoPedidoALista += UC_AgregandoNuevoPedido;
                meserosUC.eventoAbrirVentanaLocal += UC_AbrirVentanaPedidoLocal;
                meserosUC.eventoAbrirVentanaADomicilio += UC_AbrirVentanaPedidoADomicilio;
            }
            catch (CommunicationException e)
            {
                FuncionesComunes.MostrarMensajeDeError("Error de conexión con el servidor, intente más tarde");
            }
        }

        private void UC_AbrirVentanaPedidoADomicilio(object sender, EventArgs e)
        {
            NuevoPedido ventanaNuevoPedido = new NuevoPedido("Domicilio");
            ventanaNuevoPedido.eventCancelar += UC_PedidoLocalCancelar;
            ventanaNuevoPedido.Visibility = Visibility.Visible;
            gridpedidos.Children.Add(ventanaNuevoPedido);
            meserosUC.Visibility = Visibility.Hidden;
        }

        private void UC_AbrirVentanaPedidoLocal(object sender, EventArgs e)
        {         
            NuevoPedido ventanaNuevoPedido = new NuevoPedido("Local");
            ventanaNuevoPedido.eventCancelar += UC_PedidoLocalCancelar;
            ventanaNuevoPedido.Visibility = Visibility.Visible;
            gridpedidos.Children.Add(ventanaNuevoPedido);
            meserosUC.Visibility = Visibility.Hidden;            
        }

        private void UC_PedidoLocalCancelar(object sender, EventArgs e)
        {
            meserosUC.Visibility = Visibility.Visible;
        }
       

        private void UC_AgregandoNuevoPedido(object sender, EventArgs e)
        {
           
        }

        public void MensajeNotificarPedido(string mensaje)
        {
            FuncionesComunes.MostrarMensajeExitoso(mensaje);
        }

        public void RecibirPedidoDomicilio(PedidoADomicilio pedido)
        {
            FuncionesComunes.MostrarMensajeExitoso("NUEVO PEDIDO A DOMICILIO");

            PedidoEnDataGrid pedidoEnDataGrid = new PedidoEnDataGrid(pedido.Empleado.idEmpleadoGenerado, pedido.Id.ToString(), "Domicilio", pedido.Cliente.nombre + " " + pedido.Cliente.apellidoPaterno + " " + pedido.Cliente.apellidoMaterno, pedido.instruccionesEspeciales);
            meserosUC.AgregarOSeleccionarNuevoPedido = pedidoEnDataGrid;
        }

        public void RecibirPedidoLocal(PedidoLocal pedido)
        {
            FuncionesComunes.MostrarMensajeExitoso("NUEVO PEDIDO LOCAL " + pedido.Estado.estadoPedido);

            if (pedido.Estado.estadoPedido != "Preparado")
            {
                PedidoEnDataGrid pedidoEnDataGrid = new PedidoEnDataGrid(pedido.Empleado.idEmpleadoGenerado, pedido.Id.ToString(), "Local", pedido.Mesa.numeroMesa.ToString(), pedido.instruccionesEspeciales);
                meserosUC.AgregarOSeleccionarNuevoPedido = pedidoEnDataGrid;
            }            
        }

        private void ButtonSalirClick(object sender, RoutedEventArgs e)
        {
            Dispatcher.Invoke(() =>
             {
                 MainWindow ventana = new MainWindow();
                 ventana.Show();
                 this.Close();
             });
        }
    }

    public class PedidoEnDataGrid
    {
        public string Empleado { get; set; }
        public string IdPedido { get; set; }
        public string Tipo { get; set; }
        public string MesaOCliente { get; set; }
        public string InstruccionesEspeciales { get; set; }

        public PedidoEnDataGrid(string empleado, string idPedido, string tipo, string mesaOCliente, string instruccionesEspeciales)
        {
            Empleado = empleado;
            IdPedido = idPedido;
            Tipo = tipo;
            MesaOCliente = mesaOCliente;
            InstruccionesEspeciales = instruccionesEspeciales;
        }
    }
}
