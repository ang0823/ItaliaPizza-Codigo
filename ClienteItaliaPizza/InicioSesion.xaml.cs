﻿using System;

using System.ServiceModel;
using System.Windows;
using ClienteItaliaPizza.Servicio;
using ClienteItaliaPizza.Pantallas;
using System.Windows.Input;
using System.Windows.Controls;

namespace ClienteItaliaPizza
{
    [CallbackBehavior(UseSynchronizationContext = false)]

    public partial class MainWindow : Window, ILoginCallback
    {
        CuentaUsuario1 CuentaUsuario;

        public MainWindow()
        {
            InitializeComponent();
            textBoxNombreUsuario.Focus();
        }

        private void IniciarSesion()
        {
            DeshabilitarCamposYBotonones();
            string Mensaje;
            string nombreUsuario = textBoxNombreUsuario.Text.Trim();
            string contraseña = passwordBoxContraseña.Password.Trim();

            try
            {
                InstanceContext instanceContext = new InstanceContext(this);
                LoginClient cliente = new LoginClient(instanceContext);

                if (DatosCompletos(nombreUsuario, contraseña))
                {
                    cliente.IniciarSesion(nombreUsuario, contraseña);
                }
                else
                {
                    Mensaje = "Se requiere usuario y contraseña";
                    FuncionesComunes.MostrarMensajeDeError(Mensaje);
                    HabilitarCamposYBotonones();
                }
            }
            catch (EndpointNotFoundException)
            {
                Mensaje = "Falló la conexión con el servidor";
                FuncionesComunes.MostrarMensajeDeError(Mensaje);
                HabilitarCamposYBotonones();
            } 
            catch (InvalidOperationException error)
            {
                Mensaje = error.Message;
                FuncionesComunes.MostrarMensajeDeError(Mensaje);
                HabilitarCamposYBotonones();
            }
            catch(TimeoutException)
            {
                Mensaje = "Se excedió el tiempo de espera y no hubo respuesta del servidor.";
                FuncionesComunes.MostrarMensajeDeError(Mensaje);
                HabilitarCamposYBotonones();
            }
        }

        private void DeshabilitarCamposYBotonones()
        {
            textBoxNombreUsuario.IsEnabled = false;
            passwordBoxContraseña.IsEnabled = false;
            LoginBtn.IsEnabled = false;
            ButtonVentanaMeseros.IsEnabled = false;
            ButtonVentanaCocina.IsEnabled = false;
        }

        private void HabilitarCamposYBotonones()
        {
            textBoxNombreUsuario.IsEnabled = true;
            passwordBoxContraseña.IsEnabled = true;
            LoginBtn.IsEnabled = true;
            ButtonVentanaMeseros.IsEnabled = true;
            ButtonVentanaCocina.IsEnabled = true;
        }

        private void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            IniciarSesion();
        }

        private bool DatosCompletos(string nombreUsuario, string contrasena)
        {
            bool datosValidos = false;

            if (nombreUsuario != "" && contrasena != "")
            {
                datosValidos = true;
            }
                
            return datosValidos;
        }

        public void DevuelveCuenta(CuentaUsuario1 cuenta, Empleado1 empleado, Direccion1 direccion, Rol1 rol)
        {
            Dispatcher.Invoke(() =>
            {
               CuentaUsuario = cuenta;
                
                var rolCopia = rol.rol;
                if (rol.rol == "Call Center")
                {
                    VentanaPedidos ventanaPedidos = new VentanaPedidos(empleado.idEmpleado, empleado.idEmpleadoGenerado);
                    ventanaPedidos.Show();
                    this.Close();
                }
                else if (rol.rol == "Gerente" || rol.rol == "Contador")
                {
                    Principal ventana = new Principal(cuenta);
                    ventana.Show();
                    this.Close();
                }
                else
                {
                    FuncionesComunes.MostrarMensajeDeError("No cuentas con permisos para iniciar sesión");
                    HabilitarCamposYBotonones();
                    textBoxNombreUsuario.Text = "";
                    passwordBoxContraseña.Password = "";
                }                
            });
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            VentanaCocina ventanaCocina = new VentanaCocina();
            ventanaCocina.Show();
            this.Close();
        }

        private void ButtonVentanaMeseros_Click_1(object sender, RoutedEventArgs e)
        {
            VentanaPedidos ventanaPedidos = new VentanaPedidos();
            ventanaPedidos.Show();
            this.Close();
        }

        private void textBoxNombreUsuario_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                IniciarSesion();
            }
        }

        private void passwordBoxContraseña_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                DeshabilitarCamposYBotonones();
                IniciarSesion();
            }
        }

        public void RespuestaLogin(string mensaje)
        {
            Dispatcher.Invoke(() =>
            {
                FuncionesComunes.MostrarMensajeDeError(mensaje);
                HabilitarCamposYBotonones();
            });
        }

        private void passwordBoxContraseña_PasswordChanged(object sender, RoutedEventArgs e)
        {
            PasswordBox pb = sender as PasswordBox;
            pb.Tag = (!string.IsNullOrEmpty(pb.Password)).ToString();
        }
    }
}
