
//------------------------------------------------------------------------------
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
    using System.Collections.Generic;
    
public partial class Telefono
{

    public int Id { get; set; }

    public string numeroTelefono { get; set; }



    public virtual Cliente Cliente { get; set; }

}

}
