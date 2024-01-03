using Microsoft.CodeAnalysis.CSharp.Syntax;
using MMSINC.ClassExtensions.StringExtensions;
using MMSINC.Utilities.Json;
using Newtonsoft.Json;

namespace MapCallImporter.MappingAnalysis.Models
{
    public class OtherTest
    {
        #region Private Members

        protected string _name;

        #endregion

        #region Properties

        public string Name => _name ?? (_name = Test.Identifier.ValueText.ReplaceRegex("^Test", ""));

        [JsonConverter(typeof(ToStringJsonConverter))]
        public MethodDeclarationSyntax Test { get; }

        #endregion

        #region Constructors

        public OtherTest(MethodDeclarationSyntax test)
        {
            Test = test;
        }

        #endregion
    }
}