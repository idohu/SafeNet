
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, and Azure
-- --------------------------------------------------
-- Date Created: 06/25/2015 15:30:37
-- Generated from EDMX file: C:\Users\Ido\Documents\Visual Studio 2012\Projects\DigiGuard\DigiGuard\DigiGuardModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [safenet];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK__FactRepor__Categ__4D94879B]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[FactReports] DROP CONSTRAINT [FK__FactRepor__Categ__4D94879B];
GO
IF OBJECT_ID(N'[dbo].[FK__FactRepor__Statu__4E88ABD4]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[FactReports] DROP CONSTRAINT [FK__FactRepor__Statu__4E88ABD4];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[DimCategories]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DimCategories];
GO
IF OBJECT_ID(N'[dbo].[DimStatus]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DimStatus];
GO
IF OBJECT_ID(N'[dbo].[DimUsers]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DimUsers];
GO
IF OBJECT_ID(N'[dbo].[FactReports]', 'U') IS NOT NULL
    DROP TABLE [dbo].[FactReports];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'DimCategories'
CREATE TABLE [dbo].[DimCategories] (
    [CategoryId] int  NOT NULL,
    [Category] nvarchar(50)  NOT NULL
);
GO

-- Creating table 'DimStatus'
CREATE TABLE [dbo].[DimStatus] (
    [StatusID] int  NOT NULL,
    [StatusName] nvarchar(50)  NOT NULL
);
GO

-- Creating table 'DimUsers'
CREATE TABLE [dbo].[DimUsers] (
    [UserName] nvarchar(20)  NOT NULL,
    [Password] nvarchar(10)  NOT NULL
);
GO

-- Creating table 'FactReports'
CREATE TABLE [dbo].[FactReports] (
    [ReportID] int IDENTITY(1,1) NOT NULL,
    [TimeStamp] datetime  NOT NULL,
    [URL] nvarchar(max)  NOT NULL,
    [ScreenShot] nvarchar(max)  NOT NULL,
    [CategoryID] int  NULL,
    [Location] nvarchar(50)  NULL,
    [Name] nvarchar(50)  NULL,
    [LastName] nvarchar(50)  NULL,
    [Phone] nvarchar(10)  NULL,
    [Description] nvarchar(max)  NULL,
    [Email] nvarchar(50)  NULL,
    [StatusID] int  NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [CategoryId] in table 'DimCategories'
ALTER TABLE [dbo].[DimCategories]
ADD CONSTRAINT [PK_DimCategories]
    PRIMARY KEY CLUSTERED ([CategoryId] ASC);
GO

-- Creating primary key on [StatusID] in table 'DimStatus'
ALTER TABLE [dbo].[DimStatus]
ADD CONSTRAINT [PK_DimStatus]
    PRIMARY KEY CLUSTERED ([StatusID] ASC);
GO

-- Creating primary key on [UserName] in table 'DimUsers'
ALTER TABLE [dbo].[DimUsers]
ADD CONSTRAINT [PK_DimUsers]
    PRIMARY KEY CLUSTERED ([UserName] ASC);
GO

-- Creating primary key on [ReportID] in table 'FactReports'
ALTER TABLE [dbo].[FactReports]
ADD CONSTRAINT [PK_FactReports]
    PRIMARY KEY CLUSTERED ([ReportID] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [CategoryID] in table 'FactReports'
ALTER TABLE [dbo].[FactReports]
ADD CONSTRAINT [FK__FactRepor__Categ__4D94879B]
    FOREIGN KEY ([CategoryID])
    REFERENCES [dbo].[DimCategories]
        ([CategoryId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK__FactRepor__Categ__4D94879B'
CREATE INDEX [IX_FK__FactRepor__Categ__4D94879B]
ON [dbo].[FactReports]
    ([CategoryID]);
GO

-- Creating foreign key on [StatusID] in table 'FactReports'
ALTER TABLE [dbo].[FactReports]
ADD CONSTRAINT [FK__FactRepor__Statu__4E88ABD4]
    FOREIGN KEY ([StatusID])
    REFERENCES [dbo].[DimStatus]
        ([StatusID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK__FactRepor__Statu__4E88ABD4'
CREATE INDEX [IX_FK__FactRepor__Statu__4E88ABD4]
ON [dbo].[FactReports]
    ([StatusID]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------