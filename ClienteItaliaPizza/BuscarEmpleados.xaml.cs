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
        public BuscarEmpleados(CuentaUsuario cuenta)
        {
            InitializeComponent();
            criterioCb.Items.Insert(0, "Buscar por:");
            criterioCb.Items.Insert(1, "ID empleado");
            criterioCb.Items.Insert(2, "Nombre");
            criterioCb.Items.Insert(3, "Apellido paterno");
            criterioCb.Items.Insert(4, "Apellido Materno");
            criterioCb.Items.Insert(5, "Calle");
            criterioCb.Items.Insert(6, "Colonia");
            criterioCb.Items.Insert(7, "Código postal");
            criterioCb.Items.Insert(8, "Correo electrónico");
            criterioCb.Items.Insert(9, "Teléfono");
            criterioCb.Items.Insert(10, "Puesto");
            criterioCb.Items.Insert(11, "Usuario");

            criterioCb.SelectedIndex = 0;
            CuentaUsuario = cuenta;
            UsuarioLbl.Content = cuenta.nombreUsuario;
            acepttarBtn.Visibility = Visibility.Hidden;
            buscarBtn.IsEnabled = false;
            editarBtn.IsEnabled = false;
            eliminarBtn.IsEnabled = false;
            vaciarBtn.IsEnabled = false;
        }

        private Boolean CamposLlenos()
        {
            if (criterioCb.SelectedIndex != 0 && entradaTxt.Text.Length > 0)
            {
                return true;
            }

            return false;
        }

        private void CerrarSesion()
        {
            Dispatcher.Invoke(() =>
            {
                MainWindow ventana = new MainWindow();
                ventana.Show();
                this.Close();
            });
        }

        private void MostrarVentanaPrincipal()
        {
            Dispatcher.Invoke(() =>
            {
                Principal ventana = new Principal(CuentaUsuario);
                ventana.Show();
                this.Close();
            });
        }

        private void VaciarCampos()
        {
            criterioCb.SelectedIndex = 0;
            entradaTxt.Text = "";
        }

        private void buscarBtn_Click(object sender, RoutedEventArgs e)
        {
            entradaTxt.Text = "";
            Empleado n = new Servicio.Empleado();
            CuentaUsuario c = new Servicio.CuentaUsuario();
            EmpleadoDataGrid Data;

            n.IdEmpleado = 82205;
            n.nombre = "Ángel Daniel";
            n.apellidoPaterno = "Sánchez";
            n.apellidoMaterno = "Martínez";
            n.correo = "angelsanchez934@gmail.com";
            n.telefono = "2282739774";
            c.nombreUsuario = "ang0823";
            c.contraseña = "abc123";
            Data = new EmpleadoDataGrid(n, c);
            resultsData.Items.Add(Data);
        }

        private void CerrarSesionBtn_Click(object sender, RoutedEventArgs e)
        {
            CerrarSesion();
        }

        private void resultsData_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!resultsData.SelectedItem.Equals(""))
            {
                editarBtn.IsEnabled = true;
                eliminarBtn.IsEnabled = true;
            }
            else
            {
                editarBtn.IsEnabled = false;
                eliminarBtn.IsEnabled = false;
            }
        }

        private void entradaTxt_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (CamposLlenos())
            {
                buscarBtn.IsEnabled = true;
            }
            else
            {
                buscarBtn.IsEnabled = false;
            }
        }

        private void vaciarBtn_Click(object sender, RoutedEventArgs e)
        {
            vaciarBtn.IsEnabled = false;
            resultsData.Items.Clear();
            resultsData.Items.Refresh();
        }

        private void cancelarBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult opcion;

            opcion = MessageBox.Show("¿Volver a pantalla anteior?", "Cancelar",
                    MessageBoxButton.OKCancel, MessageBoxImage.Question);

                if (opcion == MessageBoxResult.OK)
                {
                    VaciarCampos();
                    MostrarVentanaPrincipal();
                }
        }

        private void criterioCb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CamposLlenos())
            {
                buscarBtn.IsEnabled = true;
            }
            else
            {
                buscarBtn.IsEnabled = false;
            }
        }

    }
}
