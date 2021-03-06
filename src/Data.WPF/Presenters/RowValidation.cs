﻿using DevZest.Data.Presenters.Primitives;
using DevZest.Data.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;

namespace DevZest.Data.Presenters
{
    internal interface IRowValidation
    {
        FlushingError GetFlushingError(UIElement element);
        void SetFlushingError(UIElement element, string flushingErrorMessage);
        void OnFlushed<T>(RowInput<T> rowInput, bool makeProgress, bool valueChanged) where T : UIElement, new();
        ValidationInfo GetInfo(RowPresenter rowPresenter, Input<RowBinding, IColumns> input);
        bool HasError(RowPresenter rowPresenter, Input<RowBinding, IColumns> input, bool? blockingPrecedence);
        bool IsLockedByFlushingError(UIElement element);
    }

    /// <summary>
    /// Contains row level validation logic.
    /// </summary>
    public sealed class RowValidation : IRowValidation
    {
        internal RowValidation(InputManager inputManager)
        {
            _inputManager = inputManager;
            InitInputs();
            if (Mode == ValidationMode.Progressive)
            {
                _progress = new Dictionary<RowPresenter, IColumns>();
                _valueChanged = new Dictionary<RowPresenter, IColumns>();
            }
            ValidateCurrentRowIfImplicit();
        }

        private readonly InputManager _inputManager;

        private DataPresenter DataPresenter
        {
            get { return _inputManager.DataPresenter; }
        }

        private DataSet DataSet
        {
            get { return _inputManager.DataSet; }
        }

        private void InvalidateView()
        {
            _inputManager.InvalidateView();
        }

        private Template Template
        {
            get { return _inputManager.Template; }
        }

        /// <summary>
        /// Gets the async validators.
        /// </summary>
        public IRowAsyncValidators AsyncValidators
        {
            get { return Template.RowAsyncValidators; }
        }

        private RowPresenter CurrentRow
        {
            get { return _inputManager.CurrentRow; }
        }

        private FlushingErrorCollection _flushingErrors;
        private FlushingErrorCollection InternalFlushingErrors
        {
            get
            {
                if (_flushingErrors == null)
                    _flushingErrors = new FlushingErrorCollection(_inputManager);
                return _flushingErrors;
            }
        }

        /// <summary>
        /// Gets the flushing errors.
        /// </summary>
        public IReadOnlyList<FlushingError> FlushingErrors
        {
            get
            {
                if (_flushingErrors == null)
                    return Array.Empty<FlushingError>();
                return _flushingErrors;
            }
        }

        internal FlushingError GetFlushingError(UIElement element)
        {
            return _flushingErrors.GetFlushingError(element);
        }

        private Dictionary<RowPresenter, IDataValidationErrors> _errorsByRow;
        private Dictionary<RowPresenter, IDataValidationErrors> ErrorsByRow
        {
            get { return _errorsByRow ?? (_errorsByRow = new Dictionary<RowPresenter, IDataValidationErrors>()); }
        }

        private Dictionary<RowPresenter, IDataValidationErrors> _asyncErrorsByRow;
        private Dictionary<RowPresenter, IDataValidationErrors> AsyncErrorsByRow
        {
            get { return _asyncErrorsByRow ?? (_asyncErrorsByRow = new Dictionary<RowPresenter, IDataValidationErrors>()); }
        }

        /// <summary>
        /// Gets the validaiton errors.
        /// </summary>
        public IReadOnlyDictionary<RowPresenter, IDataValidationErrors> Errors
        {
            get { return ErrorsByRow; }
        }

        private static IDataValidationErrors GetErrors(Dictionary<RowPresenter, IDataValidationErrors> dictionary, RowPresenter rowPresenter)
        {
            if (dictionary == null)
                return DataValidationErrors.Empty;

            if (dictionary.TryGetValue(rowPresenter, out var result))
                return result;
            else
                return DataValidationErrors.Empty;
        }

        private void ClearErrors(RowPresenter rowPresenter)
        {
            Debug.Assert(rowPresenter != null);
            if (_errorsByRow != null && _errorsByRow.ContainsKey(rowPresenter))
                _errorsByRow.Remove(rowPresenter);
        }

        private void ClearAsyncErrors(RowPresenter rowPresenter)
        {
            Debug.Assert(rowPresenter != null);
            if (_asyncErrorsByRow != null && _asyncErrorsByRow.ContainsKey(rowPresenter))
                _asyncErrorsByRow.Remove(rowPresenter);
        }

