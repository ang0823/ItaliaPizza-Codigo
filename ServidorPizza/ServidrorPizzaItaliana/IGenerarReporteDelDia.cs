using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace ServidrorPizzaItaliana
{
    [ServiceContract(CallbackContract = typeof(IGenerarReporteDelDiaCallback))]
    interface IGenerarReporteDelDia
    {
        [OperationContract(IsOneWay = true)]
        void ObtenerReporteDelDia();
    }

    [ServiceContract]
    public interface IGenerarReporteDelDiaCallback
    {
        [OperationContract(IsOneWay = true)]
        void DevuelveReporte(List<Reporte> reportes);

        [OperationContract(IsOneWay = true)]
        void RespuestaReporteDelDia(string mensaje);
    }

    [DataContract]
    public class Reporte
    {
        [DataMember]
        int IdPedido;
        [DataMember]
        DateTime fecha;
        [DataMember]
        double totalCuenta;


        public Reporte(int IdPedido, DateTime fecha, double totalCuenta)
        {
            this.IdPedido = IdPedido;
            this.fecha = fecha;
            this.totalCuenta = totalCuenta;
        }
    }
}
