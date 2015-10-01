﻿using DevZest.Data.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DevZest.Data
{
    [TestClass]
    public class _BinaryTests
    {
        [TestMethod]
        public void BinaryColumn_implicit_convert()
        {
            TestParam(new byte[0]);
            TestParam(null);
        }

        private void TestParam(Binary x)
        {
            _Binary column = x;
            column.VerifyParam(x);
        }

        [TestMethod]
        public void BinaryColumn_Const()
        {
            TestConstant(new byte[0]);
            TestConstant(null);
        }

        private void TestConstant(Binary x)
        {
            _Binary column = _Binary.Const(x);
            column.VerifyConst(x);
        }
    }
}
