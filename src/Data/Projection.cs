﻿using DevZest.Data.Annotations.Primitives;
using DevZest.Data.Primitives;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq.Expressions;

namespace DevZest.Data
{
    public abstract class Projection : ModelMember, IEntity
    {
        internal void Construct(Model model, Type declaringType, string name)
        {
            Debug.Assert(model != null);
            ConstructModelMember(model, declaringType, name);
            Mount();
        }

        internal override bool IsLocal
        {
            get { return string.IsNullOrEmpty(Name); }
        }

        public Model GetModel()
        {
            EnsureConstructed();
            return ParentModel;
        }

        public string GetName()
        {
            return Name;
        }

        private sealed class ContainerModel : Model
        {
            public ContainerModel(Projection projection)
            {
                Debug.Assert(projection != null);
                Debug.Assert(projection.ParentModel == null);
                projection.Construct(this, GetType(), string.Empty);
                Add(projection);
            }

            internal override bool IsProjectionContainer
            {
                get { return true; }
            }
        }

        internal virtual void EnsureConstructed()
        {
            if (ParentModel == null)
            {
                var containerModel = new ContainerModel(this);
                Debug.Assert(ParentModel == containerModel);
            }
        }

        private static MounterManager<Projection, Column> s_columnManager = new MounterManager<Projection, Column>();

        [PropertyRegistration]
        protected static void Register<TColumnGroup, TColumn>(Expression<Func<TColumnGroup, TColumn>> getter, Mounter<TColumn> fromMounter)
            where TColumnGroup : Projection
            where TColumn : Column, new()
        {
            var initializer = getter.Verify(nameof(getter));
            fromMounter.VerifyNotNull(nameof(fromMounter));

            var result = s_columnManager.Register(getter, mounter => CreateColumn(mounter, initializer));
            result.OriginalDeclaringType = fromMounter.OriginalDeclaringType;
            result.OriginalName = fromMounter.OriginalName;
        }

        private static TColumn CreateColumn<TColumnGroup, TColumn>(Mounter<TColumnGroup, TColumn> mounter, Action<TColumn> initializer)
            where TColumnGroup : Projection
            where TColumn : Column, new()
        {
            var result = Column.Create<TColumn>(mounter.OriginalDeclaringType, mounter.OriginalName);
            var parent = mounter.Parent;
            result.Construct(parent.ParentModel, mounter.DeclaringType, parent.GetColumnName(mounter), ColumnKind.ProjectionMember, null, initializer);
            parent.Add(result);
            return result;
        }

        private sealed class ColumnCollection : KeyedCollection<string, Column>, IReadOnlyDictionary<string, Column>
        {
            public IEnumerable<string> Keys
            {
                get
                {
                    foreach (var column in this)
                        yield return column.RelativeName;
                }
            }

            public IEnumerable<Column> Values
            {
                get { return this; }
            }

            public bool ContainsKey(string key)
            {
                return Contains(key);
            }

            public bool TryGetValue(string key, out Column value)
            {
                if (Contains(key))
                {
                    value = this[key];
                    return true;
                }
                else
                {
                    value = null;
                    return false;
                }
            }

            protected override string GetKeyForItem(Column item)
            {
                return item.RelativeName;
            }

            IEnumerator<KeyValuePair<string, Column>> IEnumerable<KeyValuePair<string, Column>>.GetEnumerator()
            {
                foreach (var column in this)
                    yield return new KeyValuePair<string, Column>(column.RelativeName, column);
            }
        }

        private ColumnCollection _columns;
        public IReadOnlyList<Column> Columns
        {
            get
            {
                if (_columns == null)
                    return Array.Empty<Column>();
                else
                    return _columns;
            }
        }

        public IReadOnlyDictionary<string, Column> ColumnsByRelativeName
        {
            get
            {
                if (_columns == null)
                    return EmptyDictionary<string, Column>.Singleton;
                else
                    return _columns;
            }
        }

        Model IEntity.Model => GetModel();

        private void Add(Column column)
        {
            if (_columns == null)
                _columns = new ColumnCollection();
            _columns.Add(column);
        }

        private void Mount()
        {
            s_columnManager.Mount(this);
        }

        private string GetColumnName<T>(Mounter<T> mounter)
        {
            return IsLocal ? mounter.Name : Name + "." + mounter.Name;
        }
    }
}