        private void ValidateCurrentRowIfImplicit()
        {
            if (Mode == ValidationMode.Implicit)
                ValidateCurrentRow();
        }

        internal void ValidateCurrentRow()
        {
            if (CurrentRow != null)
            {
                Validate(CurrentRow, true);
                InvalidateView();
            }
        }

        private Dictionary<RowPresenter, IColumns> _progress;
        private Dictionary<RowPresenter, IColumns> _valueChanged;

        internal void OnCurrentRowChanged(RowPresenter oldValue)
        {
            Template.RowAsyncValidators.ForEach(x => x.Reset());
            ValidateCurrentRowIfImplicit();
        }

        internal void Validate(RowPresenter rowPresenter, bool showAll)
        {
            Debug.Assert(rowPresenter != null);
            if (showAll)
                ShowAll(rowPresenter);
            ClearErrors(rowPresenter);
            var dataRow = rowPresenter.DataRow;
            var errors = Validate(dataRow);
            if (errors != null && errors.Count > 0)
                ErrorsByRow.Add(rowPresenter, errors);
        }

        private IDataValidationErrors Validate(DataRow dataRow)
        {
            if (dataRow == null)
                return DataValidationErrors.Empty;
            return dataRow == DataSet.AddingRow ? DataSet.ValidateAddingRow() : dataRow.Validate();
        }

        private void OnProgress<T>(RowInput<T> rowInput)
            where T : UIElement, new()
        {
            if (Mode == ValidationMode.Explicit)
                return;

            if (HasError(CurrentRow, rowInput.Target))
                return;

            var asyncValidators = Template.RowAsyncValidators;
            for (int i = 0; i < asyncValidators.Count; i++)
            {
                var asyncValidator = asyncValidators[i];
                var sourceColumns = asyncValidator.SourceColumns;
                if (sourceColumns.Overlaps(rowInput.Target) && IsVisible(CurrentRow, sourceColumns))
                    asyncValidator.Run();
            }
        }

        private bool HasError(RowPresenter rowPresenter, IColumns columns)
        {
            if (ErrorsByRow.Count == 0)
                return false;

            IDataValidationErrors errors;
            if (!ErrorsByRow.TryGetValue(rowPresenter, out errors))
                return false;

            for (int i = 0; i < errors.Count; i++)
            {
                var message = errors[i];
                if (message.Source.SetEquals(columns))
                    return true;
            }

            return false;
        }

        internal bool UpdateProgress<T>(RowInput<T> rowInput, bool valueChanged, bool makeProgress)
            where T : UIElement, new()
        {
            Debug.Assert(valueChanged || makeProgress);

            if (_progress == null)
                return valueChanged;

            var currentRow = _inputManager.CurrentRow;
            Debug.Assert(currentRow != null);
            var sourceColumns = rowInput.Target;
            if (sourceColumns == null || sourceColumns.Count == 0)
                return false;

            if (makeProgress)
            {
                var columns = GetProgress(_progress, currentRow);
                if (columns == null || columns.IsSupersetOf(sourceColumns))
                    return valueChanged;
                if (valueChanged || Exists(_valueChanged, currentRow, sourceColumns))
                {
                    _progress[currentRow] = columns.Union(sourceColumns);
                    return true;
                }
            }
            else
                _valueChanged[currentRow] = GetProgress(_valueChanged, currentRow).Union(sourceColumns);

            return false;
        }

        internal void ShowAll(RowPresenter rowPresenter)
        {
            if (_progress == null)
                return;

            if (_valueChanged.ContainsKey(rowPresenter))
                _valueChanged.Remove(rowPresenter);
            _progress[rowPresenter] = null;
        }

        internal void OnRowDisposed(RowPresenter rowPresenter)
        {
            if (_progress != null)
            {
                if (_progress.ContainsKey(rowPresenter))
                    _progress.Remove(rowPresenter);
                if (_valueChanged.ContainsKey(rowPresenter))
                    _valueChanged.Remove(rowPresenter);
            }

            ClearErrors(rowPresenter);
            ClearAsyncErrors(rowPresenter);
        }

        /// <summary>
        /// Getsl the validation mode.
        /// </summary>
        public ValidationMode Mode
        {
            get { return Template.RowValidationMode; }
        }

