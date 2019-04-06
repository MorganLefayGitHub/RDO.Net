﻿using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;

namespace DevZest.Data.Presenters.Primitives
{
    public struct SerializableSelection
    {
        public SerializableSelection(IReadOnlyList<RowPresenter> rows, IReadOnlyList<ColumnSerializer> columnSerializers)
        {
            Rows = rows.VerifyNotNull(nameof(rows)).VerifyNoNullItem(nameof(rows));
            ColumnSerializers = columnSerializers.VerifyNotNull(nameof(columnSerializers)).VerifyNoNullItem(nameof(columnSerializers));
        }

        public readonly IReadOnlyList<RowPresenter> Rows;
        public readonly IReadOnlyList<ColumnSerializer> ColumnSerializers;

        public bool IsEmpty
        {
            get { return IsEmptyList(Rows) || IsEmptyList(ColumnSerializers); }
        }

        private static bool IsEmptyList<T>(IReadOnlyList<T> list)
        {
            return list == null || list.Count == 0;
        }

        public bool CanCopyToClipboard
        {
            get { return !IsEmpty; }
        }

        public void CopyToClipboard(bool includeColumnNames, bool copy)
        {
            if (!CanCopyToClipboard)
                return;

            var tabDelimitedText = Serialize(includeColumnNames, TabularText.TabDelimiter);
            var commaDelimitedText = Serialize(includeColumnNames, TabularText.CommaDelimiter);
            var dataObject = new DataObject();
            dataObject.SetText(tabDelimitedText, TextDataFormat.UnicodeText);
            dataObject.SetText(commaDelimitedText, TextDataFormat.CommaSeparatedValue);
            Clipboard.SetDataObject(dataObject, copy);
        }

        private string Serialize(bool includeColumnNames, char delimiter)
        {
            if (IsEmpty)
                return null;

            var result = new StringBuilder();
            if (includeColumnNames)
                SerializeColumnNames(result, delimiter);

            for (int i = 0; i < Rows.Count; i++)
            {
                var row = Rows[i];
                for (int j = 0; j < ColumnSerializers.Count; j++)
                {
                    var columnSerializer = ColumnSerializers[j];
                    var s = columnSerializer.Serialize(row);
                    TabularText.Format(s, result, delimiter);
                    var isLast = j == ColumnSerializers.Count - 1;
                    if (!isLast)
                        result.Append(delimiter);
                }
                result.AppendLine();
            }
            return result.ToString();
        }

        private void SerializeColumnNames(StringBuilder output, char delimiter)
        {
            for (int i = 0; i < ColumnSerializers.Count; i++)
            {
                var column = ColumnSerializers[i].Column;
                TabularText.Format(column.DisplayName, output, delimiter);
                var isLast = i == ColumnSerializers.Count - 1;
                if (!isLast)
                    output.Append(delimiter);
            }
            output.AppendLine();
        }
    }
}