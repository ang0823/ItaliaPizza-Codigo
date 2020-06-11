﻿using System;
using AccesoBD2;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Threading;
using System.Data.Entity;
using System.ComponentModel.Design;
using System.Data.Entity.Validation;
using System.Data.Entity.Migrations;

namespace ServidrorPizzaItaliana
{

    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]

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
                    Callback.ProductoInterno(productoInterno);
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
                        Callback.ProductoExterno(provisionDeProducto, provisionDirecta);
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

                db.CuentaUsuarioSet.Attach(c);
                db.Entry(c).State = EntityState.Modified;
                db.SaveChanges();
                var rol = (from r in db.RolSet where r.Id == idrol select r).FirstOrDefault();
                Empleado e = new Empleado();
                e = empleado;
                e.Rol = rol;
                db.EmpleadoSet.Attach(e);
                db.Entry(e).State = EntityState.Modified;
                db.SaveChanges();

                Direccion d = new Direccion();
                d = direccion;
                db.DireccionSet.Attach(d);
                db.Entry(d).State = EntityState.Modified;
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
                Empleado e = new Empleado();
                e = empleado;
                e.Rol = rol;
                db.EmpleadoSet.Attach(e);
                db.Entry(e).State = EntityState.Modified;
                db.SaveChanges();

                Direccion d = new Direccion();
                d = direccion;
                db.DireccionSet.Attach(d);
                db.Entry(d).State = EntityState.Modified;
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
                OperationContext.Current.GetCallbackChannel<IGenerarRespaldoCallback>().RespuestaGR("Se modificó correctamente");
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
                    OperationContext.Current.GetCallbackChannel<IRegistrarProductoCallback>().RespuestaRP("Ocurrio un error al intentar acceder a la base de datos intentelo más tarde");
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
                        if (valor.activado == true)
                        {
                            provisionlista.Add(new Provision1(valor.Id, valor.nombre, valor.noExistencias, valor.ubicacion, valor.stockMinimo, valor.costoUnitario, valor.unidadMedida));
                        }
                    }
                    foreach (var valor in pDirectas)
                    {
                        if (valor.activado == true)
                        {
                            pDirectalista.Add(new ProvisionDirecta1(valor.Id, valor.descripcion, valor.activado, valor.restricciones));
                        }
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
        public void ModificarProvisionDirectaDeProductoExterno(ProvisionDirecta provisionDirecta)
        {
            try
            {
                if (provisionDirecta != null)
                {
                    ProvisionDirecta provisionDirectaDeProducto = new ProvisionDirecta();
                    provisionDirectaDeProducto = provisionDirecta;
                    db.ProvisionDirectaSet.AddOrUpdate(provisionDirectaDeProducto);
                    db.SaveChanges();
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

        public void ModificarProvisionDeProductoExterno(Provision provision)
        {
            try
            {
                if (provision != null)
                {
                    Provision provisionDeProducto = new Provision();
                    provisionDeProducto = provision;

                    db.ProvisionSet.AddOrUpdate(provisionDeProducto);
                    db.SaveChanges();
                    Callback2.RespuestaModificarProducto("Cambios Guardados");
                }
                else
                {
                    Callback2.RespuestaModificarProducto("Los campos de la provision no pueden ser nulos");
                }

            }


            catch (InvalidOperationException)
            {
                Callback2.RespuestaModificarProducto("Ocurrio un error al modificar provision de producto");
            }
        }

        public void ModificarProductoInterno(AccesoBD2.Producto producto)
        {
            try
            {
                if (producto != null)
                {
                    AccesoBD2.Producto productoInterno = new AccesoBD2.Producto();
                    productoInterno = producto;

                    db.ProductoSet.AddOrUpdate(productoInterno);
                    db.SaveChanges();
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
                    db.RegistroDeClienteConDireccion(cliente.nombre, cliente.apellidoPaterno, cliente.apellidoMaterno, direccionCliente.calle, direccionCliente.colonia, direccionCliente.numeroExterior, direccionCliente.numeroInterior, direccionCliente.codigoPostal);
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
                    ProvisionVentaDirecta provisionRecuperada = new ProvisionVentaDirecta(a.Id, a.Provision.Id, a.Provision.nombre, a.Provision.costoUnitario, a.descripcion, a.restricciones);
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

    public partial class Servicios : INotificarPedido
    {
        Dictionary<INotificarPedidoCallback, string> usuarios = new Dictionary<INotificarPedidoCallback, string>();

        public void AgregarUsuario(string tipoUsuario)
        {
            usuarios[Callback9] = tipoUsuario;
        }

        public void EnviarPedidoLocal(PedidoLocal pedido, string usuario)
        {
            foreach (var destinatario in usuarios)
            {
                if (!destinatario.Value.Equals(usuario))
                {
                    if (destinatario.Key == Callback9)
                        continue;
                    destinatario.Key.RecibirPedidoLocal(pedido);
                }
            }
        }

        public void EnviarPedidoADomicilio(PedidoADomicilio pedido, string usuario)
        {
            foreach (var destinatario in usuarios)
            {
                if (destinatario.Value.Equals(usuario))
                {
                    if (destinatario.Key == Callback9)
                        continue;
                    destinatario.Key.RecibirPedidoDomicilio(pedido);
                }
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

}
