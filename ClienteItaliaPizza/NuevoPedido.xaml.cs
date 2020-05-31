using System;
using System.Collections.Generic;
using ClienteItaliaPizza.Servicio;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.ServiceModel;
using ClienteItaliaPizza.Validacion;
using System.Windows.Forms;

namespace ClienteItaliaPizza
{
    /// <summary>
    /// Lógica de interacción para PedidoADomicilio.xaml
    /// </summary>
    public partial class NuevoPedido : Window, IRegistrarPedidoLocalCallback
    {
        InstanceContext context;
        private List<Producto> productosDisponibles = new List<Producto>();
        private List<ProvisionDirecta> provisionesDisponibles = new List<ProvisionDirecta>();
        private List<Mesa> Listamesas = new List<Mesa>();
        string mesaSeleccionada;

        private List<Producto> productosSeleccionados = new List<Producto>();
        private List<ProvisionDirecta> provisionesSeleccionadas = new List<ProvisionDirecta>();

        public NuevoPedido(string tipoPedido)
        {
            InitializeComponent();            

            if (tipoPedido.Equals("Local"))
            {
                UC_NuevoPLocal.Visibility = Visibility.Visible;
                //InstanceContext instanceContext = new InstanceContext(this);
              //  RegistrarPedidoLocalClient cliente = new RegistrarPedidoLocalClient(instanceContext);
               // cliente.ObtenerInformacionDeProductosYEstados();
            }
            if (tipoPedido.Equals("Domicilio"))
            {
                UC_NuevoDomicilio.Visibility = Visibility.Visible;
            }
        }       

        private void ButtonAceptar_Click(object sender, RoutedEventArgs e)
        {
            bool resultado = ValidarCamposLlenosPedidoLocal();
            if (resultado == true)
            {               
                context = new InstanceContext(this);

                //esto irá encapsulado en un método cuando comparemos si es local o a domicilio
                RegistrarPedidoLocalClient pedidoLocalClient = new RegistrarPedidoLocalClient(context);
                int numeroMesa = FuncionesComunes.ParsearAEntero(mesaSeleccionada);
                var seleccionEmpleado = UC_NuevoPLocal.EditarSeleccionComboBoxNumEmpleado;
                var numeroEmpleado = FuncionesComunes.ParsearAEntero(seleccionEmpleado);
                var descuento = FuncionesComunes.ParsearADouble(textBoxDescuento.Text);
                Mesa mesa = Listamesas.Find(x => x.numeroMesa == numeroMesa);

                PedidoLocal pedidoLocalNuevo = new PedidoLocal();
                pedidoLocalNuevo.fecha = DateTime.Now;
                pedidoLocalNuevo.instruccionesEspeciales = textBoxInstruccionesEspeciales.Text;
                pedidoLocalNuevo.MesaId = mesa.Id;
                pedidoLocalNuevo.Producto = new Producto[1];
                productosDisponibles.CopyTo(pedidoLocalNuevo.Producto);
                pedidoLocalNuevo.ProvisionDirecta = new ProvisionDirecta[1];
                provisionesDisponibles.CopyTo(pedidoLocalNuevo.ProvisionDirecta);

                Cuenta c = new Cuenta();
                c.precioTotal = 50;
                c.subTotal = 50;
                c.descuento = descuento;
                c.iva = 50;
                c.Id = GenerarIdPedidoLocal(1);

                pedidoLocalClient.RegistrarPedidoLocal(pedidoLocalNuevo, c, 1, numeroEmpleado);
            }
            else
            {
                System.Windows.MessageBox.Show("No se han ingresado todos los datos");
            }
        }

        private void ButtonCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void TextBoxDescuento_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Validador Validacion = new Validador();
            bool resultadoValidacion = Validacion.validarSoloNumeros(e.Text);
            if (resultadoValidacion == false)
            {
                e.Handled = true;
            }
        }

        public void DatosRecuperados(ProductoDePedido[] productos, ProvisionVentaDirecta[] provisiones, EstadoDePedido[] estados, MesaLocal[] mesas)
        {
           
            mostrarProductos(productos);
            mostrarProvisionesDirectas(provisiones);
            //no entiendo porque recibe los estados de pedido
            mostrarMesas(mesas);

        }

        public void MensajeRegistrarPedidoLocal(string mensaje)
        {
            FuncionesComunes.MostrarMensajeExitoso(mensaje);
        }

        public void mostrarProductos(ProductoDePedido[] productos)
        {
            //temporalmente lo pongo en un listbox
            foreach (ProductoDePedido productoDePedido in productos)
            {
                Producto producto = new Producto();

                producto.Id = productoDePedido.id;
                producto.nombre = productoDePedido.nombre;
                producto.Categoria = new Categoria();
                producto.Categoria.categoria = productoDePedido.categoria;
                producto.precioUnitario = productoDePedido.precioUnitario;
                producto.descripcion = productoDePedido.descrpcion;
                producto.restricciones = productoDePedido.restricciones;

                productosDisponibles.Add(producto);
                ListBoxEnsaladas.Items.Add(productoDePedido.nombre);
            }
        }

