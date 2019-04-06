﻿using DevZest.Data.Presenters.Primitives;
using DevZest.Data.Views;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Controls;

namespace DevZest.Data.Presenters
{
    public abstract class BlockBinding : Binding, IConcatList<BlockBinding>
    {
        #region IConcatList<BlockBinding>

        [SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes", Justification = "Child types will not call this method.")]
        int IReadOnlyCollection<BlockBinding>.Count
        {
            get { return 1; }
        }

        [SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes", Justification = "Child types will not call this method.")]
        BlockBinding IReadOnlyList<BlockBinding>.this[int index]
        {
            get
            {
                if (index != 0)
                    throw new ArgumentOutOfRangeException(nameof(index));
                return this;
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes", Justification = "Child types will not call this method.")]
        IConcatList<BlockBinding> IConcatList<BlockBinding>.Sort(Comparison<BlockBinding> comparision)
        {
            return this;
        }

        [SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes", Justification = "Child types will not call this method.")]
        IEnumerator<BlockBinding> IEnumerable<BlockBinding>.GetEnumerator()
        {
            yield return this;
        }

        [SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes", Justification = "Child types will not call this method.")]
        IEnumerator IEnumerable.GetEnumerator()
        {
            yield return this;
        }

        bool IConcatList<BlockBinding>.IsSealed
        {
            get { return true; }
        }

        IConcatList<BlockBinding> IConcatList<BlockBinding>.Seal()
        {
            return this;
        }
        #endregion

        public BlockBinding Parent { get; private set; }

        public sealed override Binding ParentBinding
        {
            get { return Parent; }
        }

        internal void Seal<T>(BlockCompositeBinding<T> parent, int ordinal)
            where T : UIElement, new()
        {
            Parent = parent;
            Ordinal = ordinal;
        }

        internal abstract UIElement Setup(BlockView blockView);

        internal override void VerifyRowRange(GridRange rowRange)
        {
            if (GridRange.IntersectsWith(rowRange))
                throw new InvalidOperationException(DiagnosticMessages.BlockBinding_IntersectsWithRowRange(Ordinal));

            if (!Template.Orientation.HasValue)
                throw new InvalidOperationException(DiagnosticMessages.BlockBinding_NullOrientation);

            var orientation = Template.Orientation.GetValueOrDefault();
            if (orientation == Orientation.Horizontal)
            {
                if (!rowRange.Contains(GridRange.Left) || !rowRange.Contains(GridRange.Right))
                    throw new InvalidOperationException(DiagnosticMessages.BlockBinding_OutOfHorizontalRowRange(Ordinal));
            }
            else
            {
                Debug.Assert(orientation == Orientation.Vertical);
                if (!rowRange.Contains(GridRange.Top) || !rowRange.Contains(GridRange.Bottom))
                    throw new InvalidOperationException(DiagnosticMessages.BlockBinding_OutOfVerticalRowRange(Ordinal));
            }
        }

        private ElementManager ElementManager
        {
            get { return Template.ElementManager; }
        }

        internal abstract UIElement GetChild(UIElement parent, int index);

        public UIElement this[int blockOrdinal]
        {
            get
            {
                if (Ordinal == -1)
                    return null;

                if (Parent != null)
                    return Parent.GetChild(Parent[blockOrdinal], Ordinal);

                if (ElementManager == null)
                    return null;

                var blockView = (BlockView)ElementManager[blockOrdinal];
                return blockView == null || blockView.Elements == null ? null : blockView[this];
            }
        }

        internal abstract void BeginSetup(UIElement value);
    }
}