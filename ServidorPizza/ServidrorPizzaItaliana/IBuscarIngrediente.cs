using AccesoBD2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ServidrorPizzaItaliana
{

    [ServiceContract(CallbackContract = typeof(IBuscarIngredienteCallback))]
    public interface IBuscarIngrediente
    {
        [OperationContract(IsOneWay = true)]
        void BuscarIngredientePorNombre(string nombreProducto);

        [OperationContract(IsOneWay = true)]
        void BuscarIngredientePorID(int idProducto);
    }

    [ServiceContract]
    public interface IBuscarIngredienteCallback
    {
        [OperationContract(IsOneWay = true)]
        void Ingrediente(Provision prov);

        [OperationContract(IsOneWay = true)]
        void ErrorAlRecuperarIngrediente(string mensajeError);

    }
}
