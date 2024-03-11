﻿IF OBJECT_ID(N'[curso_ef_core]') IS NULL
BEGIN
    CREATE TABLE [curso_ef_core] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK_curso_ef_core] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [curso_ef_core]
    WHERE [MigrationId] = N'20240305001914_PrimeiraMigracao'
)
BEGIN
    CREATE TABLE [Clientes] (
        [Id] int NOT NULL IDENTITY,
        [Nome] VARCHAR(80) NOT NULL,
        [Phone] CHAR(11) NOT NULL,
        [CEP] CHAR(8) NOT NULL,
        [Estado] CHAR(2) NOT NULL,
        [Cidade] nvarchar(60) NOT NULL,
        CONSTRAINT [PK_Clientes] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [curso_ef_core]
    WHERE [MigrationId] = N'20240305001914_PrimeiraMigracao'
)
BEGIN
    CREATE TABLE [Produtos] (
        [Id] int NOT NULL IDENTITY,
        [CodigoBarras] VARCHAR(14) NOT NULL,
        [Descricao] VARCHAR(60) NOT NULL,
        [Valor] decimal(18,2) NOT NULL,
        [TipoProduto] nvarchar(max) NOT NULL,
        [Ativo] bit NOT NULL,
        CONSTRAINT [PK_Produtos] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [curso_ef_core]
    WHERE [MigrationId] = N'20240305001914_PrimeiraMigracao'
)
BEGIN
    CREATE TABLE [Pedidos] (
        [Id] int NOT NULL IDENTITY,
        [ClienteId] int NOT NULL,
        [IniciadoEm] datetime2 NOT NULL DEFAULT (GETDATE()),
        [FinalizadoEm] datetime2 NOT NULL,
        [TipoFrete] int NOT NULL,
        [Status] nvarchar(max) NOT NULL,
        [Observacao] VARCHAR(512) NOT NULL,
        CONSTRAINT [PK_Pedidos] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Pedidos_Clientes_ClienteId] FOREIGN KEY ([ClienteId]) REFERENCES [Clientes] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [curso_ef_core]
    WHERE [MigrationId] = N'20240305001914_PrimeiraMigracao'
)
BEGIN
    CREATE TABLE [PedidoItens] (
        [Id] int NOT NULL IDENTITY,
        [PedidoId] int NOT NULL,
        [ProdutoId] int NOT NULL,
        [Quantidade] int NOT NULL DEFAULT 0,
        [Valor] decimal(18,2) NOT NULL DEFAULT 0.0,
        [Desconto] decimal(18,2) NOT NULL DEFAULT 0.0,
        CONSTRAINT [PK_PedidoItens] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_PedidoItens_Pedidos_PedidoId] FOREIGN KEY ([PedidoId]) REFERENCES [Pedidos] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_PedidoItens_Produtos_ProdutoId] FOREIGN KEY ([ProdutoId]) REFERENCES [Produtos] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [curso_ef_core]
    WHERE [MigrationId] = N'20240305001914_PrimeiraMigracao'
)
BEGIN
    CREATE INDEX [idx_cliente_telefone] ON [Clientes] ([Phone]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [curso_ef_core]
    WHERE [MigrationId] = N'20240305001914_PrimeiraMigracao'
)
BEGIN
    CREATE INDEX [IX_PedidoItens_PedidoId] ON [PedidoItens] ([PedidoId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [curso_ef_core]
    WHERE [MigrationId] = N'20240305001914_PrimeiraMigracao'
)
BEGIN
    CREATE INDEX [IX_PedidoItens_ProdutoId] ON [PedidoItens] ([ProdutoId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [curso_ef_core]
    WHERE [MigrationId] = N'20240305001914_PrimeiraMigracao'
)
BEGIN
    CREATE INDEX [IX_Pedidos_ClienteId] ON [Pedidos] ([ClienteId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [curso_ef_core]
    WHERE [MigrationId] = N'20240305001914_PrimeiraMigracao'
)
BEGIN
    INSERT INTO [curso_ef_core] ([MigrationId], [ProductVersion])
    VALUES (N'20240305001914_PrimeiraMigracao', N'8.0.2');
END;
GO

COMMIT;
GO

