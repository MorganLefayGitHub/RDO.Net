﻿using System;
using System.Diagnostics;

namespace DevZest.Data.Primitives
{
    public static class JsonDataRow
    {
        public static JsonWriter Write(this JsonWriter jsonWriter, DataRow dataRow)
        {
            return Write(jsonWriter, dataRow.DataSet, dataRow);
        }

        public static JsonWriter Write(this JsonWriter jsonWriter, DataRow dataRow, JsonView jsonView)
        {
            return Write(jsonWriter, jsonView, dataRow);
        }

        internal static JsonWriter Write(this JsonWriter jsonWriter, IJsonView jsonView, DataRow dataRow)
        {
            jsonWriter.WriteStartObject();

            var jsonFilter = jsonView.Filter;
            var _ = dataRow.Model;
            var columns = _.Columns;
            int count = 0;
            for (int i = 0; i < columns.Count; i++)
            {
                var column = columns[i];
                if (!column.ShouldSerialize)
                    continue;

                if (column.Kind == ColumnKind.ColumnListItem || column.Kind == ColumnKind.ProjectionMember)
                    continue;

                if (jsonFilter != null && !jsonFilter.ShouldSerialize(column))
                    continue;

                if (count > 0)
                    jsonWriter.WriteComma();
                jsonWriter.WriteObjectName(column.Name);
                jsonWriter.Write(dataRow, column);
                count++;
            }
            var columnLists = dataRow.Model.ColumnLists;
            for (int i =0; i < columnLists.Count; i++)
            {
                var columnList = columnLists[i];
                if (jsonFilter != null && !jsonFilter.ShouldSerialize(columnList))
                    continue;

                if (count > 0)
                    jsonWriter.WriteComma();

                jsonWriter.Write(dataRow, columnList, jsonFilter);
                count++;
            }

            var projections = dataRow.Model.Projections;
            for (int i = 0; i < projections.Count; i++)
            {
                var projection = projections[i];
                if (jsonFilter == null || jsonFilter.ShouldSerialize(projection))
                {
                    if (count > 0)
                        jsonWriter.WriteComma();
                    jsonWriter.Write(dataRow, projection, jsonFilter);
                    count++;
                }
            }

            var childDataSets = dataRow.ChildDataSets;
            for (int i = 0; i < childDataSets.Count; i++)
            {
                var dataSet = childDataSets[i];
                if (jsonFilter != null && !jsonFilter.ShouldSerialize(dataSet.Model))
                    continue;

                if (count > 0)
                    jsonWriter.WriteComma();
                var childJsonView = jsonView.GetChildView(dataSet);
                jsonWriter.WriteObjectName(dataSet.Model.Name).InternalWrite(childJsonView, dataSet);
                count++;
            }

            return jsonWriter.WriteEndObject();
        }

        private static void Write(this JsonWriter jsonWriter, DataRow dataRow, ColumnList columnList, JsonFilter jsonFilter)
        {
            jsonWriter.WriteObjectName(columnList.Name);
            jsonWriter.WriteStartArray();
            var count = 0;
            for (int i = 0; i < columnList.Count; i++)
            {
                var column = columnList[i];
                if (jsonFilter != null && !jsonFilter.ShouldSerialize(column))
                    continue;
                if (count > 0)
                    jsonWriter.WriteComma();
                jsonWriter.Write(dataRow, column);
                count++;
            }
            jsonWriter.WriteEndArray();
        }

        private static void Write(this JsonWriter jsonWriter, DataRow dataRow, Column column)
        {
            var dataSetColumn = column as IDataSetColumn;
            if (dataSetColumn != null)
                dataSetColumn.Serialize(dataRow.Ordinal, jsonWriter);
            else
                jsonWriter.WriteValue(column.Serialize(dataRow.Ordinal));
        }

        private static void Write(this JsonWriter jsonWriter, DataRow dataRow, Projection projection, JsonFilter jsonFilter)
        {
            if (string.IsNullOrEmpty(projection.Name))
                jsonWriter.WriteMembers(dataRow, projection, jsonFilter);
            else
                jsonWriter.WriteObject(dataRow, projection, jsonFilter);
        }

        private static void WriteObject(this JsonWriter jsonWriter, DataRow dataRow, Projection projection, JsonFilter jsonFilter)
        {
            Debug.Assert(!string.IsNullOrEmpty(projection.Name));
            jsonWriter.WriteObjectName(projection.Name);
            jsonWriter.WriteStartObject();
            jsonWriter.WriteMembers(dataRow, projection, jsonFilter);
            jsonWriter.WriteEndObject();
        }

