﻿using System.Windows;
using System;
using System.Windows.Media;
using System.Collections;
using DevZest.Data.Presenters;
using DevZest.Data.Presenters.Primitives;
using System.Diagnostics;

namespace DevZest.Data.Views
{
    public class InPlaceEditor : FrameworkElement, IScalarElement, IRowElement
    {
        internal static RowBinding<InPlaceEditor> AddToInPlaceEditor<TEditing, TInert>(RowInput<TEditing> rowInput, RowBinding<TInert> inertRowBinding)
            where TEditing : UIElement, new()
            where TInert : UIElement, new()
        {
            var result = new RowBinding<InPlaceEditor>(null);
            result.Input = new ProxyRowInput<TEditing, TInert>(result, rowInput, inertRowBinding);
            return result;
        }

        private interface IProxyRowInput
        {
            RowBinding EditorBinding { get; }
            RowBinding InertBinding { get; }
        }

        private sealed class ProxyRowInput<TEditor, TInert> : RowInput<InPlaceEditor>, IRowValidation, IProxyRowInput
            where TEditor : UIElement, new()
            where TInert : UIElement, new()
        {
            private readonly RowInput<TEditor> _editorInput;
            private readonly RowBinding<TInert> _inertBinding;

            public ProxyRowInput(RowBinding<InPlaceEditor> binding, RowInput<TEditor> editorInput, RowBinding<TInert> inertBinding)
                : base(binding, new ExplicitTrigger<InPlaceEditor>(), null)
            {
                Debug.Assert(editorInput != null);
                Debug.Assert(inertBinding != null);
                _editorInput = editorInput;
                _inertBinding = inertBinding;
                _editorInput.InjectRowValidation(this);
                InertBinding.Seal(binding, 0);
                EditorBinding.Seal(binding, 1);
            }

            public RowBinding EditorBinding
            {
                get { return _editorInput.Binding; }
            }

            public RowBinding InertBinding
            {
                get { return _inertBinding; }
            }

            private IRowValidation BaseRowValidation
            {
                get { return InputManager.RowValidation; }
            }

            public ValidationInfo GetInfo(RowPresenter rowPresenter, Input<RowBinding, IColumns> input)
            {
                return input == this ? BaseRowValidation.GetInfo(rowPresenter, input) : ValidationInfo.Empty;
            }

            public bool HasError(RowPresenter rowPresenter, Input<RowBinding, IColumns> input, bool? blockingPrecedence)
            {
                return input == this ? BaseRowValidation.HasError(rowPresenter, input, blockingPrecedence) : false;
            }

            public void OnFlushed<T>(RowInput<T> rowInput, bool makeProgress, bool valueChanged) where T : UIElement, new()
            {
                BaseRowValidation.OnFlushed(this, makeProgress, valueChanged);
            }

            public override IColumns Target
            {
                get { return _editorInput.Target; }
            }

            internal override void Flush(InPlaceEditor element)
            {
                var editorElement = element.EditorElement;
                if (editorElement != null)
                    _editorInput.Flush((TEditor)editorElement);
            }

            bool IRowValidation.IsLockedByFlushingError(UIElement element)
            {
                var currentInPlaceEditor = GetCurrentInPlaceEditor(element);
                return currentInPlaceEditor != null ? BaseRowValidation.IsLockedByFlushingError(currentInPlaceEditor) : false;
            }

            void IRowValidation.SetFlushingError(UIElement element, string flushingErrorMessage)
            {
                var currentInPlaceEditor = GetCurrentInPlaceEditor(element);
                Debug.Assert(currentInPlaceEditor != null);
                BaseRowValidation.SetFlushingError(currentInPlaceEditor, flushingErrorMessage);
            }
        }

        public interface ISwitcher : IService
        {
            bool AffectsIsEditing(InPlaceEditor inPlaceEditor, DependencyProperty dp);
            bool GetIsEditing(InPlaceEditor inPlaceEditor);
        }

        private sealed class Switcher : ISwitcher
        {
            public DataPresenter DataPresenter { get; private set; }

            public void Initialize(DataPresenter dataPresenter)
            {
                DataPresenter = dataPresenter;
            }

            public bool AffectsIsEditing(InPlaceEditor inPlaceEditor, DependencyProperty dp)
            {
                return dp == IsMouseOverProperty || dp == IsKeyboardFocusWithinProperty;
            }

            public bool GetIsEditing(InPlaceEditor inPlaceEditor)
            {
                return inPlaceEditor.IsMouseOver || inPlaceEditor.IsKeyboardFocusWithin;
            }
        }

        private static readonly DependencyPropertyKey IsRowEditingPropertyKey = DependencyProperty.RegisterReadOnly(nameof(IsRowEditing), typeof(bool), typeof(InPlaceEditor),
            new FrameworkPropertyMetadata(BooleanBoxes.False));
        public static readonly DependencyProperty IsRowEditingProperty = IsRowEditingPropertyKey.DependencyProperty;

        static InPlaceEditor()
        {
            ServiceManager.Register<ISwitcher, Switcher>();
        }

        public bool IsRowEditing
        {
            get { return (bool)GetValue(IsRowEditingProperty); }
            private set { SetValue(IsRowEditingPropertyKey, BooleanBoxes.Box(value)); }
        }

        private bool _isEditing;
        public bool IsEditing
        {
            get { return _isEditing; }
            private set
            {
                if (_isEditing == value)
                    return;

                _isEditing = value;
                var proxyRowInput = GetProxyRowInput();
                if (proxyRowInput != null)
                    Setup(GetProxyRowInput(), RowPresenter);
            }
        }

