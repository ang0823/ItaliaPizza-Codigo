using ClienteItaliaPizza.Servicio;
using ClienteItaliaPizza.Validacion;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
using System.ServiceModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Drawing.Imaging;
using System.Windows.Xps.Packaging;
using System.Windows.Documents;
using System.IO;

namespace ClienteItaliaPizza
{
    /// <summary>
    /// Lógica de interacción para Inventario.xaml
    /// </summary>
    public partial class Inventario : Window, IConsultarInventarioCallback, IEditarIngredienteCallback
    {
        CuentaUsuario1 cuenta = new CuentaUsuario1();
        InstanceContext contexto;

        public Inventario()
        {
            InitializeComponent();
            //cuenta = cuentaUsuario;
            try
            {
                contexto = new InstanceContext(this);
                ConsultarInventarioClient servicioInventario = new ConsultarInventarioClient(contexto);
                servicioInventario.ConsultarInventario();
            }
            catch (CommunicationException)
            {
                FuncionesComunes.MostrarMensajeDeError("Error de comunicación con el servidor");
            }
        }

        private void ButtonRegresar_Click(object sender, RoutedEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                Principal ventana = new Principal(cuenta);
                ventana.Show();
                this.Close();
            });
        }

        private void ButtonImprimir_Click(object sender, RoutedEventArgs e)
        {           
            PrintDialog dialogoImprimir = new PrintDialog();                                
            var respuesta = dialogoImprimir.ShowDialog();

            if (respuesta == true)
            {
                //dialogoImprimir.PrintVisual(this.dataGridInventario, "Imprimiendo_WPF"); //Objeto visual a imprimir y descripción de la impresión               
                 XpsDocument xpsDocument = new XpsDocument("C://FixedDocumentSequence.xps", FileAccess.ReadWrite);
                 FixedDocumentSequence fixedDocSeq = xpsDocument.GetFixedDocumentSequence();
                 dialogoImprimir.PrintDocument(fixedDocSeq.DocumentPaginator, "Test print job");
            }          
             /* var seleccion = dataGridInventario.SelectedCells[0].Item as Provision;
              var Original = seleccion.noExistencias;

              var cellInfo = dataGridInventario.SelectedCells[6];
              var content = (cellInfo.Column.GetCellContent(cellInfo.Item) as TextBlock).Text;

              MessageBox.Show("Original: " + Original + " Nuevo: " + content);*/
        }

        private void DataGridInventario_CellEditEnding(object sender, System.Windows.Controls.DataGridCellEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit)
            {
                var columna = e.Column as DataGridBoundColumn;
                if (columna != null)
                {
                    var bindingPath = (columna.Binding as Binding).Path.Path;
                    if (bindingPath == "noExistencias")
                    {
                        var celdaEditada = e.EditingElement as TextBox;
                        if (celdaEditada.Text.Contains(" "))
                        {
                            FuncionesComunes.MostrarMensajeDeError("No se han guardado los cambios.\n No se aceptan espacios vacios.");
                        }
                        else
                        {
                            Provision provisionEditada = dataGridInventario.SelectedCells[0].Item as Provision;
                            provisionEditada.activado = true;
                            provisionEditada.noExistencias = FuncionesComunes.ParsearAEntero(celdaEditada.Text);

                            contexto = new InstanceContext(this);
                            EditarIngredienteClient serverEditarIngrediente = new EditarIngredienteClient(contexto);
                            serverEditarIngrediente.Editar(provisionEditada); 
                        }
                    }
                }
            }

        }

        public void RespuestaCI(string mensaje)
        {
            FuncionesComunes.MostrarMensajeDeError(mensaje);
        }

        private void DataGridInventario_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {            
            if (Validador.validarSoloNumeros(e.Text) == false)
            {
                e.Handled = true;
            }
            e.Text.Trim();
        }

        public void RespuestaEditarIngrediente(string mensajeError)
        {
            if (mensajeError == "Cambios Guardados")
            {
                FuncionesComunes.MostrarMensajeExitoso(mensajeError);
            }
            else
            {
                FuncionesComunes.MostrarMensajeDeError(mensajeError);
            }
        }

        public void DevuelveInventario(Provision[] provisiones)
        {
            if (provisiones.Length != 0)
            {
                dataGridInventario.ItemsSource = provisiones;
            }
            else
            {
                FuncionesComunes.MostrarMensajeDeError("El inventario está vacío, no existen provisiones");
            }
        }

       
    }
}
