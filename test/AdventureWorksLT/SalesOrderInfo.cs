﻿using DevZest.Data;
using DevZest.Data.Annotations;

namespace DevZest.Samples.AdventureWorksLT
{
    [ModelExtender(typeof(Ext))]
    public class SalesOrderInfo : SalesOrder
    {
        public class Ext : ModelExtender
        {
            static Ext()
            {
                RegisterChildExtender((Ext _) => _.Customer);
                RegisterChildExtender((Ext _) => _.ShipToAddress);
                RegisterChildExtender((Ext _) => _.BillToAddress);
            }

            public Customer.Lookup Customer { get; private set; }
            public Address.Lookup ShipToAddress { get; private set; }
            public Address.Lookup BillToAddress { get; private set; }
        }

        public new SalesOrderDetailInfo SalesOrderDetails
        {
            get { return (SalesOrderDetailInfo)base.SalesOrderDetails; }
        }

        protected sealed override SalesOrderDetail CreateSalesOrderDetail()
        {
            return CreateSalesOrderDetailInfo();
        }

        protected virtual SalesOrderDetailInfo CreateSalesOrderDetailInfo()
        {
            return new SalesOrderDetailInfo();
        }
    }
}
