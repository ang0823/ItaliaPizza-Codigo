using System;
using System.Collections.Generic;
using ClienteItaliaPizza.Servicio;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.ServiceModel;
using ClienteItaliaPizza.Validacion;
using System.Collections.ObjectModel;
using System.Linq;
using ClienteItaliaPizza.Pantallas;

namespace ClienteItaliaPizza
{
    /// <summary>
    /// Lógica de interacción para PedidoADomicilio.xaml
    /// Autor: Caicero Franco Elsa Irasema
    /// </summary>
    public partial class NuevoPedido : UserControl, IAdministrarPedidosMeserosCallback , IAdministrarPedidosCallCenterCallback
    {
        string tipoDePedido;
        string mesaSeleccionada;
        InstanceContext instanceContext;
        //AdministrarPedidosMeserosClient meserosClient;
        AdministrarPedidosCallCenterClient callCenterClient;
        //listas de productos seleccionados para el NUEVO Pedido
        private List<Producto> productosSeleccionados = new List<Producto>();
        private List<ProvisionDirecta> provisionesSeleccionadas = new List<ProvisionDirecta>();
        ObservableCollection<Orden> listaOrdenes = new ObservableCollection<Orden>();
        EmpleadoPizzeria[] Meseros;
        IAdministrarPedidosMeseros serverMeseros;
        public event EventHandler eventCancelar;

        //Constructor para registrar un nuevo Pedido (hacer uno para la edicion de pedido)
        public NuevoPedido(string tipoPedido)
        {
            InitializeComponent();
            dataGridOrden.ItemsSource = listaOrdenes;
            tipoDePedido = tipoPedido;
            try
            {
                if (tipoPedido.Equals("Local"))
                {
                    UC_NuevoPLocal.Visibility = Visibility.Visible;
                    instanceContext = new InstanceContext(this);
                    //meserosClient = new AdministrarPedidosMeserosClient(instanceContext);
                    var canal  = new DuplexChannelFactory<IAdministrarPedidosMeseros>(instanceContext, "*");
                    serverMeseros = canal.CreateChannel();

                    Meseros = serverMeseros.ObtenerMeseros();
                    foreach (var mesero in Meseros)
                        UC_NuevoPLocal.EditarSeleccionComboBoxNumEmpleado = mesero.idGenerado;

                    serverMeseros.ObtenerProductos();                    
                }
                if (tipoPedido.Equals("Domicilio"))
                {
                    UC_NuevoDomicilio.Visibility = Visibility.Visible;
                    instanceContext = new InstanceContext(this);
                    callCenterClient = new AdministrarPedidosCallCenterClient(instanceContext);
                    callCenterClient.ObtenerDatos();
                }
            }
            catch (CommunicationException e)
            {
                FuncionesComunes.MostrarMensajeDeError("No se ha podido establecer comunicación con el servidor\n"+e.Data.ToString());
            }
        }       
       

        private void GridBebidas_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {                 
            //ImageList listaImagesBebidas = new ImageList();
            /* ListViewBebidas.ItemsSource = new MovieData[] {
             new MovieData{Title="Movie 1", ImageData=LoadImage("C:/Users/survi/Pictures/Granos Selectos (4).jpg")},
             new MovieData{Title="Movie 2", ImageData=LoadImage("C:/Users/survi/Pictures/Granos Selectos (4).jpg")},
             new MovieData{Title="Movie 3", ImageData=LoadImage("C:/Users/survi/Pictures/Granos Selectos (4).jpg")},
             new MovieData{Title="Movie 4", ImageData=LoadImage("C:/Users/survi/Pictures/Granos Selectos (4).jpg")},
             new MovieData{Title="Movie 5", ImageData=LoadImage("C:/Users/survi/Pictures/Granos Selectos (4).jpg")},
             new MovieData{Title="Movie 6", ImageData=LoadImage("C:/Users/survi/Pictures/Granos Selectos (4).jpg")}
             };  */
        }

        private BitmapImage LoadImage(string filename)
        {
            return new BitmapImage(new Uri(filename));
        }

        private void TextBoxDescuento_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (Validador.validarSoloNumeros(e.Text) == false)
                e.Handled = true;
        }




