using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Diagnostics;
using System;
using System.IO;
using TinyAggregate.Analyzer.Test.Helpers;
using Xunit;

namespace TinyAggregate.Analyzer.Test
{
    public class TinyAggregateAnalyzerUnitTests : CodeFixVerifier
    {

        [Fact]
        public void When_The_Aggregate_Doesnt_Implement_The_Visitor_Then_A_Warning_Is_Created()
        {
            var classWithoutVisitorImplementation = new StreamReader(GetType().Assembly.GetManifestResourceStream("TinyAggregate.Analyzer.Test.Resources.AggregateWithoutVisitorImplementation.cs")).ReadToEnd();
            
            VerifyCSharpDiagnostic(classWithoutVisitorImplementation);
        }

        //Diagnostic and CodeFix both triggered and checked for
        [Fact]
        public void TestMethod2()
        {
            var test = @"
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Diagnostics;

    namespace ConsoleApplication1
    {
        class TypeName
        {   
        }
    }";
            var expected = new DiagnosticResult
            {
                Id = "TinyAggregateAnalyzer",
                Message = String.Format("Type name '{0}' contains lowercase letters", "TypeName"),
                Severity = DiagnosticSeverity.Warning,
                Locations =
                    new[] {
                            new DiagnosticResultLocation("Test0.cs", 11, 15)
                        }
            };

            VerifyCSharpDiagnostic(test, expected);

            var fixtest = @"
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Diagnostics;

    namespace ConsoleApplication1
    {
        class TYPENAME
        {   
        }
    }";
            VerifyCSharpFix(test, fixtest);
        }

        protected override CodeFixProvider GetCSharpCodeFixProvider()
        {
            return new TinyAggregateAnalyzerCodeFixProvider();
        }

        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new TinyAggregateAnalyzerAnalyzer();
        }
    }
}
