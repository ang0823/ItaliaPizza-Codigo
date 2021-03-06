USE [PizzaItaliana]
GO
/****** Object:  StoredProcedure [dbo].[ModificarPedidoADomicilio]    Script Date: 12/07/2020 09:40:00 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Caicero Franco Elsa Irasema>
-- Create date: <10 de julio 2020>
-- Description:	<Actualizacion de pedido a Domicilio >
-- =============================================
CREATE PROCEDURE [dbo].[ModificarPedidoADomicilio] 
	@IDPedido int, 
	@InstruccionesEspeciales varchar(max),
	@IdEmpleado bigint,
	@IdCuenta varchar(max),
	@PrecioTotal float, 
	@Subtotal float, 
	@Descuento float, 
	@abierta bit,
	@IdCliente int,
	@DireccionDestino varchar(50)

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	 UPDATE PedidoSet SET instruccionesEspeciales = @InstruccionesEspeciales, 
						 Empleado_IdEmpleado = @IdEmpleado
					WHERE Id = @IDPedido;

	UPDATE CuentaSet SET precioTotal = @PrecioTotal, subTotal = @Subtotal, 
						 descuento= @Descuento, abierta = @abierta
				     WHERE Id = @IdCuenta;

	UPDATE PedidoSet_PedidoADomicilio SET ClienteId = @IdCliente, direccionDestino= @DireccionDestino WHERE Id = @IDPedido;
	
END
