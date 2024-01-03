using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    public class ChemicalDisplayItem : DisplayItem<Chemical>
    {
        public string Name { get; set; }
        public string ChemicalSymbol { get; set; }
        public string PartNumber { get; set; }

        public override string Display => string.IsNullOrWhiteSpace(ChemicalSymbol)
            ? $"{PartNumber} - {Name}"
            : $"{PartNumber} - {ChemicalSymbol} - {Name}";
    }
}
