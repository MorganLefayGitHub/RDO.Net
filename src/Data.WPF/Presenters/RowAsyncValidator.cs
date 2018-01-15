﻿using DevZest.Data.Presenters.Primitives;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevZest.Data;
using System.Diagnostics.CodeAnalysis;

namespace DevZest.Data.Presenters
{
    public abstract class RowAsyncValidator : AsyncValidator<IRowValidationResults>, IRowAsyncValidators
    {
        internal static RowAsyncValidator Create<T>(RowInput<T> rowInput, Func<Task<IDataValidationErrors>> action, Action postAction)
            where T : UIElement, new()
        {
            return new RowInputAsyncValidator<T>(rowInput, action, postAction);
        }

        internal static RowAsyncValidator Create(Template template, IColumns sourceColumns, Func<Task<IDataValidationErrors>> action, Action postAction)
        {
            return new CurrentRowAsyncValidator(template, sourceColumns, action, postAction);
        }

        private static async Task<IRowValidationResults> Validate(Func<Task<IDataValidationErrors>> action, RowPresenter currentRow)
        {
            var messages = await action();
            return messages == null || messages.Count == 0 || currentRow == null
                ? RowValidationResults.Empty : RowValidationResults.Empty.Add(currentRow, messages);
        }

        private sealed class RowInputAsyncValidator<T> : RowAsyncValidator
            where T : UIElement, new()
        {
            public RowInputAsyncValidator(RowInput<T> rowInput, Func<Task<IDataValidationErrors>> action, Action postAction)
                : base(postAction)
            {
                Debug.Assert(rowInput != null);
                Debug.Assert(action != null);
                _rowInput = rowInput;
                _action = action;
            }

            private readonly RowInput<T> _rowInput;
            private readonly Func<Task<IDataValidationErrors>> _action;

            public override IColumns SourceColumns
            {
                get { return _rowInput.Target; }
            }

            internal override InputManager InputManager
            {
                get { return _rowInput.InputManager; }
            }

            private RowPresenter CurrentRow
            {
                get { return InputManager.CurrentRow; }
            }

            internal override IRowInput RowInput
            {
                get { return _rowInput; }
            }

            protected override async Task<IRowValidationResults> ValidateAsync()
            {
                return await Validate(_action, CurrentRow);
            }
        }

        private abstract class ColumnsAsyncValidator : RowAsyncValidator
        {
            protected ColumnsAsyncValidator(Template template, IColumns sourceColumns, Action postAction)
                : base(postAction)
            {
                Debug.Assert(template != null);
                _template = template;
                _sourceColumns = sourceColumns;
            }

            private readonly Template _template;
            private readonly IColumns _sourceColumns;

            internal override InputManager InputManager
            {
                get { return _template.InputManager; }
            }

            public override IColumns SourceColumns
            {
                get { return _sourceColumns; }
            }
        }

        private sealed class CurrentRowAsyncValidator : ColumnsAsyncValidator
        {
            public CurrentRowAsyncValidator(Template template, IColumns sourceColumns, Func<Task<IDataValidationErrors>> action, Action postAction)
                : base(template, sourceColumns, postAction)
            {
                Debug.Assert(action != null);
                _action = action;
            }

            private readonly Func<Task<IDataValidationErrors>> _action;

            private RowPresenter CurrentRow
            {
                get { return InputManager.CurrentRow; }
            }

            internal override IRowInput RowInput
            {
                get { return null; }
            }

            protected override async Task<IRowValidationResults> ValidateAsync()
            {
                return await Validate(_action, CurrentRow);
            }
        }

#if DEBUG
        public RowAsyncValidator()
            : base(null)
        {
        }
#endif

        private RowAsyncValidator(Action postAction)
            : base(postAction)
        {
        }

        public abstract IColumns SourceColumns { get; }

        private IRowValidationResults _errors = RowValidationResults.Empty;
        public IRowValidationResults Errors
        {
            get { return _errors; }
            private set
            {
                Debug.Assert(value != null && value.IsSealed);
                if (_errors == value)
                    return;
                _errors = value;
                RefreshHasError();
            }
        }

        private bool _hasError;
        public sealed override bool HasError
        {
            get { return _hasError; }
        }
        private void RefreshHasError()
        {
            var value = Errors.Count > 0;
            if (value == _hasError)
                return;
            _hasError = value;
        }

        internal abstract IRowInput RowInput { get; }

        protected sealed override void ClearValidationMessages()
        {
            Errors = RowValidationResults.Empty;
        }

        protected sealed override void RefreshValidationErrors(IRowValidationResults result)
        {
            Errors = result.Seal();
        }

        protected sealed override IRowValidationResults EmptyValidationResult
        {
            get { return RowValidationResults.Empty; }
        }

        internal void OnRowDisposed(RowPresenter rowPresenter)
        {
            if (Errors.ContainsKey(rowPresenter))
                Errors = Errors.Remove(rowPresenter);
        }

        internal void OnCurrentRowChanged()
        {
            Reset();
        }

        #region IRowAsyncValidators

        [SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes", Justification = "Child types will not call this method.")]
        bool IRowAsyncValidators.IsSealed
        {
            get { return true; }
        }

        [SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes", Justification = "Child types will not call this method.")]
        int IReadOnlyCollection<RowAsyncValidator>.Count
        {
            get { return 1; }
        }

        [SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes", Justification = "Child types will not call this method.")]
        RowAsyncValidator IReadOnlyList<RowAsyncValidator>.this[int index]
        {
            get
            {
                if (index != 0)
                    throw new ArgumentOutOfRangeException(nameof(index));
                return this;
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes", Justification = "Child types will not call this method.")]
        IRowAsyncValidators IRowAsyncValidators.Seal()
        {
            return this;
        }

        [SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes", Justification = "Child types will not call this method.")]
        IRowAsyncValidators IRowAsyncValidators.Add(RowAsyncValidator value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));
            return RowAsyncValidators.New(this, value);
        }

        [SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes", Justification = "Child types will not call this method.")]
        RowAsyncValidator IRowAsyncValidators.this[IColumns sourceColumns]
        {
            get { return sourceColumns == SourceColumns ? this : null; }
        }

        [SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes", Justification = "Child types will not call this method.")]
        IEnumerator<RowAsyncValidator> IEnumerable<RowAsyncValidator>.GetEnumerator()
        {
            yield return this;
        }

        [SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes", Justification = "Child types will not call this method.")]
        IEnumerator IEnumerable.GetEnumerator()
        {
            yield return this;
        }

#endregion
    }
}
