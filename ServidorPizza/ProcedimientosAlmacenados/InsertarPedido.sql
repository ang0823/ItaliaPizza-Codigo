
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Alberto Hernández Gómez
-- Create date: 19/05/2020
-- Description:	Procedimiento almacenado para registrar una cuenta de pedido
-- =============================================

ALTER PROCEDURE InsertarPedido

	@Fecha Datetime, 
	@InstruccionesEspeciales varchar(max),
	@IdEmpleado int,
	@IdEstado int,
	@IdCuenta varchar(max)

AS
BEGIN
	
	insert PedidoSet values (@Fecha, @InstruccionesEspeciales, @IdEmpleado, @IdEstado, @IdCuenta)

END
GO
