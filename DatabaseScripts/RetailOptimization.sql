CREATE DATABASE RetailOptimizationDb;
GO

USE RetailOptimizationDb;
GO

CREATE TABLE Products (
    ProductId INT PRIMARY KEY IDENTITY(1,1),
    ProductName NVARCHAR(100) NOT NULL,
    Category NVARCHAR(100) NOT NULL,
    Price DECIMAL(18,2) NOT NULL,
    StockQuantity INT NOT NULL,
    ReorderLevel INT NOT NULL,
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE()
);

CREATE TABLE Orders (
    OrderId INT PRIMARY KEY IDENTITY(1,1),
    CustomerName NVARCHAR(100) NOT NULL,
    CustomerEmail NVARCHAR(100) NOT NULL,
    OrderDate DATETIME NOT NULL DEFAULT GETDATE(),
    TotalAmount DECIMAL(18,2) NOT NULL,
    Status NVARCHAR(50) NOT NULL
);

CREATE TABLE OrderItems (
    OrderItemId INT PRIMARY KEY IDENTITY(1,1),
    OrderId INT NOT NULL,
    ProductId INT NOT NULL,
    Quantity INT NOT NULL,
    UnitPrice DECIMAL(18,2) NOT NULL,

    CONSTRAINT FK_OrderItems_Orders 
        FOREIGN KEY (OrderId) REFERENCES Orders(OrderId),

    CONSTRAINT FK_OrderItems_Products 
        FOREIGN KEY (ProductId) REFERENCES Products(ProductId)
);

INSERT INTO Products(ProductName, Category, Price, StockQuantity, ReorderLevel)
VALUES
('Laptop', 'Electronics', 55000, 15, 5),
('Wireless Mouse', 'Accessories', 799, 50, 10),
('Keyboard', 'Accessories', 1200, 8, 10);

CREATE TABLE StockAuditLogs (
    LogId INT PRIMARY KEY IDENTITY(1,1),
    ProductId INT NOT NULL,
    OldStock INT NOT NULL,
    NewStock INT NOT NULL,
    UpdatedAt DATETIME DEFAULT GETDATE()
);
GO

CREATE TRIGGER trg_StockUpdate
ON Products
AFTER UPDATE
AS
BEGIN
    INSERT INTO StockAuditLogs(ProductId, OldStock, NewStock)
    SELECT 
        i.ProductId,
        d.StockQuantity,
        i.StockQuantity
    FROM inserted i
    INNER JOIN deleted d 
        ON i.ProductId = d.ProductId
    WHERE i.StockQuantity <> d.StockQuantity;
END;
GO