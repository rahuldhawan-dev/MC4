using Microsoft.CodeAnalysis.CSharp.Syntax;
using MMSINC.ClassExtensions.StringExtensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MapCallImporter.MappingAnalysis.Models
{
    public class SimplePropertyMapping : SimpleMappingBase
    {
        #region Private Members

        protected string _mapCallProperty;

        #endregion

        #region Properties

        [JsonProperty(Order = -2)]
        public string MapCallProperty => _mapCallProperty ?? (_mapCallProperty =
                                             Expression.ArgumentList.Arguments.Count == 1
                                                 ? null
                                                 : (Expression.ArgumentList.Arguments[1].Expression as
                                                     SimpleLambdaExpressionSyntax).Body.ToString()
                                                 .ReplaceRegex("^[^.]+\\.(.+)$", "$1"));

        #endregion

        #region Constructors

        public SimplePropertyMapping(InvocationExpressionSyntax expr) : base(expr) {}

        #endregion
    }
}