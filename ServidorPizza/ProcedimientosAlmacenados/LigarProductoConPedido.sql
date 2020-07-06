USE [PizzaItaliana]
GO
/****** Object:  StoredProcedure [dbo].[LigarProductoConPedido]    Script Date: 06/07/2020 01:25:31 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[LigarProductoConPedido]
	
	@IdProducto int,
	@IdPedido int,
	@Cantidad int

AS
BEGIN
	
	insert ProductoPedido values (@IdProducto, @IdPedido, @Cantidad)

END