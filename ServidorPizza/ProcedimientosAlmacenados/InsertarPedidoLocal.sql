SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE InsertarPedidoLocal
	
	@IdMesa int,
	@IdPedido int
AS
BEGIN
	
	insert PedidoSet_PedidoLocal values (@IdMesa, @IdPedido)

END
GO
