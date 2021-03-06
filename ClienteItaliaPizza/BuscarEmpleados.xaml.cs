﻿using ClienteItaliaPizza.Servicio;
using System;
using System.ServiceModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;


namespace ClienteItaliaPizza
{
    [CallbackBehavior(UseSynchronizationContext = false)]
    /// <summary>
    /// Lógica de interacción para BuscarEmpleados.xaml
    /// </summary>
    public partial class BuscarEmpleados : Window, IObtenerCuentasUsuarioCallback, IModificarCuentaUsuarioCallback, IEliminarCuentaUsuarioCallback
    {
        CuentaUsuario1 CuentaUsuario;

        Empleado empleado = new Empleado();
        Direccion direccion = new Direccion();
        CuentaUsuario cuenta = new CuentaUsuario();
        string nombreRol;
        bool enEdicion = false;

        public BuscarEmpleados()
        {
            InitializeComponent();
            LlenarPuestosCb();
            DeshabilitarCampos();

            SearchBtn.IsEnabled = false;
            idEmpleadoTxt.IsEnabled = false;
            EstadoTxt.IsEnabled = false;
            EditarGuardarBtn.IsEnabled = false;
            EliminarBtn.IsEnabled = false;
        }

        public BuscarEmpleados(CuentaUsuario1 cuenta)
        {
            InitializeComponent();
            LlenarPuestosCb();
            DeshabilitarCampos();

            CuentaUsuario = cuenta;
            UsuarioLbl.Content = cuenta.nombreUsuario;

            SearchBtn.IsEnabled = false;
            idEmpleadoTxt.IsEnabled = false;
            EstadoTxt.IsEnabled = false;
            EditarGuardarBtn.IsEnabled = false;
            EliminarBtn.IsEnabled = false;
        }

        private Boolean CamposLlenos()
        {
            if (nombreTxt.Text.Length > 0 && aPaternoTxt.Text.Length > 0 && aMaternoTxt.Text.Length > 0
                && correoElectronicoTxt.Text.Length > 0 && telefonoTxt.Text.Length > 0
                && calleTxt.Text.Length > 0 && coloniaTxt.Text.Length > 0
                && codigoPostalTxt.Text.Length > 0 && (puestosCB.SelectedIndex == 1
                || puestosCB.SelectedIndex == 2) && NoExteriorTxt.Text.Length > 0)
            {
                return true;
            }
            else if (nombreTxt.Text.Length > 0 && aPaternoTxt.Text.Length > 0 && aMaternoTxt.Text.Length > 0
                && correoElectronicoTxt.Text.Length > 0 && telefonoTxt.Text.Length > 0 && NoExteriorTxt.Text.Length > 0
                && calleTxt.Text.Length > 0 && coloniaTxt.Text.Length > 0
                && codigoPostalTxt.Text.Length > 0 && (puestosCB.SelectedIndex == 3
                || puestosCB.SelectedIndex == 4 || puestosCB.SelectedIndex == 5)
                && usuarioTxt.Text.Length > 0 && contrasenaTxt.Password.Length > 0)
            {
                return true;
            }

            return false;
        }

        private void DesactivarEmpleado()
        {
            try
            {
                InstanceContext context = new InstanceContext(this);
                EliminarCuentaUsuarioClient ServicioEliminar = new EliminarCuentaUsuarioClient(context);

                ServicioEliminar.EliminarCuentaUsuario(empleado.idEmpleadoGenerado);
            }
            catch (Exception e)
            {
                FuncionesComunes.MostrarMensajeDeError(e.Message);
            }
        }

