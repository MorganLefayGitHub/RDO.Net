﻿using DevZest.Data.Primitives;

namespace DevZest.Data.Addons
{
    /// <summary>Represents database table CHECK constraint.</summary>
    public sealed class DbCheck : DbTableConstraint
    {
        internal DbCheck(string name, string description, DbExpression logicalExpression)
            : base(name, description)
        {
            LogicalExpression = logicalExpression;
        }

        /// <summary>Gets the logical expression of this CHECK constraint.</summary>
        public DbExpression LogicalExpression { get; private set; }

        public override bool IsValidOnTable
        {
            get { return true; }
        }

        public override bool IsValidOnTempTable
        {
            get { return true; }
        }
    }
}