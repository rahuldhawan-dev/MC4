using Microsoft.CodeAnalysis.CSharp.Syntax;
using Newtonsoft.Json;

namespace MapCallImporter.MappingAnalysis.Models
{
    public class EquipmentCharacteristicMapping : SimpleMappingBase
    {
        #region Private Members

        protected string _mapCallCharacteristic;

        #endregion

        #region Properties

        [JsonProperty(Order = -2)]
        public string MapCallCharacteristic => _mapCallCharacteristic ?? (_mapCallCharacteristic =
                                             Expression.ArgumentList.Arguments.Count == 1
                                                 ? null
                                                 : (Expression.ArgumentList.Arguments[1].Expression as
                                                     LiteralExpressionSyntax)
                                                 .Token.ValueText);

        #endregion

        #region Constructors

        public EquipmentCharacteristicMapping(InvocationExpressionSyntax expr) : base(expr) {}

        #endregion
    }
}