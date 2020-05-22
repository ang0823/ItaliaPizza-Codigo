
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE InsertarPedioDomicilio
	
	@IdCliente int,
	@IdPedido int

AS
BEGIN
	
	insert PedidoSet_PedidoADomicilio values(@IdCliente, @IdPedido)

END
GO
