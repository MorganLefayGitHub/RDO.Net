﻿using DevZest.Data.Annotations;
using DevZest.Data.Annotations.Primitives;
using DevZest.Data.Primitives;
using DevZest.Data.Utilities;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;

namespace DevZest.Data
{
    public abstract partial class Model : ModelMember, IModels, IModelReference
    {
        #region RegisterColumn

        static MounterManager<Model, Column> s_columnManager = new MounterManager<Model, Column>();

        /// <summary>
        /// Registers a new column which has a default constructor.
        /// </summary>
        /// <typeparam name="TModel">The type of model which the column is registered on.</typeparam>
        /// <typeparam name="TColumn">The type of the column.</typeparam>
        /// <param name="getter">The lambda expression of the column getter.</param>
        /// <returns>Mounter of the column.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="getter"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="getter"/> expression is not a valid getter.</exception>
        protected static Mounter<TColumn> RegisterColumn<TModel, TColumn>(Expression<Func<TModel, TColumn>> getter)
            where TModel : Model
            where TColumn : Column, new()
        {
            var initializer = getter.Verify(nameof(getter));

            return s_columnManager.Register(getter, mounter => CreateColumn(mounter, initializer));
        }

        /// <summary>
        /// Registers a column from existing column property, without inheriting its <see cref="ColumnId"/> value.
        /// </summary>
        /// <typeparam name="TModel">The type of model which the column is registered on.</typeparam>
        /// <typeparam name="TColumn">The type of the column.</typeparam>
        /// <param name="getter">The lambda expression of the column getter.</param>
        /// <param name="fromMounter">The existing column mounter.</param>
        /// <returns>Mounter of the column.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="getter"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="getter"/> expression is not an valid getter.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="fromMounter"/> is null.</exception>
        protected static void RegisterColumn<TModel, TColumn>(Expression<Func<TModel, TColumn>> getter, Mounter<TColumn> fromMounter)
            where TModel : Model
            where TColumn : Column, new()
        {
            var initializer = getter.Verify(nameof(getter));
            Utilities.Check.NotNull(fromMounter, nameof(fromMounter));

            var result = s_columnManager.Register(getter, mounter => CreateColumn(mounter, fromMounter, initializer));
            result.OriginalDeclaringType = fromMounter.OriginalDeclaringType;
            result.OriginalName = fromMounter.OriginalName;
        }

        private static T CreateColumn<TModel, T>(Mounter<TModel, T> mounter, Action<T> initializer)
            where TModel : Model
            where T : Column, new()
        {
            var result = Column.Create<T>(mounter.DeclaringType, mounter.Name);
            result.Construct(mounter.Parent, mounter.DeclaringType, mounter.Name, ColumnKind.ModelProperty, null, initializer);
            return result;
        }

        private static T CreateColumn<TModel, T>(Mounter<TModel, T> mounter, Mounter<T> fromMounter, Action<T> initializer)
            where TModel : Model
            where T : Column, new()
        {
            var result = Column.Create<T>(fromMounter.OriginalDeclaringType, fromMounter.OriginalName);
            result.Construct(mounter.Parent, mounter.DeclaringType, mounter.Name, ColumnKind.ModelProperty, fromMounter.Initializer, initializer);
            return result;
        }

        #endregion

        #region RegisterColumnList

        static MounterManager<Model, ColumnList> s_columnListManager = new MounterManager<Model, ColumnList>();

        /// <summary>
        /// Registers a column list.
        /// </summary>
        /// <typeparam name="TModel">The type of model which the column is registered on.</typeparam>
        /// <typeparam name="T">The type of the column contained by the column list.</typeparam>
        /// <param name="getter">The lambda expression of the column list getter.</param>
        /// <returns>Mounter of the column list.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="getter"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="getter"/> expression is not a valid getter.</exception>
        protected static Mounter<ColumnList<T>> RegisterColumnList<TModel, T>(Expression<Func<TModel, ColumnList<T>>> getter)
            where TModel : Model
            where T : Column
        {
            Utilities.Check.NotNull(getter, nameof(getter));

            return s_columnListManager.Register(getter, a => CreateColumnList(a), null);
        }

        private static ColumnList<T> CreateColumnList<TModel, T>(Mounter<TModel, ColumnList<T>> mounter)
            where TModel : Model
            where T : Column
        {
            var result = new ColumnList<T>();
            var parent = mounter.Parent;
            result.ConstructModelMember(parent, mounter.DeclaringType, mounter.Name);
            parent.Add(result);
            return result;
        }

        private List<ColumnList> _columnLists;
        internal IReadOnlyList<ColumnList> ColumnLists
        {
            get
            {
                if (_columnLists == null)
                    return Array<ColumnList>.Empty;
                else
                    return _columnLists;
            }
        }

        private void Add(ColumnList columnList)
        {
            if (_columnLists == null)
                _columnLists = new List<Data.ColumnList>();
            _columnLists.Add(columnList);
        }

        #endregion

        #region RegisterChildModel

        static MounterManager<Model, Model> s_childModelManager = new MounterManager<Model, Model>();

        protected static Mounter<TChildModel> RegisterChildModel<TModel, TChildModel>(Expression<Func<TModel, TChildModel>> getter, Func<TModel, TChildModel> constructor = null)
            where TModel : Model, new()
            where TChildModel : Model, new()
        {
            Utilities.Check.NotNull(getter, nameof(getter));
            if (constructor == null)
                constructor = _ => new TChildModel();
            return s_childModelManager.Register(getter, mounter => CreateChildModel(mounter, constructor), null);
        }

