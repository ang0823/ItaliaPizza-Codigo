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
                this.Id = id;
                this.Nombre = nombre;
                this.ApellidoPaterno = apellidoPaterno;
                this.ApellidoMaterno = apellidoMaterno;
                this.Direcciones = direcciones;
                this.Telefonos = telefonos;
            }

            public int Id { get => id; set => id = value; }
            public string Nombre { get => nombre; set => nombre = value; }
            public string ApellidoPaterno { get => apellidoPaterno; set => apellidoPaterno = value; }
            public string ApellidoMaterno { get => apellidoMaterno; set => apellidoMaterno = value; }
            public List<DireccionCliente> Direcciones { get => direcciones; set => direcciones = value; }
            public List<TelefonoCliente> Telefonos { get => telefonos; set => telefonos = value; }
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
        private bool activado;

        public ProductoDePedido(int id, string nombre, string descrpcion, double precioUnitario, string restricciones, string categoria, bool activado)
        {
            this.id = id;
            this.nombre = nombre;
            this.descrpcion = descrpcion;
            this.precioUnitario = precioUnitario;
            this.restricciones = restricciones;
            this.categoria = categoria;
            this.activado = activado;
        }
    }


    [DataContract]
    public class ProvisionVentaDirecta
    {

        [DataMember]
        private int idProvision;
        [DataMember]
        private int idProvisionVentaDirecta;
        [DataMember]
        private string nombre;
        [DataMember]
        private double precioUnitario;
        [DataMember]
        private string descripcion;
        [DataMember]
        private string restricciones;
        [DataMember]
        private int cantidadExistencias;
        [DataMember]
        private string ubicacion;
        [DataMember]
        private int stock;
        [DataMember]
        private string unidadDeMedida;

        public ProvisionVentaDirecta(int idProvision, int idProvisionVentaDirecta, string nombre, double precioUnitario, string descripcion, string restricciones, int cantidadExistencias, string ubicacion, int stock, string unidadDeMedida)
        {
            this.idProvision = idProvision;
            this.idProvisionVentaDirecta = idProvisionVentaDirecta;
            this.nombre = nombre;
            this.precioUnitario = precioUnitario;
            this.descripcion = descripcion;
            this.restricciones = restricciones;
            this.cantidadExistencias = cantidadExistencias;
            this.ubicacion = ubicacion;
            this.stock = stock;
            this.unidadDeMedida = unidadDeMedida;
        }
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

            public DireccionCliente(string calle, string colonia, string numeroExterior, string numeroInterior)
            {
                this.Calle = calle;
                this.Colonia = colonia;
                this.NumeroExterior = numeroExterior;
                this.NumeroInterior = numeroInterior;
            }

            public string Calle { get => calle; set => calle = value; }
            public string Colonia { get => colonia; set => colonia = value; }
            public string NumeroExterior { get => numeroExterior; set => numeroExterior = value; }
            public string NumeroInterior { get => numeroInterior; set => numeroInterior = value; }
        }

        [DataContract]
        public class TelefonoCliente
        {
            [DataMember]
            private string telefono;

            public TelefonoCliente(string telefono)
            {
                this.Telefono = telefono;
            }

            public string Telefono { get => telefono; set => telefono = value; }
        }

        [DataContract]
        public class EstadoDePedido
        {
            [DataMember]
            private int idEstado;
            [DataMember]
            private string estado;

            public EstadoDePedido(int idEstado, string estado)
            {
                this.idEstado = idEstado;
                this.estado = estado;
            }
        }
}

