﻿using System.Windows;
using System.Windows.Controls;

namespace DevZest.Data.Windows
{
    public sealed class GridRow : GridDefinition
    {
        internal GridRow(GridTemplate owner, int ordinal, GridLength height)
            : base(owner, ordinal, height)
        {
        }

        public GridLength Height
        {
            get { return Length; }
        }
    }
}
