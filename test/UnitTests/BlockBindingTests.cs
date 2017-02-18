﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows.Controls;

namespace DevZest.Data.Windows
{
    [TestClass]
    public class BlockBindingTests
    {
        [TestMethod]
        public void BlockBinding()
        {
            var dataSet = DataSetMock.ProductCategories(1);
            var _ = dataSet._;
            BlockBinding<BlockLabel> blockLabel = null;
            BlockBinding<BlockHeader> blockHeader = null;
            var elementManager = dataSet.CreateElementManager(builder =>
            {
                blockHeader = _.BlockHeader();
                blockLabel = _.Name.BlockLabel(blockHeader);
                builder.Layout(Orientation.Vertical, 2)
                    .GridColumns("100", "100", "100").GridRows("100")
                    .AddBinding(0, 0, blockLabel)
                    .AddBinding(1, 0, blockHeader)
                    .AddBinding(2, 0, _.Name.TextBlock());
            });

            Assert.IsNull(blockLabel.SettingUpElement);
            Assert.IsNull(blockHeader.SettingUpElement);

            var currentRow = elementManager.Rows[0];
            Assert.AreEqual(_.Name.DisplayName, blockLabel[0].Content);
            Assert.AreEqual("0", blockHeader[0].Text);
            Assert.AreEqual(blockHeader[0], blockLabel[0].Target);
        }

    }
}
