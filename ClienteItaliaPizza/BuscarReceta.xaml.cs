using ClienteItaliaPizza.Servicio;
using DevExpress.Mvvm.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ClienteItaliaPizza
{
    /// <summary>
    /// Lógica de interacción para BuscarReceta.xaml
    /// </summary>
    public partial class BuscarReceta : Window, IObtenerRecetasCallback, IConsultarInventarioCallback, IEditarRecetaCallback
    {
        InstanceContext context;
        CuentaUsuario1 cuenta = new CuentaUsuario1();
        Receta1 receta = new Receta1();
        List<Ingrediente1> ingredientes = new List<Ingrediente1>();
        List<Ingrediente1> copiaIngredientes = new List<Ingrediente1>();
        bool enEdicion = false;

        //Constructor para pruebas, se debe eliminar
        public BuscarReceta()
        {
            InitializeComponent();
            DeshabiliarCamposYBotones();
            OcultarIngredientes();
        }

        public BuscarReceta(CuentaUsuario1 cuentaUsuario)
        {
            InitializeComponent();
            DeshabiliarCamposYBotones();
            OcultarIngredientes();

            cuenta = cuentaUsuario;
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

        private void SearchBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Return && SearchBox.Text.Length > 0)
            {
                ObtenerRecetaDesdeDb();
            }
        }

        private void SearchBtn_Click(object sender, RoutedEventArgs e)
        {
            if (SearchBox.Text.Length > 0)
            {
                ObtenerRecetaDesdeDb();
            }
        }

        private void ObtenerRecetaDesdeDb()
        {
            context = new InstanceContext(this);
            ObtenerRecetasClient servicioReceta = new ObtenerRecetasClient(context);

            try
            {
                string nombreReceta = SearchBox.Text;
                SearchBox.Text = "";
                servicioReceta.ObtenerReceta(nombreReceta);
            }
            catch (EndpointNotFoundException)
            {
                FuncionesComunes.MostrarMensajeDeError("El servidor no sstá disponible.");
            }
            catch (TimeoutException)
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
            foreach (var ingrediente in ingredientes)
            {
                this.ingredientes.Add(ingrediente);
            }

            this.ingredientes = this.ingredientes.OrderBy(item => item.nombre).ToList();
            RealizarCopiaDeIngredientes();
            EstablecerInfoReceta();
            HabiliarBotonesDeEdicion();
        }

        private void RealizarCopiaDeIngredientes()
        {
            foreach(var ingre in ingredientes)
            {
                copiaIngredientes.Add(ingre);
            }
        }

        private void EstablecerInfoReceta()
        {
            NombreRecetaTxt.Text = receta.nombreReceta;
            PorcionesTxt.Text = receta.porciones.ToString();
            EstablecerIngredientesDeReceta();
            textBoxProcedimiento.Text = receta.procedimiento;
        }

        private void EstablecerIngredientesDeReceta()
        {
            dataGridIngredientes.ItemsSource = ingredientes;
            dataGridIngredientes.Items.Refresh();
        }

        private void HabiliarBotonesDeEdicion()
        {
            ButtonEditarGuardar.IsEnabled = true;
            ButtonEliminar.IsEnabled = true;
        }

        // Callback llamado cuando hay un error al buscar receta
        public void RespuestaIOR(string mensaje)
        {
            FuncionesComunes.MostrarMensajeDeError(mensaje);
        }

        private void ButtonEditarGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (enEdicion)
            {
                ButtonEditarGuardar.Content = "Edtar";
                ActualizarInfoReceta();
            }
            else
            {
                ButtonEditarGuardar.Content = "Guardar";
                MostrarIngredientes();
                HabilitarEdicion();
                enEdicion = true;
            }
        }

        private void ActualizarInfoReceta()
        {
            if (InfoIngredientesCompleta())
            {
                Servicio.Receta recetaModificada = new Servicio.Receta();
                List<Ingrediente> ingredientesModificados = new List<Ingrediente>();

                context = new InstanceContext(this);
                EditarRecetaClient servicioReceta = new EditarRecetaClient(context);

                try
                {
                    ActualizarRecetaLocal(ref recetaModificada);
                    ActualizarIngredietesDeRecetaLocal(ref ingredientesModificados);
                    servicioReceta.EditarReceta(recetaModificada, ingredientesModificados.ToArray());
                }
                catch (FormatException)
                {
                    FuncionesComunes.MostrarMensajeDeError("El número de porciones es inválido");
                }
            }
            else
            {
                FuncionesComunes.MostrarMensajeDeError("Es necesario completar la información de los ingredientes para continnuar.");
            }
        }

        private bool InfoIngredientesCompleta()
        {
            foreach (var ingrediente in ingredientes)
            {
                if (ingrediente.cantidad == 0 ||
                    ingrediente.peso == "" ||
                    ingrediente.unidad == "" ||
                    ingrediente.costoPorUnidad == 0
                    )
                {
                    return false;
                }
            }

            return true;
        }

        private void ActualizarRecetaLocal(ref Servicio.Receta recetaEntrante)
        {
            try
            {
                recetaEntrante.id = receta.id;
                recetaEntrante.nombreReceta = NombreRecetaTxt.Text;
                recetaEntrante.porciones = double.Parse(PorcionesTxt.Text);
                recetaEntrante.procedimiento = textBoxProcedimiento.Text;
            }
            catch (FormatException)
            {
                throw new FormatException();
            }
        }

        private void ActualizarIngredietesDeRecetaLocal(ref List<Ingrediente> ingredientes)
        {
            foreach (var ingredient in this.ingredientes)
            {
                Ingrediente nuevo = new Ingrediente();
                nuevo.Id = ingredient.id;
                nuevo.nombre = ingredient.nombre;
                nuevo.cantidad = ingredient.cantidad;
                nuevo.peso = ingredient.peso;
                nuevo.unidad = ingredient.unidad;
                nuevo.costoPorUnidad = ingredient.costoPorUnidad;
                ingredientes.Add(nuevo);
            }
        }

        public void RespuestaER(string mensaje)
        {
            if (mensaje == "Se modificó correctamente")
            {
                string nombreRecetaModificada = NombreRecetaTxt.Text;
                VaciarCampos();
                DeshabilitarEdicion();
                RecargarReceta(nombreRecetaModificada);
                FuncionesComunes.MostrarMensajeExitoso(mensaje);
                enEdicion = false;
            }
            else
            {
                FuncionesComunes.MostrarMensajeDeError(mensaje);
            }
        }

        private void VaciarCampos()
        {
            ingredientes.Clear();
            NombreRecetaTxt.Text = string.Empty;
            PorcionesTxt.Text = string.Empty;
            dataGridIngredientes.Items.Refresh();
            textBoxProcedimiento.Text = string.Empty;
        }

        private void RecargarReceta(string nombreReceta)
        {
            context = new InstanceContext(this);
            ObtenerRecetasClient servicioReceta = new ObtenerRecetasClient(context);

            try
            {
                servicioReceta.ObtenerReceta(nombreReceta);
            }
            catch (EndpointNotFoundException)
            {
                FuncionesComunes.MostrarMensajeDeError("El servidor no sstá disponible.");
            }
            catch (TimeoutException)
            {
                FuncionesComunes.MostrarMensajeDeError("Se excedio el tiempo de espera para la respuesta.");
            }
            catch (Exception e)
            {
                FuncionesComunes.MostrarMensajeDeError(e.GetType() + ": " + e.Message);
            }
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

            nombresIngredientes.Sort();
            CargarIngredientesEnListBox(nombresIngredientes);
        }

        private void CargarIngredientesEnListBox(List<string> ingredientes)
        {
            foreach (var ingrediente in ingredientes)
            {
                ingredientesList.Items.Add(ingrediente);
            }
        }

        // Este metodo es un callback cuando hay error al recuperar el inventario
        public void RespuestaCI(string mensaje)
        {
            FuncionesComunes.MostrarMensajeDeError(mensaje);
        }

        private void HabilitarEdicion()
        {
            NombreRecetaTxt.IsEnabled = true;
            PorcionesTxt.IsEnabled = true;
            dataGridIngredientes.IsEnabled = true;
            textBoxProcedimiento.IsEnabled = true;
        }

        private void ButtonEliminar_Click(object sender, RoutedEventArgs e)
        {
            FuncionesComunes.MostrarMensajeDeError("Aún no se implementa.");
        }

        private void ButtonRegresar_Click(object sender, RoutedEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                if (enEdicion)
                {
                    string titulo = "Descartar cambios";
                    string pregunta = "¿Seguro que desea descartar los cambios?";
                    bool confirmado = FuncionesComunes.ConfirmarOperacion(titulo, pregunta);
                    if (confirmado)
                    {
                        ButtonEditarGuardar.Content = "Editar";
                        RestablecerListaDeIngredientes();
                        EstablecerInfoReceta();
                        OcultarIngredientes();
                        DeshabilitarEdicion();
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

        private void RestablecerListaDeIngredientes()
        {
            ingredientes.Clear();
            foreach (var copiaIngrediente in copiaIngredientes)
            {
                ingredientes.Add(copiaIngrediente);
            }
        }

        private void DeshabilitarEdicion()
        {
            NombreRecetaTxt.IsEnabled = false;
            PorcionesTxt.IsEnabled = false;
            dataGridIngredientes.IsEnabled = false;
            textBoxProcedimiento.IsEnabled = false;
        }

        

        private bool YaSeRegistroIngredienteSeleccionado()
        {
            bool ingredienteRegistrado = false;

            if(ingredientesList.Items.Count > 0)
            {
                var nombreProvision = ingredientesList.SelectedItem.ToString();
                
                foreach (var ing in ingredientes)
                {
                    if (ing.nombre == nombreProvision)
                    {
                        ingredienteRegistrado = true;
                    }
                }

            }
            else
            {
                ingredienteRegistrado = true;
            }

            return ingredienteRegistrado;
        }

        private void LogoutBtn_Click(object sender, RoutedEventArgs e)
        {

            string titulo = "Cerrar sesión";
            string pregunta = "¿Seguro que deseas cerrar la sesión?";
            bool opcion = FuncionesComunes.ConfirmarOperacion(titulo, pregunta);
            
            if (opcion)
            {
                FuncionesComunes.CerrarSesion();
                this.Close();
            }
        }

         private void RemoverIngrediente(object sender, RoutedEventArgs e)
        {
                try
                {
                    Ingrediente1 seleccion = (Ingrediente1)dataGridIngredientes.SelectedItem;
                    string nombreIng = seleccion.nombre;
                    int indiceIngrediente = ObtenerIndiceIngrediente(nombreIng);
                    ingredientes.RemoveAt(indiceIngrediente);
                    dataGridIngredientes.Items.Refresh();
                }
                catch (InvalidCastException)
                {
                    removerBtn.IsEnabled = false;
                }
        }

        private int ObtenerIndiceIngrediente(string nombreIngrediente)
        {
            for (int index = 0; index < ingredientes.Count; index++)
            {
                if (ingredientes[index].nombre == nombreIngrediente)
                {
                    return index;
                }
            }

            return 0;
        }

        private void ActivarDesactivarBotonGuardar(object sender, TextChangedEventArgs e)
        {
            if (CamposLlenos())
            {
                ButtonEditarGuardar.IsEnabled = true;
            }
            else
            {
                ButtonEditarGuardar.IsEnabled = false;
            }
        }

        private bool CamposLlenos()
        {
            if(NombreRecetaTxt.Text.Length > 0 && PorcionesTxt.Text.Length > 0
                && ingredientes.Count > 0 && textBoxProcedimiento.Text.Length > 0)
            {
                return true;
            }

            return false;
        }

        private void dataGridIngredientes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CamposLlenos())
            {
                ButtonEditarGuardar.IsEnabled = true;
            }
            else
            {
                ButtonEditarGuardar.IsEnabled = false;
            }
        }
        
        public void DevuelveRecetas(Receta1[] recetas)
        {
            throw new NotImplementedException();
        }

        private void ingredientesList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (!YaSeRegistroIngredienteSeleccionado())
            {
                Ingrediente1 ingrediente = new Ingrediente1();
                ingrediente.nombre = ingredientesList.SelectedItem.ToString();
                ingredientes.Add(ingrediente);
                dataGridIngredientes.Items.Refresh();
            }
        }
    }
}
