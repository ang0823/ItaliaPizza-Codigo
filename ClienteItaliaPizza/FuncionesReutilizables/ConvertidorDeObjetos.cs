using ClienteItaliaPizza.Servicio;
using DevExpress.Xpf.Bars;
using System.Collections;
using System.Collections.Generic;

namespace ClienteItaliaPizza.Validacion
{
    public static class ConvertidorDeObjetos
    {
        static public ProvisionDirecta ProvisionVentaDirecta_A_ProvisionDirecta(ProvisionVentaDirecta provisionVentaDirecta)
        {
            ProvisionDirecta provisionDirecta = new ProvisionDirecta();
            provisionDirecta.Provision = new Provision();
            provisionDirecta.Id = provisionVentaDirecta.idProvisionVentaDirecta; 
            provisionDirecta.Provision.Id = provisionVentaDirecta.idProvision;    
            provisionDirecta.Provision.nombre = provisionVentaDirecta.nombre;
            provisionDirecta.Provision.costoUnitario = provisionVentaDirecta.precioUnitario;
            provisionDirecta.descripcion = provisionVentaDirecta.descripcion;
            provisionDirecta.restricciones = provisionVentaDirecta.restricciones;
            provisionDirecta.Provision.noExistencias = provisionVentaDirecta.cantidadExistencias;
            provisionDirecta.Provision.ubicacion = provisionVentaDirecta.ubicacion;
            provisionDirecta.Provision.stockMinimo = provisionVentaDirecta.stock;
            provisionDirecta.Provision.unidadMedida = provisionVentaDirecta.unidadDeMedida;
            provisionDirecta.cantidad = provisionVentaDirecta.cantidad;
            return provisionDirecta;           
        }

        static public Producto ProductoDePedido_A_Producto(ProductoDePedido productoDePedido)
        {
            Producto producto = new Producto
            {
                Id = productoDePedido.id,
                nombre = productoDePedido.nombre,
                descripcion = productoDePedido.descrpcion,
                precioUnitario = productoDePedido.precioUnitario,
                //activado = productoDePedido.activado,
                restricciones = productoDePedido.restricciones,
                Categoria = new Categoria {
                    categoria = productoDePedido.categoria
                },
                cantidad = productoDePedido.cantidad
            };
            return producto;
        }

        static public PedidoLocal PedidoLocalDeServidor_A_PedidoLocal(PedidoLocalDeServidor pedidoLocalDeServidor)
        {
            PedidoLocal pedidoLocal = new PedidoLocal
            {
                Id = pedidoLocalDeServidor.id,
                fecha = pedidoLocalDeServidor.fecha,
                instruccionesEspeciales = pedidoLocalDeServidor.instruccionesDePedido,
                Empleado = new Empleado
                {
                    IdEmpleado = pedidoLocalDeServidor.idEmpleado,
                    idEmpleadoGenerado = pedidoLocalDeServidor.idGeneradoDeEmpleado
                },
                Estado = new Estado
                {
                    estadoPedido = pedidoLocalDeServidor.estado
                },
                Cuenta = new Cuenta
                {
                    Id = pedidoLocalDeServidor.cuenta.Id,
                    precioTotal = pedidoLocalDeServidor.cuenta.toal,
                    subTotal= pedidoLocalDeServidor.cuenta.subtotal,
                    iva= pedidoLocalDeServidor.cuenta.iva,
                    descuento = pedidoLocalDeServidor.cuenta.descuento,
                    abierta = pedidoLocalDeServidor.cuenta.abierta
                },
                Mesa = new Mesa
                {
                    Id = pedidoLocalDeServidor.mesaId,
                    numeroMesa = (short)pedidoLocalDeServidor.numeroMesa
                },
                MesaId = pedidoLocalDeServidor.mesaId,                
            };

            List<Producto> productos = new List<Producto>();

            foreach(var productoLocal in pedidoLocalDeServidor.productosLocales)
            {
                var producto = ProductoDePedido_A_Producto(productoLocal);
                productos.Add(producto);
            }

            pedidoLocal.Producto = productos.ToArray();

            List<ProvisionDirecta> provisionesDirectas = new List<ProvisionDirecta>();

            foreach(var provisionVentaDirecta in pedidoLocalDeServidor.productosExternos)
            {
                var provisionDirecta = ProvisionVentaDirecta_A_ProvisionDirecta(provisionVentaDirecta);
                provisionesDirectas.Add(provisionDirecta);
            }

            pedidoLocal.ProvisionDirecta = provisionesDirectas.ToArray();
            return pedidoLocal;
        }

