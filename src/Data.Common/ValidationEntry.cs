﻿using DevZest.Data.Primitives;
using System.Collections.Generic;
using System.Text;

namespace DevZest.Data
{
    public struct ValidationEntry
    {
        public ValidationEntry(DataRow dataRow, IReadOnlyList<ColumnValidationMessage> messages)
        {
            DataRow = dataRow;
            Messages = messages;
        }

        public readonly DataRow DataRow;

        public readonly IReadOnlyList<ColumnValidationMessage> Messages;
    }
}
