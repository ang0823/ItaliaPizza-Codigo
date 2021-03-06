USE [PizzaItaliana]
GO
/****** Object:  StoredProcedure [dbo].[InsertarPedidoADomicilio]    Script Date: 12/07/2020 09:34:30 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[InsertarPedidoADomicilio]

	@IdCliente int,
	@Fecha Datetime, 
	@InstruccionesEspeciales varchar(max),
	@IdEmpleado bigint,
	@NombreEstado varchar(max),
	@IdCuenta varchar(max),
	@PrecioTotal float, 
	@SubTotal float,
	@Iva float,
	@Descuento float,
	@Abierta bit,
	@DireccionDestino varchar(50),
	@IDPedido int OUTPUT

AS
BEGIN

	EXEC RegistrarPedidoYCuenta @Fecha, @InstruccionesEspeciales, @IdEmpleado, @NombreEstado, @IdCuenta, @PrecioTotal, @SubTotal, @Iva, @Descuento, @Abierta, @IDPedido OUTPUT

	INSERT PedidoSet_PedidoADomicilio VALUES (@idCliente,@DireccionDestino, @IDPedido)
	
END