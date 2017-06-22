﻿using DevZest.Data.Views;
using DevZest.Data.Views.Primitives;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Controls;

namespace DevZest.Data.Presenters.Primitives
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
        void IConcatList<BlockBinding>.Sort(Comparison<BlockBinding> comparision)
        {
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

        #endregion

        public CompositeBlockBinding Parent { get; private set; }

        public sealed override Binding ParentBinding
        {
            get { return Parent; }
        }

        internal void Seal(CompositeBlockBinding parent, int ordinal)
        {
            Parent = parent;
            Ordinal = ordinal;
        }

        internal abstract UIElement Setup(BlockView blockView);

        internal override void VerifyRowRange(GridRange rowRange)
        {
            if (GridRange.IntersectsWith(rowRange))
                throw new InvalidOperationException(Strings.BlockBinding_IntersectsWithRowRange(Ordinal));

            if (!Template.Orientation.HasValue)
                throw new InvalidOperationException(Strings.BlockBinding_NullOrientation);

            var orientation = Template.Orientation.GetValueOrDefault();
            if (orientation == Orientation.Horizontal)
            {
                if (!rowRange.Contains(GridRange.Left) || !rowRange.Contains(GridRange.Right))
                    throw new InvalidOperationException(Strings.BlockBinding_OutOfHorizontalRowRange(Ordinal));
            }
            else
            {
                Debug.Assert(orientation == Orientation.Vertical);
                if (!rowRange.Contains(GridRange.Top) || !rowRange.Contains(GridRange.Bottom))
                    throw new InvalidOperationException(Strings.BlockBinding_OutOfVerticalRowRange(Ordinal));
            }
        }

        private ElementManager ElementManager
        {
            get { return Template.ElementManager; }
        }

        public UIElement this[int blockOrdinal]
        {
            get
            {
                if (Ordinal == -1)
                    return null;

                if (Parent != null)
                {
                    var view = (ICompositeView)Parent[blockOrdinal];
                    return view == null ? null : view.CompositeBinding.Children[Ordinal];
                }

                if (ElementManager == null)
                    return null;

                var blockView = (BlockView)ElementManager[blockOrdinal];
                return blockView == null || blockView.Elements == null ? null : blockView[this];
            }
        }
    }
}