        private static TChildModel CreateChildModel<TModel, TChildModel>(Mounter<TModel, TChildModel> mounter, Func<TModel, TChildModel> constructor)
            where TModel : Model, new()
            where TChildModel : Model, new()
        {
            var parentModel = mounter.Parent;
            TChildModel result = constructor(parentModel);
            result.Construct(parentModel, mounter.DeclaringType, mounter.Name);
            return result;
        }

        internal void Construct(Model parentModel, Type declaringType, string name)
        {
            Construct(parentModel, declaringType, name, Array<ColumnMapping>.Empty, Array<ColumnMapping>.Empty);
        }

        /// <summary>
        /// Registers a child model.
        /// </summary>
        /// <typeparam name="TModel">The type of model which the child model is registered on.</typeparam>
        /// <typeparam name="TChildModel">The type of the child model.</typeparam>
        /// <param name="getter">The lambda expression of the child model getter.</param>
        /// <param name="relationshipGetter">Gets relationship between child model and parent model.</param>
        /// <returns>Mounter of the child model.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="getter"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="getter"/> expression is not a valid getter.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="relationshipGetter"/> is <see langword="null"/>.</exception>
        protected static Mounter<TChildModel> RegisterChildModel<TModel, TModelKey, TChildModel>(Expression<Func<TModel, TChildModel>> getter,
            Func<TChildModel, TModelKey> relationshipGetter, Func<TModel, TChildModel> constructor = null)
            where TModel : Model<TModelKey>
            where TModelKey : PrimaryKey
            where TChildModel : Model, new()
        {
            Utilities.Check.NotNull(getter, nameof(getter));
            Utilities.Check.NotNull(relationshipGetter, nameof(relationshipGetter));
            if (constructor == null)
                constructor = _ => new TChildModel();
            return s_childModelManager.Register(getter, a => CreateChildModel<TModel, TModelKey, TChildModel>(a, relationshipGetter, constructor), null);
        }

        private static TChildModel CreateChildModel<TModel, TModelKey, TChildModel>(Mounter<TModel, TChildModel> mounter,
            Func<TChildModel, TModelKey> relationshipGetter, Func<TModel, TChildModel> constructor)
            where TModel : Model<TModelKey>
            where TModelKey : PrimaryKey
            where TChildModel : Model, new()
        {
            var parentModel = mounter.Parent;
            TChildModel result = constructor(parentModel);
            var parentRelationship = relationshipGetter(result).Join(parentModel.PrimaryKey);
            var parentMappings = AppendColumnMappings(parentRelationship, null, result, parentModel);
            result.Construct(parentModel, mounter.DeclaringType, mounter.Name, parentRelationship, parentMappings);
            return result;
        }

        private static IReadOnlyList<ColumnMapping> AppendColumnMappings<TChildModel, TParentModel>(IReadOnlyList<ColumnMapping> parentRelationship,
            Action<ColumnMapper, TChildModel, TParentModel> parentMappingsBuilderAction, TChildModel childModel, TParentModel parentModel)
            where TChildModel : Model
            where TParentModel : Model
        {
            if (parentMappingsBuilderAction == null)
                return parentRelationship;

            var parentMappingsBuilder = new ColumnMapper(childModel, parentModel);
            var parentMappings = parentMappingsBuilder.Build(x => parentMappingsBuilderAction(x, childModel, parentModel));

            var result = new ColumnMapping[parentRelationship.Count + parentMappings.Count];
            for (int i = 0; i < parentRelationship.Count; i++)
                result[i] = parentRelationship[i];
            for (int i = 0; i < parentMappings.Count; i++)
                result[i + parentRelationship.Count] = parentMappings[i];
            return result;
        }

        private void Construct(Model parentModel, Type declaringType, string name, IReadOnlyList<ColumnMapping> parentRelationship, IReadOnlyList<ColumnMapping> parentMappings)
        {
            this.ConstructModelMember(parentModel, declaringType, name);

            Debug.Assert(parentMappings != null);
            Debug.Assert(parentRelationship != null);
            ParentRelationship = parentRelationship;
            ParentMappings = parentMappings;
            InitChildColumns();
        }

        private void InitChildColumns()
        {
            var parentMappings = ParentMappings;
            for (int i = 0; i < parentMappings.Count; i++)
            {
                var mapping = parentMappings[i];
                mapping.Source.InitAsChild(mapping.Target);
            }
        }

        /// <summary>
        /// Gets the column mappings between its parent model and this model.
        /// </summary>
        internal IReadOnlyList<ColumnMapping> ParentRelationship { get; private set; }

        internal IReadOnlyList<ColumnMapping> ParentMappings { get; private set; }

        #endregion

        #region RegisterLocalColumn

        static MounterManager<Model, Column> s_localColumnManager = new MounterManager<Model, Column>();

        /// <summary>
        /// Registers a new local column.
        /// </summary>
        /// <typeparam name="TModel">The type of model which the column is registered on.</typeparam>
        /// <typeparam name="TColumn">The type of the column.</typeparam>
        /// <param name="getter">The lambda expression of the column getter.</param>
        /// <param name="constructor">The constructor of the computed local column. If null, a normal local column will be created.</param>
        /// <exception cref="ArgumentNullException"><paramref name="getter"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="getter"/> expression is not a valid getter.</exception>
        protected static void RegisterLocalColumn<TModel, T>(Expression<Func<TModel, Column<T>>> getter, Func<TModel, Column<T>> constructor = null)
            where TModel : Model
        {
            var initializer = getter.Verify(nameof(getter));

            if (constructor == null)
                constructor = _ => _.CreateLocalColumn<T>();
            s_localColumnManager.Register(getter, mounter => CreateLocalColumn(mounter, constructor, initializer));
        }

