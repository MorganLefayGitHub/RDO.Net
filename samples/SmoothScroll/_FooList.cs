﻿using DevZest.Windows.Controls;
using DevZest.Windows.Data;
using System.Windows.Controls;

namespace SmoothScroll
{
    internal class _FooList : DataPresenter<Foo>
    {
        protected override void BuildTemplate(TemplateBuilder builder)
        {
            builder
                .RowView<RowView>(RowView.SelectableStyle)
                /*** Vertical layout ***/
                .GridColumns("*")
                .GridRows("Auto")
                .Layout(Orientation.Vertical)
                /*** Flowable vertical layout ***/
                //.GridColumns("300")
                //.GridRows("Auto")
                //.Layout(Orientation.Vertical, 2)
                .WithSelectionMode(SelectionMode.Extended)
                .AddBinding(0, 0, _.TextBlock());
        }
    }
}
