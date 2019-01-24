﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using DevZest.Samples.AdventureWorksLT;
using System.Text;

namespace DevZest.Data
{
    [TestClass]
    public class MockDbTests : AdventureWorksTestsBase
    {
        /// <remarks><see cref="ProductCategory"/> is chosen for only having self referencing foreign key.</remarks>
        [TestMethod]
        public void MockDb_ProductCategory()
        {
            var log = new StringBuilder();
            using (var db = new ProductCategoryMockDb().InitializeAsync(OpenDbAsync(log).Result).Result)
            {
                Assert.AreEqual(13, db.ProductCategory.CountAsync().Result);
            }
        }


        [TestMethod]
        public void MockDb_SalesOrder()
        {
            var log = new StringBuilder();
            using (var db = new EmptySalesOrderMockDb().InitializeAsync(OpenDbAsync(log).Result).Result)
            {
                Assert.AreEqual(0, db.SalesOrderHeader.CountAsync().Result);
                Assert.AreEqual(0, db.SalesOrderDetail.CountAsync().Result);
            }
        }
    }
}
