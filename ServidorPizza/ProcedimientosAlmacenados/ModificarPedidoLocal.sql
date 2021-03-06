USE [PizzaItaliana]
GO
/****** Object:  StoredProcedure [dbo].[ModificarPedidoLocal]    Script Date: 12/07/2020 09:40:54 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Caicero Franco Elsa Irasema>
-- Create date: <9 de julio 2020>
-- Description:	<Modificar Pedido>
-- =============================================
CREATE PROCEDURE [dbo].[ModificarPedidoLocal]
	
	@IDPedido int, 
	@InstruccionesEspeciales varchar(max),
	@IdEmpleado bigint,
	@IdCuenta varchar(max),
	@PrecioTotal float, 
	@Subtotal float, 
	@Descuento float, 
	@abierta bit,
	@NumeroMesa smallint
	
AS
BEGIN
	DECLARE @IdMesa INT
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;


    UPDATE PedidoSet SET instruccionesEspeciales = @InstruccionesEspeciales, 
						 Empleado_IdEmpleado = @IdEmpleado
					WHERE Id = @IDPedido;

	UPDATE CuentaSet SET precioTotal = @PrecioTotal, subTotal = @Subtotal, 
						 descuento= @Descuento, abierta = @abierta
				     WHERE Id = @IdCuenta;


	IF NOT EXISTS (SELECT Id FROM MesaSet WHERE numeroMesa = @NumeroMesa)
		BEGIN
			INSERT INTO MesaSet VALUES (@NumeroMesa);
		END

		SET @IdMesa = (SELECT Id FROM MesaSet WHERE numeroMesa = @NumeroMesa);
		UPDATE PedidoSet_PedidoLocal SET MesaId= @IdMesa WHERE Id = @IDPedido;
END