        /// <summary>
        /// Determines whether validation error is visible.
        /// </summary>
        /// <param name="rowPresenter">The <see cref="RowPresenter"/>.</param>
        /// <param name="columns">The columns.</param>
        /// <returns><see langword="true"/> if validation error is visible, otherwise <see langword="false"/>.</returns>
        public bool IsVisible(RowPresenter rowPresenter, IColumns columns)
        {
            rowPresenter.VerifyNotNull(nameof(rowPresenter));

            if (columns == null || columns.Count == 0)
                return false;

            if (_progress == null)
                return true;

            return Exists(_progress, rowPresenter, columns);
        }

        private static bool Exists(Dictionary<RowPresenter, IColumns> progressByRow, RowPresenter rowPresenter, IColumns columns)
        {
            Debug.Assert(progressByRow != null);
            Debug.Assert(rowPresenter != null);
            Debug.Assert(columns != null && columns.Count > 0);

            var progress = GetProgress(progressByRow, rowPresenter);
            return progress == null ? true : progress.IsSupersetOf(columns);
        }

        private static IColumns GetProgress(Dictionary<RowPresenter, IColumns> progressByRow, RowPresenter rowPresenter)
        {
            Debug.Assert(progressByRow != null);
            IColumns result;
            if (progressByRow.TryGetValue(rowPresenter, out result))
                return result;
            return Columns.Empty;
        }

        /// <summary>
        /// Performs validaiton operation.
        /// </summary>
        /// <param name="errorLimit">The validation error limit.</param>
        public void Validate(int errorLimit = 1)
        {
            if (errorLimit < 1)
                throw new ArgumentOutOfRangeException(nameof(errorLimit));

            if (CurrentRow == null)
                return;

            var errors = 0;
            var moreToValidate = Validate(CurrentRow, ref errors, errorLimit);
            if (moreToValidate)
            {
                foreach (var rowPresenter in _inputManager.Rows)
                {
                    if (rowPresenter == CurrentRow || rowPresenter.IsVirtual)
                        continue;

                    moreToValidate = Validate(rowPresenter, ref errors, errorLimit);
                    if (!moreToValidate)
                        break;
                }
            }

            InvalidateView();
        }

        private bool Validate(RowPresenter rowPresenter, ref int errors, int errorLimit)
        {
            Debug.Assert(rowPresenter != null);
            rowPresenter.Validate(false);
            if (Errors.ContainsKey(rowPresenter))
                errors++;
            return errors < errorLimit;
        }

        internal ValidationInfo GetInfo(RowView rowView)
        {
            Debug.Assert(rowView != null);

            var rowPresenter = rowView.RowPresenter;
            for (int i = 0; i < Inputs.Count; i++)
            {
                if (HasError(rowPresenter, Inputs[i], true) || IsValidatingStatus(rowPresenter, Inputs[i], true))
                    return ValidationInfo.Empty;
            }

            var errors = GetErrors(ValidationErrors.Empty, rowPresenter, null, false);
            errors = GetErrors(errors, rowPresenter, null, true);
            if (errors.Count > 0)
                return ValidationInfo.Error(errors.Seal());

            foreach (var asyncValidator in AsyncValidators)
            {
                if (asyncValidator.Status == AsyncValidatorStatus.Running)
                    return ValidationInfo.Validating;
            }

            return ValidationInfo.Empty;
        }

        internal ValidationInfo GetInfo(RowPresenter rowPresenter, Input<RowBinding, IColumns> input)
        {
            Debug.Assert(rowPresenter != null);
            Debug.Assert(input != null);

            if (rowPresenter == CurrentRow)
            {
                var flushingError = GetFlushingError(input.Binding[rowPresenter]);
                if (flushingError != null)
                    return ValidationInfo.Error(flushingError);
            }

            if (AnyBlockingErrorInput(rowPresenter, input, true) || AnyBlockingValidatingInput(rowPresenter, input, true))
                return ValidationInfo.Empty;

            var errors = GetErrors(ValidationErrors.Empty, rowPresenter, input.Target, false);
            errors = GetErrors(errors, rowPresenter, input.Target, true);
            if (errors.Count > 0)
                return ValidationInfo.Error(errors.Seal());

            if (IsValidatingStatus(rowPresenter, input, null))
                return ValidationInfo.Validating;

            if (!IsVisible(rowPresenter, input.Target) || AnyBlockingErrorInput(rowPresenter, input, false) || AnyBlockingValidatingInput(rowPresenter, input, false))
                return ValidationInfo.Empty;
            else
                return ValidationInfo.Validated;
        }

