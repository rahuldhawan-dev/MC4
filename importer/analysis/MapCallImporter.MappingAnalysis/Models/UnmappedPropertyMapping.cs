using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace MapCallImporter.MappingAnalysis.Models
{
    public class UnmappedPropertyMapping : SimpleMappingBase
    {
        #region Constructors

        public UnmappedPropertyMapping(InvocationExpressionSyntax expr) : base(expr) { }

        #endregion
    }
}