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

namespace ClienteItaliaPizza
{
    /// <summary>
    /// Lógica de interacción para PedidoADomicilio.xaml
    /// </summary>
    public partial class NuevoPedido : Window, IAdministrarPedidosMeserosCallback
    {
        string mesaSeleccionada;
        InstanceContext instanceContext;
        AdministrarPedidosMeserosClient cliente;

        //listas de productos seleccionados para el NUEVO Pedido
        private List<Producto> productosSeleccionados = new List<Producto>();
        private List<ProvisionDirecta> provisionesSeleccionadas = new List<ProvisionDirecta>();

        ObservableCollection<Orden> listaOrdenes = new ObservableCollection<Orden>(); 

        //Constructor para registrar un nuevo Pedido (hacer uno para la edicion de pedido)
        public NuevoPedido(string tipoPedido)
        {
            InitializeComponent();
            dataGridOrden.ItemsSource = listaOrdenes;
            try
            {
                if (tipoPedido.Equals("Local"))
                {
                    UC_NuevoPLocal.Visibility = Visibility.Visible;
                    instanceContext = new InstanceContext(this);
                    cliente = new AdministrarPedidosMeserosClient(instanceContext);
                    cliente.ObtenerProductos();
                }
                if (tipoPedido.Equals("Domicilio"))
                {
                    UC_NuevoDomicilio.Visibility = Visibility.Visible;
                }
            }
            catch (CommunicationException e)
            {
                FuncionesComunes.MostrarMensajeDeError("No se ha podido establecer comunicación con el servidor\n"+e.Data.ToString());
            }
        }

        //Llamará al registrar local, domicilio, editar local y editar domicilio
        private void ButtonAceptar_Click(object sender, RoutedEventArgs e)
        {
            bool resultado = ValidarCamposLlenosPedidoLocal();
            if (resultado == true)
            {
                try
                {
                    RegistrarPedidoLocal();
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

        private void ButtonCancelar_Click(object sender, RoutedEventArgs e)
        {
            // this.Close();         
            ListViewBebidas.UnselectAll();
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

        // for this code image needs to be a project resource
        private BitmapImage LoadImage(string filename)
        {
            return new BitmapImage(new Uri(filename));
        }

        public string GenerarIdPedidoLocal(int numeroMesa)
        {
            string id;
            TimeSpan horaRealizacionDePedido = DateTime.Now.TimeOfDay;
            id = "PL-" + numeroMesa + horaRealizacionDePedido;
            return id;
        }

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

        public bool ValidarCamposLlenosPedidoDomicilio()
        {
            if (UC_NuevoDomicilio.comboBoxClienteNombre.SelectedIndex != -1 &&
                UC_NuevoDomicilio.comboBoxDireccion.SelectedIndex != -1 &&
                UC_NuevoDomicilio.textBoxTelefono.Text != null &&
                UC_NuevoDomicilio.comboBoxDireccion.SelectedIndex != -1 &&
                textBoxDescuento.Text != null && listaOrdenes.Count != 0)
            {
                return true;
            }
            return false;
        }

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

        public void RegistrarPedidoLocal()
        {
            try
            {
                int numeroMesa = FuncionesComunes.ParsearAEntero(mesaSeleccionada);          
                var numeroEmpleado = FuncionesComunes.ParsearAEntero(UC_NuevoPLocal.EditarSeleccionComboBoxNumEmpleado);

                textBoxDescuento.Text = "." + textBoxDescuento.Text;
                var descuento = FuncionesComunes.ParsearADouble(textBoxDescuento.Text);
                // Mesa mesa = Listamesas.Find(x => x.numeroMesa == numeroMesa);

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
                        IdEmpleado = numeroEmpleado
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

                cliente.RegistrarPedidoLocal(pedidoLocalNuevo);
            }
            catch (CommunicationException)
            {
                FuncionesComunes.MostrarMensajeDeError("No se ha podido establecer comunicación con el servidor");
            }
        }

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
        /// Obtiene el producto seleccionado de los ListView que exponen productos y los agrega a la lista de ordenes del pedido
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

        private void TextBoxDescuento_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (Validador.validarSoloNumeros(e.Text) == false)
                e.Handled = true;
        }

        /**
         * Métodos Correspondientes al UserControl Local
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
            List<String> lista = new List<string>();
            lista.Add("1");
            foreach (string empleado in lista)
            {
                UC_NuevoPLocal.EditarSeleccionComboBoxNumEmpleado = empleado;
            }
        }

        private void UC_NuevoPLocal_eventSeleccionarNoMesa(object sender, EventArgs e)
        {
            mesaSeleccionada = UC_NuevoPLocal.EditarSeleccionComboBoxNoMesa.ToString();
        }

        private void UC_NuevoPLocal_eventSeleccionarNumEmpleado(object sender, EventArgs e)
        {
            var opcionSeleccionada = UC_NuevoPLocal.EditarSeleccionComboBoxNumEmpleado.ToString();
        }

        /**
         * Métodos Correspondientes al UserControl de Nuevo Pedido A Domicilio 
         */
        private void UC_NuevoDomicilio_eventLlenarComboBoxClienteNombre(object sender, EventArgs e)
        {
            List<String> lista = new List<string>();
            lista.Add("fulano");
            lista.Add("sutano");
            foreach (string nombre in lista)
            {
                UC_NuevoDomicilio.EditarComboBoxClienteNombre = nombre;
            }
        }

        private void UC_NuevoDomicilio_eventSeleccionarCliente(object sender, EventArgs e)
        {
            var opcionSeleccionada = UC_NuevoDomicilio.EditarComboBoxClienteNombre.ToString();
        }

        private void UC_NuevoDomicilio_eventLlenarComboBoxDireccion(object sender, EventArgs e)
        {
            List<String> lista = new List<string>();
            lista.Add("av.juan chochito #2");
            lista.Add("av.njksnd #1");
            foreach (string direccion in lista)
            {
                UC_NuevoDomicilio.EditarComboBoxDireccion = direccion;
            }
        }

        private void UC_NuevoDomicilio_eventSeleccionarDirección(object sender, EventArgs e)
        {
            var opcionSeleccionada = UC_NuevoDomicilio.EditarComboBoxDireccion.ToString();
        }

        private void UC_NuevoDomicilio_eventEditarTelefono(object sender, EventArgs e)
        {
            var NumeroTelefono = UC_NuevoDomicilio.EditartextBoxTelefono;
        }
       

        //esta será una clase para probar el datagrid de ordenes
        public class Orden
        {
            public int cantidad { get; set; }
            public string nombreProducto { get; set; }
            public double precioUnitario { get; set; }
            public double precioTotal { get; set; }
        }       
    }
}
