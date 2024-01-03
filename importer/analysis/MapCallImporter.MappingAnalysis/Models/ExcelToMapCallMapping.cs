using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using MMSINC.ClassExtensions.StringExtensions;
using Newtonsoft.Json;

namespace MapCallImporter.MappingAnalysis.Models
{
    public class ExcelToMapCallMapping
    {
        #region Private Members

        protected string _excelModelName, _mapCallEntityName;

        #endregion

        #region Properties

        [JsonIgnore]
        public ClassDeclarationSyntax Class { get; }

        [JsonProperty(Order = -3)]
        public string ExcelModelName => _excelModelName ?? (_excelModelName = Class.Identifier.ValueText.ReplaceRegex("Test$", ""));

        [JsonProperty(Order = -2)]
        public string MapCallEntityName => _mapCallEntityName ?? (_mapCallEntityName = GetMapCallEntityName());

        [JsonProperty(Order = -1)]
        public IList<SimpleMappingBase> SimpleMappings { get; }

        [JsonProperty(Order = 2)]
        public IList<OtherTest> OtherTests { get; }

        #endregion

        #region Constructors

        public ExcelToMapCallMapping(ClassDeclarationSyntax cls)
        {
            Class = cls;
            SimpleMappings = new List<SimpleMappingBase>();
            OtherTests = new List<OtherTest>();
        }

        #endregion

        #region Private Methods

        protected virtual string GetMapCallEntityName()
        {
            return (Class.BaseList.Types[0].Type as GenericNameSyntax).TypeArgumentList.Arguments[0].ToString();
        }

        #endregion

        #region Exposed Methods

        public void AddSimpleMapping(InvocationExpressionSyntax expr)
        {
            SimpleMappings.Add(expr.ArgumentList.Arguments.Count == 1
                ? (SimpleMappingBase)new UnmappedPropertyMapping(expr)
                : new SimplePropertyMapping(expr));
        }

        public void AddOtherTest(MethodDeclarationSyntax test)
        {
            OtherTests.Add(new OtherTest(test));
        }

        #endregion
    }
}