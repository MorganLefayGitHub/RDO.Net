﻿using DevZest.Data.Presenters.Primitives;
using System.Windows;

namespace DevZest.Data.Presenters
{
    public sealed class RoutedEventTrigger<T> : Trigger<T>
        where T : UIElement, new()
    {
        public RoutedEventTrigger(RoutedEvent routedEvent)
        {
            Check.NotNull(routedEvent, nameof(routedEvent));
            _routedEvent = routedEvent;
        }

        private readonly RoutedEvent _routedEvent;

        protected internal override void Attach(T element)
        {
            element.AddHandler(_routedEvent, new RoutedEventHandler(OnExecute));
        }

        protected internal override void Detach(T element)
        {
            element.RemoveHandler(_routedEvent, new RoutedEventHandler(OnExecute));
        }

        private void OnExecute(object sender, RoutedEventArgs e)
        {
            Execute((T)sender);
        }
    }
}
