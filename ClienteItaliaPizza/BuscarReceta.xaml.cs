using ClienteItaliaPizza.Servicio;
using DevExpress.Mvvm.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Printing;
using System.ServiceModel;
using System.Windows;
using System.Windows.Input;

namespace ClienteItaliaPizza
{
    /// <summary>
    /// Lógica de interacción para BuscarReceta.xaml
    /// </summary>
    public partial class BuscarReceta : Window, IObtenerRecetasCallback, IConsultarInventarioCallback
    {
        CuentaUsuario1 cuenta = new CuentaUsuario1();
        Receta1 receta = new Receta1();
        List<Ingrediente1> ingredientes = new List<Ingrediente1>();
        bool enEdicion = false;

        public BuscarReceta(CuentaUsuario1 cuentaUsuario)
        {
            InitializeComponent();
            DeshabiliarCamposYBotones();
            cuenta = cuentaUsuario;

            SelectLbl.Visibility = Visibility.Hidden;
            ingredientesList.Visibility = Visibility.Hidden;
            removerBtn.Visibility = Visibility.Hidden;
            //telerik.Windows.Controls.dll;
            //aquí llamaré al servicio para obtener todas las recetas y mostrarlas en el datagrid

        }

        private void DeshabiliarCamposYBotones()
        {
            NombreRecetaTxt.IsEnabled = false;
            PorcionesTxt.IsEnabled = false;
            dataGridIngredientes.IsEnabled = false;
            textBoxProcedimiento.IsEnabled = false;
            ButtonEditarGuardar.IsEnabled = false;
            ButtonEliminar.IsEnabled = false;
        }

        private void HabiliarCamposYBotones()
        {
            NombreRecetaTxt.IsEnabled = true;
            PorcionesTxt.IsEnabled = true;
            dataGridIngredientes.IsEnabled = true;
            textBoxProcedimiento.IsEnabled = true;
            ButtonEditarGuardar.IsEnabled = true;
            ButtonEliminar.IsEnabled = true;
        }

        private void HabiliarBotonesDeEdicion()
        {
            ButtonEditarGuardar.IsEnabled = true;
            ButtonEliminar.IsEnabled = true;
        }

        private void DeshabiliarBotonesDeEdicion()
        {
            ButtonEditarGuardar.IsEnabled = false;
            ButtonEliminar.IsEnabled = false;
        }

        private void ButtonRegresar_Click(object sender, RoutedEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                if (enEdicion)
                {
                    bool confirmado = FuncionesComunes.ConfirmarOperacion("Descartar cambios", "¿Seguro que desea descartar los cambios?");
                    if (confirmado)
                    {
                        ButtonEditarGuardar.Content = "Editar";
                        DeshabiliarCamposYBotones();
                        OcultarIngredientes();
                        EstablecerInfoReceta();
                        enEdicion = false;
                    }
                }
                else
                {
                    Principal ventana = new Principal(cuenta);
                    ventana.Show();
                    this.Close();
                }
            });
        }

        private void ButtonEditarGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (enEdicion)
            {
                ButtonEditarGuardar.Content = "Edtar";
                OcultarIngredientes();
                DeshabiliarCamposYBotones();
                enEdicion = false;
            }
            else
            {
                ButtonEditarGuardar.Content = "Guardar";
                MostrarIngredientes();
                HabiliarCamposYBotones();
                enEdicion = true;
            }
        }

        private void OcultarIngredientes()
        {
            SelectLbl.Visibility = Visibility.Hidden;
            ingredientesList.Visibility = Visibility.Hidden;
            removerBtn.Visibility = Visibility.Hidden;
            VaciarListaIngredientes();
        }

        private void VaciarListaIngredientes()
        {
            ingredientesList.Items.Clear();
        }

        private void MostrarIngredientes()
        {
            CargarProvisionesDesdeDb();
            SelectLbl.Visibility = Visibility.Visible;
            ingredientesList.Visibility = Visibility.Visible;
            removerBtn.Visibility = Visibility.Visible;
        }

        private void CargarProvisionesDesdeDb()
        {
            InstanceContext context = new InstanceContext(this);
            ConsultarInventarioClient servicioInventario = new ConsultarInventarioClient(context);

            try
            {
                servicioInventario.ConsultarInventario();
            }
            catch (EndpointNotFoundException)
            {
                FuncionesComunes.MostrarMensajeDeError("No se pudo recuperar los ingredientes en DB");
            }
        }

        public void DevuelveInventario(Provision[] cuentas)
        {
            List<string> nombresIngredientes = new List<string>();
            foreach (var provision in cuentas)
            {
                nombresIngredientes.Add(provision.nombre);
            }

            CargarIngredientesEnListBox(nombresIngredientes);
        }

        private void CargarIngredientesEnListBox(List<string> ingredientes)
        {
            foreach (var ingrediente in ingredientes)
            {
                ingredientesList.Items.Add(ingrediente);
            }
        }

        private void ButtonEliminar_Click(object sender, RoutedEventArgs e)
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

        // -----------------------------------AGREGADO POR ANGEL---------------------------------
        public void RespuestaCI(string mensaje)
        {
            FuncionesComunes.MostrarMensajeDeError(mensaje);
        }

        private void SearchBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Return && SearchBox.Text.Length > 0)
            {
                ObtenerRcetaDesdeDb();
            }
        }

        private void SearchBtn_Click(object sender, RoutedEventArgs e)
        {
            if(SearchBox.Text.Length > 0)
            {
                ObtenerRcetaDesdeDb();
            }
        }

        private void ObtenerRcetaDesdeDb()
        {
            InstanceContext context = new InstanceContext(this);
            ObtenerRecetasClient servicioReceta = new ObtenerRecetasClient(context);

            try
            {
                string nombreReceta = SearchBox.Text;
                SearchBox.Text = "";
                servicioReceta.ObtenerReceta(nombreReceta);
            }
            catch(EndpointNotFoundException)
            {
                FuncionesComunes.MostrarMensajeDeError("El servidor no sstá disponible.");
            }
            catch(TimeoutException)
            {
                FuncionesComunes.MostrarMensajeDeError("Se excedio el tiempo de espera para la respuesta.");
            }
            catch (Exception e)
            {
                FuncionesComunes.MostrarMensajeDeError(e.GetType() + ": " + e.Message);
            }
        }

        public void DevuelveReceta(Receta1 receta, Ingrediente1[] ingredientes)
        {
            this.receta = receta;
            foreach(var ingrediente in ingredientes)
            {
                this.ingredientes.Add(ingrediente);
            }

            this.ingredientes = this.ingredientes.OrderBy(item => item.nombre).ToList();
            EstablecerInfoReceta();
        }

        private void EstablecerInfoReceta()
        {
            NombreRecetaTxt.Text = receta.nombreReceta;
            PorcionesTxt.Text = receta.porciones.ToString();
            EstablecerIngredientesDeReceta();
            textBoxProcedimiento.Text = receta.procedimiento;
            HabiliarBotonesDeEdicion();
        }

        private void EstablecerIngredientesDeReceta()
        {
            dataGridIngredientes.ItemsSource = ingredientes;
            dataGridIngredientes.Items.Refresh();
        }

        public void DevuelveRecetas(Receta1[] recetas)
        {
            throw new NotImplementedException();
        }

        public void RespuestaIOR(string mensaje)
        {
            FuncionesComunes.MostrarMensajeDeError(mensaje);
        }
    }
}
