using ClienteItaliaPizza.Servicio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClienteItaliaPizza
{
    public static class FuncionesComunes
    {
        public static void CerrarSesion()
        {
            MainWindow ventana = new MainWindow();
            ventana.Show();
        }

        public static void MostrarVentanaPrincipal(CuentaUsuario CuentaUsuario)
        {
                Principal ventana = new Principal(CuentaUsuario);
                ventana.Show();
        }
    }
}
