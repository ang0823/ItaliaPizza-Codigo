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
    /// Lógica de interacción para Inventario.xaml
    /// </summary>
    public partial class Inventario : Window
    {
        CuentaUsuario cuenta = new CuentaUsuario(); //creo esta clase temporalmente para conectar las ventanas con la principal
        public Inventario(CuentaUsuario cuentaUsuario)
        {
            InitializeComponent();
            cuenta = cuentaUsuario;
        }

        private void ButtonAceptar_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonCancelar_Click(object sender, RoutedEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                Principal ventana = new Principal(cuenta);
                ventana.Show();
                this.Close();
            });
        }
    }
}
