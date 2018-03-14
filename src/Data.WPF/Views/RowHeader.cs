﻿using DevZest.Data.Presenters.Primitives;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using DevZest.Data.Presenters;
using System;
using System.Windows.Input;
using System.Linq;
using System.Diagnostics;
using System.Collections.Generic;
using System.Windows.Controls;

namespace DevZest.Data.Views
{
    internal interface IRowHeader
    {
    }

    [TemplateVisualState(GroupName = VisualStates.GroupCommon, Name = VisualStates.StateNormal)]
    [TemplateVisualState(GroupName = VisualStates.GroupCommon, Name = VisualStates.StateMouseOver)]
    [TemplateVisualState(GroupName = VisualStates.GroupSelection, Name = VisualStates.StateSelected)]
    [TemplateVisualState(GroupName = VisualStates.GroupSelection, Name = VisualStates.StateUnselected)]
    [TemplateVisualState(GroupName = VisualStates.GroupRowIndicator, Name = VisualStates.StateRegularRow)]
    [TemplateVisualState(GroupName = VisualStates.GroupRowIndicator, Name = VisualStates.StateCurrentRow)]
    [TemplateVisualState(GroupName = VisualStates.GroupRowIndicator, Name = VisualStates.StateCurrentEditingRow)]
    [TemplateVisualState(GroupName = VisualStates.GroupRowIndicator, Name = VisualStates.StateNewRow)]
    [TemplateVisualState(GroupName = VisualStates.GroupRowIndicator, Name = VisualStates.StateNewCurrentRow)]
    [TemplateVisualState(GroupName = VisualStates.GroupRowIndicator, Name = VisualStates.StateNewEditingRow)]
    public class RowHeader : ButtonBase, IRowElement, IRowHeader
    {
        private interface IFocusTracker : IService
        {
        }

        private sealed class FocusTracker : IFocusTracker
        {
            private DataPresenter _dataPresenter;
            public DataPresenter DataPresenter
            {
                get { return _dataPresenter; }
            }

            private DataView _dataView;
            private DataView DataView
            {
                get { return _dataView; }
                set
                {
                    if (_dataView == value)
                        return;

                    if (_dataView != null)
                        _dataView.GotKeyboardFocus -= OnGotKeyboardFocus;
                    _dataView = value;
                    _activeHeader = null;
                    if (_dataView != null)
                        _dataView.GotKeyboardFocus += OnGotKeyboardFocus;
                }
            }

            private IRowHeader _activeHeader;
            private IRowHeader ActiveHeader
            {
                get { return _activeHeader; }
                set
                {
                    if (_activeHeader == value)
                        return;
                    if (_activeHeader != null && value == null)
                        DeselectAll();
                    _activeHeader = value;
                }
            }

            private void OnGotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
            {
                ActiveHeader = FindAncestorRowHeader(e.OriginalSource as DependencyObject);
            }

            private static IRowHeader FindAncestorRowHeader(DependencyObject child)
            {
                if (child == null)
                    return null;

                if (child is IRowHeader)
                    return (IRowHeader)child;

                DependencyObject parentObject = VisualTreeHelper.GetParent(child);
                if (parentObject == null)
                    return null;

                var parent = parentObject as IRowHeader;
                return parent ?? FindAncestorRowHeader(parentObject);
            }

            private void DeselectAll()
            {
                var rows = DataPresenter.SelectedRows.ToArray();
                foreach (var row in rows)
                    row.IsSelected = false;
            }

            public void Initialize(DataPresenter dataPresenter)
            {
                _dataPresenter = dataPresenter;
                DataView = dataPresenter.View;
                _dataPresenter.ViewChanged += OnViewChanged;
            }

            private void OnViewChanged(object sender, EventArgs e)
            {
                DataView = DataPresenter.View;
            }
        }

        internal static void EnsureFocusTrackerInitialized(DataPresenter dataPresenter)
        {
            if (!ServiceManager.IsRegistered<IFocusTracker>())
                ServiceManager.Register<IFocusTracker, FocusTracker>();
            var service = ServiceManager.GetService<IFocusTracker>(dataPresenter);
            Debug.Assert(service != null);
        }

        public abstract class Commands
        {
            public static RoutedUICommand DeleteSelected { get { return ApplicationCommands.Delete; } }
        }

        public interface ICommandService : IService
        {
            IEnumerable<CommandEntry> GetCommandEntries(RowHeader rowHeader);
        }

        public interface IDeletingConfirmation : IService
        {
            bool Confirm();
        }

        private sealed class CommandService : ICommandService
        {
            public DataPresenter DataPresenter { get; private set; }

            public void Initialize(DataPresenter dataPresenter)
            {
                DataPresenter = dataPresenter;
            }

            public IEnumerable<CommandEntry> GetCommandEntries(RowHeader rowHeader)
            {
                yield return Commands.DeleteSelected.Bind(ExecDeleteSelected, CanExecDeleteSelected, new KeyGesture(Key.Delete));
            }

            private void CanExecDeleteSelected(object sender, CanExecuteRoutedEventArgs e)
            {
                e.CanExecute = DataPresenter.SelectedRows.Count > 0;
            }

