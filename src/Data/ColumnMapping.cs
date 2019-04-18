﻿using DevZest.Data.Primitives;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;

namespace DevZest.Data
{
    /// <summary>
    /// Defines the mapping between source column or expression and target column.
    /// </summary>
    public struct ColumnMapping
    {
        public static ColumnMapping Map<T>(Column<T> source, Column<T> target)
        {
            source.VerifyNotNull(nameof(source));
            target.VerifyNotNull(nameof(target));
            return new ColumnMapping(source, target);
        }

        public static ColumnMapping UnsafeMap(Column source, Column target)
        {
            target.VerifyNotNull(nameof(target));
            return new ColumnMapping(source, target);
        }

        public static IReadOnlyList<ColumnMapping> Map<TSource, TTarget>(TSource source, TTarget target, Action<ColumnMapper, TSource, TTarget> columnMapper, bool isInsertable)
            where TSource : class, IEntity, new()
            where TTarget : class, IEntity, new()
        {
            target.VerifyNotNull(nameof(target));
            source.VerifyNotNull(nameof(source));

            if (columnMapper == null)
                return GetColumnMappings(source.Model, target.Model, isInsertable);

            var result = new ColumnMapper(source.Model, target.Model).Build(builder => columnMapper(builder, source, target));
            var columns = isInsertable ? target.Model.GetInsertableColumns() : target.Model.GetUpdatableColumns();
            var targetModelIds = new HashSet<ColumnId>(columns.Select(x => x.Id));
            foreach (var resultItem in result)
            {
                if (!targetModelIds.Contains(resultItem.Target.Id))
                    throw new InvalidOperationException(DiagnosticMessages.ColumnMapper_InvalidTarget(resultItem.Target));
            }

            return result;
        }

        private static List<ColumnMapping> GetColumnMappings(Model sourceModel, Model targetModel, bool isInsertable)
        {
            targetModel.VerifyNotNull(nameof(targetModel));
            sourceModel.VerifyNotNull(nameof(sourceModel));

            var result = new List<ColumnMapping>();
            var sourceColumns = sourceModel.Columns;
            var targetColumns = isInsertable ? targetModel.GetInsertableColumns() : targetModel.GetUpdatableColumns();
            foreach (var targetColumn in targetColumns)
            {
                if (targetColumn.IsSystem)
                    continue;
                var sourceColumn = sourceColumns.AutoSelect(targetColumn);
                if (sourceColumn != null)
                    result.Add(new ColumnMapping(sourceColumn, targetColumn));
            }

            return result;
        }

        internal ColumnMapping(Column source, Column target)
        {
            if (source == null)
                _source = DbConstantExpression.Null;
            else
                _source = source;

            Debug.Assert(target != null);
            _target = target;
        }

        internal ColumnMapping(DbExpression source, Column target)
        {
            Debug.Assert(source != null);
            Debug.Assert(target != null);
            _source = source;
            _target = target;
        }

        private readonly object _source;
        private Column _target;

        /// <summary>Gets the source <see cref="Column"/> of this mapping.</summary>
        /// <value>Returns <see langword="null"/> if source is expression.</value>
        public Column Source
        {
            get
            {
                var source = _source as Column;
                return source ?? CalculatedSource;
            }
        }

        private Column CalculatedSource
        {
            get
            {
                var columnExpression = SourceExpression as DbColumnExpression;
                return columnExpression == null ? null : columnExpression.Column;
            }
        }

        /// <summary>Gets the source <see cref="DbExpression"/> of this mapping.</summary>
        public DbExpression SourceExpression
        {
            get
            {
                var sourceExpression = _source as DbExpression;
                return sourceExpression ?? Source.DbExpression;
            }
        }

        private DbExpression CalculatedSourceExpression
        {
            get
            {
                var sourceColumn = Source;
                return sourceColumn == null ? null : sourceColumn.DbExpression;
            }
        }

        /// <summary>Gets the target <see cref="Column"/> of this mapping.</summary>
        public Column Target
        {
            get { return _target; }
        }

        /// <summary>Gets the target <see cref="DbColumnExpression"/> of this mapping.</summary>
        public DbColumnExpression TargetExpression
        {
            get { return _target.DbExpression as DbColumnExpression; }
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "({0}, {1})", _source, _target);
        }

        public void CopyValue(DataRow sourceDataRow, DataRow targetDataRow)
        {
            Source.CopyValue(sourceDataRow, Target, targetDataRow);
        }
    }
}
