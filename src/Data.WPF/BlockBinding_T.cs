﻿using DevZest.Data.Windows.Primitives;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;

namespace DevZest.Data.Windows
{
    public sealed class BlockBinding<T> : BlockBinding
        where T : UIElement, new()
    {
        public BlockBinding(Action<T, BlockPresenter> onRefresh)
        {
            _onRefresh = onRefresh;
        }

        public BlockBinding(Action<T, BlockPresenter> onRefresh,
            Action<T, BlockPresenter> onSetup,
            Action<T, BlockPresenter> onCleanup)
            : this(onRefresh)
        {
            _onSetup = onSetup;
            _onCleanup = onCleanup;
        }

        private T Create()
        {
            var result = new T();
            OnCreated(result);
            return result;
        }

        public T SettingUpElement { get; private set; }

        internal sealed override UIElement GetSettingUpElement()
        {
            return SettingUpElement;
        }

        internal sealed override void BeginSetup(UIElement value)
        {
            SettingUpElement = value == null ? Create() : (T)value;
        }

        private BlockPresenter BlockPresenter
        {
            get { return Template.BlockPresenter; }
        }

        internal sealed override UIElement Setup(BlockView blockView)
        {
            Debug.Assert(SettingUpElement != null);
            SettingUpElement.SetBlockView(blockView);
            BlockPresenter.BlockView = blockView;
            Setup(SettingUpElement, BlockPresenter);
            Refresh(SettingUpElement, BlockPresenter);
            BlockPresenter.BlockView = null;
            return SettingUpElement;
        }

        internal sealed override void EndSetup()
        {
            SettingUpElement = null;
        }

        private Action<T, BlockPresenter> _onSetup;
        private void Setup(T element, BlockPresenter blockPresenter)
        {
            if (_onSetup != null)
                _onSetup(element, blockPresenter);
        }

        private Action<T, BlockPresenter> _onRefresh;
        private void Refresh(T element, BlockPresenter blockPresenter)
        {
            if (_onRefresh != null)
                _onRefresh(element, blockPresenter);
        }

        private Action<T, BlockPresenter> _onCleanup;
        private void Cleanup(T element, BlockPresenter blockPresenter)
        {
            if (_onCleanup != null)
                _onCleanup(element, blockPresenter);
        }

        internal sealed override void Refresh(UIElement element)
        {
            BlockPresenter.BlockView = element.GetBlockView();
            Refresh((T)element, BlockPresenter);
            BlockPresenter.BlockView = null;
        }

        internal override void Cleanup(UIElement element)
        {
            BlockPresenter.BlockView = element.GetBlockView();
            Cleanup((T)element, BlockPresenter);
            BlockPresenter.BlockView = null;
            element.SetBlockView(null);
        }

        public new T this[int blockOrdinal]
        {
            get { return (T)base[blockOrdinal]; }
        }
    }
}
