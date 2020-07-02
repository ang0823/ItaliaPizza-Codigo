using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using AccesoBD2;


namespace ServidrorPizzaItaliana
{
    [ServiceContract(CallbackContract = typeof(IRegistrarProductoCallback))]
    interface IRegistrarProducto
    {
        [OperationContract(IsOneWay = true)]
        void RegistrarProducto(AccesoBD2.Producto producto, Categoria categoria, int receta);

    }

    [ServiceContract]
    public interface IRegistrarProductoCallback
    {
        [OperationContract(IsOneWay = true)]
        void RespuestaRP(string mensaje);
    }
}
