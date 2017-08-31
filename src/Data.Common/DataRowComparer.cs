﻿using DevZest.Data.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace DevZest.Data
{
    public static class DataRowComparer
    {
        internal static IColumnComparer Create<T>(Column<T> column, SortDirection direction, IComparer<T> comparer)
        {
            return ComparerBase.Create(column, direction, comparer);
        }

        private sealed class CompositeComparer : IComparer<DataRow>
        {
            public CompositeComparer(IComparer<DataRow> comparer1, IComparer<DataRow> comparer2)
            {
                _comparer1 = comparer1;
                _comparer2 = comparer2;
            }

            private readonly IComparer<DataRow> _comparer1;
            private readonly IComparer<DataRow> _comparer2;

            public int Compare(DataRow x, DataRow y)
            {
                var result = _comparer1.Compare(x, y);
                return result != 0 ? result : _comparer2.Compare(x, y);
            }
        }

        public static IComparer<DataRow> ThenBy(this IComparer<DataRow> orderBy, IComparer<DataRow> thenBy)
        {
            Check.NotNull(orderBy, nameof(orderBy));
            Check.NotNull(thenBy, nameof(thenBy));
            return new CompositeComparer(orderBy, thenBy);
        }

        public static IDataRowComparer ThenBy(this IDataRowComparer orderBy, IDataRowComparer thenBy)
        {
            Check.NotNull(orderBy, nameof(orderBy));
            Check.NotNull(thenBy, nameof(thenBy));
            if (orderBy.ModelType != thenBy.ModelType)
                throw new ArgumentException(Strings.DataRowComparer_DifferentDataRowModel, nameof(thenBy));
            return ComparerBase.Create(orderBy, thenBy);
        }

        public static IDataRowComparer ThenBy(this IDataRowComparer orderBy, Column column, SortDirection direction = SortDirection.Ascending)
        {
            Check.NotNull(orderBy, nameof(orderBy));
            var thenBy = DataRow.OrderBy(column, direction);
            if (orderBy.ModelType != thenBy.ModelType)
                throw new ArgumentException(Strings.DataRowComparer_DifferentDataRowModel, nameof(column));
            return ComparerBase.Create(orderBy, thenBy);
        }

        public static IDataRowComparer ThenBy<T>(this IDataRowComparer orderBy, Column<T> column, SortDirection direction = SortDirection.Ascending, IComparer<T> comparer = null)
        {
            Check.NotNull(orderBy, nameof(orderBy));
            var thenBy = DataRow.OrderBy(column, direction, comparer);
            if (orderBy.ModelType != thenBy.ModelType)
                throw new ArgumentException(Strings.DataRowComparer_DifferentDataRowModel, nameof(column));
            return ComparerBase.Create(orderBy, thenBy);
        }

        private abstract class ComparerBase : IDataRowComparer
        {
            public static IDataRowComparer Create(IDataRowComparer comparer1, IDataRowComparer comparer2)
            {
                return new CompositeComparer(comparer1, comparer2);
            }

            public static IColumnComparer Create<T>(Column<T> column, SortDirection direction, IComparer<T> comparer)
            {
                if (column.ParentModel != null)
                {
                    if (column.IsLocal)
                        return new LocalColumnComparer<T>(column, direction, comparer);
                    else
                        return new SimpleColumnComparer<T>(column, direction, comparer);
                }
                else
                    return new ExpressionColumnComparer<T>(column, direction, comparer);

            }

            public abstract int Compare(DataRow x, DataRow y);

            public abstract Type ModelType { get; }

            protected Model Verify(DataRow x, DataRow y)
            {
                Check.NotNull(x, nameof(x));
                Check.NotNull(y, nameof(y));
                var model = x.Model;
                if (model == null || model.GetType() != ModelType)
                    throw new ArgumentException(Strings.DataRowComparer_InvalidDataRowModel, nameof(x));
                if (y.Model != model)
                    throw new ArgumentException(Strings.DataRowComparer_DifferentDataRowModel, nameof(y));
                return model;
            }

            private sealed class CompositeComparer : ComparerBase
            {
                public CompositeComparer(IDataRowComparer comparer1, IDataRowComparer comparer2)
                {
                    Debug.Assert(comparer1 != null);
                    Debug.Assert(comparer2 != null);
                    Debug.Assert(comparer1.ModelType == comparer2.ModelType);
                    _comparer1 = comparer1;
                    _comparer2 = comparer2;
                }

                private readonly IDataRowComparer _comparer1;
                private readonly IDataRowComparer _comparer2;

                public override int Compare(DataRow x, DataRow y)
                {
                    Verify(x, y);
                    var result = _comparer1.Compare(x, y);
                    return result != 0 ? result : _comparer2.Compare(x, y);
                }

                public override Type ModelType
                {
                    get { return _comparer1.ModelType; }
                }
            }

            private abstract class ColumnComparer<T> : ComparerBase, IColumnComparer
            {
                protected ColumnComparer(Column<T> column, SortDirection direction, IComparer<T> comparer)
                {
                    Debug.Assert(column.ScalarSourceModels.Count == 1);
                    _modelType = ((Model)column.ScalarSourceModels).GetType();
                    _direction = direction;
                    _comparer = comparer;
                }

                private readonly Type _modelType;
                public sealed override Type ModelType
                {
                    get { return _modelType; }
                }

                private readonly SortDirection _direction;
                public SortDirection Direction
                {
                    get { return _direction; }
                }

                private readonly IComparer<T> _comparer;
                public Column GetColumn(Model model)
                {
                    Check.NotNull(model, nameof(model));
                    if (model.GetType() != ModelType)
                        throw new ArgumentException(Strings.DataRowComparer_DifferentModelType, nameof(model));
                    return GetTypedColumn(model);
                }

                protected abstract Column<T> GetTypedColumn(Model model);

                public sealed override int Compare(DataRow x, DataRow y)
                {
                    var model = Verify(x, y);
                    var result = GetTypedColumn(model).Compare(x, y, Direction, _comparer);
                    return result;
                }
            }

            private abstract class MemberColumnComparer<T> : ColumnComparer<T>
            {
                protected MemberColumnComparer(Column<T> column, SortDirection direction, IComparer<T> comparer)
                    : base(column, direction, comparer)
                {
                    _ordinal = column.Ordinal;
                }

                private readonly int _ordinal;

                protected abstract IReadOnlyList<Column> GetColumnList(Model model);

                protected sealed override Column<T> GetTypedColumn(Model model)
                {
                    return (Column<T>)GetColumnList(model)[_ordinal];
                }
            }

            private sealed class SimpleColumnComparer<T> : MemberColumnComparer<T>
            {
                public SimpleColumnComparer(Column<T> column, SortDirection direction, IComparer<T> comparer)
                    : base(column, direction, comparer)
                {
                }

                protected override IReadOnlyList<Column> GetColumnList(Model model)
                {
                    return model.Columns;
                }
            }

            private sealed class LocalColumnComparer<T> : MemberColumnComparer<T>
            {
                public LocalColumnComparer(Column<T> column, SortDirection direction, IComparer<T> comparer)
                    : base(column, direction, comparer)
                {
                }

                protected override IReadOnlyList<Column> GetColumnList(Model model)
                {
                    return model.LocalColumns;
                }
            }

            private sealed class ExpressionColumnComparer<T> : ColumnComparer<T>
            {
                public ExpressionColumnComparer(Column<T> column, SortDirection direction, IComparer<T> comparer)
                    : base(column, direction, comparer)
                {
                    _json = column.ToJson(false);
                }

                private readonly string _json;
                private readonly ConditionalWeakTable<Model, Column<T>> _columnsByModel = new ConditionalWeakTable<Model, Column<T>>();

                protected override Column<T> GetTypedColumn(Model model)
                {
                    return _columnsByModel.GetValue(model, CreateColumn);
                }

                private Column<T> CreateColumn(Model model)
                {
                    return Column.ParseJson<Column<T>>(model, _json);
                }
            }
        }
    }
}