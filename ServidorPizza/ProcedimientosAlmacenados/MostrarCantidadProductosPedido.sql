USE [PizzaItaliana]
GO
/****** Object:  StoredProcedure [dbo].[MostrarCantidadProductosPedido]    Script Date: 06/07/2020 01:26:18 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[MostrarCantidadProductosPedido]
@IdPedido int
AS
BEGIN
SELECT ProductoSet.* , cantidad, Pedido_Id, categoria FROM ProductoSet  
 right JOIN ProductoPedido ON ProductoSet.Id=ProductoPedido.Producto_Id 
 JOIN CategoriaSet on CategoriaSet.Id=ProductoSet.Categoria_Id
 WHERE ProductoPedido.Pedido_Id= @IdPedido
END