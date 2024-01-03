using System;
using System.Collections.Generic;
using System.Linq;
using MapCall.Common.Model.Mappings;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class VehicleEZPass : IEntity, IThingWithNotes, IThingWithDocuments
    {
        #region Consts

        public struct StringLengths
        {
            public const int SERIAL_NUMBER = 50,
                             BILLING_INFO = 255;
        }

        #endregion

        #region Properties

        public virtual int Id { get; set; }
        public virtual string EZPassSerialNumber { get; set; }
        public virtual string BillingInfo { get; set; }

        public virtual IList<Document<VehicleEZPass>> Documents { get; set; }
        public virtual IList<Note<VehicleEZPass>> Notes { get; set; }

        public virtual IList<IDocumentLink> LinkedDocuments => Documents.Cast<IDocumentLink>().ToList();

        public virtual IList<INoteLink> LinkedNotes => Notes.Cast<INoteLink>().ToList();

        public virtual string TableName => VehicleEZPassMap.TABLE_NAME;

        #endregion

        #region Public Methods

        public override string ToString()
        {
            return EZPassSerialNumber;
        }

        #endregion
    }
}