        public void mostrarProvisionesDirectas(ProvisionVentaDirecta[] provisiones)
        {
            foreach (ProvisionVentaDirecta pr in provisiones)
            {
                ProvisionDirecta provisionDirecta = new ProvisionDirecta();

                provisionDirecta.Id = pr.idProvisionVentaDirecta;
                provisionDirecta.Provision = new Provision();
                provisionDirecta.Provision.Id = pr.idProvision;
                provisionDirecta.Provision.nombre = pr.nombre;

                provisionesDisponibles.Add(provisionDirecta);
                ListBoxEnsaladas.Items.Add(pr.nombre);
            }
        }

        public void mostrarMesas(MesaLocal[] mesas)
        {
            foreach (MesaLocal numMesa in mesas)
            {
                Mesa mesa = new Mesa();
                mesa.Id = numMesa.idMesa;
                mesa.numeroMesa = numMesa.numeroMesa;

                Listamesas.Add(mesa);

                UC_NuevoPLocal.EditarSeleccionComboBoxNoMesa = mesa.numeroMesa.ToString();
                // comboBoxNoMesa.Items.Add(numMesa.numeroMesa);
            }
        }

        private void SeleccionaUnProducto(object sender, SelectionChangedEventArgs e)
        {
            var ProductoSeleccionado = ListBoxEnsaladas.SelectedItem.ToString();
            var ProductoDelPedido = productosDisponibles.Find(p => p.nombre == ProductoSeleccionado);
            productosSeleccionados.Add(ProductoDelPedido);
            labelIVA.Content = ProductoDelPedido.nombre;
        }


        private void GridBebidas_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            //ImageList listaImagesBebidas = new ImageList();
            ListViewBebidas.ItemsSource = new MovieData[] {
            new MovieData{Title="Movie 1", ImageData=LoadImage("C:/Users/survi/Pictures/Granos Selectos (4).jpg")},
            new MovieData{Title="Movie 2", ImageData=LoadImage("C:/Users/survi/Pictures/Granos Selectos (4).jpg")},
            new MovieData{Title="Movie 3", ImageData=LoadImage("C:/Users/survi/Pictures/Granos Selectos (4).jpg")},
            new MovieData{Title="Movie 4", ImageData=LoadImage("C:/Users/survi/Pictures/Granos Selectos (4).jpg")},
            new MovieData{Title="Movie 5", ImageData=LoadImage("C:/Users/survi/Pictures/Granos Selectos (4).jpg")},
            new MovieData{Title="Movie 6", ImageData=LoadImage("C:/Users/survi/Pictures/Granos Selectos (4).jpg")}
            };          
            /* Esto era para mostrar la imagen del producto
             * 
             * Image image = new Image();            
             image.Height = 66;
             image.Width = 78;
             image.Margin = new Thickness(21, 17, 0, 0);
             image.Stretch = Stretch.Fill;
             image.Name = "imagen";
             image.Opacity = 100;
             Uri uri = new Uri("C:/Users/survi/Pictures/Granos Selectos (4).jpg");
             image.Source = new BitmapImage(uri);

             gridBebidas.Children.Add(image); */
        }

        // for this code image needs to be a project resource
        private BitmapImage LoadImage(string filename)
        {
            return new BitmapImage(new Uri(filename));
        }


        /**
         * Métodos Correspondientes al UserControl Local
         */
        private void UC_NuevoPLocal_eventLlenarNoMesa(object sender, EventArgs e)
        {
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

        public string GenerarIdPedidoLocal(int numeroMesa)
        {
            string id;
            TimeSpan horaRealizacionDePedido = DateTime.Now.TimeOfDay;
            id = "PL-" + numeroMesa + horaRealizacionDePedido;
            return id;
        }

        public bool ValidarCamposLlenosPedidoLocal()
        {
            if (UC_NuevoPLocal.comboBoxNumEmpleado.SelectedIndex != -1 && UC_NuevoPLocal.comboBoxNoMesa.SelectedIndex != -1 && textBoxDescuento.Text != null)
            {          
                return true;
            }
            return false;
        }

        private void ListViewBebidas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selecto = ListViewBebidas.SelectedItem.ToString();
            textBlockInstruccionesEspeciales.Text =  selecto;
        }
    }

    public class MovieData
    {
        private string _Title;
        public string Title
        {
            get { return this._Title; }
            set { this._Title = value; }
        }

        private BitmapImage _ImageData;
        public BitmapImage ImageData
        {
            get { return this._ImageData; }
            set { this._ImageData = value; }
        }
    }
}
