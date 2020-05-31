using System;
using AccesoBD2;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Threading;
using System.Data.Entity;
using System.ComponentModel.Design;
using System.Data.Entity.Validation;

namespace ServidrorPizzaItaliana
{

    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]

    public partial class Servicios : IBuscarProducto
    {
        private BDPizzaEntities db = new BDPizzaEntities();

        public Servicios()
        {

        }

        public void BuscarPorNombre(string nombreProducto)
        {

            db.Configuration.ProxyCreationEnabled = false;

            try
            {
                var provision = (from prod in db.ProvisionSet where prod.nombre == nombreProducto select prod).First();
                Callback.Provision(provision);
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine(e.StackTrace);
                Callback.ErrorAlRecuperarProducto("Ocurrio un error al recuperar producto");
            }
        }

        public void BuscarPorID(int idProducto)
        {
            db.Configuration.ProxyCreationEnabled = false;

            try
            {
                var provisionDirecta = (from provi in db.ProvisionDirectaSet where provi.Provision.Id == idProducto select provi).First();

                Callback.ProvicionDirecta(provisionDirecta);
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine(e.StackTrace);
                Callback.ErrorAlRecuperarProducto("Ocurrio un error al recuperar producto");
            }
        }

        IBuscarProductoCallback Callback
        {
            get
            {
                return OperationContext.Current.GetCallbackChannel<IBuscarProductoCallback>();
            }
        }
    }

    public partial class Servicios : ILogin
    {
        public void IniciarSesion(string nombreUsuario, string contraseña)
        {
            try
            {
                BDPizzaEntities db = new BDPizzaEntities();
                db.Configuration.ProxyCreationEnabled = false;
                //db.CuentaSet.Where(d => d.nombreUsuario == nombreUsuario && d.contraseña == contraseña).First();
                var cuenta = (from per in db.CuentaUsuarioSet where per.nombreUsuario == nombreUsuario && per.contraseña == contraseña select per).Include(x => x.Empleado.Rol).First();

                CuentaCliente cuentaCliente = new CuentaCliente(cuenta.Empleado.Rol.nombreRol, cuenta.nombreUsuario);

                OperationContext.Current.GetCallbackChannel<ILoginCallback>().DevuelveCuenta(cuentaCliente);
                Console.WriteLine(cuenta.nombreUsuario + ": Ha iniciado sesión");
                Console.WriteLine(cuenta.Empleado.Rol.nombreRol);
                //db.Dispose();
            }
            catch (InvalidOperationException)
            {
                OperationContext.Current.GetCallbackChannel<ILoginCallback>().RespuestaLogin("Alguno de los datos introducidos no son correctos");
            }
        }
    }

    public partial class Servicios : IRegistrarCuentaUsuario
    {
        public void RegistrarCuentaUsuario(CuentaUsuario cuenta, Empleado empleado, Direccion direccion, Rol rol)
        {

            try
            {

                Console.WriteLine("BDloteriaEntities2");
                var c = (from per in db.EmpleadoSet where per.idEmpleadoGenerado == empleado.idEmpleadoGenerado select per).First();
                Console.WriteLine("Consulta");

                if (c != null)
                {

                    OperationContext.Current.GetCallbackChannel<IRegistrarCuentaUsuarioCallback>().RespuestaRCU("El usuario ya ha sido registrado");
                }
            }
            catch (InvalidOperationException)
            {
                empleado.Rol = rol;
                empleado.Direccion = direccion;
                cuenta.Empleado = empleado;
                db.CuentaUsuarioSet.Add(cuenta);
                db.SaveChanges();
                db.Dispose();
                OperationContext.Current.GetCallbackChannel<IRegistrarCuentaUsuarioCallback>().RespuestaRCU("La cuenta de usuario se registró correctamente");
            }
        }

        public void RegistrarCuentaUsuario2(Empleado empleado, Direccion direccion, Rol rol)
        {
            try
            {

                Console.WriteLine("BDloteriaEntities2");
                var c = (from per in db.EmpleadoSet where per.idEmpleadoGenerado == empleado.idEmpleadoGenerado select per).First();
                Console.WriteLine("Consulta");

                if (c != null)
                {

                    OperationContext.Current.GetCallbackChannel<IRegistrarCuentaUsuarioCallback>().RespuestaRCU("El usuario ya ha sido registrado");
                }
            }
            catch (InvalidOperationException)
            {
                empleado.Rol = rol;
                empleado.Direccion = direccion;
                db.EmpleadoSet.Add(empleado);
                db.SaveChanges();
                db.Dispose();
                OperationContext.Current.GetCallbackChannel<IRegistrarCuentaUsuarioCallback>().RespuestaRCU("La cuenta de usuario se registró correctamente");
            }
        }
    }

    public partial class Servicios : IModificarCuentaUsuario
    {
        public void ModificarCuentaUsuario(CuentaUsuario cuenta, Empleado empleado, Direccion direccion, Rol rol)
        {
            try
            {
                CuentaUsuario c = new CuentaUsuario();
                c = cuenta;

                db.CuentaUsuarioSet.Attach(c);
                db.Entry(c).State = EntityState.Modified;
                db.SaveChanges();

                Empleado e = new Empleado();
                e = empleado;
                db.EmpleadoSet.Attach(e);
                db.Entry(e).State = EntityState.Modified;
                db.SaveChanges();

                Direccion d = new Direccion();
                d = direccion;
                db.DireccionSet.Attach(d);
                db.Entry(d).State = EntityState.Modified;
                db.SaveChanges();

                Rol r = new Rol();
                r = rol;
                db.RolSet.Attach(r);
                db.Entry(r).State = EntityState.Modified;
                db.SaveChanges();
                OperationContext.Current.GetCallbackChannel<IModificarCuentaUsuarioCallback>().RespuestaMCU("Se modificó correctamente");
                Console.WriteLine("Se modificó correctamente");

            }
            catch (InvalidOperationException)
            {
                OperationContext.Current.GetCallbackChannel<IModificarCuentaUsuarioCallback>().RespuestaMCU("Alguno de los datos introducidos no son correctos");
            }
        }
    }

    public partial class Servicios : IGenerarRespaldo
    {
        public void GenerarRespaldoAutomatico(string nombreArchivo)
        {
            string dbname = db.Database.Connection.Database;
            string sqlCommand = @"BACKUP DATABASE [{0}] TO  DISK = N'{1}' WITH NOFORMAT, NOINIT,  NAME = N'MyAir-Full Database Backup', SKIP, NOREWIND, NOUNLOAD,  STATS = 10";
            db.Database.ExecuteSqlCommand(System.Data.Entity.TransactionalBehavior.DoNotEnsureTransaction, string.Format(sqlCommand, dbname, nombreArchivo));
            OperationContext.Current.GetCallbackChannel<IGenerarRespaldoCallback>().RespuestaGR("Se modificó correctamente");
        }
    }

    public partial class Servicios : IObtenerCuentasUsuario
    {
        public void ObtenerCuentas()
        {
            try
            {
                List<CuentaUsuario1> cuentaslista = new List<CuentaUsuario1>();
                List<Empleado1> empleadolista = new List<Empleado1>();
                List<Direccion1> direccionelista = new List<Direccion1>();
                List<Rol1> roleslista = new List<Rol1>();
                using (var ctx = new BDPizzaEntities())
                {
                    var cuentas = from s in ctx.CuentaUsuarioSet
                                  select s;
                    var empleados = from s in ctx.EmpleadoSet
                                    select s;
                    var direcciones = from s in ctx.DireccionSet
                                      select s;
                    var roles = from s in ctx.RolSet
                                select s;
                    foreach (var valor in cuentas)
                    {
                        cuentaslista.Add(new CuentaUsuario1(valor.nombreUsuario, valor.contraseña, valor.Id));
                    }
                    foreach (var valor in empleados)
                    {
                        empleadolista.Add(new Empleado1(valor.IdEmpleado, valor.nombre, valor.apellidoPaterno, valor.apellidoMaterno, valor.telefono, valor.correo, valor.idEmpleadoGenerado));
                    }
                    foreach (var valor in direcciones)
                    {
                        direccionelista.Add(new Direccion1(valor.Id, valor.calle, valor.colonia, valor.numeroExterior, valor.numeroInterior,valor.codigoPostal));
                    }
                    foreach (var valor in roles)
                    {
                        roleslista.Add(new Rol1(valor.Id, valor.nombreRol));
                    }
                }
                OperationContext.Current.GetCallbackChannel<IObtenerCuentasCallback>().DevuelveCuentas(cuentaslista, empleadolista, direccionelista, roleslista);
            }
            catch (InvalidOperationException)
            {
                OperationContext.Current.GetCallbackChannel<IObtenerCuentasCallback>().RespuestaOCU("Ocurrio un error al intentar acceder a la base de datos intentelo más tarde");
            }
        }
    }

    public partial class Servicios : IEliminarCuentaUsuario
    {
        public void EliminarCuentaUsuario(string nombreUsuario, int id)
        {
            try
            {
                var nombreC = (from p in db.CuentaUsuarioSet
                               where p.nombreUsuario == nombreUsuario
                               select p).Single();
                var empleadoC = (from p in db.EmpleadoSet
                                 where p.IdEmpleado == id
                                 select p).Single();
                var direccionC = (from p in db.DireccionSet
                                  where p.Id == id
                                  select p).Single();
                var rolC = (from p in db.RolSet
                            where p.Id == id
                            select p).Single();

                db.CuentaUsuarioSet.Remove(nombreC);
                db.EmpleadoSet.Remove(empleadoC);
                db.DireccionSet.Remove(direccionC);
                db.RolSet.Remove(rolC);
                db.SaveChanges();
                OperationContext.Current.GetCallbackChannel<IEliminarCuentaUsuarioCallback>().RespuestaECU("Éxito al eliminar la cuenta de usuario");
            }
            catch (InvalidOperationException)
            {
                OperationContext.Current.GetCallbackChannel<IEliminarCuentaUsuarioCallback>().RespuestaECU("Error al intentar acceder a la base de datos");
            }
        }
    }

    public partial class Servicios : IRegistrarProducto
    {
        public void RegistrarProducto(AccesoBD2.Producto producto, Categoria categoria)
        {
            try
            {

                Console.WriteLine("BDloteriaEntities2");
                var c = (from per in db.ProductoSet where per.nombre == producto.nombre select per).First();
                Console.WriteLine("Consulta");

                if (c != null)
                {

                    OperationContext.Current.GetCallbackChannel<IRegistrarProductoCallback>().RespuestaRP("El producto ya ha sido registrado");
                }
            }
            catch (InvalidOperationException)
            {
                db.ProductoSet.Add(producto);
                db.CategoriaSet.Add(categoria);
                db.SaveChanges();
                Console.WriteLine(producto.nombre + ": Se ha registrado");
                db.Dispose();
            }
        }
    }

    public partial class Servicios : IConsultarInventario
    {
        public void ConsultarInventario()
        {
            try
            {
                List<Provision1> provisionlista = new List<Provision1>();
                List<ProvisionDirecta1> pDirectalista = new List<ProvisionDirecta1>();
                using (var ctx = new BDPizzaEntities())
                {
                    var provisiones = from s in ctx.ProvisionSet
                                      select s;
                    var pDirectas = from s in ctx.ProvisionDirectaSet
                                    select s;

                    foreach (var valor in provisiones)
                    {
                        provisionlista.Add(new Provision1(valor.Id, valor.nombre, valor.noExistencias, valor.ubicacion, valor.stockMinimo, valor.costoUnitario, valor.unidadMedida));
                    }
                    foreach (var valor in pDirectas)
                    {
                        pDirectalista.Add(new ProvisionDirecta1(valor.Id, valor.descripcion, valor.activado, valor.restricciones));
                    }
                }
                OperationContext.Current.GetCallbackChannel<IConsultarInventarioCallback>().DevuelveInventario(provisionlista, pDirectalista);
            }
            catch (InvalidOperationException)
            {
                OperationContext.Current.GetCallbackChannel<IConsultarInventarioCallback>().RespuestaCI("Ocurrio un error al intentar acceder a la base de datos intentelo más tarde");
            }
        }
    }

    public partial class Servicios : IRegistrarReceta
    {
        public void RegistrarReceta(Receta receta, List<Ingrediente> ingredientes)
        {
            try
            {
                var c1 = (from per in db.RecetaSet where per.nombreReceta == receta.nombreReceta select per).First();
                Console.WriteLine("Consulta");

                if (c1 != null)
                {

                    OperationContext.Current.GetCallbackChannel<IRegistrarRecetaCallback>().RespuestaRR("La receta ya se encuentra registrada");
                }
            }
            catch (InvalidOperationException)
            {
                receta.Ingrediente = ingredientes;
                db.RecetaSet.Add(receta);
                db.SaveChanges();
                db.Dispose();
                OperationContext.Current.GetCallbackChannel<IRegistrarRecetaCallback>().RespuestaRR("La receta se registró correctamente");
            }
        }
    }

    public partial class Servicios : IEditarReceta
    {
        public void EditarReceta(Receta receta, List<Ingrediente> ingredinetes)
        {
            try
            {
                Receta r = new Receta();
                r = receta;
                receta.Ingrediente = ingredinetes;
                db.RecetaSet.Attach(r);
                db.Entry(r).State = EntityState.Modified;
                db.SaveChanges();

                OperationContext.Current.GetCallbackChannel<IEditarRecetaCallback>().RespuestaER("Se modificó correctamente");
                Console.WriteLine("Se modificó correctamente");

            }
            catch (InvalidOperationException)
            {
                OperationContext.Current.GetCallbackChannel<IEditarRecetaCallback>().RespuestaER("Alguno de los datos introducidos no son correctos");
            }
        }
    }

    public partial class Servicios : IObtenerRecetas
    {
        public void ObtenerRecetas()
        {
            try
            {
                List<Receta1> recetaslista = new List<Receta1>();
                List<Ingrediente1> ingredienteslista = new List<Ingrediente1>();
                using (var ctx = new BDPizzaEntities())
                {
                    var recetas = from s in ctx.RecetaSet
                                  select s;
                    var ingredientes = from s in ctx.IngredienteSet
                                    select s;

                    foreach (var valor in recetas)
                    {
                        recetaslista.Add(new Receta1(valor.id,valor.porciones, valor.procedimiento,valor.nombreReceta));
                    }
                    foreach (var valor in ingredientes)
                    {
                        ingredienteslista.Add(new Ingrediente1(valor.Id,valor.nombre,valor.cantidad,valor.peso,valor.costoPorUnidad,valor.unidad));
                    }

                }
                OperationContext.Current.GetCallbackChannel<IObtenerRecetasCallback>().DevuelveRecetas(recetaslista,ingredienteslista);
            }
            catch (InvalidOperationException)
            {
                OperationContext.Current.GetCallbackChannel<IObtenerRecetasCallback>().RespuestaIOR("Ocurrio un error al intentar acceder a la base de datos intentelo más tarde");
            }
        }
    }

    public partial class Servicios : IModificarProducto
    {
        public void Modificar(Provision provision, ProvisionDirecta provDirecta)
        {
            try
            {
                Provision p = new Provision();
                p = provision;

                db.ProvisionSet.Attach(p);
                db.Entry(p).State = EntityState.Modified;
                db.SaveChanges();

                ProvisionDirecta d = new ProvisionDirecta();
                d = provDirecta;
                db.ProvisionDirectaSet.Attach(d);
                db.Entry(d).State = EntityState.Modified;
                db.SaveChanges();

                Callback2.RespuestaModificarProducto("Cambios Guardados");
            }
            catch (InvalidOperationException)
            {
                Callback2.RespuestaModificarProducto("Error al guardar cambios");
            }

        }

        IModificarProductoCallback Callback2
        {
            get
            {
                return OperationContext.Current.GetCallbackChannel<IModificarProductoCallback>();
            }
        }
    }

    public partial class Servicios : IRegistrarIngrediente
    {
        public void RegistrarIngrediente(Provision provision)
        {
            try
            {
                var prov = (from p in db.ProvisionSet where p.nombre == provision.nombre select p).FirstOrDefault();

                if (prov != null) {
                    Callback3.Respuesta("El producto con el nombre: " + provision.nombre + " ya esta registrado");
                }
                else
                {
                    
                    db.ProvisionSet.Add(provision);
                    db.SaveChanges();
                    Callback3.Respuesta("Exito al registrar ingrediente");
                }
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine(e.StackTrace);
                Callback3.Respuesta("Error al registrar ingrediente");
            }
        }

        IRegistrarIngredienteCallback Callback3
        {
            get
            {
                return OperationContext.Current.GetCallbackChannel<IRegistrarIngredienteCallback>();
            }
        }
    }

    public partial class Servicios : IBuscarIngrediente
    {

        public void BuscarIngredientePorNombre(string nombreProducto)
        {
            db.Configuration.ProxyCreationEnabled = false;

            try
            {
                var ingrediente = (from prod in db.ProvisionSet where prod.nombre == nombreProducto select prod).First();
                Callback4.Ingrediente(ingrediente);
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine(e.StackTrace);
                Callback4.ErrorAlRecuperarIngrediente("Ocurrio un error al recuperar ingrediente");
            }
        }

        public void BuscarIngredientePorID(int idProducto)
        {
            db.Configuration.ProxyCreationEnabled = false;

            try
            {
                var ingrediente = (from provi in db.ProvisionSet where provi.Id == idProducto select provi).First();

                Callback4.Ingrediente(ingrediente);
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine(e.StackTrace);
                Callback4.ErrorAlRecuperarIngrediente("Ocurrio un error al recuperar ingrediente");
            }
        }

        IBuscarIngredienteCallback Callback4
        {
            get
            {
                return OperationContext.Current.GetCallbackChannel<IBuscarIngredienteCallback>();
            }
        }
    }

    public partial class Servicios : IEditarIngrediente
    {
        public void Editar(Provision provision)
        {
            try
            {
                Provision p = new Provision();
                p = provision;

                db.ProvisionSet.Attach(p);
                db.Entry(p).State = EntityState.Modified;
                db.SaveChanges();

                Callback5.RespuestaEditarIngrediente("Cambios Guardados");
            }
            catch (InvalidOperationException)
            {
                Callback5.RespuestaEditarIngrediente("Error al guardar cambios");
            }

        }

        IEditarIngredienteCallback Callback5
        {
            get
            {
                return OperationContext.Current.GetCallbackChannel<IEditarIngredienteCallback>();
            }
        }
    }

    public partial class Servicios : IRecuperarProductos
    {

        public void RecuperarProductos()
        {
            try
            {
                List<Producto> productos = new List<Producto>();

                var productosRecuperados = db.ProvisionSet.ToList();

                foreach (Provision a in productosRecuperados)
                {
                    Producto productoRecuperado = new Producto(a.Id, a.nombre, a.noExistencias, a.ubicacion, a.unidadMedida);
                    productos.Add(productoRecuperado);
                }

                Callback6.Productos(productos);

            }
            catch (InvalidOperationException)
            {
                Callback6.ErrorAlRecuperarProductos("Error al recuperar productos");
            }
        }

        IRecuperarProductosCallback Callback6
        {
            get
            {
                return OperationContext.Current.GetCallbackChannel<IRecuperarProductosCallback>();
            }
        }
    }

    public partial class Servicios : IRegistrarPedidoADomicilio
    {

        public void ObtenerDatos()
        {
            List<ProductoDePedido> productos = new List<ProductoDePedido>();
            List<ProvisionVentaDirecta> provisionesVentaDirectas = new List<ProvisionVentaDirecta>();
            List<Cliente> clientes = new List<Cliente>();
            List<DireccionCliente> di = new List<DireccionCliente>();
            List<TelefonoCliente> telefonosDeCliente = new List<TelefonoCliente>();
            List<EstadoDePedido> estados = new List<EstadoDePedido>();
         
            try
            {
                db.Configuration.ProxyCreationEnabled = false;
                var productosRecuperados = db.ProductoSet.Include(x => x.Categoria).ToList();
                var provisionesRecuperadas = db.ProvisionDirectaSet.Include(x => x.Provision).ToList();
                var clientesRecuperados = db.ClienteSet.Include(x => x.Direccion).Include(b => b.Telefono).ToList();
                var estadosRecuperados = db.EstadoSet.ToList();

                foreach (AccesoBD2.Producto a in productosRecuperados)
                {
                    ProductoDePedido productoRecuperado = new ProductoDePedido(a.Id, a.nombre, a.descripcion, a.precioUnitario, a.restricciones, a.Categoria.categoria);
                    productos.Add(productoRecuperado);
                    Console.WriteLine(productoRecuperado.Categoria);
                }

                foreach (ProvisionDirecta a in provisionesRecuperadas)
                {
                    ProvisionVentaDirecta provisionRecuperada = new ProvisionVentaDirecta(a.Id, a.Provision.Id, a.Provision.nombre, a.Provision.costoUnitario, a.descripcion, a.restricciones);
                    provisionesVentaDirectas.Add(provisionRecuperada);
                    Console.WriteLine(provisionRecuperada.IdProvision.ToString());
                }

                foreach (AccesoBD2.Cliente a in clientesRecuperados)
                {
                    foreach (Direccion b in a.Direccion) {
                        DireccionCliente dir = new DireccionCliente(b.calle, b.colonia, b.numeroExterior, b.numeroInterior);
                        di.Add(dir);

                        foreach (Telefono t in a.Telefono)
                        {
                            TelefonoCliente tel = new TelefonoCliente(t.numeroTelefono);
                            telefonosDeCliente.Add(tel);
                        }
                    }

                    Cliente clienteRecuperado = new Cliente(a.Id, a.nombre, a.apellidoPaterno, a.apellidoMaterno, di, telefonosDeCliente);
                    Console.WriteLine(clienteRecuperado.Id + clienteRecuperado.Nombre);
                    clientes.Add(clienteRecuperado);    
                }

                foreach (Estado e in estadosRecuperados)
                {
                    EstadoDePedido estado = new EstadoDePedido(e.Id, e.estadoPedido);
                    estados.Add(estado);
                }

                Callback7.Datos(clientes, productos, provisionesVentaDirectas, estados);
        }
            catch (Exception e){
                Console.WriteLine(e.StackTrace);
                Callback7.Mensaje("Mensaje de error");
            }
        }

       public void RegistrarPedido(PedidoADomicilio pedido, Cuenta cuenta, int idEstado, int idEmpleado)
        {
            try
            {
                db.InsertarCuentaDePedido(cuenta.Id, cuenta.precioTotal, cuenta.subTotal, cuenta.iva, cuenta.descuento);
                db.InsertarPedido(pedido.fecha, pedido.instruccionesEspeciales, idEmpleado, idEstado, cuenta.Id);
                var pad = (from pe in db.PedidoSet where pe.Cuenta.Id == cuenta.Id select pe).First();

                foreach (AccesoBD2.Producto p in pedido.Producto)
                {
                    db.LigarProductoConPedido(p.Id, pad.Id);
                }

                foreach (ProvisionDirecta pd in pedido.ProvisionDirecta)
                {
                    db.LigarProvisionConPedido(pd.Id, pad.Id);
                }

                db.InsertarPedioDomicilio(pedido.ClienteId, pad.Id);

                Callback7.Mensaje("Exito al registrar pedido");
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine(e.StackTrace);
                Callback7.Mensaje("Ocurrio un error");
            } 
        }

        public void RegistrarCliente(AccesoBD2.Cliente cliente, Direccion direccionCliente, Telefono telefonoCliente)
        {
            try
            {
                var c = (from p in db.ClienteSet where p.nombre == cliente.nombre && p.apellidoMaterno == cliente.apellidoMaterno && p.apellidoPaterno == cliente.apellidoPaterno select p).FirstOrDefault();

                if (c != null)
                {
                    Callback7.Mensaje("El cliente ya esta registrado");
                }
                else
                {
                    db.RegistroDeClienteConDireccion(cliente.nombre, cliente.apellidoPaterno, cliente.apellidoMaterno, direccionCliente.calle, direccionCliente.colonia, direccionCliente.numeroExterior, direccionCliente.numeroInterior);
                    var clienteBD = (from p in db.ClienteSet where p.nombre == cliente.nombre && p.apellidoMaterno == cliente.apellidoMaterno && p.apellidoPaterno == cliente.apellidoPaterno select p).First();
                    var direcionBD = (from d in db.DireccionSet where d.calle == direccionCliente.calle && d.numeroExterior == direccionCliente.numeroExterior && d.numeroInterior == direccionCliente.numeroInterior select d).First();
                    
                    db.LigarClienteADireccion(direcionBD.Id, clienteBD.Id);
                    db.RegistrarTelefono(telefonoCliente.numeroTelefono, clienteBD.Id);
                    Callback7.Mensaje("Exito al registrar cliente");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                Console.WriteLine(e.Message);
                Callback7.Mensaje("Error al registrar cliente");
            }
        }

        IRegistrarPedidoADomicilioCallback Callback7
        {
            get
            {
                return OperationContext.Current.GetCallbackChannel<IRegistrarPedidoADomicilioCallback>();
            }
        }
    }

    public partial class Servicios : IRegistrarPedidoLocal
    {

        public void ObtenerInformacionDeProductosYEstados()
        {
            List<ProductoDePedido> productos = new List<ProductoDePedido>();
            List<ProvisionVentaDirecta> provisionesVentaDirectas = new List<ProvisionVentaDirecta>();
            List<EstadoDePedido> estados = new List<EstadoDePedido>();
            List<MesaLocal> mesas = new List<MesaLocal>();

            try
            {
                db.Configuration.ProxyCreationEnabled = false;
                var productosRecuperados = db.ProductoSet.Include(x => x.Categoria).ToList();
                var provisionesRecuperadas = db.ProvisionDirectaSet.Include(x => x.Provision).ToList();
                var estadosRecuperados = db.EstadoSet.ToList();
                var mesasRecuperadas = db.MesaSet.ToList();

                foreach (AccesoBD2.Producto a in productosRecuperados)
                {
                    ProductoDePedido productoRecuperado = new ProductoDePedido(a.Id, a.nombre, a.descripcion, a.precioUnitario, a.restricciones, a.Categoria.categoria);
                    productos.Add(productoRecuperado);
                    Console.WriteLine(productoRecuperado.Categoria);
                }

                foreach (ProvisionDirecta a in provisionesRecuperadas)
                {
                    ProvisionVentaDirecta provisionRecuperada = new ProvisionVentaDirecta(a.Provision.Id, a.Id,  a.Provision.nombre, a.Provision.costoUnitario, a.descripcion, a.restricciones);
                    provisionesVentaDirectas.Add(provisionRecuperada);
                    Console.WriteLine(provisionRecuperada.IdProvision.ToString());
                }

                foreach (Estado e in estadosRecuperados)
                {
                    EstadoDePedido estado = new EstadoDePedido(e.Id, e.estadoPedido);
                    estados.Add(estado);
                }

                foreach (Mesa m in mesasRecuperadas)
                {
                    MesaLocal mesa = new MesaLocal(m.Id, m.numeroMesa);
                    mesas.Add(mesa);
                }

                Callback8.DatosRecuperados(productos, provisionesVentaDirectas, estados, mesas);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                Callback8.MensajeRegistrarPedidoLocal("Mensaje de error");
            }
        }

        public void RegistrarPedidoLocal(PedidoLocal pedido, Cuenta cuenta, int idEstado, int idEmpleado)
        {
            try
            {
                db.InsertarCuentaDePedido(cuenta.Id, cuenta.precioTotal, cuenta.subTotal, cuenta.iva, cuenta.descuento);
                db.InsertarPedido(pedido.fecha, pedido.instruccionesEspeciales, idEmpleado, idEstado, cuenta.Id);
                var pl = (from pe in db.PedidoSet where pe.Cuenta.Id == cuenta.Id select pe).First();

                foreach (AccesoBD2.Producto p in pedido.Producto)
                {
                    db.LigarProductoConPedido(p.Id, pl.Id);
                }

                foreach (ProvisionDirecta pd in pedido.ProvisionDirecta)
                {
                    db.LigarProvisionConPedido(pd.Id, pl.Id);
                }

                db.InsertarPedidoLocal(pedido.MesaId, pl.Id);

                Callback8.MensajeRegistrarPedidoLocal("Exito al registrar pedido");
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine(e.StackTrace);
                Callback8.MensajeRegistrarPedidoLocal("Ocurrio un error");
            }
        }

        IRegistrarPedidoLocalCallback Callback8
        {
            get
            {
                return OperationContext.Current.GetCallbackChannel<IRegistrarPedidoLocalCallback>();
            }
        }
    }
}
