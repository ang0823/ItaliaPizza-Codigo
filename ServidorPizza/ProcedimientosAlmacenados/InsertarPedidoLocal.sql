USE [PizzaItaliana]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[InsertarPedidoLocal]
	
	@NumeroMesa smallint,
	@Fecha Datetime, 
	@InstruccionesEspeciales varchar(max),
	@IdEmpleado bigint,
	@NombreEstado varchar(max),
	@IdCuenta varchar(max),
	@PrecioTotal float, 
	@SubTotal float,
	@Iva float,
	@Descuento float,
	@IDPedido int OUTPUT

AS
BEGIN

	DECLARE @IDMesa int

	SET @IDMesa = (SELECT Id FROM MesaSet WHERE numeroMesa = @NumeroMesa);


	EXEC RegistrarPedidoYCuenta @Fecha, @InstruccionesEspeciales, @IdEmpleado, @NombreEstado, @IdCuenta, @PrecioTotal, @SubTotal, @Iva, @Descuento, @IDPedido OUTPUT

	INSERT PedidoSet_PedidoLocal VALUES (@IDMesa, @IDPedido);

END
