SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE RegistroDeClienteConDireccion
	
	@Nombre varchar(max),
	@ApellidoPaterno varchar(max),
	@ApellidoMaterno varchar(max),
	@Calle varchar(max),
	@Colonia varchar(max),
	@NumeroExterior varchar(10),
	@NumeroInterior varchar(10)

AS
BEGIN
	
	insert ClienteSet values (@Nombre, @ApellidoPaterno, @ApellidoMaterno)

	insert DireccionSet values (@Calle, @Colonia, @NumeroExterior, @NumeroInterior)

END
GO
