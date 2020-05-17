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
    /// Lógica de interacción para BuscarIngrediente.xaml
    /// </summary>
    public partial class BuscarIngrediente : Window
    {
        CuentaUsuario CuentaUsuario;

        public BuscarIngrediente(CuentaUsuario cuenta)
        {
            this.CuentaUsuario = cuenta;
            UserLbl.Content = cuenta.nombreUsuario;
            InitializeComponent();
        }

        private void LogoutBtn_Click(object sender, RoutedEventArgs e)
        {
            FuncionesComunes.CerrarSesion();
        }

        private void SearchBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (SearchBox.Text.Length > 0 && e.Key == Key.Return)
            {
                UserLbl.Content = "Button pushed";
            }
        }
    }
}
