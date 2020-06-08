using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using AccesoBD2;

namespace ServidrorPizzaItaliana
{
    [ServiceContract(CallbackContract = typeof(IConsultarInventarioCallback))]
    interface IConsultarInventario
    {
        [OperationContract(IsOneWay = true)]
        void ConsultarInventario();
    }

    [ServiceContract]
    public interface IConsultarInventarioCallback
    {
        [OperationContract(IsOneWay = true)]
        void DevuelveInventario(List<Provision> provisiones);

        [OperationContract(IsOneWay = true)]
        void RespuestaInventario(string mensaje);
    }
}
