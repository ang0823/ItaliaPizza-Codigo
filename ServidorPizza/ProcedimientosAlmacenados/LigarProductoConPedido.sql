USE [PizzaItaliana]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[LigarProductoConPedido]
	
	@IdProducto int,
	@IdPedido int

AS
BEGIN
	
	insert ProductoPedido values (@IdProducto, @IdPedido)

END
