using ClienteItaliaPizza.Servicio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ClienteItaliaPizza
{
    class EmpleadoDataGrid
    {
        public int IdEmpleado { get; set; }
        public string Nombre { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public string CorreoElectronico { get; set; }
        public string Telefono { get; set; }
        public string Puesto { get; set; }
        public string Calle { get; set; }
        public string Colonia { get; set; }
        public string CodigoPostal { get; set; }
        public string Usuario { get; set; }
        public string Contrasena { get; set; }

        private EmpleadoDataGrid()
        {

        }

        public EmpleadoDataGrid(Empleado empleado, CuentaUsuario cuenta)
        {
            this.IdEmpleado = empleado.IdEmpleado;
            this.Nombre = empleado.nombre;
            this.ApellidoPaterno = empleado.apellidoPaterno;
            this.ApellidoMaterno = empleado.apellidoMaterno;
            this.CorreoElectronico = empleado.correo;
            this.Telefono = empleado.telefono;
            this.Usuario = cuenta.nombreUsuario;
            this.Contrasena = cuenta.contraseña;
        }
    }
}
