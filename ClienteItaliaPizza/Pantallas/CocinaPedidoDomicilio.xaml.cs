using ClienteItaliaPizza.Servicio;
using iTextSharp.text;
using System;
using System.Collections.Generic;
using System.Windows.Controls;


namespace ClienteItaliaPizza.Pantallas
{
    /// <summary>
    /// Lógica de interacción para CocinaPedidoLocal.xaml
    /// </summary>
    public partial class CocinaPedidoDomicilio : UserControl
    {
        public Producto[] llenarDatagridDomicilio
        {
            set { DataGridPlatillos.ItemsSource = value; }
        }

        public string EditarLabelIDPedido
        {
            get { return labelIDPedido.Content.ToString(); }
            set { labelIDPedido.Content = value;  }
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

        public CocinaPedidoDomicilio()
        {
            InitializeComponent();
        }
    }
}
