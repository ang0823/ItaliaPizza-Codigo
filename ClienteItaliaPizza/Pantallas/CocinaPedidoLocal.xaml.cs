using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace ClienteItaliaPizza.Pantallas
{
    /// <summary>
    /// Lógica de interacción para CocinaPedidoLocal.xaml
    /// </summary>
    public partial class CocinaPedidoLocal : UserControl
    {
        public List<platillo> llenarDataGrid
        {
            set { DataGridPlatillos.ItemsSource = value; }
        }

        public string EditarLabelIDPedido
        {
            get { return labelIDpedido.Content.ToString(); }
            set { labelIDpedido.Content = value; }
        }

        public string EditarLabelTipo
        {
            get { return labelTipo.Content.ToString(); }
            set { labelTipo.Content = value; }
        }

        public string EditarLabelInstrucciones
        {
            get { return labelInstrucciones.Content.ToString(); }
            set { labelInstrucciones.Content = value; }
        }

        public CocinaPedidoLocal()
        {
            InitializeComponent();
        }

        public event EventHandler eventoDataGridPlatillos;
        public event EventHandler eventoNotificarPedidoPreparado;
        private void StackPanelPedidoLocal_Loaded(object sender, RoutedEventArgs e)
        {
            eventoDataGridPlatillos?.Invoke(this, e);
        }

        private void CellChanged_VerificarPlatillosPreparados(object sender, EventArgs e)
        {
            var listaPlatillos = DataGridPlatillos.ItemsSource as List<platillo>;
            if (listaPlatillos.TrueForAll(p => p.preparado == true))
            {
                eventoNotificarPedidoPreparado?.Invoke(this, e);
                FuncionesComunes.MostrarMensajeExitoso("Preparado Listo");
            }            
        }
    }
}
