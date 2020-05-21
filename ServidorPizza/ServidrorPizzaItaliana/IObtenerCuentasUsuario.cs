using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;


namespace ServidrorPizzaItaliana
{
    [ServiceContract(CallbackContract = typeof(IObtenerCuentasCallback))]
    interface IObtenerCuentasUsuario
    {
        [OperationContract(IsOneWay = true)]
        void ObtenerCuentas();
    }

    [ServiceContract]
    public interface IObtenerCuentasCallback
    {
        [OperationContract(IsOneWay = true)]
        void DevuelveCuentas(List<CuentaUsuario1> cuentas, List<Empleado1> empleados, List<Direccion1> direcciones, List<Rol1> roles);

        [OperationContract(IsOneWay = true)]
        void ObtenerCuentaUsuarioRespuesta(string mensaje);
    }

    [DataContract]
    public class CuentaUsuario1
    {
        [DataMember]
        string nombreUsuario;
        [DataMember]
        string contraseña;
        [DataMember]
        int id;

        public CuentaUsuario1(string nombreUsuario, string contraseña, int id)
        {
            this.id = id;
            this.nombreUsuario = nombreUsuario;
            this.contraseña = contraseña;
        }
    }

    [DataContract]
    public class Empleado1
    {
        [DataMember]
        int idEmpleado;
        [DataMember]
        string nombre;
        [DataMember]
        string apellidoPaterno;
        [DataMember]
        string apellidoMaterno;
        [DataMember]
        string telefono;
        [DataMember]
        string correo;


        public Empleado1(int idEmpleado, string nombre, string apellidoPaterno, string apellidoMaterno, string telefono, string correo)
        {
            this.idEmpleado = idEmpleado;
            this.nombre = nombre;
            this.apellidoPaterno = apellidoPaterno;
            this.apellidoMaterno = apellidoMaterno;
            this.telefono = telefono;
            this.correo = correo;
        }
    }

    [DataContract]
    public class Direccion1
    {
        [DataMember]
        int id;
        [DataMember]
        string calle;
        [DataMember]
        string colonia;
        [DataMember]
        string numeroExterior;
        [DataMember]
        string numeroInterior;


        public Direccion1(int id, string calle, string colonia, string numeroExterior, string numeroInterior)
        {
            this.id = id;
            this.calle = calle;
            this.colonia = colonia;
            this.numeroExterior = numeroExterior;
            this.numeroInterior = numeroInterior;
        }
    }

    [DataContract]
    public class Rol1
    {
        [DataMember]
        int id;
        [DataMember]
        string rol;

        public Rol1(int id, string rol)
        {
            this.id = id;
            this.rol = rol;
        }
    }
}
