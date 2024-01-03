using Microsoft.CodeAnalysis.CSharp.Syntax;
using MMSINC.ClassExtensions.StringExtensions;
using MMSINC.Utilities.Json;
using Newtonsoft.Json;

namespace MapCallImporter.MappingAnalysis.Models
{
    public abstract class SimpleMappingBase
    {
        #region Private Members

        protected string _mappingType;
        protected string _excelColumn;

        #endregion

        #region Properties

        [JsonProperty(Order = 1)]
        [JsonConverter(typeof(ToStringJsonConverter))]
        public InvocationExpressionSyntax Expression { get; }

        [JsonProperty(Order = -1)]
        public string MappingType => _mappingType ??
                                     (_mappingType = Expression.Expression.ToString()
                                                               .ReplaceRegex("^.+\\.([^.]+)$", "$1"));

        [JsonProperty(Order = -3)]
        public string ExcelColumn => _excelColumn ??
                                     (_excelColumn =
                                         ((Expression.ArgumentList.Arguments[0].Expression as
                                             SimpleLambdaExpressionSyntax)?.Body as MemberAccessExpressionSyntax)?.Name
                                                                                                                  .ToString());

        #endregion

        #region Constructors

        public SimpleMappingBase(InvocationExpressionSyntax expr)
        {
            Expression = expr;
        }

        #endregion
    }
}