        //      SELECCIÓN DE PRODUCTOS  **************************************************+
        private void ListViewBebidas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var provisionSeleccionada = ListViewBebidas.SelectedItem as ProvisionVentaDirecta;
            var ordenExistente = listaOrdenes.FirstOrDefault<Orden>(i => i.nombreProducto == provisionSeleccionada.nombre);
            if (ordenExistente == null)
            {
                Orden orden = new Orden();
                orden.cantidad = 1;
                orden.nombreProducto = provisionSeleccionada.nombre;
                orden.precioUnitario = provisionSeleccionada.precioUnitario;
                orden.precioTotal = provisionSeleccionada.precioUnitario;
                listaOrdenes.Add(orden);

                ProvisionDirecta provision = ConvertidorDeObjetos.ProvisionVentaDirecta_A_ProvisionDirecta(provisionSeleccionada);
                provisionesSeleccionadas.Add(provision);
                labelSubtotal.Content = orden.precioUnitario + FuncionesComunes.ParsearADouble(labelSubtotal.Content.ToString());
                labelTotal.Content = orden.precioUnitario + FuncionesComunes.ParsearADouble(labelTotal.Content.ToString());
            }
            else
            {                
                ordenExistente.cantidad++;
                ordenExistente.precioTotal = ordenExistente.precioUnitario * ordenExistente.cantidad;
                dataGridOrden.Items.Refresh();
                labelTotal.Content = ordenExistente.precioUnitario + FuncionesComunes.ParsearADouble(labelTotal.Content.ToString());
            }
        }

        private void ListViewPostres_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ObtenerProductoSeleccionado<System.Windows.Controls.ListView>(ListViewPostres);
        }
        private void ListViewEnsaladas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ObtenerProductoSeleccionado<System.Windows.Controls.ListView>(ListViewEnsaladas);
        }

        private void ListViewPizzas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ObtenerProductoSeleccionado<System.Windows.Controls.ListView>(ListViewPizzas);
        }

        private void ListViewPastas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ObtenerProductoSeleccionado<System.Windows.Controls.ListView>(ListViewPastas);
        }

        /// <summary>
        /// Obtiene el producto seleccionado de los ListView que exponen PRODUCTOS y los agrega a la lista de ordenes del pedido
        /// </summary>
        /// <typeparam name="T"> T es un tipo de dato System.Windows.Controls.ListView  </typeparam>
        /// <param name="t"> Es el ListView que expone los productos </param>
        private void ObtenerProductoSeleccionado<T>(System.Windows.Controls.ListView t) where T : System.Windows.Controls.ListView
        {
            var productoSeleccionado = t.SelectedItem as ProductoDePedido;
            var ordenExistente = listaOrdenes.FirstOrDefault(i => i.nombreProducto == productoSeleccionado.nombre);
            if (ordenExistente == null)
            {
                Orden orden = new Orden
                {
                    cantidad = 1,
                    nombreProducto = productoSeleccionado.nombre,
                    precioUnitario = productoSeleccionado.precioUnitario,
                    precioTotal = productoSeleccionado.precioUnitario
                };
                listaOrdenes.Add(orden);

                Producto producto = ConvertidorDeObjetos.ProductoDePedido_A_Producto(productoSeleccionado);
                productosSeleccionados.Add(producto);
                labelSubtotal.Content = orden.precioUnitario + FuncionesComunes.ParsearADouble(labelSubtotal.Content.ToString());
                labelTotal.Content = orden.precioUnitario + FuncionesComunes.ParsearADouble(labelTotal.Content.ToString());
            }
            else
            {
                ordenExistente.cantidad++;
                ordenExistente.precioTotal = ordenExistente.precioUnitario * ordenExistente.cantidad;
                dataGridOrden.Items.Refresh();
                labelTotal.Content = ordenExistente.precioUnitario + FuncionesComunes.ParsearADouble(labelTotal.Content.ToString());
            }
        }
        //      SELECCIÓN DE PRODUCTOS  **************************************************




        //      BOTONES **************************************************************************
        private void ButtonCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
            this.eventCancelar?.Invoke(this, e);
        }

        //Llamará al registrar local, domicilio, editar local y editar domicilio
        private void ButtonAceptar_Click(object sender, RoutedEventArgs e)
        {
            if (tipoDePedido == "Local")
            {
                if (ValidarCamposLlenosPedidoLocal())
                {
                    try
                    {
                        if (RegistrarPedidoLocal()) VaciarCamposPedidoLocal();
                        else
                        {
                            var canal = new DuplexChannelFactory<IAdministrarPedidosMeseros>(instanceContext, "*");
                            serverMeseros = canal.CreateChannel();
                        }
                    }
                    catch (Exception ex)
                    {
                        FuncionesComunes.MostrarMensajeDeError(ex.Message + ex.StackTrace);
                    }
                }
                else
                {
                    FuncionesComunes.MostrarMensajeDeError("Existen campos vacíos");
                }
            }
            else
            {
                if (ValidarCamposLlenosPedidoDomicilio())
                {
                    try
                    {
                        RegistrarPedidoADomicilio();
                    }
                    catch (Exception ex)
                    {
                        FuncionesComunes.MostrarMensajeDeError(ex.Message + "\n" + ex.StackTrace);
                    }
                }
            }
        }
        //      BOTONES **************************************************************************



        //      MÉTODOS ESPECÍFICOS DEL PEDIDO LOCAL **************************************************
        public bool ValidarCamposLlenosPedidoLocal()
        {
            if (UC_NuevoPLocal.comboBoxNumEmpleado.SelectedIndex != -1 &&
                UC_NuevoPLocal.comboBoxNoMesa.SelectedIndex != -1 &&
                textBoxDescuento.Text != null && listaOrdenes != null)
            {
                int descuento = FuncionesComunes.ParsearAEntero(textBoxDescuento.Text);
                if (descuento >= 100)
                {
                    FuncionesComunes.MostrarMensajeDeError("El límite del descuento es 100%"); return false;
                }
                return true;
            }
            return false;
        }

        public void VaciarCamposPedidoLocal()
        {            
            listaOrdenes.Clear();
            dataGridOrden.ItemsSource = listaOrdenes;
            textBoxInstruccionesEspeciales.Text = "";
            textBoxDescuento.Text = "0";
            labelSubtotal.Content = 0;
            labelTotal.Content = 0;
        }

        public bool RegistrarPedidoLocal()
        {
            try
            {
                int numeroMesa = FuncionesComunes.ParsearAEntero(mesaSeleccionada);

                var mesero = Meseros.FirstOrDefault<EmpleadoPizzeria>(e => e.idGenerado == UC_NuevoPLocal.EditarSeleccionComboBoxNumEmpleado);

                textBoxDescuento.Text = "." + textBoxDescuento.Text;
                var descuento = FuncionesComunes.ParsearADouble(textBoxDescuento.Text);

                PedidoLocal pedidoLocalNuevo = new PedidoLocal
                {
                    fecha = DateTime.Now,                     
                    instruccionesEspeciales = textBoxInstruccionesEspeciales.Text,
                    Mesa = new Mesa
                    {
                        numeroMesa = (short)numeroMesa
                    },
                    Empleado = new Empleado
                    {
                        IdEmpleado = mesero.id,
                        idEmpleadoGenerado = mesero.idGenerado                        
                    },
                    Estado = new Estado
                    {
                       
                        estadoPedido = "En Espera"
                    },
                    Cuenta = new Cuenta
                    {
                        Id = GenerarIdPedidoLocal(numeroMesa),
                        subTotal = (double)labelSubtotal.Content,
                        iva = 0.16,
                        descuento = descuento,
                        precioTotal = (double)labelTotal.Content,
                    }                    
                };

                pedidoLocalNuevo.Producto = new Producto[productosSeleccionados.Count];
                productosSeleccionados.CopyTo(pedidoLocalNuevo.Producto);
                pedidoLocalNuevo.ProvisionDirecta = new ProvisionDirecta[provisionesSeleccionadas.Count];
                provisionesSeleccionadas.CopyTo(pedidoLocalNuevo.ProvisionDirecta);

                return serverMeseros.RegistrarPedidoLocal(pedidoLocalNuevo);
            }
            catch (CommunicationException)
            {
                FuncionesComunes.MostrarMensajeDeError("No se ha podido establecer comunicación con el servidor");
                return false;
            }
        }

        public string GenerarIdPedidoLocal(int numeroMesa)
        {
            string id;
            TimeSpan horaRealizacionDePedido = DateTime.Now.TimeOfDay;
            id = "PL-" + numeroMesa + horaRealizacionDePedido;
            return id;
        }
        //      MÉTODOS ESPECÍFICOS DEL PEDIDO LOCAL **************************************************




        //      MÉTODOS ESPECÍFICOS DEL PEDIDO A DOMICILIO ****************************************************
        public bool ValidarCamposLlenosPedidoDomicilio()
        {
            if (textBoxDescuento.Text != null && listaOrdenes.Count != 0)
            {
                return true;
            }
            return false;
        }

        public bool ValidarCamposVacios_AgregarCliente_ADomicilio()
        {
            if (UC_NuevoDomicilio.textBoxNombre.Text.Length > 0 &&
                UC_NuevoDomicilio.textBoxApellidoPaterno.Text.Length > 0 &&
                UC_NuevoDomicilio.textBoxApellidoMaterno.Text.Length > 0 &&               
                UC_NuevoDomicilio.ObtenerTexto_ComboBoxTelefono != null &&
                UC_NuevoDomicilio.textBoxCalle.Text.Length > 0 &&
                UC_NuevoDomicilio.textBoxColonia.Text.Length > 0 &&
                UC_NuevoDomicilio.textBoxCodigoPostal.Text.Length > 0 &&
                UC_NuevoDomicilio.textBoxNoInterior.Text.Length > 0 &&
                UC_NuevoDomicilio.textBoxNoExterior.Text.Length > 0) return true;
            return false;
        }
       

        public void RegistrarPedidoADomicilio()
        {
            Cliente1 clienteEnLista = new Cliente1();
            var nombreCompleto = UC_NuevoDomicilio.EditarComboBoxClienteNombre.ToString();

            foreach(var cliente in UC_NuevoDomicilio.clientes)
            {
                if (nombreCompleto.Contains(cliente.nombre + " " +cliente.apellidoPaterno + " " + cliente.apellidoMaterno))
                {
                    clienteEnLista = cliente;
                    break;
                }                
            }

            PedidoADomicilio pedidoADomicilio = new PedidoADomicilio
            {
                Cliente = new Cliente
                {
                    Id = clienteEnLista.id,
                    nombre = clienteEnLista.nombre,
                    apellidoPaterno = clienteEnLista.apellidoPaterno,
                    apellidoMaterno = clienteEnLista.apellidoMaterno
                },
                ClienteId = clienteEnLista.id,
                fecha = DateTime.Now,
                instruccionesEspeciales = textBoxInstruccionesEspeciales.Text,
                Empleado = new Empleado
                {
                    IdEmpleado = VentanaPedidos.idEmpleadoCallCenter,
                    idEmpleadoGenerado = VentanaPedidos.idEmpleadoGeneradoCallCenter
                    },
                    Estado = new Estado { estadoPedido = "En Espera" },
                    Cuenta = new Cuenta
                    {
                        Id = GenerarIdPedidoADomicilio(clienteEnLista.id),
                        subTotal = (double)labelSubtotal.Content,
                        iva = 0.16,
                        descuento = FuncionesComunes.ParsearADouble(textBoxDescuento.Text),
                        precioTotal = (double)labelTotal.Content
                    }
                };

            pedidoADomicilio.Producto = new Producto[productosSeleccionados.Count];
            productosSeleccionados.CopyTo(pedidoADomicilio.Producto);
            pedidoADomicilio.ProvisionDirecta = new ProvisionDirecta[provisionesSeleccionadas.Count];
            provisionesSeleccionadas.CopyTo(pedidoADomicilio.ProvisionDirecta);

            callCenterClient.RegistrarPedidoADomicilio(pedidoADomicilio);            
        }

        public string GenerarIdPedidoADomicilio(int idCliente)
        {
            string id;
            TimeSpan horaRealizacionDePedido = DateTime.Now.TimeOfDay;
            id = "PD-" + idCliente + horaRealizacionDePedido;
            return id;
        }
        //      MÉTODOS ESPECÍFICOS DEL PEDIDO A DOMICILIO ****************************************************




        //      CALLBACKS DE LOS MESEROS *******************************
        public void DatosRecuperados(ProductoDePedido[] productos, ProvisionVentaDirecta[] provisiones)
        {
            foreach (var producto in productos)
            {
                if (producto.categoria == "Ensaladas")
                    ListViewEnsaladas.Items.Add(producto);
                if (producto.categoria == "Pizzas")
                    ListViewPizzas.Items.Add(producto);
                if (producto.categoria == "Pastas")
                    ListViewPastas.Items.Add(producto);
                if (producto.categoria == "Postres")
                    ListViewPostres.Items.Add(producto);
            }
            ListViewBebidas.ItemsSource = provisiones;
        }

        public void MensajeAdministrarPedidosMeseros(string mensaje)
        {
            MessageBox.Show(mensaje); 
        }
        //      CALLBACKS DE LOS MESEROS *******************************





        //      CALLBACKS DEL CALL CENTER ******************************
        public void Datos(Cliente1[] clientes, ProductoDePedido[] productos, ProvisionVentaDirecta[] provisiones)
        {
            foreach (var cliente in clientes) 
            {
                UC_NuevoDomicilio.clientes.Add(cliente); 

                UC_NuevoDomicilio.EditarComboBoxClienteNombre = cliente.nombre + " " + cliente.apellidoPaterno+" " + cliente.apellidoMaterno;
            }

            foreach (var producto in productos)
            {
                if (producto.categoria == "Ensaladas")
                    ListViewEnsaladas.Items.Add(producto);
                if (producto.categoria == "Pizzas")
                    ListViewPizzas.Items.Add(producto);
                if (producto.categoria == "Pastas")
                    ListViewPastas.Items.Add(producto);
                if (producto.categoria == "Postres")
                    ListViewPostres.Items.Add(producto);
            }
            ListViewBebidas.ItemsSource = provisiones;
        }

        public void Mensaje([MessageParameter(Name = "mensaje")] string mensaje1)
        {
            MessageBox.Show(mensaje1);         
        }

        public void NotificacionClienteDePedido(string mensaje, Cliente1 cliente)
        {
            UC_NuevoDomicilio.clientes.Add(cliente); FuncionesComunes.MostrarMensajeExitoso(mensaje);
            UC_NuevoDomicilio.EditarComboBoxClienteNombre = cliente.nombre + " " + cliente.apellidoPaterno + " " + cliente.apellidoMaterno;
            UC_NuevoDomicilio.MostrarSoloComboBox();
        }

        //      CALLBACKS DEL CALL CENTER ******************************



        /**
         * MÉTODOS CORRESPONDIENTES AL USER CONTROL DEL PEDIDO LOCAL
         */
        private void UC_NuevoPLocal_eventLlenarNoMesa(object sender, EventArgs e)
        {
            for (int i = 1; i <= 5; i++)
            {
                UC_NuevoPLocal.EditarSeleccionComboBoxNoMesa = i.ToString();
            }
        }

        private void UC_NuevoPLocal_eventLlenarNumEmpleado(object sender, EventArgs e)
        {          
        }

        private void UC_NuevoPLocal_eventSeleccionarNoMesa(object sender, EventArgs e)
        {
            mesaSeleccionada = UC_NuevoPLocal.EditarSeleccionComboBoxNoMesa.ToString();
        }

        private void UC_NuevoPLocal_eventSeleccionarNumEmpleado(object sender, EventArgs e)
        {           
        }




        /**
         * MÉTODOS CORRESPONIDENTES AL USER CONTROL DEL PEDIDO A DOMICILIO
         */
        private void UC_NuevoDomicilio_eventLlenarComboBoxClienteNombre(object sender, EventArgs e)
        {
        }

        private void UC_NuevoDomicilio_eventSeleccionarCliente(object sender, EventArgs e)
        {            
        }

        private void UC_NuevoDomicilio_eventLlenarComboBoxDireccion(object sender, EventArgs e)
        {           
        }

        private void UC_NuevoDomicilio_eventSeleccionarDirección(object sender, EventArgs e)
        {            
        }

        private void UC_NuevoDomicilio_eventEditarTelefono(object sender, EventArgs e)
        {           
        }

        private void UC_NuevoDomicilio_eventEditarTextBoxDireccion(object sender, EventArgs e)
        {
        }

        private void UC_NuevoDomicilio_eventAgregarNuevoCliente(object sender, EventArgs e)
        {
            if(ValidarCamposVacios_AgregarCliente_ADomicilio())
            {
                Cliente cliente = new Cliente
                {
                    nombre = UC_NuevoDomicilio.textBoxNombre.Text,
                    apellidoPaterno = UC_NuevoDomicilio.textBoxApellidoPaterno.Text,
                    apellidoMaterno = UC_NuevoDomicilio.textBoxApellidoMaterno.Text
                };

                Direccion direccion = new Direccion
                {
                    calle = UC_NuevoDomicilio.textBoxCalle.Text,
                    colonia = UC_NuevoDomicilio.textBoxColonia.Text,
                    codigoPostal = UC_NuevoDomicilio.textBoxCodigoPostal.Text,
                    numeroInterior = UC_NuevoDomicilio.textBoxNoInterior.Text,
                    numeroExterior = UC_NuevoDomicilio.textBoxNoExterior.Text
                };
   
                Telefono telefono = new Telefono
                {
                     numeroTelefono = UC_NuevoDomicilio.ObtenerTexto_ComboBoxTelefono
                };

                callCenterClient.RegistrarCliente(cliente, direccion, telefono);
            }
            else
            {
                FuncionesComunes.MostrarMensajeDeError("Existen campos vacíos");
            }
        }

        

        /// <summary>
        /// Clase que servirá de apoyo para guardar los productos elegidos para el Pedido y calcular los costos.
        /// Es utilizada en el DataGrid de Ordenes. 
        /// </summary>
        public class Orden
        {
            public int cantidad { get; set; }
            public string nombreProducto { get; set; }
            public double precioUnitario { get; set; }
            public double precioTotal { get; set; }
        }      
    }
}
