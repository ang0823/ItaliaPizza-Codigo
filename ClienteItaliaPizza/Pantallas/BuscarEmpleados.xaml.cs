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
        public BuscarEmpleados()
        {
            InitializeComponent();
            criterioCb.Items.Insert(0, "Buscar por:");
            criterioCb.Items.Insert(1, "Nombre");
            criterioCb.Items.Insert(2, "Apellido paterno");
            criterioCb.Items.Insert(3, "Apellido Materno");
            criterioCb.Items.Insert(4, "Calle");
            criterioCb.Items.Insert(5, "Colonia");
            criterioCb.Items.Insert(6, "Código postal");
            criterioCb.Items.Insert(7, "Correo electrónico");
            criterioCb.Items.Insert(8, "Teléfono");
            criterioCb.Items.Insert(9, "Puesto");
            criterioCb.Items.Insert(10, "ID de empleado");
            criterioCb.Items.Insert(11, "Usuario");

            criterioCb.SelectedIndex = 0;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void buscarBtn_Click(object sender, RoutedEventArgs e)
        {
            Servicio.Empleado n = new Servicio.Empleado();
            Servicio.CuentaUsuario c = new Servicio.CuentaUsuario();

            n.IdEmpleado = 82205;
            n.nombre = "Ángel Daniel";
            n.apellidoPaterno = "Sánchez";
            n.apellidoMaterno = "Martínez";
            n.correo = "angelsanchez934@gmail.com";
            n.telefono = "2282739774";
            c.nombreUsuario = "ang0823";
            c.contraseña = "abc123";
            resultsData.Items.Add(n);
            resultsData.Items.Add(c);
        }
    }
}