        private static Column<T> CreateLocalColumn<TModel, T>(Mounter<TModel, Column<T>> mounter, Func<TModel, Column<T>> constructor, Action<Column<T>> initializer)
            where TModel : Model
        {
            var result = constructor(mounter.Parent);
            initializer(result);
            return result;
        }

        #endregion

        protected Model()
        {
            Columns = new ColumnCollection(this);
            ChildModels = new ModelCollection(this);

            s_columnManager.Mount(this);
            s_columnListManager.Mount(this);
            PerformConstructing();
        }

        internal override void ConstructModelMember(Model parentModel, Type declaringType, string name)
        {
            base.ConstructModelMember(parentModel, declaringType, name);
            ParentModel.ChildModels.Add(this);
            Depth = ParentModel.Depth + 1;
            _rootModel = ParentModel.RootModel;
        }

        internal int Depth { get; private set; }

        private Model _rootModel;
        internal Model RootModel
        {
            get { return _rootModel ?? this; }
        }

        /// <summary>
        /// Gets a value indicates whether child models are initialized.
        /// </summary>
        /// <remarks>Unlike <see cref="Column"/> and <see cref="ColumnList"/>,
        /// child models are not initialized by default. This design decision is to deal with the situation when recursive child models registered.
        /// <see cref="EnsureInitialized"/> will be called automatically when creating the first <see cref="DataRow"/> the query builder.
        /// </remarks>
        public bool IsInitialized { get; private set; }

        /// <summary>
        /// Ensures child models are initialized.
        /// </summary>
        /// <remarks>Unlike <see cref="Column"/> and <see cref="ColumnList"/>,
        /// child models are not initialized by default. This design decision is to deal with the situation when recursive child models registered.
        /// <see cref="EnsureInitialized"/> will be called automatically when creating the first <see cref="DataRow"/>.
        /// </remarks>
        protected internal void EnsureInitialized()
        {
            if (IsInitialized)
                return;
            EnsureInitialized(true);
        }

#if DEBUG
        internal
#else
        private
#endif
        void EnsureInitialized(bool verifyDataSource)
        {
            if (verifyDataSource &&  DataSource == null)
                throw new InvalidOperationException(DiagnosticMessages.Model_EnsureInitializedNullDataSource);

            s_childModelManager.Mount(this);
            PerformChildModelsMounted();
            if (DataSource != null && DataSource.Kind == DataSourceKind.DataSet)
            {
                foreach (var model in ChildModels)
                {
                    var modelType = model.GetType();
                    var invoker = s_createDataSetInvokers.GetOrAdd(modelType, t => BuildCreateDataSetInvoker(t));
                    invoker(model);
                }
                PerformChildDataSetsCreated();
                DataSetContainer.MergeComputations(this);
                foreach (var column in Columns)
                    column.InitValueManager();
            }

            IsInitialized = true;
            PerformInitialized();
        }

        private void PerformConstructing()
        {
            ModelWireupAttribute.WireupAttributes(this, ModelWireupEvent.Constructing);
            OnConstructing();
        }

        protected virtual void OnConstructing()
        {
            Constructing(this, EventArgs.Empty);
        }

        private void PerformInitializing()
        {
            ModelWireupAttribute.WireupAttributes(this, ModelWireupEvent.Initializing);
            s_localColumnManager.Mount(this);
            OnInitializing();
        }

        protected virtual void OnInitializing()
        {
            Initializing(this, EventArgs.Empty);
        }

        private void PerformChildModelsMounted()
        {
            ModelWireupAttribute.WireupAttributes(this, ModelWireupEvent.ChildModelsMounted);
            OnChildModelsMounted();
        }

        protected virtual void OnChildModelsMounted()
        {
            ChildModelsMounted(this, EventArgs.Empty);
        }

        private void PerformChildDataSetsCreated()
        {
            ModelWireupAttribute.WireupAttributes(this, ModelWireupEvent.ChildDataSetsCreated);
            OnChildDataSetsCreated();
        }

        protected virtual void OnChildDataSetsCreated()
        {
            ChildDataSetsCreated(this, EventArgs.Empty);
        }

        private void PerformInitialized()
        {
            ModelWireupAttribute.WireupAttributes(this, ModelWireupEvent.Initialized);
            var localColumns = LocalColumns;
            for (int i = 0; i < localColumns.Count; i++)
                localColumns[i].SealLocalColumn();
            OnInitialized();
        }

        protected virtual void OnInitialized()
        {
            Initialized(this, EventArgs.Empty);
        }

        private static ConcurrentDictionary<Type, Action<Model>> s_createDataSetInvokers = new ConcurrentDictionary<Type, Action<Model>>();

        private static DataSet<T> _CreateDataSet<T>(Model model)
            where T : Model, new()
        {
            return DataSet<T>.Create((T)model);
        }

        private static Action<Model> BuildCreateDataSetInvoker(Type modelType)
        {
            var methodInfo = typeof(Model).GetStaticMethodInfo(nameof(_CreateDataSet));
            methodInfo = methodInfo.MakeGenericMethod(modelType);
            var paramModel = Expression.Parameter(typeof(Model), methodInfo.GetParameters()[0].Name);
            var call = Expression.Call(methodInfo, paramModel);
            return Expression.Lambda<Action<Model>>(call, paramModel).Compile();
        }

        protected internal ColumnCollection Columns { get; private set; }

        protected internal IReadOnlyList<Column> LocalColumns
        {
            get
            {
                if (LocalColumnList == null)
                    return Array<Column>.Empty;
                else
                    return LocalColumnList;
            }
        }

        internal List<Column> LocalColumnList { get; set; }

        protected internal ModelCollection ChildModels { get; private set; }

        private List<IValidator> _validators = new List<IValidator>();
        internal List<IValidator> Validators
        {
            get { return _validators; }
        }

