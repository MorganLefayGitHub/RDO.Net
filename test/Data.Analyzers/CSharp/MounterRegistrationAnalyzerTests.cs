using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace DevZest.Data.Analyzers.CSharp
{
    [TestClass]
    public class MounterRegistrationAnalyzerTests : DiagnosticVerifier
    {
        //No diagnostics expected to show up
        [TestMethod]
        public void empty_source_generates_no_diagnostics()
        {
            var test = @"";

            VerifyCSharpDiagnostic(test);
        }

        [TestMethod]
        public void RegisterColumn_with_no_error()
        {
            var test = @"
using DevZest.Data;

class SimpleModel : Model
{
    public static readonly Mounter<_Int32> _Column1 = RegisterColumn((SimpleModel x) => x.Column1);
    public static readonly Mounter<_Int32> _Column2;

    static SimpleModel()
    {
        _Column2 = RegisterColumn((SimpleModel x) => x.Column2);
        RegisterColumn((SimpleModel x) => x.Column3);
    }

    public _Int32 Column1 { get; private set; }

    public _Int32 Column2 { get; private set; }

    public _Int32 Column3 { get; private set; }
}";

            VerifyCSharpDiagnostic(test);
        }

        [TestMethod]
        public void RegisterColumn_InvalidInvocation_assigned_to_local_variable()
        {
            var test = @"
using DevZest.Data;

class SimpleModel : Model
{
    public static readonly Mounter<_Int32> _Column1 = RegisterColumn((SimpleModel x) => x.Column1);
    public static readonly Mounter<_Int32> _Column2;

    static SimpleModel()
    {
        var column2 = RegisterColumn((SimpleModel x) => x.Column2);
    }

    public _Int32 Column1 { get; private set; }

    public _Int32 Column2 { get; private set; }
}";

            var expected = new DiagnosticResult
            {
                Id = DiagnosticIds.MounterRegistration_InvalidInvocation,
                Message = Resources.MounterRegistration_InvalidInvocation,
                Severity = DiagnosticSeverity.Error,
                Locations = new[] { new DiagnosticResultLocation("Test0.cs", 11, 23) }
            };
            VerifyCSharpDiagnostic(test, expected);
        }

        [TestMethod]
        public void RegisterColumn_InvalidInvocation_in_a_separate_method()
        {
            var test = @"
using DevZest.Data;

class SimpleModel : Model
{
    public static readonly Mounter<_Int32> _Column1 = RegisterColumn((SimpleModel x) => x.Column1);
    public static readonly Mounter<_Int32> _Column2;

    private static void AnotherMethod()
    {
        _Column2 = RegisterColumn((SimpleModel x) => x.Column2);
    }

    public _Int32 Column1 { get; private set; }

    public _Int32 Column2 { get; private set; }
}";

            var expected = new DiagnosticResult
            {
                Id = DiagnosticIds.MounterRegistration_InvalidInvocation,
                Message = Resources.MounterRegistration_InvalidInvocation,
                Severity = DiagnosticSeverity.Error,
                Locations = new[] { new DiagnosticResultLocation("Test0.cs", 11, 20) }
            };
            VerifyCSharpDiagnostic(test, expected);
        }

        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new MounterRegistrationAnalyzer();
        }
    }
}