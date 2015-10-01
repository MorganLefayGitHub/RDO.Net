﻿using DevZest.Data.Helpers;
using DevZest.Data.Primitives;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace DevZest.Data
{
    [TestClass]
    public class _DecimalTests
    {
        [TestMethod]
        public void DecimalColumn_Param()
        {
            TestParam(5.5m);
            TestParam(null);
        }

        private static void TestParam(Decimal? x)
        {
            var column = _Decimal.Param(x);
            column.VerifyParam(x);
        }

        [TestMethod]
        public void DecimalColumn_implicit_convert()
        {
            TestImplicit(5.5m);
            TestImplicit(null);
        }

        private static void TestImplicit(Decimal? x)
        {
            _Decimal column = x;
            column.VerifyParam(x);
        }

        [TestMethod]
        public void DecimalColumn_Const()
        {
            TestConst(5.5m);
            TestConst(null);
        }

        private static void TestConst(Decimal? x)
        {
            _Decimal column = _Decimal.Const(x);
            column.VerifyConst(x);
        }

        [TestMethod]
        public void DecimalColumn_negate()
        {
            TestNegate(2, -2);
            TestNegate(null, null);
        }

        private void TestNegate(Decimal? x, Decimal? expectedValue)
        {
            _Decimal column = x;
            var expr = -column;
            var dbExpr = (DbUnaryExpression)expr.DbExpression;
            dbExpr.Verify(DbUnaryExpressionKind.Negate, column);
            expr.VerifyEval(expectedValue);
        }

        [TestMethod]
        public void DecimalColumn_add()
        {
            TestAdd(1, 2, 3);
            TestAdd(1, null, null);
            TestAdd(null, null, null);
        }

        private void TestAdd(Decimal? x, Decimal? y, Decimal? expectedValue)
        {
            _Decimal column1 = x;
            _Decimal column2 = y;
            var expr = column1 + column2;
            var dbExpr = (DbBinaryExpression)expr.DbExpression;
            dbExpr.Verify(BinaryExpressionKind.Add, column1, column2);
            expr.VerifyEval(expectedValue);
        }

        [TestMethod]
        public void DecimalColumn_substract()
        {
            TestSubstract(5, 2, 3);
            TestSubstract(null, 2, null);
            TestSubstract(2, null, null);
        }

        private void TestSubstract(Decimal? x, Decimal? y, Decimal? expectedValue)
        {
            _Decimal column1 = x;
            _Decimal column2 = y;
            var expr = column1 - column2;
            var dbExpr = (DbBinaryExpression)expr.DbExpression;
            dbExpr.Verify(BinaryExpressionKind.Substract, column1, column2);
            expr.VerifyEval(expectedValue);
        }

        [TestMethod]
        public void DecimalColumn_multipy()
        {
            TestMultiply(5, 5, 25);
            TestMultiply(5, null, null);
            TestMultiply(null, null, null);
        }

        private void TestMultiply(Decimal? x, Decimal? y, Decimal? expectedValue)
        {
            _Decimal column1 = x;
            _Decimal column2 = y;
            var expr = column1 * column2;
            var dbExpr = (DbBinaryExpression)expr.DbExpression;
            dbExpr.Verify(BinaryExpressionKind.Multiply, column1, column2);
            expr.VerifyEval(expectedValue);
        }

        [TestMethod]
        public void DecimalColumn_divide()
        {
            TestDivide(12, 4, 3);
            TestDivide(5, null, null);
            TestDivide(null, 5, null);
            TestDivide(null, null, null);
        }

        private void TestDivide(Decimal? x, Decimal? y, Decimal? expectedValue)
        {
            _Decimal column1 = x;
            _Decimal column2 = y;
            var expr = column1 / column2;
            var dbExpr = (DbBinaryExpression)expr.DbExpression;
            dbExpr.Verify(BinaryExpressionKind.Divide, column1, column2);
            expr.VerifyEval(expectedValue);
        }

        [TestMethod]
        public void DecimalColumn_modulo()
        {
            TestModulo(5, 3, 2);
            TestModulo(100, null, null);
            TestModulo(null, null, null);
        }

        private void TestModulo(Decimal? x, Decimal? y, Decimal? expectedValue)
        {
            _Decimal column1 = x;
            _Decimal column2 = y;
            var expr = column1 % column2;
            var dbExpr = (DbBinaryExpression)expr.DbExpression;
            dbExpr.Verify(BinaryExpressionKind.Modulo, column1, column2);
            expr.VerifyEval(expectedValue);
        }

        [TestMethod]
        public void DecimalColumn_less_than()
        {
            TestLessThan(99, 100, true);
            TestLessThan(99, 99, false);
            TestLessThan(99, 98, false);
            TestLessThan(5, null, null);
            TestLessThan(null, 5, null);
            TestLessThan(null, null, null);
        }

        private void TestLessThan(Decimal? x, Decimal? y, bool? expectedValue)
        {
            _Decimal column1 = x;
            _Decimal column2 = y;
            var expr = column1 < column2;
            var dbExpr = (DbBinaryExpression)expr.DbExpression;
            dbExpr.Verify(BinaryExpressionKind.LessThan, column1, column2);
            expr.VerifyEval(expectedValue);
        }

        [TestMethod]
        public void DecimalColumn_less_than_or_equal()
        {
            TestLessThanOrEqual(99, 98, false);
            TestLessThanOrEqual(99, 99, true);
            TestLessThanOrEqual(99, 100, true);
            TestLessThanOrEqual(5, null, null);
            TestLessThanOrEqual(null, 5, null);
            TestLessThanOrEqual(null, null, null);
        }

        private void TestLessThanOrEqual(Decimal? x, Decimal? y, bool? expectedValue)
        {
            _Decimal column1 = x;
            _Decimal column2 = y;
            var expr = column1 <= column2;
            var dbExpr = (DbBinaryExpression)expr.DbExpression;
            dbExpr.Verify(BinaryExpressionKind.LessThanOrEqual, column1, column2);
            expr.VerifyEval(expectedValue);
        }

        [TestMethod]
        public void DecimalColumn_greater_than()
        {
            TestGreaterThan(100, 99, true);
            TestGreaterThan(100, 100, false);
            TestGreaterThan(99, 100, false);
            TestGreaterThan(5, null, null);
            TestGreaterThan(null, 5, null);
            TestGreaterThan(null, null, null);
        }

        private void TestGreaterThan(Decimal? x, Decimal? y, bool? expectedValue)
        {
            _Decimal column1 = x;
            _Decimal column2 = y;
            var expr = column1 > column2;
            var dbExpr = (DbBinaryExpression)expr.DbExpression;
            dbExpr.Verify(BinaryExpressionKind.GreaterThan, column1, column2);
            expr.VerifyEval(expectedValue);
        }

        [TestMethod]
        public void DecimalColumn_greater_than_or_equal()
        {
            TestGreaterThanOrEqual(100, 99, true);
            TestGreaterThanOrEqual(100, 100, true);
            TestGreaterThanOrEqual(99, 100, false);
            TestGreaterThanOrEqual(5, null, null);
            TestGreaterThanOrEqual(null, 5, null);
            TestGreaterThanOrEqual(null, null, null);
        }

        private void TestGreaterThanOrEqual(Decimal? x, Decimal? y, bool? expectedValue)
        {
            _Decimal column1 = x;
            _Decimal column2 = y;
            var expr = column1 >= column2;
            var dbExpr = (DbBinaryExpression)expr.DbExpression;
            dbExpr.Verify(BinaryExpressionKind.GreaterThanOrEqual, column1, column2);
            expr.VerifyEval(expectedValue);
        }

        [TestMethod]
        public void DecimalColumn_equal()
        {
            TestEqual(2, 2, true);
            TestEqual(4, 5, false);
            TestEqual(1, null, null);
            TestEqual(null, null, null);
        }

        private void TestEqual(Decimal? x, Decimal? y, bool? expectedValue)
        {
            _Decimal column1 = x;
            _Decimal column2 = y;
            var expr = column1 == column2;
            var dbExpr = (DbBinaryExpression)expr.DbExpression;
            dbExpr.Verify(BinaryExpressionKind.Equal, column1, column2);
            expr.VerifyEval(expectedValue);
        }

        [TestMethod]
        public void DecimalColumn_not_equal()
        {
            TestNotEqual(1, 1, false);
            TestNotEqual(1, 2, true);
            TestNotEqual(1, null, null);
            TestNotEqual(null, null, null);
        }

        private void TestNotEqual(Decimal? x, Decimal? y, bool? expectedValue)
        {
            _Decimal column1 = x;
            _Decimal column2 = y;
            var expr = column1 != column2;
            var dbExpr = (DbBinaryExpression)expr.DbExpression;
            dbExpr.Verify(BinaryExpressionKind.NotEqual, column1, column2);
            expr.VerifyEval(expectedValue);
        }

        [TestMethod]
        public void DecimalColumn_cast_from_BooleanColumn()
        {
            TestBooleanColumnCast(_Boolean.True, 1);
            TestBooleanColumnCast(_Boolean.False, 0);
            TestBooleanColumnCast(_Boolean.Null, null);
        }

        private void TestBooleanColumnCast(_Boolean x, Decimal? expectedValue)
        {
            _Decimal expr = (_Decimal)x;
            var dbExpr = (DbCastExpression)expr.DbExpression;
            dbExpr.Verify(x, typeof(bool?), typeof(Decimal?));
            expr.VerifyEval(expectedValue);
        }

        [TestMethod]
        public void DecimalColumn_cast_from_ByteColumn()
        {
            TestByteColumnCast(null, null);
            TestByteColumnCast(127, 127);
        }

        private void TestByteColumnCast(byte? x, Decimal? expectedValue)
        {
            _Byte column1 = x;
            _Decimal expr = (_Decimal)column1;
            var dbExpr = (DbCastExpression)expr.DbExpression;
            dbExpr.Verify(column1, typeof(byte?), typeof(Decimal?));
            expr.VerifyEval(expectedValue);
        }

        [TestMethod]
        public void DecimalColumn_cast_from_Int16Column()
        {
            TestInt16ColumnCast(null, null);
            TestInt16ColumnCast(5, 5);
        }

        private void TestInt16ColumnCast(Int16? x, Decimal? expectedValue)
        {
            _Int16 column1 = x;
            _Decimal expr = (_Decimal)column1;
            var dbExpr = (DbCastExpression)expr.DbExpression;
            dbExpr.Verify(column1, typeof(Int16?), typeof(Decimal?));
            expr.VerifyEval(expectedValue);
        }

        [TestMethod]
        public void DecimalColumn_cast_from_Int64Column()
        {
            TestInt64ColumnCast(8, 8);
            TestInt64ColumnCast(null, null);
        }

        private void TestInt64ColumnCast(Int64? x, Decimal? expectedValue)
        {
            _Int64 column1 = x;
            _Decimal expr = (_Decimal)column1;
            var dbExpr = (DbCastExpression)expr.DbExpression;
            dbExpr.Verify(column1, typeof(Int64?), typeof(Decimal?));
            expr.VerifyEval(expectedValue);
        }

        [TestMethod]
        public void DecimalColumn_cast_from_Int32Column()
        {
            TestInt32ColumnCast(8, 8);
            TestInt32ColumnCast(null, null);
        }

        private void TestInt32ColumnCast(Int32? x, Decimal? expectedValue)
        {
            _Int32 column1 = x;
            _Decimal expr = (_Decimal)column1;
            var dbExpr = (DbCastExpression)expr.DbExpression;
            dbExpr.Verify(column1, typeof(Int32?), typeof(Decimal?));
            expr.VerifyEval(expectedValue);
        }

        [TestMethod]
        public void DecimalColumn_cast_from_DoubleColumn()
        {
            TestDoubleColumnCast(8, 8);
            TestDoubleColumnCast(null, null);
        }

        private void TestDoubleColumnCast(Double? x, Decimal? expectedValue)
        {
            _Double column1 = x;
            _Decimal expr = (_Decimal)column1;
            var dbExpr = (DbCastExpression)expr.DbExpression;
            dbExpr.Verify(column1, typeof(Double?), typeof(Decimal?));
            expr.VerifyEval(expectedValue);
        }

        [TestMethod]
        public void DecimalColumn_cast_from_SingleColumn()
        {
            TestSingleColumnCast(8, 8);
            TestSingleColumnCast(null, null);
        }

        private void TestSingleColumnCast(Single? x, Decimal? expectedValue)
        {
            _Single column1 = x;
            _Decimal expr = (_Decimal)column1;
            var dbExpr = (DbCastExpression)expr.DbExpression;
            dbExpr.Verify(column1, typeof(Single?), typeof(Decimal?));
            expr.VerifyEval(expectedValue);
        }

        [TestMethod]
        public void DecimalColumn_cast_from_StringColumn()
        {
            TestStringColumnCast("8", 8);
            TestStringColumnCast(null, null);
        }

        private void TestStringColumnCast(String x, Decimal? expectedValue)
        {
            _String column1 = x;
            _Decimal expr = (_Decimal)column1;
            var dbExpr = (DbCastExpression)expr.DbExpression;
            dbExpr.Verify(column1, typeof(String), typeof(Decimal?));
            expr.VerifyEval(expectedValue);
        }
    }
}
