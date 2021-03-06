USE [PizzaItaliana]
GO
/****** Object:  StoredProcedure [dbo].[MostrarCantidadProvisionesPedido]    Script Date: 12/07/2020 09:42:06 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Caicero Franco Elsa Irasema>
-- Create date: <3 julio 20>
-- Description:	<Devuelve los productos junto con su cantidad (está en una tabla intermedia)>
-- =============================================
CREATE PROCEDURE [dbo].[MostrarCantidadProvisionesPedido]
	@IdPedido int
	AS
BEGIN
	SELECT ProvisionDirectaSet.* , ProvisionSet.nombre, ProvisionSet.noExistencias, ProvisionSet.ubicacion, ProvisionSet.stockMinimo,
	ProvisionSet.costoUnitario, ProvisionSet.unidadMedida, cantidad, Pedido_Id, categoria FROM ProvisionDirectaSet  
	right JOIN ProvisionDirectaPedido ON ProvisionDirectaSet.Id=ProvisionDirectaPedido.ProvisionDirecta_Id
	JOIN CategoriaSet on CategoriaSet.Id=ProvisionDirectaSet.Categoria_Id
	JOIN ProvisionSet ON ProvisionSet.Id= ProvisionDirectaSet.Provision_Id
	WHERE ProvisionDirectaPedido.Pedido_Id= @IdPedido
END