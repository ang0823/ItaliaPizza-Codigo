using ClienteItaliaPizza.Servicio;
using ClienteItaliaPizza.Validacion;
using DevExpress.Xpf.Docking.VisualElements;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace ClienteItaliaPizza.Pantallas
{
    /// <summary>
    /// Lógica de interacción para VentanaPedidos.xaml
    /// </summary>
    //[CallbackBehavior(ConcurrencyMode = ConcurrencyMode.Multiple)]
    public partial class VentanaPedidos : Window, INotificarPedidoCallback, IBuscarPedidosCallback 
    { 
        NotificarPedidoClient server;
        MeserosUC meserosUC;
        public static long idEmpleadoCallCenter; public static string idEmpleadoGeneradoCallCenter;

        ObservableCollection<PedidoEnDataGrid> pedidosEnEspera = new ObservableCollection<PedidoEnDataGrid>();
        ObservableCollection<PedidoEnDataGrid> pedidosPreparados = new ObservableCollection<PedidoEnDataGrid>();
        ObservableCollection<PedidoEnDataGrid> pedidosEnviados;
        ObservableCollection<PedidoEnDataGrid> pedidosEntregados = new ObservableCollection<PedidoEnDataGrid>();
        ObservableCollection<PedidoEnDataGrid> pedidosCancelados = new ObservableCollection<PedidoEnDataGrid>();

        List<PedidoLocal> ListaPedidosLocales = new List<PedidoLocal>();
        List<PedidoADomicilio> ListaPedidosDomicilio = new List<PedidoADomicilio>();
        
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
            
                meserosUC.EventoAbrirVentanaLocal += UC_AbrirVentanaPedidoLocal;
                meserosUC.EventEditarPedido += UC_EditarPedido;
                meserosUC.EventQuitarPedido += UC_QuitarPedido;
                meserosUC.EventCambiarEstado_Entregado += UC_CambiarEstadoAEntregado;
                meserosUC.EventTicketPDF += UC_GenerarTicketPDF;

                BuscarPedidosClient buscarPedidos = new BuscarPedidosClient(context);
                buscarPedidos.BuscarPedidosMesero();
            }
            catch (CommunicationException e)
            {
                FuncionesComunes.MostrarMensajeDeError("Error de conexión con el servidor, intente más tarde.");
                labelDesconectado.Content = "No hay conexión con el Servidor";
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
            pedidosEnviados = new ObservableCollection<PedidoEnDataGrid>();

            meserosUC = new MeserosUC("Call Center");
            gridpedidos.Children.Add(meserosUC);
            meserosUC.Visibility = Visibility.Visible;
            try
            {
                server = new NotificarPedidoClient(context);
                server.AgregarUsuario("Call Center");
                meserosUC.EventoAgregarNuevoPedidoALista += UC_AgregandoNuevoPedido;
                meserosUC.EventoAbrirVentanaLocal += UC_AbrirVentanaPedidoLocal;
                meserosUC.EventoAbrirVentanaADomicilio += UC_AbrirVentanaPedidoADomicilio;
                meserosUC.EventEditarPedido += UC_EditarPedido;
                meserosUC.EventQuitarPedido += UC_QuitarPedido;
                meserosUC.EventCambiarEstado_Entregado += UC_CambiarEstadoAEntregado;
                meserosUC.EventCambiarEstado_Enviado += UC_CambiarEstadoAEnviado;
                meserosUC.EventTicketPDF += UC_GenerarTicketPDF;
                BuscarPedidosClient buscarPedidos = new BuscarPedidosClient(context);
                buscarPedidos.BuscarPedidosCallCenter();


                //BuscarPedidosClient buscarPedidos = new BuscarPedidosClient(context);
                //((ICommunicationObject)buscarPedidos).Faulted += delegate { MessageBox.Show("Faulted " + buscarPedidos.State +); };
            // ((ICommunicationObject)buscarPedidos).Closed += delegate { MessageBox.Show(" Te desconectaste : Closed"); };
               // buscarPedidos.BuscarPedidosCallCenter();
            }
            catch (Exception ex)
            {
                FuncionesComunes.MostrarMensajeDeError("Error de conexión con el servidor, intente más tarde "+ex.Message + " " +ex.StackTrace);
                labelDesconectado.Content = "No hay conexión con el Servidor";
            }
        }

        private void UC_AgregandoNuevoPedido(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void UC_CambiarEstadoAEntregado(object sender, EventArgs e)
        {
            if (meserosUC.dataGridPedidosPreparados.IsVisible)
            {
                var pedidoLocalPreparadoSeleccionado = meserosUC.AgregarOSeleccionar_PedidoPreparado;
                var pedidoLocalEnLista = ListaPedidosLocales.Find(p => p.Id == Convert.ToInt32(pedidoLocalPreparadoSeleccionado.IdPedido));
                pedidoLocalEnLista.Estado = new Estado
                {
                    estadoPedido = "Entregado"
                };
                server.ModificarEstadoPedidoLocal(pedidoLocalEnLista);
            }
               
            if (meserosUC.dataGridPedidosEnviados.IsVisible)
            {
                var pedidoDomicilioPreparadoSeleccionado = meserosUC.AgregarOSeleccionar_PedidoEnviado;
                var pedidoDomicilioEnLista = ListaPedidosDomicilio.Find(p => p.Id == Convert.ToInt32(pedidoDomicilioPreparadoSeleccionado.IdPedido));
                pedidoDomicilioEnLista.Estado = new Estado
                {
                    estadoPedido = "Entregado"
                };
                server.ModificarEstadoPedidoADomicilio(pedidoDomicilioEnLista);
            }               
        }

        private void UC_CambiarEstadoAEnviado(object sender, EventArgs e)
        {
            var pedidoDomicilioPreparadoSeleccionado = meserosUC.AgregarOSeleccionar_PedidoPreparado;
            var pedidoDomicilioEnLista = ListaPedidosDomicilio.Find(p => p.Id == Convert.ToInt32(pedidoDomicilioPreparadoSeleccionado.IdPedido));
            pedidoDomicilioEnLista.Estado = new Estado
            {
                estadoPedido = "Enviado"
            };
            server.ModificarEstadoPedidoADomicilio(pedidoDomicilioEnLista);
        }

        private void UC_QuitarPedido(object sender, EventArgs e)
        {
            if (meserosUC.dataGridPedidosEnEspera.IsVisible)
            {
                var pedidoSeleccionado = meserosUC.AgregarOSeleccionar_PedidoEnEspera;
                if (pedidoSeleccionado.Tipo == "Domicilio")
                {
                    PedidoADomicilio pedidoDomicilioEnLista = ListaPedidosDomicilio.Find(p => p.Id == Convert.ToInt32(pedidoSeleccionado.IdPedido));
                    pedidoDomicilioEnLista.Estado = new Estado
                    {
                        estadoPedido = "Cancelado"
                    };                    
                    server.ModificarEstadoPedidoADomicilio(pedidoDomicilioEnLista);
                }
                else
                {
                    PedidoLocal pedidoLocalEnLista = ListaPedidosLocales.Find(p => p.Id == Convert.ToInt32(pedidoSeleccionado.IdPedido));
                    pedidoLocalEnLista.Estado = new Estado
                    {
                        estadoPedido = "Cancelado"
                    };
                    server.ModificarEstadoPedidoLocal(pedidoLocalEnLista);
                }
            }
            else if (meserosUC.dataGridPedidosPreparados.IsVisible)
            {
                var pedidoLocalPreparadoSeleccionado = meserosUC.AgregarOSeleccionar_PedidoPreparado;
                var pedidoLocalEnLista = ListaPedidosLocales.Find(p => p.Id == Convert.ToInt32(pedidoLocalPreparadoSeleccionado.IdPedido));
                pedidoLocalEnLista.Estado = new Estado
                {
                    estadoPedido = "Cancelado"
                };
                server.ModificarEstadoPedidoLocal(pedidoLocalEnLista);
            }
            else if (meserosUC.dataGridPedidosEnviados.IsVisible)
            {
                var pedidoDomicilioEnviadoSeleccionado = meserosUC.AgregarOSeleccionar_PedidoEnviado;
                var pedidoDomicilioEnLista = ListaPedidosDomicilio.Find(p => p.Id == Convert.ToInt32(pedidoDomicilioEnviadoSeleccionado.IdPedido));
                pedidoDomicilioEnLista.Estado = new Estado
                {
                    estadoPedido = "Cancelado"
                };
                server.ModificarEstadoPedidoADomicilio(pedidoDomicilioEnLista);
            }              
        }

        private void UC_EditarPedido(object sender, EventArgs e)
        {
            var pedidoSeleccionado = meserosUC.AgregarOSeleccionar_PedidoEnEspera;
            if(pedidoSeleccionado.Tipo == "Domicilio")
            {
                var pedidoDomicilioEncontrado = ListaPedidosDomicilio.Find(p => p.Id == Convert.ToInt32(pedidoSeleccionado.IdPedido));
                NuevoPedido edicionPedido = new NuevoPedido(pedidoDomicilioEncontrado);
                edicionPedido.EventCancelar += UC_PedidoLocalCancelar;
                edicionPedido.Visibility = Visibility.Visible;
                gridpedidos.Children.Add(edicionPedido);
                meserosUC.Visibility = Visibility.Hidden;
            }
            else
            {
                var pedidoLocalEncontrado = ListaPedidosLocales.Find(p => p.Id == Convert.ToInt32(pedidoSeleccionado.IdPedido));
                NuevoPedido edicionPedido = new NuevoPedido(pedidoLocalEncontrado);
                edicionPedido.EventCancelar += UC_PedidoLocalCancelar;
                edicionPedido.Visibility = Visibility.Visible;
                gridpedidos.Children.Add(edicionPedido);
                meserosUC.Visibility = Visibility.Hidden;
            }            
        }

        private void UC_GenerarTicketPDF(object sender, EventArgs e)
        {
            PedidoEnDataGrid pedidoEntregado = meserosUC.AgregarOSeleccionar_PedidoEntregado;

            string rutaTicketsPDF = "TicketsPDF";
            string rootDirectory = System.IO.Path.GetFullPath(AppDomain.CurrentDomain.BaseDirectory + "../../" + rutaTicketsPDF);

            if (!Directory.Exists(@rootDirectory))
            {               
                DirectoryInfo di = Directory.CreateDirectory(@rootDirectory);
            }
           
            if (pedidoEntregado.Tipo == "Domicilio")
            {
                PedidoADomicilio pedidoDomicilioEncontrado = ListaPedidosDomicilio.Find(p => p.Id == Convert.ToInt32(pedidoEntregado.IdPedido));
                meserosUC.GenerarTicketPDF(@rootDirectory, pedidoDomicilioEncontrado);
            }
            else
            {
                PedidoLocal pedidoLocalEncontrado = ListaPedidosLocales.Find(p => p.Id == Convert.ToInt32(pedidoEntregado.IdPedido));
                meserosUC.GenerarTicketPDF(@rootDirectory, pedidoLocalEncontrado);
            }
        }      

        private void UC_AbrirVentanaPedidoADomicilio(object sender, EventArgs e)
        {
            NuevoPedido ventanaNuevoPedido = new NuevoPedido("Domicilio");
            ventanaNuevoPedido.EventCancelar += UC_PedidoLocalCancelar;
            ventanaNuevoPedido.Visibility = Visibility.Visible;
            gridpedidos.Children.Add(ventanaNuevoPedido);
            meserosUC.Visibility = Visibility.Hidden;
        }

        private void UC_AbrirVentanaPedidoLocal(object sender, EventArgs e)
        {         
            NuevoPedido ventanaNuevoPedido = new NuevoPedido("Local");
            ventanaNuevoPedido.EventCancelar += UC_PedidoLocalCancelar;
            ventanaNuevoPedido.Visibility = Visibility.Visible;
            gridpedidos.Children.Add(ventanaNuevoPedido);
            meserosUC.Visibility = Visibility.Hidden;            
        }      

        private void UC_PedidoLocalCancelar(object sender, EventArgs e)
        {
            meserosUC.Visibility = Visibility.Visible;
        }      

        public void MensajeNotificarPedido(string mensaje)
        {
            FuncionesComunes.MostrarMensajeExitoso(mensaje);
        }

        public void RecibirPedidoDomicilio(PedidoADomicilio pedido)
        {
            FuncionesComunes.MostrarMensajeExitoso("PEDIDO A DOMICILIO: " + pedido.Estado.estadoPedido + "\nEmpleado: " + pedido.Empleado.idEmpleadoGenerado);

            if(pedido.Estado.estadoPedido == "En Espera")
            {
                PedidoADomicilio pedidoExiste = ListaPedidosDomicilio.Find(p => p.Id == Convert.ToInt32(pedido.Id));
                if (pedidoExiste != null)
                {
                    ListaPedidosDomicilio.Remove(pedidoExiste); ListaPedidosDomicilio.Add(pedido);
                    PedidoEnDataGrid pedidoAActualizar = pedidosEnEspera.First(p => p.IdPedido == pedido.Id.ToString());
                    pedidoAActualizar.Empleado = pedido.Empleado.idEmpleadoGenerado;
                    pedidoAActualizar.MesaOCliente = pedido.Cliente.nombre + " " + pedido.Cliente.apellidoPaterno + " " + pedido.Cliente.apellidoMaterno + ": " + pedido.direccionDestino;
                    pedidoAActualizar.InstruccionesEspeciales = pedido.instruccionesEspeciales;
                    meserosUC.dataGridPedidosEnEspera.Items.Refresh();
                }
                else
                {
                    PedidoEnDataGrid pedidoEnDataGrid = new PedidoEnDataGrid(pedido.Empleado.idEmpleadoGenerado, pedido.Id.ToString(), "Domicilio", pedido.Cliente.nombre + " " + pedido.Cliente.apellidoPaterno + " " + pedido.Cliente.apellidoMaterno + ": " + pedido.direccionDestino, pedido.instruccionesEspeciales);
                    meserosUC.AgregarOSeleccionar_PedidoEnEspera = pedidoEnDataGrid;
                    pedidosEnEspera.Add(pedidoEnDataGrid);
                    ListaPedidosDomicilio.Add(pedido);
                }               
            }
            else if(pedido.Estado.estadoPedido == "Preparado")
            {
                PedidoEnDataGrid pedidoEnDataGrid = new PedidoEnDataGrid(pedido.Empleado.idEmpleadoGenerado, pedido.Id.ToString(), "Domicilio", pedido.Cliente.nombre + " " + pedido.Cliente.apellidoPaterno + " " + pedido.Cliente.apellidoMaterno + ": " + pedido.direccionDestino, pedido.instruccionesEspeciales);
                meserosUC.AgregarOSeleccionar_PedidoEnEspera = pedidoEnDataGrid;
                pedidosPreparados.Add(pedidoEnDataGrid);
                pedidosEnEspera.Remove(pedidosEnEspera.FirstOrDefault(p => p.IdPedido == pedidoEnDataGrid.IdPedido));
            }
            else if(pedido.Estado.estadoPedido == "Enviado")
            {
                PedidoEnDataGrid pedidoEnDataGrid = new PedidoEnDataGrid(pedido.Empleado.idEmpleadoGenerado, pedido.Id.ToString(), "Domicilio", pedido.Cliente.nombre + " " + pedido.Cliente.apellidoPaterno + " " + pedido.Cliente.apellidoMaterno + ": " + pedido.direccionDestino, pedido.instruccionesEspeciales);
                meserosUC.AgregarOSeleccionar_PedidoEnviado = pedidoEnDataGrid;
                pedidosEnviados.Add(pedidoEnDataGrid);
                pedidosPreparados.Remove(pedidosPreparados.FirstOrDefault(p => p.IdPedido == pedidoEnDataGrid.IdPedido));
            }
            else if(pedido.Estado.estadoPedido == "Entregado")
            {
                PedidoEnDataGrid pedidoEnDataGrid = new PedidoEnDataGrid(pedido.Empleado.idEmpleadoGenerado, pedido.Id.ToString(), "Domicilio", pedido.Cliente.nombre + " " + pedido.Cliente.apellidoPaterno + " " + pedido.Cliente.apellidoMaterno + ": " + pedido.direccionDestino, pedido.instruccionesEspeciales);
                meserosUC.AgregarOSeleccionar_PedidoEntregado = pedidoEnDataGrid;
                pedidosEntregados.Add(pedidoEnDataGrid);
                pedidosEnviados.Remove(pedidosEnviados.FirstOrDefault(p => p.IdPedido == pedidoEnDataGrid.IdPedido));
            }
            else if(pedido.Estado.estadoPedido == "Cancelado")
            {
                PedidoEnDataGrid pedidoEnDataGrid = new PedidoEnDataGrid(pedido.Empleado.idEmpleadoGenerado, pedido.Id.ToString(), "Domicilio", pedido.Cliente.nombre + " " + pedido.Cliente.apellidoPaterno + " " + pedido.Cliente.apellidoMaterno + ": " + pedido.direccionDestino, pedido.instruccionesEspeciales);
                meserosUC.AgregarOSeleccionar_PedidoCancelado = pedidoEnDataGrid;
                pedidosCancelados.Add(pedidoEnDataGrid);

                if (pedidosEnEspera.FirstOrDefault(p => p.IdPedido == pedidoEnDataGrid.IdPedido) != null)
                    pedidosEnEspera.Remove(pedidosEnEspera.FirstOrDefault(p => p.IdPedido == pedidoEnDataGrid.IdPedido));
                else if (pedidosEnviados.FirstOrDefault(p => p.IdPedido == pedidoEnDataGrid.IdPedido) != null)
                    pedidosEnviados.Remove(pedidosEnviados.FirstOrDefault(p => p.IdPedido == pedidoEnDataGrid.IdPedido));                
            }
        }

        public void RecibirPedidoLocal(PedidoLocal pedido)
        {
            FuncionesComunes.MostrarMensajeExitoso("PEDIDO LOCAL: " + pedido.Estado.estadoPedido + "\nEmpleado: "+ pedido.Empleado.idEmpleadoGenerado);

            if (pedido.Estado.estadoPedido == "En Espera")
            {
                PedidoLocal pedidoExiste = ListaPedidosLocales.Find(p => p.Id == Convert.ToInt32(pedido.Id));
                if(pedidoExiste != null)
                {
                    ListaPedidosLocales.Remove(pedidoExiste); ListaPedidosLocales.Add(pedido);
                    PedidoEnDataGrid pedidoEditado = pedidosEnEspera.First(p => p.IdPedido == pedido.Id.ToString());
                    pedidoEditado.Empleado = pedido.Empleado.idEmpleadoGenerado;
                    pedidoEditado.MesaOCliente = pedido.Mesa.numeroMesa.ToString();
                    pedidoEditado.InstruccionesEspeciales = pedido.instruccionesEspeciales;
                    meserosUC.dataGridPedidosEnEspera.Items.Refresh();
                }
                else
                {
                    PedidoEnDataGrid pedidoEnDataGrid = new PedidoEnDataGrid(pedido.Empleado.idEmpleadoGenerado, pedido.Id.ToString(), "Local", pedido.Mesa.numeroMesa.ToString(), pedido.instruccionesEspeciales);
                    meserosUC.AgregarOSeleccionar_PedidoEnEspera = pedidoEnDataGrid;
                    pedidosEnEspera.Add(pedidoEnDataGrid);
                    ListaPedidosLocales.Add(pedido);
                }               
            }
            else if(pedido.Estado.estadoPedido == "Preparado")
            {
                PedidoEnDataGrid pedidoEnDataGrid = new PedidoEnDataGrid(pedido.Empleado.idEmpleadoGenerado, pedido.Id.ToString(), "Local", pedido.Mesa.numeroMesa.ToString(), pedido.instruccionesEspeciales);
                meserosUC.AgregarOSeleccionar_PedidoPreparado = pedidoEnDataGrid;
                pedidosPreparados.Add(pedidoEnDataGrid);
                pedidosEnEspera.Remove(pedidosEnEspera.FirstOrDefault(p => p.IdPedido == pedidoEnDataGrid.IdPedido));
            }
            else if(pedido.Estado.estadoPedido == "Entregado")
            {
                PedidoEnDataGrid pedidoEnDataGrid = new PedidoEnDataGrid(pedido.Empleado.idEmpleadoGenerado, pedido.Id.ToString(), "Local", pedido.Mesa.numeroMesa.ToString(), pedido.instruccionesEspeciales);
                meserosUC.AgregarOSeleccionar_PedidoEntregado = pedidoEnDataGrid;
                pedidosEntregados.Add(pedidoEnDataGrid);
                pedidosPreparados.Remove(pedidosPreparados.FirstOrDefault(p => p.IdPedido == pedidoEnDataGrid.IdPedido));
            }
            else if(pedido.Estado.estadoPedido == "Cancelado")
            {
                PedidoEnDataGrid pedidoEnDataGrid = new PedidoEnDataGrid(pedido.Empleado.idEmpleadoGenerado, pedido.Id.ToString(), "Local", pedido.Mesa.numeroMesa.ToString(), pedido.instruccionesEspeciales);
                meserosUC.AgregarOSeleccionar_PedidoCancelado = pedidoEnDataGrid;
                pedidosCancelados.Add(pedidoEnDataGrid);

                if (pedidosEnEspera.FirstOrDefault(p => p.IdPedido == pedidoEnDataGrid.IdPedido) != null)
                    pedidosEnEspera.Remove(pedidosEnEspera.FirstOrDefault(p => p.IdPedido == pedidoEnDataGrid.IdPedido));
                else if (pedidosPreparados.FirstOrDefault(p => p.IdPedido == pedidoEnDataGrid.IdPedido) != null)
                    pedidosPreparados.Remove(pedidosPreparados.FirstOrDefault(p => p.IdPedido == pedidoEnDataGrid.IdPedido));
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

        void IBuscarPedidosCallback.ObtenerTodosPedidos(PedidoADomicilioDeServidor[] pedidosADomicilio, PedidoLocalDeServidor[] pedidosLocales)
        {
            //Pedidos A Domicilio                        
            foreach (var pedidoDomicilioEspera in pedidosADomicilio.Where(p => p.estado == "En Espera"))
            {
                ListaPedidosDomicilio.Add(ConvertidorDeObjetos.PedidoADomicilioDeServidor_A_PedidoADomicilio(pedidoDomicilioEspera));
                PedidoEnDataGrid pedidoEnDataGrid = new PedidoEnDataGrid(pedidoDomicilioEspera.idGeneradoDeEmpleado, pedidoDomicilioEspera.id.ToString(), "Domicilio", pedidoDomicilioEspera.cliente.nombre + " " + pedidoDomicilioEspera.cliente.apellidoPaterno + " " + pedidoDomicilioEspera.cliente.apellidoMaterno + ": " + pedidoDomicilioEspera.direccionDestino, pedidoDomicilioEspera.instruccionesDePedido);
                pedidosEnEspera.Add(pedidoEnDataGrid);
            }

            foreach (var pedidoDomicilioPreparado in pedidosADomicilio.Where(p => p.estado == "Preparado"))
            {
                ListaPedidosDomicilio.Add(ConvertidorDeObjetos.PedidoADomicilioDeServidor_A_PedidoADomicilio(pedidoDomicilioPreparado));
                PedidoEnDataGrid pedidoEnDataGrid = new PedidoEnDataGrid(pedidoDomicilioPreparado.idGeneradoDeEmpleado, pedidoDomicilioPreparado.id.ToString(), "Domicilio", pedidoDomicilioPreparado.cliente.nombre + " " + pedidoDomicilioPreparado.cliente.apellidoPaterno + " " + pedidoDomicilioPreparado.cliente.apellidoMaterno + ": " + pedidoDomicilioPreparado.direccionDestino, pedidoDomicilioPreparado.instruccionesDePedido);
                pedidosPreparados.Add(pedidoEnDataGrid);
            }

            foreach (var pedidoDomicilioEnviado in pedidosADomicilio.Where(p => p.estado == "Enviado"))
            {
                ListaPedidosDomicilio.Add(ConvertidorDeObjetos.PedidoADomicilioDeServidor_A_PedidoADomicilio(pedidoDomicilioEnviado));
                PedidoEnDataGrid pedidoEnDataGrid = new PedidoEnDataGrid(pedidoDomicilioEnviado.idGeneradoDeEmpleado, pedidoDomicilioEnviado.id.ToString(), "Domicilio", pedidoDomicilioEnviado.cliente.nombre + " " + pedidoDomicilioEnviado.cliente.apellidoPaterno + " " + pedidoDomicilioEnviado.cliente.apellidoMaterno + ": " + pedidoDomicilioEnviado.direccionDestino, pedidoDomicilioEnviado.instruccionesDePedido);
                pedidosEnviados.Add(pedidoEnDataGrid);
            }
            foreach (var pedidoDomicilioEnviado in pedidosADomicilio.Where(p => p.estado == "Entregado"))
            {
                ListaPedidosDomicilio.Add(ConvertidorDeObjetos.PedidoADomicilioDeServidor_A_PedidoADomicilio(pedidoDomicilioEnviado));
                PedidoEnDataGrid pedidoEnDataGrid = new PedidoEnDataGrid(pedidoDomicilioEnviado.idGeneradoDeEmpleado, pedidoDomicilioEnviado.id.ToString(), "Domicilio", pedidoDomicilioEnviado.cliente.nombre + " " + pedidoDomicilioEnviado.cliente.apellidoPaterno + " " + pedidoDomicilioEnviado.cliente.apellidoMaterno + ": " + pedidoDomicilioEnviado.direccionDestino, pedidoDomicilioEnviado.instruccionesDePedido);
                pedidosEntregados.Add(pedidoEnDataGrid);
            }
            foreach (var pedidoDomicilioEnviado in pedidosADomicilio.Where(p => p.estado == "Cancelado"))
            {
                ListaPedidosDomicilio.Add(ConvertidorDeObjetos.PedidoADomicilioDeServidor_A_PedidoADomicilio(pedidoDomicilioEnviado));
                PedidoEnDataGrid pedidoEnDataGrid = new PedidoEnDataGrid(pedidoDomicilioEnviado.idGeneradoDeEmpleado, pedidoDomicilioEnviado.id.ToString(), "Domicilio", pedidoDomicilioEnviado.cliente.nombre + " " + pedidoDomicilioEnviado.cliente.apellidoPaterno + " " + pedidoDomicilioEnviado.cliente.apellidoMaterno + ": " + pedidoDomicilioEnviado.direccionDestino, pedidoDomicilioEnviado.instruccionesDePedido);
                pedidosCancelados.Add(pedidoEnDataGrid);
            }

            // Pedidos Locales
            CargarPedidosLocales(pedidosLocales);

            meserosUC.ListaEnEspera_DataGrid = pedidosEnEspera;
            meserosUC.ListaPreparados_DataGrid = pedidosPreparados;
            meserosUC.ListaEnviados_DataGrid = pedidosEnviados;
            meserosUC.ListaEntregados_DataGrid = pedidosEntregados;
            meserosUC.ListaCancelados_DataGrid = pedidosCancelados;
        }

        void IBuscarPedidosCallback.ObtenerPedidosLocales(PedidoLocalDeServidor[] pedidosLocales)
        {
            CargarPedidosLocales(pedidosLocales);

            meserosUC.ListaEnEspera_DataGrid = pedidosEnEspera;
            meserosUC.ListaPreparados_DataGrid = pedidosPreparados;
            meserosUC.ListaEntregados_DataGrid = pedidosEntregados;
            meserosUC.ListaCancelados_DataGrid = pedidosCancelados;
        }

        void IBuscarPedidosCallback.MensajeErrorBuscarPedidos(string mensaje)
        {
            FuncionesComunes.MostrarMensajeDeError(mensaje);
        }       

        /// <summary>
        /// Método que carga en los DataGrid los pedidos Locales que son devueltos por el SERVIDOR.
        /// </summary>
        /// <param name="pedidosLocales"></param>
        public void CargarPedidosLocales(PedidoLocalDeServidor[] pedidosLocales)
        {
            foreach (var pedidoLocalEspera in pedidosLocales.Where(p => p.estado == "En Espera"))
            {
                ListaPedidosLocales.Add(ConvertidorDeObjetos.PedidoLocalDeServidor_A_PedidoLocal(pedidoLocalEspera));
                PedidoEnDataGrid pedidoEnDataGrid = new PedidoEnDataGrid(pedidoLocalEspera.idGeneradoDeEmpleado, pedidoLocalEspera.id.ToString(), "Local", pedidoLocalEspera.numeroMesa.ToString(), pedidoLocalEspera.instruccionesDePedido);
                pedidosEnEspera.Add(pedidoEnDataGrid);
            }
            foreach (var pedidoLocalPreparado in pedidosLocales.Where(p => p.estado == "Preparado"))
            {
                ListaPedidosLocales.Add(ConvertidorDeObjetos.PedidoLocalDeServidor_A_PedidoLocal(pedidoLocalPreparado));
                PedidoEnDataGrid pedidoEnDataGrid = new PedidoEnDataGrid(pedidoLocalPreparado.idGeneradoDeEmpleado, pedidoLocalPreparado.id.ToString(), "Local", pedidoLocalPreparado.numeroMesa.ToString(), pedidoLocalPreparado.instruccionesDePedido);
                pedidosPreparados.Add(pedidoEnDataGrid);
            }

            foreach (var pedidoLocalEnviado in pedidosLocales.Where(p => p.estado == "Entregado"))
            {
                ListaPedidosLocales.Add(ConvertidorDeObjetos.PedidoLocalDeServidor_A_PedidoLocal(pedidoLocalEnviado));
                PedidoEnDataGrid pedidoEnDataGrid = new PedidoEnDataGrid(pedidoLocalEnviado.idGeneradoDeEmpleado, pedidoLocalEnviado.id.ToString(), "Local", pedidoLocalEnviado.numeroMesa.ToString(), pedidoLocalEnviado.instruccionesDePedido);
                pedidosEntregados.Add(pedidoEnDataGrid);
            }

            foreach (var pedidoLocalEnviado in pedidosLocales.Where(p => p.estado == "Cancelado"))
            {
                ListaPedidosLocales.Add(ConvertidorDeObjetos.PedidoLocalDeServidor_A_PedidoLocal(pedidoLocalEnviado));
                PedidoEnDataGrid pedidoEnDataGrid = new PedidoEnDataGrid(pedidoLocalEnviado.idGeneradoDeEmpleado, pedidoLocalEnviado.id.ToString(), "Local", pedidoLocalEnviado.numeroMesa.ToString(), pedidoLocalEnviado.instruccionesDePedido);
                pedidosCancelados.Add(pedidoEnDataGrid);
            }
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
