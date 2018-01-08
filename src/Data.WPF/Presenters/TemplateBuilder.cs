﻿using DevZest.Data.Presenters.Primitives;
using DevZest.Data.Primitives;
using System;
using System.Diagnostics;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Linq;
using DevZest.Data.Views;
using System.Windows.Input;

namespace DevZest.Data.Presenters
{
    public sealed class TemplateBuilder : IDisposable
    {
        internal TemplateBuilder(Template template, Model model, bool inherited = false)
        {
            Debug.Assert(template != null);
            Template = template;
            _model = model;
            _inherited = inherited;
            if (inherited)
                template.InitializeAsInherited();
        }

        private readonly bool _inherited;

        internal void Seal()
        {
            Template.Seal();
        }

        public void Dispose()
        {
            Template = null;
        }

        internal Template Template { get; private set; }

        private Model _model;

        public TemplateBuilder GridColumns(params string[] widths)
        {
            if (_inherited)
                return this;

            if (widths == null)
                throw new ArgumentNullException(nameof(widths));

            Template.AddGridColumns(widths);
            return this;
        }

        public TemplateBuilder GridRows(params string[] heights)
        {
            if (_inherited)
                return this;

            if (heights == null)
                throw new ArgumentNullException(nameof(heights));

            Template.AddGridRows(heights);
            return this;
        }

        public TemplateBuilder RowRange(int left, int top, int right, int bottom)
        {
            if (_inherited)
                return this;

            Template.RowRange = Template.Range(left, top, right, bottom);
            return this;
        }

        public TemplateBuilder Layout(Orientation orientation, int flowRepeatCount = 1)
        {
            if (_inherited)
                return this;

            if (flowRepeatCount < 0)
                throw new ArgumentOutOfRangeException(nameof(flowRepeatCount));

            Template.Layout(orientation, flowRepeatCount);
            return this;
        }

        [DefaultValue(0)]
        public TemplateBuilder WithFrozenLeft(int tracks)
        {
            if (_inherited)
                return this;

            if (tracks < 0)
                throw new ArgumentOutOfRangeException(nameof(tracks));
            Template.FrozenLeft = tracks;
            return this;
        }

        [DefaultValue(0)]
        public TemplateBuilder WithFrozenTop(int tracks)
        {
            if (_inherited)
                return this;

            if (tracks < 0)
                throw new ArgumentOutOfRangeException(nameof(tracks));
            Template.FrozenTop = tracks;
            return this;
        }

        [DefaultValue(0)]
        public TemplateBuilder WithFrozenRight(int tracks)
        {
            if (_inherited)
                return this;

            if (tracks < 0)
                throw new ArgumentOutOfRangeException(nameof(tracks));
            Template.FrozenRight = tracks;
            return this;
        }

        [DefaultValue(0)]
        public TemplateBuilder WithFrozenBottom(int tracks)
        {
            if (_inherited)
                return this;

            if (tracks < 0)
                throw new ArgumentOutOfRangeException(nameof(tracks));
            Template.FrozenBottom = tracks;
            return this;
        }

        [DefaultValue(0)]
        public TemplateBuilder WithStretches(int tracks)
        {
            if (_inherited)
                return this;

            if (tracks < 0)
                throw new ArgumentOutOfRangeException(nameof(tracks));
            Template.Stretches = tracks;
            return this;
        }

        [DefaultValue(VirtualRowPlacement.Explicit)]
        public TemplateBuilder WithVirtualRowPlacement(VirtualRowPlacement value)
        {
            if (_inherited)
                return this;

            Template.VirtualRowPlacement = value;
            return this;
        }

        [DefaultValue(null)]
        public TemplateBuilder WithSelectionMode(SelectionMode value)
        {
            if (_inherited)
                return this;

            Template.SelectionMode = value;
            return this;
        }

        public TemplateBuilder AddAsyncValidator(Func<Task<IScalarValidationMessages>> action, Action postAction = null, IScalars sourceScalars = null)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            Template.InternalScalarAsyncValidators = Template.InternalScalarAsyncValidators.Add(ScalarAsyncValidator.Create(Template, sourceScalars, action, postAction));
            return this;
        }