        private void DeshabilitarCampos()
        {
            enEdicion = false;
            nombreTxt.IsEnabled = false;
            aPaternoTxt.IsEnabled = false;
            aMaternoTxt.IsEnabled = false;
            calleTxt.IsEnabled = false;
            NoInteriorTxt.IsEnabled = false;
            NoExteriorTxt.IsEnabled = false;
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
                InstanceContext context = new InstanceContext(this);
                ModificarCuentaUsuarioClient ServicioModificar = new ModificarCuentaUsuarioClient(context);

                if (HayInformacionEditada())
                {
                    empleado.nombre = nombreTxt.Text.Trim();
                    empleado.apellidoPaterno = aPaternoTxt.Text.Trim();
                    empleado.apellidoMaterno = aMaternoTxt.Text.Trim();
                    empleado.correo = correoElectronicoTxt.Text.Trim();
                    empleado.telefono = telefonoTxt.Text.Trim();
                    empleado.activado = EstaActivado();
                    direccion.calle = calleTxt.Text.Trim();
                    direccion.colonia = coloniaTxt.Text.Trim();
                    direccion.numeroExterior = NoExteriorTxt.Text.Trim();
                    direccion.numeroInterior = NoInteriorTxt.Text.Trim();
                    direccion.codigoPostal = codigoPostalTxt.Text.Trim();
                    nombreRol = puestosCB.SelectedItem.ToString();

                    if (EsAdministrativo())
                    {
                        cuenta = new CuentaUsuario();
                        cuenta.nombreUsuario = usuarioTxt.Text;
                        cuenta.contraseña = contrasenaTxt.Password;
                        ServicioModificar.ModificarCuentaUsuario(cuenta, empleado, direccion, nombreRol);
                        EstablecerInformacion();
                    }
                    else
                    {
                        ServicioModificar.ModificarCuentaUsuario2(empleado, direccion, nombreRol);
                        EstablecerInformacion();
                    }
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

        private Boolean EsAdministrativo()
        {
            bool EsAdministrativo = false;

            if (puestosCB.SelectedIndex == 3 || puestosCB.SelectedIndex == 4 || puestosCB.SelectedIndex == 5)
            {
                EsAdministrativo = true;
            }

            return EsAdministrativo;
        }

        private Boolean EsCorreoElectronicoValido()
        {
            Boolean EsValido = false;
            string ExpresionRegular = "^[_a-z0-9-]+(.[_a-z0-9-]+)@[a-z0-9-]+(.[a-z0-9-]+)(.[a-z]{2,4})$";
            string CorreoIngresado = correoElectronicoTxt.Text;
            Match validacion = Regex.Match(CorreoIngresado, ExpresionRegular);
            EsValido = validacion.Success;

            return EsValido;
        }

        private bool EstaActivado()
        {
            bool estaActivado = false;

            if (empleado.activado)
            {
                estaActivado = true;
            }

            return estaActivado;
        }

        private void EstablecerInformacion()
        {
            idEmpleadoTxt.Text = empleado.idEmpleadoGenerado;
            nombreTxt.Text = empleado.nombre;
            aPaternoTxt.Text = empleado.apellidoPaterno;
            aMaternoTxt.Text = empleado.apellidoMaterno;
            correoElectronicoTxt.Text = empleado.correo;
            telefonoTxt.Text = empleado.telefono;
            if (empleado.activado)
            {
                EstadoTxt.Text = "Activo";
            }
            else
            {
                EstadoTxt.Text = "Inactivo";
            }
            calleTxt.Text = direccion.calle;
            NoExteriorTxt.Text = direccion.numeroExterior;
            NoInteriorTxt.Text = direccion.numeroInterior;
            coloniaTxt.Text = direccion.colonia;
            codigoPostalTxt.Text = direccion.codigoPostal;
            puestosCB.SelectedItem = nombreRol;
            if (cuenta != null)
            {
                usuarioTxt.Text = cuenta.nombreUsuario;
                contrasenaTxt.Password = cuenta.contraseña;
            }
        }

        private void HabilitarCampos()
        {
            enEdicion = true;
            nombreTxt.IsEnabled = true;
            aPaternoTxt.IsEnabled = true;
            aMaternoTxt.IsEnabled = true;
            calleTxt.IsEnabled = true;
            NoExteriorTxt.IsEnabled = true;
            NoInteriorTxt.IsEnabled = true;
            coloniaTxt.IsEnabled = true;
            codigoPostalTxt.IsEnabled = true;
            correoElectronicoTxt.IsEnabled = true;
            telefonoTxt.IsEnabled = true;
            usuarioTxt.IsEnabled = true;
            puestosCB.IsEnabled = true;
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
            string NuevoNoExterior = NoExteriorTxt.Text;
            string NuevoNoInterior = NoInteriorTxt.Text;
            string NuevaColonia = coloniaTxt.Text;
            string NuevoCodigoPostal = codigoPostalTxt.Text;
            string NuevoRol = puestosCB.SelectedItem.ToString();
            string NuevaContrasena = contrasenaTxt.Password;

            if (NuevoNombre != empleado.nombre || NuevoPaterno != empleado.apellidoPaterno || 
                NuevoMaterno != empleado.apellidoMaterno || NuevoCorreo != empleado.correo || 
                NuevoTelefono != empleado.telefono || NuevaCalle != direccion.calle || 
                NuevoNoExterior != direccion.numeroExterior || NuevoNoInterior != direccion.numeroInterior || 
                NuevaColonia != direccion.colonia || NuevoCodigoPostal !=  direccion.codigoPostal || 
                NuevoRol != nombreRol || NuevaContrasena != contrasenaTxt.Password)
            {
                InformacionEditada = true;
            }

            return InformacionEditada;
        }

        private void LlenarPuestosCb()
        {
            puestosCB.Items.Insert(0, string.Empty);
            puestosCB.Items.Insert(1, "Mesero");
            puestosCB.Items.Insert(2, "Cocinero");
            puestosCB.Items.Insert(3, "Call Center");
            puestosCB.Items.Insert(4, "Contador");
            puestosCB.Items.Insert(5, "Gerente");
            puestosCB.SelectedIndex = 0;
        }

        private void ObtenerEmpleado()
        {
            string idEmpleado = SearchBox.Text;

            try
            {
                InstanceContext context = new InstanceContext(this);
                ObtenerCuentasUsuarioClient ServicioBusqueda = new ObtenerCuentasUsuarioClient(context);

                if (SearchBox.Text.Length > 0)
                {
                    if (enEdicion)
                    {
                        DeshabilitarCampos();
                        EditarGuardarBtn.Content = "Editar";
                    }

                    // Se realiza un parse para corroborar que el ID son números
                    int.Parse(idEmpleado);
                    ServicioBusqueda.ObtenerCuentas(idEmpleado);
                }
                else
                {
                    FuncionesComunes.MostrarMensajeDeError("Se debe introducir un ID para buscar");
                }
            }
            catch (FormatException)
            {
                FuncionesComunes.MostrarMensajeDeError("El ID de empleado solo puede contener números");
            }
            catch (Exception e)
            {
                FuncionesComunes.MostrarMensajeDeError(e.GetType() + "Valor máximo: " + int.MaxValue + " " + e.Message);
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
            DeshabilitarCampos();
            ObtenerEmpleado();
        }

        private void cancelarBtn_Click(object sender, RoutedEventArgs e)
        {
            if (enEdicion)
            {
                string titulo = "Cancelar";
                string pregunta = "¿Volver a pantalla principal?";
                bool salir = FuncionesComunes.ConfirmarOperacion(titulo, pregunta);

                if (salir)
                {
                    FuncionesComunes.MostrarVentanaPrincipal(this.CuentaUsuario);
                    this.Close();
                }
            }
            else
            {
                FuncionesComunes.MostrarVentanaPrincipal(CuentaUsuario);
                this.Close();
            }

            
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

            if (CamposLlenos() && EstaActivado())
            {
                EditarGuardarBtn.IsEnabled = true;
            }
            else
            {
                EditarGuardarBtn.IsEnabled = false;
            }
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

        private void puestosCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (EsAdministrativo())
            {
                usuarioLbl.Visibility = Visibility.Visible;
                usuarioTxt.Visibility = Visibility.Visible;
                contrasenaLbl.Visibility = Visibility.Visible;
                contrasenaTxt.Visibility = Visibility.Visible;
            }
            else if(!EsAdministrativo())
            {
                usuarioLbl.Visibility = Visibility.Hidden;
                usuarioTxt.Visibility = Visibility.Hidden;
                contrasenaLbl.Visibility = Visibility.Hidden;
                contrasenaTxt.Visibility = Visibility.Hidden;
                usuarioTxt.Text = "";
                contrasenaTxt.Password = "";
            }

            if (CamposLlenos() && EstaActivado())
            {
                EditarGuardarBtn.IsEnabled = true;
            }
            else
            {
                EditarGuardarBtn.IsEnabled = false;
            }
        }
        private void EditarGuardarBtn_Click(object sender, RoutedEventArgs e)
        {

            if (EditarGuardarBtn.Content.ToString() == "Editar")
            {
                enEdicion = true;
                EditarGuardarBtn.Content = "Guardar";
                HabilitarCampos();
            }
            else
            {
                if (EsCorreoElectronicoValido())
                {
                    enEdicion = false;
                    EditarInformacion();
                    EditarGuardarBtn.Content = "Editar";
                }
                else
                {
                    FuncionesComunes.MostrarMensajeDeError("El correo electrónico no tiene un formato válido");
                }
            }
        }

        private void EliminarBtn_Click(object sender, RoutedEventArgs e)
        {
            string usuarioActual = usuarioTxt.Text;

            if (usuarioActual == CuentaUsuario.nombreUsuario)
            {
                FuncionesComunes.MostrarMensajeDeError("No se puede dar de baja al usuario con la sesión actual.");
            }
            else
            {
                string titulo = "Confirmar acción";
                string mensaje = "La acción que está a punto de realizar no se puede revertir, ¿Deseas proseguir con la desactivación del empleado?";
                bool opcion = FuncionesComunes.ConfirmarOperacion(titulo, mensaje);

                if (opcion)
                {
                    DesactivarEmpleado();
                }
            }
        }

        public void RespuestaOCU(string mensaje)
        {
           Dispatcher.Invoke(() =>
           {
               mensaje = "El ID ingresado no arrojó ningún resultado";
               FuncionesComunes.MostrarMensajeDeError(mensaje);
           });
        }

        public void RespuestaMCU(string mensaje)
        {
            Dispatcher.Invoke(() =>
           {
               if(mensaje == "Se modificó correctamente")
               {
                   mensaje = "La información se modificó correctamente";
                   FuncionesComunes.MostrarMensajeExitoso(mensaje);
               }
               else
               {
                   FuncionesComunes.MostrarMensajeDeError(mensaje);
               }
               EstablecerInformacion();
           });
        }

        private void CamposDeTexto_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (CamposLlenos() && EstaActivado())
            {
                EditarGuardarBtn.IsEnabled = true;
            }
            else
            {
                EditarGuardarBtn.IsEnabled = false;
            }
        }

        private void contrasenaTxt_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (CamposLlenos() && EstaActivado())
            {
                EditarGuardarBtn.IsEnabled = true;
            }
            else
            {
                EditarGuardarBtn.IsEnabled = false;
            }
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (SearchBox.Text.Length > 0)
            {
                SearchBtn.IsEnabled = true;
            }
            else
            {
                SearchBtn.IsEnabled = false;
            }
        }

        private void SearchBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                ObtenerEmpleado();
                SearchBox.Text = "";
            }
        }

