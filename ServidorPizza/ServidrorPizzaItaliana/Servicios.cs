using System;
using AccesoBD2;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Threading;
using System.Data.Entity;
using System.ComponentModel.Design;
using System.Data.Entity.Validation;
using System.Data.Entity.Migrations;
using System.Data.Entity.Core.Objects;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

namespace ServidrorPizzaItaliana
{

    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Single, InstanceContextMode = InstanceContextMode.Single)]

    public partial class Servicios : IBuscarProducto
    {
        private BDPizzaEntities db = new BDPizzaEntities();

        public Servicios()
        {
        }

        public void BuscarProductoInternoPorNombre(string nombreProducto)
        {
            try
            {
                db.Configuration.ProxyCreationEnabled = false;
                var productoInterno = (from producto in db.ProductoSet where producto.nombre == nombreProducto select producto).FirstOrDefault();

                if (productoInterno != null)
                {
                    
                    Callback.ProductoInterno(productoInterno, ObtenerImagen(nombreProducto));
                }
                else
                {
                    Callback.ErrorAlRecuperarProducto("No hay algun producto con ese nombre");
                }
            }
            catch (InvalidOperationException)
            {
                Callback.ErrorAlRecuperarProducto("Ocurrio un error al recuperar el producto");
            }
        }

        public void BuscarProductoExternoPorNombre(string nombreProducto)
        {
            try
            {
                db.Configuration.ProxyCreationEnabled = false;
                var provision = (from producto in db.ProvisionSet where producto.nombre == nombreProducto select producto).Include(x => x.ProvisionDirecta).FirstOrDefault();

                if (provision != null)
                {
                    Provision1 provisionDeProducto = new Provision1(provision.Id, provision.nombre, provision.noExistencias, provision.ubicacion, provision.stockMinimo, provision.costoUnitario, provision.unidadMedida);
                    ProvisionDirecta1 provisionDirecta;

                    foreach (ProvisionDirecta producto in provision.ProvisionDirecta)
                    {
                        provisionDirecta = new ProvisionDirecta1(producto.Id, producto.descripcion, producto.activado, producto.restricciones);
                        Callback.ProductoExterno(provisionDeProducto, provisionDirecta, ObtenerImagen(nombreProducto));
                    }
                }
                else
                {
                    Callback.ErrorAlRecuperarProducto("No hay algun producto con ese nombre");
                }
            }
            catch (InvalidOperationException)
            {
                Callback.ErrorAlRecuperarProducto("Ocurrio un error al recuperar el producto");
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
                var cuenta = (from per in db.CuentaUsuarioSet where per.nombreUsuario == nombreUsuario && per.contraseña == contraseña select per).Include(x => x.Empleado.Rol).Include(x => x.Empleado.Direccion).First();
                if (cuenta.Empleado.activado == true)
                {
                    CuentaUsuario1 cuentaCliente = new CuentaUsuario1(cuenta.nombreUsuario, cuenta.contraseña, cuenta.Id);
                    Empleado1 empleado = new Empleado1(cuenta.Empleado.IdEmpleado, cuenta.Empleado.nombre, cuenta.Empleado.apellidoPaterno, cuenta.Empleado.apellidoMaterno, cuenta.Empleado.telefono, cuenta.Empleado.correo, cuenta.Empleado.idEmpleadoGenerado, cuenta.Empleado.activado);
                    Rol1 rol = new Rol1(cuenta.Empleado.Rol.Id, cuenta.Empleado.Rol.nombreRol);
                    Direccion1 direccion = new Direccion1(cuenta.Empleado.Direccion.Id, cuenta.Empleado.Direccion.calle, cuenta.Empleado.Direccion.colonia, cuenta.Empleado.Direccion.numeroExterior, cuenta.Empleado.Direccion.numeroInterior, cuenta.Empleado.Direccion.codigoPostal);

                    OperationContext.Current.GetCallbackChannel<ILoginCallback>().DevuelveCuenta(cuentaCliente, empleado, direccion, rol);
                    Console.WriteLine(cuenta.nombreUsuario + ": Ha iniciado sesión");
                    db.Dispose();
                }
                else
                {
                    OperationContext.Current.GetCallbackChannel<ILoginCallback>().RespuestaLogin("El usuario no está activado");
                    db.Dispose();
                }

            }
            catch (InvalidOperationException)
            {
                OperationContext.Current.GetCallbackChannel<ILoginCallback>().RespuestaLogin("Alguno de los datos introducidos no son correctos");
            }
        }
    }

    public partial class Servicios : IRegistrarCuentaUsuario
    {
        public void RegistrarCuentaUsuario(CuentaUsuario cuenta, Empleado empleado, Direccion direccion, int rol)
        {
            try
            {
                var rece = (from p in db.EmpleadoSet where p.idEmpleadoGenerado == empleado.idEmpleadoGenerado select p).FirstOrDefault();

                if (rece != null)
                {
                    OperationContext.Current.GetCallbackChannel<IRegistrarCuentaUsuarioCallback>().RespuestaRCU("Ocurrio un error al intentar acceder a la base de datos intentelo más tarde");
                }
                else
                {
                    var roldb = (from p in db.RolSet where p.Id == rol select p).FirstOrDefault();

                    empleado.Rol = roldb;
                    empleado.Direccion = direccion;
                    cuenta.Empleado = empleado;
                    db.CuentaUsuarioSet.Add(cuenta);
                    db.SaveChanges();
                    OperationContext.Current.GetCallbackChannel<IRegistrarCuentaUsuarioCallback>().RespuestaRCU("Éxito al cuenta de usuario");
                }
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine(e.StackTrace);
                OperationContext.Current.GetCallbackChannel<IRegistrarCuentaUsuarioCallback>().RespuestaRCU("Ocurrio un error al registrar al empleado");
            }
        }

        public void RegistrarCuentaUsuario2(Empleado empleado, Direccion direccion, int rol)
        {
            try
            {
                var rece = (from p in db.EmpleadoSet where p.idEmpleadoGenerado == empleado.idEmpleadoGenerado select p).FirstOrDefault();

                if (rece != null)
                {
                    OperationContext.Current.GetCallbackChannel<IRegistrarCuentaUsuarioCallback>().RespuestaRCU("Ocurrio un error al intentar acceder a la base de datos intentelo más tarde");
                }
                else
                {
                    var roldb = (from p in db.RolSet where p.Id == rol select p).FirstOrDefault();
                    empleado.Rol = roldb;
                    empleado.Direccion = direccion;
                    db.EmpleadoSet.Add(empleado);
                    db.SaveChanges();
                    OperationContext.Current.GetCallbackChannel<IRegistrarCuentaUsuarioCallback>().RespuestaRCU("Éxito al registrarReceta");
                }
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine(e.StackTrace);
                OperationContext.Current.GetCallbackChannel<IRegistrarCuentaUsuarioCallback>().RespuestaRCU("Ocurrio un error al registrarReceta");
            }
        }
    }

    public partial class Servicios : IModificarCuentaUsuario
    {
        public void ModificarCuentaUsuario(CuentaUsuario cuenta, Empleado empleado, Direccion direccion, int idrol)
        {
            try
            {
                CuentaUsuario c = new CuentaUsuario();
                c = cuenta;

                // db.CuentaUsuarioSet.Attach(c);
                // db.Entry(c).State = EntityState.Modified;
                db.CuentaUsuarioSet.AddOrUpdate(c);
                db.SaveChanges();
                var rol = (from r in db.RolSet where r.Id == idrol select r).FirstOrDefault();
                Empleado e = new Empleado();
                e = empleado;
                e.Rol = rol;
                // db.EmpleadoSet.Attach(e);
                // db.Entry(e).State = EntityState.Modified;
                db.EmpleadoSet.AddOrUpdate(e);
                db.SaveChanges();

                Direccion d = new Direccion();
                d = direccion;
                // db.DireccionSet.Attach(d);
                // db.Entry(d).State = EntityState.Modified;
                db.DireccionSet.AddOrUpdate(d);
                db.SaveChanges();


                OperationContext.Current.GetCallbackChannel<IModificarCuentaUsuarioCallback>().RespuestaMCU("Se modificó correctamente");
                Console.WriteLine("Se modificó correctamente");

            }
            catch (InvalidOperationException)
            {
                OperationContext.Current.GetCallbackChannel<IModificarCuentaUsuarioCallback>().RespuestaMCU("Alguno de los datos introducidos no son correctos");
            }
        }

        public void ModificarCuentaUsuario2(Empleado empleado, Direccion direccion, int idrol)
        {
            try
            {
                var rol = (from r in db.RolSet where r.Id == idrol select r).FirstOrDefault();
                //Empleado e = new Empleado();
                //e = empleado;
                empleado.Rol = rol;
                //db.EmpleadoSet.Attach(e);
                //db.Entry(e).State = EntityState.Modified;
                db.EmpleadoSet.AddOrUpdate(empleado);
                db.SaveChanges();

                //Direccion d = new Direccion();
                //d = direccion;
                //db.DireccionSet.Attach(d);
                //db.Entry(d).State = EntityState.Modified;
                db.DireccionSet.AddOrUpdate(direccion);
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
        public void GenerarRespaldo(string nombreArchivo)
        {
            try
            {
                string dbname = db.Database.Connection.Database;
                string sqlCommand = @"BACKUP DATABASE [{0}] TO  DISK = N'{1}' WITH NOFORMAT, NOINIT,  NAME = N'MyAir-Full Database Backup', SKIP, NOREWIND, NOUNLOAD,  STATS = 10";
                db.Database.ExecuteSqlCommand(System.Data.Entity.TransactionalBehavior.DoNotEnsureTransaction, string.Format(sqlCommand, dbname, nombreArchivo));
                OperationContext.Current.GetCallbackChannel<IGenerarRespaldoCallback>().RespuestaGR("Se realizó el respaldo correctamente");
            }
            catch(Exception)
            {
                OperationContext.Current.GetCallbackChannel<IGenerarRespaldoCallback>().RespuestaGR("Error al conectar con la base de datos");
            }
        }
    }

    public partial class Servicios : IObtenerCuentasUsuario
    {
        public void ObtenerCuentas(string idEmpleadoGenerado)
        {
            try
            {
                var tipo = (from p in db.EmpleadoSet where p.idEmpleadoGenerado == idEmpleadoGenerado select p).Include(x => x.CuentaUsuario).FirstOrDefault();
                if (tipo.CuentaUsuario != null)
                {
                    var cuenta = (from per in db.EmpleadoSet where per.idEmpleadoGenerado == idEmpleadoGenerado select per).Include(x => x.Rol).Include(x => x.Direccion).Include(x => x.CuentaUsuario).First();

                    CuentaUsuario1 cuenta1 = new CuentaUsuario1(cuenta.CuentaUsuario.nombreUsuario, cuenta.CuentaUsuario.contraseña, cuenta.CuentaUsuario.Id);
                    Empleado1 empleado = new Empleado1(cuenta.IdEmpleado, cuenta.nombre, cuenta.apellidoPaterno, cuenta.apellidoMaterno, cuenta.telefono, cuenta.correo, cuenta.idEmpleadoGenerado, cuenta.activado);
                    Rol1 rol = new Rol1(cuenta.Rol.Id, cuenta.Rol.nombreRol);
                    Direccion1 direccion = new Direccion1(cuenta.Direccion.Id, cuenta.Direccion.calle, cuenta.Direccion.colonia, cuenta.Direccion.numeroExterior, cuenta.Direccion.numeroInterior, cuenta.Direccion.codigoPostal);

                    OperationContext.Current.GetCallbackChannel<IObtenerCuentasCallback>().DevuelveCuentas(cuenta1, empleado, direccion, rol);
                }
                else
                {
                    var cuenta = (from per in db.EmpleadoSet where per.idEmpleadoGenerado == idEmpleadoGenerado select per).Include(x => x.Rol).Include(x => x.Direccion).First();
                    Empleado1 empleado = new Empleado1(cuenta.IdEmpleado, cuenta.nombre, cuenta.apellidoPaterno, cuenta.apellidoMaterno, cuenta.telefono, cuenta.correo, cuenta.idEmpleadoGenerado, cuenta.activado);
                    Rol1 rol = new Rol1(cuenta.Rol.Id, cuenta.Rol.nombreRol);
                    Direccion1 direccion = new Direccion1(cuenta.Direccion.Id, cuenta.Direccion.calle, cuenta.Direccion.colonia, cuenta.Direccion.numeroExterior, cuenta.Direccion.numeroInterior, cuenta.Direccion.codigoPostal);

                    OperationContext.Current.GetCallbackChannel<IObtenerCuentasCallback>().DevuelveCuentas2(empleado, direccion, rol);
                }

            }
            catch (InvalidOperationException)
            {
                OperationContext.Current.GetCallbackChannel<IObtenerCuentasCallback>().RespuestaOCU("Ocurrio un error al intentar acceder a la base de datos intentelo más tarde");
            }
        }
    }

    public partial class Servicios : IEliminarCuentaUsuario
    {
        public void EliminarCuentaUsuario(string idEmpleadoGenerado)
        {
            try
            {
                var empleadoC = (from p in db.EmpleadoSet
                                 where p.idEmpleadoGenerado == idEmpleadoGenerado
                                 select p).Single();

                Empleado e = new Empleado();
                e = empleadoC;
                e.activado = false;
                db.EmpleadoSet.Attach(e);
                db.Entry(e).State = EntityState.Modified;
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
        public void RegistrarProducto(AccesoBD2.Producto producto, Categoria categoria, int receta)
        {
            try
            {
                var rece = (from p in db.ProductoSet where p.nombre == producto.nombre select p).FirstOrDefault();

                if (rece != null)
                {
                    OperationContext.Current.GetCallbackChannel<IRegistrarProductoCallback>().RespuestaRP("Ya existe un producto con ese nombre. Ingresa otro nombre");
                }
                else
                {
                    var recetadb = (from p in db.RecetaSet where p.id == receta select p).FirstOrDefault();

                    producto.Receta = recetadb;
                    producto.Categoria = categoria;
                    db.ProductoSet.Add(producto);
                    db.SaveChanges();
                    OperationContext.Current.GetCallbackChannel<IRegistrarRecetaCallback>().RespuestaRR("Éxito al registrarReceta");
                }
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine(e.StackTrace);
                OperationContext.Current.GetCallbackChannel<IRegistrarRecetaCallback>().RespuestaRR("Ocurrio un error al registrarReceta");
            }
        }
    }

    public partial class Servicios : IConsultarInventario
    {
        public void ConsultarInventario()
        {
            try
            {
                List<Provision> provisionlista = new List<Provision>();
              
                using (var ctx = new BDPizzaEntities())
                {
                    var provisiones = from s in ctx.ProvisionSet
                                      select s;
                    var pDirectas = from s in ctx.ProvisionDirectaSet
                                    select s;

                    foreach (var valor in provisiones)
                    {
                        if (valor.activado == true)
                        {
                            //provisionlista.Add(new Provision(valor.Id, valor.nombre, valor.noExistencias, valor.ubicacion, valor.stockMinimo, valor.costoUnitario, valor.unidadMedida));
                        }
                    }
                   
                }
                OperationContext.Current.GetCallbackChannel<IConsultarInventarioCallback>().DevuelveInventario(provisionlista);
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
                var rece = (from p in db.RecetaSet where p.nombreReceta == receta.nombreReceta select p).FirstOrDefault();

                if (rece != null)
                {
                    OperationContext.Current.GetCallbackChannel<IRegistrarRecetaCallback>().RespuestaRR("Ya existe una receta con el nombre");
                }
                else
                {
                    receta.Ingrediente = ingredientes;
                    db.RecetaSet.Add(receta);
                    db.SaveChanges();
                    OperationContext.Current.GetCallbackChannel<IRegistrarRecetaCallback>().RespuestaRR("Éxito al registrarReceta");
                }
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine(e.StackTrace);
                OperationContext.Current.GetCallbackChannel<IRegistrarRecetaCallback>().RespuestaRR("Ocurrio un error al registrarReceta");
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
        public void ObtenerReceta(string nombre)
        {
            try
            {
                var receta = (from per in db.RecetaSet where per.nombreReceta == nombre select per).Include(x => x.Ingrediente).First();
                if (receta.activado == true)
                {
                    Receta1 receta1 = new Receta1(receta.id, receta.porciones, receta.procedimiento, receta.nombreReceta);
                    List<Ingrediente1> ingredienteslista = new List<Ingrediente1>();
                    var ingredientes = db.Set<Ingrediente>().Where(receta3 => receta3.Receta.Any(ingrediente => ingrediente.nombreReceta == nombre));
                    foreach (var valor in ingredientes)
                    {
                        ingredienteslista.Add(new Ingrediente1(valor.Id, valor.nombre, valor.cantidad, valor.peso, valor.costoPorUnidad, valor.unidad));

                    }
                    OperationContext.Current.GetCallbackChannel<IObtenerRecetasCallback>().DevuelveReceta(receta1, ingredienteslista);
                }
                else
                {
                    OperationContext.Current.GetCallbackChannel<IObtenerRecetasCallback>().RespuestaIOR("La receta se encuentra desactivada");
                }
            }
            catch (InvalidOperationException)
            {
                OperationContext.Current.GetCallbackChannel<IObtenerRecetasCallback>().RespuestaIOR("Ocurrio un error al intentar acceder a la base de datos intentelo más tarde");
            }
        }

        public void ObtenerRecetas()
        {
            try
            {
                List<Receta1> recetalista = new List<Receta1>();
                var recetas = db.RecetaSet.ToList();

                foreach (var valor in recetas)
                {
                    if (valor.activado == true)
                    {
                        recetalista.Add(new Receta1(valor.id, valor.porciones, valor.procedimiento, valor.nombreReceta));
                    }
                   
                }
                OperationContext.Current.GetCallbackChannel<IObtenerRecetasCallback>().DevuelveRecetas(recetalista);
            }
            catch (InvalidOperationException)
            {
                OperationContext.Current.GetCallbackChannel<IObtenerRecetasCallback>().RespuestaIOR("Error al obtener Recetas");
            }

        }
    }

    public partial class Servicios : IModificarProducto
    {

        public void ModificarProductoInterno(AccesoBD2.Producto producto, byte[] imagen, bool modificarImagen)
        {
            try
            {
                if (producto != null)
                {
                    AccesoBD2.Producto productoInterno = new AccesoBD2.Producto();
                    productoInterno = producto;

                    db.ProductoSet.AddOrUpdate(productoInterno);
                    db.SaveChanges();

                    if (modificarImagen == true)
                    {
                        EliminarImagen(producto.nombre);
                        GuardarImagen(imagen, producto.nombre);
                    }

                    Callback2.RespuestaModificarProducto("Cambios Guardados");
                }
                else
                {
                    Callback2.RespuestaModificarProducto("El producto no puede ser nulo");
                }

            }
            catch (InvalidOperationException)
            {
                Callback2.RespuestaModificarProducto("Ocurrio un error al modificar producto");
            }
        }

        public void ModificarProductoExterno(ProvisionDirecta producto, Provision provision, byte[] imagen, bool modificarImagen)
        {
            try
            {
                if (producto != null && provision != null)
                {
                    ProvisionDirecta provisionDirectaDeProducto = new ProvisionDirecta();
                    provisionDirectaDeProducto = producto;
                    db.ProvisionDirectaSet.AddOrUpdate(provisionDirectaDeProducto);

                    Provision provisionDeProducto = new Provision();
                    provisionDeProducto = provision;

                    db.ProvisionSet.AddOrUpdate(provisionDeProducto);
                    db.SaveChanges();

                    if(modificarImagen == true)
                    {
                        EliminarImagen(provision.nombre);
                        GuardarImagen(imagen, provision.nombre);
                    }

                    Callback2.RespuestaModificarProducto("Cambios Guardados");
                }
                else
                {
                    Callback2.RespuestaModificarProducto("Los campos de la provision directa no pueden ser nulos");
                }
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
                // Provision p = new Provision();
                // p = provision;

                // db.ProvisionSet.Attach(p);
                // db.Entry(p).State = EntityState.Modified;
                db.ProvisionSet.AddOrUpdate(provision);
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

    public partial class Servicios : IAdministrarPedidosCallCenter
    {
        const string ESTADOFINAL = "Entregado";

        public void ObtenerDatos()
        {
            try
            {
                db.Configuration.ProxyCreationEnabled = false;
                Callback7.Datos(ObtenerClientes(), ObtenerProductosParaPedido(), ObtenerProvisionesParaPedido());
            }
            catch (Exception)
            {
                Callback7.Mensaje("Error al recuperar informacion");
            }
        }

        public void RegistrarPedidoADomicilio(PedidoADomicilio pedido)
        {
            try
            {
                ObjectParameter idPedido = new ObjectParameter("IDPedido", typeof(int));

                db.InsertarPedidoADomicilio(pedido.ClienteId, pedido.fecha, pedido.instruccionesEspeciales, pedido.Empleado.IdEmpleado, pedido.Estado.estadoPedido, pedido.Cuenta.Id, pedido.Cuenta.precioTotal, pedido.Cuenta.subTotal, pedido.Cuenta.iva, pedido.Cuenta.descuento, idPedido);

                int idPedidoRegistrado = Convert.ToInt32(idPedido.Value);

                foreach (AccesoBD2.Producto p in pedido.Producto)
                {
                    db.LigarProductoConPedido(p.Id, idPedidoRegistrado);
                }

                foreach (ProvisionDirecta pd in pedido.ProvisionDirecta)
                {
                    db.LigarProvisionConPedido(pd.Id, idPedidoRegistrado);
                }

                NotificarPedidoADomicilio(pedido);
            }
            catch (InvalidOperationException)
            {
                Callback7.Mensaje("Ocurrio un error al registrar pedido");
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
                    db.RegistroDeClienteConDireccion(cliente.nombre, cliente.apellidoPaterno, cliente.apellidoMaterno, direccionCliente.calle, direccionCliente.colonia, direccionCliente.numeroExterior, direccionCliente.numeroInterior, telefonoCliente.numeroTelefono, direccionCliente.codigoPostal);

                    Callback7.Mensaje("Exito al registrar cliente");
                }
            }
            catch (Exception)
            {
                Callback7.Mensaje("Error al registrar cliente");
            }
        }

        public void ModificarPedidoADomicilio(PedidoADomicilio pedido)
        {
            if (ModififcarPedido(pedido))
            {

                if (pedido.Estado.estadoPedido.Equals(ESTADOFINAL))
                {
                    if (DisminuirExsistenciasDeProductoExterno(pedido.ProvisionDirecta.ToList()))
                    {
                        NotificarPedidoADomicilio(pedido);
                    }
                    else
                    {
                        Callback7.Mensaje("Error al disminuir stock de productos");
                    }
                }
                else
                {
                    NotificarPedidoADomicilio(pedido);
                }
            }
            else
            {
                Callback7.Mensaje("Error al modificar pedido");
            }
        }

        public void RegistrarPedidoLocalCallCenter(PedidoLocal pedido)
        {
            if (ServicioRegistrarPedidoLocal(pedido))
            {
                NotificarATodosPedidoLocal(pedido);
            }
            else
            {
                Callback8.MensajeAdministrarPedidosMeseros("Error al registrar pedido local");
            }
        }

        public void ModificarPedidoLocalCallCenter(PedidoLocal pedido)
        {
            if (ModififcarPedido(pedido))
            {
                if (pedido.Estado.estadoPedido.Equals(ESTADOFINAL))
                {
                    if (DisminuirExsistenciasDeProductoExterno(pedido.ProvisionDirecta.ToList()))
                    {
                        NotificarATodosPedidoLocal(pedido);
                    }
                    else
                    {
                        Callback7.Mensaje("Error al disminuir stock de productos");
                    }
                }
                else
                {
                    NotificarATodosPedidoLocal(pedido);
                }
            }
            else
            {
                Callback7.Mensaje("Error al modificar pedido");
            }
        }

        IAdministrarPedidosCallCenterCallback Callback7
        {
            get
            {
                return OperationContext.Current.GetCallbackChannel<IAdministrarPedidosCallCenterCallback>();
            }
        }
    }

    public partial class Servicios : IAdministrarPedidosMeseros
    {
        public void ObtenerProductos()
        {
            try
            {
                Callback8.DatosRecuperados(ObtenerProductosParaPedido(), ObtenerProvisionesParaPedido());
            }
            catch (Exception)
            {
                Callback8.MensajeAdministrarPedidosMeseros("Mensaje de error");
            }
        }

        public void RegistrarPedidoLocal(PedidoLocal pedido)
        {
            if (ServicioRegistrarPedidoLocal(pedido))
            {
                NotificarATodosPedidoLocal(pedido);
            }
            else
            {
                Callback8.MensajeAdministrarPedidosMeseros("Error al registrar pedido local");
            }
        }

        public void ModificarPedidoLocal(PedidoLocal pedido)
        {
            if (ModififcarPedido(pedido))
            {
                if (pedido.Estado.estadoPedido.Equals(ESTADOFINAL))
                {
                    if (DisminuirExsistenciasDeProductoExterno(pedido.ProvisionDirecta.ToList()))
                    {
                        NotificarATodosPedidoLocal(pedido);
                    }
                    else
                    {
                        Callback8.MensajeAdministrarPedidosMeseros("Error al disminuir stock de productos");
                    }
                }
                else
                {
                    NotificarATodosPedidoLocal(pedido);
                }
            }
            else
            {
                Callback8.MensajeAdministrarPedidosMeseros("Error al modificar pedido");
            }
        }

        IAdministrarPedidosMeserosCallback Callback8
        {
            get
            {
                return OperationContext.Current.GetCallbackChannel<IAdministrarPedidosMeserosCallback>();
            }
        }
    }

    public partial class Servicios : INotificarPedido
    {
        public Dictionary<INotificarPedidoCallback, string> usuarios = new Dictionary<INotificarPedidoCallback, string>();

        public void AgregarUsuario(string tipoUsuario)
        {
            usuarios[Callback9] = tipoUsuario;
        }

        public void NotificarPedidoADomicilio(PedidoADomicilio pedido)
        {
            const string NOESDESTINATARIO = "Mesero";

            foreach (var destinatario in usuarios)
            {
                if (!destinatario.Value.Equals(NOESDESTINATARIO))
                {
                    destinatario.Key.RecibirPedidoDomicilio(pedido);
                }
            }
        }

        public void NotificarPedidoLocalPreparado(PedidoLocal pedido, string usuario)
        {
            if (ModififcarPedido(pedido) && DisminuirExistenciasDeIngrediente(pedido.Producto.ToList()))
            {
                foreach (var destinatario in usuarios)
                {
                    if (!destinatario.Value.Equals(usuario))
                    {
                        destinatario.Key.RecibirPedidoLocal(pedido);
                    }
                }
            }
            else
            {
                Callback9.MensajeNotificarPedido("Error al notificar pedido");
            }
        }

        public void NotificarATodosPedidoLocal(PedidoLocal pedido)
        {
            foreach (var destinatario in usuarios)
            {
                destinatario.Key.RecibirPedidoLocal(pedido);
            }
        }

        public void NotificarPedidoADomicilioPreparado(PedidoADomicilio pedido, string usuario)
        {
            if (ModififcarPedido(pedido) && DisminuirExistenciasDeIngrediente(pedido.Producto.ToList()))
            {
                foreach (var destinatario in usuarios)
                {
                    if (destinatario.Value.Equals(usuario))
                    {
                        destinatario.Key.RecibirPedidoDomicilio(pedido);
                    }
                }
            }
            else
            {
                Callback9.MensajeNotificarPedido("Error al notificar pedido");
            }
        }

        INotificarPedidoCallback Callback9
        {
            get
            {
                return OperationContext.Current.GetCallbackChannel<INotificarPedidoCallback>();
            }
        }
    }

    public partial class Servicios : IBuscarPedidos
    {
        private List<PedidoADomicilioDeServidor> pedidosADomicilio = new List<PedidoADomicilioDeServidor>();
        private List<PedidoLocalDeServidor> pedidosLocalesDeServidor = new List<PedidoLocalDeServidor>();
        private static DateTime fecha = DateTime.Now;
        private string fechaDelDia = fecha.ToString("dd/MM/yyyy");
        private const string IDPEDIDOLOCAL = "PL";
        private const string IDPEDIDOADOMICILIO = "PD";

        public void BuscarPedidos()
        {
            if(ObtenerPedidosAaDomicilio() && ObtenerPedidosLocales())
            {
                if(pedidosADomicilio.Count > 0 || pedidosLocalesDeServidor.Count > 0)
                {
                    Callback10.Pedidos(pedidosADomicilio, pedidosLocalesDeServidor);
                }
                else
                {
                    Callback10.MensajeErrorBuscarPedidos("No hay pedidos registrados");
                }
            }
            else
            {
                Callback10.MensajeErrorBuscarPedidos("Ocurrio un error al obtener pedidos");
            }
        }

        public bool ObtenerPedidosADomicilio()
        {
            bool exitoAlObtenerPedidos = false;

            try
            {
                var pedidosDomicilio = db.PedidoSet.Where(x => x.Cuenta.Id.Contains(IDPEDIDOADOMICILIO)).Where(x => x.Cuenta.Id.Contains(fechaDelDia)).Include(x => x.Empleado).Include(x => x.Producto.Select(j => j.Categoria)).Include(x => x.ProvisionDirecta.Select(j => j.Provision)).Include(x => x.Estado).Include(x => x.Cuenta).ToList();

                foreach (Pedido pedido in pedidosDomicilio)
                {
                    List<DireccionCliente> di = new List<DireccionCliente>();
                    List<TelefonoCliente> telefonosDeCliente = new List<TelefonoCliente>();

                    var cliente = db.ClienteSet.Where(x => x.PedidoADomicilio.Any(j => j.Id == pedido.Id)).Include(x => x.Direccion).Include(x => x.Telefono).FirstOrDefault();

                    foreach (Direccion b in cliente.Direccion)
                    {
                        DireccionCliente dir = new DireccionCliente(b.calle, b.colonia, b.numeroExterior, b.numeroInterior, b.codigoPostal);
                        di.Add(dir);
                    }

                    foreach (Telefono t in cliente.Telefono)
                    {
                        TelefonoCliente tel = new TelefonoCliente(t.numeroTelefono);
                        telefonosDeCliente.Add(tel);
                    }

                    Cliente clienteRecuperado = new Cliente(cliente.Id, cliente.nombre, cliente.apellidoPaterno, cliente.apellidoMaterno, di, telefonosDeCliente);
                    PedidoADomicilioDeServidor pedidoADomicilio = new PedidoADomicilioDeServidor(clienteRecuperado);
                    
                    double descuento = Convert.ToDouble(pedido.Cuenta.descuento.Value);
                    CuentaDePedido cuenta = new CuentaDePedido(pedido.Cuenta.Id, pedido.Cuenta.precioTotal, pedido.Cuenta.subTotal, pedido.Cuenta.iva, descuento);

                    pedidoADomicilio.Estado = pedido.Estado.estadoPedido;
                    pedidoADomicilio.Cuenta = cuenta;
                    pedidoADomicilio.Id = pedido.Id;
                    pedidoADomicilio.Fecha = pedido.fecha;
                    pedidoADomicilio.InstruccionesDePedido = pedido.instruccionesEspeciales;
                    pedidoADomicilio.IdEmpleado = pedido.Empleado.IdEmpleado;
                    pedidoADomicilio.IdGeneradoDeEmpleado = pedido.Empleado.idEmpleadoGenerado;

                    foreach (AccesoBD2.Producto producto in pedido.Producto)
                    {
                        ProductoDePedido productoLocal = new ProductoDePedido(producto.Id, producto.nombre, producto.descripcion, producto.precioUnitario, producto.restricciones, producto.Categoria.categoria);
                        pedidoADomicilio.ProductosLocales.Add(productoLocal);
                    }

                    foreach (ProvisionDirecta a in pedido.ProvisionDirecta)
                    {
                        ProvisionVentaDirecta productoExterno = new ProvisionVentaDirecta(a.Id, a.Provision.Id, a.Provision.nombre, a.Provision.noExistencias, a.Provision.ubicacion, a.Provision.stockMinimo, a.Provision.costoUnitario, a.Provision.unidadMedida, a.Provision.activado, a.descripcion, a.restricciones, a.Categoria.categoria);
                        pedidoADomicilio.ProductosExternos.Add(productoExterno);
                    }

                    pedidosADomicilio.Add(pedidoADomicilio);
                }

                return exitoAlObtenerPedidos = true;
            }
            catch (InvalidOperationException)
            {
                return exitoAlObtenerPedidos;
            }
        }

        public bool ObtenerPedidosLocales()
        {
            bool exitoAlObtenerPedidos = false;

            try
            {
                var pedidosLocales = db.PedidoSet.Where(x => x.Cuenta.Id.Contains(IDPEDIDOLOCAL)).Where(x => x.Cuenta.Id.Contains(fechaDelDia)).Include(x => x.Empleado).Include(x => x.Producto.Select(j => j.Categoria)).Include(x => x.ProvisionDirecta.Select(j => j.Provision)).Include(x => x.Estado).Include(x => x.Cuenta).ToList();
                
                foreach (Pedido pedido in pedidosLocales)
                {  
                    var mesa = db.MesaSet.Where(x => x.PedidoLocal.Any(j => j.Id == pedido.Id)).FirstOrDefault();
                    PedidoLocalDeServidor pedidoLocal = new PedidoLocalDeServidor(mesa.Id, mesa.numeroMesa);

                    double descuento = Convert.ToDouble(pedido.Cuenta.descuento.Value);
                    CuentaDePedido cuenta = new CuentaDePedido(pedido.Cuenta.Id, pedido.Cuenta.precioTotal, pedido.Cuenta.subTotal, pedido.Cuenta.iva, descuento);

                    pedidoLocal.Estado = pedido.Estado.estadoPedido;
                    pedidoLocal.Cuenta = cuenta;
                    pedidoLocal.Id = pedido.Id;
                    pedidoLocal.Fecha = pedido.fecha;
                    pedidoLocal.InstruccionesDePedido = pedido.instruccionesEspeciales;
                    pedidoLocal.IdEmpleado = pedido.Empleado.IdEmpleado;
                    pedidoLocal.IdGeneradoDeEmpleado = pedido.Empleado.idEmpleadoGenerado;

                    foreach(AccesoBD2.Producto producto in pedido.Producto)
                    {
                        ProductoDePedido productoLocal = new ProductoDePedido(producto.Id, producto.nombre, producto.descripcion, producto.precioUnitario, producto.restricciones, producto.Categoria.categoria);
                        pedidoLocal.ProductosLocales.Add(productoLocal);
                    }

                    foreach(ProvisionDirecta a in pedido.ProvisionDirecta)
                    {
                        ProvisionVentaDirecta productoExterno = new ProvisionVentaDirecta(a.Id, a.Provision.Id, a.Provision.nombre, a.Provision.noExistencias, a.Provision.ubicacion, a.Provision.stockMinimo, a.Provision.costoUnitario, a.Provision.unidadMedida, a.Provision.activado, a.descripcion, a.restricciones, a.Categoria.categoria);
                        pedidoLocal.ProductosExternos.Add(productoExterno);
                    }

                    pedidosLocalesDeServidor.Add(pedidoLocal);
                }

                return exitoAlObtenerPedidos = true;
            }
            catch (InvalidOperationException)
            {
                return exitoAlObtenerPedidos;
            }
        }

        public 

        IBuscarPedidosCallback Callback10
        {
            get
            {
                return OperationContext.Current.GetCallbackChannel<IBuscarPedidosCallback>();
            }
        }
    }

    public partial class Servicios
    {
        public bool ServicioRegistrarPedidoLocal(PedidoLocal pedido)
        {
            bool exitoAlRegistrarPedido = false;

            try
            {
                ObjectParameter IdPedido = new ObjectParameter("IDPedido", typeof(int));

                db.InsertarPedidoLocal(pedido.Mesa.numeroMesa, pedido.fecha, pedido.instruccionesEspeciales, pedido.Empleado.IdEmpleado, pedido.Estado.estadoPedido, pedido.Cuenta.Id, pedido.Cuenta.precioTotal, pedido.Cuenta.subTotal, pedido.Cuenta.iva, pedido.Cuenta.descuento, IdPedido);

                int idPedidoRegistrado = Convert.ToInt32(IdPedido.Value);

                foreach (AccesoBD2.Producto p in pedido.Producto)
                {
                    db.LigarProductoConPedido(p.Id, idPedidoRegistrado);
                }

                foreach (ProvisionDirecta pd in pedido.ProvisionDirecta)
                {
                    db.LigarProvisionConPedido(pd.Id, idPedidoRegistrado);
                }

                return exitoAlRegistrarPedido = true;
            }
            catch (InvalidOperationException)
            {
                return exitoAlRegistrarPedido;
            }
        }

        public List<EmpleadoPizzeria> ObtenerMeseros()
        {
            List<EmpleadoPizzeria> meseros = new List<EmpleadoPizzeria>();
            const string ROL_DE_EMPLEADO = "Mesero";

            try
            {
                var meserosObtenidos = db.EmpleadoSet.Where(x => x.activado == true).Where(x => x.Rol.nombreRol == ROL_DE_EMPLEADO).ToList();

                foreach (Empleado mesero in meserosObtenidos)
                {
                    EmpleadoPizzeria empleado = new EmpleadoPizzeria(mesero.IdEmpleado, mesero.idEmpleadoGenerado);
                    meseros.Add(empleado);
                }

            }
            catch (Exception)
            {

            }

            return meseros;
        }

        public List<ProductoDePedido> ObtenerProductosParaPedido()
        {
            List<ProductoDePedido> productos = new List<ProductoDePedido>();

            try
            {
                var productosRecuperados = db.ProductoSet.Where(x => x.activado == true).Include(x => x.Categoria).ToList();

                foreach (AccesoBD2.Producto a in productosRecuperados)
                {
                    ProductoDePedido productoRecuperado = new ProductoDePedido(a.Id, a.nombre, a.descripcion, a.precioUnitario, a.restricciones, a.Categoria.categoria);
                    //productoRecuperado.Imagen = ObtenerImagen(a.nombre);
                    productos.Add(productoRecuperado);
                }

            }
            catch (Exception)
            {

            }

            return productos;
        }

        public List<ProvisionVentaDirecta> ObtenerProvisionesParaPedido()
        {
            List<ProvisionVentaDirecta> provisionesVentaDirectas = new List<ProvisionVentaDirecta>();

            try
            {
                db.Configuration.ProxyCreationEnabled = false;
                var provisionesRecuperadas = db.ProvisionDirectaSet.Where(a => a.activado == true).Include(x => x.Provision).Include(x => x.Categoria).ToList();

                foreach (ProvisionDirecta a in provisionesRecuperadas)
                {
                    ProvisionVentaDirecta provisionRecuperada = new ProvisionVentaDirecta(a.Id, a.Provision.Id, a.Provision.nombre, a.Provision.noExistencias, a.Provision.ubicacion, a.Provision.stockMinimo, a.Provision.costoUnitario, a.Provision.unidadMedida, a.Provision.activado, a.descripcion, a.restricciones, a.Categoria.categoria);
                    //provisionRecuperada.Imagen = ObtenerImagen(a.Provision.nombre);
                    provisionesVentaDirectas.Add(provisionRecuperada);
                }
            }
            catch (Exception)
            {

            }

            return provisionesVentaDirectas;
        }

        public List<Cliente> ObtenerClientes()
        {
            List<Cliente> clientes = new List<Cliente>();
            List<DireccionCliente> di;
            List<TelefonoCliente> telefonosDeCliente;

            try
            {
                var clientesRecuperados = db.ClienteSet.Include(x => x.Direccion).Include(b => b.Telefono).ToList();

                foreach (AccesoBD2.Cliente a in clientesRecuperados)
                {
                    di = new List<DireccionCliente>();
                    telefonosDeCliente = new List<TelefonoCliente>();

                    foreach (Direccion b in a.Direccion)
                    {
                        DireccionCliente dir = new DireccionCliente(b.calle, b.colonia, b.numeroExterior, b.numeroInterior, b.codigoPostal);
                        di.Add(dir);
                    }

                    foreach (Telefono t in a.Telefono)
                    {
                        TelefonoCliente tel = new TelefonoCliente(t.numeroTelefono);
                        telefonosDeCliente.Add(tel);
                    }

                    Cliente clienteRecuperado = new Cliente(a.Id, a.nombre, a.apellidoPaterno, a.apellidoMaterno, di, telefonosDeCliente);
                    clientes.Add(clienteRecuperado);
                }
            }
            catch (Exception)
            {

            }
            return clientes;
        }

        public bool DisminuirExsistenciasDeProductoExterno(List<ProvisionDirecta> provisiones)
        {
            bool exitoAlDisminuir = false;

            try
            {
                foreach (ProvisionDirecta provisionDirecta in provisiones)
                {
                    provisionDirecta.Provision.noExistencias -= 1;
                    db.ProvisionSet.AddOrUpdate(provisionDirecta.Provision);
                    db.SaveChanges();
                }

                return exitoAlDisminuir = true;
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
                return exitoAlDisminuir;
            }
        }

        public bool DisminuirExistenciasDeIngrediente(List<AccesoBD2.Producto> productos)
        {
            bool exitoAlDisminuir = false;

            try
            {
                foreach (AccesoBD2.Producto producto in productos)
                {
                    var receta = db.RecetaSet.Where(x => x.Producto.nombre == producto.nombre).Include(x => x.Ingrediente).FirstOrDefault();
                    
                    foreach (Ingrediente ingrediente in receta.Ingrediente)
                    {
                        var provision = db.ProvisionSet.Where(x => x.nombre == ingrediente.nombre).First();

                        provision.noExistencias -= ingrediente.cantidad;

                        db.ProvisionSet.AddOrUpdate(provision);
                        db.SaveChanges();
                    }
                }

                return exitoAlDisminuir = true;
            }
            catch (InvalidOperationException)
            {
                return exitoAlDisminuir;
            }
        }

        public bool ModififcarPedido(Pedido pedido)
        {
            bool exitoAlModificar = false;

            try
            {
                var pe = (from ped in db.PedidoSet where ped.Cuenta.Id == pedido.Cuenta.Id select ped).First();
                var estadoRecuperado = (from estado in db.EstadoSet where estado.estadoPedido == pedido.Estado.estadoPedido select estado).FirstOrDefault();

                pe.Estado = estadoRecuperado;
                db.PedidoSet.Attach(pe);
                db.Entry(pe).State = EntityState.Modified;
                db.SaveChanges();

                return exitoAlModificar = true;
            }
            catch (InvalidOperationException)
            {
                return exitoAlModificar;
            }
        }

        public void EliminarImagen(String nombreImagen)
        {
            File.Delete("C:/Users/BETO/Documents/GitHub/ItaliaPizza-Codigo/ServidorPizza/ServidrorPizzaItaliana/ImagenesDeProductos/" + nombreImagen + ".jpg");
        }

        public void GuardarImagen(byte[] arrayImagen, string nombreDeImagen)
        {
            Image imagen = (Bitmap)((new ImageConverter()).ConvertFrom(arrayImagen));
            imagen.Save("C:/Users/BETO/Documents/GitHub/ItaliaPizza-Codigo/ServidorPizza/ServidrorPizzaItaliana/ImagenesDeProductos/" + nombreDeImagen + ".jpg", ImageFormat.Jpeg);
        }

        public byte[] ObtenerImagen(string nombreImagen)
        {
            byte[] imagen;

            Stream archivo = new FileStream("C:/Users/BETO/Documents/GitHub/ItaliaPizza-Codigo/ServidorPizza/ServidrorPizzaItaliana/ImagenesDeProductos/" + nombreImagen + ".jpg", FileMode.Open, FileAccess.Read);

            using (MemoryStream ms = new MemoryStream())
            {
                archivo.CopyTo(ms);
                imagen = ms.ToArray();
            }

            archivo.Close();

            return imagen;
        }

        /*public void ObtenerPedidos()
        {
            DateTime fecha = DateTime.Now;
            string fechaDelDia = fecha.ToString("dd/MM/yyyy");
            const string IDPEDIDOLOCAL = "PL";
            const string IDPEDIDOADOMICILIO = "PD";

            //var pedidosLocales = db.PedidoSet.Where(x => x.Cuenta.Id.Contains(IDPEDIDOLOCAL)).Where(x => x.Cuenta.Id.Contains(fechaDelDia)).Include(x => x.Empleado).Include(x => x.Producto.Select(j => j.Categoria)).Include(x => x.ProvisionDirecta.Select(j => j.Provision)).Include(x => x.Estado).Include(x => x.Cuenta).ToList();
            var pedidosDomicilio = db.PedidoSet.Where(x => x.Cuenta.Id.Contains(IDPEDIDOADOMICILIO)).Where(x => x.Cuenta.Id.Contains(fechaDelDia)).Include(x => x.Empleado).Include(x => x.Producto.Select(j => j.Categoria)).Include(x => x.ProvisionDirecta.Select(j => j.Provision)).Include(x => x.Estado).Include(x => x.Cuenta).ToList();

            /*foreach (Pedido pedido in pedidosLocales)
            {  
                var mesa = db.MesaSet.Where(x => x.PedidoLocal.Any(j => j.Id == pedido.Id)).FirstOrDefault();
                PedidoLocalDeServidor pedidoLocal = new PedidoLocalDeServidor(mesa.Id, mesa.numeroMesa);
                pedidoLocal.Estado = pedido.Estado.estadoPedido;
                pedidoLocal.Cuenta = pedido.Cuenta;
                pedidoLocal.Id = pedido.Id;
                pedidoLocal.Fecha = pedido.fecha;
                pedidoLocal.InstruccionesDePedido = pedido.instruccionesEspeciales;
                pedidoLocal.IdEmpleado = pedido.Empleado.IdEmpleado;
                pedidoLocal.IdGeneradoDeEmpleado = pedido.Empleado.idEmpleadoGenerado;

                foreach(AccesoBD2.Producto producto in pedido.Producto)
                {
                    ProductoDePedido productoLocal = new ProductoDePedido(producto.Id, producto.nombre, producto.descripcion, producto.precioUnitario, producto.restricciones, producto.Categoria.categoria);
                    pedidoLocal.ProductosLocales.Add(productoLocal);
                }

                foreach(ProvisionDirecta a in pedido.ProvisionDirecta)
                {
                    ProvisionVentaDirecta productoExterno = new ProvisionVentaDirecta(a.Id, a.Provision.Id, a.Provision.nombre, a.Provision.noExistencias, a.Provision.ubicacion, a.Provision.stockMinimo, a.Provision.costoUnitario, a.Provision.unidadMedida, a.Provision.activado, a.descripcion, a.restricciones, a.Categoria.categoria);
                    pedidoLocal.ProductosExternos.Add(productoExterno);
                }
            }

            foreach (Pedido pedido in pedidosDomicilio)
            {
                List<DireccionCliente> di = new List<DireccionCliente>();
                List<TelefonoCliente> telefonosDeCliente = new List<TelefonoCliente>();
                
                var cliente = db.ClienteSet.Where(x => x.PedidoADomicilio.Any(j => j.Id == pedido.Id)).Include(x => x.Direccion).Include(x => x.Telefono).FirstOrDefault();

                foreach (Direccion b in cliente.Direccion)
                {
                    DireccionCliente dir = new DireccionCliente(b.calle, b.colonia, b.numeroExterior, b.numeroInterior, b.codigoPostal);
                    di.Add(dir);

                    foreach (Telefono t in cliente.Telefono)
                    {
                        TelefonoCliente tel = new TelefonoCliente(t.numeroTelefono);
                        telefonosDeCliente.Add(tel);
                    }
                }

                Cliente clienteRecuperado = new Cliente(cliente.Id, cliente.nombre, cliente.apellidoPaterno, cliente.apellidoMaterno, di, telefonosDeCliente);
                PedidoADomicilioDeServidor pedidoADomicilio = new PedidoADomicilioDeServidor(clienteRecuperado);

                pedidoADomicilio.Estado = pedido.Estado.estadoPedido;
                pedidoADomicilio.Cuenta = pedido.Cuenta;
                pedidoADomicilio.Id = pedido.Id;
                pedidoADomicilio.Fecha = pedido.fecha;
                pedidoADomicilio.InstruccionesDePedido = pedido.instruccionesEspeciales;
                pedidoADomicilio.IdEmpleado = pedido.Empleado.IdEmpleado;
                pedidoADomicilio.IdGeneradoDeEmpleado = pedido.Empleado.idEmpleadoGenerado;

                foreach (AccesoBD2.Producto producto in pedido.Producto)
                {
                    ProductoDePedido productoLocal = new ProductoDePedido(producto.Id, producto.nombre, producto.descripcion, producto.precioUnitario, producto.restricciones, producto.Categoria.categoria);
                    pedidoADomicilio.ProductosLocales.Add(productoLocal);
                }

                foreach (ProvisionDirecta a in pedido.ProvisionDirecta)
                {
                    ProvisionVentaDirecta productoExterno = new ProvisionVentaDirecta(a.Id, a.Provision.Id, a.Provision.nombre, a.Provision.noExistencias, a.Provision.ubicacion, a.Provision.stockMinimo, a.Provision.costoUnitario, a.Provision.unidadMedida, a.Provision.activado, a.descripcion, a.restricciones, a.Categoria.categoria);
                    pedidoADomicilio.ProductosExternos.Add(productoExterno);
                }
            }
        }*/
    }
}