        private bool AnyBlockingErrorInput(RowPresenter rowPresenter, Input<RowBinding, IColumns> input, bool isPreceding)
        {
            for (int i = 0; i < Inputs.Count; i++)
            {
                if (input.Index == i)
                    continue;
                var canBlock = isPreceding ? Inputs[i].IsPrecedingOf(input) : input.IsPrecedingOf(Inputs[i]);
                if (canBlock && HasError(rowPresenter, Inputs[i], null))
                    return true;
            }
            return false;
        }

        internal bool HasError(RowPresenter rowPresenter, Input<RowBinding, IColumns> input, bool? blockingPrecedence)
        {
            if (rowPresenter == CurrentRow)
            {
                var flushingError = GetFlushingError(input.Binding[rowPresenter]);
                if (flushingError != null)
                    return true;
            }

            if (blockingPrecedence.HasValue)
            {
                if (AnyBlockingErrorInput(rowPresenter, input, blockingPrecedence.Value))
                    return false;
            }

            if (HasError(rowPresenter, input.Target, false))
                return true;
            if (HasError(rowPresenter, input.Target, true))
                return true;
            return false;
        }

        internal bool HasError(RowPresenter rowPresenter, IColumns source, bool isAsync)
        {
            IReadOnlyDictionary<RowPresenter, IDataValidationErrors> dictionary = isAsync ? _asyncErrorsByRow : _errorsByRow;
            if (dictionary != null)
            {
                IDataValidationErrors errors;
                if (dictionary.TryGetValue(rowPresenter, out errors))
                {
                    if (errors != null && errors.Count > 0 && HasError(rowPresenter, errors, source, !isAsync))
                        return true;
                }
            }

            if (isAsync && rowPresenter == CurrentRow)
            {
                foreach (var asyncValidator in AsyncValidators)
                {
                    if (asyncValidator.GetFault(source) != null)
                        return true;
                }
            }
            return false;
        }

        private bool HasError(RowPresenter rowPresenter, IDataValidationErrors messages, IColumns columns, bool ensureVisible)
        {
            Debug.Assert(messages != null);

            for (int i = 0; i < messages.Count; i++)
            {
                var message = messages[i];
                if (ensureVisible && !IsVisible(rowPresenter, message.Source))
                    continue;
                if (columns == null || columns.IsSupersetOf(message.Source))
                    return true;
            }

            return false;
        }

        private IValidationErrors GetErrors(IValidationErrors result, RowPresenter rowPresenter, IColumns source, bool isAsync)
        {
            IReadOnlyDictionary<RowPresenter, IDataValidationErrors> dictionary = isAsync ? _asyncErrorsByRow : _errorsByRow;
            if (dictionary != null)
            {
                IDataValidationErrors errors;
                if (dictionary.TryGetValue(rowPresenter, out errors))
                {
                    if (errors != null && errors.Count > 0)
                        result = GetErrors(result, rowPresenter, errors, source, !isAsync);
                }
            }

            if (isAsync && rowPresenter == CurrentRow)
            {
                foreach (var asyncValidator in AsyncValidators)
                {
                    var fault = asyncValidator.GetFault(source);
                    if (fault != null)
                        result = result.Add(fault);
                }
            }

            return result;
        }

        private IValidationErrors GetErrors(IValidationErrors result, RowPresenter rowPresenter, IDataValidationErrors messages, IColumns columns, bool ensureVisible)
        {
            Debug.Assert(messages != null);

            for (int i = 0; i < messages.Count; i++)
            {
                var message = messages[i];
                if (ensureVisible && !IsVisible(rowPresenter, message.Source))
                    continue;
                if (columns == null || columns.IsSupersetOf(message.Source))
                    result = result.Add(message);
            }

            return result;
        }

        /// <summary>
        /// Sets the async validation errors.
        /// </summary>
        /// <param name="validationResults">The validation results.</param>
        public void SetAsyncErrors(IDataValidationResults validationResults)
        {
            if (_asyncErrorsByRow != null)
                _asyncErrorsByRow.Clear();

            for (int i = 0; i < validationResults.Count; i++)
            {
                var entry = validationResults[i];
                var rowPresenter = _inputManager[entry.DataRow];
                if (rowPresenter != null)
                    AsyncErrorsByRow.Add(rowPresenter, entry.Errors);
            }
            InvalidateView();
        }

