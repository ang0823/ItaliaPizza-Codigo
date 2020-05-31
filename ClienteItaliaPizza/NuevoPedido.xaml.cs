using System;
using System.Collections.Generic;
using ClienteItaliaPizza.Servicio;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.ServiceModel;

namespace ClienteItaliaPizza
{
    /// <summary>
    /// Lógica de interacción para PedidoADomicilio.xaml
    /// </summary>
    public partial class NuevoPedido : Window, IRegistrarPedidoLocalCallback
    {      
        private List<Producto> productosDisponibles = new List<Producto>();
        private List<ProvisionDirecta> provisionesDisponibles = new List<ProvisionDirecta>();
        private List<Estado> estados = new List<Estado>();
        private List<Mesa> Listamesas = new List<Mesa>();
        private Empleado empleado = new Empleado();
        public NuevoPedido()
        {
            InitializeComponent();

            InstanceContext instanceContext = new InstanceContext(this);
            RegistrarPedidoLocalClient cliente = new RegistrarPedidoLocalClient(instanceContext);

            cliente.ObtenerInformacionDeProductosYEstados();
        }
        public string GenerarIdPedidoLocal(int numeroMesa)
        {
            string id;
            TimeSpan horaRealizacionDePedido = DateTime.Now.TimeOfDay;
            id = "PL-" + numeroMesa + horaRealizacionDePedido;

            return id;
        }

        private void TextBoxInstruccionesEspeciales_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {

        }

        private void ButtonAceptar_Click(object sender, RoutedEventArgs e)
        {
            InstanceContext context = new InstanceContext(this);
            RegistrarPedidoLocalClient pedidoLocalClient = new RegistrarPedidoLocalClient(context);

            var mesaSeleccionada = comboBoxNoMesa.SelectedItem.ToString();
            int numeroMesa = FuncionesComunes.ParsearAEntero(mesaSeleccionada);

            var numEmpleado = comboBoxNumEmpleado.SelectedItem.ToString();
            var numeroEmpleado = FuncionesComunes.ParsearAEntero(numEmpleado);
           
            Mesa m = Listamesas.Find(x => x.numeroMesa == numeroMesa);

            PedidoLocal pedidoLocalNuevo = new PedidoLocal();
            pedidoLocalNuevo.fecha = DateTime.Now;
            pedidoLocalNuevo.instruccionesEspeciales = textBoxInstruccionesEspeciales.Text;
            pedidoLocalNuevo.MesaId = m.Id;
            pedidoLocalNuevo.Producto = new Producto[1];
            productosDisponibles.CopyTo(pedidoLocalNuevo.Producto);
            pedidoLocalNuevo.ProvisionDirecta = new ProvisionDirecta[1];
            provisionesDisponibles.CopyTo(pedidoLocalNuevo.ProvisionDirecta);

            Cuenta c = new Cuenta();
            c.precioTotal = 50;
            c.subTotal = 50;
            c.descuento = 50;
            c.iva = 50;
            c.Id = GenerarIdPedidoLocal(1);

            pedidoLocalClient.RegistrarPedidoLocal(pedidoLocalNuevo, c, 1, numeroEmpleado);

        }

        private void ButtonCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void TextBoxDescuento_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {

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
            foreach(ProductoDePedido productoDePedido in productos)
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
                ListBox1Productos.Items.Add(productoDePedido.id);
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
                ListBox1Productos.Items.Add(pr.nombre);
            }
        }

        public void mostrarMesas(MesaLocal[] mesas)
        {
            foreach(MesaLocal numMesa in mesas)
            {
                Mesa mesa = new Mesa();
                mesa.Id = numMesa.idMesa;
                mesa.numeroMesa = numMesa.numeroMesa;

                Listamesas.Add(mesa);
                comboBoxNoMesa.Items.Add(numMesa.numeroMesa);
            }
        }

        private void SeleccionaUnProducto(object sender, SelectionChangedEventArgs e)
        {
            var ProductoSeleccionado = ListBox1Productos.SelectedItem.ToString();
            int idProducto = FuncionesComunes.ParsearAEntero(ProductoSeleccionado);
        }

        private void ComboBoxNumEmpleado_Loaded(object sender, RoutedEventArgs e)
        {
            comboBoxNumEmpleado.Items.Add("1");
        }
    }
}
