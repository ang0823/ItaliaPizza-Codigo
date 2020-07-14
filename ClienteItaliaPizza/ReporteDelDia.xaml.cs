using ClienteItaliaPizza.Servicio;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
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

    public partial class ReporteDelDia : Window, IGenerarReporteDelDiaCallback
    {
        private CuentaUsuario1 cuentaUsuario;
        InstanceContext contexto;

        public ReporteDelDia()
        {
            InitializeComponent();
            try
            {
                contexto = new InstanceContext(this);
                GenerarReporteDelDiaClient cliente = new GenerarReporteDelDiaClient(contexto);
                cliente.ObtenerReporteDelDia();
            }
            catch (CommunicationException)
            {
                FuncionesComunes.MostrarMensajeDeError("Error de comunicación con el servidor");
            }
        }

        public ReporteDelDia(CuentaUsuario1 cuentaUsuario)
        {
            this.cuentaUsuario = cuentaUsuario;
        }

        public void DevuelveReporte(Reporte[] reportes)
        {
            double total = 0;
            List<Reporte> reportes1 = new List<Reporte>();
            ReporteDelDia1 reportes2 = new ReporteDelDia1();

            for(int i=0; i<reportes.Length; i++)
            {
                reportes1.Add(new Reporte() { IdPedido = reportes[i].IdPedido, fecha = reportes[i].fecha, totalCuenta = reportes[i].totalCuenta, nombreEmpleado = reportes[i].nombreEmpleado });
                total = total + reportes[i].totalCuenta;

                if (i == reportes.Length-1)
                {
                    Reporte r = new Reporte();
                    reportes1.Add(new Reporte() { fecha = reportes[i].fecha, nombreEmpleado = "Total " + total.ToString()});
                }
            }

            /*foreach (var valor in reportes)
            {
                reportes1.Add(new Reporte() { IdPedido = valor.IdPedido, fecha = valor.fecha, totalCuenta = valor.totalCuenta, nombreEmpleado = valor.nombreEmpleado });
                total = total + valor.totalCuenta;
            }*/



            this.Dispatcher.Invoke(() =>
            {
                lvReportes.ItemsSource = reportes1;
            });
        }

        public void Imprimir()
        {
            PrintDialog dialogoImprimir = new PrintDialog();
            var respuesta = dialogoImprimir.ShowDialog();

            if (respuesta == true)
            {
                dialogoImprimir.PrintVisual(lvReportes, "Imprimiendo_WPF"); //Objeto visual a imprimir y descripción de la impresión               
                
            }
        }

        public void RespuestaReporteDelDia(string mensaje)
        {
            FuncionesComunes.MostrarMensajeDeError(mensaje);
        }

        private void VolverMenu(object sender, RoutedEventArgs e)
        {
            Principal ventana = new Principal(cuentaUsuario);
            ventana.Show();
            this.Close();
        }

        private void buttonImprimirReporte_Click(object sender, RoutedEventArgs e)
        {
            Imprimir();
        }
    }
}
