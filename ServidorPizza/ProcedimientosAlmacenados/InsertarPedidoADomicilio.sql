USE [PizzaItaliana]
GO
/****** Object:  StoredProcedure [dbo].[InsertarPedidoADomicilio]    Script Date: 16/06/2020 07:58:08 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[InsertarPedidoADomicilio]

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
	@IDPedido int OUTPUT

AS
BEGIN

	EXEC RegistrarPedidoYCuenta @Fecha, @InstruccionesEspeciales, @IdEmpleado, @NombreEstado, @IdCuenta, @PrecioTotal, @SubTotal, @Iva, @Descuento, @IDPedido OUTPUT

	INSERT PedidoSet_PedidoADomicilio VALUES (@idCliente, @IDPedido)
	
END
