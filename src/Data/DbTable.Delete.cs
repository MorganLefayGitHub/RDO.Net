﻿using DevZest.Data.Primitives;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DevZest.Data
{
    partial class DbTable<T>
    {
        public Task<int> DeleteAsync(CancellationToken ct = default(CancellationToken))
        {
            return DeleteAsync(null, ct);
        }

        public Task<int> DeleteAsync(Func<T, _Boolean> where, CancellationToken ct = default(CancellationToken))
        {
            VerifyDeletable();
            return DbTableDelete<T>.ExecuteAsync(this, where, ct);
        }

        internal DbSelectStatement BuildDeleteStatement(Func<T, _Boolean> where)
        {
            var whereExpr = VerifyWhere(where);
            return new DbSelectStatement(Model, null, null, whereExpr, null, -1, -1);
        }

        public Task<int> DeleteAsync<TSource>(DbSet<TSource> source, CancellationToken ct = default(CancellationToken))
            where TSource : class, T, new()
        {
            return DeleteAsync(source, KeyMapping.Match, ct);
        }

        public Task<int> DeleteAsync<TSource>(DbSet<TSource> source, Func<TSource, T, KeyMapping> keyMapper, CancellationToken ct = default(CancellationToken))
            where TSource : class, IEntity, new()
        {
            VerifyDeletable();
            Verify(source, nameof(source));
            var columnMappings = Verify(keyMapper, nameof(keyMapper), source._).GetColumnMappings();
            return DbTableDelete<T>.ExecuteAsync(this, source, columnMappings, ct);
        }

        public Task<int> DeleteAsync<TSource>(DataSet<TSource> source, int rowIndex, CancellationToken ct = default(CancellationToken))
            where TSource : class, T, new()
        {
            return DeleteAsync(source, rowIndex, KeyMapping.Match, ct);
        }

        public Task<int> DeleteAsync<TSource>(DataSet<TSource> source, int rowIndex, Func<TSource, T, KeyMapping> keyMapper, CancellationToken ct = default(CancellationToken))
            where TSource : class, IEntity, new()
        {
            VerifyDeletable();
            Verify(source, nameof(source), rowIndex, nameof(rowIndex));
            var columnMappings = Verify(keyMapper, nameof(keyMapper), source._).GetColumnMappings();
            return DbTableDelete<T>.ExecuteAsync(this, source, rowIndex, columnMappings, ct);
        }

        public Task<int> DeleteAsync<TSource>(DataSet<TSource> source, CancellationToken ct)
            where TSource : class, T, new()
        {
            return DeleteAsync(source, KeyMapping.Match, ct);
        }

        public Task<int> DeleteAsync<TSource>(DataSet<TSource> source, Func<TSource, T, KeyMapping> keyMapper, CancellationToken ct = default(CancellationToken))
            where TSource : class, IEntity, new()
        {
            Verify(source, nameof(source));
            if (source.Count == 1)
                return DeleteAsync(source, 0, keyMapper, ct);

            VerifyDeletable();
            var keyMappingTarget = Verify(keyMapper, nameof(keyMapper), source._).TargetKey;
            return DbTableDelete<T>.ExecuteAsync(this, source, keyMappingTarget, ct);
        }

        internal DbSelectStatement BuildDeleteScalarStatement<TLookup>(DataSet<TLookup> source, int ordinal, IReadOnlyList<ColumnMapping> join)
            where TLookup : class, IEntity, new()
        {
            Debug.Assert(source != null && source._ != null);
            return BuildDeleteScalarStatement(source[ordinal], join);
        }

        internal DbSelectStatement BuildDeleteScalarStatement(DataRow dataRow, IReadOnlyList<ColumnMapping> join)
        {
            var paramManager = new ScalarParamManager(dataRow);
            var from = new DbJoinClause(DbJoinKind.InnerJoin, GetScalarDataSource(paramManager, join), FromClause, join);
            return new DbSelectStatement(Model, null, from, null, null, -1, -1);
        }

        internal DbSelectStatement BuildDeleteStatement<TLookup>(DbSet<TLookup> source, IReadOnlyList<ColumnMapping> join)
            where TLookup : class, IEntity, new()
        {
            Debug.Assert(source != null);
            Debug.Assert(join != null);
            return source.QueryStatement.BuildDeleteStatement(Model, join);
        }

        private void VerifyDeletable()
        {
            if (Model.ChildModels.Any(x => x != null))
                throw new NotSupportedException(DiagnosticMessages.DbTable_DeleteNotSupportedForParentTable);
        }
    }
}
