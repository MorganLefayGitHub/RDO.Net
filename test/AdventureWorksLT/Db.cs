﻿using DevZest.Data;
using DevZest.Data.SqlServer;
using System;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;

namespace DevZest.Samples.AdventureWorksLT
{
    public class Db : SqlSession
    {
        public static Db Open(string connectionString)
        {
            return Open(connectionString, null);
        }

        public static Db Open(string connectionString, Action<Db> initializer)
        {
            var result = InternalOpen(connectionString, initializer);
            result.OpenConnection();
            return result;
        }

        private static Db InternalOpen(string connectionString, Action<Db> initializer)
        {
            if (string.IsNullOrEmpty(connectionString))
                throw new ArgumentNullException("connectionString");
            var result = new Db(new SqlConnection(connectionString));
            if (initializer != null)
                initializer(result);
            return result;
        }

        public static Task<Db> OpenAsync(string connectionString)
        {
            return OpenAsync(connectionString, null);
        }

        public static async Task<Db> OpenAsync(string connectionString, Action<Db> initializer)
        {
            var result = InternalOpen(connectionString, initializer);
            await result.OpenConnectionAsync(CancellationToken.None);
            return result;
        }

        public static Db Open(SqlVersion sqlVersion)
        {
            var result = new Db(new SqlConnection());
            result.SqlVersion = sqlVersion;
            return result;
        }

        private Db(SqlConnection sqlConnection)
            : base(sqlConnection)
        {
        }

        protected Db(SqlVersion sqlVersion)
            : base(new SqlConnection())
        {
            SqlVersion = sqlVersion;
        }

        private DbTable<Address> _addresses;
        public DbTable<Address> Addresses
        {
            get { return GetTable(ref _addresses, "[SalesLT].[Address]"); }
        }

        private DbTable<Customer> _customers;
        public DbTable<Customer> Customers
        {
            get { return GetTable(ref _customers, "[SalesLT].[Customer]"); }
        }

        private DbTable<CustomerAddress> _customerAddresses;
        public DbTable<CustomerAddress> CustomerAddresses
        {
            get
            {
                return GetTable(ref _customerAddresses, "[SalesLT].[Customer]", m =>
                {
                    ForeignKey(null, m.Customer, Customers._, ForeignKeyAction.NoAction, ForeignKeyAction.NoAction);
                    ForeignKey(null, m.Address, Addresses._, ForeignKeyAction.NoAction, ForeignKeyAction.NoAction);
                });
            }
        }

        private DbTable<ProductCategory> _productCategories;
        public DbTable<ProductCategory> ProductCategories
        {
            get { return GetTable(ref _productCategories, "[SalesLT].[ProductCategory]", m =>
            {
                ForeignKey(null, m.ParentProductCategory, m, ForeignKeyAction.NoAction, ForeignKeyAction.NoAction);
            }); }
        }

        private DbTable<ProductModel> _productModels;
        public DbTable<ProductModel> ProductModels
        {
            get { return GetTable(ref _productModels, "[SalesLT].[ProductModel]"); }
        }

        private DbTable<ProductDescription> _productDescriptions;
        public DbTable<ProductDescription> ProductDescriptions
        {
            get { return GetTable(ref _productDescriptions, "[SalesLT].[ProductDescription]"); }
        }

        private DbTable<ProductModelProductDescription> _productModelProductDescriptions;
        public DbTable<ProductModelProductDescription> ProductModelProductDescriptions
        {
            get
            {
                return GetTable(ref _productModelProductDescriptions, "[SalesLT].[ProductModelProductDescription]", m =>
                {
                    ForeignKey(null, m.ProductModel, ProductModels._, ForeignKeyAction.NoAction, ForeignKeyAction.NoAction);
                    ForeignKey(null, m.ProductDescription, ProductDescriptions._, ForeignKeyAction.NoAction, ForeignKeyAction.NoAction);
                });
            }
        }

        private DbTable<Product> _products;
        public DbTable<Product> Products
        {
            get
            {
                return GetTable(ref _products, "[SalesLT].[Product]", m =>
                {
                    ForeignKey(null, m.ProductModel, ProductModels._, ForeignKeyAction.NoAction, ForeignKeyAction.NoAction);
                    ForeignKey(null, m.ProductCategory, ProductCategories._, ForeignKeyAction.NoAction, ForeignKeyAction.NoAction);
                });
            }
        }

        private DbTable<SalesOrder> _salesOrders;
        public DbTable<SalesOrder> SalesOrders
        {
            get
            {
                return GetTable(ref _salesOrders, "[SalesLT].[SalesOrderHeader]", m =>
                {
                    ForeignKey(null, m.Customer, Customers._, ForeignKeyAction.NoAction, ForeignKeyAction.NoAction);
                    ForeignKey(null, m.BillToAddress, Addresses._, ForeignKeyAction.NoAction, ForeignKeyAction.NoAction);
                    ForeignKey(null, m.ShipToAddress, Addresses._, ForeignKeyAction.NoAction, ForeignKeyAction.NoAction);
                });
            }
        }

        private DbTable<SalesOrderDetail> _salesOrderDetails;
        public DbTable<SalesOrderDetail> SalesOrderDetails
        {
            get
            {
                return GetTable(ref _salesOrderDetails, "[SalesLT].[SalesOrderDetail]", m =>
                {
                    ForeignKey(null, m.SalesOrder, SalesOrders._, ForeignKeyAction.NoAction, ForeignKeyAction.NoAction);
                    ForeignKey(null, m.Product, Products._, ForeignKeyAction.NoAction, ForeignKeyAction.NoAction);
                });
            }
        }
    }
}
