using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using AccesoBD2;

namespace ServidrorPizzaItaliana
{
    [ServiceContract(CallbackContract = typeof(IEditarRecetaCallback))]
    interface IEditarReceta
    {
        [OperationContract(IsOneWay = true)]
        void EditarReceta(Receta receta, AccesoBD2.Producto producto, Categoria categoria, List<Ingrediente> ingredinetes);

    }

    [ServiceContract]
    public interface IEditarRecetaCallback
    {

        [OperationContract(IsOneWay = true)]
        void RespuestaER(string mensaje);
    }
}
