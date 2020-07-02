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
    [ServiceContract(CallbackContract = typeof(IBuscarPedidosCallback))]
    public interface IBuscarPedidos
    {
        [OperationContract(IsOneWay = true)]
        void BuscarPedidos();

    }

    [ServiceContract]
    public interface IBuscarPedidosCallback
    {
        [OperationContract(IsOneWay = true)]
        void Pedidos(List<PedidoADomicilioDeServidor> pedidosADomicilio, List<PedidoLocalDeServidor> pedidosLocales);


        [OperationContract(IsOneWay = true)]
        void MensajeErrorBuscarPedidos(string mensaje);
       

    }

    [DataContract]
    public class PedidoServidor
    {
        [DataMember]
        List<ProductoDePedido> productosLocales;
        [DataMember]
        List<ProvisionVentaDirecta> productosExternos;
        [DataMember]
        CuentaDePedido cuenta;
        [DataMember]
        int id;
        [DataMember]
        string instruccionesDePedido;
        [DataMember]
        DateTime fecha;
        [DataMember]
        long idEmpleado;
        [DataMember]
        string idGeneradoDeEmpleado;
        [DataMember]
        string estado;

        public List<ProductoDePedido> ProductosLocales { get => productosLocales; set => productosLocales = value; }
        public List<ProvisionVentaDirecta> ProductosExternos { get => productosExternos; set => productosExternos = value; }
        public CuentaDePedido Cuenta { get => cuenta; set => cuenta = value; }
        public int Id { get => id; set => id = value; }
        public string InstruccionesDePedido { get => instruccionesDePedido; set => instruccionesDePedido = value; }
        public DateTime Fecha { get => fecha; set => fecha = value; }
        public long IdEmpleado { get => idEmpleado; set => idEmpleado = value; }
        public string IdGeneradoDeEmpleado { get => idGeneradoDeEmpleado; set => idGeneradoDeEmpleado = value; }
        public string Estado { get => estado; set => estado = value; }
    }

    [DataContract]
    public class PedidoADomicilioDeServidor: PedidoServidor
    {
        [DataMember]
        Cliente cliente;

        public PedidoADomicilioDeServidor(Cliente cliente)
        {
            this.cliente = cliente;
            ProductosExternos = new List<ProvisionVentaDirecta>();
            this.ProductosLocales = new List<ProductoDePedido>();
        }
    }

    [DataContract]
    public class PedidoLocalDeServidor : PedidoServidor
    {
        [DataMember]
        int mesaId;
        [DataMember]
        int numeroMesa;

        public PedidoLocalDeServidor(int mesaId, int numeroMesa)
        {
            this.mesaId = mesaId;
            this.numeroMesa = numeroMesa;
            ProductosExternos = new List<ProvisionVentaDirecta>();
            ProductosLocales = new List<ProductoDePedido>();
        }
    }

    [DataContract]
    public class CuentaDePedido
    {
        [DataMember]
        string Id;
        [DataMember]
        double toal;
        [DataMember]
        double subtotal;
        [DataMember]
        double iva;
        [DataMember]
        double descuento;

        public CuentaDePedido(string id, double toal, double subtotal, double iva, double descuento)
        {
            Id = id;
            this.toal = toal;
            this.subtotal = subtotal;
            this.iva = iva;
            this.descuento = descuento;
        }
    }
}