        public void DevuelveCuentas(CuentaUsuario1 cuenta, Empleado1 empleado, Direccion1 direccion, Rol1 rol)
        {
            Dispatcher.Invoke(() =>
            {
                VaciarCampos();
                this.empleado.IdEmpleado = empleado.idEmpleado;
                this.empleado.idEmpleadoGenerado = empleado.idEmpleadoGenerado;
                this.empleado.nombre = empleado.nombre;
                this.empleado.apellidoPaterno = empleado.apellidoPaterno;
                this.empleado.apellidoMaterno = empleado.apellidoMaterno;
                this.empleado.correo = empleado.correo;
                this.empleado.telefono = empleado.telefono;
                this.empleado.activado = empleado.activado;
                this.direccion.Id = direccion.id;
                this.direccion.calle = direccion.calle;
                this.direccion.numeroExterior = direccion.numeroExterior;
                this.direccion.numeroInterior = direccion.numeroInterior;
                this.direccion.colonia = direccion.colonia;
                this.direccion.codigoPostal = direccion.codigoPostal;
                nombreRol = rol.rol;
                this.cuenta.nombreUsuario = cuenta.nombreUsuario;
                this.cuenta.contraseña = cuenta.contraseña;
                EstablecerInformacion();
            });
        }

        public void DevuelveCuentas2(Empleado1 empleado, Direccion1 direccion, Rol1 rol)
        {
            cuenta.nombreUsuario = "";
            cuenta.contraseña = "";
            Dispatcher.Invoke(() =>
            {
                VaciarCampos();
                this.empleado.IdEmpleado = empleado.idEmpleado;
                this.empleado.idEmpleadoGenerado = empleado.idEmpleadoGenerado;
                this.empleado.nombre = empleado.nombre;
                this.empleado.apellidoPaterno = empleado.apellidoPaterno;
                this.empleado.apellidoMaterno = empleado.apellidoMaterno;
                this.empleado.correo = empleado.correo;
                this.empleado.telefono = empleado.telefono;
                this.empleado.activado = empleado.activado;
                this.direccion.Id = direccion.id;
                this.direccion.calle = direccion.calle;
                this.direccion.numeroExterior = direccion.numeroExterior;
                this.direccion.numeroInterior = direccion.numeroInterior;
                this.direccion.colonia = direccion.colonia;
                this.direccion.codigoPostal = direccion.codigoPostal;
                nombreRol = rol.rol;
                EstablecerInformacion();
            });
        }

        public void RespuestaECU(string mensaje)
        {
            if (mensaje == "Éxito al eliminar la cuenta de usuario")
            {
                EstadoTxt.Text = "Inactivo";
            }
            else
            {
                FuncionesComunes.MostrarMensajeDeError(mensaje);
            }
        }

        private void EstadoTxt_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (EstadoTxt.Text == "Inactivo")
            {
                EditarGuardarBtn.IsEnabled = false;
                EliminarBtn.IsEnabled = false;
            }
            else
            {
                EditarGuardarBtn.IsEnabled = true;
                EliminarBtn.IsEnabled = true;
            }
        }
    }
}
