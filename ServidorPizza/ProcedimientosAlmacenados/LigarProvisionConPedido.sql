SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE LigarProvisionConPedido

	@IdProvision int, 
	@IdPedido int

AS
BEGIN

	insert ProvisionDirectaPedido values (@IdProvision, @IdPedido)

END
GO
