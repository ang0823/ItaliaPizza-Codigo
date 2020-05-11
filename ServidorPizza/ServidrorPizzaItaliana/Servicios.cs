using System;
using AccesoBD2;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Threading;
using System.Data.Entity;
using System.ComponentModel.Design;

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

                Callback2.Respuesta("Cambios Guardados");
            }
            catch (InvalidOperationException)
            {
                Callback2.Respuesta("Error al guardar cambios");
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
        public void RegistrarProducto(Provision provision)
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

    public partial class Servicios : IEditararIngrediente
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
}
