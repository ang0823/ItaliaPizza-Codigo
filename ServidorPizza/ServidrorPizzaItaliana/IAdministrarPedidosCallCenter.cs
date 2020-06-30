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

        public ProductoDePedido(int id, string nombre, string descrpcion, double precioUnitario, string restricciones, string categoria)
        {
            this.id = id;
            this.nombre = nombre;
            this.descrpcion = descrpcion;
            this.precioUnitario = precioUnitario;
            this.restricciones = restricciones;
            this.categoria = categoria;
        }

        public byte[] Imagen { get => imagen; set => imagen = value; }
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
            this.idProvisionVentaDirecta = idProvisionVentaDirecta;
            this.idProvision = idProvision;
            this.nombre = nombre;
            this.cantidadExistencias = cantidadExistencias;
            this.ubicacion = ubicacion;
            this.stock = stock;
            this.precioUnitario = precioUnitario;
            this.unidadDeMedida = unidadDeMedida;
            this.activado = activado;
            this.descripcion = descripcion;
            this.restricciones = restricciones;
            this.categoria = categoria;
        }

        public byte[] Imagen { get => imagen; set => imagen = value; }
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

