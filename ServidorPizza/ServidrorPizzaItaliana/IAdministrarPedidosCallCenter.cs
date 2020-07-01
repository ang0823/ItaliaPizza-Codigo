using AccesoBD2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ServidrorPizzaItaliana
{
    [ServiceContract(CallbackContract = typeof(IAdministrarPedidosCallCenterCallback))]
    public interface IAdministrarPedidosCallCenter
    {
        [OperationContract(IsOneWay = true)]
        void ObtenerDatos();

        [OperationContract(IsOneWay = true)]
        void RegistrarPedidoADomicilio(PedidoADomicilio pedido);

        [OperationContract(IsOneWay = true)]
        void RegistrarPedidoLocalCallCenter(PedidoLocal pedido);

        [OperationContract(IsOneWay = true)]
        void RegistrarCliente(AccesoBD2.Cliente cliente, Direccion direccionCliente, Telefono telefonoCliente);

        [OperationContract(IsOneWay = true)]
        void ModificarPedidoADomicilio(PedidoADomicilio pedido);

        [OperationContract(IsOneWay = true)]
        void ModificarPedidoLocalCallCenter(PedidoLocal pedido);

    }

    [ServiceContract]
    public interface IAdministrarPedidosCallCenterCallback
    {
        [OperationContract(IsOneWay = true)]
        void Datos(List<Cliente> clientes, List<ProductoDePedido> productos, List<ProvisionVentaDirecta> provisiones);

        [OperationContract(IsOneWay = true)]
        void Mensaje(string mensaje);

        [OperationContract(IsOneWay = true)]
        void NotificacionClienteDePedido(string mensaje, Cliente cliente);
    }

       [DataContract]
        public class Cliente
        {
            [DataMember]
            private int id;
            [DataMember]
            private string nombre;
            [DataMember]
            private string apellidoPaterno;
            [DataMember]
            private string apellidoMaterno;
            [DataMember]
            private List<DireccionCliente> direcciones;
            [DataMember]
            private List<TelefonoCliente> telefonos;

            public Cliente(int id, string nombre, string apellidoPaterno, string apellidoMaterno, List<DireccionCliente> direcciones, List<TelefonoCliente> telefonos)
            {
                this.id = id;
                this.nombre = nombre;
                this.apellidoPaterno = apellidoPaterno;
                this.apellidoMaterno = apellidoMaterno;
                this.direcciones = direcciones;
                this.telefonos = telefonos;
            }
    }

    [DataContract]
    public class ProductoDePedido
    {
        [DataMember]
        private int id;
        [DataMember]
        private string nombre;
        [DataMember]
        private string descrpcion;
        [DataMember]
        private double precioUnitario;
        [DataMember]
        private string restricciones;
        [DataMember]
        private string categoria;
        [DataMember]
        private byte[] imagen;
        [DataMember]
        private string nombreReceta;
        [DataMember]
        private bool activado;

        public ProductoDePedido(int id, string nombre, string descrpcion, double precioUnitario, string restricciones, string categoria)
        {
            this.Id = id;
            this.Nombre = nombre;
            this.Descrpcion = descrpcion;
            this.PrecioUnitario = precioUnitario;
            this.Restricciones = restricciones;
            this.Categoria = categoria;
        }

        public byte[] Imagen { get => imagen; set => imagen = value; }
        public string NombreReceta { get => nombreReceta; set => nombreReceta = value; }
        public bool Activado { get => activado; set => activado = value; }
        public int Id { get => id; set => id = value; }
        public string Nombre { get => nombre; set => nombre = value; }
        public string Descrpcion { get => descrpcion; set => descrpcion = value; }
        public double PrecioUnitario { get => precioUnitario; set => precioUnitario = value; }
        public string Restricciones { get => restricciones; set => restricciones = value; }
        public string Categoria { get => categoria; set => categoria = value; }
    }


    [DataContract]
    public class ProvisionVentaDirecta
    {
        [DataMember]
        private int idProvisionVentaDirecta;
        [DataMember]
        private int idProvision;
        [DataMember]
        private string nombre;
        [DataMember]
        private int cantidadExistencias;
        [DataMember]
        private string ubicacion;
        [DataMember]
        private int stock;
        [DataMember]
        private double precioUnitario;
        [DataMember]
        private string unidadDeMedida;
        [DataMember]
        private bool activado;
        [DataMember]
        private string descripcion;
        [DataMember]
        private string restricciones;
        [DataMember]
        private string categoria;
        [DataMember]
        private byte[] imagen;

        public ProvisionVentaDirecta(int idProvisionVentaDirecta, int idProvision, string nombre, int cantidadExistencias, string ubicacion, int stock, double precioUnitario, string unidadDeMedida, bool activado, string descripcion, string restricciones, string categoria)
        {
            this.IdProvisionVentaDirecta = idProvisionVentaDirecta;
            this.IdProvision = idProvision;
            this.Nombre = nombre;
            this.CantidadExistencias = cantidadExistencias;
            this.Ubicacion = ubicacion;
            this.Stock = stock;
            this.PrecioUnitario = precioUnitario;
            this.UnidadDeMedida = unidadDeMedida;
            this.Activado = activado;
            this.Descripcion = descripcion;
            this.Restricciones = restricciones;
            this.Categoria = categoria;
        }

        public byte[] Imagen { get => imagen; set => imagen = value; }
        public int IdProvisionVentaDirecta { get => idProvisionVentaDirecta; set => idProvisionVentaDirecta = value; }
        public int IdProvision { get => idProvision; set => idProvision = value; }
        public string Nombre { get => nombre; set => nombre = value; }
        public int CantidadExistencias { get => cantidadExistencias; set => cantidadExistencias = value; }
        public string Ubicacion { get => ubicacion; set => ubicacion = value; }
        public int Stock { get => stock; set => stock = value; }
        public double PrecioUnitario { get => precioUnitario; set => precioUnitario = value; }
        public string UnidadDeMedida { get => unidadDeMedida; set => unidadDeMedida = value; }
        public bool Activado { get => activado; set => activado = value; }
        public string Descripcion { get => descripcion; set => descripcion = value; }
        public string Restricciones { get => restricciones; set => restricciones = value; }
        public string Categoria { get => categoria; set => categoria = value; }
    }

    [DataContract]
    public class DireccionCliente
    {
        [DataMember]
        private string calle;
        [DataMember]
        private string colonia;
        [DataMember]
        private string numeroExterior;
        [DataMember]
        private string numeroInterior;
        [DataMember]
        private string codigoPostal;

        public DireccionCliente(string calle, string colonia, string numeroExterior, string numeroInterior, string codigoPostal)
        {
            this.calle = calle;
            this.colonia = colonia;
            this.numeroExterior = numeroExterior;
            this.numeroInterior = numeroInterior;
            this.codigoPostal = codigoPostal;
        }
    }

    [DataContract]
    public class TelefonoCliente
    {
        [DataMember]
        private string telefono;
        
        public TelefonoCliente(string telefono)
        {
            this.telefono = telefono;
        }
    }
}

