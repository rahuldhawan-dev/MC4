using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace MapCallImporter.MappingAnalysis.Models
{
    public class ExcelToMapCallEquipmentMapping : ExcelToMapCallMapping
    {
        #region Properties

        public IList<EquipmentCharacteristicMapping> EquipmentCharacteristicMappings { get; }

        #endregion

        #region Constructors

        public ExcelToMapCallEquipmentMapping(ClassDeclarationSyntax cls) : base(cls)
        {
            EquipmentCharacteristicMappings = new List<EquipmentCharacteristicMapping>();
        }

        #endregion

        #region Private Methods

        protected override string GetMapCallEntityName()
        {
            return "Equipment";
        }

        #endregion

        #region Exposed Methods

        public void AddEquipmentCharacteristicMapping(InvocationExpressionSyntax expr)
        {
            EquipmentCharacteristicMappings.Add(new EquipmentCharacteristicMapping(expr));
        }

        #endregion
    }
}