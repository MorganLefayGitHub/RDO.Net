// <auto-generated />
namespace DevZest.Data
{
    using System.Diagnostics;
    using System.Globalization;
    using System.Reflection;
    using System.Resources;

    internal static class Strings
    {
        private static readonly ResourceManager _resourceManager
            = new ResourceManager("DevZest.Data.Strings", typeof(Strings).GetTypeInfo().Assembly);

        /// <summary>
        /// The provided getter expression is invalid.
        /// </summary>
        public static string Property_InvalidGetter
        {
            get { return GetString("Property_InvalidGetter"); }
        }

        /// <summary>
        /// Cannot register property for type '{type}' after an instance of this type or its derived type has been created.
        /// </summary>
        public static string Property_RegisterAfterUse(object type)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("Property_RegisterAfterUse", "type"), type);
        }

        /// <summary>
        /// The property with OwnerType '{ownerType}' and Name '{name}' has been registered already.
        /// </summary>
        public static string Property_RegisterDuplicate(object ownerType, object name)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("Property_RegisterDuplicate", "ownerType", "name"), ownerType, name);
        }

        /// <summary>
        /// The argument '{argumentName}' cannot be null, empty or contain only white space.
        /// </summary>
        public static string ArgumentIsNullOrWhitespace(object argumentName)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("ArgumentIsNullOrWhitespace", "argumentName"), argumentName);
        }

        /// <summary>
        /// Cannot evaluate a non DataSet column: Column={column}, DataSource.Kind={dataSourceKind}.
        /// </summary>
        public static string ColumnAggregateFunction_EvalOnNonDataSet(object column, object dataSourceKind)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("ColumnAggregateFunction_EvalOnNonDataSet", "column", "dataSourceKind"), column, dataSourceKind);
        }

        /// <summary>
        /// Cannot resolve model chain from Column "{column}" to provided dataRow.
        /// </summary>
        public static string ColumnAggregateFunction_NoModelChain(object column)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("ColumnAggregateFunction_NoModelChain", "column"), column);
        }

        /// <summary>
        /// Duplicate ColumnKey is not allowed: OriginalOwnerType={originalOwnerType}, OriginalName={originalName}.
        /// </summary>
        public static string ColumnCollection_DuplicateColumnKey(object originalOwnerType, object originalName)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("ColumnCollection_DuplicateColumnKey", "originalOwnerType", "originalName"), originalOwnerType, originalName);
        }

        /// <summary>
        /// The type resolver callback returns null.
        /// </summary>
        public static string GenericInvoker_TypeResolverReturnsNull
        {
            get { return GetString("GenericInvoker_TypeResolverReturnsNull"); }
        }

        /// <summary>
        /// The type "{type}" is not implemented correctly. The column getter "{columnGetter}" returns null.
        /// </summary>
        public static string ColumnGroup_GetterReturnsNull(object type, object columnGetter)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("ColumnGroup_GetterReturnsNull", "type", "columnGetter"), type, columnGetter);
        }

        /// <summary>
        /// The type "{type}" is not implemented correctly. The parent models of column "{column1}" and "{column2}" are inconsistent.
        /// </summary>
        public static string ColumnGroup_InconsistentParentModel(object type, object column1, object column2)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("ColumnGroup_InconsistentParentModel", "type", "column1", "column2"), type, column1, column2);
        }

        /// <summary>
        /// The expression is already attached to a Column.
        /// </summary>
        public static string ColumnExpression_AlreadyAttached
        {
            get { return GetString("ColumnExpression_AlreadyAttached"); }
        }

        /// <summary>
        /// The DataRow is invalid, it does not contain this Column.
        /// </summary>
        public static string Column_InvalidDataRow
        {
            get { return GetString("Column_InvalidDataRow"); }
        }

        /// <summary>
        /// The DbReader is invalid, it does not contain this Column.
        /// </summary>
        public static string Column_InvalidDbReader
        {
            get { return GetString("Column_InvalidDbReader"); }
        }

        /// <summary>
        /// -- Canceled in {elapsedMilliSeconds} ms
        /// </summary>
        public static string DbLogger_CommandCanceled(object elapsedMilliSeconds)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("DbLogger_CommandCanceled", "elapsedMilliSeconds"), elapsedMilliSeconds);
        }

        /// <summary>
        /// -- Completed in {elapsedMilliSeconds} ms with result: {result}
        /// </summary>
        public static string DbLogger_CommandComplete(object elapsedMilliSeconds, object result)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("DbLogger_CommandComplete", "elapsedMilliSeconds", "result"), elapsedMilliSeconds, result);
        }

        /// <summary>
        /// -- Executing at {time}
        /// </summary>
        public static string DbLogger_CommandExecuting(object time)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("DbLogger_CommandExecuting", "time"), time);
        }

        /// <summary>
        /// -- Executing asynchronously at {time}
        /// </summary>
        public static string DbLogger_CommandExecutingAsync(object time)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("DbLogger_CommandExecutingAsync", "time"), time);
        }

        /// <summary>
        /// -- Failed in {elapsedMilliSeconds} ms with error: {error}
        /// </summary>
        public static string DbLogger_CommandFailed(object elapsedMilliSeconds, object error)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("DbLogger_CommandFailed", "elapsedMilliSeconds", "error"), elapsedMilliSeconds, error);
        }

        /// <summary>
        /// Closed connection at {time}
        /// </summary>
        public static string DbLogger_ConnectionClosed(object time)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("DbLogger_ConnectionClosed", "time"), time);
        }

        /// <summary>
        /// Failed to close connection at {time} with error: {error}
        /// </summary>
        public static string DbLogger_ConnectionCloseError(object time, object error)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("DbLogger_ConnectionCloseError", "time", "error"), time, error);
        }

        /// <summary>
        /// Opened connection at {time}
        /// </summary>
        public static string DbLogger_ConnectionOpen(object time)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("DbLogger_ConnectionOpen", "time"), time);
        }

        /// <summary>
        /// Opened connection asynchronously at {time}
        /// </summary>
        public static string DbLogger_ConnectionOpenAsync(object time)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("DbLogger_ConnectionOpenAsync", "time"), time);
        }

        /// <summary>
        /// Cancelled open connection at {time}
        /// </summary>
        public static string DbLogger_ConnectionOpenCanceled(object time)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("DbLogger_ConnectionOpenCanceled", "time"), time);
        }

        /// <summary>
        /// Failed to open connection at {time} with error: {error}
        /// </summary>
        public static string DbLogger_ConnectionOpenError(object time, object error)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("DbLogger_ConnectionOpenError", "time", "error"), time, error);
        }

        /// <summary>
        /// Failed to open connection asynchronously at {time} with error: {error}
        /// </summary>
        public static string DbLogger_ConnectionOpenErrorAsync(object time, object error)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("DbLogger_ConnectionOpenErrorAsync", "time", "error"), time, error);
        }

        /// <summary>
        /// Failed to commit transaction at {time} with error: {error}
        /// </summary>
        public static string DbLogger_TransactionCommitError(object time, object error)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("DbLogger_TransactionCommitError", "time", "error"), time, error);
        }

        /// <summary>
        /// Committed transaction at {time}
        /// </summary>
        public static string DbLogger_TransactionCommitted(object time)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("DbLogger_TransactionCommitted", "time"), time);
        }

        /// <summary>
        /// Failed to rollback transaction at {time} with error: {error}
        /// </summary>
        public static string DbLogger_TransactionRollbackError(object time, object error)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("DbLogger_TransactionRollbackError", "time", "error"), time, error);
        }

        /// <summary>
        /// Rolled back transaction at {time}
        /// </summary>
        public static string DbLogger_TransactionRolledBack(object time)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("DbLogger_TransactionRolledBack", "time"), time);
        }

        /// <summary>
        /// Started transaction at {time}
        /// </summary>
        public static string DbLogger_TransactionStarted(object time)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("DbLogger_TransactionStarted", "time"), time);
        }

        /// <summary>
        /// Failed to start transaction at {time} with error: {error}
        /// </summary>
        public static string DbLogger_TransactionStartError(object time, object error)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("DbLogger_TransactionStartError", "time", "error"), time, error);
        }

        /// <summary>
        /// Cannot call From method multiple times.
        /// </summary>
        public static string DbQueryBuilder_DuplicateFrom
        {
            get { return GetString("DbQueryBuilder_DuplicateFrom"); }
        }

        /// <summary>
        /// Invalid left key. Its ParentModel must be previously added as source query.
        /// </summary>
        public static string DbQueryBuilder_Join_InvalidLeftKey
        {
            get { return GetString("DbQueryBuilder_Join_InvalidLeftKey"); }
        }

        /// <summary>
        /// Invalid right key. Its ParentModel must be the same as the provided model parameter.
        /// </summary>
        public static string DbQueryBuilder_Join_InvalidRightKey
        {
            get { return GetString("DbQueryBuilder_Join_InvalidRightKey"); }
        }

        /// <summary>
        /// Aggregate expression is not allowed.
        /// </summary>
        public static string DbQueryBuilder_AggregateNotAllowed
        {
            get { return GetString("DbQueryBuilder_AggregateNotAllowed"); }
        }

        /// <summary>
        /// Invalid targetColumn. It must be a column of target model, and cannot be selected already.
        /// </summary>
        public static string DbQueryBuilder_VerifyTargetColumn
        {
            get { return GetString("DbQueryBuilder_VerifyTargetColumn"); }
        }

        /// <summary>
        /// The child model's ParentModel must be the same as this object's Model.
        /// </summary>
        public static string InvalidChildModel
        {
            get { return GetString("InvalidChildModel"); }
        }

        /// <summary>
        /// The child model returned by the getter is invalid. It cannot be null and its ParentModel must be the calling model.
        /// </summary>
        public static string InvalidChildModelGetter
        {
            get { return GetString("InvalidChildModelGetter"); }
        }

        /// <summary>
        /// The identity increment value cannot be 0.
        /// </summary>
        public static string Model_InvalidIdentityIncrement
        {
            get { return GetString("Model_InvalidIdentityIncrement"); }
        }

        /// <summary>
        /// The column must be child of this model.
        /// </summary>
        public static string Model_VerifyChildColumn
        {
            get { return GetString("Model_VerifyChildColumn"); }
        }

        /// <summary>
        /// The operation is only allowed in design mode.
        /// </summary>
        public static string VerifyDesignMode
        {
            get { return GetString("VerifyDesignMode"); }
        }

        /// <summary>
        /// Cannot define multiple identity column on the same table.
        /// </summary>
        public static string Model_MultipleIdentityColumn
        {
            get { return GetString("Model_MultipleIdentityColumn"); }
        }

        /// <summary>
        /// Cannot have more than one clustered index.  The clustered index '{existingClusterIndexName}' already exists.
        /// </summary>
        public static string Model_MultipleClusteredIndex(object existingClusterIndexName)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("Model_MultipleClusteredIndex", "existingClusterIndexName"), existingClusterIndexName);
        }

        /// <summary>
        /// The constraint '{constraintName}' already exists.
        /// </summary>
        public static string Model_DuplicateConstraintName(object constraintName)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("Model_DuplicateConstraintName", "constraintName"), constraintName);
        }

        /// <summary>
        /// The columns cannot be empty.
        /// </summary>
        public static string Model_EmptyColumns
        {
            get { return GetString("Model_EmptyColumns"); }
        }

        /// <summary>
        /// The reference table model is invalid. It must either be a self reference or a reference to a existing table.
        /// </summary>
        public static string Model_InvalidRefTableModel
        {
            get { return GetString("Model_InvalidRefTableModel"); }
        }

        /// <summary>
        /// The child DbSet has been created already.
        /// </summary>
        public static string DbSet_VerifyCreateChild_AlreadyCreated
        {
            get { return GetString("DbSet_VerifyCreateChild_AlreadyCreated"); }
        }

        /// <summary>
        /// Creating child DbSet on DbTable is not allowed.
        /// </summary>
        public static string DbSet_VerifyCreateChild_InvalidDataSourceKind
        {
            get { return GetString("DbSet_VerifyCreateChild_InvalidDataSourceKind"); }
        }

        /// <summary>
        /// Cannot set value of readonly column '{column}'.
        /// </summary>
        public static string Column_SetReadOnlyValue(object column)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("Column_SetReadOnlyValue", "column"), column);
        }

        /// <summary>
        /// The column[{columnIndex}] '{column}' is not supported by this DbSession.
        /// </summary>
        public static string DbSession_ColumnNotSupported(object columnIndex, object column)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("DbSession_ColumnNotSupported", "columnIndex", "column"), columnIndex, column);
        }

        /// <summary>
        /// Reached EOF unexpectedly.
        /// </summary>
        public static string JsonParser_UnexpectedEof
        {
            get { return GetString("JsonParser_UnexpectedEof"); }
        }

        /// <summary>
        /// Invalid char '{ch}' at index {index}.
        /// </summary>
        public static string JsonParser_InvalidChar(object ch, object index)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("JsonParser_InvalidChar", "ch", "index"), ch, index);
        }

        /// <summary>
        /// Char '{ch}' at index {index} is not a valid hex number.
        /// </summary>
        public static string JsonParser_InvalidHexChar(object ch, object index)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("JsonParser_InvalidHexChar", "ch", "index"), ch, index);
        }

        /// <summary>
        /// '{ch}' expected at index {index}.
        /// </summary>
        public static string JsonParser_InvalidLiteral(object ch, object index)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("JsonParser_InvalidLiteral", "ch", "index"), ch, index);
        }

        /// <summary>
        /// Invalid string escape '{stringEscape}' at index {index}.
        /// </summary>
        public static string JsonParser_InvalidStringEscape(object stringEscape, object index)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("JsonParser_InvalidStringEscape", "stringEscape", "index"), stringEscape, index);
        }

        /// <summary>
        /// Invalid member name "{memberName}" for Model "{model}".
        /// </summary>
        public static string JsonParser_InvalidModelMember(object memberName, object model)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("JsonParser_InvalidModelMember", "memberName", "model"), memberName, model);
        }

        /// <summary>
        /// Cannot deserialize from JSON value. Provided JSON value must be 'true', 'false' or 'null'.
        /// </summary>
        public static string BooleanColumn_CannotDeserialize
        {
            get { return GetString("BooleanColumn_CannotDeserialize"); }
        }

        /// <summary>
        /// Current token kind "{tokenKind}" is invalid. Expected token kind: {expectedTokenKind}.
        /// </summary>
        public static string JsonParser_InvalidTokenKind(object tokenKind, object expectedTokenKind)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("JsonParser_InvalidTokenKind", "tokenKind", "expectedTokenKind"), tokenKind, expectedTokenKind);
        }

        /// <summary>
        /// The source column derives from invalid model '{model}'.
        /// </summary>
        public static string ColumnMappingsBuilder_InvalidSourceParentModelSet(object model)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("ColumnMappingsBuilder_InvalidSourceParentModelSet", "model"), model);
        }

        /// <summary>
        /// The target column '{targetColumn}' is invalid.
        /// </summary>
        public static string ColumnMappingsBuilder_InvalidTarget(object targetColumn)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("ColumnMappingsBuilder_InvalidTarget", "targetColumn"), targetColumn);
        }

        /// <summary>
        /// Cannot match primary key of current table and the source Model.
        /// </summary>
        public static string DbTable_GetKeyMappings_CannotMatch
        {
            get { return GetString("DbTable_GetKeyMappings_CannotMatch"); }
        }

        /// <summary>
        /// The operation is not supported by readonly list.
        /// </summary>
        public static string NotSupportedByReadOnlyList
        {
            get { return GetString("NotSupportedByReadOnlyList"); }
        }

        /// <summary>
        /// Circular reference detected for table "{tableName}".
        /// </summary>
        public static string MockDb_CircularReference(object tableName)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("MockDb_CircularReference", "tableName"), tableName);
        }

        /// <summary>
        /// The table "{tableName}" cannot be mocked twice.
        /// </summary>
        public static string MockDb_DuplicateTable(object tableName)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("MockDb_DuplicateTable", "tableName"), tableName);
        }

        /// <summary>
        /// DbMock object cannot be initialized twice.
        /// </summary>
        public static string MockDb_InitializeTwice
        {
            get { return GetString("MockDb_InitializeTwice"); }
        }

        /// <summary>
        /// The mocking table is invalid. It must belong to the given DbSession.
        /// </summary>
        public static string MockDb_InvalidTable
        {
            get { return GetString("MockDb_InvalidTable"); }
        }

        /// <summary>
        /// Mock can only be called during initialization.
        /// </summary>
        public static string MockDb_MockOnlyAllowedDuringInitialization
        {
            get { return GetString("MockDb_MockOnlyAllowedDuringInitialization"); }
        }

        /// <summary>
        /// The type argument "{typeArgument}" does not match with type argument "{expectedTypeArgument}" used for for table "{tableName}".
        /// </summary>
        public static string MockDb_ModelTypeMismatch(object typeArgument, object expectedTypeArgument, object tableName)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("MockDb_ModelTypeMismatch", "typeArgument", "expectedTypeArgument", "tableName"), typeArgument, expectedTypeArgument, tableName);
        }

        /// <summary>
        /// The returned where expression is invalid.
        /// </summary>
        public static string DbTable_VerifyWhere
        {
            get { return GetString("DbTable_VerifyWhere"); }
        }

        /// <summary>
        /// The child column '{childColumn}' does not exist in the column mappings.
        /// </summary>
        public static string ChildColumnNotExistInColumnMappings(object childColumn)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("ChildColumnNotExistInColumnMappings", "childColumn"), childColumn);
        }

        /// <summary>
        /// The operation requires a primary key of model '{model}'.
        /// </summary>
        public static string DbTable_NoPrimaryKey(object model)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("DbTable_NoPrimaryKey", "model"), model);
        }

        /// <summary>
        /// The source column's data type '{sourceColumnDataType}' is invalid. Data type '{expectedDataType}' required.
        /// </summary>
        public static string ColumnMappingsBuilder_InvalidSourceDataType(object sourceColumnDataType, object expectedDataType)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("ColumnMappingsBuilder_InvalidSourceDataType", "sourceColumnDataType", "expectedDataType"), sourceColumnDataType, expectedDataType);
        }

        /// <summary>
        /// No ColumnMapping specified.
        /// </summary>
        public static string ColumnMappingsBuilder_NoColumnMapping
        {
            get { return GetString("ColumnMappingsBuilder_NoColumnMapping"); }
        }

        /// <summary>
        /// Cannot add DataRow into multiple DataSet.
        /// </summary>
        public static string DataSet_InvalidNewDataRow
        {
            get { return GetString("DataSet_InvalidNewDataRow"); }
        }

        /// <summary>
        /// Value is required for column '{column}'.
        /// </summary>
        public static string RequiredAttribute_DefaultErrorMessage(object column)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("RequiredAttribute_DefaultErrorMessage", "column"), column);
        }

        /// <summary>
        /// Cannot resolve static method of Func&lt;Column, DataRow, string&gt; from provided type '{funcType}' and method name '{funcName}'.
        /// </summary>
        public static string ColumnValidatorAttribute_InvalidMessageFunc(object funcType, object funcName)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("ColumnValidatorAttribute_InvalidMessageFunc", "funcType", "funcName"), funcType, funcName);
        }

        /// <summary>
        /// Target table must be a permanent table and has a identity column.
        /// </summary>
        public static string DbTable_VerifyUpdateIdentity
        {
            get { return GetString("DbTable_VerifyUpdateIdentity"); }
        }

        /// <summary>
        /// Delete is not supported for parent table.
        /// </summary>
        public static string DbTable_DeleteNotSupportedForParentTable
        {
            get { return GetString("DbTable_DeleteNotSupportedForParentTable"); }
        }

        /// <summary>
        /// The source is invalid: cross DbSession is not supported.
        /// </summary>
        public static string DbTable_InvalidDbSetSource
        {
            get { return GetString("DbTable_InvalidDbSetSource"); }
        }

        /// <summary>
        /// Char '{ch}' expected after '{prevInput}'.
        /// </summary>
        public static string DataRow_FromString_ExpectChar(object ch, object prevInput)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("DataRow_FromString_ExpectChar", "ch", "prevInput"), ch, prevInput);
        }

        /// <summary>
        /// The child model name '{childModelName}' is invalid for DataRow '{dataRowPath}'.
        /// </summary>
        public static string DataRow_FromString_InvalidChildModelName(object childModelName, object dataRowPath)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("DataRow_FromString_InvalidChildModelName", "childModelName", "dataRowPath"), childModelName, dataRowPath);
        }

        /// <summary>
        /// The DataRow ordinal '{dataRowOrdinal}' is invalid for DataSet '{dataSetPath}'.
        /// </summary>
        public static string DataRow_FromString_InvalidDataRowOrdinal(object dataRowOrdinal, object dataSetPath)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("DataRow_FromString_InvalidDataRowOrdinal", "dataRowOrdinal", "dataSetPath"), dataRowOrdinal, dataSetPath);
        }

        /// <summary>
        /// Cannot parse string '{input}' into an integer value.
        /// </summary>
        public static string DataRow_FromString_ParseInt(object input)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("DataRow_FromString_ParseInt", "input"), input);
        }

        /// <summary>
        /// The column is invalid. It must be created by calling CreateColumn method.
        /// </summary>
        public static string Scalar_InvalidColumn
        {
            get { return GetString("Scalar_InvalidColumn"); }
        }

        /// <summary>
        /// The column name '{columnName}' is invalid for DataRow '{dataRowString}'.
        /// </summary>
        public static string DataRow_DeserializeColumn_InvalidColumnName(object columnName, object dataRowString)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("DataRow_DeserializeColumn_InvalidColumnName", "columnName", "dataRowString"), columnName, dataRowString);
        }

        /// <summary>
        /// Mismatch with Model.SavedDataRow. The DataRow must be previously saved by calling DataRow.Save.
        /// </summary>
        public static string DataRow_MismatchWithSavedDataRow
        {
            get { return GetString("DataRow_MismatchWithSavedDataRow"); }
        }

        /// <summary>
        /// The object name "{objectName}" is invalid, "{expectedObjectName}" expected.
        /// </summary>
        public static string JsonParser_InvalidObjectName(object objectName, object expectedObjectName)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("JsonParser_InvalidObjectName", "objectName", "expectedObjectName"), objectName, expectedObjectName);
        }

        /// <summary>
        /// Invalid object name "{objName}". "{expected1}" or "{expected2}" expected.
        /// </summary>
        public static string ColumnJsonParser_InvalidObjectName(object objName, object expected1, object expected2)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("ColumnJsonParser_InvalidObjectName", "objName", "expected1", "expected2"), objName, expected1, expected2);
        }

        /// <summary>
        /// Invalid Model["{name}"]: Type "{expectedType}" expected.
        /// </summary>
        public static string ColumnJsonParser_InvalidColumnType(object name, object expectedType)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("ColumnJsonParser_InvalidColumnType", "name", "expectedType"), name, expectedType);
        }

        /// <summary>
        /// The TypeId "{typeId}" is invalid, null ColumnConverter resolved.
        /// </summary>
        public static string ColumnJsonParser_InvalidTypeId(object typeId)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("ColumnJsonParser_InvalidTypeId", "typeId"), typeId);
        }

        /// <summary>
        /// The type is invalid. It must be a generic type definition with two generic parameters DataType and ColumnType.
        /// </summary>
        public static string GenericExpressionConverterAttribute_InvalidGenericExpressionType
        {
            get { return GetString("GenericExpressionConverterAttribute_InvalidGenericExpressionType"); }
        }

        /// <summary>
        /// There is no ColumnConverterAttribute defined for type "{columnType}".
        /// </summary>
        public static string ColumnConverter_NotDefined(object columnType)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("ColumnConverter_NotDefined", "columnType"), columnType);
        }

        /// <summary>
        /// The columnType must be assignable to "{assignableTo}".
        /// </summary>
        public static string ColumnExpression_InvalidMakeColumnType(object assignableTo)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("ColumnExpression_InvalidMakeColumnType", "assignableTo"), assignableTo);
        }

        /// <summary>
        /// There is no ExpressionConverterAttribute defined for type "{expressionType}".
        /// </summary>
        public static string ExpressionConverter_NotDefined(object expressionType)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("ExpressionConverter_NotDefined", "expressionType"), expressionType);
        }

        /// <summary>
        /// Cannot match When and Then columns. They cannot be empty and must have identical size.
        /// </summary>
        public static string Case_WhenThenNotMatch
        {
            get { return GetString("Case_WhenThenNotMatch"); }
        }

        /// <summary>
        /// The source DataRow and this DataRow must belong to DataSets which have clone relationship.
        /// </summary>
        public static string DataRow_VerifyPrototype
        {
            get { return GetString("DataRow_VerifyPrototype"); }
        }

        /// <summary>
        /// The StringBuilder is empty.
        /// </summary>
        public static string JsonWriter_EmptyStringBuilder
        {
            get { return GetString("JsonWriter_EmptyStringBuilder"); }
        }

        /// <summary>
        /// The source colums cannot be empty.
        /// </summary>
        public static string ValidationMessage_EmptySourceColumns
        {
            get { return GetString("ValidationMessage_EmptySourceColumns"); }
        }

        /// <summary>
        /// The messages cannot be empty.
        /// </summary>
        public static string ValidationEntry_EmptyMessages
        {
            get { return GetString("ValidationEntry_EmptyMessages"); }
        }

        /// <summary>
        /// The column name '{columnName}' is invalid.
        /// </summary>
        public static string Model_InvalidColumnName(object columnName)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("Model_InvalidColumnName", "columnName"), columnName);
        }

        /// <summary>
        /// This column is already a computed column.
        /// </summary>
        public static string Column_AlreadyComputed
        {
            get { return GetString("Column_AlreadyComputed"); }
        }

        /// <summary>
        /// Computed column must be member of column.
        /// </summary>
        public static string Column_ComputedColumnMustBeMemberOfModel
        {
            get { return GetString("Column_ComputedColumnMustBeMemberOfModel"); }
        }

        /// <summary>
        /// Computation is not allowed for child column {columnName}.
        /// </summary>
        public static string Column_ComputationNotAllowedForChildColumn(object columnName)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("Column_ComputationNotAllowedForChildColumn", "columnName"), columnName);
        }

        /// <summary>
        /// The column has invalid scalar source model: "{0}".
        /// </summary>
        public static string DbQueryBuilder_InvalidScalarSourceModel(object p0)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("DbQueryBuilder_InvalidScalarSourceModel"), p0);
        }

        /// <summary>
        /// The column's ScalarSourceModels is empty.
        /// </summary>
        public static string Column_EmptyScalarSourceModels
        {
            get { return GetString("Column_EmptyScalarSourceModels"); }
        }

        /// <summary>
        /// The column contains invalid aggregate source model: "{0}".
        /// </summary>
        public static string DbQueryBuilder_InvalidAggregateSourceModel(object p0)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("DbQueryBuilder_InvalidAggregateSourceModel"), p0);
        }

        /// <summary>
        /// Circular computation detected for column {columnName}.
        /// </summary>
        public static string ComputationManager_CircularComputation(object columnName)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("ComputationManager_CircularComputation", "columnName"), columnName);
        }

        /// <summary>
        /// Update in DataRowChanged event handler is not allowed.
        /// </summary>
        public static string DataRow_UpdateInDataRowChangedEventNotAllowed
        {
            get { return GetString("DataRow_UpdateInDataRowChangedEventNotAllowed"); }
        }

        /// <summary>
        /// SuspendComputation and ResumeComputation must be called in tandem.
        /// </summary>
        public static string DataSetContainer_ResumeComputationWithoutSuspendComputation
        {
            get { return GetString("DataSetContainer_ResumeComputationWithoutSuspendComputation"); }
        }

        /// <summary>
        /// The expression must be static.
        /// </summary>
        public static string DataSetContainer_InvalidLocalColumnExpression
        {
            get { return GetString("DataSetContainer_InvalidLocalColumnExpression"); }
        }

        /// <summary>
        /// DataSetContainer does not contain this model.
        /// </summary>
        public static string DataSetContainer_InvalidLocalColumnModel
        {
            get { return GetString("DataSetContainer_InvalidLocalColumnModel"); }
        }

        private static string GetString(string name, params string[] formatterNames)
        {
            var value = _resourceManager.GetString(name);

            Debug.Assert(value != null);

            if (formatterNames != null)
            {
                for (var i = 0; i < formatterNames.Length; i++)
                {
                    value = value.Replace("{" + formatterNames[i] + "}", "{" + i + "}");
                }
            }

            return value;
        }
    }
}
