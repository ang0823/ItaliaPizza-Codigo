using ClienteItaliaPizza.Servicio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ClienteItaliaPizza
{
    /// <summary>
    /// Lógica de interacción para BuscarReceta.xaml
    /// </summary>
    public partial class BuscarReceta : Window
    {
        CuentaUsuario1 cuenta = new CuentaUsuario1(); 
        public BuscarReceta(CuentaUsuario1 cuentaUsuario)
        {
            InitializeComponent();
            cuenta = cuentaUsuario;
            //telerik.Windows.Controls.dll;
            //aquí llamaré al servicio para obtener todas las recetas y mostrarlas en el datagrid
        }

        private void ButtonRegresar_Click(object sender, RoutedEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                Principal ventana = new Principal(cuenta);
                ventana.Show();
                this.Close();
            });
        }

        private void ButtonEditar_Click(object sender, RoutedEventArgs e)
        {
            object receta = new object();

            Receta ventanaEditarReceta = new Receta(cuenta, receta);
            ventanaEditarReceta.Show();
            this.Hide();
        }

        private void ButtonEliminar_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