        static public PedidoADomicilio PedidoADomicilioDeServidor_A_PedidoADomicilio(PedidoADomicilioDeServidor pedidoDomicilioDeServidor)
        {
            PedidoADomicilio pedidoADomicilio = new PedidoADomicilio
            {
                Id = pedidoDomicilioDeServidor.id,
                fecha = pedidoDomicilioDeServidor.fecha,
                instruccionesEspeciales = pedidoDomicilioDeServidor.instruccionesDePedido,
                direccionDestino = pedidoDomicilioDeServidor.direccionDestino,
                Empleado = new Empleado
                {
                    IdEmpleado = pedidoDomicilioDeServidor.idEmpleado,
                    idEmpleadoGenerado = pedidoDomicilioDeServidor.idGeneradoDeEmpleado
                },
                Estado = new Estado
                {
                    estadoPedido = pedidoDomicilioDeServidor.estado
                },
                Cuenta = new Cuenta
                {
                    Id = pedidoDomicilioDeServidor.cuenta.Id,
                    precioTotal = pedidoDomicilioDeServidor.cuenta.toal,
                    subTotal = pedidoDomicilioDeServidor.cuenta.subtotal,
                    iva = pedidoDomicilioDeServidor.cuenta.iva,
                    descuento = pedidoDomicilioDeServidor.cuenta.descuento,
                    abierta = pedidoDomicilioDeServidor.cuenta.abierta
                },
                Cliente = ClienteServidor_A_Cliente(pedidoDomicilioDeServidor.cliente),
                ClienteId = pedidoDomicilioDeServidor.cliente.id,                                
            };

            List<Producto> productos = new List<Producto>();
            foreach (var productoLocal in pedidoDomicilioDeServidor.productosLocales)
            {
                var producto = ProductoDePedido_A_Producto(productoLocal);
                productos.Add(producto);
            }
            pedidoADomicilio.Producto = productos.ToArray();

            List<ProvisionDirecta> provisionesDirectas = new List<ProvisionDirecta>();
            foreach (var provisionVentaDirecta in pedidoDomicilioDeServidor.productosExternos)
            {
                var provisionDirecta = ProvisionVentaDirecta_A_ProvisionDirecta(provisionVentaDirecta);
                provisionesDirectas.Add(provisionDirecta);
            }
            pedidoADomicilio.ProvisionDirecta = provisionesDirectas.ToArray();

            return pedidoADomicilio;
        }


        static public Cliente ClienteServidor_A_Cliente (Cliente1 cliente1)
        {
            Cliente cliente = new Cliente
            {
                Id = cliente1.id,
                nombre = cliente1.nombre,
                apellidoPaterno = cliente1.apellidoPaterno,
                apellidoMaterno = cliente1.apellidoMaterno
            };

            List<Direccion> direcciones = new List<Direccion>();

            foreach(var direccionCliente in cliente1.direcciones)
            {
                var direccion = DireccionCliente_A_Direccion(direccionCliente);
                direcciones.Add(direccion);
            }

            cliente.Direccion = direcciones.ToArray();

            List<Telefono> telefonos = new List<Telefono>();

            foreach(var telefonoCliente in cliente1.telefonos)
            {
                var telefono = TelefonoCliente_A_Telefono(telefonoCliente);
                telefonos.Add(telefono);
            }

            cliente.Telefono = telefonos.ToArray();

            return cliente;
        }

        static public Direccion DireccionCliente_A_Direccion(DireccionCliente direccionCliente)
        {
            Direccion direccion = new Direccion
            {
                calle = direccionCliente.calle,
                colonia = direccionCliente.colonia,
                numeroExterior = direccionCliente.numeroExterior,
                numeroInterior = direccionCliente.numeroInterior,
                codigoPostal = direccionCliente.codigoPostal
            };
            return direccion;
        }

        static public Telefono TelefonoCliente_A_Telefono (TelefonoCliente telefonoCliente)
        {
            Telefono telefono = new Telefono
            {
                numeroTelefono = telefonoCliente.telefono
            };
            return telefono;
        }
    }
}