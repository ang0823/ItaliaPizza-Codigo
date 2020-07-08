using ClienteItaliaPizza.Servicio;
using ClienteItaliaPizza.Validacion;
using System;
using System.Collections.Generic;
using System.Data;
using System.ServiceModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace ClienteItaliaPizza
{
    /// <summary>
    /// Lógica de interacción para Receta.xaml
    /// </summary>
    public partial class Receta : Window, IRegistrarRecetaCallback, IConsultarInventarioCallback
    {
        protected object recetaExistente; //creo esta clase temporalmente para VALIDAR si la ventna se llama con un objeto receta o no
        protected CuentaUsuario1 cuenta = new CuentaUsuario1();
        InstanceContext context;
        List<string> nombreIngredientesEnDb = new List<string>();

        List<Ingrediente> Ingredientes = new List<Ingrediente>();
        
        public Receta(CuentaUsuario1 cuentaUsuario)
        {            
            InitializeComponent();
            CargarProvisionesEnDb();
            dataGridIngredientes.ItemsSource = Ingredientes;
            cuenta = cuentaUsuario;
            ButtonAceptar.IsEnabled = false;
            EliminarIngredienteBtn.IsEnabled = false;
        }

        public Receta(CuentaUsuario1 cuentausuario, object recetaexistente)
        {
            InitializeComponent();
           // cuenta = cuentausuario;
            recetaExistente = recetaexistente;
        }

        private void TextBoxNombreReceta_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {                      
            if (Validador.validarLetrasConAcentosYNumeros(e.Text) == false)
            {
                e.Handled = true;
            }
        }

        private void TextBoxPorciones_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (Validador.validarSoloNumeros(e.Text) == false )
            {
                e.Handled = true;
            }
        }

        private void TextBoxProcedimiento_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (Validador.validarLetrasConAcentosYNumeros(e.Text) == false)
            {
                e.Handled = true;
            }
        }

        private void ButtonAceptar_Click(object sender, RoutedEventArgs e)
        {
             context = new InstanceContext(this);               
             RegistrarRecetaClient registrarRecetaClient = new RegistrarRecetaClient(context);
            try
            {
             Servicio.Receta receta = new Servicio.Receta();
             receta.nombreReceta = textBoxNombreReceta.Text;
             receta.porciones = FuncionesComunes.ParsearADouble(textBoxPorciones.Text);
             receta.procedimiento = textBoxProcedimiento.Text;
            receta.activado = true;
             
            //receta.Ingrediente = new Ingrediente[1];
            //Ingredientes.CopyTo(receta.Ingrediente);
             var array = Ingredientes.ToArray();
             registrarRecetaClient.RegistrarReceta(receta, array);
            }
            catch (CommunicationException)
            {
                FuncionesComunes.MostrarMensajeDeError("no hay conexion");
            }                         
        }

        private void ButtonCancelar_Click(object sender, RoutedEventArgs e)
        {
            if (recetaExistente != null)
            {
                BuscarReceta ventanaBuscarReceta = new BuscarReceta(cuenta);
                ventanaBuscarReceta.Show();
                this.Close();
            }
            else
            {
     
                Principal ventana = new Principal(cuenta);
                ventana.Show();
                this.Close();
            }
        }

        private void DesabilitarCopy_Paste(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Command == ApplicationCommands.Copy ||
        e.Command == ApplicationCommands.Cut ||
        e.Command == ApplicationCommands.Paste)
            {
                e.Handled = true;
            }
        }
       
        public void RespuestaRR(string mensaje)
        {
            FuncionesComunes.MostrarMensajeExitoso(mensaje);
            //MessageBox.Show(mensaje);
            VaciarCampos();
        }

        private void VaciarCampos()
        {
            textBoxNombreReceta.Text = "";
            textBoxPorciones.Text = "";
            Ingredientes.Clear();
            dataGridIngredientes.Items.Refresh();
            textBoxProcedimiento.Text = "";
        }

        private void DataGridIngredientes_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            // AGREADO POR ÁNGEL
            if (CamposEstanLlenos())
            {
                ButtonAceptar.IsEnabled = true;
            }
            else
            {
                ButtonAceptar.IsEnabled = false;
            }
        }

        private void DataGridIngredientes_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {           
            if (e.EditAction == DataGridEditAction.Commit)
            {
                var n = Ingredientes.Count;
                var columna = e.Column as DataGridBoundColumn;
                if (columna != null)
                {
                    var nombreColumna = (columna.Binding as Binding).Path.Path;
                    if (nombreColumna == "nombre")
                    {
                        var celdaEditada = e.EditingElement as TextBox;

                        if (celdaEditada.Text.Length == 0)
                        {
                            FuncionesComunes.MostrarMensajeDeError("No se aceptan espacios vacios.");
                        }                       
                    }
                    else if(nombreColumna == "cantidad")
                    {
                        var entradaCantidad = e.EditingElement as TextBox;
                        if (entradaCantidad.Text.Length == 0 )
                        {
                            FuncionesComunes.MostrarMensajeDeError("No se aceptan espacios vacios.");
                        }                       
                    }
                    else if(nombreColumna == "peso")
                    {
                        var entradaPeso = e.EditingElement as TextBox;
                        if (entradaPeso.Text.Length == 0)
                        {
                            FuncionesComunes.MostrarMensajeDeError("No se aceptan espacios vacios.");
                        }                       
                    }
                    else if(nombreColumna == "unidad")
                    {
                        var entradaUnidad = e.EditingElement as TextBox;
                        if (entradaUnidad.Text.Length == 0)
                        {
                            FuncionesComunes.MostrarMensajeDeError("No se aceptan espacios vacios.");
                        }                       
                    }
                    else if(nombreColumna == "costoPorUnidad")
                    {
                        var entradaCostoPorUnidad = e.EditingElement as TextBox;
                        if (entradaCostoPorUnidad.Text.Length == 0)
                        {
                            FuncionesComunes.MostrarMensajeDeError("No se aceptan espacios vacios.");
                            
                        }                        
                    }
                    else
                    {
                        FuncionesComunes.MostrarMensajeExitoso("No hay columna seleccionada");
                    }
                }
            }

        }

        private void DataGridIngredientes_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
          if(dataGridIngredientes.CurrentColumn.Header.Equals("Ingrediente"))
            {
                if (Validador.validarLetrasConAcentosYNumeros(e.Text) == false)
                    e.Handled = true;
            }
            if (dataGridIngredientes.CurrentColumn.Header.Equals("Cantidad"))
            {
                if (Validador.validarSoloNumeros(e.Text) == false)
                    e.Handled = true;
            }
            if (dataGridIngredientes.CurrentColumn.Header.Equals("Peso"))
            {
                if (Validador.validarSoloNumerosConPunto(e.Text) == false)
                    e.Handled = true;
                    
            }
            if (dataGridIngredientes.CurrentColumn.Header.Equals("Unidad"))
            {
                if (Validador.validarSoloLetrasConAcentos(e.Text) == false)
                    e.Handled = true;
                
            }
            if (dataGridIngredientes.CurrentColumn.Header.Equals("Costo por Unidad"))
            {
                if (Validador.validarSoloNumerosConPunto(e.Text) == false)
                    e.Handled = true;                
            }
        }

        // ---------------------------AGREADO POR ÁNGEL-----------------------------------------------------------
        private bool CamposEstanLlenos()
        {
            if(textBoxNombreReceta.Text.Length > 0 && textBoxPorciones.Text.Length > 0
                && dataGridIngredientes.Items.Count > 0 && dataGridIngredientes.Items.Count > 1
                && textBoxProcedimiento.Text.Length > 0)
            {
                return true;
            }

            return false;
        }

        private void ActivardesactivarBtnAceptar(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (CamposEstanLlenos())
                {
                    ButtonAceptar.IsEnabled = true;
                }
                else
                {
                    ButtonAceptar.IsEnabled = false;
                }
            }
            catch(Exception)
            {

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

        private void CargarProvisionesEnDb()
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
            foreach (var provision in cuentas)
            {
                ProvisionesDb.Items.Add(provision.nombre);
                nombreIngredientesEnDb.Add(provision.nombre);
            }
        }

        public void RespuestaCI(string mensaje)
        {
            throw new NotImplementedException();
        }

        private void AgregarIngrediente(object sender, SelectionChangedEventArgs e)
        {
            if (!YaSeRegistroIngredienteSeleccionado())
            {
                Ingrediente ingrediente = new Ingrediente();
                ingrediente.nombre = ProvisionesDb.SelectedItem.ToString();
                Ingredientes.Add(ingrediente);
                dataGridIngredientes.Items.Refresh();
            }
        }

        private bool YaSeRegistroIngredienteSeleccionado()
        {
            var nombreProvision = ProvisionesDb.SelectedItem.ToString();

            foreach (var ing in Ingredientes)
            {
                if (ing.nombre == nombreProvision)
                {
                    return true;
                }
            }

            return false;
        }

        private void RemoverIngrediente(object sender, RoutedEventArgs e)
        {
            try
            {
                Ingrediente seleccion = (Ingrediente)dataGridIngredientes.SelectedItem;
                string nombreIng = seleccion.nombre;
                int indiceIngrediente = ObtenerIndiceIngrediente(nombreIng);
                Ingredientes.RemoveAt(indiceIngrediente);
                dataGridIngredientes.Items.Refresh();
            }
            catch(InvalidCastException)
            {
                EliminarIngredienteBtn.IsEnabled = false;
            }
        }

        private int ObtenerIndiceIngrediente(string nombreIngrediente)
        {
            for(int index = 0; index < Ingredientes.Count; index++)
            {
                if (Ingredientes[index].nombre == nombreIngrediente)
                {
                    return index;
                }
            }

            return 0;
        }

        private void ActivarBotonRemover(object sender, SelectionChangedEventArgs e)
        {
            var seleccion = dataGridIngredientes.SelectedItem;

            if(seleccion != null)
            {
                EliminarIngredienteBtn.IsEnabled = true;
            }
            else
            {
                EliminarIngredienteBtn.IsEnabled = false;
            }
        }
    }
}
