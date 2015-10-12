﻿using DevZest.Data.Primitives;
using System;
using System.Diagnostics;
using System.Windows;

namespace DevZest.Data.Wpf
{
    public abstract class ColumnViewGenerator<T> : ViewGenerator<T>
        where T : UIElement, new()
    {
        internal ColumnViewGenerator(Column column, Action<T> initializer)
            : base(initializer)
        {
            Debug.Assert(column != null);
            Column = column;
        }

        public Column Column { get; private set; }

        internal sealed override bool IsValidFor(Model model)
        {
            return Column.GetParentModel() == model;
        }
    }
}