        private void UpdateAsyncErrors(IColumns changedColumns)
        {
            Debug.Assert(CurrentRow != null);
            var errors = GetErrors(_asyncErrorsByRow, CurrentRow);
            errors = Remove(errors, x => x.Source.Overlaps(changedColumns));
            UpdateAsyncErrors(CurrentRow, errors);
        }

        internal void UpdateAsyncErrors(RowAsyncValidator rowAsyncValidator)
        {
            Debug.Assert(CurrentRow != null);

            var sourceColumns = rowAsyncValidator.SourceColumns;
            var errors = GetErrors(_asyncErrorsByRow, CurrentRow);
            errors = Remove(errors, x => !x.Source.SetEquals(sourceColumns));
            errors = Merge(errors, rowAsyncValidator.Results);
            UpdateAsyncErrors(CurrentRow, errors);
        }

        private void UpdateAsyncErrors(RowPresenter rowPresenter, IDataValidationErrors errors)
        {
            ClearAsyncErrors(rowPresenter);
            if (errors.Count > 0)
                AsyncErrorsByRow.Add(rowPresenter, errors);
        }

        private static IDataValidationErrors Merge(IDataValidationErrors result, IDataValidationErrors errors)
        {
            return Merge(result, errors, errors.Count);
        }

        private static IDataValidationErrors Merge(IDataValidationErrors result, IDataValidationErrors errors, int count)
        {
            for (int i = 0; i < count; i++)
                result = result.Add(errors[i]);
            return result;
        }

        private static IDataValidationErrors Remove(IDataValidationErrors errors, Predicate<DataValidationError> predicate)
        {
            var result = errors;

            for (int i = 0; i < errors.Count; i++)
            {
                var error = errors[i];
                if (predicate(error))
                {
                    if (result != errors)
                        result = result.Add(error);
                    else
                        result = Merge(DataValidationErrors.Empty, errors, i);
                }
            }
            return result;
        }

        private Input<RowBinding, IColumns>[] _inputs;
        /// <summary>
        /// Gets the input objects.
        /// </summary>
        public IReadOnlyList<Input<RowBinding, IColumns>> Inputs
        {
            get { return _inputs; }
        }

        private void InitInputs()
        {
            _inputs = GetInputs(Template.RowBindings).ToArray();
            for (int i = 0; i < Inputs.Count; i++)
                Inputs[i].Index = i;
        }

        private static IEnumerable<Input<RowBinding, IColumns>> GetInputs(IReadOnlyList<RowBinding> rowBindings)
        {
            if (rowBindings == null)
                yield break;
            for (int i = 0; i < rowBindings.Count; i++)
            {
                var rowBinding = rowBindings[i];
                var rowInput = rowBinding.RowInput;
                if (rowInput != null)
                    yield return rowInput;

                foreach (var childInput in GetInputs(rowBinding.ChildBindings))
                    yield return childInput;
            }
        }

        private bool AnyBlockingValidatingInput(RowPresenter rowPresenter, Input<RowBinding, IColumns> input, bool isPreceding)
        {
            for (int i = 0; i < Inputs.Count; i++)
            {
                if (input.Index == i)
                    continue;
                var canBlock = isPreceding ? Inputs[i].IsPrecedingOf(input) : input.IsPrecedingOf(Inputs[i]);
                if (canBlock && IsValidatingStatus(rowPresenter, Inputs[i], null))
                    return true;
            }
            return false;
        }

        internal bool IsValidatingStatus(RowPresenter rowPresenter, Input<RowBinding, IColumns> input, bool? blockingPrecedence)
        {
            if (blockingPrecedence.HasValue)
            {
                if (AnyBlockingValidatingInput(rowPresenter, input, blockingPrecedence.Value))
                    return false;
            }

            if (rowPresenter == CurrentRow)
            {
                foreach (var asyncValidator in AsyncValidators)
                {
                    if (asyncValidator.Status == AsyncValidatorStatus.Running && input.Target.IsSupersetOf(asyncValidator.SourceColumns))
                        return true;
                }
            }
            return false;
        }

