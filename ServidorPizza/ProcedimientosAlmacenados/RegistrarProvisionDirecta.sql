USE [PizzaItaliana]
GO
/****** Object:  StoredProcedure [dbo].[RegistrarProvisionDirecta]    Script Date: 12/07/2020 09:43:20 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Irasema Caicero>
-- Create date: <7 de julio 2020>
-- Description:	<Registro de un producto externo/Provision Directa>
-- =============================================
CREATE PROCEDURE [dbo].[RegistrarProvisionDirecta]

	@NombreProvisionDirecta varchar(max),
	@noExistencias int,
	@Ubicacion varchar(max), 
	@StockMinimo int, 
	@CostoUnitario float,
	@UnidadMedida varchar(max),
	@Activado bit,
	@DescripcionProvisionDirecta varchar(max), 
	@ActivadoProvisionDirecta bit,
	@RestriccionesProvisionDirecta varchar(max),
	@Categoria varchar(max)

AS
BEGIN
	DECLARE @IDProvision INT
	DECLARE @IDCategoria INT

	INSERT INTO ProvisionSet VALUES (@NombreProvisionDirecta, @noExistencias, @Ubicacion, @StockMinimo, @CostoUnitario, @UnidadMedida, @Activado);
	SET @IDProvision = SCOPE_IDENTITY();

	IF  NOT EXISTS (SELECT Id FROM CategoriaSet WHERE @Categoria = categoria)
		BEGIN
		INSERT INTO CategoriaSet VALUES (@Categoria);
		END

	SET @IDCategoria = (SELECT Id FROM CategoriaSet WHERE @Categoria = categoria);

	INSERT INTO ProvisionDirectaSet VALUES (@DescripcionProvisionDirecta, @ActivadoProvisionDirecta, @RestriccionesProvisionDirecta, @IDProvision, @IDCategoria);

END
