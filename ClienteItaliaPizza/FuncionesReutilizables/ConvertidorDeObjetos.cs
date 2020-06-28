using ClienteItaliaPizza.Servicio;

namespace ClienteItaliaPizza.Validacion
{
    public static class ConvertidorDeObjetos
    {
        static public ProvisionDirecta ProvisionVentaDirecta_A_ProvisionDirecta(ProvisionVentaDirecta provisionVentaDirecta)
        {
            ProvisionDirecta provisionDirecta = new ProvisionDirecta();
            provisionDirecta.Provision = new Provision();
            provisionDirecta.Id = provisionVentaDirecta.idProvisionVentaDirecta; //6
            provisionDirecta.Provision.Id = provisionVentaDirecta.idProvision;    //2 
            provisionDirecta.Provision.nombre = provisionVentaDirecta.nombre;
            provisionDirecta.Provision.costoUnitario = provisionVentaDirecta.precioUnitario;
            provisionDirecta.descripcion = provisionVentaDirecta.descripcion;
            provisionDirecta.restricciones = provisionVentaDirecta.restricciones;
            provisionDirecta.Provision.noExistencias = provisionVentaDirecta.cantidadExistencias;
            provisionDirecta.Provision.ubicacion = provisionVentaDirecta.ubicacion;
            provisionDirecta.Provision.stockMinimo = provisionVentaDirecta.stock;
            provisionDirecta.Provision.unidadMedida = provisionVentaDirecta.unidadDeMedida;
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
                restricciones = productoDePedido.restricciones,                
                Categoria = new Categoria {
                    categoria = productoDePedido.categoria
                }
            };
            return producto;
        }
    }
}