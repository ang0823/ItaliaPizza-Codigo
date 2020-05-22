﻿using System;
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

namespace ClienteItaliaPizza.Pantallas
{
    /// <summary>
    /// Lógica de interacción para VentanaPedidos.xaml
    /// </summary>
    public partial class VentanaPedidos : Window
    {
        
        public VentanaPedidos(string tipoUsuario)
        {
            InitializeComponent();

            if(tipoUsuario == "CallCenter")
            {
                MeserosUC meserosUC = new MeserosUC("CallCenter");
                gridpedidos.Children.Add(meserosUC);
                meserosUC.Visibility = Visibility.Visible;
            }
            if(tipoUsuario== "Mesero")
            {
                MeserosUC meserosUC = new MeserosUC("Mesero");
                gridpedidos.Children.Add(meserosUC);
                meserosUC.Visibility = Visibility.Visible;
            }
            
        }

        private void ButtonSalirClick(object sender, RoutedEventArgs e)
        {
            Dispatcher.Invoke(() =>
             {
                 MainWindow ventana = new MainWindow();
                 ventana.Show();
                 this.Close();
             });
        }
    }
}
