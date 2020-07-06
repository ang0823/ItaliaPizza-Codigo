USE [PizzaItaliana]
GO
/****** Object:  StoredProcedure [dbo].[LigarProvisionConPedido]    Script Date: 06/07/2020 01:26:00 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[LigarProvisionConPedido]

	@IdProvision int, 
	@IdPedido int,
	@Cantidad int

AS
BEGIN

	insert ProvisionDirectaPedido values (@IdProvision, @IdPedido, @Cantidad)

END