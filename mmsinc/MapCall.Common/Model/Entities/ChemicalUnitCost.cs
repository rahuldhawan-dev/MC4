using System;
using System.Collections.Generic;
using System.Linq;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class ChemicalUnitCost : IEntity, IThingWithDocuments, IThingWithNotes
    {
        public virtual string TableName => nameof(ChemicalUnitCost) + "s";

        public virtual int Id { get; set; }
        public virtual Chemical Chemical { get; set; }
        public virtual ChemicalWarehouseNumber WarehouseNumber { get; set; }
        public virtual ChemicalVendor Vendor { get; set; }
        public virtual DateTime? StartDate { get; set; }
        public virtual DateTime? EndDate { get; set; }
        public virtual decimal? PricePerPoundWet { get; set; }
        public virtual string PoNumber { get; set; }
        public virtual int? ChemicalLeadTimeDays { get; set; }
        public virtual string ChemicalOrderingProcess { get; set; }
        public virtual IList<Document<ChemicalUnitCost>> Documents { get; set; }
        public virtual IList<Note<ChemicalUnitCost>> Notes { get; set; }

        public virtual IList<IDocumentLink> LinkedDocuments => Documents.Cast<IDocumentLink>().ToList();
        public virtual IList<INoteLink> LinkedNotes => Notes.Cast<INoteLink>().ToList();

        public ChemicalUnitCost()
        {
            Documents = new List<Document<ChemicalUnitCost>>();
            Notes = new List<Note<ChemicalUnitCost>>();
        }

        public override string ToString()
        {
            return new ChemicalUnitCostDisplayItem {PricePerPoundWet = PricePerPoundWet}.Display;
        }
    }

    public class ChemicalUnitCostDisplayItem : DisplayItem<ChemicalUnitCost>
    {
        public virtual decimal? PricePerPoundWet { get; set; }

        public override string Display => PricePerPoundWet.HasValue ? $"{PricePerPoundWet.Value:C}" : "n/a";
    }
}
