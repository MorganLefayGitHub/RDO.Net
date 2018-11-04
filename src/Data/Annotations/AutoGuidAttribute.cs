﻿using DevZest.Data.Annotations.Primitives;
using DevZest.Data.Primitives;
using System;

namespace DevZest.Data.Annotations
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    [ModelMemberAttributeSpec(typeof(ColumnDefault), true, typeof(_Guid))]
    public sealed class AutoGuidAttribute : ColumnAttribute
    {
        protected override void Wireup(Column column)
        {
            ((_Guid)column).SetDefault(Functions.NewGuid(), Name, Description);
        }

        public string Name { get; set; }

        public string Description { get; set; }
    }
}
