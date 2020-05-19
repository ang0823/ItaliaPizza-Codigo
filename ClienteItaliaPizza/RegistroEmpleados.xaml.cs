using ClienteItaliaPizza.Servicio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Security.Tokens;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Lógica de interacción para RegistroEmpleados.xaml
    /// </summary>
    public partial class RegistroEmpleados : Window
    {
        DateTime FechaActual = DateTime.Now;
        Random NumeroGenerado = new Random();
        CuentaUsuario CuentaUsuario;
        string DiaHora;

        public RegistroEmpleados(/*CuentaUsuario cuenta*/)
        {
            // CuentaUsuario = cuenta;
            // UsuarioLbl.Content = cuenta.nombreUsuario;
            InitializeComponent();
            LlenarPuestosCb();
            idEmpleadoTxt.Text = GenerarIdEmpleado();

            idEmpleadoTxt.IsEnabled = false;
            limpiarBtn.IsEnabled = false;
            guardarBtn.IsEnabled = false;

            usuarioLbl.Visibility = Visibility.Hidden;
            usuarioTxt.Visibility = Visibility.Hidden;
            contrasenaLbl.Visibility = Visibility.Hidden;
            contrasenaTxt.Visibility = Visibility.Hidden;
        }

        private Boolean AlgunCampoLleno()
        {
            if (nombreTxt.Text.Length > 0 || aPaternoTxt.Text.Length > 0 || aMaternoTxt.Text.Length > 0
                || correoElectronicoTxt.Text.Length > 0 || telefonoTxt.Text.Length > 0
                || calleTxt.Text.Length > 0 || coloniaTxt.Text.Length > 0
                || codigoPostalTxt.Text.Length > 0 || puestosCB.SelectedIndex != 0
                || usuarioTxt.Text.Length > 0 || contrasenaTxt.Password.Length > 0)
            {
                return true;
            }

            return false;
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

        private String GenerarIdEmpleado() 
        {
            FechaActual = DateTime.Now;
            DiaHora = FechaActual.Day.ToString() + FechaActual.Hour.ToString();

            return DiaHora + NumeroGenerado.Next(10, 99).ToString();
        }

        private void LlenarPuestosCb()
        {
            puestosCB.Items.Insert(0, "Seleccionar rol");
            puestosCB.Items.Insert(1, "Mesero");
            puestosCB.Items.Insert(2, "Cocinero");
            puestosCB.Items.Insert(3, "Call center");
            puestosCB.Items.Insert(4, "Contador");
            puestosCB.Items.Insert(5, "Gerente");
            puestosCB.SelectedIndex = 0;
        }

        private void VaciarCampos()
        {
            puestosCB.SelectedIndex = 0;
            nombreTxt.Text = "";
            aPaternoTxt.Text = "";
            aMaternoTxt.Text = "";
            correoElectronicoTxt.Text = "";
            telefonoTxt.Text = "";
            calleTxt.Text = "";
            coloniaTxt.Text = "";
            codigoPostalTxt.Text = "";
            usuarioTxt.Text = "";
            contrasenaTxt.Password = "";
        }

        private void nombreTxT_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (CamposLlenos())
            {
                guardarBtn.IsEnabled = true;
            }
            else
            {
                guardarBtn.IsEnabled = false;
            }

            if (AlgunCampoLleno())
            {
                limpiarBtn.IsEnabled = true;
            }
            else
            {
                limpiarBtn.IsEnabled = false;
            }
        }

        private void aPaternoTxt_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (CamposLlenos())
            {
                guardarBtn.IsEnabled = true;
            }
            else
            {
                guardarBtn.IsEnabled = false;
            }

            if (AlgunCampoLleno())
            {
                limpiarBtn.IsEnabled = true;
            }
            else
            {
                limpiarBtn.IsEnabled = false;
            }
        }

        private void aMaternoTxt_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (CamposLlenos())
            {
                guardarBtn.IsEnabled = true;
            }
            else
            {
                guardarBtn.IsEnabled = false;
            }

            if (AlgunCampoLleno())
            {
                limpiarBtn.IsEnabled = true;
            }
            else
            {
                limpiarBtn.IsEnabled = false;
            }
        }

        private void calleTxt_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (CamposLlenos())
            {
                guardarBtn.IsEnabled = true;
            }
            else
            {
                guardarBtn.IsEnabled = false;
            }

            if (AlgunCampoLleno())
            {
                limpiarBtn.IsEnabled = true;
            }
            else
            {
                limpiarBtn.IsEnabled = false;
            }
        }

        private void coloniaTxt_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (CamposLlenos())
            {
                guardarBtn.IsEnabled = true;
            }
            else
            {
                guardarBtn.IsEnabled = false;
            }

            if (AlgunCampoLleno())
            {
                limpiarBtn.IsEnabled = true;
            }
            else
            {
                limpiarBtn.IsEnabled = false;
            }
        }

        private void codigoPostalTxt_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (CamposLlenos())
            {
                guardarBtn.IsEnabled = true;
            }
            else
            {
                guardarBtn.IsEnabled = false;
            }

            if (AlgunCampoLleno())
            {
                limpiarBtn.IsEnabled = true;
            }
            else
            {
                limpiarBtn.IsEnabled = false;
            }
        }

        private void correoElectronicoTxt_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (CamposLlenos())
            {
                guardarBtn.IsEnabled = true;
            }
            else
            {
                guardarBtn.IsEnabled = false;
            }

            if (AlgunCampoLleno())
            {
                limpiarBtn.IsEnabled = true;
            }
            else
            {
                limpiarBtn.IsEnabled = false;
            }
        }

        private void telefonoTxt_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (CamposLlenos())
            {
                guardarBtn.IsEnabled = true;
            }
            else
            {
                guardarBtn.IsEnabled = false;
            }

            if (AlgunCampoLleno())
            {
                limpiarBtn.IsEnabled = true;
            }
            else
            {
                limpiarBtn.IsEnabled = false;
            }
        }

        private void usuarioTxt_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (CamposLlenos())
            {
                guardarBtn.IsEnabled = true;
            }
            else
            {
                guardarBtn.IsEnabled = false;
            }

            if (AlgunCampoLleno())
            {
                limpiarBtn.IsEnabled = true;
            }
            else
            {
                limpiarBtn.IsEnabled = false;
            }
        }

        private void contrasenaTxt_TextChanged(object sender, RoutedEventArgs e)
        {
            if (CamposLlenos())
            {
                guardarBtn.IsEnabled = true;
            }
            else
            {
                guardarBtn.IsEnabled = false;
            }

            if (AlgunCampoLleno())
            {
                limpiarBtn.IsEnabled = true;
            }
            else
            {
                limpiarBtn.IsEnabled = false;
            }
        }

        private void limpiarBtn_Click(object sender, RoutedEventArgs e)
        {
            VaciarCampos();
        }

        private void guardarBtn_Click(object sender, RoutedEventArgs e)
        {
            idEmpleadoTxt.Text = GenerarIdEmpleado();
        }

        private void cancelarBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult opcion;

            if (AlgunCampoLleno())
            {
                opcion = MessageBox.Show("¿Seguro que deseea descartar los cambios?", "Descartar cambios",
                    MessageBoxButton.OKCancel, MessageBoxImage.Question);

                if (opcion == MessageBoxResult.OK)
                {
                    VaciarCampos();
                    FuncionesComunes.MostrarVentanaPrincipal(this.CuentaUsuario);
                    this.Close();
                }
            }
            else
            {
                FuncionesComunes.MostrarVentanaPrincipal(this.CuentaUsuario);
                this.Close();
            }
        }

        private void puestosCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CamposLlenos())
            {
                guardarBtn.IsEnabled = true;
            }
            else
            {
                guardarBtn.IsEnabled = false;
            }

            if (puestosCB.SelectedIndex == 3 || puestosCB.SelectedIndex == 4
                || puestosCB.SelectedIndex == 5)
            {
                usuarioLbl.Visibility = Visibility.Visible;
                usuarioTxt.Visibility = Visibility.Visible;
                contrasenaLbl.Visibility = Visibility.Visible;
                contrasenaTxt.Visibility = Visibility.Visible;
            }
            else
            {
                usuarioLbl.Visibility = Visibility.Hidden;
                usuarioTxt.Visibility = Visibility.Hidden;
                contrasenaLbl.Visibility = Visibility.Hidden;
                contrasenaTxt.Visibility = Visibility.Hidden;
                usuarioTxt.Text = "";
                contrasenaTxt.Password = "";
            }

            if (AlgunCampoLleno())
            {
                limpiarBtn.IsEnabled = true;
            }
            else
            {
                limpiarBtn.IsEnabled = false;
            }
        }

        private void nombreTxt_TextInput(object sender, TextCompositionEventArgs e)
        {
            int ascci = Convert.ToInt32(Convert.ToChar(e.Text));

            if (ascci >= 65 && ascci <= 90 || ascci >= 97 && ascci <= 122)
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void aPaternoTxt_TextInput(object sender, TextCompositionEventArgs e)
        {
            int ascci = Convert.ToInt32(Convert.ToChar(e.Text));

            if (ascci >= 65 && ascci <= 90 || ascci >= 97 && ascci <= 122)
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void aMaternoTxt_TextInput(object sender, TextCompositionEventArgs e)
        {
            int ascci = Convert.ToInt32(Convert.ToChar(e.Text));

            if (ascci >= 65 && ascci <= 90 || ascci >= 97 && ascci <= 122)
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void codigoPostalTxt_TextInput(object sender, TextCompositionEventArgs e)
        {
            int ascci = Convert.ToInt32(Convert.ToChar(e.Text));

            if (ascci >= 48 && ascci <= 57) e.Handled = false;

            else e.Handled = true;
        }

        private void telefonoTxt_TextInput(object sender, TextCompositionEventArgs e)
        {
            int ascci = Convert.ToInt32(Convert.ToChar(e.Text));

            if (ascci >= 48 && ascci <= 57)
            {
                e.Handled = false;
            }
            else 
            { 
                e.Handled = true; 
            }
        }

        // Se usara para permitir solo letras y numeros en el campo de direcciob
        private void calleTxt_TextInput(object sender, TextCompositionEventArgs e)
        {
            
        }



        private void LogoutBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult opcion;

            opcion = MessageBox.Show("¿Seguro que deseas cerrar la sesión?", "Cerrar sesión",
                    MessageBoxButton.OKCancel, MessageBoxImage.Question);

            if (opcion == MessageBoxResult.OK)
            {
                FuncionesComunes.CerrarSesion();
                this.Close();
            }
        }
    }
}
