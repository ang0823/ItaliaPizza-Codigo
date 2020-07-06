﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AccesoBD2
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class BDPizzaEntities : DbContext
    {
        public BDPizzaEntities()
            : base("name=BDPizzaEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<CuentaUsuario> CuentaUsuarioSet { get; set; }
        public virtual DbSet<Empleado> EmpleadoSet { get; set; }
        public virtual DbSet<Direccion> DireccionSet { get; set; }
        public virtual DbSet<Rol> RolSet { get; set; }
        public virtual DbSet<Pedido> PedidoSet { get; set; }
        public virtual DbSet<Cliente> ClienteSet { get; set; }
        public virtual DbSet<Mesa> MesaSet { get; set; }
        public virtual DbSet<Estado> EstadoSet { get; set; }
        public virtual DbSet<Categoria> CategoriaSet { get; set; }
        public virtual DbSet<Producto> ProductoSet { get; set; }
        public virtual DbSet<Cuenta> CuentaSet { get; set; }
        public virtual DbSet<Telefono> TelefonoSet { get; set; }
        public virtual DbSet<ProvisionDirecta> ProvisionDirectaSet { get; set; }
        public virtual DbSet<Receta> RecetaSet { get; set; }
        public virtual DbSet<Provision> ProvisionSet { get; set; }
        public virtual DbSet<Ingrediente> IngredienteSet { get; set; }
    
        public virtual int InsertarPedidoADomicilio(Nullable<int> idCliente, Nullable<System.DateTime> fecha, string instruccionesEspeciales, Nullable<long> idEmpleado, string nombreEstado, string idCuenta, Nullable<double> precioTotal, Nullable<double> subTotal, Nullable<double> iva, Nullable<double> descuento, Nullable<bool> abierta, string direccionDestino, ObjectParameter iDPedido)
        {
            var idClienteParameter = idCliente.HasValue ?
                new ObjectParameter("IdCliente", idCliente) :
                new ObjectParameter("IdCliente", typeof(int));
    
            var fechaParameter = fecha.HasValue ?
                new ObjectParameter("Fecha", fecha) :
                new ObjectParameter("Fecha", typeof(System.DateTime));
    
            var instruccionesEspecialesParameter = instruccionesEspeciales != null ?
                new ObjectParameter("InstruccionesEspeciales", instruccionesEspeciales) :
                new ObjectParameter("InstruccionesEspeciales", typeof(string));
    
            var idEmpleadoParameter = idEmpleado.HasValue ?
                new ObjectParameter("IdEmpleado", idEmpleado) :
                new ObjectParameter("IdEmpleado", typeof(long));
    
            var nombreEstadoParameter = nombreEstado != null ?
                new ObjectParameter("NombreEstado", nombreEstado) :
                new ObjectParameter("NombreEstado", typeof(string));
    
            var idCuentaParameter = idCuenta != null ?
                new ObjectParameter("IdCuenta", idCuenta) :
                new ObjectParameter("IdCuenta", typeof(string));
    
            var precioTotalParameter = precioTotal.HasValue ?
                new ObjectParameter("PrecioTotal", precioTotal) :
                new ObjectParameter("PrecioTotal", typeof(double));
    
            var subTotalParameter = subTotal.HasValue ?
                new ObjectParameter("SubTotal", subTotal) :
                new ObjectParameter("SubTotal", typeof(double));
    
            var ivaParameter = iva.HasValue ?
                new ObjectParameter("Iva", iva) :
                new ObjectParameter("Iva", typeof(double));
    
            var descuentoParameter = descuento.HasValue ?
                new ObjectParameter("Descuento", descuento) :
                new ObjectParameter("Descuento", typeof(double));
    
            var abiertaParameter = abierta.HasValue ?
                new ObjectParameter("Abierta", abierta) :
                new ObjectParameter("Abierta", typeof(bool));
    
            var direccionDestinoParameter = direccionDestino != null ?
                new ObjectParameter("DireccionDestino", direccionDestino) :
                new ObjectParameter("DireccionDestino", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("InsertarPedidoADomicilio", idClienteParameter, fechaParameter, instruccionesEspecialesParameter, idEmpleadoParameter, nombreEstadoParameter, idCuentaParameter, precioTotalParameter, subTotalParameter, ivaParameter, descuentoParameter, abiertaParameter, direccionDestinoParameter, iDPedido);
        }
    
        public virtual int InsertarPedidoLocal(Nullable<short> numeroMesa, Nullable<System.DateTime> fecha, string instruccionesEspeciales, Nullable<long> idEmpleado, string nombreEstado, string idCuenta, Nullable<double> precioTotal, Nullable<double> subTotal, Nullable<double> iva, Nullable<double> descuento, Nullable<bool> abierta, ObjectParameter iDPedido)
        {
            var numeroMesaParameter = numeroMesa.HasValue ?
                new ObjectParameter("NumeroMesa", numeroMesa) :
                new ObjectParameter("NumeroMesa", typeof(short));
    
            var fechaParameter = fecha.HasValue ?
                new ObjectParameter("Fecha", fecha) :
                new ObjectParameter("Fecha", typeof(System.DateTime));
    
            var instruccionesEspecialesParameter = instruccionesEspeciales != null ?
                new ObjectParameter("InstruccionesEspeciales", instruccionesEspeciales) :
                new ObjectParameter("InstruccionesEspeciales", typeof(string));
    
            var idEmpleadoParameter = idEmpleado.HasValue ?
                new ObjectParameter("IdEmpleado", idEmpleado) :
                new ObjectParameter("IdEmpleado", typeof(long));
    
            var nombreEstadoParameter = nombreEstado != null ?
                new ObjectParameter("NombreEstado", nombreEstado) :
                new ObjectParameter("NombreEstado", typeof(string));
    
            var idCuentaParameter = idCuenta != null ?
                new ObjectParameter("IdCuenta", idCuenta) :
                new ObjectParameter("IdCuenta", typeof(string));
    
            var precioTotalParameter = precioTotal.HasValue ?
                new ObjectParameter("PrecioTotal", precioTotal) :
                new ObjectParameter("PrecioTotal", typeof(double));
    
            var subTotalParameter = subTotal.HasValue ?
                new ObjectParameter("SubTotal", subTotal) :
                new ObjectParameter("SubTotal", typeof(double));
    
            var ivaParameter = iva.HasValue ?
                new ObjectParameter("Iva", iva) :
                new ObjectParameter("Iva", typeof(double));
    
            var descuentoParameter = descuento.HasValue ?
                new ObjectParameter("Descuento", descuento) :
                new ObjectParameter("Descuento", typeof(double));
    
            var abiertaParameter = abierta.HasValue ?
                new ObjectParameter("Abierta", abierta) :
                new ObjectParameter("Abierta", typeof(bool));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("InsertarPedidoLocal", numeroMesaParameter, fechaParameter, instruccionesEspecialesParameter, idEmpleadoParameter, nombreEstadoParameter, idCuentaParameter, precioTotalParameter, subTotalParameter, ivaParameter, descuentoParameter, abiertaParameter, iDPedido);
        }
    
        public virtual int LigarProductoConPedido(Nullable<int> idProducto, Nullable<int> idPedido, Nullable<int> cantidad)
        {
            var idProductoParameter = idProducto.HasValue ?
                new ObjectParameter("IdProducto", idProducto) :
                new ObjectParameter("IdProducto", typeof(int));
    
            var idPedidoParameter = idPedido.HasValue ?
                new ObjectParameter("IdPedido", idPedido) :
                new ObjectParameter("IdPedido", typeof(int));
    
            var cantidadParameter = cantidad.HasValue ?
                new ObjectParameter("Cantidad", cantidad) :
                new ObjectParameter("Cantidad", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("LigarProductoConPedido", idProductoParameter, idPedidoParameter, cantidadParameter);
        }
    
        public virtual int LigarProvisionConPedido(Nullable<int> idProvision, Nullable<int> idPedido, Nullable<int> cantidad)
        {
            var idProvisionParameter = idProvision.HasValue ?
                new ObjectParameter("IdProvision", idProvision) :
                new ObjectParameter("IdProvision", typeof(int));
    
            var idPedidoParameter = idPedido.HasValue ?
                new ObjectParameter("IdPedido", idPedido) :
                new ObjectParameter("IdPedido", typeof(int));
    
            var cantidadParameter = cantidad.HasValue ?
                new ObjectParameter("Cantidad", cantidad) :
                new ObjectParameter("Cantidad", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("LigarProvisionConPedido", idProvisionParameter, idPedidoParameter, cantidadParameter);
        }
    
        public virtual ObjectResult<MostrarCantidadProductosPedido_Result4> MostrarCantidadProductosPedido(Nullable<int> idPedido)
        {
            var idPedidoParameter = idPedido.HasValue ?
                new ObjectParameter("IdPedido", idPedido) :
                new ObjectParameter("IdPedido", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<MostrarCantidadProductosPedido_Result4>("MostrarCantidadProductosPedido", idPedidoParameter);
        }
    
        public virtual ObjectResult<MostrarCantidadProvisionesPedido_Result3> MostrarCantidadProvisionesPedido(Nullable<int> idPedido)
        {
            var idPedidoParameter = idPedido.HasValue ?
                new ObjectParameter("IdPedido", idPedido) :
                new ObjectParameter("IdPedido", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<MostrarCantidadProvisionesPedido_Result3>("MostrarCantidadProvisionesPedido", idPedidoParameter);
        }
    
        public virtual int RegistrarPedidoYCuenta(Nullable<System.DateTime> fecha, string instruccionesEspeciales, Nullable<long> idEmpleado, string nombreEstado, string idCuenta, Nullable<double> precioTotal, Nullable<double> subTotal, Nullable<double> iva, Nullable<double> descuento, Nullable<bool> abierta, ObjectParameter idPedido)
        {
            var fechaParameter = fecha.HasValue ?
                new ObjectParameter("Fecha", fecha) :
                new ObjectParameter("Fecha", typeof(System.DateTime));
    
            var instruccionesEspecialesParameter = instruccionesEspeciales != null ?
                new ObjectParameter("InstruccionesEspeciales", instruccionesEspeciales) :
                new ObjectParameter("InstruccionesEspeciales", typeof(string));
    
            var idEmpleadoParameter = idEmpleado.HasValue ?
                new ObjectParameter("IdEmpleado", idEmpleado) :
                new ObjectParameter("IdEmpleado", typeof(long));
    
            var nombreEstadoParameter = nombreEstado != null ?
                new ObjectParameter("NombreEstado", nombreEstado) :
                new ObjectParameter("NombreEstado", typeof(string));
    
            var idCuentaParameter = idCuenta != null ?
                new ObjectParameter("IdCuenta", idCuenta) :
                new ObjectParameter("IdCuenta", typeof(string));
    
            var precioTotalParameter = precioTotal.HasValue ?
                new ObjectParameter("PrecioTotal", precioTotal) :
                new ObjectParameter("PrecioTotal", typeof(double));
    
            var subTotalParameter = subTotal.HasValue ?
                new ObjectParameter("SubTotal", subTotal) :
                new ObjectParameter("SubTotal", typeof(double));
    
            var ivaParameter = iva.HasValue ?
                new ObjectParameter("Iva", iva) :
                new ObjectParameter("Iva", typeof(double));
    
            var descuentoParameter = descuento.HasValue ?
                new ObjectParameter("Descuento", descuento) :
                new ObjectParameter("Descuento", typeof(double));
    
            var abiertaParameter = abierta.HasValue ?
                new ObjectParameter("Abierta", abierta) :
                new ObjectParameter("Abierta", typeof(bool));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("RegistrarPedidoYCuenta", fechaParameter, instruccionesEspecialesParameter, idEmpleadoParameter, nombreEstadoParameter, idCuentaParameter, precioTotalParameter, subTotalParameter, ivaParameter, descuentoParameter, abiertaParameter, idPedido);
        }
    
        public virtual int RegistroDeClienteConDireccion(string nombre, string apellidoPaterno, string apellidoMaterno, string calle, string colonia, string numeroExterior, string numeroInterior, string numeroTelefonico, string codigoPostal, ObjectParameter iDCliente)
        {
            var nombreParameter = nombre != null ?
                new ObjectParameter("Nombre", nombre) :
                new ObjectParameter("Nombre", typeof(string));
    
            var apellidoPaternoParameter = apellidoPaterno != null ?
                new ObjectParameter("ApellidoPaterno", apellidoPaterno) :
                new ObjectParameter("ApellidoPaterno", typeof(string));
    
            var apellidoMaternoParameter = apellidoMaterno != null ?
                new ObjectParameter("ApellidoMaterno", apellidoMaterno) :
                new ObjectParameter("ApellidoMaterno", typeof(string));
    
            var calleParameter = calle != null ?
                new ObjectParameter("Calle", calle) :
                new ObjectParameter("Calle", typeof(string));
    
            var coloniaParameter = colonia != null ?
                new ObjectParameter("Colonia", colonia) :
                new ObjectParameter("Colonia", typeof(string));
    
            var numeroExteriorParameter = numeroExterior != null ?
                new ObjectParameter("NumeroExterior", numeroExterior) :
                new ObjectParameter("NumeroExterior", typeof(string));
    
            var numeroInteriorParameter = numeroInterior != null ?
                new ObjectParameter("NumeroInterior", numeroInterior) :
                new ObjectParameter("NumeroInterior", typeof(string));
    
            var numeroTelefonicoParameter = numeroTelefonico != null ?
                new ObjectParameter("NumeroTelefonico", numeroTelefonico) :
                new ObjectParameter("NumeroTelefonico", typeof(string));
    
            var codigoPostalParameter = codigoPostal != null ?
                new ObjectParameter("CodigoPostal", codigoPostal) :
                new ObjectParameter("CodigoPostal", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("RegistroDeClienteConDireccion", nombreParameter, apellidoPaternoParameter, apellidoMaternoParameter, calleParameter, coloniaParameter, numeroExteriorParameter, numeroInteriorParameter, numeroTelefonicoParameter, codigoPostalParameter, iDCliente);
        }
    }
}
