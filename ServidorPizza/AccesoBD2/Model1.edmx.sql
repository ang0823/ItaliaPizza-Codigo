
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 05/31/2020 02:13:59
-- Generated from EDMX file: C:\Users\javie\Desktop\Nueva carpeta (2)\ItaliaPizza-Codigo-master\ServidorPizza\AccesoBD2\Model1.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_CuentaUsuarioEmpleado]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CuentaUsuarioSet] DROP CONSTRAINT [FK_CuentaUsuarioEmpleado];
GO
IF OBJECT_ID(N'[dbo].[FK_EmpleadoDireccion]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[EmpleadoSet] DROP CONSTRAINT [FK_EmpleadoDireccion];
GO
IF OBJECT_ID(N'[dbo].[FK_DireccionCliente_Direccion]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DireccionCliente] DROP CONSTRAINT [FK_DireccionCliente_Direccion];
GO
IF OBJECT_ID(N'[dbo].[FK_DireccionCliente_Cliente]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DireccionCliente] DROP CONSTRAINT [FK_DireccionCliente_Cliente];
GO
IF OBJECT_ID(N'[dbo].[FK_EmpleadoRol]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[EmpleadoSet] DROP CONSTRAINT [FK_EmpleadoRol];
GO
IF OBJECT_ID(N'[dbo].[FK_EmpleadoPedido]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PedidoSet] DROP CONSTRAINT [FK_EmpleadoPedido];
GO
IF OBJECT_ID(N'[dbo].[FK_EstadoPedido]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PedidoSet] DROP CONSTRAINT [FK_EstadoPedido];
GO
IF OBJECT_ID(N'[dbo].[FK_CuentaPedido]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PedidoSet] DROP CONSTRAINT [FK_CuentaPedido];
GO
IF OBJECT_ID(N'[dbo].[FK_ProductoPedido_Producto]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ProductoPedido] DROP CONSTRAINT [FK_ProductoPedido_Producto];
GO
IF OBJECT_ID(N'[dbo].[FK_ProductoPedido_Pedido]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ProductoPedido] DROP CONSTRAINT [FK_ProductoPedido_Pedido];
GO
IF OBJECT_ID(N'[dbo].[FK_ProvisionDirectaPedido_ProvisionDirecta]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ProvisionDirectaPedido] DROP CONSTRAINT [FK_ProvisionDirectaPedido_ProvisionDirecta];
GO
IF OBJECT_ID(N'[dbo].[FK_ProvisionDirectaPedido_Pedido]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ProvisionDirectaPedido] DROP CONSTRAINT [FK_ProvisionDirectaPedido_Pedido];
GO
IF OBJECT_ID(N'[dbo].[FK_ProductoCategoria]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ProductoSet] DROP CONSTRAINT [FK_ProductoCategoria];
GO
IF OBJECT_ID(N'[dbo].[FK_ClienteTelefono]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TelefonoSet] DROP CONSTRAINT [FK_ClienteTelefono];
GO
IF OBJECT_ID(N'[dbo].[FK_ProvisionDirectaProvision]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ProvisionDirectaSet] DROP CONSTRAINT [FK_ProvisionDirectaProvision];
GO
IF OBJECT_ID(N'[dbo].[FK_ProductoReceta]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[RecetaSet] DROP CONSTRAINT [FK_ProductoReceta];
GO
IF OBJECT_ID(N'[dbo].[FK_RecetaProvision_Receta]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[RecetaProvision] DROP CONSTRAINT [FK_RecetaProvision_Receta];
GO
IF OBJECT_ID(N'[dbo].[FK_RecetaProvision_Provision]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[RecetaProvision] DROP CONSTRAINT [FK_RecetaProvision_Provision];
GO
IF OBJECT_ID(N'[dbo].[FK_ClientePedidoADomicilio]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PedidoSet_PedidoADomicilio] DROP CONSTRAINT [FK_ClientePedidoADomicilio];
GO
IF OBJECT_ID(N'[dbo].[FK_MesaPedidoLocal]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PedidoSet_PedidoLocal] DROP CONSTRAINT [FK_MesaPedidoLocal];
GO
IF OBJECT_ID(N'[dbo].[FK_IngredienteReceta_Ingrediente]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[IngredienteReceta] DROP CONSTRAINT [FK_IngredienteReceta_Ingrediente];
GO
IF OBJECT_ID(N'[dbo].[FK_IngredienteReceta_Receta]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[IngredienteReceta] DROP CONSTRAINT [FK_IngredienteReceta_Receta];
GO
IF OBJECT_ID(N'[dbo].[FK_PedidoADomicilio_inherits_Pedido]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PedidoSet_PedidoADomicilio] DROP CONSTRAINT [FK_PedidoADomicilio_inherits_Pedido];
GO
IF OBJECT_ID(N'[dbo].[FK_PedidoLocal_inherits_Pedido]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PedidoSet_PedidoLocal] DROP CONSTRAINT [FK_PedidoLocal_inherits_Pedido];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[CuentaUsuarioSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CuentaUsuarioSet];
GO
IF OBJECT_ID(N'[dbo].[EmpleadoSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[EmpleadoSet];
GO
IF OBJECT_ID(N'[dbo].[DireccionSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DireccionSet];
GO
IF OBJECT_ID(N'[dbo].[RolSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[RolSet];
GO
IF OBJECT_ID(N'[dbo].[PedidoSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PedidoSet];
GO
IF OBJECT_ID(N'[dbo].[ClienteSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ClienteSet];
GO
IF OBJECT_ID(N'[dbo].[MesaSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[MesaSet];
GO
IF OBJECT_ID(N'[dbo].[EstadoSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[EstadoSet];
GO
IF OBJECT_ID(N'[dbo].[CategoriaSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CategoriaSet];
GO
IF OBJECT_ID(N'[dbo].[ProductoSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ProductoSet];
GO
IF OBJECT_ID(N'[dbo].[CuentaSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CuentaSet];
GO
IF OBJECT_ID(N'[dbo].[TelefonoSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TelefonoSet];
GO
IF OBJECT_ID(N'[dbo].[ProvisionDirectaSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ProvisionDirectaSet];
GO
IF OBJECT_ID(N'[dbo].[RecetaSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[RecetaSet];
GO
IF OBJECT_ID(N'[dbo].[ProvisionSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ProvisionSet];
GO
IF OBJECT_ID(N'[dbo].[IngredienteSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[IngredienteSet];
GO
IF OBJECT_ID(N'[dbo].[PedidoSet_PedidoADomicilio]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PedidoSet_PedidoADomicilio];
GO
IF OBJECT_ID(N'[dbo].[PedidoSet_PedidoLocal]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PedidoSet_PedidoLocal];
GO
IF OBJECT_ID(N'[dbo].[DireccionCliente]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DireccionCliente];
GO
IF OBJECT_ID(N'[dbo].[ProductoPedido]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ProductoPedido];
GO
IF OBJECT_ID(N'[dbo].[ProvisionDirectaPedido]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ProvisionDirectaPedido];
GO
IF OBJECT_ID(N'[dbo].[RecetaProvision]', 'U') IS NOT NULL
    DROP TABLE [dbo].[RecetaProvision];
GO
IF OBJECT_ID(N'[dbo].[IngredienteReceta]', 'U') IS NOT NULL
    DROP TABLE [dbo].[IngredienteReceta];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'CuentaUsuarioSet'
CREATE TABLE [dbo].[CuentaUsuarioSet] (
    [nombreUsuario] nvarchar(max)  NOT NULL,
    [contraseña] nvarchar(max)  NOT NULL,
    [Id] int IDENTITY(1,1) NOT NULL,
    [Empleado_IdEmpleado] bigint  NOT NULL
);
GO

-- Creating table 'EmpleadoSet'
CREATE TABLE [dbo].[EmpleadoSet] (
    [IdEmpleado] bigint IDENTITY(1,1) NOT NULL,
    [nombre] nvarchar(max)  NOT NULL,
    [apellidoPaterno] nvarchar(max)  NOT NULL,
    [apellidoMaterno] nvarchar(max)  NOT NULL,
    [telefono] nvarchar(max)  NOT NULL,
    [correo] nvarchar(max)  NOT NULL,
    [idEmpleadoGenerado] nvarchar(max)  NOT NULL,
    [activado] bit  NOT NULL,
    [Direccion_Id] int  NOT NULL,
    [Rol_Id] int  NOT NULL
);
GO

-- Creating table 'DireccionSet'
CREATE TABLE [dbo].[DireccionSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [calle] nvarchar(max)  NOT NULL,
    [colonia] nvarchar(max)  NOT NULL,
    [numeroExterior] nvarchar(max)  NOT NULL,
    [numeroInterior] nvarchar(max)  NOT NULL,
    [codigoPostal] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'RolSet'
CREATE TABLE [dbo].[RolSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [nombreRol] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'PedidoSet'
CREATE TABLE [dbo].[PedidoSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [fecha] datetime  NOT NULL,
    [instruccionesEspeciales] nvarchar(max)  NULL,
    [Empleado_IdEmpleado] bigint  NOT NULL,
    [Estado_Id] int  NOT NULL,
    [Cuenta_Id] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'ClienteSet'
CREATE TABLE [dbo].[ClienteSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [nombre] nvarchar(max)  NOT NULL,
    [apellidoPaterno] nvarchar(max)  NOT NULL,
    [apellidoMaterno] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'MesaSet'
CREATE TABLE [dbo].[MesaSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [numeroMesa] smallint  NOT NULL
);
GO

-- Creating table 'EstadoSet'
CREATE TABLE [dbo].[EstadoSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [estadoPedido] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'CategoriaSet'
CREATE TABLE [dbo].[CategoriaSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [categoria] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'ProductoSet'
CREATE TABLE [dbo].[ProductoSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [nombre] nvarchar(max)  NOT NULL,
    [descripcion] nvarchar(max)  NOT NULL,
    [precioUnitario] float  NOT NULL,
    [imagen] tinyint  NULL,
    [activado] bit  NOT NULL,
    [restricciones] nvarchar(max)  NOT NULL,
    [Categoria_Id] int  NOT NULL,
    [Receta_id] int  NOT NULL
);
GO

-- Creating table 'CuentaSet'
CREATE TABLE [dbo].[CuentaSet] (
    [Id] nvarchar(max)  NOT NULL,
    [precioTotal] float  NOT NULL,
    [subTotal] float  NOT NULL,
    [iva] float  NOT NULL,
    [descuento] float  NULL
);
GO

-- Creating table 'TelefonoSet'
CREATE TABLE [dbo].[TelefonoSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [numeroTelefono] nvarchar(max)  NOT NULL,
    [Cliente_Id] int  NOT NULL
);
GO

-- Creating table 'ProvisionDirectaSet'
CREATE TABLE [dbo].[ProvisionDirectaSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [descripcion] nvarchar(max)  NOT NULL,
    [imagen] tinyint  NULL,
    [activado] bit  NOT NULL,
    [restricciones] nvarchar(max)  NOT NULL,
    [Provision_Id] int  NOT NULL
);
GO

-- Creating table 'RecetaSet'
CREATE TABLE [dbo].[RecetaSet] (
    [id] int IDENTITY(1,1) NOT NULL,
    [porciones] float  NOT NULL,
    [procedimiento] nvarchar(max)  NOT NULL,
    [nombreReceta] nvarchar(max)  NOT NULL,
    [activado] bit  NOT NULL
);
GO

-- Creating table 'ProvisionSet'
CREATE TABLE [dbo].[ProvisionSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [nombre] nvarchar(max)  NOT NULL,
    [noExistencias] int  NOT NULL,
    [ubicacion] nvarchar(max)  NOT NULL,
    [stockMinimo] int  NOT NULL,
    [costoUnitario] float  NOT NULL,
    [unidadMedida] nvarchar(max)  NOT NULL,
    [activado] bit  NOT NULL
);
GO

-- Creating table 'IngredienteSet'
CREATE TABLE [dbo].[IngredienteSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [nombre] nvarchar(max)  NOT NULL,
    [cantidad] int  NOT NULL,
    [peso] nvarchar(max)  NOT NULL,
    [costoPorUnidad] float  NOT NULL,
    [unidad] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'PedidoSet_PedidoADomicilio'
CREATE TABLE [dbo].[PedidoSet_PedidoADomicilio] (
    [ClienteId] int  NOT NULL,
    [Id] int  NOT NULL
);
GO

-- Creating table 'PedidoSet_PedidoLocal'
CREATE TABLE [dbo].[PedidoSet_PedidoLocal] (
    [MesaId] int  NOT NULL,
    [Id] int  NOT NULL
);
GO

-- Creating table 'DireccionCliente'
CREATE TABLE [dbo].[DireccionCliente] (
    [Direccion_Id] int  NOT NULL,
    [Cliente_Id] int  NOT NULL
);
GO

-- Creating table 'ProductoPedido'
CREATE TABLE [dbo].[ProductoPedido] (
    [Producto_Id] int  NOT NULL,
    [Pedido_Id] int  NOT NULL
);
GO

-- Creating table 'ProvisionDirectaPedido'
CREATE TABLE [dbo].[ProvisionDirectaPedido] (
    [ProvisionDirecta_Id] int  NOT NULL,
    [Pedido_Id] int  NOT NULL
);
GO

-- Creating table 'RecetaProvision'
CREATE TABLE [dbo].[RecetaProvision] (
    [Receta_id] int  NOT NULL,
    [Provision_Id] int  NOT NULL
);
GO

-- Creating table 'IngredienteReceta'
CREATE TABLE [dbo].[IngredienteReceta] (
    [Ingrediente_Id] int  NOT NULL,
    [Receta_id] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'CuentaUsuarioSet'
ALTER TABLE [dbo].[CuentaUsuarioSet]
ADD CONSTRAINT [PK_CuentaUsuarioSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [IdEmpleado] in table 'EmpleadoSet'
ALTER TABLE [dbo].[EmpleadoSet]
ADD CONSTRAINT [PK_EmpleadoSet]
    PRIMARY KEY CLUSTERED ([IdEmpleado] ASC);
GO

-- Creating primary key on [Id] in table 'DireccionSet'
ALTER TABLE [dbo].[DireccionSet]
ADD CONSTRAINT [PK_DireccionSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'RolSet'
ALTER TABLE [dbo].[RolSet]
ADD CONSTRAINT [PK_RolSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'PedidoSet'
ALTER TABLE [dbo].[PedidoSet]
ADD CONSTRAINT [PK_PedidoSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ClienteSet'
ALTER TABLE [dbo].[ClienteSet]
ADD CONSTRAINT [PK_ClienteSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'MesaSet'
ALTER TABLE [dbo].[MesaSet]
ADD CONSTRAINT [PK_MesaSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'EstadoSet'
ALTER TABLE [dbo].[EstadoSet]
ADD CONSTRAINT [PK_EstadoSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'CategoriaSet'
ALTER TABLE [dbo].[CategoriaSet]
ADD CONSTRAINT [PK_CategoriaSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ProductoSet'
ALTER TABLE [dbo].[ProductoSet]
ADD CONSTRAINT [PK_ProductoSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'CuentaSet'
ALTER TABLE [dbo].[CuentaSet]
ADD CONSTRAINT [PK_CuentaSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'TelefonoSet'
ALTER TABLE [dbo].[TelefonoSet]
ADD CONSTRAINT [PK_TelefonoSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ProvisionDirectaSet'
ALTER TABLE [dbo].[ProvisionDirectaSet]
ADD CONSTRAINT [PK_ProvisionDirectaSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [id] in table 'RecetaSet'
ALTER TABLE [dbo].[RecetaSet]
ADD CONSTRAINT [PK_RecetaSet]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [Id] in table 'ProvisionSet'
ALTER TABLE [dbo].[ProvisionSet]
ADD CONSTRAINT [PK_ProvisionSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'IngredienteSet'
ALTER TABLE [dbo].[IngredienteSet]
ADD CONSTRAINT [PK_IngredienteSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'PedidoSet_PedidoADomicilio'
ALTER TABLE [dbo].[PedidoSet_PedidoADomicilio]
ADD CONSTRAINT [PK_PedidoSet_PedidoADomicilio]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'PedidoSet_PedidoLocal'
ALTER TABLE [dbo].[PedidoSet_PedidoLocal]
ADD CONSTRAINT [PK_PedidoSet_PedidoLocal]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Direccion_Id], [Cliente_Id] in table 'DireccionCliente'
ALTER TABLE [dbo].[DireccionCliente]
ADD CONSTRAINT [PK_DireccionCliente]
    PRIMARY KEY CLUSTERED ([Direccion_Id], [Cliente_Id] ASC);
GO

-- Creating primary key on [Producto_Id], [Pedido_Id] in table 'ProductoPedido'
ALTER TABLE [dbo].[ProductoPedido]
ADD CONSTRAINT [PK_ProductoPedido]
    PRIMARY KEY CLUSTERED ([Producto_Id], [Pedido_Id] ASC);
GO

-- Creating primary key on [ProvisionDirecta_Id], [Pedido_Id] in table 'ProvisionDirectaPedido'
ALTER TABLE [dbo].[ProvisionDirectaPedido]
ADD CONSTRAINT [PK_ProvisionDirectaPedido]
    PRIMARY KEY CLUSTERED ([ProvisionDirecta_Id], [Pedido_Id] ASC);
GO

-- Creating primary key on [Receta_id], [Provision_Id] in table 'RecetaProvision'
ALTER TABLE [dbo].[RecetaProvision]
ADD CONSTRAINT [PK_RecetaProvision]
    PRIMARY KEY CLUSTERED ([Receta_id], [Provision_Id] ASC);
GO

-- Creating primary key on [Ingrediente_Id], [Receta_id] in table 'IngredienteReceta'
ALTER TABLE [dbo].[IngredienteReceta]
ADD CONSTRAINT [PK_IngredienteReceta]
    PRIMARY KEY CLUSTERED ([Ingrediente_Id], [Receta_id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [Empleado_IdEmpleado] in table 'CuentaUsuarioSet'
ALTER TABLE [dbo].[CuentaUsuarioSet]
ADD CONSTRAINT [FK_CuentaUsuarioEmpleado]
    FOREIGN KEY ([Empleado_IdEmpleado])
    REFERENCES [dbo].[EmpleadoSet]
        ([IdEmpleado])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CuentaUsuarioEmpleado'
CREATE INDEX [IX_FK_CuentaUsuarioEmpleado]
ON [dbo].[CuentaUsuarioSet]
    ([Empleado_IdEmpleado]);
GO

-- Creating foreign key on [Direccion_Id] in table 'EmpleadoSet'
ALTER TABLE [dbo].[EmpleadoSet]
ADD CONSTRAINT [FK_EmpleadoDireccion]
    FOREIGN KEY ([Direccion_Id])
    REFERENCES [dbo].[DireccionSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_EmpleadoDireccion'
CREATE INDEX [IX_FK_EmpleadoDireccion]
ON [dbo].[EmpleadoSet]
    ([Direccion_Id]);
GO

-- Creating foreign key on [Direccion_Id] in table 'DireccionCliente'
ALTER TABLE [dbo].[DireccionCliente]
ADD CONSTRAINT [FK_DireccionCliente_Direccion]
    FOREIGN KEY ([Direccion_Id])
    REFERENCES [dbo].[DireccionSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Cliente_Id] in table 'DireccionCliente'
ALTER TABLE [dbo].[DireccionCliente]
ADD CONSTRAINT [FK_DireccionCliente_Cliente]
    FOREIGN KEY ([Cliente_Id])
    REFERENCES [dbo].[ClienteSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_DireccionCliente_Cliente'
CREATE INDEX [IX_FK_DireccionCliente_Cliente]
ON [dbo].[DireccionCliente]
    ([Cliente_Id]);
GO

-- Creating foreign key on [Rol_Id] in table 'EmpleadoSet'
ALTER TABLE [dbo].[EmpleadoSet]
ADD CONSTRAINT [FK_EmpleadoRol]
    FOREIGN KEY ([Rol_Id])
    REFERENCES [dbo].[RolSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_EmpleadoRol'
CREATE INDEX [IX_FK_EmpleadoRol]
ON [dbo].[EmpleadoSet]
    ([Rol_Id]);
GO

-- Creating foreign key on [Empleado_IdEmpleado] in table 'PedidoSet'
ALTER TABLE [dbo].[PedidoSet]
ADD CONSTRAINT [FK_EmpleadoPedido]
    FOREIGN KEY ([Empleado_IdEmpleado])
    REFERENCES [dbo].[EmpleadoSet]
        ([IdEmpleado])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_EmpleadoPedido'
CREATE INDEX [IX_FK_EmpleadoPedido]
ON [dbo].[PedidoSet]
    ([Empleado_IdEmpleado]);
GO

-- Creating foreign key on [Estado_Id] in table 'PedidoSet'
ALTER TABLE [dbo].[PedidoSet]
ADD CONSTRAINT [FK_EstadoPedido]
    FOREIGN KEY ([Estado_Id])
    REFERENCES [dbo].[EstadoSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_EstadoPedido'
CREATE INDEX [IX_FK_EstadoPedido]
ON [dbo].[PedidoSet]
    ([Estado_Id]);
GO

-- Creating foreign key on [Cuenta_Id] in table 'PedidoSet'
ALTER TABLE [dbo].[PedidoSet]
ADD CONSTRAINT [FK_CuentaPedido]
    FOREIGN KEY ([Cuenta_Id])
    REFERENCES [dbo].[CuentaSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CuentaPedido'
CREATE INDEX [IX_FK_CuentaPedido]
ON [dbo].[PedidoSet]
    ([Cuenta_Id]);
GO

-- Creating foreign key on [Producto_Id] in table 'ProductoPedido'
ALTER TABLE [dbo].[ProductoPedido]
ADD CONSTRAINT [FK_ProductoPedido_Producto]
    FOREIGN KEY ([Producto_Id])
    REFERENCES [dbo].[ProductoSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Pedido_Id] in table 'ProductoPedido'
ALTER TABLE [dbo].[ProductoPedido]
ADD CONSTRAINT [FK_ProductoPedido_Pedido]
    FOREIGN KEY ([Pedido_Id])
    REFERENCES [dbo].[PedidoSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ProductoPedido_Pedido'
CREATE INDEX [IX_FK_ProductoPedido_Pedido]
ON [dbo].[ProductoPedido]
    ([Pedido_Id]);
GO

-- Creating foreign key on [ProvisionDirecta_Id] in table 'ProvisionDirectaPedido'
ALTER TABLE [dbo].[ProvisionDirectaPedido]
ADD CONSTRAINT [FK_ProvisionDirectaPedido_ProvisionDirecta]
    FOREIGN KEY ([ProvisionDirecta_Id])
    REFERENCES [dbo].[ProvisionDirectaSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Pedido_Id] in table 'ProvisionDirectaPedido'
ALTER TABLE [dbo].[ProvisionDirectaPedido]
ADD CONSTRAINT [FK_ProvisionDirectaPedido_Pedido]
    FOREIGN KEY ([Pedido_Id])
    REFERENCES [dbo].[PedidoSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ProvisionDirectaPedido_Pedido'
CREATE INDEX [IX_FK_ProvisionDirectaPedido_Pedido]
ON [dbo].[ProvisionDirectaPedido]
    ([Pedido_Id]);
GO

-- Creating foreign key on [Categoria_Id] in table 'ProductoSet'
ALTER TABLE [dbo].[ProductoSet]
ADD CONSTRAINT [FK_ProductoCategoria]
    FOREIGN KEY ([Categoria_Id])
    REFERENCES [dbo].[CategoriaSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ProductoCategoria'
CREATE INDEX [IX_FK_ProductoCategoria]
ON [dbo].[ProductoSet]
    ([Categoria_Id]);
GO

-- Creating foreign key on [Cliente_Id] in table 'TelefonoSet'
ALTER TABLE [dbo].[TelefonoSet]
ADD CONSTRAINT [FK_ClienteTelefono]
    FOREIGN KEY ([Cliente_Id])
    REFERENCES [dbo].[ClienteSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ClienteTelefono'
CREATE INDEX [IX_FK_ClienteTelefono]
ON [dbo].[TelefonoSet]
    ([Cliente_Id]);
GO

-- Creating foreign key on [Provision_Id] in table 'ProvisionDirectaSet'
ALTER TABLE [dbo].[ProvisionDirectaSet]
ADD CONSTRAINT [FK_ProvisionDirectaProvision]
    FOREIGN KEY ([Provision_Id])
    REFERENCES [dbo].[ProvisionSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ProvisionDirectaProvision'
CREATE INDEX [IX_FK_ProvisionDirectaProvision]
ON [dbo].[ProvisionDirectaSet]
    ([Provision_Id]);
GO

-- Creating foreign key on [Receta_id] in table 'ProductoSet'
ALTER TABLE [dbo].[ProductoSet]
ADD CONSTRAINT [FK_ProductoReceta]
    FOREIGN KEY ([Receta_id])
    REFERENCES [dbo].[RecetaSet]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ProductoReceta'
CREATE INDEX [IX_FK_ProductoReceta]
ON [dbo].[ProductoSet]
    ([Receta_id]);
GO

-- Creating foreign key on [Receta_id] in table 'RecetaProvision'
ALTER TABLE [dbo].[RecetaProvision]
ADD CONSTRAINT [FK_RecetaProvision_Receta]
    FOREIGN KEY ([Receta_id])
    REFERENCES [dbo].[RecetaSet]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Provision_Id] in table 'RecetaProvision'
ALTER TABLE [dbo].[RecetaProvision]
ADD CONSTRAINT [FK_RecetaProvision_Provision]
    FOREIGN KEY ([Provision_Id])
    REFERENCES [dbo].[ProvisionSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_RecetaProvision_Provision'
CREATE INDEX [IX_FK_RecetaProvision_Provision]
ON [dbo].[RecetaProvision]
    ([Provision_Id]);
GO

-- Creating foreign key on [ClienteId] in table 'PedidoSet_PedidoADomicilio'
ALTER TABLE [dbo].[PedidoSet_PedidoADomicilio]
ADD CONSTRAINT [FK_ClientePedidoADomicilio]
    FOREIGN KEY ([ClienteId])
    REFERENCES [dbo].[ClienteSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ClientePedidoADomicilio'
CREATE INDEX [IX_FK_ClientePedidoADomicilio]
ON [dbo].[PedidoSet_PedidoADomicilio]
    ([ClienteId]);
GO

-- Creating foreign key on [MesaId] in table 'PedidoSet_PedidoLocal'
ALTER TABLE [dbo].[PedidoSet_PedidoLocal]
ADD CONSTRAINT [FK_MesaPedidoLocal]
    FOREIGN KEY ([MesaId])
    REFERENCES [dbo].[MesaSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_MesaPedidoLocal'
CREATE INDEX [IX_FK_MesaPedidoLocal]
ON [dbo].[PedidoSet_PedidoLocal]
    ([MesaId]);
GO

-- Creating foreign key on [Ingrediente_Id] in table 'IngredienteReceta'
ALTER TABLE [dbo].[IngredienteReceta]
ADD CONSTRAINT [FK_IngredienteReceta_Ingrediente]
    FOREIGN KEY ([Ingrediente_Id])
    REFERENCES [dbo].[IngredienteSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Receta_id] in table 'IngredienteReceta'
ALTER TABLE [dbo].[IngredienteReceta]
ADD CONSTRAINT [FK_IngredienteReceta_Receta]
    FOREIGN KEY ([Receta_id])
    REFERENCES [dbo].[RecetaSet]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_IngredienteReceta_Receta'
CREATE INDEX [IX_FK_IngredienteReceta_Receta]
ON [dbo].[IngredienteReceta]
    ([Receta_id]);
GO

-- Creating foreign key on [Id] in table 'PedidoSet_PedidoADomicilio'
ALTER TABLE [dbo].[PedidoSet_PedidoADomicilio]
ADD CONSTRAINT [FK_PedidoADomicilio_inherits_Pedido]
    FOREIGN KEY ([Id])
    REFERENCES [dbo].[PedidoSet]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Id] in table 'PedidoSet_PedidoLocal'
ALTER TABLE [dbo].[PedidoSet_PedidoLocal]
ADD CONSTRAINT [FK_PedidoLocal_inherits_Pedido]
    FOREIGN KEY ([Id])
    REFERENCES [dbo].[PedidoSet]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------