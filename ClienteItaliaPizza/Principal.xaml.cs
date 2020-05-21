﻿using System;
using System.Windows;
using ClienteItaliaPizza.Pantallas;
using ClienteItaliaPizza.Servicio;


namespace ClienteItaliaPizza
{
    /// <summary>
    /// Lógica de interacción para Principal.xaml
    /// </summary>
    public partial class Principal : Window
    {
        CuentaUsuario CuentaUsuario;
        public Principal(/*CuentaUsuario cuenta*/)
        {
            InitializeComponent();
            //CuentaUsuario = cuenta;
            //nombreUs.Content = CuentaUsuario.nombreUsuario;
        }

        private void MostrarBuscarEmpledosGui()
        {
            //BuscarEmpleados ventana = new BuscarEmpleados(CuentaUsuario);
            BuscarEmpleados ventana = new BuscarEmpleados(); //Esto se debe quitar
            ventana.Show();
            this.Close();
        }

        private void MostrarBusdarIngredienteGui()
        {
            //BuscarIngrediente ventana = new BuscarIngrediente(CuentaUsuario);
            BuscarIngrediente ventana = new BuscarIngrediente();
            ventana.Show();
            this.Close();
        }

        private void MostrarBuscarProductosGui()
        {
            BuscarProductos ventana = new BuscarProductos(CuentaUsuario);
            ventana.Show();
            this.Close();
        }

        private void MostrarRegistroEmpleadosGui()
        {
            RegistroEmpleados RegistroEmpleadosGui = new RegistroEmpleados(/*CuentaUsuario*/);
            RegistroEmpleadosGui.Show();
            this.Close();
        }
        private void MostrarRegistroIngredientesGui()
        {
            RegistroIngredientes RegistroIngredientesGui = new RegistroIngredientes();
            RegistroIngredientesGui.Show();
            this.Close();
        }

        private void MostrarRegistroProductosGui(object sender, RoutedEventArgs e)
        {
            RegistroProductos RegistroProdcutosGui = new RegistroProductos();
            RegistroProdcutosGui.Show();
            this.Close();
        }

        private void registrarEmpBtn_Click(object sender, RoutedEventArgs e)
        {
            MostrarRegistroEmpleadosGui();
        }

        private void buscarEmpBtn_Click(object sender, RoutedEventArgs e)
        {
            MostrarBuscarEmpledosGui();
        }

        private void CerrarSesionBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult opcion;

            opcion = MessageBox.Show("¿Seguro que deseas cerrar la sesión?", "Cerrar sesión",
                    MessageBoxButton.OKCancel, MessageBoxImage.Question);

            if (opcion == MessageBoxResult.OK)
            {
                FuncionesComunes.CerrarSesion();
                this.Close();
            }
        }

        private void BuscarProductosBtn_Click(object sender, RoutedEventArgs e)
        {
            MostrarBuscarProductosGui();
        }

        private void ButtonRegistrarReceta_Click(object sender, RoutedEventArgs e)
        {
            Receta ventanaRegistroReceta = new Receta(CuentaUsuario);
            ventanaRegistroReceta.Show();
            this.Close();
        }

        private void ButtonGenerarInventario_Click(object sender, RoutedEventArgs e)
        {
            Inventario ventanaInvventario = new Inventario(CuentaUsuario);
            ventanaInvventario.Show();
            this.Close();
        }

        private void ButtonBuscarReceta_Click(object sender, RoutedEventArgs e)
        {
            BuscarReceta ventanaBuscarReceta = new BuscarReceta(CuentaUsuario);
            ventanaBuscarReceta.Show();
            this.Close();
        }
        
        private void BuscarIngedienteBtn_Click(object sender, RoutedEventArgs e)
        {
            MostrarBusdarIngredienteGui();
        }

        private void RegistrarIngredienteBtn_Click(object sender, RoutedEventArgs e)
        {
            MostrarRegistroIngredientesGui();
        }
    }
}
