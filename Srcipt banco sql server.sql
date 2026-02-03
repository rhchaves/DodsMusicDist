-- Criar a tabela Produtos
USE DodsMusicDev;
GO

CREATE TABLE dbo.Produtos
(
    Id UNIQUEIDENTIFIER NOT NULL,
    Nome NVARCHAR(200) NOT NULL,
    Descricao NVARCHAR(MAX) NULL,
    Ativo BIT NOT NULL,
    Valor DECIMAL(18,2) NOT NULL,
    DataCadastro DATETIME2 NOT NULL,
    Imagem NVARCHAR(500) NULL,
    QuantidadeEstoque INT NOT NULL,

    CONSTRAINT PK_Produtos PRIMARY KEY (Id)
);
GO

-- Alterar tabela existente (ALTER TABLE)
-- Se a tabela já existir e estiver incompleta, exemplo de ajustes comuns:
USE DodsMusicDev;
GO

ALTER TABLE dbo.Produtos
ADD
    Ativo BIT NOT NULL DEFAULT 1,
    QuantidadeEstoque INT NOT NULL DEFAULT 0,
    Imagem NVARCHAR(500) NULL;
GO

-- Ajustar tipo do campo Valor
ALTER TABLE dbo.Produtos
ALTER COLUMN Valor DECIMAL(18,2) NOT NULL;
GO

-- Garantir valor padrão para DataCadastro
ALTER TABLE dbo.Produtos
ADD CONSTRAINT DF_Produtos_DataCadastro
DEFAULT GETDATE() FOR DataCadastro;
GO


-- ==================== ATÉ AQUI JÁ FOI EXECUTADO ==================== --


-- Criar a tabela CarrinhoCliente
CREATE TABLE CarrinhoCliente
(
    Id UNIQUEIDENTIFIER NOT NULL,
    ClienteId UNIQUEIDENTIFIER NOT NULL,
    ValorTotal DECIMAL(18,2) NOT NULL,
    Desconto DECIMAL(18,2) NOT NULL CONSTRAINT DF_CarrinhoCliente_Desconto DEFAULT (0),
    VoucherUtilizado BIT NOT NULL CONSTRAINT DF_CarrinhoCliente_VoucherUtilizado DEFAULT (0),

    -- Voucher (Owned Entity)
    VoucherCodigo VARCHAR(50) NULL,
    Percentual DECIMAL(18,2) NULL,
    TipoDesconto INT NULL,
    ValorDesconto DECIMAL(18,2) NULL,

    CONSTRAINT PK_CarrinhoCliente
        PRIMARY KEY (Id)
);
GO


-- Índice em ClienteId
CREATE INDEX IDX_Cliente
ON CarrinhoCliente (ClienteId);
GO


-- Criar a tabela CarrinhoItens
CREATE TABLE CarrinhoItens
(
    Id UNIQUEIDENTIFIER NOT NULL,
    ProdutoId UNIQUEIDENTIFIER NOT NULL,
    Nome VARCHAR(100) NULL,
    Quantidade INT NOT NULL,
    Valor DECIMAL(18,2) NOT NULL,
    Imagem VARCHAR(100) NULL,
    CarrinhoId UNIQUEIDENTIFIER NOT NULL,

    CONSTRAINT PK_CarrinhoItens
        PRIMARY KEY (Id),

    CONSTRAINT FK_CarrinhoItens_CarrinhoCliente_CarrinhoId
        FOREIGN KEY (CarrinhoId)
        REFERENCES CarrinhoCliente (Id)
        ON DELETE CASCADE
);
GO

-- Índice em CarrinhoId
CREATE INDEX IX_CarrinhoItens_CarrinhoId
ON CarrinhoItens (CarrinhoId);
GO
