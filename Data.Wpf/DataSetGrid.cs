﻿using DevZest.Data.Primitives;
using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace DevZest.Data.Windows
{
    public class DataSetGrid : Control
    {
        private static readonly DependencyPropertyKey ViewPropertyKey = DependencyProperty.RegisterReadOnly(nameof(View),
            typeof(DataSetView), typeof(DataSetGrid), new FrameworkPropertyMetadata(null, OnViewChanged));

        private static void OnViewChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var newValue = (DataSetView)e.NewValue;
            Debug.Assert(newValue != null);
            ((DataSetGrid)d).ScrollOption = newValue.ScrollOption;
        }

        public static readonly DependencyProperty ViewProperty = ViewPropertyKey.DependencyProperty;

        private static readonly DependencyPropertyKey ScrollOptionPropertyKey = DependencyProperty.RegisterReadOnly(nameof(ScrollOption),
            typeof(ScrollOption), typeof(DataSetGrid), new FrameworkPropertyMetadata(ScrollOption.None));

        public static readonly DependencyProperty ScrollOptionProperty = ScrollOptionPropertyKey.DependencyProperty;

        public DataSetView View
        {
            get { return (DataSetView)GetValue(ViewProperty); }
            private set { SetValue(ViewPropertyKey, value); }
        }

        public ScrollOption ScrollOption
        {
            get { return (ScrollOption)GetValue(ScrollOptionProperty); }
            private set { SetValue(ScrollOptionPropertyKey, value); }
        }

        public void Initialize<TModel>(DataSet<TModel> dataSet, Action<GridTemplate, TModel> templateInitializer = null)
            where TModel : Model, new()
        {
            if (dataSet == null)
                throw new ArgumentNullException(nameof(dataSet));

            var gridTemplate = new GridTemplate();
            gridTemplate.BeginInit(dataSet.Model);
            if (templateInitializer != null)
                templateInitializer(gridTemplate, dataSet._);
            else
                gridTemplate.DefaultInitialize();
            gridTemplate.EndInit();
            View = new DataSetView(null, gridTemplate);
        }
    }
}
