﻿using DevZest.Data;
using DevZest.Data.Views;
using DevZest.Data.Views.Primitives;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;

namespace DevZest.Data.Presenters.Primitives
{
    internal sealed class LayoutZManager : LayoutManager
    {
        internal LayoutZManager(LayoutManager inherit, Template template, DataSet dataSet, IReadOnlyList<Column> rowMatchColumns, Predicate<DataRow> where, IComparer<DataRow> orderBy)
            : base(inherit, template, dataSet, rowMatchColumns, where, orderBy, true)
        {
        }

        protected override void PrepareMeasureContainers()
        {
            if (CurrentContainerView != null)
                CurrentContainerView.Measure(Size.Empty);  // Available size is ignored when preparing blocks
        }

        protected override Point GetPosition(ScalarBinding scalarBinding, int flowIndex)
        {
            Debug.Assert(flowIndex == 0);
            return scalarBinding.GridRange.MeasuredPosition;
        }

        protected override Size GetSize(ScalarBinding scalarBinding)
        {
            return scalarBinding.GridRange.MeasuredSize;
        }

        internal override Thickness GetFrozenClip(ScalarBinding scalarBinding, int flowIndex)
        {
            return new Thickness();
        }

        protected override Point GetPosition(ContainerView containerView)
        {
            return Template.Range().GetRelativePosition(Template.ContainerRange);
        }

        protected override Size GetSize(ContainerView containerView)
        {
            return Template.ContainerRange.MeasuredSize;
        }

        internal override Thickness GetFrozenClip(ContainerView containerView)
        {
            return new Thickness();
        }

        protected override Point GetPosition(BlockView blockView, BlockBinding blockBinding)
        {
            return Template.ContainerRange.GetRelativePosition(blockBinding.GridRange);
        }

        protected override Size GetSize(BlockView blockView, BlockBinding blockBinding)
        {
            return blockBinding.GridRange.MeasuredSize;
        }

        internal override Thickness GetFrozenClip(BlockView blockView, BlockBinding blockBinding)
        {
            return new Thickness();
        }

        protected override Point GetPosition(BlockView blockView, int flowIndex)
        {
            Debug.Assert(flowIndex == 0);
            return Template.ContainerRange.GetRelativePosition(Template.RowRange);
        }

        protected override Size GetSize(BlockView blockView, int flowIndex)
        {
            return Template.RowRange.MeasuredSize;
        }

        internal override Thickness GetFrozenClip(int flowIndex)
        {
            return new Thickness();
        }

        protected override Point GetPosition(RowView rowView, RowBinding rowBinding)
        {
            return Template.RowRange.GetRelativePosition(rowBinding.GridRange);
        }

        protected override Size GetSize(RowView rowView, RowBinding rowBinding)
        {
            return rowBinding.GridRange.MeasuredSize;
        }

        internal override Thickness GetFrozenClip(RowView rowView, RowBinding rowBinding)
        {
            return new Thickness();
        }

        protected override Size MeasuredSize
        {
            get { return Template.Range().MeasuredSize; }
        }

        internal override IEnumerable<GridLineFigure> GridLineFigures
        {
            get
            {
                foreach (var gridLine in Template.GridLines)
                {
                    var startPoint = gridLine.StartGridPoint.ToPoint(Template);
                    var endPoint = gridLine.EndGridPoint.ToPoint(Template);
                    yield return new GridLineFigure(gridLine, startPoint, endPoint);
                }
            }
        }
    }
}
