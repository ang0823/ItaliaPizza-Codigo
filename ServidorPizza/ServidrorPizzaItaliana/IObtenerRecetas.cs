using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace ServidrorPizzaItaliana
{
    [ServiceContract(CallbackContract = typeof(IObtenerRecetasCallback))]
    interface IObtenerRecetas
    {
        [OperationContract(IsOneWay = true)]
        void ObtenerRecetas();
    }

    [ServiceContract]
    public interface IObtenerRecetasCallback
    {
        [OperationContract(IsOneWay = true)]
        void DevuelveRecetas(List<Receta1> receta, List<Ingrediente1> ingredientes);

        [OperationContract(IsOneWay = true)]
        void RespuestaIOR(string mensaje);
    }

    [DataContract]
    public class Receta1
    {
        [DataMember]
        double porciones;
        [DataMember]
        string procedimiento;
        [DataMember]
        string nombreReceta;
        [DataMember]
        int id;

        public Receta1(int id,double porciones, string procedimiento,string nombreReceta)
        {
            this.id = id;
            this.porciones = porciones;
            this.procedimiento = procedimiento;
            this.nombreReceta = nombreReceta;
        }
    }

    [DataContract]
    public class Ingrediente1
    {
        [DataMember]
        int id;
        [DataMember]
        string nombre;
        [DataMember]
        int cantidad;
        [DataMember]
        string peso;
        [DataMember]
        double costoPorUnidad;
        [DataMember]
        string unidad;


        public Ingrediente1(int id, string nombre, int cantidad, string peso, double costoPorUnidad, string unidad)
        {
            this.id = id;
            this.nombre = nombre;
            this.cantidad = cantidad;
            this.peso = peso;
            this.costoPorUnidad = costoPorUnidad;
            this.unidad = unidad;
        }
    }


}