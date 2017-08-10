﻿using System;
using System.Windows;
using System.Collections.Generic;
using DevZest.Data.Views;
using DevZest.Data.Views.Primitives;

namespace DevZest.Data.Presenters.Primitives
{
    public abstract class CompositeBlockBinding : BlockBinding, ICompositeBinding
    {
        private List<BlockBinding> _bindings = new List<BlockBinding>();
        private List<string> _names = new List<string>();

        IReadOnlyList<Binding> ICompositeBinding.Bindings
        {
            get { return _bindings; }
        }

        IReadOnlyList<string> ICompositeBinding.Names
        {
            get { return _names; }
        }

        internal void InternalAddChild<T>(BlockBinding<T> binding, string name)
            where T : UIElement, new()
        {
            Binding.VerifyAdding(binding, nameof(binding));

            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            VerifyNotSealed();
            _bindings.Add(binding);
            _names.Add(name);
            binding.Seal(this, _bindings.Count - 1);
        }

        internal abstract ICompositeView CreateView();

        private ICompositeView Initialize()
        {
            return CreateView().BindingDispatcher.Initialize(this);
        }

        private ICompositeView _settingUpView;
        private List<ICompositeView> _cachedViews;

        internal sealed override UIElement GetSettingUpElement()
        {
            return (UIElement)_settingUpView;
        }

        internal sealed override void BeginSetup(UIElement value)
        {
            if (value == null)
                _settingUpView = Initialize();
            else
            {
                _settingUpView = (ICompositeView)value;
                _settingUpView.BindingDispatcher.BeginSetup();
            }
        }

        internal sealed override UIElement Setup(BlockView blockView)
        {
            for (int i = 0; i < _bindings.Count; i++)
                _bindings[i].Setup(blockView);
            return (UIElement)_settingUpView;
        }

        internal sealed override void Refresh(UIElement element)
        {
            ((ICompositeView)element).BindingDispatcher.Refresh();
        }

        internal sealed override void Cleanup(UIElement element)
        {
            var view = (ICompositeView)element;
            view.BindingDispatcher.Cleanup();
        }

        internal sealed override void EndSetup()
        {
            _settingUpView.BindingDispatcher.EndSetup();
            _settingUpView = null;
        }
    }
}
