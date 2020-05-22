SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================

CREATE PROCEDURE LigarClienteADireccion
	
	@IdDireccion int,
	@IdCliente int

AS
BEGIN
	
	insert DireccionCliente values (@IdDireccion, @IdCliente)

END
GO
