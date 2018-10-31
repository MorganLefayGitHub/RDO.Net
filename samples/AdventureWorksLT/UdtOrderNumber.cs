﻿using DevZest.Data;
using DevZest.Data.SqlServer;
using DevZest.Data.Annotations.Primitives;

namespace DevZest.Samples.AdventureWorksLT
{
    public sealed class UdtOrderNumber : ColumnAttribute
    {
        protected override void Wireup(Column column)
        {
            column.Nullable(true);
            ((Column<string>)column).AsNVarChar(25);
        }
    }
}
