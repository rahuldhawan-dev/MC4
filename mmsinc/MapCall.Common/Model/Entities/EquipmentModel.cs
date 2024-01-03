using System;
using System.Collections.Generic;
using MapCall.Common.Model.Mappings;
using MMSINC.ClassExtensions.IListExtensions;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class EquipmentModel : EntityLookup, IThingWithDocuments, IThingWithNotes
    {
        #region Properties

        public virtual EquipmentManufacturer EquipmentManufacturer { get; set; }

        public virtual IList<Equipment> Equipment { get; set; }

        #region Logical Properties

        #region Documents

        public virtual IList<EquipmentModelDocument> EquipmentModelDocuments { get; set; }

        public virtual IList<IDocumentLink> LinkedDocuments
        {
            get { return EquipmentModelDocuments.Map(epd => (IDocumentLink)epd); }
        }

        public virtual IList<Document> Documents
        {
            get { return EquipmentModelDocuments.Map(epd => epd.Document); }
        }

        #endregion

        #region Notes

        public virtual IList<EquipmentModelNote> EquipmentModelNotes { get; set; }

        public virtual IList<INoteLink> LinkedNotes
        {
            get { return EquipmentModelNotes.Map(n => (INoteLink)n); }
        }

        public virtual IList<Note> Notes
        {
            get { return EquipmentModelNotes.Map(n => n.Note); }
        }

        #endregion

        public virtual string TableName => nameof(EquipmentModel) + "s";

        #endregion

        #endregion

        #region Constructors

        public EquipmentModel()
        {
            Equipment = new List<Equipment>();
        }

        #endregion
    }
}
