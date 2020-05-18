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
    /// Lógica de interacción para BuscarEmpleados.xaml
    /// </summary>
    public partial class BuscarEmpleados : Window
    {
        CuentaUsuario CuentaUsuario;
        public BuscarEmpleados()
        {
            InitializeComponent();

            //CuentaUsuario = cuenta;
            //UsuarioLbl.Content = cuenta.nombreUsuario;
            EditarGuardarBtn.IsEnabled = false;
            EliminarBtn.IsEnabled = false;
        }

        private Boolean CamposLlenos()
        {
            if (nombreTxt.Text.Length > 0 && aPaternoTxt.Text.Length > 0 && aMaternoTxt.Text.Length > 0
                && correoElectronicoTxt.Text.Length > 0 && telefonoTxt.Text.Length > 0
                && calleTxt.Text.Length > 0 && coloniaTxt.Text.Length > 0
                && codigoPostalTxt.Text.Length > 0 && (puestosCB.SelectedIndex == 1
                || puestosCB.SelectedIndex == 2) && usuarioTxt.Text.Length == 0
                && contrasenaTxt.Password.Length == 0)
            {
                return true;
            }
            else if (nombreTxt.Text.Length > 0 && aPaternoTxt.Text.Length > 0 && aMaternoTxt.Text.Length > 0
                && correoElectronicoTxt.Text.Length > 0 && telefonoTxt.Text.Length > 0
                && calleTxt.Text.Length > 0 && coloniaTxt.Text.Length > 0
                && codigoPostalTxt.Text.Length > 0 && (puestosCB.SelectedIndex == 3
                || puestosCB.SelectedIndex == 4 || puestosCB.SelectedIndex == 5)
                && usuarioTxt.Text.Length > 0 && contrasenaTxt.Password.Length > 0)
            {
                return true;
            }

            return false;
        }

        private void SearchBtn_Click(object sender, RoutedEventArgs e)
        {
            // Falta el servidor para implementar este metodo
            SearchBox.Text = "";
            Empleado n = new Servicio.Empleado();
            CuentaUsuario c = new Servicio.CuentaUsuario();
        }

        private void LogoutBtn_Click(object sender, RoutedEventArgs e)
        {
            FuncionesComunes.CerrarSesion();
        }

        private void entradaTxt_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (CamposLlenos())
            {
                EditarGuardarBtn.IsEnabled = true;
            }
            else
            {
                EditarGuardarBtn.IsEnabled = false;
            }
        }

        private void cancelarBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult opcion;

            opcion = MessageBox.Show("¿Volver a pantalla anteior?", "Cancelar",
                    MessageBoxButton.OKCancel, MessageBoxImage.Question);

                if (opcion == MessageBoxResult.OK)
                {
                    FuncionesComunes.MostrarVentanaPrincipal(this.CuentaUsuario);
                }
        }

        private void cancelarBtn_Click_1(object sender, RoutedEventArgs e)
        {
            FuncionesComunes.MostrarVentanaPrincipal(this.CuentaUsuario);
            this.Close();
        }

        private void LogoutBtn_Click_1(object sender, RoutedEventArgs e)
        {
            FuncionesComunes.CerrarSesion();
            this.Close();
        }
    }
}
