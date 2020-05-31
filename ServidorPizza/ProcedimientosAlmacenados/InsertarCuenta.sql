
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Alberto Hernández Gómez
-- Create date: 19/05/2020
-- Description:	Procedimiento almacenado para registrar una cuenta de pedido
-- =============================================

CREATE PROCEDURE InsertarCuentaDePedido

	@Id varchar (max),
	@PrecioTotal float, 
	@SubTotal float,
	@Iva float,
	@Descuento float

AS
BEGIN
	
	insert CuentaSet values (@Id, @PrecioTotal, @SubTotal, @Iva, @Descuento)

END
GO
