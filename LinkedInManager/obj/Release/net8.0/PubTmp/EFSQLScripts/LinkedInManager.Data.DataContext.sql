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

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240314162630_Initial'
)
BEGIN
    CREATE TABLE [LinkedInUsers] (
        [Id] int NOT NULL IDENTITY,
        [FirstName] nvarchar(max) NOT NULL,
        [LastName] nvarchar(max) NOT NULL,
        [Email] nvarchar(max) NOT NULL,
        [ClientId] nvarchar(max) NOT NULL,
        [ClientPassword] nvarchar(max) NOT NULL,
        [CompanyURL] nvarchar(max) NOT NULL,
        [RedirectURL] nvarchar(max) NOT NULL,
        [AccessToken] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_LinkedInUsers] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240314162630_Initial'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20240314162630_Initial', N'8.0.4');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240328153127_LinkedInUser'
)
BEGIN
    ALTER TABLE [LinkedInUsers] ADD [LinkedInProfile] nvarchar(max) NOT NULL DEFAULT N'';
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240328153127_LinkedInUser'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20240328153127_LinkedInUser', N'8.0.4');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240328164524_AppUser'
)
BEGIN
    CREATE TABLE [AppUsers] (
        [Id] int NOT NULL IDENTITY,
        [FirstName] nvarchar(max) NOT NULL,
        [LastName] nvarchar(max) NOT NULL,
        [Email] nvarchar(max) NOT NULL,
        [SMTPAppPassword] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_AppUsers] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240328164524_AppUser'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20240328164524_AppUser', N'8.0.4');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240331181335_AddingTableLNEmployees'
)
BEGIN
    CREATE TABLE [LinkedInEmployees] (
        [Id] int NOT NULL IDENTITY,
        [FirstName] nvarchar(max) NOT NULL,
        [LastName] nvarchar(max) NOT NULL,
        [Name] nvarchar(max) NOT NULL,
        [State] nvarchar(max) NOT NULL,
        [City] nvarchar(max) NOT NULL,
        [Country] nvarchar(max) NOT NULL,
        [Email] nvarchar(max) NOT NULL,
        [PhoneNumber] nvarchar(max) NOT NULL,
        [PhoneNumberType] nvarchar(max) NOT NULL,
        [LinkedInUrl] nvarchar(max) NOT NULL,
        [Ranking] int NOT NULL,
        CONSTRAINT [PK_LinkedInEmployees] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240331181335_AddingTableLNEmployees'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20240331181335_AddingTableLNEmployees', N'8.0.4');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240331182550_UpdateStatePropertyToNullable'
)
BEGIN
    DECLARE @var0 sysname;
    SELECT @var0 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[LinkedInEmployees]') AND [c].[name] = N'State');
    IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [LinkedInEmployees] DROP CONSTRAINT [' + @var0 + '];');
    ALTER TABLE [LinkedInEmployees] ALTER COLUMN [State] nvarchar(max) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240331182550_UpdateStatePropertyToNullable'
)
BEGIN
    DECLARE @var1 sysname;
    SELECT @var1 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[LinkedInEmployees]') AND [c].[name] = N'PhoneNumberType');
    IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [LinkedInEmployees] DROP CONSTRAINT [' + @var1 + '];');
    ALTER TABLE [LinkedInEmployees] ALTER COLUMN [PhoneNumberType] nvarchar(max) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240331182550_UpdateStatePropertyToNullable'
)
BEGIN
    DECLARE @var2 sysname;
    SELECT @var2 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[LinkedInEmployees]') AND [c].[name] = N'PhoneNumber');
    IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [LinkedInEmployees] DROP CONSTRAINT [' + @var2 + '];');
    ALTER TABLE [LinkedInEmployees] ALTER COLUMN [PhoneNumber] nvarchar(max) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240331182550_UpdateStatePropertyToNullable'
)
BEGIN
    DECLARE @var3 sysname;
    SELECT @var3 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[LinkedInEmployees]') AND [c].[name] = N'Name');
    IF @var3 IS NOT NULL EXEC(N'ALTER TABLE [LinkedInEmployees] DROP CONSTRAINT [' + @var3 + '];');
    ALTER TABLE [LinkedInEmployees] ALTER COLUMN [Name] nvarchar(max) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240331182550_UpdateStatePropertyToNullable'
)
BEGIN
    DECLARE @var4 sysname;
    SELECT @var4 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[LinkedInEmployees]') AND [c].[name] = N'LinkedInUrl');
    IF @var4 IS NOT NULL EXEC(N'ALTER TABLE [LinkedInEmployees] DROP CONSTRAINT [' + @var4 + '];');
    ALTER TABLE [LinkedInEmployees] ALTER COLUMN [LinkedInUrl] nvarchar(max) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240331182550_UpdateStatePropertyToNullable'
)
BEGIN
    DECLARE @var5 sysname;
    SELECT @var5 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[LinkedInEmployees]') AND [c].[name] = N'LastName');
    IF @var5 IS NOT NULL EXEC(N'ALTER TABLE [LinkedInEmployees] DROP CONSTRAINT [' + @var5 + '];');
    ALTER TABLE [LinkedInEmployees] ALTER COLUMN [LastName] nvarchar(max) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240331182550_UpdateStatePropertyToNullable'
)
BEGIN
    DECLARE @var6 sysname;
    SELECT @var6 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[LinkedInEmployees]') AND [c].[name] = N'FirstName');
    IF @var6 IS NOT NULL EXEC(N'ALTER TABLE [LinkedInEmployees] DROP CONSTRAINT [' + @var6 + '];');
    ALTER TABLE [LinkedInEmployees] ALTER COLUMN [FirstName] nvarchar(max) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240331182550_UpdateStatePropertyToNullable'
)
BEGIN
    DECLARE @var7 sysname;
    SELECT @var7 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[LinkedInEmployees]') AND [c].[name] = N'Email');
    IF @var7 IS NOT NULL EXEC(N'ALTER TABLE [LinkedInEmployees] DROP CONSTRAINT [' + @var7 + '];');
    ALTER TABLE [LinkedInEmployees] ALTER COLUMN [Email] nvarchar(max) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240331182550_UpdateStatePropertyToNullable'
)
BEGIN
    DECLARE @var8 sysname;
    SELECT @var8 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[LinkedInEmployees]') AND [c].[name] = N'Country');
    IF @var8 IS NOT NULL EXEC(N'ALTER TABLE [LinkedInEmployees] DROP CONSTRAINT [' + @var8 + '];');
    ALTER TABLE [LinkedInEmployees] ALTER COLUMN [Country] nvarchar(max) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240331182550_UpdateStatePropertyToNullable'
)
BEGIN
    DECLARE @var9 sysname;
    SELECT @var9 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[LinkedInEmployees]') AND [c].[name] = N'City');
    IF @var9 IS NOT NULL EXEC(N'ALTER TABLE [LinkedInEmployees] DROP CONSTRAINT [' + @var9 + '];');
    ALTER TABLE [LinkedInEmployees] ALTER COLUMN [City] nvarchar(max) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240331182550_UpdateStatePropertyToNullable'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20240331182550_UpdateStatePropertyToNullable', N'8.0.4');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240412075524_Update'
)
BEGIN
    ALTER TABLE [LinkedInEmployees] ADD [Headline] nvarchar(max) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240412075524_Update'
)
BEGIN
    ALTER TABLE [LinkedInEmployees] ADD [Seniority] nvarchar(max) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240412075524_Update'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20240412075524_Update', N'8.0.4');
END;
GO

COMMIT;
GO

