using System;
using System.Windows;
using ClienteItaliaPizza.Servicio;


namespace ClienteItaliaPizza
{
    /// <summary>
    /// Lógica de interacción para Principal.xaml
    /// </summary>
    public partial class Principal : Window
    {
        public Principal(CuentaUsuario cuenta)
        {
            InitializeComponent();
            nombreUs.Content = cuenta.nombreUsuario;
        }
    }
}