        internal bool HasVisibleError(RowPresenter rowPresenter)
        {
            Debug.Assert(rowPresenter != null);

            if (rowPresenter == CurrentRow)
            {
                if (FlushingErrors.Count > 0)
                    return true;
            }

            if (HasVisibleError(rowPresenter, false))
                return true;

            if (HasVisibleError(rowPresenter, true))
                return true;

            return false;
        }

        private bool HasVisibleError(RowPresenter rowPresenter, bool isAsync)
        {
            IReadOnlyDictionary<RowPresenter, IDataValidationErrors> dictionary = isAsync ? _asyncErrorsByRow : _errorsByRow;
            if (dictionary != null)
            {
                IDataValidationErrors errors;
                if (dictionary.TryGetValue(rowPresenter, out errors))
                {
                    if (errors != null && errors.Count > 0 && errors.Any(x => IsVisible(rowPresenter, x.Source)))
                        return true;
                }
            }

            if (isAsync && rowPresenter == CurrentRow)
            {
                if (AsyncValidators.Any(x => x.Status == AsyncValidatorStatus.Faulted))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Gets a value indicates whether any async validator is running.
        /// </summary>
        public bool IsValidating
        {
            get { return AsyncValidators.Any(x => x.Status == AsyncValidatorStatus.Running); }
        }

        FlushingError IRowValidation.GetFlushingError(UIElement element)
        {
            return GetFlushingError(element);
        }

        void IRowValidation.SetFlushingError(UIElement element, string flushingErrorMessage)
        {
            var flushingError = string.IsNullOrEmpty(flushingErrorMessage) ? null : new FlushingError(flushingErrorMessage, element);
            InternalFlushingErrors.SetFlushError(element, flushingError);
        }

        void IRowValidation.OnFlushed<T>(RowInput<T> rowInput, bool makeProgress, bool valueChanged)
        {
            if (!makeProgress && !valueChanged)
                return;

            if (valueChanged)
            {
                UpdateAsyncErrors(rowInput.Target);
                if (Mode != ValidationMode.Explicit)
                    Validate(CurrentRow, false);
            }
            if (UpdateProgress(rowInput, valueChanged, makeProgress))
                OnProgress(rowInput);
            InvalidateView();
        }

        ValidationInfo IRowValidation.GetInfo(RowPresenter rowPresenter, Input<RowBinding, IColumns> input)
        {
            return GetInfo(rowPresenter, input);
        }

        bool IRowValidation.HasError(RowPresenter rowPresenter, Input<RowBinding, IColumns> input, bool? blockingPrecedence)
        {
            return HasError(rowPresenter, input, blockingPrecedence);
        }

        bool IRowValidation.IsLockedByFlushingError(UIElement element)
        {
            return GetFlushingError(element) != null;
        }

        private sealed class Snapshot
        {
            private RowValidation _rowValidation;
            private IColumns _progress;
            private IColumns _valueChanged;

            public Snapshot(RowValidation rowValidation)
            {
                _rowValidation = rowValidation;
                _progress = GetProgress(rowValidation._progress, rowValidation.CurrentRow);
                _valueChanged = GetProgress(rowValidation._valueChanged, rowValidation.CurrentRow);
            }

            public void Restore()
            {
                Restore(_progress, _rowValidation._progress, _rowValidation.CurrentRow);
                Restore(_valueChanged, _rowValidation._valueChanged, _rowValidation.CurrentRow);
            }

            private static void Restore(IColumns progress, Dictionary<RowPresenter, IColumns> progressByRow, RowPresenter currentRow)
            {
                if (progress == Columns.Empty && progressByRow.ContainsKey(currentRow))
                    progressByRow.Remove(currentRow);
                else
                    progressByRow[currentRow] = progress;
            }
        }

        private Snapshot _snapshot;
        internal void EnterEdit()
        {
            if (_progress != null)
                _snapshot = new Snapshot(this);
        }

        internal void CancelEdit(RowPresenter rowAfterEditing)
        {
            _snapshot?.Restore();
            ExitEdit(rowAfterEditing);
        }

        internal void ExitEdit(RowPresenter rowAfterEditing)
        {
            Debug.Assert(!CurrentRow.IsEditing);
            if (_progress != null && rowAfterEditing != null)
                Validate(rowAfterEditing, false);
            _snapshot = null;
            _flushingErrors = null;
            if (rowAfterEditing != null)
                ClearAsyncErrors(rowAfterEditing);
            Template.RowAsyncValidators.ForEach(x => x.Reset());
        }
    }
}