        protected internal virtual IDataValidationErrors Validate(DataRow dataRow)
        {
            var result = DataValidationErrors.Empty;
            foreach (var validator in Validators)
                result = Merge(result, validator.Validate(dataRow));
            return result;
        }

        private static IDataValidationErrors Merge(IDataValidationErrors result, IDataValidationErrors validationErrors)
        {
            if (validationErrors != null)
            {
                foreach (var validationMessage in validationErrors)
                    result = result.Add(validationMessage);
            }
            return result;
        }

        public PrimaryKey PrimaryKey
        {
            get { return GetPrimaryKeyCore(); }
        }

        internal virtual PrimaryKey GetPrimaryKeyCore()
        {
            return null;
        }

        protected internal DataSource DataSource { get; private set; }

        internal void SetDataSource(DataSource dataSource)
        {
            Debug.Assert(dataSource != null);
            Debug.Assert(DataSource == null);

            bool isDataSet = dataSource is DataSet;

            Columns.InitDbColumnNames();
            if (!isDataSet)
                PerformInitializing();
            DataSource = dataSource;
            DataSet = dataSource as DataSet;
            if (DataSet != null && RootModel == this)
                _dataSetContainer = new DataSetContainer();
            if (isDataSet)
                PerformInitializing();
        }

        protected internal sealed override bool DesignMode
        {
            get
            {
                if (DataSource == null)
                    return true;
                if (DataSource.Kind != DataSourceKind.DataSet)
                    return false;
                return !IsInitialized;
            }
        }

        public DataSet DataSet { get; private set; }

