using System;
using System.Collections.Generic;
using System.Linq;
using MapCall.Common.Model.Mappings;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class ChemicalInventoryTransaction : IEntity, IThingWithDocuments, IThingWithNotes
    {
        public virtual string TableName => nameof(ChemicalInventoryTransaction) + "s";

        public virtual int Id { get; set; }
        public virtual ChemicalStorage Storage { get; set; }
        public virtual ChemicalDelivery Delivery { get; set; }
        public virtual DateTime? Date { get; set; }
        public virtual string InventoryRecordType { get; set; }
        public virtual int? QuantityGallons { get; set; }
        public virtual int? QuantityPounds { get; set; }
        public virtual IList<Document<ChemicalInventoryTransaction>> Documents { get; set; }
        public virtual IList<Note<ChemicalInventoryTransaction>> Notes { get; set; }

        public virtual IList<IDocumentLink> LinkedDocuments => Documents.Cast<IDocumentLink>().ToList();

        public virtual IList<INoteLink> LinkedNotes => Notes.Cast<INoteLink>().ToList();

        public ChemicalInventoryTransaction()
        {
            Notes = new List<Note<ChemicalInventoryTransaction>>();
            Documents = new List<Document<ChemicalInventoryTransaction>>();
        }
    }
}
