using System;
using MapCall.Common.Model.Entities.Users;
using MMSINC.Data;
using MMSINC.Data.ChangeTracking;
using MMSINC.Metadata;
using MMSINC.Utilities;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class ServicePremiseContact : IEntityWithCreationUserTracking<User>
    {
        #region Properties

        public virtual int Id { get; set; }
        public virtual Service Service { get; set; }
        public virtual ServicePremiseContactMethod ContactMethod { get; set; }
        public virtual ServicePremiseContactType ContactType { get; set; }
        public virtual bool NotifiedCustomerServiceCenter { get; set; }
        public virtual bool CertifiedLetterSent { get; set; }
        public virtual User CreatedBy { get; set; }
       
        [Multiline]
        public virtual string CommunicationResults { get; set; }

        /// <summary>
        /// Additional contact information(the person's name, phone number, whatever they want).
        /// </summary>
        [Multiline]
        public virtual string ContactInformation { get; set; }

        [View(FormatStyle.Date)]
        public virtual DateTime ContactDate { get; set; }

        #endregion
    }

    [Serializable]
    public class ServicePremiseContactMethod : EntityLookup { }

    [Serializable]
    public class ServicePremiseContactType : EntityLookup { }
}
