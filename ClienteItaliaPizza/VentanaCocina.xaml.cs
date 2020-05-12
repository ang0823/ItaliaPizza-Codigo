using System;
using System.Collections;
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
    /// Lógica de interacción para VentanaCocina.xaml
    /// </summary>
    public partial class VentanaCocina : Window
    {
        public VentanaCocina()
        {
            InitializeComponent();
        }

        private void dataGridPlatillos_Loaded(object sender, RoutedEventArgs e)
        {
            /*
             * Esto es una prueba de carga de elementos en eL data grid
             * */
            
           
           // CheckBox casillaListo = new CheckBox();
            //DataGridPlatillos.Items.Add(casillaListo);
        }

        private void ButtonPedidoDomicilioListo_Click(object sender, RoutedEventArgs e)
        {
            ArrayList arrayList = new ArrayList();
            arrayList.Add("Bolillo");
            arrayList.Add("Camote");

            foreach (var platillo in arrayList)
            {
                DataGridPlatillos.Items.Add(platillo);
                DataGridPlatillos.ScrollIntoView(DataGridPlatillos.Items[DataGridPlatillos.Items.Count - 1]);
                
            }

        }
    }
}
