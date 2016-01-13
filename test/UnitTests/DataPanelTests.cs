﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Threading;

namespace DevZest.Data.Windows
{
    [TestClass]
    public class DataPanelTests
    {
        [TestMethod]
        public void DataPanel_properly_initialized()
        {
            var dataForm = new DataForm();
            dataForm.Show(DataView.Create(DataSet<Adhoc>.New()));

            dataForm.RunAfterLoaded(x =>
            {
                var layoutManager = dataForm.View.LayoutManager;
                var dataPanel = x.FindVisualChild<DataPanel>();
                Assert.IsTrue(dataPanel != null, "Failed to resolve DataSetPanel from control template.");
                Verify(dataPanel, layoutManager.Elements);
            });
        }

        private static void Verify(DataPanel dataPanel, IReadOnlyList<UIElement> elements)
        {
            Assert.IsTrue(dataPanel.Elements == elements);
            Assert.IsTrue(((IElementCollection)elements).Parent == dataPanel);
        }
    }
}