        private static void WriteMembers(this JsonWriter jsonWriter, DataRow dataRow, Projection projection, JsonFilter jsonFilter)
        {
            var count = 0;
            var columns = projection.Columns;
            for (int i = 0; i < columns.Count; i++)
            {
                var column = columns[i];
                if (jsonFilter != null && !jsonFilter.ShouldSerialize(column))
                    continue;

                if (count > 0)
                    jsonWriter.WriteComma();
                jsonWriter.WriteObjectName(column.RelativeName);
                jsonWriter.Write(dataRow, column);
                count++;
            }
        }


        public static void Parse(this JsonReader jsonReader, DataRow dataRow)
        {
            var model = dataRow.Model;
            if (model.IsProjectionContainer)
            {
                Debug.Assert(model.Projections.Count == 1);
                var projection = model.Projections[0];
                jsonReader.Parse(projection, dataRow);
                return;
            }

            jsonReader.ExpectToken(JsonTokenKind.CurlyOpen);

            var token = jsonReader.PeekToken();
            if (token.Kind == JsonTokenKind.String)
            {
                jsonReader.ConsumeToken();
                jsonReader.Parse(dataRow, token.Text);

                while (jsonReader.PeekToken().Kind == JsonTokenKind.Comma)
                {
                    jsonReader.ConsumeToken();
                    token = jsonReader.ExpectToken(JsonTokenKind.String);
                    jsonReader.Parse(dataRow, token.Text);
                }
            }

            jsonReader.ExpectToken(JsonTokenKind.CurlyClose);
        }

        private static void Parse(this JsonReader jsonReader, DataRow dataRow, string memberName)
        {
            jsonReader.ExpectToken(JsonTokenKind.Colon);

            var model = dataRow.Model;

            var member = model[memberName];
            if (member == null)
                throw new FormatException(DiagnosticMessages.JsonParser_InvalidModelMember(memberName, model.GetType().FullName));
            if (member is Column column)
                jsonReader.Parse(column, dataRow.Ordinal);
            else if (member is ColumnList columnList)
                jsonReader.Parse(columnList, dataRow.Ordinal);
            else if (member is Projection projection)
                jsonReader.Parse(projection, dataRow);
            else if (member is Model childModel)
                jsonReader.Parse(dataRow[childModel], isTopLevel:false);
            else
                throw new FormatException(DiagnosticMessages.JsonParser_InvalidModelMember(memberName, model.GetType().FullName));
        }

        private static void Parse(this JsonReader jsonReader, Projection projection, DataRow dataRow)
        {
            jsonReader.ExpectToken(JsonTokenKind.CurlyOpen);
            var token = jsonReader.PeekToken();
            if (token.Kind == JsonTokenKind.String)
            {
                jsonReader.ConsumeToken();
                jsonReader.Parse(projection, token.Text, dataRow);

                while (jsonReader.PeekToken().Kind == JsonTokenKind.Comma)
                {
                    jsonReader.ConsumeToken();
                    token = jsonReader.ExpectToken(JsonTokenKind.String);
                    jsonReader.Parse(projection, token.Text, dataRow);
                }
            }
            jsonReader.ExpectToken(JsonTokenKind.CurlyClose);
        }

        private static void Parse(this JsonReader jsonReader, Projection projection, string memberName, DataRow dataRow)
        {
            jsonReader.ExpectToken(JsonTokenKind.Colon);
            if (projection.ColumnsByRelativeName.ContainsKey(memberName))
                jsonReader.Parse(projection.ColumnsByRelativeName[memberName], dataRow.Ordinal);
            else
                throw new FormatException(DiagnosticMessages.JsonParser_InvalidColumnGroupMember(memberName, projection.Name));
        }

        private static void Parse(this JsonReader jsonReader, Column column, int ordinal)
        {
            Debug.Assert(column != null);

            if (column is IDataSetColumn dataSetColumn)
            {
                var dataSet = jsonReader.Parse(() => dataSetColumn.NewValue(ordinal), false);
                if (column.ShouldSerialize)
                    dataSetColumn.Deserialize(ordinal, dataSet);
                return;
            }

            var token = jsonReader.ExpectToken(JsonTokenKind.ColumnValues);
            if (column.ShouldSerialize)
                column.Deserialize(ordinal, token.JsonValue);
        }

        private static void Parse(this JsonReader jsonReader, ColumnList columnList, int ordinal)
        {
            jsonReader.ExpectToken(JsonTokenKind.SquaredOpen);
            for (int i = 0; i < columnList.Count; i++)
            {
                jsonReader.Parse(columnList[i], ordinal);
                if (i < columnList.Count - 1)
                    jsonReader.ExpectComma();
            }
            jsonReader.ExpectToken(JsonTokenKind.SquaredClose);
        }
    }
}
