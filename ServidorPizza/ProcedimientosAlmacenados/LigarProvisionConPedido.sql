USE [PizzaItaliana]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[LigarProvisionConPedido]

	@IdProvision int, 
	@IdPedido int

AS
BEGIN

	insert ProvisionDirectaPedido values (@IdProvision, @IdPedido)

END
