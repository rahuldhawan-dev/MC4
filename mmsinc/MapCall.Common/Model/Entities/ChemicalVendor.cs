using System;
using System.Collections.Generic;
using System.Linq;
using MMSINC.Data;
using MMSINC.Metadata;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class ChemicalVendor : IEntity, IThingWithDocuments, IThingWithNotes, IThingWithCoordinate
    {
        public virtual string TableName => nameof(ChemicalVendor) + "s";

        public virtual int Id { get; set; }
        public virtual MapIcon Icon => Coordinate?.Icon;

        [View("JDE Vendor Id")]
        public virtual string JdeVendorId { get; set; }

        public virtual string Vendor { get; set; }
        public virtual string OrderContact { get; set; }
        public virtual string PhoneOffice { get; set; }
        public virtual string PhoneCell { get; set; }
        public virtual string Fax { get; set; }
        public virtual string Email { get; set; }
        public virtual string Address { get; set; }
        public virtual string City { get; set; }
        public virtual string State { get; set; }
        public virtual string Zip { get; set; }
        public virtual Coordinate Coordinate { get; set; }
        public virtual decimal? Latitude => Coordinate?.Latitude;
        public virtual decimal? Longitude => Coordinate?.Longitude;
        public virtual IList<Document<ChemicalVendor>> Documents { get; set; }
        public virtual IList<Note<ChemicalVendor>> Notes { get; set; }

        public virtual IList<IDocumentLink> LinkedDocuments => Documents.Cast<IDocumentLink>().ToList();

        public virtual IList<INoteLink> LinkedNotes => Notes.Cast<INoteLink>().ToList();

        public ChemicalVendor()
        {
            Documents = new List<Document<ChemicalVendor>>();
            Notes = new List<Note<ChemicalVendor>>();
        }

        public override string ToString()
        {
            return new ChemicalVendorDisplayItem {Vendor = Vendor}.Display;
        }
    }

    public class ChemicalVendorDisplayItem : DisplayItem<ChemicalVendor>
    {
        public string Vendor { get; set; }
        public override string Display => Vendor;
    }
}
