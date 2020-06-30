using ClienteItaliaPizza.Servicio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
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

namespace ClienteItaliaPizza.Pantallas
{
    /// <summary>
    /// Lógica de interacción para VentanaPedidos.xaml
    /// </summary>
    public partial class VentanaPedidos : Window, INotificarPedidoCallback
    {
        NotificarPedidoClient server;
        MeserosUC meserosUC;
        public VentanaPedidos(string tipoUsuario)
        {
            InitializeComponent();
            InstanceContext context = new InstanceContext(this);
            if (tipoUsuario == "CallCenter")
            {
                MeserosUC meserosUC = new MeserosUC("CallCenter");
                gridpedidos.Children.Add(meserosUC);
                meserosUC.Visibility = Visibility.Visible;
            }
            if(tipoUsuario== "Mesero")
            {
                meserosUC = new MeserosUC("Mesero");
                gridpedidos.Children.Add(meserosUC);
                meserosUC.Visibility = Visibility.Visible;
                try
                {
                    server = new NotificarPedidoClient(context);
                    server.AgregarUsuario("Mesero");
                    meserosUC.AgregarNuevoPedidoALista += UC_AgregandoNuevoPedido;
                }
                catch (CommunicationException e)
                {
                    FuncionesComunes.MostrarMensajeDeError("Error de conexión con el servidor, intente más tarde");
                }               
            }            
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
            throw new NotImplementedException();
        }

        public void RecibirPedidoLocal(PedidoLocal pedido)
        {
            FuncionesComunes.MostrarMensajeExitoso("NUEVO PEDIDO LOCAL");
            meserosUC.AgregarOSeleccionarNuevoPedido = pedido.Id +" "+ pedido.Mesa.numeroMesa.ToString() + " ";
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
}