        private void InitIsEditing()
        {
            var switcher = DataPresenter?.GetService<ISwitcher>();
            if (switcher != null)
                _isEditing = switcher.GetIsEditing(this);
        }

        private RowView RowView
        {
            get { return RowView.GetCurrent(this); }
        }

        private RowPresenter RowPresenter
        {
            get { return RowView?.RowPresenter; }
        }

        private UIElement _inertElement;
        public UIElement InertElement
        {
            get { return _inertElement;  }
            private set
            {
                if (_inertElement == value)
                    return;

                var oldValue = _inertElement;
                _inertElement = value;
                OnChildChanged(oldValue, value);
            }
        }

        private void OnChildChanged(UIElement oldValue, UIElement newValue)
        {
            if (oldValue != null)
            {
                RemoveLogicalChild(oldValue);
                RemoveVisualChild(oldValue);
            }
            if (newValue != null)
            {
                AddLogicalChild(newValue);
                AddVisualChild(newValue);
            }
        }

        private UIElement _editorElement;
        public UIElement EditorElement
        {
            get { return _editorElement; }
            private set
            {
                if (_editorElement == value)
                    return;

                var oldValue = _editorElement;
                _editorElement = value;
                OnChildChanged(oldValue, value);
            }
        }

        public UIElement Child
        {
            get { return IsEditing ? EditorElement : InertElement; }
        }

        protected override Visual GetVisualChild(int index)
        {
            if (index < 0 || index >= VisualChildrenCount)
                throw new ArgumentOutOfRangeException(nameof(index));

            return Child;
        }

        protected override IEnumerator LogicalChildren
        {
            get
            {
                var child = Child;
                return child == null ? EmptyEnumerator.Singleton : new SingleChildEnumerator(child);
            }
        }

        protected override int VisualChildrenCount
        {
            get { return Child == null ? 0 : 1; }
        }

        protected override Size MeasureOverride(Size constraint)
        {
            UIElement child = Child;
            if (child != null)
            {
                child.Measure(constraint);
                return child.DesiredSize;
            }
            return default(Size);
        }

        protected override Size ArrangeOverride(Size arrangeSize)
        {
            UIElement child = Child;
            if (child != null)
                child.Arrange(new Rect(arrangeSize));
            return arrangeSize;
        }

        private DataView DataView
        {
            get { return DataView.GetCurrent(this); }
        }

        private DataPresenter DataPresenter
        {
            get { return DataView?.DataPresenter; }
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            var switcher = DataPresenter?.GetService<ISwitcher>();
            if (switcher != null)
            {
                if (switcher.AffectsIsEditing(this, e.Property))
                    IsEditing = switcher.GetIsEditing(this);
            }
        }

        void IScalarElement.Setup(ScalarPresenter scalarPresenter)
        {
            throw new NotImplementedException();
        }

        void IScalarElement.Refresh(ScalarPresenter scalarPresenter)
        {
        }

        void IScalarElement.Cleanup(ScalarPresenter scalarPresenter)
        {
            throw new NotImplementedException();
        }

        void IRowElement.Setup(RowPresenter rowPresenter)
        {
            InitIsEditing();
            Setup(GetProxyRowInput(), rowPresenter);
        }

        private void Setup(IProxyRowInput proxyRowInput, RowPresenter rowPresenter)
        {
            if (proxyRowInput == null)
                return;

            if (IsEditing)
            {
                InertElement = null;
                EditorElement = GenerateElement(proxyRowInput.EditorBinding, rowPresenter);
            }
            else
            {
                EditorElement = null;
                InertElement = GenerateElement(proxyRowInput.InertBinding, rowPresenter);
            }
            InvalidateMeasure();
        }

        private static UIElement GenerateElement(RowBinding binding, RowPresenter p)
        {
            binding.BeginSetup(null);
            var result = binding.Setup(p);
            binding.EndSetup();
            binding.Refresh(result);
            return result;
        }

        void IRowElement.Refresh(RowPresenter rowPresenter)
        {
            IsRowEditing = rowPresenter.IsEditing;

            var proxyRowInput = GetProxyRowInput();
            if (proxyRowInput == null)
                return;

            if (EditorElement != null)
                proxyRowInput.EditorBinding.Refresh(EditorElement);
            else if (InertElement != null)
                proxyRowInput.InertBinding.Refresh(InertElement);
        }

        void IRowElement.Cleanup(RowPresenter rowPresenter)
        {
            var proxyRowInput = GetProxyRowInput();
            Cleanup(proxyRowInput);
        }

        private void Cleanup(IProxyRowInput proxyRowInput)
        {
            if (proxyRowInput == null)
                return;

            if (EditorElement != null)
            {
                proxyRowInput.EditorBinding.Cleanup(EditorElement);
                EditorElement = null;
            }
            else if (InertElement != null)
            {
                proxyRowInput.InertBinding.Cleanup(InertElement);
                InertElement = null;
            }
        }

        private static InPlaceEditor GetCurrentInPlaceEditor(UIElement element)
        {
            var result = element as InPlaceEditor;
            return result ?? VisualTreeHelper.GetParent(element) as InPlaceEditor;
        }

        private static bool IsInPlaceEditor(UIElement element)
        {
            return GetCurrentInPlaceEditor(element) == element;
        }

        private IProxyRowInput GetProxyRowInput()
        {
            return (this.GetBinding() as RowBinding)?.RowInput as IProxyRowInput;
        }
    }
}
