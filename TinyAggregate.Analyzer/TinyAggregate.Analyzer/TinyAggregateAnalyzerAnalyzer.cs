using System.Collections.Generic;
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
            // See https://github.com/dotnet/roslyn/blob/master/docs/analyzers/Analyzer%20Actions%20Semantics.md for more information
            context.RegisterSymbolAction(AnalyzeSymbol, SymbolKind.NamedType);
        }

        private static void AnalyzeSymbol(SymbolAnalysisContext context)
        {
            var namedTypeSymbol = (INamedTypeSymbol)context.Symbol;

            if (IsAggregate(namedTypeSymbol))
            {
                var ivisitorInterfaceGenericArgument = namedTypeSymbol.AllInterfaces.First(symbol => symbol.IsGenericType && symbol.Name == "IAggregate").TypeArguments.First();

                var implemented = namedTypeSymbol.AllInterfaces.Any(symbol => symbol.Equals(ivisitorInterfaceGenericArgument));

                if (implemented)
                {
                    return;
                }

                var diagnostic = Diagnostic.Create(MustImplementTVisitorOrOverrideVisitorRule, namedTypeSymbol.Locations[0], namedTypeSymbol.Name);
                context.ReportDiagnostic(diagnostic);
            }
        }

        private static bool IsAggregate(INamedTypeSymbol namedTypeSymbol)
        {
            if (namedTypeSymbol.TypeKind != TypeKind.Class || namedTypeSymbol.IsAbstract)
            {
                return false;
            }

            var baseTypes = new List<INamedTypeSymbol>();
            var baseType = namedTypeSymbol.BaseType;
            while (baseType != null)
            {
                baseTypes.Add(baseType);
                baseType = baseType.BaseType;
            }

            const string namespaceName = "TinyAggregate";
            const string typeName = "IAggregate";

            // TODO: find a slicker way to match the type and namespace
            return baseTypes.Any(symbol => symbol.TypeKind == TypeKind.Class
                                           && symbol.ContainingNamespace.Name == namespaceName
                                           && symbol.AllInterfaces.Any(typeSymbol => 
                                               typeSymbol.Name == typeName 
                                               && typeSymbol.ContainingNamespace.Name == namespaceName));
        }
    }
}