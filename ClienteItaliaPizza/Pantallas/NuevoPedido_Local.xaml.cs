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

        public string RecargarComboBoxNumEmpleado
        {
            set{ comboBoxNumEmpleado.Text = value; }
        }

        /// <summary>
        /// Método en el que accederá la ventana NuevoPedido a este componente
        /// </summary>
        public string EditarSeleccionComboBoxNoMesa
        {
            get { return comboBoxNoMesa.SelectedItem.ToString(); }
            set { comboBoxNoMesa.Items.Add(value); }
        }

        public string RecargarComboBoxNoMesa
        {
            set { comboBoxNumEmpleado.Text = value; }
        }

        /// <summary>
        /// Evento que podrá ser invocado desde otra ventana o UserControl para editar los componentes
        /// </summary>
        public event EventHandler eventSeleccionarNumEmpleado;

        /// <summary>
        /// Evento que podrá ser invocado desde otra ventana o UserControl para editar los componentes
        /// </summary>
        public event EventHandler eventSeleccionarNoMesa;

        public event EventHandler eventLlenarNumEmpleado;
        public event EventHandler eventLlenarNoMesa;

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
            eventSeleccionarNoMesa?.Invoke(this, e);
        }

        /// <summary>
        /// cuando se seleccione una opción se invocará el evento
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComboBoxNumEmpleado_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            eventSeleccionarNumEmpleado?.Invoke(this, e);
        }

        private void ComboBoxNumEmpleado_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            eventLlenarNumEmpleado?.Invoke(this, e);
        }

        private void ComboBoxNoMesa_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            eventLlenarNoMesa?.Invoke(this, e);
        }
    }
}
