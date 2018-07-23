using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace TinyAggregate.Analyzer
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class TinyAggregateAnalyzerAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "TinyAggregateAnalyzer";

        private static readonly DiagnosticDescriptor MustImplementTVisitorOrOverrideVisitorRule = new DiagnosticDescriptor(
            DiagnosticId, 
            "Aggregate must implement TVisitor or override the Visitor property.",
            "Type name '{0}' does not implement TVisitor or override the Visitor property",
            "Design", 
            DiagnosticSeverity.Warning, 
            isEnabledByDefault: true, 
            description: "Aggregate must implement TVisitor or override the Visitor property.");

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(MustImplementTVisitorOrOverrideVisitorRule);

        public override void Initialize(AnalysisContext context)
        {
            // TODO: Consider registering other actions that act on syntax instead of or in addition to symbols
            // See https://github.com/dotnet/roslyn/blob/master/docs/analyzers/Analyzer%20Actions%20Semantics.md for more information
            context.RegisterSymbolAction(AnalyzeSymbol, SymbolKind.NamedType);
        }

        private static void AnalyzeSymbol(SymbolAnalysisContext context)
        {
            // TODO: Replace the following code with your own analysis, generating Diagnostic objects for any issues you find
            var namedTypeSymbol = (INamedTypeSymbol)context.Symbol;

            if (IsAggregate(namedTypeSymbol))
            {
                // Find just those named type symbols with names containing lowercase letters.
                if (namedTypeSymbol.Name.ToCharArray().Any(char.IsLower))
                {
                    // For all such symbols, produce a diagnostic.
                    var diagnostic = Diagnostic.Create(MustImplementTVisitorOrOverrideVisitorRule, namedTypeSymbol.Locations[0], namedTypeSymbol.Name);

                    context.ReportDiagnostic(diagnostic);
                }
            }
        }

        private static bool IsAggregate(INamedTypeSymbol namedTypeSymbol)
        {
            return namedTypeSymbol.TypeKind == TypeKind.Class 
                   && namedTypeSymbol.BaseType.ContainingNamespace.Name == typeof(Aggregate<object>).Namespace
                   && namedTypeSymbol.AllInterfaces.Any(symbol => symbol.Name == "IAggregate" && symbol.ContainingNamespace.Name == typeof(IAggregate<object>).Namespace);
        }

        private static bool ImplementsTVisitor(INamedTypeSymbol namedTypeSymbol)
        {
            return false;
        }

    }
}
