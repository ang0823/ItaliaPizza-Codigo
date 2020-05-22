SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE RegistrarTelefono 
	
	@Telefono varchar(max),
	@IdCliente int

AS
BEGIN

	insert TelefonoSet values (@Telefono, @IdCliente)

END
GO
