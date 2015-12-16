
Drop table [dbo].[Changes];
Drop table [dbo].[Log];
Drop table [dbo].[FactReports];
Drop table [dbo].[DimStatus];
Drop table [dbo].[DimUsers];
Drop table [dbo].[DimCategories];

CREATE TABLE [dbo].[DimCategories] (
    [CategoryId] INT           NOT NULL,
    [Category]   NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_Categories] PRIMARY KEY CLUSTERED ([CategoryId] ASC)
);

CREATE TABLE [dbo].[DimStatus] (
    [StatusID]   INT           NOT NULL,
    [StatusName] NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_DimStatus] PRIMARY KEY CLUSTERED ([StatusID] ASC)
);

CREATE TABLE [dbo].[DimUsers] (
    [UserName]     NVARCHAR (20)  NOT NULL,
    [Password]     NVARCHAR (50)  NOT NULL,
    [Salt]         NVARCHAR (50)  NOT NULL,
    [Permisssions] NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_DimUsers] PRIMARY KEY CLUSTERED ([UserName] ASC)
);

CREATE TABLE [dbo].[FactReports] (
    [ReportID]    INT            IDENTITY (1, 1) NOT NULL,
    [TimeStamp]   DATETIME       NOT NULL,
    [URL]         NVARCHAR (MAX) NOT NULL,
    [ScreenShot]  NVARCHAR (MAX) NOT NULL,
    [CategoryID]  INT            NULL,
    [Location]    NVARCHAR (50)  NULL,
    [Name]        NVARCHAR (50)  NULL,
    [LastName]    NVARCHAR (50)  NULL,
    [Phone]       NVARCHAR (10)  NULL,
    [Description] NVARCHAR (MAX) NULL,
    [Email]       NVARCHAR (50)  NULL,
    [StatusID]    INT            NULL,
    PRIMARY KEY CLUSTERED ([ReportID] ASC),
    FOREIGN KEY ([CategoryID]) REFERENCES [dbo].[DimCategories] ([CategoryId]),
    FOREIGN KEY ([StatusID]) REFERENCES [dbo].[DimStatus] ([StatusID])
);
CREATE TABLE [dbo].[Changes] (
    [Id]       INT            IDENTITY (1, 1) NOT NULL,
    [UserName] NVARCHAR (20)  NULL,
    [ReportID] INT            NULL,
    [Data]     NVARCHAR (MAX) NULL,
    [Time]     DATETIME       NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Changes_ToReports] FOREIGN KEY ([ReportID]) REFERENCES [dbo].[FactReports] ([ReportID]),
    CONSTRAINT [FK_Changes_ToUsers] FOREIGN KEY ([UserName]) REFERENCES [dbo].[DimUsers] ([UserName])
);
CREATE TABLE [dbo].[Log] (
    [Id]        INT            IDENTITY (1, 1) NOT NULL,
    [TimeStamp] DATETIME       NULL,
    [Type]      INT            NULL,
    [Data]      NVARCHAR (MAX) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

