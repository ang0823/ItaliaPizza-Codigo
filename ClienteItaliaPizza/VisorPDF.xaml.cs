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
    /// Lógica de interacción para VisorPDF.xaml
    /// </summary>
    public partial class VisorPDF : Window
    {
        public VisorPDF(string rutaPDF)
        {
            InitializeComponent();
            pdfDX.OpenDocument(@rutaPDF);
        }

        private void PdfDX_Loaded(object sender, RoutedEventArgs e)
        {
          // pdfDX. .OpenDocument("C:/Users/survi/Documents/ItaliaPizza-Codigo/ClienteItaliaPizza/bin/Debug/hola.pdf");

        }

        private void pdfDX_PrintPage(DependencyObject d, DevExpress.Xpf.PdfViewer.PdfPrintPageEventArgs e)
        {            

            PrintDialog dialogoImprimir = new PrintDialog();
            var respuesta = dialogoImprimir.ShowDialog();

            if (respuesta == true)
            {
                
            }
        }

        /* private void VisorDePDFControl_Loaded(object sender, RoutedEventArgs e)
         {
             visorDePDFControl.Load("C:/Users/survi/Documents/ItaliaPizza-Codigo/ClienteItaliaPizza/bin/Debug/hola.pdf");
         }*/
    }
}
