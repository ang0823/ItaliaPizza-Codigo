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
    [ServiceContract(CallbackContract = typeof(IRecuperarProductosCallback))]
    public interface IRecuperarProductos
    {
        [OperationContract(IsOneWay = true)]
        void RecuperarProductos();
    }

    [ServiceContract]
    public interface IRecuperarProductosCallback
    {
        [OperationContract(IsOneWay = true)]
        void Productos(List<Producto> productos);

        [OperationContract(IsOneWay = true)]
        void ErrorAlRecuperarProductos(string mensajeError);
    }

    [DataContract]
    public class Producto
    {
        [DataMember]
        private int id;
        [DataMember]
        private string nombreProducto;
        [DataMember]
        private int cantidadDisponible;
        [DataMember]
        private string ubicacion;
        [DataMember]
        private string unidadDeMedida;

        public Producto(int id, string nombreProducto, int cantidadDisponible, string ubicacion, string unidadDeMedida)
        {
            this.id = id;
            this.nombreProducto = nombreProducto;
            this.cantidadDisponible = cantidadDisponible;
            this.unidadDeMedida = unidadDeMedida;
            this.ubicacion = ubicacion;
        }
    }
}
