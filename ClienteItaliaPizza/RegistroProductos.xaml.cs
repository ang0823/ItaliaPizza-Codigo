﻿using ClienteItaliaPizza.Servicio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MessageBox = System.Windows.MessageBox;

namespace ClienteItaliaPizza
{
    /// <summary>
    /// Lógica de interacción para RegistroProductos.xaml
    /// </summary>
    public partial class RegistroProductos : Window
    {
        CuentaUsuario CuentaUsuario;
        public RegistroProductos(CuentaUsuario cuenta)
        {
            CuentaUsuario = cuenta;
            InitializeComponent();
            IniciarUnidadesCb();

            UsuarioLbl.Content = cuenta.nombreUsuario;
            GuardarBtn.IsEnabled = false;
            VaciarBtn.IsEnabled = false;
        }

        private Boolean AlgunCampoLleno()
        {
            if (nombreTxt.Text.Length > 0 || PrecioTxt.Text.Length > 0 || MinimoTxt.Text.Length > 0
                || UnidadCb.SelectedIndex != 0 || ActualTxt.Text.Length > 0 || UbicacionTxt.Text.Length > 0
                || CodigoTxt.Text.Length > 0 || DescripcionTxt.Text.Length > 0 || RestriccionesTxt.Text.Length > 0)
            {
                return true;
            }

            return false;
        }

        private Boolean CamposLlenos()
        {
            if (nombreTxt.Text.Length > 0 && PrecioTxt.Text.Length > 0 && MinimoTxt.Text.Length > 0
                && UnidadCb.SelectedIndex != 0 && ActualTxt.Text.Length > 0 && UbicacionTxt.Text.Length > 0
                && CodigoTxt.Text.Length > 0 && DescripcionTxt.Text.Length > 0 && RestriccionesTxt.Text.Length > 0)
            {
                return true;
            }

            return false;
        }

        private void CerrarSesion()
        {
            Dispatcher.Invoke(() =>
            {
                MainWindow ventana = new MainWindow();
                ventana.Show();
                this.Close();
            });
        }

        private void IniciarUnidadesCb()
        {
            UnidadCb.Items.Insert(0, "Unidad");
            UnidadCb.Items.Insert(1, "Kg");
            UnidadCb.Items.Insert(2, "Lt");

            UnidadCb.SelectedIndex = 0;
        }

        private void MostrarVentanaPrincipal()
        {
            Dispatcher.Invoke(() =>
            {
                Principal ventana = new Principal(CuentaUsuario);
                ventana.Show();
                this.Close();
            });
        }

        private void VaciarCampos()
        {
            nombreTxt.Text = "";
            PrecioTxt.Text = "";
            MinimoTxt.Text = "";
            UnidadCb.SelectedIndex = 0;
            ActualTxt.Text = "";
            UbicacionTxt.Text = "";
            CodigoTxt.Text = "";
            DescripcionTxt.Text = "";
            RestriccionesTxt.Text = "";
            ProductoImg.Source = null;
        }

        private void AceptarBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void VaciarBtn_Click(object sender, RoutedEventArgs e)
        {
            VaciarCampos();
        }

        private void nombreTxt_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (AlgunCampoLleno())
            {
                VaciarBtn.IsEnabled = true;
            } 
            else
            {
                VaciarBtn.IsEnabled = false;
            }

            if (CamposLlenos())
            {
                GuardarBtn.IsEnabled = true;
            }
            else
            {
                GuardarBtn.IsEnabled = false;
            }
        }

        private void PrecioTxt_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (AlgunCampoLleno())
            {
                VaciarBtn.IsEnabled = true;
            }
            else
            {
                VaciarBtn.IsEnabled = false;
            }

            if (CamposLlenos())
            {
                GuardarBtn.IsEnabled = true;
            }
            else
            {
                GuardarBtn.IsEnabled = false;
            }
        }

        private void MinimoTxt_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (AlgunCampoLleno())
            {
                VaciarBtn.IsEnabled = true;
            }
            else
            {
                VaciarBtn.IsEnabled = false;
            }

            if (CamposLlenos())
            {
                GuardarBtn.IsEnabled = true;
            }
            else
            {
                GuardarBtn.IsEnabled = false;
            }
        }

        private void UnidadCb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (AlgunCampoLleno())
            {
                VaciarBtn.IsEnabled = true;
            }
            else
            {
                VaciarBtn.IsEnabled = false;
            }

            if (CamposLlenos())
            {
                GuardarBtn.IsEnabled = true;
            }
            else
            {
                GuardarBtn.IsEnabled = false;
            }
        }

        private void ActualTxt_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (AlgunCampoLleno())
            {
                VaciarBtn.IsEnabled = true;
            }
            else
            {
                VaciarBtn.IsEnabled = false;
            }

            if (CamposLlenos())
            {
                GuardarBtn.IsEnabled = true;
            }
            else
            {
                GuardarBtn.IsEnabled = false;
            }
        }

        private void UbicacionTxt_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (AlgunCampoLleno())
            {
                VaciarBtn.IsEnabled = true;
            }
            else
            {
                VaciarBtn.IsEnabled = false;
            }

            if (CamposLlenos())
            {
                GuardarBtn.IsEnabled = true;
            }
            else
            {
                GuardarBtn.IsEnabled = false;
            }
        }

        private void CodigoTxt_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (AlgunCampoLleno())
            {
                VaciarBtn.IsEnabled = true;
            }
            else
            {
                VaciarBtn.IsEnabled = false;
            }

            if (CamposLlenos())
            {
                GuardarBtn.IsEnabled = true;
            }
            else
            {
                GuardarBtn.IsEnabled = false;
            }
        }

        private void DescripcionTxt_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (AlgunCampoLleno())
            {
                VaciarBtn.IsEnabled = true;
            }
            else
            {
                VaciarBtn.IsEnabled = false;
            }

            if (CamposLlenos())
            {
                GuardarBtn.IsEnabled = true;
            }
            else
            {
                GuardarBtn.IsEnabled = false;
            }
        }

        private void RestriccionesTxt_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (AlgunCampoLleno())
            {
                VaciarBtn.IsEnabled = true;
            }
            else
            {
                VaciarBtn.IsEnabled = false;
            }

            if (CamposLlenos())
            {
                GuardarBtn.IsEnabled = true;
            }
            else
            {
                GuardarBtn.IsEnabled = false;
            }
        }

        private void ImagenBtn_Click(object sender, RoutedEventArgs e)
        {
            var ExploradorArchivos = new OpenFileDialog();

            DialogResult RutaImagen = ExploradorArchivos.ShowDialog();

            if(RutaImagen == System.Windows.Forms.DialogResult.OK)
            {
                string path = ExploradorArchivos.FileName;
                Uri FilePath = new Uri(path);
                ProductoImg.Source = new BitmapImage(FilePath);
            }
        }

        private void CancelarBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult opcion;

            if (AlgunCampoLleno())
            {
                opcion = MessageBox.Show("¿Seguro que deseea descartar los cambios?", "Descartar cambios",
                    MessageBoxButton.OKCancel, MessageBoxImage.Question);

                if (opcion == MessageBoxResult.OK)
                {
                    VaciarCampos();
                    MostrarVentanaPrincipal();
                }
            }
            else
            {
                MostrarVentanaPrincipal();
            }
        }

        private void CerrarSesionBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult opcion;

            opcion = MessageBox.Show("¿Seguro que deseas cerrar la sesión?", "Cerrar sesión",
                    MessageBoxButton.OKCancel, MessageBoxImage.Question);

            if (opcion == MessageBoxResult.OK)
            {
                CerrarSesion();
            }
        }
    }
}