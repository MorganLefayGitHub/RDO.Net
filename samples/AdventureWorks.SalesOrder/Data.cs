﻿using DevZest.Data;
using DevZest.Samples.AdventureWorksLT;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace AdventureWorks.SalesOrders
{
    static class Data
    {
        public static Task<DataSet<SalesOrder>> GetSalesOrdersAsync(string filterText, IReadOnlyList<IColumnComparer> orderBy, CancellationToken ct)
        {
            DbSet<SalesOrder> salesOrders;
            using (var db = Db.Open(App.ConnectionString))
            {
                if (string.IsNullOrEmpty(filterText))
                    salesOrders = db.SalesOrders;
                else
                    salesOrders = db.SalesOrders.Where(_ => _.SalesOrderNumber.Contains(filterText) | _.PurchaseOrderNumber.Contains(filterText));
            }

            if (orderBy != null && orderBy.Count > 0)
                salesOrders = salesOrders.OrderBy(GetOrderBy(salesOrders._, orderBy));

            return salesOrders.ToDataSetAsync(ct);
        }

        private static ColumnSort[] GetOrderBy(Model model, IReadOnlyList<IColumnComparer> orderBy)
        {
            Debug.Assert(orderBy != null && orderBy.Count > 0);
            var result = new ColumnSort[orderBy.Count];
            for (int i = 0; i < orderBy.Count; i++)
            {
                var column = orderBy[i].GetColumn(model);
                var direction = orderBy[i].Direction;
                result[i] = direction == SortDirection.Descending ? column.Desc() : column.Asc();
            }
            return result;
        }
    }
}