        public TemplateBuilder AddAsyncValidator(Func<Task<IColumnValidationMessages>> action, Action postAction = null, IColumns sourceColumns = null)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            Template.InternalRowAsyncValidators = Template.InternalRowAsyncValidators.Add(RowAsyncValidator.Create(Template, sourceColumns, action, postAction));
            return this;
        }

        [DefaultValue(ValidationMode.Progressive)]
        public TemplateBuilder WithRowValidationMode(ValidationMode value)
        {
            if (_inherited)
                return this;

            Template.RowValidationMode = value;
            return this;
        }

        [DefaultValue(ValidationMode.Progressive)]
        public TemplateBuilder WithScalarValidationMode(ValidationMode value)
        {
            if (_inherited)
                return this;

            Template.ScalarValidationMode = value;
            return this;
        }

        public TemplateBuilder MakeRecursive(Model childModel)
        {
            if (_inherited)
                return this;

            if (childModel == null)
                throw new ArgumentNullException(nameof(childModel));

            if (childModel.GetParentModel() != _model || childModel.GetType() != _model.GetType())
                throw new ArgumentException(DiagnosticMessages.TemplateBuilder_InvalidRecursiveChildModel);

            Template.RecursiveModelOrdinal = childModel.GetOrdinal();
            return this;
        }

        public TemplateBuilder BlockView<T>(Style style = null)
            where T : BlockView, new()
        {
            if (_inherited)
                return this;

            Template.BlockView<T>(style);
            return this;
        }

        public TemplateBuilder BlockView<T>(StyleId styleId)
            where T : BlockView, new()
        {
            if (_inherited)
                return this;

            if (styleId == null)
                throw new ArgumentNullException(nameof(styleId));
            return BlockView<T>(styleId.GetOrLoad());
        }

        public TemplateBuilder RowView<T>(Style style = null)
            where T : RowView, new()
        {
            if (_inherited)
                return this;

            Template.RowView<T>(style);
            return this;
        }

        public TemplateBuilder RowView<T>(StyleId styleId)
            where T : RowView, new()
        {
            if (_inherited)
                return this;

            if (styleId == null)
                throw new ArgumentNullException(nameof(styleId));
            return RowView<T>(styleId.GetOrLoad());
        }

        public TemplateBuilder AddBinding(int column, int row, ScalarBinding scalarBinding)
        {
            return AddBinding(column, row, column, row, scalarBinding);
        }

        public TemplateBuilder AddBinding(int left, int top, int right, int bottom, ScalarBinding scalarBinding)
        {
            Binding.VerifyAdding(scalarBinding, nameof(scalarBinding));
            Template.AddBinding(Template.Range(left, top, right, bottom), scalarBinding);
            return this;
        }

        public TemplateBuilder AddBinding(int column, int row, BlockBinding blockBinding)
        {
            return AddBinding(column, row, column, row, blockBinding);
        }

        public TemplateBuilder AddBinding(int left, int top, int right, int bottom, BlockBinding blockBinding)
        {
            Binding.VerifyAdding(blockBinding, nameof(blockBinding));
            Template.AddBinding(Template.Range(left, top, right, bottom), blockBinding);
            return this;
        }

        public TemplateBuilder AddBinding(int column, int row, RowBinding rowBinding)
        {
            return AddBinding(column, row, column, row, rowBinding);
        }

        public TemplateBuilder AddBinding(int left, int top, int right, int bottom, RowBinding rowBinding)
        {
            Binding.VerifyAdding(rowBinding, nameof(rowBinding));
            Template.AddBinding(Template.Range(left, top, right, bottom), rowBinding);
            return this;
        }

        public TemplateBuilder GridLineX(GridPoint startGridPoint, int length, Pen pen = null, GridPlacement? placement = null)
        {
            if (_inherited)
                return this;

            return GridLine(Orientation.Horizontal, startGridPoint, length, pen, placement);
        }

        public TemplateBuilder GridLineY(GridPoint startGridPoint, int length, Pen pen = null, GridPlacement? placement = null)
        {
            if (_inherited)
                return this;

            return GridLine(Orientation.Vertical, startGridPoint, length, pen, placement);
        }

        private static readonly Pen DefaultGridLinePen = GetDefaultGridLinePen();
        private static Pen GetDefaultGridLinePen()
        {
            var result = new Pen(Brushes.LightGray, 1);
            result.Freeze();
            return result;
        }

        private TemplateBuilder GridLine(Orientation orientation, GridPoint startGridPoint, int length, Pen pen, GridPlacement? placement)
        {
            IReadOnlyList<GridTrack> gridTracks;
            if (orientation == Orientation.Horizontal)
                gridTracks = Template.GridColumns;
            else
                gridTracks = Template.GridRows;

            if (startGridPoint.X > Template.GridColumns.Count || startGridPoint.Y > Template.GridRows.Count)
                throw new ArgumentOutOfRangeException(nameof(startGridPoint));

            if (length <= 0)
                throw new ArgumentOutOfRangeException(nameof(length));

            if (pen == null)
                pen = DefaultGridLinePen;

            int endGridPointX = startGridPoint.X;
            int endGridPointY = startGridPoint.Y;
            if (orientation == Orientation.Horizontal)
            {
                endGridPointX += length;
                if (endGridPointX > Template.GridColumns.Count)
                    throw new ArgumentOutOfRangeException(nameof(length));
            }
            else
            {
                endGridPointY += length;
                if (endGridPointY > Template.GridRows.Count)
                    throw new ArgumentOutOfRangeException(nameof(length));
            }

            var gridLine = new GridLine(startGridPoint, new GridPoint(endGridPointX, endGridPointY), pen, placement);
            Template.AddGridLine(gridLine);
            return this;
        }

        public TemplateBuilder AddBehavior(BlockViewBehavior behavior)
        {
            if (_inherited)
                return this;

            if (behavior == null)
                throw new ArgumentNullException(nameof(behavior));

            Template.AddBehavior(behavior);
            return this;
        }

        public TemplateBuilder AddBehavior(RowViewBehavior behavior)
        {
            if (_inherited)
                return this;

            if (behavior == null)
                throw new ArgumentNullException(nameof(behavior));

            Template.AddBehavior(behavior);
            return this;
        }

        public TemplateBuilder WithRowViewToggleEditGestures(params InputGesture[] gestures)
        {
            Template.SetRowViewToggleEditGestures(gestures);
            return this;
        }

        public TemplateBuilder WithRowViewBeginEditGestures(params InputGesture[] gestures)
        {
            Template.SetRowViewBeginEditGestures(gestures);
            return this;
        }

        public TemplateBuilder WithRowViewCancelEditGestures(params InputGesture[] gestures)
        {
            Template.SetRowViewCancelEditGestures(gestures);
            return this;
        }

        public TemplateBuilder WithRowViewEndEditGestures(params InputGesture[] gestures)
        {
            Template.SetRowViewEndEditGestures(gestures);
            return this;
        }

        public TemplateBuilder WithInitialFocus(InitialFocus initialFocus)
        {
            Check.NotNull(initialFocus, nameof(initialFocus));
            Template.InitialFocus = initialFocus;
            return this;
        }
    }
}
