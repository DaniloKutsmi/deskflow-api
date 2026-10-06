IF DB_ID('DeskFlowDb') IS NULL
    CREATE DATABASE DeskFlowDb;
GO

USE DeskFlowDb;
GO

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
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261005143349_CriarTabelaCategoria'
)
BEGIN
    CREATE TABLE [Categorias] (
        [Id] int NOT NULL IDENTITY,
        [Nome] nvarchar(100) NOT NULL,
        CONSTRAINT [PK_Categorias] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261005143349_CriarTabelaCategoria'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20261005143349_CriarTabelaCategoria', N'10.0.12');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261005154552_CriarTabelaChamado'
)
BEGIN
    CREATE TABLE [Chamados] (
        [Id] int NOT NULL IDENTITY,
        [Titulo] nvarchar(150) NOT NULL,
        [Descricao] nvarchar(max) NOT NULL,
        [Prioridade] nvarchar(10) NOT NULL,
        [Status] nvarchar(20) NOT NULL,
        [SolicitanteNome] nvarchar(150) NOT NULL,
        [DataAbertura] datetime2 NOT NULL,
        [DataFechamento] datetime2 NULL,
        [Solucao] nvarchar(max) NULL,
        [CategoriaId] int NOT NULL,
        CONSTRAINT [PK_Chamados] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Chamados_Categorias_CategoriaId] FOREIGN KEY ([CategoriaId]) REFERENCES [Categorias] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261005154552_CriarTabelaChamado'
)
BEGIN
    CREATE INDEX [IX_Chamados_CategoriaId] ON [Chamados] ([CategoriaId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261005154552_CriarTabelaChamado'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20261005154552_CriarTabelaChamado', N'10.0.12');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261005222118_TornarStatusChamadoOpcional'
)
BEGIN
    DECLARE @var nvarchar(max);
    SELECT @var = QUOTENAME([d].[name])
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Chamados]') AND [c].[name] = N'Status');
    IF @var IS NOT NULL EXEC(N'ALTER TABLE [Chamados] DROP CONSTRAINT ' + @var + ';');
    ALTER TABLE [Chamados] ALTER COLUMN [Status] nvarchar(20) NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261005222118_TornarStatusChamadoOpcional'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20261005222118_TornarStatusChamadoOpcional', N'10.0.12');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261005231920_CriarTabelaInteracao'
)
BEGIN
    CREATE TABLE [Interacoes] (
        [Id] int NOT NULL IDENTITY,
        [ChamadoId] int NOT NULL,
        [Autor] nvarchar(100) NOT NULL,
        [Mensagem] nvarchar(max) NOT NULL,
        [DataRegistro] datetime2 NOT NULL,
        CONSTRAINT [PK_Interacoes] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Interacoes_Chamados_ChamadoId] FOREIGN KEY ([ChamadoId]) REFERENCES [Chamados] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261005231920_CriarTabelaInteracao'
)
BEGIN
    CREATE INDEX [IX_Interacoes_ChamadoId] ON [Interacoes] ([ChamadoId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261005231920_CriarTabelaInteracao'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20261005231920_CriarTabelaInteracao', N'10.0.12');
END;

COMMIT;
GO

