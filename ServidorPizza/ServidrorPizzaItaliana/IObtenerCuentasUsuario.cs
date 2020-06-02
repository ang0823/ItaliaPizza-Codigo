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
        void ObtenerCuentas(string idEmleadoGenerado);
    }

    [ServiceContract]
    public interface IObtenerCuentasCallback
    {
        [OperationContract(IsOneWay = true)]
        void DevuelveCuentas(CuentaUsuario1 cuenta, Empleado1 empleado, Direccion1 direccion, Rol1 rol);

        [OperationContract(IsOneWay = true)]
        void DevuelveCuentas2(Empleado1 empleado, Direccion1 direccion, Rol1 rol);

        [OperationContract(IsOneWay = true)]
        void RespuestaOCU(string mensaje);
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
        Int64 idEmpleado;
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
        [DataMember]
        string idEmpleadoGenerado;
        [DataMember]
        bool activado;



        public Empleado1(Int64 idEmpleado, string nombre, string apellidoPaterno, string apellidoMaterno, string telefono, string correo, string idEmpleadoGenerado, bool activado)
        {
            this.idEmpleado = idEmpleado;
            this.nombre = nombre;
            this.apellidoPaterno = apellidoPaterno;
            this.apellidoMaterno = apellidoMaterno;
            this.telefono = telefono;
            this.correo = correo;
            this.idEmpleadoGenerado = idEmpleadoGenerado;
            this.activado = activado;
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
        [DataMember]
        string codigoPostal;


        public Direccion1(int id, string calle, string colonia, string numeroExterior, string numeroInterior, string codigoPostal)
        {
            this.id = id;
            this.calle = calle;
            this.colonia = colonia;
            this.numeroExterior = numeroExterior;
            this.numeroInterior = numeroInterior;
            this.codigoPostal = codigoPostal;
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
