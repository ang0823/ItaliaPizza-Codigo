SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE LigarProductoConPedido
	
	@IdProducto int,
	@IdPedido int

AS
BEGIN
	
	insert ProductoPedido values (@IdProducto, @IdPedido)

END
GO
