﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using AccesoBD2;

namespace ServidrorPizzaItaliana
{
    [ServiceContract(CallbackContract = typeof(ILoginCallback))]
    public interface ILogin
    {
        [OperationContract(IsOneWay = true)]
        void IniciarSesion(string nombreUsuario, string contraseña);
    }

    [ServiceContract]
    public interface ILoginCallback
    {
        [OperationContract(IsOneWay = true)]
        void DevuelveCuenta(CuentaCliente cuenta);

        [OperationContract(IsOneWay = true)]
        void LoginRespuesta(string mensaje);
    }

    [DataContract]
    public class CuentaCliente
    {
        [DataMember]
        public string rol;

        [DataMember]
        public string nombreUsuario;

        public CuentaCliente(string rol, string nombreUsuario)
        {
            this.rol = rol;
            this.nombreUsuario = nombreUsuario;
        }
    }
}
