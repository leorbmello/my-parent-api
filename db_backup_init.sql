CREATE TABLE USERS (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(32) NOT NULL,
    Password NVARCHAR(32) NOT NULL,
	Salt NVARCHAR(128) NOT NULL,
	FullName NVARCHAR(128) NOT NULL,
    Email NVARCHAR(128) NOT NULL,
	ParentId1 INT NOT NULL,
	ParentId2 INT NOT NULL,
	FamilyId INT NOT NULL,
	Type TINYINT NOT NULL,
	Active TINYINT NOT NULL,
	Points INT NOT NULL,
    CreatedDate DATETIME DEFAULT GETDATE(),
	ModifiedDate DATETIME,
);

CREATE TABLE FAMILIES (
	Id INT PRIMARY KEY IDENTITY(1,1),
	Name NVARCHAR(32) NOT NULL,
	Description NVARCHAR(128) NOT NULL,
	CreatorId INT NOT NULL,
	Active TINYINT NOT NULL,
    CreatedDate DATETIME DEFAULT GETDATE(),
    ModifiedDate DATETIME 
);
-- As tabelas acima desta linha falam por sí. --------------------------------------------


-- Tarefas podem ser simples tarefas que vão dar pontos, como podem ser uma penalidade.
-- Penalidades terão em geral categoria como "0", por não serem relacionadas a um grupo.
CREATE TABLE TASKS (
	Id INT PRIMARY KEY IDENTITY(1,1),
	FamilyId INT NOT NULL,
	CategoryId INT NOT NULL,
	Name NVARCHAR(32) NOT NULL,
	Description NVARCHAR(128) NOT NULL,
	Points INT NOT NULL,
	IsPenality TINYINT NOT NULL,
	Active TINYINT NOT NULL,
    CreatedDate DATETIME DEFAULT GETDATE(),
);

-- As tarefas podem ser divididas por categoria (Higiene, Tarefas Basicas, Responsabilidades Diarias, etc...)
CREATE TABLE CATEGORIES (
	Id INT PRIMARY KEY IDENTITY(1,1),
	FamilyId INT NOT NULL,
	Name NVARCHAR(32) NOT NULL,
	Description NVARCHAR(128) NOT NULL,
    CreatedDate DATETIME DEFAULT GETDATE(),
);

CREATE TABLE SALES (
	Id INT PRIMARY KEY IDENTITY(1,1),
	UserId INT NOT NULL, -- Usuario
	TotalCost DECIMAL(18, 2) NOT NULL, -- Total da venda (Capa Pedido/Venda)
	Status TINYINT NOT NULL, -- Pendente, Finalizada, Cancelada?
    CreatedDate DATETIME DEFAULT GETDATE(),
    FinishDate DATETIME, -- Definido quando a venda for finalizada
);

-- Produtos da venda efetuada
CREATE TABLE SALES_PRODUCTS (
	Id INT PRIMARY KEY IDENTITY(1,1),
	SaleId INT NOT NULL, -- id da venda
	ProductId INT NOT NULL, -- id do produto
	Amount TINYINT NOT NULL, -- quantidade
	ProductCost DECIMAL(18, 2) NOT NULL, -- custo no ato da compra
	TotalProductCost DECIMAL(18, 2) NOT NULL, -- custo total (Amount * price)
);

-- Produtos podem ser gerenciados por família, cada uma tem a sua.
-- Também podem haver produtos criados automaticamente no insert de uma nova familia.
CREATE TABLE PRODUCTS (
	Id INT PRIMARY KEY IDENTITY(10000,1),
	FamilyId INT NOT NULL,
	Name NVARCHAR(32) NOT NULL,
	Description NVARCHAR(128) NOT NULL,
	Cost DECIMAL(18, 2) NOT NULL,
	Active TINYINT NOT NULL, -- Um produto pode ser "excluido", ficando inativo
	Amount INT NOT NULL, -- Quantidade disponível atual
	AmountLimit INT NOT NULL, -- Limite de Compras por semana talves?
    CreatedDate DATETIME DEFAULT GETDATE(), --
	ModifiedDate DATETIME -- 
);

CREATE TABLE LOGHISTORY (
	Id INT PRIMARY KEY IDENTITY(1,1),
	UserId INT NOT NULL, -- Usuario
	OperationType NVARCHAR(50) NOT NULL, -- Tipo de operacao
	Text NVARCHAR(128) NOT NULL, -- O que foi alterado
    Time DATETIME DEFAULT GETDATE(), -- Data/Hora
);

-- Operação pode ser: TrocaDeSenha, HistoricoPedido, etc...
-- Porem, vale observar que pode ser melhor separar histori
