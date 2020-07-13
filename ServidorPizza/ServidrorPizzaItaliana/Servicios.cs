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
using System.Data.Common;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Core.Metadata.Edm;
using System.Collections;
using System.Globalization;
using System.Security.Cryptography;

namespace ServidrorPizzaItaliana
{

    [ServiceBehavior(ConcurrencyMode = System.ServiceModel.ConcurrencyMode.Single, InstanceContextMode = InstanceContextMode.Single)]

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
                var productoInterno = db.ProductoSet.Where(x => x.nombre == nombreProducto).FirstOrDefault();
                var receta = db.RecetaSet.Where(x => x.Producto.nombre == nombreProducto).Select(x => x.nombreReceta).FirstOrDefault();
                var categoria = db.CategoriaSet.Where(x => x.Producto.FirstOrDefault().nombre == nombreProducto).Select(x => x.categoria).FirstOrDefault();

                if (productoInterno != null)
                {
                    Callback.ProductoInterno(productoInterno, ObtenerImagen(nombreProducto), receta, categoria);
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
                var provision = db.ProvisionDirectaSet.Where(x => x.Provision.nombre == nombreProducto).Include(x => x.Categoria).Include(x => x.Provision).FirstOrDefault();

                if (provision != null)
                {
                    ProvisionVentaDirecta productoExterno = new ProvisionVentaDirecta(provision.Id, provision.Provision.Id, provision.Provision.nombre, provision.Provision.noExistencias, provision.Provision.ubicacion, provision.Provision.stockMinimo, provision.Provision.costoUnitario, provision.Provision.unidadMedida, provision.activado, provision.descripcion, provision.restricciones, provision.Categoria.categoria)
                    {
                        Imagen = ObtenerImagen(nombreProducto)
                    };

                    Callback.ProductoExterno(productoExterno);
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
                    var username = (from c in db.CuentaUsuarioSet where c.nombreUsuario == cuenta.nombreUsuario select c).FirstOrDefault();
                    if (username == null)
                    {
                        var roldb = (from p in db.RolSet where p.Id == rol select p).FirstOrDefault();

                        empleado.Rol = roldb;
                        empleado.Direccion = direccion;
                        cuenta.Empleado = empleado;
                        db.CuentaUsuarioSet.Add(cuenta);
                        db.SaveChanges();
                        OperationContext.Current.GetCallbackChannel<IRegistrarCuentaUsuarioCallback>().RespuestaRCU("Éxito al cuenta de usuario");
                    }
                    else
                    {
                        OperationContext.Current.GetCallbackChannel<IRegistrarCuentaUsuarioCallback>().RespuestaRCU("El nombre de usuario introducido ya se encuentra en uso");
                    }
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
        public void ModificarCuentaUsuario(CuentaUsuario cuenta, Empleado empleado, Direccion direccion, string nombreRol)
        {
            try
            {
                db.Configuration.ProxyCreationEnabled = false;
                var empleadoEncontrado = (from c in db.EmpleadoSet where c.IdEmpleado == empleado.IdEmpleado select c).FirstOrDefault();
                var rol = (from r in db.RolSet where r.nombreRol == nombreRol select r).FirstOrDefault();
                empleadoEncontrado.Rol = rol;
                empleadoEncontrado.nombre = empleado.nombre;
                empleadoEncontrado.apellidoPaterno = empleado.apellidoPaterno;
                empleadoEncontrado.apellidoMaterno = empleado.apellidoMaterno;
                empleadoEncontrado.correo = empleado.correo;
                empleadoEncontrado.telefono = empleado.telefono;
                db.EmpleadoSet.Attach(empleadoEncontrado);
                db.Entry(empleadoEncontrado).State = EntityState.Modified;

                var direccionEncontrada = (from d in db.DireccionSet where d.Id == direccion.Id select d).FirstOrDefault();
                direccionEncontrada.calle = direccion.calle;
                direccionEncontrada.colonia = direccion.colonia;
                direccionEncontrada.numeroInterior = direccion.numeroInterior;
                direccionEncontrada.numeroExterior = direccion.numeroExterior;
                direccionEncontrada.codigoPostal = direccion.codigoPostal;
                db.DireccionSet.Attach(direccionEncontrada);
                db.Entry(direccionEncontrada).State = EntityState.Modified;
                db.SaveChanges();

                var cuentaEncontrada = (from c in db.CuentaUsuarioSet where c.Empleado.IdEmpleado == empleado.IdEmpleado select c)
                    .FirstOrDefault();

                if (cuentaEncontrada == null)
                {
                    db.CuentaUsuarioSet.Add(cuenta);
                    cuenta.Empleado = empleadoEncontrado;
                    db.SaveChanges();
                }
                else
                {
                    cuentaEncontrada.contraseña = cuenta.contraseña;
                    cuenta.Empleado = empleadoEncontrado;
                    db.CuentaUsuarioSet.Attach(cuentaEncontrada);
                    db.Entry(cuentaEncontrada).State = EntityState.Modified;
                    db.SaveChanges();
                }
                
                OperationContext.Current.GetCallbackChannel<IModificarCuentaUsuarioCallback>().RespuestaMCU("Se modificó correctamente");
                Console.WriteLine("Se modificó correctamente");

            }
            catch (InvalidOperationException db)
            {
                OperationContext.Current.GetCallbackChannel<IModificarCuentaUsuarioCallback>().RespuestaMCU(db.Message + "\n\n" 
                    + db.InnerException + "\n\n" 
                    + db.StackTrace + "\n\n" 
                    + db.Source);
            }
        }

        public void ModificarCuentaUsuario2(Empleado empleado, Direccion direccion, string nombreRol)
        {
            try
            {
                var rol = (from r in db.RolSet where r.nombreRol == nombreRol select r).FirstOrDefault();
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
            catch (NullReferenceException)
            {
                OperationContext.Current.GetCallbackChannel<IObtenerCuentasCallback>().RespuestaOCU("No se encontraron resultados con el id especificado");
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
                                 select p).FirstOrDefault();

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
            catch (ArgumentNullException)
            {
                OperationContext.Current.GetCallbackChannel<IEliminarCuentaUsuarioCallback>().RespuestaECU("Error al intentar acceder a la base de datos");
            }
        }
    }

    public partial class Servicios : IRegistrarProducto
    {
        public void RegistrarProducto(AccesoBD2.Producto producto, Categoria categoria, string receta, byte[] imagen)
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
                    var recetadb = (from p in db.RecetaSet where p.nombreReceta == receta select p).FirstOrDefault();

                    producto.Receta = recetadb;
                    producto.Categoria = categoria;
                    db.ProductoSet.Add(producto);
                    db.SaveChanges();
                    GuardarImagen(imagen, producto.nombre);
                    OperationContext.Current.GetCallbackChannel<IRegistrarProductoCallback>().RespuestaRP("Guardado");
                }
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine(e.StackTrace);
                OperationContext.Current.GetCallbackChannel<IRegistrarRecetaCallback>().RespuestaRR("Ocurrio un error al registrarReceta");
            }
            catch(Exception e)
            {
                OperationContext.Current.GetCallbackChannel<IRegistrarRecetaCallback>().RespuestaRR(e.GetType() + " " + e.Message);
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
                            Provision provision = new Provision();
                            provision.Id = valor.Id;
                            provision.nombre = valor.nombre;
                            provision.noExistencias = valor.noExistencias;
                            provision.ubicacion = valor.ubicacion;
                            provision.stockMinimo = valor.stockMinimo;
                            provision.costoUnitario = valor.costoUnitario;
                            provision.unidadMedida = valor.unidadMedida;
                            provisionlista.Add(provision);                        
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
                    receta.activado = true;
                    db.RecetaSet.AddOrUpdate(receta);
                    db.SaveChanges();
                    OperationContext.Current.GetCallbackChannel<IRegistrarRecetaCallback>().RespuestaRR("Éxito al registrar la receta");
                }
            }
            catch(DbEntityValidationException)
            {
                OperationContext.Current.GetCallbackChannel<IRegistrarRecetaCallback>().RespuestaRR("Faltan detalles de las porciones en alguno de los ingredientes, favor de incluirlos para poder continuar.");
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
                r.Ingrediente = ingredinetes;
                db.RecetaSet.Attach(r);
                db.Entry(r).State = EntityState.Modified;
                db.SaveChanges();
                
                OperationContext.Current.GetCallbackChannel<IEditarRecetaCallback>().RespuestaER("Se modificó correctamente");
                Console.WriteLine("Se modificó correctamente");

            }
            catch (Exception e)
            {
                OperationContext.Current.GetCallbackChannel<IEditarRecetaCallback>().RespuestaER(e.GetType()+": "+e.Message);
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
                OperationContext.Current.GetCallbackChannel<IObtenerRecetasCallback>().RespuestaIOR("No se encontro receta con el nombre " + nombre);
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

        public void ModificarProductoInterno(AccesoBD2.Producto producto, string nombreReceta, string antiguoNombreImagen, byte[] imagen)
        {
            try
            {
                if (producto != null)
                {
                    var receta = db.RecetaSet.Where(x => x.nombreReceta == nombreReceta).FirstOrDefault();

                    producto.Receta = receta;

                    db.ProductoSet.AddOrUpdate(producto);
                    db.SaveChanges();

                    EliminarImagen(antiguoNombreImagen);
                    GuardarImagen(imagen, producto.nombre);

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

        public void ModificarProductoExterno(ProvisionVentaDirecta productoExterno, string antiguoNombreImagen)
        {
            try
            {
                if (productoExterno != null)
                {
                    ProvisionDirecta provisionDirecta = new ProvisionDirecta()
                    {
                        Id = productoExterno.IdProvisionVentaDirecta,
                        descripcion = productoExterno.Descripcion,
                        activado = productoExterno.Activado,
                        restricciones = productoExterno.Restricciones
                    };

                    Provision provision = new Provision()
                    {
                        Id = productoExterno.IdProvision,
                        nombre = productoExterno.Nombre,
                        noExistencias = productoExterno.CantidadExistencias,
                        ubicacion = productoExterno.Ubicacion,
                        stockMinimo = productoExterno.Stock,
                        costoUnitario = productoExterno.PrecioUnitario,
                        unidadMedida = productoExterno.UnidadDeMedida,
                        activado = productoExterno.Activado
                    };

                    db.ProvisionDirectaSet.AddOrUpdate(provisionDirecta);
                    db.ProvisionSet.AddOrUpdate(provision);
                    db.SaveChanges();

                    EliminarImagen(antiguoNombreImagen);
                    GuardarImagen(productoExterno.Imagen, provision.nombre);


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

        public void ObtenerNombresDeRecetas()
        {
            var recetas = db.RecetaSet.Where(x => x.Producto == null).Select(x => x.nombreReceta).ToList();

            Callback2.ListaDeRecetas(recetas);
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
                    Callback3.Respuesta("La provision con el nombre: " + provision.nombre + " ya esta registrado");
                }
                else
                {
                    
                    db.ProvisionSet.Add(provision);
                    db.SaveChanges();
                    Callback3.Respuesta("Registro exitoso");
                }
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine(e.StackTrace);
                Callback3.Respuesta("Ocurrió un error al guardar los datos del ingrediente");
            }
        }

        public void RegistrarProvisionDirecta(Provision provision, ProvisionDirecta provisionDirecta, byte[] imagen)
        {
            try
            {
                var provisionEncontrada = (from p in db.ProvisionSet where p.nombre == provision.nombre select p).FirstOrDefault();

                if (provisionEncontrada != null)
                {
                    Callback3.Respuesta("La provision con el nombre: " + provision.nombre + " ya esta registrado");
                }
                else
                {
                    db.RegistrarProvisionDirecta(provision.nombre, provision.noExistencias, provision.ubicacion, provision.stockMinimo, provision.costoUnitario, provision.unidadMedida, provision.activado, provisionDirecta.descripcion, provisionDirecta.activado, provisionDirecta.restricciones, provisionDirecta.Categoria.categoria);
                    GuardarImagen(imagen, provision.nombre);
                    Callback3.Respuesta("Registro exitoso");
                }
            }catch(Exception e)
            {
                Console.WriteLine(e.Message + "\n" + e.GetType() + "\n" + e.StackTrace);
                Callback3.Respuesta("Ocurrió un error al guardar los datos del ingrediente");
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
                var ingrediente = (from prod in db.ProvisionSet where prod.nombre == nombreProducto select prod).FirstOrDefault();

                if(ingrediente != null)
                {
                    Callback4.Ingrediente(ingrediente);
                }
                else
                {
                    Callback4.ErrorAlRecuperarIngrediente("No hay algun ingrediente con ese nombre");
                } 
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
            catch (Exception)
            {
                Callback5.RespuestaEditarIngrediente("Ocurrió un error al tratar de guardar las modificaciones");                
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

                db.InsertarPedidoADomicilio(pedido.ClienteId, pedido.fecha, pedido.instruccionesEspeciales, pedido.Empleado.IdEmpleado, pedido.Estado.estadoPedido, pedido.Cuenta.Id, pedido.Cuenta.precioTotal, pedido.Cuenta.subTotal, pedido.Cuenta.iva, pedido.Cuenta.descuento, true, pedido.direccionDestino, idPedido);

                int idPedidoRegistrado = Convert.ToInt32(idPedido.Value);

                foreach (AccesoBD2.Producto p in pedido.Producto)
                {
                    db.LigarProductoConPedido(p.Id, idPedidoRegistrado, 2);
                }

                foreach (ProvisionDirecta pd in pedido.ProvisionDirecta)
                {
                    db.LigarProvisionConPedido(pd.Id, idPedidoRegistrado, 2);
                }
                pedido.Id = idPedidoRegistrado;
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
                    ObjectParameter idCliente = new ObjectParameter("IDCliente", typeof(int));
                    db.RegistroDeClienteConDireccion(cliente.nombre, cliente.apellidoPaterno, cliente.apellidoMaterno, direccionCliente.calle, direccionCliente.colonia, direccionCliente.numeroExterior, direccionCliente.numeroInterior, telefonoCliente.numeroTelefono, direccionCliente.codigoPostal, idCliente);

                    List<DireccionCliente> direcciones = new List<DireccionCliente>();
                    List<TelefonoCliente> telefonos = new List<TelefonoCliente>();
                    
                    DireccionCliente dir = new DireccionCliente(direccionCliente.calle, direccionCliente.colonia, direccionCliente.numeroExterior, direccionCliente.numeroInterior, direccionCliente.codigoPostal);
                    direcciones.Add(dir);
                    
                    TelefonoCliente tel = new TelefonoCliente(telefonoCliente.numeroTelefono);
                    telefonos.Add(tel);
                    
                    Cliente clienteRegistrado = new Cliente(cliente.Id, cliente.nombre, cliente.apellidoPaterno, cliente.apellidoMaterno, direcciones, telefonos);

                    Callback7.NotificacionClienteDePedido("Exito al registrar cliente", clienteRegistrado);
                }
            }
            catch (Exception)
            {
                Callback7.Mensaje("Error al registrar cliente");
            }
        }

        public bool ModificarDatosPedidoADomicilio(PedidoADomicilio pedido)
        {
            try
            {
                Pedido pedidoRecuperado = (from ped in db.PedidoSet where ped.Cuenta.Id == pedido.Cuenta.Id select ped).Include(x => x.Producto).Include(pd => pd.ProvisionDirecta).First();
                pedidoRecuperado.Producto.Clear();
                pedidoRecuperado.ProvisionDirecta.Clear();
                db.PedidoSet.Attach(pedidoRecuperado);
                db.Entry(pedidoRecuperado).State = EntityState.Modified;
                db.SaveChanges();

                db.ModificarPedidoADomicilio(pedido.Id, pedido.instruccionesEspeciales, pedido.Empleado.IdEmpleado, pedido.Cuenta.Id, pedido.Cuenta.precioTotal, pedido.Cuenta.subTotal, pedido.Cuenta.descuento, true, pedido.ClienteId, pedido.direccionDestino);

                foreach (AccesoBD2.Producto producto in pedido.Producto)
                {
                    db.LigarProductoConPedido(producto.Id, pedido.Id, producto.cantidad);
                }
                foreach (AccesoBD2.ProvisionDirecta provision in pedido.ProvisionDirecta)
                {
                    db.LigarProvisionConPedido(provision.Id, pedido.Id, provision.cantidad);
                }
                NotificarPedidoADomicilio(pedido);
                return true;

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + "\n" + e.StackTrace);
                return false;
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

        public bool RegistrarPedidoLocal(PedidoLocal pedido)
        {
            if (ServicioRegistrarPedidoLocal(pedido))
            {
                NotificarATodosPedidoLocal(pedido);
                return true;
            }
            else
            {                
                Callback8.MensajeAdministrarPedidosMeseros("Error al registrar pedido local");
                return false;
            }
        }

        public bool ModificarDatosPedidoLocal(PedidoLocal pedido)
        {
            try
            {
                Pedido pedidoRecuperado = (from ped in db.PedidoSet where ped.Cuenta.Id == pedido.Cuenta.Id select ped).Include(x => x.Producto).Include(pd => pd.ProvisionDirecta).First();
                pedidoRecuperado.Producto.Clear();
                pedidoRecuperado.ProvisionDirecta.Clear();
                db.PedidoSet.Attach(pedidoRecuperado);
                db.Entry(pedidoRecuperado).State = EntityState.Modified;
                db.SaveChanges();

                db.ModificarPedidoLocal(pedido.Id, pedido.instruccionesEspeciales, pedido.Empleado.IdEmpleado, pedido.Cuenta.Id, pedido.Cuenta.precioTotal, pedido.Cuenta.subTotal, pedido.Cuenta.descuento, true, pedido.Mesa.numeroMesa);

                foreach(AccesoBD2.Producto producto in pedido.Producto)
                {
                    db.LigarProductoConPedido(producto.Id, pedido.Id, producto.cantidad);
                }
                foreach (AccesoBD2.ProvisionDirecta provision in pedido.ProvisionDirecta)
                {
                    db.LigarProvisionConPedido(provision.Id, pedido.Id, provision.cantidad);
                }
                NotificarATodosPedidoLocal(pedido);
                return true;
            
            }catch(Exception e)
            {
                Console.WriteLine(e.Message +"\n" +e.StackTrace);
                return false;
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
            OperationContext.Current.Channel.Closed += delegate { Desconectar(); };
            OperationContext.Current.Channel.Faulted += delegate { Desconectar(); };
            Console.WriteLine($"{tipoUsuario} conectado. \n {usuarios.Count} usuarios connectados.");
        }
        public void Desconectar()
        {
            var usuarioAEliminar = usuarios.ContainsKey(Callback9);
            if(usuarioAEliminar== true)
            {
                usuarios.Remove(Callback9);
                Console.WriteLine("Se ha desconectado un usuario" );
            }            
        }

        public void ModificarEstadoPedidoLocal(PedidoLocal pedido)
        {
            if (ModififcarPedido(pedido))
            {
                if(pedido.Estado.estadoPedido.Equals("Cancelado"))
                    NotificarATodosPedidoLocal(pedido);

                else if (pedido.Estado.estadoPedido.Equals(ESTADOFINAL))
                {
                    if (DisminuirExsistenciasDeProductoExterno(pedido.ProvisionDirecta.ToList()))
                    {
                        NotificarPedidoLocalExceptoACocinero(pedido);
                        //NotificarATodosPedidoLocal(pedido); //Notificar a todos menos al Cocinero
                    }
                    else
                    {
                        Callback9.MensajeNotificarPedido("Error al disminuir stock de productos");
                    }
                }
                else
                {
                    NotificarPedidoLocalExceptoACocinero(pedido);
                    //NotificarATodosPedidoLocal(pedido); //Notificar a todos menos al Cocinero
                }
            }
            else
            {
                Callback9.MensajeNotificarPedido("Error al modificar pedido");
            }
        }

        public void ModificarEstadoPedidoADomicilio(PedidoADomicilio pedido)
        {
            if (ModififcarPedido(pedido))
            {
                if (pedido.Estado.estadoPedido == "Cancelado")
                    NotificarPedidoADomicilio(pedido);               
                else if (pedido.Estado.estadoPedido.Equals(ESTADOFINAL))
                {
                    if (DisminuirExsistenciasDeProductoExterno(pedido.ProvisionDirecta.ToList()))
                    {
                        NotificarPedidoADomicilioACallCenters(pedido);
                        //NotificarPedidoADomicilio(pedido); //notificar solo al callcenter
                    }
                    else
                    {
                        Callback9.MensajeNotificarPedido("Error al disminuir stock de productos");
                    }
                }
                else                
                    NotificarPedidoADomicilioACallCenters(pedido);                                   
            }
            else
            {
                Callback9.MensajeNotificarPedido("Error al modificar pedido");
            }
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

        public void NotificarPedidoADomicilioACallCenters(PedidoADomicilio pedido)
        {
            foreach (var destinatario in usuarios)
            {
                if (destinatario.Value.Equals("Call Center"))
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

        public void NotificarPedidoLocalExceptoACocinero(PedidoLocal pedido)
        {
            foreach (var destinatario in usuarios)
            {
                if (!destinatario.Value.Equals("Cocinero"))
                {
                    destinatario.Key.RecibirPedidoLocal(pedido);
                }
            }
        }

        public void NotificarPedidoADomicilioPreparado(PedidoADomicilio pedido, string usuario)
        {
            if (ModififcarPedido(pedido) && DisminuirExistenciasDeIngrediente(pedido.Producto.ToList()))
            {
                foreach (var destinatario in usuarios)
                {
                    if (destinatario.Value.Equals("Call Center"))
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
        private List<PedidoADomicilioDeServidor> pedidosADomicilio;
        private List<PedidoLocalDeServidor> pedidosLocalesDeServidor;
        private static DateTime fecha = DateTime.Now;
        private string fechaDelDia = fecha.ToString("dd/MM/yyyy");
        private const string IDPEDIDOLOCAL = "PL";
        private const string IDPEDIDOADOMICILIO = "PD";

        public void BuscarPedidosCallCenter()
        {
           
            db.Configuration.LazyLoadingEnabled = false;
            pedidosADomicilio = new List<PedidoADomicilioDeServidor>();
            pedidosLocalesDeServidor = new List<PedidoLocalDeServidor>();

            try
            {
                if (ObtenerPedidosADomicilio() && ObtenerPedidosLocales())
                {
                    if (pedidosADomicilio.Count > 0 || pedidosLocalesDeServidor.Count > 0)
                    {
                        Callback10.ObtenerTodosPedidos(pedidosADomicilio, pedidosLocalesDeServidor);
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
                db.Configuration.LazyLoadingEnabled = true;
            }catch(CommunicationObjectAbortedException e)
            {
                Console.WriteLine(e.InnerException + " " + e.Message);
            }catch(CommunicationObjectFaultedException e)
            {
                Console.WriteLine(e.InnerException + " " + e.Message);
            }
           
        }

        public void BuscarPedidosMesero()
        {
            db.Configuration.LazyLoadingEnabled = false;
            pedidosADomicilio = new List<PedidoADomicilioDeServidor>();
            pedidosLocalesDeServidor = new List<PedidoLocalDeServidor>();
            if (ObtenerPedidosLocales())
            {
                if(pedidosLocalesDeServidor.Count > 0)
                {
                    Callback10.ObtenerPedidosLocales(pedidosLocalesDeServidor);
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
            db.Configuration.LazyLoadingEnabled = true;
        }

        public bool ObtenerPedidosADomicilio()
        {
            bool exitoAlObtenerPedidos = false;

            try
            {
                //var pedidosDomicilio = db.PedidoSet.Where(x => x.Cuenta.Id.Contains(IDPEDIDOADOMICILIO)).Where(x => x.Cuenta.Id.Contains(fechaDelDia)).Include(x => x.Empleado).Include(x => x.Producto.Select(j => j.Categoria)).Include(x => x.ProvisionDirecta.Select(j => j.Provision)).Include(x => x.Estado).Include(x => x.Cuenta).ToList();

                var pedidosDomicilio = db.PedidoSet.Where(p => p.Cuenta.Id.Contains(IDPEDIDOADOMICILIO) && p.Cuenta.Id.Contains(fechaDelDia)).Include("Cuenta").Include("Empleado").Include("Estado").ToList();

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
                    CuentaDePedido cuenta = new CuentaDePedido(pedido.Cuenta.Id, pedido.Cuenta.precioTotal, pedido.Cuenta.subTotal, pedido.Cuenta.iva, descuento, pedido.Cuenta.abierta);                    
                  
                    pedidoADomicilio.Estado = pedido.Estado.estadoPedido;
                    pedidoADomicilio.Cuenta = cuenta;
                    pedidoADomicilio.Id = pedido.Id;
                    pedidoADomicilio.Fecha = pedido.fecha;
                    pedidoADomicilio.InstruccionesDePedido = pedido.instruccionesEspeciales;
                    pedidoADomicilio.IdEmpleado = pedido.Empleado.IdEmpleado;
                    pedidoADomicilio.IdGeneradoDeEmpleado = pedido.Empleado.idEmpleadoGenerado;
                    pedidoADomicilio.DireccionDestino = cliente.PedidoADomicilio.First().direccionDestino;
                   
                    var listaProductos = db.MostrarCantidadProductosPedido(pedido.Id).ToList();
                    foreach (var producto in listaProductos)
                    {
                        ProductoDePedido productoLocal = new ProductoDePedido((int)producto.Id, producto.nombre, producto.descripcion, (double)producto.precioUnitario, producto.restricciones, producto.categoria, (int)producto.cantidad);
                        pedidoADomicilio.ProductosLocales.Add(productoLocal);
                    }
                    
                    var listaProvisiones = db.MostrarCantidadProvisionesPedido(pedido.Id).ToList();
                    foreach(var provision in listaProvisiones)
                    {
                        ProvisionVentaDirecta productoExterno = new ProvisionVentaDirecta((int)provision.Id, (int)provision.Provision_Id, provision.nombre, provision.noExistencias, provision.ubicacion, provision.stockMinimo, provision.costoUnitario, provision.unidadMedida, (bool)provision.activado, provision.descripcion, provision.restricciones, provision.categoria, (int)provision.cantidad);
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
                //  var pedidosLocales = db.PedidoSet.Where(x => x.Cuenta.Id.Contains(IDPEDIDOLOCAL)).Where(x => x.Cuenta.Id.Contains(fechaDelDia)).Include(x => x.Empleado).Include(x => x.Producto.Select(j => j.Categoria)).Include(x => x.ProvisionDirecta.Select(j => j.Provision)).Include(x => x.Estado).Include(x => x.Cuenta).ToList();
                var pedidosLocales = db.PedidoSet.Where(p => p.Cuenta.Id.Contains(IDPEDIDOLOCAL) && p.Cuenta.Id.Contains(fechaDelDia)).Include("Cuenta").Include("Estado").Include("Empleado").ToList();
                
                foreach (Pedido pedido in pedidosLocales)
                {  
                    var mesa = db.MesaSet.Where(x => x.PedidoLocal.Any(j => j.Id == pedido.Id)).FirstOrDefault();
                    PedidoLocalDeServidor pedidoLocal = new PedidoLocalDeServidor(mesa.Id, mesa.numeroMesa);

                    double descuento = Convert.ToDouble(pedido.Cuenta.descuento.Value);
                    CuentaDePedido cuenta = new CuentaDePedido(pedido.Cuenta.Id, pedido.Cuenta.precioTotal, pedido.Cuenta.subTotal, pedido.Cuenta.iva, descuento, pedido.Cuenta.abierta);
   
                    pedidoLocal.Estado = pedido.Estado.estadoPedido;
                    pedidoLocal.Cuenta = cuenta;
                    pedidoLocal.Id = pedido.Id;
                    pedidoLocal.Fecha = pedido.fecha;
                    pedidoLocal.InstruccionesDePedido = pedido.instruccionesEspeciales;
                    pedidoLocal.IdEmpleado = pedido.Empleado.IdEmpleado;
                    pedidoLocal.IdGeneradoDeEmpleado = pedido.Empleado.idEmpleadoGenerado;
                    var y = db.ChangeTracker;
                   
                    var listaProductosPedido = db.MostrarCantidadProductosPedido(pedido.Id).ToList();
                    foreach(var producto in listaProductosPedido)
                    {
                        ProductoDePedido productoLocal = new ProductoDePedido((int)producto.Id, producto.nombre, producto.descripcion, (double)producto.precioUnitario, producto.restricciones, producto.categoria, (int)producto.cantidad);
                        pedidoLocal.ProductosLocales.Add(productoLocal);
                    }
                                           
                    var listaProvisionesPedido = db.MostrarCantidadProvisionesPedido(pedido.Id).ToList();
                    foreach (var provision in listaProvisionesPedido)
                    {
                        ProvisionVentaDirecta productoExterno = new ProvisionVentaDirecta((int)provision.Id, (int)provision.Provision_Id, provision.nombre, provision.noExistencias, provision.ubicacion, provision.stockMinimo, provision.costoUnitario, provision.unidadMedida, (bool)provision.activado, provision.descripcion, provision.restricciones, provision.categoria, (int)provision.cantidad);
                        pedidoLocal.ProductosExternos.Add(productoExterno);
                    }
                    pedidosLocalesDeServidor.Add(pedidoLocal);
                }

                return exitoAlObtenerPedidos = true;
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine("Ha ocurrido una excepcion " + ex.Message);
                return exitoAlObtenerPedidos;
            }
        }

        public IBuscarPedidosCallback Callback10
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
                var mesa = (from m in db.MesaSet where m.numeroMesa == pedido.Mesa.numeroMesa select m).FirstOrDefault();

                if (mesa == null)
                {
                    db.MesaSet.Add(new Mesa
                    {
                        numeroMesa = pedido.Mesa.numeroMesa
                    });
                    if (db.SaveChanges() == 0) return exitoAlRegistrarPedido;
                }                

                db.InsertarPedidoLocal(pedido.Mesa.numeroMesa, pedido.fecha, pedido.instruccionesEspeciales, pedido.Empleado.IdEmpleado, pedido.Estado.estadoPedido, pedido.Cuenta.Id, pedido.Cuenta.precioTotal, pedido.Cuenta.subTotal, pedido.Cuenta.iva, pedido.Cuenta.descuento, true, IdPedido);

                int idPedidoRegistrado = Convert.ToInt32(IdPedido.Value);

                foreach (AccesoBD2.Producto p in pedido.Producto)
                {
                    db.LigarProductoConPedido(p.Id, idPedidoRegistrado, p.cantidad);
                }
               

                foreach (ProvisionDirecta pd in pedido.ProvisionDirecta)
                {
                    db.LigarProvisionConPedido(pd.Id, idPedidoRegistrado, pd.cantidad);
                }
                pedido.Id = idPedidoRegistrado;
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
                var meserosObtenidos = db.EmpleadoSet.Where(x => x.Rol.nombreRol == ROL_DE_EMPLEADO).ToList();

                foreach (Empleado mesero in meserosObtenidos)
                {
                    EmpleadoPizzeria empleado = new EmpleadoPizzeria(mesero.IdEmpleado, mesero.idEmpleadoGenerado);
                    meseros.Add(empleado);
                }

            }
            catch (Exception excepcion)
            {
                Console.WriteLine(excepcion.Message);
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
                    ProductoDePedido productoRecuperado = new ProductoDePedido(a.Id, a.nombre, a.descripcion, a.precioUnitario, a.restricciones, a.Categoria.categoria, 0);
                    //productoRecuperado.Imagen = ObtenerImagen(a.nombre);
                    productos.Add(productoRecuperado);
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + " \n " + e.StackTrace);
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
                    ProvisionVentaDirecta provisionRecuperada = new ProvisionVentaDirecta(a.Id, a.Provision.Id, a.Provision.nombre, a.Provision.noExistencias, a.Provision.ubicacion, a.Provision.stockMinimo, a.Provision.costoUnitario, a.Provision.unidadMedida, a.Provision.activado, a.descripcion, a.restricciones, a.Categoria.categoria, 0);
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
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
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
                var pedidoRecuperado = (from ped in db.PedidoSet where ped.Cuenta.Id == pedido.Cuenta.Id select ped).First();
                var estadoRecuperado = (from estado in db.EstadoSet where estado.estadoPedido == pedido.Estado.estadoPedido select estado).FirstOrDefault();

                pedidoRecuperado.Estado = estadoRecuperado;               
                db.PedidoSet.Attach(pedidoRecuperado);
                db.Entry(pedidoRecuperado).State = EntityState.Modified;
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
            File.Delete("ImagenesDeProductos/" + nombreImagen + ".jpg");
        }

        public void GuardarImagen(byte[] arrayImagen, string nombreDeImagen)
        {
            Image imagen = (Bitmap)((new ImageConverter()).ConvertFrom(arrayImagen));
            imagen.Save("ImagenesDeProductos/" + nombreDeImagen + ".jpg", ImageFormat.Jpeg);
        }

        public byte[] ObtenerImagen(string nombreImagen)
        {
            byte[] imagen;

            Stream archivo = new FileStream("ImagenesDeProductos/" + nombreImagen + ".jpg", FileMode.Open, FileAccess.Read);

            using (MemoryStream ms = new MemoryStream())
            {
                archivo.CopyTo(ms);
                imagen = ms.ToArray();
            }

            archivo.Close();

            return imagen;
        }
    }
}
