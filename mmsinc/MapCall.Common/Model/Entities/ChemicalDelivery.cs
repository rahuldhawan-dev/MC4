using System;
using System.Collections.Generic;
using System.Linq;
using MapCall.Common.Model.Mappings;
using MMSINC.ClassExtensions.IQueryableExtensions;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class ChemicalDelivery : IEntity, IThingWithDocuments, IThingWithNotes
    {
        public virtual string TableName => ChemicalDeliveryMap.TABLE_NAME;

        public virtual int Id { get; set; }
        public virtual ChemicalStorage Storage { get; set; }
        public virtual Facility Facility { get; set; }
        public virtual ChemicalUnitCost UnitCost { get; set; }
        public virtual Chemical Chemical { get; set; }
        public virtual DateTime? DateOrdered { get; set; }
        public virtual DateTime? ScheduledDeliveryDate { get; set; }
        public virtual DateTime? ActualDeliveryDate { get; set; }
        public virtual string ConfirmationInformation { get; set; }
        public virtual string ReceiptNumberJde { get; set; }
        public virtual string BatchNumberJde { get; set; }
        public virtual int? EstimatedDeliveryQuantityGallons { get; set; }
        public virtual int? ActualDeliveryQuantityGallons { get; set; }
        public virtual int? EstimatedDeliveryQuantityPounds { get; set; }
        public virtual int? ActualDeliveryQuantityPounds { get; set; }
        public virtual string DeliveryTicketNumber { get; set; }
        public virtual string DeliveryInstructions { get; set; }
        public virtual bool SplitFacilityDelivery { get; set; }
        public virtual string SecurityInformation { get; set; }

        public virtual IList<Document<ChemicalDelivery>> Documents { get; set; }
        public virtual IList<Note<ChemicalDelivery>> Notes { get; set; }

        public virtual IList<IDocumentLink> LinkedDocuments => Documents.Cast<IDocumentLink>().ToList();
        public virtual IList<INoteLink> LinkedNotes => Notes.Cast<INoteLink>().ToList();

        public ChemicalDelivery()
        {
            Documents = new List<Document<ChemicalDelivery>>();
            Notes = new List<Note<ChemicalDelivery>>();
        }

        public override string ToString()
        {
            return new ChemicalDeliveryDisplayItem {
                ChemicalName = Chemical?.Name,
                ChemicalSymbol = Chemical?.ChemicalSymbol,
                DeliveryTicketNumber = DeliveryTicketNumber
            }.Display;
        }
    }

    public class ChemicalDeliveryDisplayItem : DisplayItem<ChemicalDelivery>
    {
        [SelectDynamic("Name", Field = "Chemical")]
        public string ChemicalName { get; set; }

        [SelectDynamic("ChemicalSymbol", Field = "Chemical")]
        public string ChemicalSymbol { get; set; }

        public string DeliveryTicketNumber { get; set; }
        public override string Display => $"{ChemicalSymbol} - {ChemicalName} - {DeliveryTicketNumber}";
    }
}
