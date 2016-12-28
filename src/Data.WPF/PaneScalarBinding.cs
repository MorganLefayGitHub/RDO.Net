﻿using DevZest.Data.Windows.Primitives;
using System;
using System.Collections.Generic;
using System.Windows;

namespace DevZest.Data.Windows
{
    public abstract class PaneScalarBinding : ScalarBinding
    {
        private sealed class ConcretePaneScalarBinding<T> : PaneScalarBinding
            where T : Pane, new()
        {
            internal override Pane CreatePane()
            {
                return new T();
            }
        }

        public static PaneScalarBinding Create<T>()
            where T : Pane, new()
        {
            return new ConcretePaneScalarBinding<T>();
        }

        private List<ScalarBinding> _bindings = new List<ScalarBinding>();
        private List<string> _names = new List<string>();

        public void AddChild<T>(ScalarBinding<T> binding, string name)
            where T : UIElement, new()
        {
            if (binding == null)
                throw new ArgumentNullException(nameof(binding));

            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            VerifyNotSealed();
            _bindings.Add(binding);
            _names.Add(name);
        }

        internal abstract Pane CreatePane();

        private Pane Create()
        {
            return CreatePane().BeginSetup(_bindings, _names);
        }

        private Pane _settingUpPane;
        private List<Pane> _cachedPanes;

        internal sealed override UIElement GetSettingUpElement()
        {
            return _settingUpPane;
        }

        internal sealed override void BeginSetup(UIElement value)
        {
            _settingUpPane = (Pane)value;
        }

        internal sealed override void BeginSetup()
        {
            _settingUpPane = CachedList.GetOrCreate(ref _cachedPanes, Create).BeginSetup(_bindings);
        }

        internal sealed override UIElement Setup()
        {
            for (int i = 0; i < _bindings.Count; i++)
                _bindings[i].Setup();
            return _settingUpPane;
        }

        internal sealed override void Refresh(UIElement element)
        {
            ((Pane)element).Refresh(_bindings);
        }

        internal sealed override void Cleanup(UIElement element, bool recycle)
        {
            var pane = (Pane)element;
            pane.Cleanup(_bindings);
            if (recycle)
                CachedList.Recycle(ref _cachedPanes, pane);
        }

        internal sealed override void EndSetup()
        {
            _settingUpPane.EndSetup(_bindings);
            _settingUpPane = null;
        }

        internal sealed override void FlushInput(UIElement element)
        {
            ((Pane)element).FlushInput(_bindings);
        }

        internal sealed override bool HasPreValidatorError
        {
            get { return _bindings.HasPreValidatorError(); }
        }

        internal sealed override void RunAsyncValidatorIfNecessary()
        {
            for (int i = 0; i < _bindings.Count; i++)
                _bindings[i].RunAsyncValidatorIfNecessary();
        }
    }
}
