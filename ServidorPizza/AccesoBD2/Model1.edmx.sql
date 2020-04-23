
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 04/19/2020 23:34:25
-- Generated from EDMX file: C:\Users\javie\Documents\8voSemestre\Desarrollo de software\Proyecto\ServidorLoteria\AccesoBD2\Model1.edmx
-- Date Created: 04/20/2020 18:44:29
-- Generated from EDMX file: C:\Users\BETO\Documents\OCTAVO SEMESTRE\DESARROLLO DE SOFTWARE\ServidorPizza\AccesoBD2\Model1.edmx
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

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[CuentaUsuarioSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CuentaUsuarioSet];
GO
IF OBJECT_ID(N'[dbo].[EmpleadoSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[EmpleadoSet];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'CuentaUsuarioSet'
CREATE TABLE [dbo].[CuentaUsuarioSet] (
    [IdCuentaUsuario] int IDENTITY(1,1) NOT NULL,
    [nombreUsuario] nvarchar(max)  NOT NULL,
    [contraseña] nvarchar(max)  NOT NULL,
    [Empleado_IdEmpleado] int  NOT NULL
);
GO

-- Creating table 'EmpleadoSet'
CREATE TABLE [dbo].[EmpleadoSet] (
    [IdEmpleado] int IDENTITY(1,1) NOT NULL,
    [nombre] nvarchar(max)  NOT NULL,
    [apellidoPaterno] nvarchar(max)  NOT NULL,
    [apellidoMaterno] nvarchar(max)  NOT NULL,
    [telefono] nvarchar(max)  NOT NULL,
    [correo] nvarchar(max)  NOT NULL,
    [rol] nvarchar(max)  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [IdCuentaUsuario] in table 'CuentaUsuarioSet'
ALTER TABLE [dbo].[CuentaUsuarioSet]
ADD CONSTRAINT [PK_CuentaUsuarioSet]
    PRIMARY KEY CLUSTERED ([IdCuentaUsuario] ASC);
GO

-- Creating primary key on [IdEmpleado] in table 'EmpleadoSet'
ALTER TABLE [dbo].[EmpleadoSet]
ADD CONSTRAINT [PK_EmpleadoSet]
    PRIMARY KEY CLUSTERED ([IdEmpleado] ASC);
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

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------