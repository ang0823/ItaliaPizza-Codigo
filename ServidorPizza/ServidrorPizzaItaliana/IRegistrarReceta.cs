using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using AccesoBD2;

namespace ServidrorPizzaItaliana
{
    [ServiceContract(CallbackContract = typeof(IRegistrarRecetaCallback))]
    public interface IRegistrarReceta
    {
        [OperationContract(IsOneWay = true)]
        void RegistrarReceta(Receta receta, AccesoBD2.Producto producto, Categoria categoria, List<Ingrediente> ingredientes);

    }

    [ServiceContract]
    public interface IRegistrarRecetaCallback
    {

        [OperationContract(IsOneWay = true)]
        void RespuestaRR(string mensaje);
    }
}
