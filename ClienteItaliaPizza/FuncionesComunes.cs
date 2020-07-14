using ClienteItaliaPizza.Servicio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ClienteItaliaPizza
{
    public static class FuncionesComunes
    {
        /// <summary>
        /// Muestra la pantalla de inicio de sesión
        /// </summary>
        public static void CerrarSesion()
        {
            MainWindow ventana = new MainWindow();
            ventana.Show();
        }

        /// <summary>
        /// Muestra la pantalla principal del administrador
        /// </summary>
        /// <param name="CuentaUsuario">El usuario actual que está loggeado en la pantalla que llama a este método</param>
        public static void MostrarVentanaPrincipal(CuentaUsuario1 CuentaUsuario)
        {
            Principal ventana = new Principal(CuentaUsuario);
            ventana.Show();
        }

        /// <summary>
        /// Intenta convertir una cadena de texto en un entero.
        /// </summary>
        /// <param name="EntradaUsuario">La cadena de texto ingreda por el usuario en un TextBox o cualquier cadena.</param>
        /// <returns>La cadena de texto recivida, pero de tipo int.</returns>
        /// <exception cref="FormatException">Se produce cu8ando la cedan de texto enviada incluye caracteres diferentes a números.</exception>
        /// <exception cref="OverflowException">Se produce cuando la cadena de texto enviada produce un número con valor más grande que int.MaxValue</exception>
        public static int ParsearAEntero(string EntradaUsuario)
        {
            int entero;

            try
            {
                entero = int.Parse(EntradaUsuario);
            }
            catch (FormatException)
            {
                throw new FormatException("Ingresó datos inválidos.");
            }
            catch (OverflowException)
            {
                throw new OverflowException("El valor de uno o varios datos numéricos es demaiado grande.");
            }

            return entero;
        }

        /// <summary>
        /// Intenta convertir una cadena de texto en un tipo de dato "short".
        /// </summary>
        /// <param name="EntradaUsuario">La cadena de texto ingreda por el usuario en un TextBox o cualquier otra cadena.</param>
        /// <returns>La cadena de texto recivida, pero de tipo "short".</returns>
        /// <exception cref="FormatException">Se produce cuando la cadena de texto enviada incluye caracteres diferentes a números.</exception>
        /// <exception cref="OverflowException">Se produce cuando la cadena de texto enviada produce un número con valor más grande que short.MaxValue</exception>
        public static short ParsearAShort(string EntradaUsuario)
        {
            short shortNumber;

            try
            {
                shortNumber = short.Parse(EntradaUsuario);
            }
            catch (FormatException)
            {
                throw new FormatException("Ingresó datos inválidos.");
            }
            catch (OverflowException)
            {
                throw new OverflowException("El valor de uno o varios datos numéricos es demaiado grande.");
            }

            return shortNumber;
        }

        /// <summary>
        /// Intenta convertir una cadena de texto en un tipo de dato "float".
        /// </summary>
        /// <param name="EntradaUsuario">La cadena de texto ingreda por el usuario en un TextBox o cualquier otra cadena.</param>
        /// <returns>La cadena de texto recivida, pero de tipo "float".</returns>
        /// <exception cref="FormatException">Se produce cuando la cadena de texto enviada incluye caracteres diferentes a números.</exception>
        /// <exception cref="OverflowException">Se produce cuando la cadena de texto enviada produce un número con valor más grande que float.MaxValue</exception>
        public static float ParsearAFloat(string EntradaUsuario)
        {
            float costo = 0;

            try
            {
                costo = float.Parse(EntradaUsuario);
            }
            catch (FormatException)
            {
                throw new FormatException("Ingresó datos inválidos.");
            }
            catch (OverflowException)
            {
                throw new OverflowException("El valor de uno o varios datos numéricos es demaiado grande.");
            }

            return costo;
        }

        public static double ParsearADouble(string EntradaUsuario)
        {
            double valor = 0;
            try
            {
                valor = Convert.ToDouble(EntradaUsuario);
            }
            catch (FormatException)
            {
                MessageBox.Show("El formato es incorrecto");
            }
            catch (OverflowException)
            {
                MessageBox.Show("Ha ocurrido un error por desbordamiento");
            }
            return valor;
        }

        /// <summary>
        /// Muestra un mensaje en la interfaz, con el título "Información" y un icono de error dentro de él, incluyendo solo el botón "Aceptar".
        /// </summary>
        /// <param name="mensaje">El mensaje que se dea mostrar em el interior.</param>
        public static void MostrarMensajeDeError(string mensaje)
        {
            string titulo = "Información";
            MessageBoxResult opcion;
            opcion = MessageBox.Show(mensaje, titulo,
                MessageBoxButton.OK, MessageBoxImage.Error);
        }

        /// <summary>
        /// Muestra un mensaje de exito en la interfaz con el título "Operación exitosa", el icono de información dentro de él y un solo botón de "Aceptar".
        /// </summary>
        /// <param name="Mensaje">El mensaje que se desea mostrar en el contenido.</param>
        public static void MostrarMensajeExitoso(string Mensaje)
        {
            MessageBoxResult opcion;
            string titulo = "Operación exitósa";
            opcion = MessageBox.Show(Mensaje, titulo,
                MessageBoxButton.OK, MessageBoxImage.Information);
        }

        /// <summary>
        /// Muestra un mensaje de confirmación con las opciones "Aceptar" y "Cancelar"
        /// </summary>
        /// <param name="titulo">Nombre de la operación a realizar</param>
        /// <param name="mensaje">Pregunta a hacer al cliente para confirmar la operación</param>
        /// <returns>true: si la operación se acepta. false: si se rechaza la operación</returns>
        public static bool ConfirmarOperacion(string titulo, string mensaje)
        {
            MessageBoxResult option;
            option = MessageBox.Show(mensaje, titulo, MessageBoxButton.OKCancel, MessageBoxImage.Question);

            if (option == MessageBoxResult.OK)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
