USE [PizzaItaliana]
GO
/****** Object:  StoredProcedure [dbo].[RegistroDeClienteConDireccion]    Script Date: 12/07/2020 09:43:53 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[RegistroDeClienteConDireccion]
	
	@Nombre varchar(max),
	@ApellidoPaterno varchar(max),
	@ApellidoMaterno varchar(max),
	@Calle varchar(max),
	@Colonia varchar(max),
	@NumeroExterior varchar(10),
	@NumeroInterior varchar(10),
	@NumeroTelefonico varchar(16),
	@CodigoPostal varchar(5),
	@IDCliente int OUTPUT

AS
BEGIN
	
	DECLARE @IDDireccion int
	
	INSERT ClienteSet VALUES (@Nombre, @ApellidoPaterno, @ApellidoMaterno)
	SET @IDCliente = SCOPE_IDENTITY();

	INSERT DireccionSet VALUES (@Calle, @Colonia, @NumeroExterior, @NumeroInterior, @CodigoPostal)
	SET @IDDireccion = SCOPE_IDENTITY();

	INSERT DireccionCliente VALUES (@IDDireccion, @IDCliente)

	INSERT TelefonoSet VALUES (@NumeroTelefonico, @IDCliente)

END