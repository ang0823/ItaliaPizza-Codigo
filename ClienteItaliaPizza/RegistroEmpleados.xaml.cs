using ClienteItaliaPizza.Servicio;
using System;
using System.ServiceModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ClienteItaliaPizza
{
    /// <summary>
    /// Lógica de interacción para RegistroEmpleados.xaml
    /// </summary>
    public partial class RegistroEmpleados : Window, IRegistrarCuentaUsuarioCallback
    {
        Random idGenerado = new Random();
        CuentaUsuario1 CuentaUsuario;

        public RegistroEmpleados(CuentaUsuario1 cuenta)
        {
            CuentaUsuario = cuenta;
            InitializeComponent();
            LlenarPuestosCb();
            UsuarioLbl.Content = cuenta.nombreUsuario;
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

        private void AsignarCuentaUsuario(ref CuentaUsuario cuentaUsuario)
        {
            cuentaUsuario.nombreUsuario = usuarioTxt.Text;
            cuentaUsuario.contraseña = contrasenaTxt.Password;
        }

        private void AsignarDireccion(ref Direccion direccion)
        {
            direccion.calle = calleTxt.Text;
            direccion.colonia = coloniaTxt.Text;
            direccion.numeroExterior = NoExteroorTxt.Text;
            direccion.numeroInterior = NoInteroorTxt.Text;
            direccion.codigoPostal = codigoPostalTxt.Text;
        }

        private void AsignarInfoEmpleado(ref Empleado empleado)
        {
            bool trabajadorActual = true;

            empleado.idEmpleadoGenerado = idEmpleadoTxt.Text;
            empleado.nombre = nombreTxt.Text;
            empleado.apellidoPaterno = aPaternoTxt.Text;
            empleado.apellidoMaterno = aMaternoTxt.Text;
            empleado.correo = correoElectronicoTxt.Text;
            empleado.telefono = telefonoTxt.Text;
            empleado.activado = trabajadorActual;
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

        private Boolean EsAdministrativo()
        {
            Boolean EsAdministrativo = false;

            if (puestosCB.SelectedIndex == 3 || puestosCB.SelectedIndex == 4 || puestosCB.SelectedIndex == 5)
            {
                EsAdministrativo = true;
            }

            return EsAdministrativo;
        }

        private Boolean EsCorreoElectronicoValido()
        {
            Boolean EsValido = true;
            string ExpresionRegular = "^[_a-z0-9-]+(.[_a-z0-9-]+)@[a-z0-9-]+(.[a-z0-9-]+)(.[a-z]{2,4})$";
            string CorreoIngresado = correoElectronicoTxt.Text;
            Match validacion = Regex.Match(CorreoIngresado, ExpresionRegular);
            EsValido = validacion.Success;

            return EsValido;
        }

        private String GenerarIdEmpleado() 
        {
            int primerPar = idGenerado.Next(10, 20);
            int segundoPar = idGenerado.Next(10, 99);
            int tercerPar = idGenerado.Next(10, 99);
            int cuartoPart = idGenerado.Next(20, 99);

            return primerPar.ToString() + segundoPar.ToString() + tercerPar.ToString() + cuartoPart.ToString();
        }

        private void LlenarPuestosCb()
        {
            puestosCB.Items.Insert(0, "Seleccionar rol");
            puestosCB.Items.Insert(1, "Mesero");
            puestosCB.Items.Insert(2, "Cocinero");
            puestosCB.Items.Insert(3, "Call Center");
            puestosCB.Items.Insert(4, "Contador");
            puestosCB.Items.Insert(5, "Gerente");
            puestosCB.SelectedIndex = 0;
        }

        private void RegistrarInfoEmpleado()
        {
            Empleado empleado = new Empleado();
            Direccion direccion = new Direccion();
            int rol = puestosCB.SelectedIndex;

            try
            {
                InstanceContext context = new InstanceContext(this);
                RegistrarCuentaUsuarioClient ServicioEmpleado = new RegistrarCuentaUsuarioClient(context);
                AsignarInfoEmpleado(ref empleado);
                AsignarDireccion(ref direccion);

                if (EsAdministrativo())
                {
                    CuentaUsuario cuenta = new CuentaUsuario();
                    AsignarCuentaUsuario(ref cuenta);
                    ServicioEmpleado.RegistrarCuentaUsuario(cuenta, empleado, direccion, rol);
                }
                else
                {
                    ServicioEmpleado.RegistrarCuentaUsuario2(empleado, direccion, rol);
                }
            }
            catch(Exception error)
            {
                FuncionesComunes.MostrarMensajeDeError(error.Message + " " + error.GetType());
            }
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
            NoExteroorTxt.Text = "";
            NoInteroorTxt.Text = "";
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
            if (correoElectronicoTxt.Text.Length == 0 || EsCorreoElectronicoValido())
            {
                correoElectronicoTxt.BorderBrush = System.Windows.Media.Brushes.LightBlue;
            }
            else
            {
                correoElectronicoTxt.BorderBrush = System.Windows.Media.Brushes.Red;
            }

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
            if (EsCorreoElectronicoValido())
            {
                RegistrarInfoEmpleado();
            }
            else
            {
                FuncionesComunes.MostrarMensajeDeError("Correo electrónico invalido");
            }
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

        // Se usara para permitir solo letras y numeros en el campo de dirección
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

        public void RespuestaRCU(string mensaje)
        {
            Dispatcher.Invoke(() =>
            {
                if(mensaje == "Éxito al cuenta de usuario" || mensaje == "Éxito al registrarReceta")
                {
                    mensaje = "Se registro la información del empleado exitosamente";
                    FuncionesComunes.MostrarMensajeExitoso(mensaje);
                }
                else
                {
                    FuncionesComunes.MostrarMensajeDeError(mensaje);
                }
            });
            VaciarCampos();
            idEmpleadoTxt.Text = GenerarIdEmpleado().ToString();
        }
    }
}
