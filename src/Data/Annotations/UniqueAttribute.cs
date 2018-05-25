﻿using DevZest.Data.Annotations.Primitives;
using System;

namespace DevZest.Data.Annotations
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class UniqueAttribute : ValidationColumnAttribute
    {
        protected override void Initialize(Column column)
        {
            base.Initialize(column);
            column.ParentModel.DbUnique(Name, Description, IsCluster, SortDirection == SortDirection.Descending ? column.Desc() : column.Asc());
        }

        public string Name { get; set; }

        public string Description { get; set; }

        public bool IsCluster { get; set; }

        public SortDirection SortDirection { get; set; }

        protected override bool IsValid(Column column, DataRow dataRow)
        {
            var dataSet = column.ParentModel.DataSet;
            foreach (var other in dataSet)
            {
                if (other == dataRow)
                    continue;
                if (column.Compare(dataRow, other, SortDirection) == 0)
                    return false;
            }
            return true;
        }

        protected override string DefaultMessageString
        {
            get { return UserMessages.UniqueAttribute; }
        }

        protected override bool CoerceDeclaringTypeOnly(bool value)
        {
            return true;
        }
    }
}