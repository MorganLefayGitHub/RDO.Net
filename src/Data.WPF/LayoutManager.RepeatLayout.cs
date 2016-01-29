﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Controls;

namespace DevZest.Data.Windows
{
    partial class LayoutManager
    {
        private abstract class RepeatLayout : LayoutManager
        {
            public static new LayoutManager Create(DataView view)
            {
                var orientation = view.Template.RepeatOrientation;
                if (orientation == RepeatOrientation.Y)
                    return new LayoutY(view);
                else if (orientation == RepeatOrientation.XY)
                    return new LayoutXY(view);
                else if (orientation == RepeatOrientation.X)
                    return new LayoutX(view);
                else
                {
                    Debug.Assert(orientation == RepeatOrientation.YX);
                    return new LayoutYX(view);
                }
            }

            private sealed class LayoutXY : RepeatLayout
            {
                public LayoutXY(DataView view)
                    : base(view)
                {
                }

                protected override int GetFlowCount()
                {
                    var gridColumns = Template.GridColumns;
                    var totalWidth = GetMeasuredLength(gridColumns[0], gridColumns[gridColumns.Count - 1]);
                    var repeatWidth = GetMeasuredLength(Template.RepeatRange.Left, Template.RepeatRange.Right);
                    return 1 + (int)(Math.Max(0d, _availableSize.Width - totalWidth) / repeatWidth);
                }
            }

            private sealed class LayoutY : RepeatLayout
            {
                public LayoutY(DataView view)
                    : base(view)
                {
                }
            }

            private sealed class LayoutYX : RepeatLayout
            {
                public LayoutYX(DataView view)
                    : base(view)
                {
                }

                public override Orientation GrowOrientation
                {
                    get { return Orientation.Horizontal; }
                }

                protected override int GetFlowCount()
                {
                    var gridRows = Template.GridRows;
                    var totalHeight = GetMeasuredLength(gridRows[0], gridRows[gridRows.Count - 1]);
                    var repeatHeight = GetMeasuredLength(Template.RepeatRange.Top, Template.RepeatRange.Bottom);
                    return 1 + (int)(Math.Max(0d, _availableSize.Height - totalHeight) / repeatHeight);
                }
            }

            private sealed class LayoutX : RepeatLayout
            {
                public LayoutX(DataView view)
                    : base(view)
                {
                }

                public override Orientation GrowOrientation
                {
                    get { return Orientation.Horizontal; }
                }
            }

            public RepeatLayout(DataView view)
                : base(view)
            {
            }

            private IList<double> _lengths;

            protected override void InitMeasure()
            {
                FlowCount = GetFlowCount();

                throw new NotImplementedException();
            }

            protected virtual int GetFlowCount()
            {
                return 1;
            }

            protected override double GetMeasuredLength(GridTrack gridTrack, int repeatIndex)
            {
                return IsVariantAuto(gridTrack) ? _lengths[GetVariantAutoIndex(gridTrack, repeatIndex)] : gridTrack.MeasuredLength;
            }

            protected override void SetMeasureLength(GridTrack gridTrack, int repeatIndex, double value)
            {
                if (IsVariantAuto(gridTrack))
                    _lengths[GetVariantAutoIndex(gridTrack, repeatIndex)] = value;
                else
                    gridTrack.MeasuredLength = value;
            }

            private bool IsVariantAuto(GridTrack gridTrack)
            {
                return gridTrack.AutoLengthIndex >= 0 && gridTrack.Orientation == GrowOrientation;
            }

            private GridTrack[] VariantAutoTracks
            {
                get
                {
                    if (GrowOrientation == Orientation.Vertical)
                        return _autoHeightRows;
                    else
                        return _autoWidthColumns;
                }
            }

            private int GetVariantAutoIndex(GridTrack gridTrack, int repeatIndex)
            {
                Debug.Assert(repeatIndex > 0 && repeatIndex < _realizedRows.Count);
                Debug.Assert(VariantAutoTracks.Length > 0);
                Debug.Assert(IsVariantAuto(gridTrack));
                return repeatIndex * VariantAutoTracks.Length + gridTrack.AutoLengthIndex;
            }
        }
    }
}