            private void ExecDeleteSelected(object sender, ExecutedRoutedEventArgs e)
            {
                var confirmService = DataPresenter.GetService<IDeletingConfirmation>();
                var confirmed = confirmService == null ? true : confirmService.Confirm();
                if (confirmed)
                {
                    foreach (var row in DataPresenter.SelectedRows.ToArray())
                        row.Delete();
                }
                e.Handled = true;
            }
        }

        public abstract class Styles
        {
            public static readonly StyleId Flat = new StyleId(typeof(RowHeader));
        }

        private static readonly DependencyPropertyKey IsSelectedPropertyKey = DependencyProperty.RegisterReadOnly(nameof(IsSelected), typeof(bool),
            typeof(RowHeader), new FrameworkPropertyMetadata(BooleanBoxes.False));
        public static readonly DependencyProperty IsSelectedProperty = IsSelectedPropertyKey.DependencyProperty;

        public static readonly DependencyProperty SeparatorBrushProperty = DependencyProperty.Register(nameof(SeparatorBrush), typeof(Brush),
            typeof(RowHeader), new FrameworkPropertyMetadata(null));

        public static readonly DependencyProperty SeparatorVisibilityProperty = DependencyProperty.Register(nameof(SeparatorVisibility), typeof(Visibility),
            typeof(RowHeader), new FrameworkPropertyMetadata(Visibility.Visible));

        static RowHeader()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RowHeader), new FrameworkPropertyMetadata(typeof(RowHeader)));
            ServiceManager.Register<ICommandService, CommandService>();
        }

        public RowHeader()
        {
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            var rowPresenter = RowView.GetCurrent(this).RowPresenter;
            UpdateVisualStates(rowPresenter);
        }

        public bool IsSelected
        {
            get { return (bool)GetValue(IsSelectedProperty); }
            private set { SetValue(IsSelectedPropertyKey, BooleanBoxes.Box(value)); }
        }

        public Brush SeparatorBrush
        {
            get { return (Brush)GetValue(SeparatorBrushProperty); }
            set { SetValue(SeparatorBrushProperty, value); }
        }

        public Visibility SeparatorVisibility
        {
            get { return (Visibility)GetValue(SeparatorVisibilityProperty); }
            set { SetValue(SeparatorVisibilityProperty, value); }
        }

        void IRowElement.Setup(RowPresenter p)
        {
            var dataPresenter = p.DataPresenter;
            EnsureFocusTrackerInitialized(dataPresenter);
            this.SetupCommandEntries(dataPresenter.GetService<ICommandService>().GetCommandEntries(this));
        }

        void IRowElement.Refresh(RowPresenter p)
        {
            UpdateVisualStates(p);
        }

        void IRowElement.Cleanup(RowPresenter p)
        {
            this.CleanupCommandEntries();
        }

        private void UpdateVisualStates(RowPresenter p)
        {
            UpdateVisualStates(p, true);
        }

        private void UpdateVisualStates(RowPresenter p, bool useTransitions)
        {
            if (!IsLoaded)
                return;

            if (p.IsSelected)
            {
                IsSelected = true;
                VisualStates.GoToState(this, useTransitions, VisualStates.StateSelected);
            }
            else
            {
                IsSelected = false;
                VisualStates.GoToState(this, useTransitions, VisualStates.StateUnselected);
            }

            if (p.IsVirtual)
            {
                if (p.IsEditing)
                    VisualStates.GoToState(this, useTransitions, VisualStates.StateNewEditingRow);
                else if (p.IsCurrent)
                    VisualStates.GoToState(this, useTransitions, VisualStates.StateNewCurrentRow);
                else
                    VisualStates.GoToState(this, useTransitions, VisualStates.StateNewRow);
            }
            else if (p.IsCurrent)
            {
                if (p.IsEditing)
                    VisualStates.GoToState(this, useTransitions, VisualStates.StateCurrentEditingRow);
                else
                    VisualStates.GoToState(this, useTransitions, VisualStates.StateCurrentRow);
            }
            else
                VisualStates.GoToState(this, useTransitions, VisualStates.StateRegularRow);
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            if (!e.Handled)
                HandleMouseButtonDown(MouseButton.Left);
            base.OnMouseLeftButtonDown(e);
        }

        protected override void OnMouseRightButtonDown(MouseButtonEventArgs e)
        {
            if (!e.Handled)
                HandleMouseButtonDown(MouseButton.Right);
            base.OnMouseRightButtonDown(e);
        }

        private RowView RowView
        {
            get { return RowView.GetCurrent(this); }
        }

        private RowPresenter RowPresenter
        {
            get { return RowView?.RowPresenter; }
        }

        private DataPresenter DataPresenter
        {
            get { return RowPresenter?.DataPresenter; }
        }

        private void HandleMouseButtonDown(MouseButton mouseButton)
        {
            var dataPresenter = DataPresenter;
            if (dataPresenter == null)
                return;

            dataPresenter.Select(RowPresenter, mouseButton, () =>
            {
                if (!IsKeyboardFocusWithin)
                    Focus();
            });
        }
    }
}
