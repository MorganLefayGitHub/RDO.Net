﻿using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DevZest.Data.CodeAnalysis
{
    [TestClass]
    public class KnownTypesTests
    {
        [TestMethod]
        public void KnownTypes_GetKnownType_not_null()
        {
            var project = string.Empty.CreateProject();
            var compilation = project.GetCompilationAsync().Result;

            Assert.IsNotNull(compilation.GetKnownType(KnownTypes.AttributeUsageAttribute));

            Assert.IsNotNull(compilation.GetKnownType(KnownTypes.Model));
            Assert.IsNotNull(compilation.GetKnownType(KnownTypes.Column));
            Assert.IsNotNull(compilation.GetKnownType(KnownTypes.LocalColumn));
            Assert.IsNotNull(compilation.GetKnownType(KnownTypes.ColumnList));
            Assert.IsNotNull(compilation.GetKnownType(KnownTypes.Projection));
            Assert.IsNotNull(compilation.GetKnownType(KnownTypes.PrimaryKey));
            Assert.IsNotNull(compilation.GetKnownType(KnownTypes.NamedModelAttribute));
            Assert.IsNotNull(compilation.GetKnownType(KnownTypes.AscAttribute));
            Assert.IsNotNull(compilation.GetKnownType(KnownTypes.DescAttribute));
            Assert.IsNotNull(compilation.GetKnownType(KnownTypes.MounterRegistrationAttribute));
            Assert.IsNotNull(compilation.GetKnownType(KnownTypes.CreateKeyAttribute));
        }
    }
}