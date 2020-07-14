USE [PizzaItaliana]
GO
/****** Object:  StoredProcedure [dbo].[LigarProductoConPedido]    Script Date: 12/07/2020 09:38:26 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[LigarProductoConPedido]
	
	@IdProducto int,
	@IdPedido int,
	@Cantidad int

AS
BEGIN	
			INSERT ProductoPedido VALUES (@IdProducto, @IdPedido, @Cantidad)
		
END