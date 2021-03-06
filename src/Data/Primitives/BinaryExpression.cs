﻿using System;

namespace DevZest.Data.Primitives
{
    /// <summary>Represents column expression which contains two column operands.</summary>
    /// <typeparam name="T">The data type of the column operands and column expression.</typeparam>
    public abstract class BinaryExpression<T> : BinaryExpression<T, T>
    {
        /// <summary>Initializes a new instance of <see cref="BinaryExpression{T}"/> class.</summary>
        /// <param name="left">The left column operand.</param>
        /// <param name="right">The right column operand.</param>
        protected BinaryExpression(Column<T> left, Column<T> right)
            : base(left, right)
        {
        }
    }

    /// <summary>Represents column expression which contains two column operands.</summary>
    /// <typeparam name="T">The data type of column operands.</typeparam>
    /// <typeparam name="TResult">The data type of the column expression.</typeparam>
    public abstract class BinaryExpression<T, TResult> : ColumnExpression<TResult>
    {
        private const string LEFT = nameof(Left);
        private const string RIGHT = nameof(Right);

        /// <summary>Initializes a new instance of <see cref="BinaryExpression{T, TResult}"/> class.</summary>
        /// <param name="left">The left column operand.</param>
        /// <param name="right">The right column operand.</param>
        protected BinaryExpression(Column<T> left, Column<T> right)
        {
            left.VerifyNotNull(nameof(left));
            right.VerifyNotNull(nameof(right));

            Left = left;
            Right = right;
        }

        /// <summary>Gets the left column operand.</summary>
        public Column<T> Left { get; private set; }

        /// <summary>Gets the right column operand.</summary>
        public Column<T> Right { get; private set; }

        /// <summary>Gets the kind of this binary expression.</summary>
        protected abstract BinaryExpressionKind Kind { get; }

        /// <inheritdoc/>
        protected sealed override IModels GetScalarSourceModels()
        {
            return Left.ScalarSourceModels.Union(Right.ScalarSourceModels).Seal();
        }

        /// <inheritdoc/>
        protected sealed override IModels GetAggregateBaseModels()
        {
            return Left.AggregateSourceModels.Union(Right.AggregateSourceModels).Seal();
        }

        /// <inheritdoc/>
        public sealed override DbExpression GetDbExpression()
        {
            return new DbBinaryExpression(typeof(TResult), Kind, Left.DbExpression, Right.DbExpression);
        }

        /// <inheritdoc/>
        public sealed override TResult this[DataRow dataRow]
        {
            get
            {
                var x = Left[dataRow];
                var y = Right[dataRow];
                return EvalCore(x, y);
            }
        }

        /// <summary>Evaluates the expression against two operand values.</summary>
        /// <param name="x">The left operand value.</param>
        /// <param name="y">The right operand value.</param>
        /// <returns>The result.</returns>
        protected abstract TResult EvalCore(T x, T y);

        /// <inheritdoc />
        protected sealed override IColumns GetBaseColumns()
        {
            return Left.BaseColumns.Union(Right.BaseColumns).Seal();
        }

        /// <inheritdoc />
        protected internal sealed override ColumnExpression PerformTranslateTo(Model model)
        {
            var left = Left.TranslateTo(model);
            var right = Right.TranslateTo(model);
            if (left == Left && right == Right)
                return this;
            else
                return (ColumnExpression)Activator.CreateInstance(GetType(), left, right);
        }
    }
}
