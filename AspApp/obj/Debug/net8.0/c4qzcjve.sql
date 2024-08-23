IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [Posts] (
    [Id] int NOT NULL IDENTITY,
    [Description] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Posts] PRIMARY KEY ([Id])
);
GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Description') AND [object_id] = OBJECT_ID(N'[Posts]'))
    SET IDENTITY_INSERT [Posts] ON;
INSERT INTO [Posts] ([Id], [Description])
VALUES (1, N'Hello World 2 !!');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Description') AND [object_id] = OBJECT_ID(N'[Posts]'))
    SET IDENTITY_INSERT [Posts] OFF;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240814143556_init', N'8.0.8');
GO

COMMIT;
GO

