using System;
using System.Windows.Controls;


namespace ClienteItaliaPizza.Pantallas
{
    /// <summary>
    /// Lógica de interacción para NuevoPedido_Local.xaml
    /// </summary>
    public partial class NuevoPedido_Local : UserControl
    {
        /// <summary>
        /// Método en el que accederá la ventana NuevoPedido a este componente
        /// </summary>
        public string EditarSeleccionComboBoxNumEmpleado
        {
            get { return comboBoxNumEmpleado.SelectedItem.ToString(); }
            set { comboBoxNumEmpleado.Items.Add(value); }
        }

        /// <summary>
        /// Método en el que accederá la ventana NuevoPedido a este componente
        /// </summary>
        public string EditarSeleccionComboBoxNoMesa
        {
            get { return comboBoxNoMesa.SelectedItem.ToString(); }
            set { comboBoxNoMesa.Items.Add(value); }
        }

        /// <summary>
        /// Evento que podrá ser invocado desde otra ventana o UserControl para editar los componentes
        /// </summary>
        public event EventHandler eventEditarNumEmpleado;

        /// <summary>
        /// Evento que podrá ser invocado desde otra ventana o UserControl para editar los componentes
        /// </summary>
        public event EventHandler eventEditarNoMesa;

        public NuevoPedido_Local()
        {
            InitializeComponent();
        }    

        /// <summary>
        /// Cuando se seleccione una opción se invocará el evento
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComboBoxNoMesa_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            eventEditarNoMesa?.Invoke(this, e);
        }

        /// <summary>
        /// cuando se seleccione una opción se invocará el evento
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComboBoxNumEmpleado_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            eventEditarNumEmpleado?.Invoke(this, e);
        }
    }
}
