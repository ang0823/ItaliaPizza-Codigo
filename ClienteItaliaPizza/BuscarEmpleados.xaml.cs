using ClienteItaliaPizza.Servicio;
using System;
using System.ServiceModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

/**
 * HAY ERROR AL DAR CLICK EN BOTON EDITAR
 * FALTA DEPURAR
 * 
 */


namespace ClienteItaliaPizza
{
    /// <summary>
    /// Lógica de interacción para BuscarEmpleados.xaml
    /// </summary>
    public partial class BuscarEmpleados : Window, IObtenerCuentasUsuarioCallback, IModificarCuentaUsuarioCallback
    {
        CuentaUsuario CuentaUsuario;

        Empleado empleado = new Empleado();
        Direccion direccion = new Direccion();
        CuentaUsuario cuenta = new CuentaUsuario();
        Rol puesto = new Rol();

        public BuscarEmpleados(CuentaUsuario cuenta)
        {
            InitializeComponent();
            LlenarPuestosCb();
            DeshabilitarCampos();

            CuentaUsuario = cuenta;
            UsuarioLbl.Content = cuenta.nombreUsuario;

            idEmpleadoTxt.IsEnabled = false;
            //EditarGuardarBtn.IsEnabled = false;
            //EliminarBtn.IsEnabled = false;
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

        private void DeshabilitarCampos()
        {
            nombreTxt.IsEnabled = false;
            aPaternoTxt.IsEnabled = false;
            aMaternoTxt.IsEnabled = false;
            calleTxt.IsEnabled = false;
            coloniaTxt.IsEnabled = false;
            codigoPostalTxt.IsEnabled = false;
            correoElectronicoTxt.IsEnabled = false;
            telefonoTxt.IsEnabled = false;
            puestosCB.IsEnabled = false;
            usuarioTxt.IsEnabled = false;
            contrasenaTxt.IsEnabled = false;
        }

        private void EditarInformacion() 
        {
                try
                {
                    if (HayInformacionEditada())
                    {
                        InstanceContext context = new InstanceContext(this);
                        ModificarCuentaUsuarioClient ServicioModificar = new ModificarCuentaUsuarioClient(context);

                        //empleado.IdEmpleado  = FuncionesComunes.ParsearAEntero(idEmpleadoTxt.Text.Trim());
                        empleado.nombre = nombreTxt.Text.Trim();
                        empleado.apellidoPaterno = aPaternoTxt.Text.Trim();
                        empleado.apellidoMaterno = aMaternoTxt.Text.Trim();
                        empleado.correo = correoElectronicoTxt.Text.Trim();
                        empleado.telefono = telefonoTxt.Text;
                        direccion.calle = calleTxt.Text;
                        direccion.colonia = coloniaTxt.Text;
                        direccion.numeroExterior = codigoPostalTxt.Text;
                        puesto.Id = puestosCB.SelectedIndex;
                        //if (usuarioTxt.Text != "" && contrasenaTxt.Password != "")
                        //{
                            usuarioTxt.Text = cuenta.nombreUsuario;
                            contrasenaTxt.Password = cuenta.contraseña;
                        //}
                        ServicioModificar.ModificarCuentaUsuario(cuenta, empleado, direccion, puesto);
                        EstablecerInformacion();
                    }
                    else
                    {
                        EstablecerInformacion();
                    }
                DeshabilitarCampos();
            }
            catch (Exception e)
            {
                FuncionesComunes.MostrarMensajeDeError(e.Message + ": " + e.GetType());
            }
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

        private void EstablecerInformacion()
        {
            idEmpleadoTxt.Text = empleado.IdEmpleado.ToString();
            nombreTxt.Text = empleado.nombre;
            aPaternoTxt.Text = empleado.apellidoPaterno;
            aMaternoTxt.Text = empleado.apellidoMaterno;
            correoElectronicoTxt.Text = empleado.correo;
            telefonoTxt.Text = empleado.telefono;
            calleTxt.Text = direccion.calle;
            coloniaTxt.Text = direccion.colonia;
            codigoPostalTxt.Text = direccion.numeroExterior;
            puestosCB.SelectedIndex = puesto.Id;
            if (usuarioTxt.Text != "" && contrasenaTxt.Password != "")
            {
                usuarioTxt.Text = cuenta.nombreUsuario;
                contrasenaTxt.Password = cuenta.contraseña;
            }
        }

        private void HabilitarCampos()
        {
            nombreTxt.IsEnabled = true;
            aPaternoTxt.IsEnabled = true;
            aMaternoTxt.IsEnabled = true;
            calleTxt.IsEnabled = true;
            coloniaTxt.IsEnabled = true;
            codigoPostalTxt.IsEnabled = true;
            correoElectronicoTxt.IsEnabled = true;
            telefonoTxt.IsEnabled = true;
            puestosCB.IsEnabled = true;
            usuarioTxt.IsEnabled = true;
            contrasenaTxt.IsEnabled = true;
        }

        private Boolean HayInformacionEditada()
        {
            Boolean InformacionEditada = false;
            string NuevoNombre = nombreTxt.Text;
            string NuevoPaterno = aPaternoTxt.Text;
            string NuevoMaterno = aMaternoTxt.Text;
            string NuevoCorreo = correoElectronicoTxt.Text;
            string NuevoTelefono = telefonoTxt.Text;
            string NuevaCalle = calleTxt.Text;
            string NuevaColonia = coloniaTxt.Text;
            string NuevoCodigoPostal = codigoPostalTxt.Text;
            int NuevoRol = puestosCB.SelectedIndex;
            string NuevoUsuario = usuarioTxt.Text;
            string NuevaContrasena = contrasenaTxt.Password;

            if (NuevoNombre != empleado.nombre || NuevoPaterno != empleado.apellidoPaterno || NuevoMaterno != empleado.apellidoMaterno
                || NuevoCorreo != empleado.correo || NuevoTelefono != empleado.telefono
                || NuevaCalle != direccion.calle || NuevaColonia != direccion.colonia || NuevoCodigoPostal !=  direccion.numeroExterior
                || NuevoUsuario != usuarioTxt.Text || NuevaContrasena != contrasenaTxt.Password
                || NuevoRol != puesto.Id)
            {
                InformacionEditada = true;
            }

            return InformacionEditada;
        }

        private void LlenarPuestosCb()
        {
            puestosCB.Items.Insert(0, "");
            puestosCB.Items.Insert(1, "Mesero");
            puestosCB.Items.Insert(2, "Cocinero");
            puestosCB.Items.Insert(3, "Call center");
            puestosCB.Items.Insert(4, "Contador");
            puestosCB.Items.Insert(5, "Gerente");
            puestosCB.SelectedIndex = 0;
        }

        private void ObtenerEmpledo()
        {
            try
            {
                InstanceContext context = new InstanceContext(this);
                ObtenerCuentasUsuarioClient ServicioBusqueda = new ObtenerCuentasUsuarioClient(context);

                ServicioBusqueda.ObtenerCuentas();
                EstablecerInformacion();
            }
            catch (Exception e)
            {
                FuncionesComunes.MostrarMensajeDeError(e.Message);
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
            coloniaTxt.Text = "";
            codigoPostalTxt.Text = "";
            usuarioTxt.Text = "";
            contrasenaTxt.Password = "";
        }

        private void SearchBtn_Click(object sender, RoutedEventArgs e)
        {
            ObtenerEmpledo();
            /*
             * Falta corrección del servidor para implementar este apartado
             */
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

        private void correoElectronicoTxt_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (EsCorreoElectronicoValido())
            {
                correoElectronicoTxt.BorderBrush = System.Windows.Media.Brushes.LightGray;
            }
            else
            {
                correoElectronicoTxt.BorderBrush = System.Windows.Media.Brushes.Red;
            }

            if (CamposLlenos())
            {
                EditarGuardarBtn.IsEnabled = true;
            }
            else
            {
                EditarGuardarBtn.IsEnabled = false;
            }
        }

        private void LogoutBtn_Click_1(object sender, RoutedEventArgs e)
        {
            FuncionesComunes.CerrarSesion();
            this.Close();
        }

        private void puestosCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
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
        }
        private void EditarGuardarBtn_Click(object sender, RoutedEventArgs e)
        {
            // Recuerda el método Trim() para eliminar espacios

            if (EditarGuardarBtn.Content.ToString() == "Editar")
            {
                EditarGuardarBtn.Content = "Guardar";
                HabilitarCampos();
            }
            else
            {
                EditarGuardarBtn.Content = "Editar";
                // Aquí debe ir el método que envía los cambios añl servidor
                EditarInformacion();
            }
        }

        public void DevuelveCuentas(CuentaUsuario1[] cuentas, Empleado1[] empleados, Direccion1[] direcciones, Rol1[] roles)
        {
           Dispatcher.Invoke(() =>
           {
               /**
                * Aqui se deben inicializar los objetos de la clase
                * Empleado empleado;
                * Direccion direccion;
                * CuentaUsuario cuenta;
                * Rol puesto;
                * Esto ayudara a que funcione el método "EstablecerInformacion()"
                */
               EstablecerInformacion();
           });
        }

        public void RespuestaOCU(string mensaje)
        {
           Dispatcher.Invoke(() =>
           {
               FuncionesComunes.MostrarMensajeDeError(mensaje);
           });
        }

        public void RespuestaMCU(string mensaje)
        {
            Dispatcher.Invoke(() =>
           {
               if(mensaje == "Se modificó correctamente")
               {
                   FuncionesComunes.MostrarMensajeExitoso(mensaje);
               }
               else
               {
                   FuncionesComunes.MostrarMensajeDeError(mensaje);
               }
           });
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