        private int _ordinal = -1;
        internal int Ordinal
        {
            get { return _ordinal; }
            set { _ordinal = value; }
        }

#region IModels
        [SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes", Justification = "Child types will not call this method.")]
        bool IModels.Contains(Model model)
        {
            return model == this;
        }

        [SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes", Justification = "Child types will not call this method.")]
        int IReadOnlyCollection<Model>.Count
        {
            get { return 1; }
        }

        [SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes", Justification = "Child types will not call this method.")]
        IEnumerator<Model> IEnumerable<Model>.GetEnumerator()
        {
            yield return this;
        }

        [SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes", Justification = "Child types will not call this method.")]
        IEnumerator IEnumerable.GetEnumerator()
        {
            yield return this;
        }

        [SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes", Justification = "Child types will not call this method.")]
        bool IModels.IsSealed
        {
            get { return true; }
        }

        [SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes", Justification = "Child types will not call this method.")]
        IModels IModels.Seal()
        {
            return this;
        }

        [SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes", Justification = "Child types will not call this method.")]
        IModels IModels.Add(Model value)
        {
            Utilities.Check.NotNull(value, nameof(value));
            if (value == this)
                return this;
            return Models.New(this, value);
        }

        [SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes", Justification = "Child types will not call this method.")]
        IModels IModels.Remove(Model value)
        {
            Utilities.Check.NotNull(value, nameof(value));
            if (value == this)
                return Models.Empty;
            else
                return this;
        }

        [SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes", Justification = "Child types will not call this method.")]
        IModels IModels.Clear()
        {
            return Models.Empty;
        }
#endregion

        internal Model Clone(bool setDataSource)
        {
            Model result = (Model)Activator.CreateInstance(this.GetType());
            result.InitializeClone(this, setDataSource);
            return result;
        }

        internal Action<IModelReference> Initializer { get; set; }

        internal void InitializeClone(Model prototype, bool setDataSource)
        {
            Debug.Assert(prototype != null && prototype != this);
            InitializeColumnLists(prototype);
            if (prototype.Initializer != null)
            {
                Initializer = prototype.Initializer;
                Initializer(this);
            }
            if (setDataSource && prototype.DataSource != null)
                SetDataSource(prototype.DataSource);
        }

        private void InitializeColumnLists(Model prototype)
        {
            var properties = s_columnListManager.GetAll(this.GetType());
            foreach (var property in properties)
            {
                var columnList = property.GetInstance(this);
                var sourceColumnList = property.GetInstance(prototype);
                columnList.Initialize(sourceColumnList);
            }
        }

        internal void InitializeExtraColumns(Model prototype)
        {
            var prototypeExt = prototype.ExtraColumns;
            if (prototypeExt != null && ExtraColumns == null)
                ExtraColumns = (ColumnCombination)Activator.CreateInstance(prototypeExt.GetType());
        }

        private IIndexConstraint _clusteredIndex;
        private IIndexConstraint ClusteredIndex
        {
            get { return _clusteredIndex; }
            set
            {
                if (_clusteredIndex != null && _clusteredIndex != value)
                    throw new InvalidOperationException(DiagnosticMessages.Model_MultipleClusteredIndex(_clusteredIndex.SystemName));
                else
                    _clusteredIndex = value;
            }
        }

        internal void ChangeClusteredIndexAsNonClustered()
        {
            if (_clusteredIndex == null)
                return;

            _clusteredIndex.AsNonClustered();
            _clusteredIndex = null;
        }

        internal void AddDbTableConstraint(DbTableConstraint constraint, bool overwritable)
        {
            Debug.Assert(constraint != null);

            if (!overwritable && ContainsExtension(((IExtension)constraint).Key))
                throw new InvalidOperationException(DiagnosticMessages.Model_DuplicateConstraintName(constraint.SystemName));

            var index = constraint as IIndexConstraint;
            if (index != null && index.IsClustered)
                ClusteredIndex = index;

            this.AddOrUpdateExtension(constraint);
        }

        internal void AddIndex(DbIndex index)
        {
            Debug.Assert(index != null);

            if (ContainsExtension(((IExtension)index).Key))
                throw new InvalidOperationException(DiagnosticMessages.Model_DuplicateIndexName(index.Name));

            if (index.IsClustered)
                ClusteredIndex = index;

            this.AddOrUpdateExtension(index);
        }

        internal KeyOutput CreateSequentialKey()
        {
            return new KeyOutput(this, true);
        }

        protected internal virtual string DbAlias
        {
            get { return this.GetType().Name; }
        }

        private Dictionary<string, ModelMember> _members = new Dictionary<string, ModelMember>();
        internal void AddMember(ModelMember member)
        {
            Debug.Assert(member != null);
            Debug.Assert(!string.IsNullOrEmpty(member.Name));
            _members.Add(member.Name, member);
        }

        private bool ContainsMember(string memberName)
        {
            return string.IsNullOrEmpty(memberName) ? false : _members.ContainsKey(memberName);
        }

        internal ModelMember this[string memberName]
        {
            get { return ContainsMember(memberName) ? _members[memberName] : null; }
        }

        public Identity GetIdentity(bool isTempTable)
        {
            var results = GetExtensions<Identity>();
            foreach (var result in results)
            {
                if (result.IsTempTable == isTempTable)
                    return result;
            }
            return null;
        }

        internal const string SYS_ROW_ID_COL_NAME = "sys_row_id";
        private const string SYS_PARENT_ROW_ID_COL_NAME = "sys_parent_row_id";

        internal void AddTempTableIdentity()
        {
            _Int32 identityColumn = AddSysRowIdColumn(false);
            var identity = identityColumn.Identity(1, 1, true);

            var primaryKeyConstraint = GetExtension<DbPrimaryKey>();
            if (primaryKeyConstraint == null)
                AddDbTableConstraint(new DbPrimaryKey(this, null, null, true, () => GetIdentityOrderByList(identity)), false);
            else
            {
                ChangeClusteredIndexAsNonClustered();
                AddDbTableConstraint(new DbUnique(null, null, true, GetIdentityOrderByList(identity)), false);
            }
        }

        private static IList<ColumnSort> GetIdentityOrderByList(Identity identity)
        {
            return new ColumnSort[] { new ColumnSort(identity.Column, identity.Increment > 0 ? SortDirection.Ascending : SortDirection.Descending) };
        }

        internal DbFromClause FromClause
        {
            get
            {
                var dbSet = DataSource as IDbSet;
                return dbSet == null ? null : dbSet.FromClause;
            }
        }

        private _Int32 AddSysRowIdColumn(bool isParent)
        {
            _Int32 result = new _Int32();
            result.Initialize(this, this.GetType(), isParent ? SYS_PARENT_ROW_ID_COL_NAME : SYS_ROW_ID_COL_NAME,
                isParent ? ColumnKind.SystemParentRowId : ColumnKind.SystemRowId, null);
            return result;
        }

        private class SysRowId : IExtension
        {
            public SysRowId(_Int32 column)
            {
                Column = column;
            }

            public object Key
            {
                get { return this.GetType(); }
            }

            public _Int32 Column { get; private set; }
        }

        private sealed class SysParentRowId : IExtension
        {
            public SysParentRowId(_Int32 column)
            {
                Column = column;
            }

            public object Key
            {
                get { return this.GetType(); }
            }

            public _Int32 Column { get; private set; }
        }

        internal _Int32 GetSysRowIdColumn(bool createIfNotExist)
        {
            var sysRowId = GetExtension<SysRowId>();
            if (sysRowId == null)
            {
                if (!createIfNotExist)
                    return null;

                sysRowId = new SysRowId(AddSysRowIdColumn(false));
                AddOrUpdateExtension(sysRowId);
            }
            return sysRowId.Column;
        }

        internal _Int32 GetSysParentRowIdColumn(bool createIfNotExist)
        {
            var sysParentRowId = GetExtension<SysParentRowId>();
            if (sysParentRowId == null)
            {
                if (!createIfNotExist)
                    return null;

                sysParentRowId = new SysParentRowId(AddSysRowIdColumn(true));
                AddOrUpdateExtension(sysParentRowId);
            }
            return sysParentRowId.Column;
        }

        protected internal void Index(string name, string description, bool isUnique, bool isClustered, bool isMemberOfTable, bool isMemberOfTempTable, params ColumnSort[] orderByList)
        {
            Utilities.Check.NotEmpty(name, nameof(name));
            Utilities.Check.NotNull(orderByList, nameof(orderByList));
            if (orderByList.Length == 0)
                throw new ArgumentException(DiagnosticMessages.Model_EmptyColumns, nameof(orderByList));

            for (int i = 0; i < orderByList.Length; i++)
            {
                var column = orderByList[i].Column;
                if (column == null || column.ParentModel != this)
                    throw new ArgumentException(DiagnosticMessages.Model_VerifyChildColumn, string.Format(CultureInfo.InvariantCulture, nameof(orderByList) + "[{0}]", i));
            }

            AddIndex(new DbIndex(name, description, isUnique, isClustered, isMemberOfTable, isMemberOfTempTable, orderByList));
        }

        protected internal void DbUnique(string name, string description, bool isClustered, params ColumnSort[] orderByList)
        {
            Utilities.Check.NotNull(orderByList, nameof(orderByList));
            if (orderByList.Length == 0)
                throw new ArgumentException(DiagnosticMessages.Model_EmptyColumns, nameof(orderByList));

            for (int i = 0; i < orderByList.Length; i++)
            {
                var column = orderByList[i].Column;
                if (column == null || column.ParentModel != this)
                    throw new ArgumentException(DiagnosticMessages.Model_VerifyChildColumn, string.Format(CultureInfo.InvariantCulture, nameof(orderByList) + "[{0}]", i));
            }

            AddDbTableConstraint(new DbUnique(name, description, isClustered, orderByList), false);
        }

        protected internal void DbCheck(string name, string description, _Boolean condition)
        {
            Utilities.Check.NotNull(condition, nameof(condition));

            AddDbTableConstraint(new DbCheck(name, description, condition.DbExpression), false);
        }

        private int _suspendIdentityCount;
        internal bool IsIdentitySuspended
        {
            get { return _suspendIdentityCount > 0; }
        }

        internal void SuspendIdentity()
        {
            _suspendIdentityCount++;
        }

        internal void ResumeIdentity()
        {
            _suspendIdentityCount--;
        }

        internal DbTable<KeyOutput> SequentialKeyTempTable { get; set; }

        public override string ToString()
        {
            return this.GetType().Name + ": [" + string.Join(", ", Columns.Select(x => x.Name)) + "]";
        }

        internal KeyOutput ParentSequentialKeyModel
        {
            get
            {
                var parentModel = ParentModel;
                if (parentModel == null)
                    return null;

                var parentQuery = parentModel.DataSource as IDbSet;
                if (parentQuery == null || parentQuery.Kind != DataSourceKind.DbQuery)
                    return null;

                var result = parentQuery.QueryStatement.SequentialKeyTempTable._;
                return result;
            }
        }

        internal IReadOnlyList<ColumnMapping> GetParentRelationship(IReadOnlyList<ColumnMapping> columnMappings)
        {
            var parentRelationship = ParentRelationship;
            if (parentRelationship == null)
                return null;

            var result = new ColumnMapping[parentRelationship.Count];
            for (int i = 0; i < result.Length; i++)
            {
                var mapping = parentRelationship[i];
                var source = GetSource(columnMappings, mapping.SourceExpression);
                if (source == null)
                    throw new InvalidOperationException(DiagnosticMessages.ChildColumnNotExistInColumnMappings(mapping.SourceExpression));
                result[i] = new ColumnMapping(source, mapping.Target);
            }

            return result;
        }

        private static DbExpression GetSource(IReadOnlyList<ColumnMapping> mappings, DbExpression target)
        {
            foreach (var mapping in mappings)
            {
                if (mapping.TargetExpression == target)
                    return mapping.SourceExpression;
            }
            return null;
        }

        protected internal virtual DataSet<T> NewDataSetValue<T>(_DataSet<T> dataSetColumn, int rowOrdinal)
            where T : Model, new()
        {
            return DataSet<T>.New();
        }

        internal DataRow EditingRow { get; private set; }

        internal void BeginEdit(DataRow dataRow)
        {
            Debug.Assert(EditingRow == null && dataRow != null);
            EnsureInitialized();
            EditingRow = dataRow;
            for (int i = 0; i < Columns.Count; i++)
                Columns[i].BeginEdit(dataRow);
            for (int i = 0; i < LocalColumns.Count; i++)
                LocalColumns[i].BeginEdit(dataRow);
        }

        internal void EndEdit(DataRow dataRow, bool discardChanges)
        {
            Debug.Assert(EditingRow != null);
            Debug.Assert(EditingRow == DataRow.Placeholder || EditingRow == dataRow);

            dataRow.SuspendValueChangedNotification(discardChanges);
            for (int i = 0; i < Columns.Count; i++)
                Columns[i].EndEdit(dataRow);
            for (int i = 0; i < LocalColumns.Count; i++)
                LocalColumns[i].EndEdit(dataRow);
            EditingRow = null;
            dataRow.ResumeValueChangedNotification();
        }

        internal void CancelEdit()
        {
            Debug.Assert(EditingRow != null);
            EditingRow = null;
        }

        internal void LockEditingRow(Action action)
        {
            var editingRow = EditingRow;
            EditingRow = DataRow.Placeholder;
            action();
            EditingRow = editingRow;
        }

        internal Column DeserializeColumn(string columnName)
        {
            if (string.IsNullOrWhiteSpace(columnName))
                return null;

            var result = Columns[columnName];
            if (result == null)
                throw new FormatException(DiagnosticMessages.Model_InvalidColumnName(columnName));
            return result;
        }

        public event EventHandler<EventArgs> Constructing = delegate { };
        public event EventHandler<EventArgs> Initializing = delegate { };
        public event EventHandler<EventArgs> ChildModelsMounted = delegate { };
        public event EventHandler<EventArgs> ChildDataSetsCreated = delegate { };
        public event EventHandler<EventArgs> Initialized = delegate { };
        public event EventHandler<DataRowEventArgs> DataRowInserting = delegate { };
        public event EventHandler<DataRowEventArgs> BeforeDataRowInserted = delegate { };
        public event EventHandler<DataRowEventArgs> AfterDataRowInserted = delegate { };
        public event EventHandler<DataRowEventArgs> DataRowRemoving = delegate { };
        public event EventHandler<DataRowRemovedEventArgs> DataRowRemoved = delegate { };
        public event EventHandler<ValueChangedEventArgs> ValueChanged = delegate { };

        internal void HandlesDataRowInserted(DataRow dataRow, Action<DataRow> updateAction)
        {
            dataRow.SuspendValueChangedNotification(true);
            var e = new DataRowEventArgs(dataRow);
            OnDataRowInserting(e);
            DataSetContainer.OnDataRowInserting(e);
            if (updateAction != null)
                updateAction(dataRow);
            OnBeforeDataRowInserted(e);
            DataSetContainer.OnBeforeDataRowInserted(e);
            dataRow.ResumeValueChangedNotification();
            DataSetContainer.SuspendComputation();
            OnDataRowInserted(e);
            DataSetContainer.OnAfterDataRowInserted(e);
            DataSetContainer.ResumeComputation();
        }

        protected virtual void OnDataRowInserting(DataRowEventArgs e)
        {
            DataRowInserting(this, e);
        }

        protected virtual void OnBeforeDataRowInserted(DataRowEventArgs e)
        {
            BeforeDataRowInserted(this, e);
        }

        protected virtual void OnDataRowInserted(DataRowEventArgs e)
        {
            AfterDataRowInserted(this, e);
        }

        internal void HandlesDataRowRemoving(DataRow dataRow)
        {
            dataRow.SuspendValueChangedNotification(true);
            var e = new DataRowEventArgs(dataRow);
            OnDataRowRemoving(e);
            DataSetContainer.OnDataRowRemoving(e);
        }

        protected virtual void OnDataRowRemoving(DataRowEventArgs e)
        {
            DataRowRemoving(this, e);
        }

        internal void HandlesDataRowRemoved(DataRow dataRow, DataSet baseDataSet, int ordinal, DataSet dataSet, int index)
        {
            if (EditingRow == dataRow)
                CancelEdit();

            dataRow.ResumeValueChangedNotification();
            DataSetContainer.SuspendComputation();
            var e = new DataRowRemovedEventArgs(dataRow, baseDataSet, ordinal, dataSet, index);
            OnDataRowRemoved(e);
            DataSetContainer.OnDataRowRemoved(e);
            DataSetContainer.ResumeComputation();
        }

        protected virtual void OnDataRowRemoved(DataRowRemovedEventArgs e)
        {
            DataRowRemoved(this, e);
        }

        internal void HandlesValueChanged(DataRow dataRow, IColumns columns)
        {
            DataSetContainer.SuspendComputation();
            var e = new ValueChangedEventArgs(dataRow, columns);
            OnValueChanged(e);
            DataSetContainer.OnValueChanged(e);
            DataSetContainer.ResumeComputation();
        }

        protected virtual void OnValueChanged(ValueChangedEventArgs e)
        {
            ValueChanged(this, e);
        }

        private DataSetContainer _dataSetContainer;
        public DataSetContainer DataSetContainer
        {
            get { return RootModel._dataSetContainer; }
        }

        protected Column<T> CreateLocalColumn<T>(Action<LocalColumnBuilder<T>> builder = null)
        {
            return DataSetContainer == null ? null : DataSetContainer.CreateLocalColumn(this, builder);
        }

        protected Column<T> CreateLocalColumn<T1, T>(T1 column, Func<DataRow, T1, T> expression, Action<LocalColumnBuilder<T>> builder = null)
            where T1 : Column
        {
            return DataSetContainer == null ? null : DataSetContainer.CreateLocalColumn(this, column, expression, builder);
        }

        protected Column<T> CreateLocalColumn<T1, T2, T>(T1 column1, T2 column2, Func<DataRow, T1, T2, T> expression, Action<LocalColumnBuilder<T>> builder = null)
            where T1 : Column
            where T2 : Column
        {
            return DataSetContainer == null ? null : DataSetContainer.CreateLocalColumn(this, column1, column2, expression, builder);
        }

        protected Column<T> CreateLocalColumn<T1, T2, T3, T>(T1 column1, T2 column2, T3 column3, Func<DataRow, T1, T2, T3, T> expression, Action<LocalColumnBuilder<T>> builder = null)
            where T1 : Column
            where T2 : Column
            where T3 : Column
        {
            return DataSetContainer == null ? null : DataSetContainer.CreateLocalColumn(this, column1, column2, column3, expression, builder);
        }

        protected Column<T> CreateLocalColumn<T1, T2, T3, T4, T>(T1 column1, T2 column2, T3 column3, T4 column4,
            Func<DataRow, T1, T2, T3, T4, T> expression, Action<LocalColumnBuilder<T>> builder = null)
            where T1 : Column
            where T2 : Column
            where T3 : Column
            where T4 : Column
        {
            return DataSetContainer == null ? null : DataSetContainer.CreateLocalColumn(this, column1, column2, column3, column4, expression, builder);
        }

        protected Column<T> CreateLocalColumn<T1, T2, T3, T4, T5, T>(T1 column1, T2 column2, T3 column3, T4 column4, T5 column5,
            Func<DataRow, T1, T2, T3, T4, T5, T> expression, Action<LocalColumnBuilder<T>> builder = null)
            where T1 : Column
            where T2 : Column
            where T3 : Column
            where T4 : Column
            where T5 : Column
        {
            return DataSetContainer == null ? null : DataSetContainer.CreateLocalColumn(this, column1, column2, column3, column4, column5, expression, builder);
        }

        protected Column<T> CreateLocalColumn<T1, T2, T3, T4, T5, T6, T>(T1 column1, T2 column2, T3 column3, T4 column4, T5 column5, T6 column6,
            Func<DataRow, T1, T2, T3, T4, T5, T6, T> expression, Action<LocalColumnBuilder<T>> builder = null)
            where T1 : Column
            where T2 : Column
            where T3 : Column
            where T4 : Column
            where T5 : Column
            where T6 : Column
        {
            return DataSetContainer == null ? null : DataSetContainer.CreateLocalColumn(this, column1, column2, column3, column4, column5, column6, expression, builder);
        }

        protected Column<T> CreateLocalColumn<T1, T2, T3, T4, T5, T6, T7, T>(T1 column1, T2 column2, T3 column3, T4 column4, T5 column5, T6 column6, T7 column7,
            Func<DataRow, T1, T2, T3, T4, T5, T6, T7, T> expression, Action<LocalColumnBuilder<T>> builder = null)
            where T1 : Column
            where T2 : Column
            where T3 : Column
            where T4 : Column
            where T5 : Column
            where T6 : Column
            where T7 : Column
        {
            return DataSetContainer == null ? null : DataSetContainer.CreateLocalColumn(this, column1, column2, column3, column4, column5, column6, column7, expression, builder);
        }

        protected Column<T> CreateLocalColumn<T1, T2, T3, T4, T5, T6, T7, T8, T>(T1 column1, T2 column2, T3 column3, T4 column4, T5 column5, T6 column6, T7 column7, T8 column8,
            Func<DataRow, T1, T2, T3, T4, T5, T6, T7, T8, T> expression, Action<LocalColumnBuilder<T>> builder = null)
            where T1 : Column
            where T2 : Column
            where T3 : Column
            where T4 : Column
            where T5 : Column
            where T6 : Column
            where T7 : Column
            where T8 : Column
        {
            return DataSetContainer == null ? null : DataSetContainer.CreateLocalColumn(this, column1, column2, column3, column4, column5, column6, column7, column8, expression, builder);
        }

        protected Column<T> CreateLocalColumn<T1, T2, T3, T4, T5, T6, T7, T8, T9, T>(T1 column1, T2 column2, T3 column3, T4 column4, T5 column5, T6 column6, T7 column7, T8 column8,
            T9 column9, Func<DataRow, T1, T2, T3, T4, T5, T6, T7, T8, T9, T> expression, Action<LocalColumnBuilder<T>> builder = null)
            where T1 : Column
            where T2 : Column
            where T3 : Column
            where T4 : Column
            where T5 : Column
            where T6 : Column
            where T7 : Column
            where T8 : Column
            where T9 : Column
        {
            return DataSetContainer == null ? null : DataSetContainer.CreateLocalColumn(this, column1, column2, column3, column4, column5, column6, column7, column8, column9, expression, builder);
        }

        protected Column<T> CreateLocalColumn<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T>(T1 column1, T2 column2, T3 column3, T4 column4, T5 column5, T6 column6, T7 column7,
            T8 column8, T9 column9, T10 column10, Func<DataRow, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T> expression, Action<LocalColumnBuilder<T>> builder = null)
            where T1 : Column
            where T2 : Column
            where T3 : Column
            where T4 : Column
            where T5 : Column
            where T6 : Column
            where T7 : Column
            where T8 : Column
            where T9 : Column
            where T10 : Column
        {
            return DataSetContainer == null ? null : DataSetContainer.CreateLocalColumn(this, column1, column2, column3, column4, column5, column6, column7, column8, column9, column10, expression, builder);
        }

        protected Column<T> CreateLocalColumn<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T>(T1 column1, T2 column2, T3 column3, T4 column4, T5 column5, T6 column6, T7 column7,
            T8 column8, T9 column9, T10 column10, T11 column11, Func<DataRow, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T> expression, Action<LocalColumnBuilder<T>> builder = null)
            where T1 : Column
            where T2 : Column
            where T3 : Column
            where T4 : Column
            where T5 : Column
            where T6 : Column
            where T7 : Column
            where T8 : Column
            where T9 : Column
            where T10 : Column
            where T11 : Column
        {
            return DataSetContainer == null ? null : DataSetContainer.CreateLocalColumn(this, column1, column2, column3, column4, column5, column6, column7,
                column8, column9, column10, column11, expression, builder);
        }

        protected Column<T> CreateLocalColumn<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T>(T1 column1, T2 column2, T3 column3, T4 column4, T5 column5, T6 column6,
            T7 column7, T8 column8, T9 column9, T10 column10, T11 column11, T12 column12,
            Func<DataRow, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T> expression, Action<LocalColumnBuilder<T>> builder = null)
            where T1 : Column
            where T2 : Column
            where T3 : Column
            where T4 : Column
            where T5 : Column
            where T6 : Column
            where T7 : Column
            where T8 : Column
            where T9 : Column
            where T10 : Column
            where T11 : Column
            where T12 : Column
        {
            return DataSetContainer == null ? null : DataSetContainer.CreateLocalColumn(this, column1, column2, column3, column4, column5, column6, column7,
                column8, column9, column10, column11, column12, expression, builder);
        }

        private ColumnCombination _extraColumns;
        internal ColumnCombination ExtraColumns
        {
            get { return _extraColumns; }
            set
            {
                Debug.Assert(_extraColumns == null);
                Debug.Assert(value != null);
                _extraColumns = value;
                _extraColumns.Initialize(this);
            }
        }

        public void SetExtraColumns<T>()
            where T : ColumnCombination, new()
        {
            VerifyDesignMode();
            if (ExtraColumns != null)
                throw new InvalidOperationException(DiagnosticMessages.Model_ExtensionAlreadyExists);
            else
                ExtraColumns = new T();
        }

        internal void SetExtraColumns(Type extType)
        {
            Debug.Assert(extType != null);
            if (ExtraColumns != null)
                throw new InvalidOperationException(DiagnosticMessages.Model_ExtensionAlreadyExists);
            else
                ExtraColumns = (ColumnCombination)Activator.CreateInstance(extType);
        }

        public T GetExtraColumns<T>()
            where T : ColumnCombination, new()
        {
            return ExtraColumns as T;
        }

        private int _suspendEditingValueCount = 0;
        internal void SuspendEditingValue()
        {
            RootModel._suspendEditingValueCount++;
        }

        internal void ResumeEditingValue()
        {
            Debug.Assert(IsEditingValueSuspended);
            RootModel._suspendEditingValueCount--;
        }

        internal bool IsEditingValueSuspended
        {
            get { return RootModel._suspendEditingValueCount > 0; }
        }

        Model IModelReference.Model
        {
            get { return this; }
        }

        internal virtual bool IsExtRoot
        {
            get { return false; }
        }
    }
}