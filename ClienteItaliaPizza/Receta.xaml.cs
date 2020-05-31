using ClienteItaliaPizza.Servicio;
using ClienteItaliaPizza.Validacion;
using System.Windows;
using System.Windows.Input;

namespace ClienteItaliaPizza
{
    /// <summary>
    /// Lógica de interacción para Receta.xaml
    /// </summary>
    public partial class Receta : Window
    {
        protected object recetaExistente; //creo esta clase temporalmente para VALIDAR si la ventna se llama con un objeto receta o no
        protected CuentaUsuario1 cuenta = new CuentaUsuario1(); 
        public Receta(CuentaUsuario1 cuentaUsuario)
        {
            InitializeComponent();
            cuenta = cuentaUsuario;
        }

        public Receta(CuentaUsuario1 cuentausuario, object recetaexistente)
        {
            InitializeComponent();
            cuenta = cuentausuario;
            recetaExistente = recetaexistente;
        }

        private void TextBoxNombreReceta_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Validador Validacion = new Validador();
            bool resultadoValidacion = Validacion.validarSoloLetrasConAcentos(e.Text);
            if (resultadoValidacion == false)
            {
                e.Handled = true;
            }
        }

        private void TextBoxPorciones_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Validador Validacion = new Validador();
            bool resultadoValidacion = Validacion.validarSoloNumeros(e.Text);
            if (resultadoValidacion == false)
            {
                e.Handled = true;
            }
        }

        private void TextBoxProcedimiento_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Validador Validacion = new Validador();
            bool resultadoValidacion = Validacion.validarLetrasConAcentosYNumeros(e.Text);
            if (resultadoValidacion == false)
            {
                e.Handled = true;
            }

        }

        private void ButtonAceptar_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonCancelar_Click(object sender, RoutedEventArgs e)
        {
            if (recetaExistente != null)
            {
                BuscarReceta ventanaBuscarReceta = new BuscarReceta(cuenta);
                ventanaBuscarReceta.Show();
                this.Close();
            }
            else
            {
     
                Principal ventana = new Principal(cuenta);
                ventana.Show();
                this.Close();
            }
        }

        private void DesabilitarCopy_Paste(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Command == ApplicationCommands.Copy ||
        e.Command == ApplicationCommands.Cut ||
        e.Command == ApplicationCommands.Paste)
            {
                e.Handled = true;
            }
        }
    }
}
