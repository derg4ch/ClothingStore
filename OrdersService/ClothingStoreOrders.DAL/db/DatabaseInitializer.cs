using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothingStoreOrders.DAL.db
{
    public static class DatabaseInitializer
    {
        public static void Initialize(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentException("Connection string cannot be null or empty.", nameof(connectionString));
            }

            // Отримуємо connection string для master database
            var masterConnectionString = GetMasterConnectionString(connectionString);

            // Підключення до master для створення бази, якщо її ще нема
            using (var masterConnection = new SqlConnection(masterConnectionString))
            {
                masterConnection.Open();

                var createDbCommand = @"
            IF DB_ID('ClothingStoreOrders') IS NULL
            BEGIN
                CREATE DATABASE ClothingStoreOrders;
            END";

                using var cmd = new SqlCommand(createDbCommand, masterConnection);
                cmd.ExecuteNonQuery();
            }

            // Підключення до нової бази для створення таблиць
            using var conn = new SqlConnection(connectionString);
            conn.Open();


            var createTablesScript = @"
        IF OBJECT_ID('Customers') IS NULL
        CREATE TABLE Customers (
            CustomerId INT IDENTITY(1,1) PRIMARY KEY,
            FullName NVARCHAR(100) NOT NULL,
            Phone NVARCHAR(20),
            Email NVARCHAR(100),
            Address NVARCHAR(200),
            CreatedAt DATETIME DEFAULT GETDATE()
        );

        IF OBJECT_ID('Orders') IS NULL
        CREATE TABLE Orders (
            OrderId INT IDENTITY(1,1) PRIMARY KEY,
            CustomerId INT NOT NULL,
            OrderDate DATETIME DEFAULT GETDATE(),
            Status NVARCHAR(50) DEFAULT 'Pending',
            TotalAmount DECIMAL(10,2) DEFAULT 0,
            ShippingAddress NVARCHAR(200),
            PaymentMethod NVARCHAR(50),
            ShippedDate DATETIME,
            DeliveredDate DATETIME,
            FOREIGN KEY (CustomerId) REFERENCES Customers(CustomerId)
        );

        IF OBJECT_ID('OrderDetails') IS NULL
        CREATE TABLE OrderDetails (
            OrderId INT PRIMARY KEY,
            ShippingMethod NVARCHAR(50),
            TrackingNumber NVARCHAR(100),
            Notes NVARCHAR(500),
            EstimatedDeliveryDate DATETIME,
            FOREIGN KEY (OrderId) REFERENCES Orders(OrderId)
        );

        IF OBJECT_ID('Products') IS NULL
        CREATE TABLE Products (
            ProductId INT IDENTITY(1,1) PRIMARY KEY,
            Name NVARCHAR(100) NOT NULL,
            Price DECIMAL(10,2) NOT NULL,
            SKU NVARCHAR(50)
        );

        IF OBJECT_ID('OrderItems') IS NULL
        CREATE TABLE OrderItems (
            OrderItemId INT IDENTITY(1,1) PRIMARY KEY,
            OrderId INT NOT NULL,
            ProductId INT NOT NULL,
            ProductName NVARCHAR(200) NOT NULL,
            UnitPrice DECIMAL(10,2) NOT NULL,
            Quantity INT NOT NULL CHECK (Quantity > 0),
            Subtotal DECIMAL(10,2) NOT NULL,
            Size NVARCHAR(20),
            Color NVARCHAR(50),
            FOREIGN KEY (OrderId) REFERENCES Orders(OrderId) ON DELETE CASCADE,
            FOREIGN KEY (ProductId) REFERENCES Products(ProductId)
        );

        CREATE INDEX IX_Orders_CustomerId ON Orders(CustomerId);
        CREATE INDEX IX_Orders_Status ON Orders(Status);
        CREATE INDEX IX_OrderItems_OrderId ON OrderItems(OrderId);
        CREATE INDEX IX_OrderItems_ProductId ON OrderItems(ProductId);";

            using var cmd2 = new SqlCommand(createTablesScript, conn);
            cmd2.ExecuteNonQuery();
        }

        private static string GetMasterConnectionString(string connectionString)
        {
            // Парсимо connection string і замінюємо Database на master
            var builder = new SqlConnectionStringBuilder(connectionString);
            builder.InitialCatalog = "master";
            return builder.ConnectionString;
        }
    }
}
