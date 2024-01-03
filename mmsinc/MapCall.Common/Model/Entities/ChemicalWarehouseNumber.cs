using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using MapCall.Common.Model.Mappings;
using MMSINC.ClassExtensions.IQueryableExtensions;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class ChemicalWarehouseNumber : IEntity, IThingWithDocuments, IThingWithNotes
    {
        public virtual string TableName => nameof(ChemicalWarehouseNumber) + "s";

        public virtual int Id { get; set; }
        public virtual OperatingCenter OperatingCenter { get; set; }
        public virtual string WarehouseNumber { get; set; }
        public virtual IList<Document<ChemicalWarehouseNumber>> Documents { get; set; }
        public virtual IList<Note<ChemicalWarehouseNumber>> Notes { get; set; }

        public virtual IList<IDocumentLink> LinkedDocuments => Documents.Cast<IDocumentLink>().ToList();

        public virtual IList<INoteLink> LinkedNotes => Notes.Cast<INoteLink>().ToList();

        public override string ToString()
        {
            return WarehouseNumber;
        }
    }
}
