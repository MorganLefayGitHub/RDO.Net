﻿using DevZest.Data.Presenters.Primitives;
using DevZest.Data.Views.Primitives;
using System.Windows.Controls;
using System.Windows.Markup;
using System;
using System.Diagnostics;
using System.Windows;
using System.Collections.Generic;

namespace DevZest.Data.Views
{
    [ContentPropertyAttribute(nameof(Pane.Content))]
    public class Pane : ContentPresenter, ICompositeView
    {
        private sealed class PaneBindingDispatcher : CompositeBindingDispatcher
        {
            public PaneBindingDispatcher(Pane view)
            {
                Debug.Assert(view != null);
                _view = view;
            }

            private readonly Pane _view;
            protected override ICompositeView View
            {
                get { return _view; }
            }

            private readonly List<UIElement> _children = new List<UIElement>();
            public override IReadOnlyList<UIElement> Children
            {
                get { return _children; }
            }

            protected override void AddChild(UIElement child, string name)
            {
                var placeholder = _view.GetPlaceholder(name);
                if (placeholder == null)
                    throw new InvalidOperationException();
                _children.Add(child);
                placeholder.Content = child;
            }
        }

        public Pane()
        {
            _bindingDispatcher = new PaneBindingDispatcher(this);
        }

        private readonly PaneBindingDispatcher _bindingDispatcher;
        public CompositeBindingDispatcher BindingDispatcher
        {
            get { return _bindingDispatcher; }
        }

        public virtual ContentPresenter GetPlaceholder(string name)
        {
            return FindName(name) as ContentPresenter;
        }
    }
}
