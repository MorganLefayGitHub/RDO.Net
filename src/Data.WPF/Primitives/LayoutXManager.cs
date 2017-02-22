﻿using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace DevZest.Data.Windows.Primitives
{
    internal sealed class LayoutXManager : LayoutScrollableManager
    {
        public LayoutXManager(Template template, DataSet dataSet, _Boolean where, ColumnSort[] orderBy)
            : base(template, dataSet, where, orderBy)
        {
        }

        internal override IGridTrackCollection GridTracksMain
        {
            get { return Template.InternalGridColumns; }
        }

        internal override IGridTrackCollection GridTracksCross
        {
            get { return Template.InternalGridRows; }
        }

        public override double ViewportWidth
        {
            get { return ViewportMain; }
        }

        public override double ViewportHeight
        {
            get { return ViewportCross; }
        }

        public override double ExtentWidth
        {
            get { return ExtentMain; }
        }

        public override double ExtentHeight
        {
            get { return ExtentCross; }
        }

        public override double HorizontalOffset
        {
            get { return ScrollOffsetMain; }
            set { ScrollOffsetMain = value; }
        }

        public override double VerticalOffset
        {
            get { return ScrollOffsetCross; }
            set { ScrollOffsetCross = value; }
        }

        protected override IEnumerable<LineFigure> GetLineFiguresX(int startGridOrdinalX, int endGridOrdinalX, GridLinePosition position, int gridOrdinalY)
        {
            return GetLineFiguresMain(startGridOrdinalX, endGridOrdinalX, position, gridOrdinalY);
        }

        protected override IEnumerable<LineFigure> GetLineFiguresY(int startGridOrdinalY, int endGridOrdinalY, GridLinePosition position, int gridOrdinalX)
        {
            return GetLineFiguresCross(startGridOrdinalY, endGridOrdinalY, position, gridOrdinalX);
        }
    }
}
