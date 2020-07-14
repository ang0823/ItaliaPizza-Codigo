using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClienteItaliaPizza
{
    public class ReporteDelDia1
    {
        string IdPedido;
        string fecha;
        string totalCuenta;
        string nombreEmpleado;

        public string IdPedido1 { get => IdPedido; set => IdPedido = value; }
        public string Fecha { get => fecha; set => fecha = value; }
        public string TotalCuenta { get => totalCuenta; set => totalCuenta = value; }
        public string NombreEmpleado { get => nombreEmpleado; set => nombreEmpleado = value; }
    }
}
