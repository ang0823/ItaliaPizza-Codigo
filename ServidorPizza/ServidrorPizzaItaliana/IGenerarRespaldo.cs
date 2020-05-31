using AccesoBD2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ServidrorPizzaItaliana
{
    [ServiceContract(CallbackContract = typeof(IGenerarRespaldoCallback))]
    public interface IGenerarRespaldo
    {
        [OperationContract(IsOneWay = true)]
        void GenerarRespaldoAutomatico(string nombreArchivo);
    }

    [ServiceContract]
    public interface IGenerarRespaldoCallback
    {
        [OperationContract(IsOneWay = true)]
        void RespuestaGR(string mensaje);
    }
}

