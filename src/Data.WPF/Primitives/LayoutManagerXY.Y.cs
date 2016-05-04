﻿using System;
using System.Collections.Generic;
using System.Windows;

namespace DevZest.Data.Windows.Primitives
{
    partial class LayoutManagerXY
    {
        private sealed class Y : LayoutManagerXY
        {
            public Y(Template template, DataSet dataSet)
                : base(template.InternalGridRows, dataSet)
            {
            }

            protected override Vector BlockDimensionVector
            {
                get { return new Vector(0, Template.RowRange.MeasuredHeight); }
            }
        }
    }
}
