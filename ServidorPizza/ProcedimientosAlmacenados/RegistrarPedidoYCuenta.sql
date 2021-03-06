USE [PizzaItaliana]
GO
/****** Object:  StoredProcedure [dbo].[RegistrarPedidoYCuenta]    Script Date: 12/07/2020 09:42:55 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
CREATE PROCEDURE [dbo].[RegistrarPedidoYCuenta]

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
	@IdPedido int OUTPUT

AS
BEGIN

	DECLARE @IDEstado int

	SET @IDEstado = (SELECT Id FROM EstadoSet WHERE @NombreEstado = estadoPedido);
	
	INSERT PedidoSet VALUES (@Fecha, @InstruccionesEspeciales, @IdEmpleado, @IDEstado, @IdCuenta);

	SET @IdPedido = SCOPE_IDENTITY();

	INSERT CuentaSet VALUES (@IdCuenta, @PrecioTotal, @SubTotal, @Iva, @Descuento, @Abierta)

END