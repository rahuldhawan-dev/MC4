using System;
using System.Collections.Generic;
using System.Linq;
using MapCall.Common.Model.Entities.Users;
using MMSINC.Data;
using MMSINC.Data.ChangeTracking;
using MMSINC.Metadata;
using MMSINC.Utilities;

namespace MapCall.Common.Model.Entities
{
    /// <summary>
    /// AWIA = America’s Water Infrastructure Act. It was passed in 2018 and requires community water systems serving over
    /// 3,300 people to conduct Risk & Resilience Assessments(RRA) and update their Emergency Response Plans(ERP) on a
    /// 5-year cycle staggered by size.For more info: https://www.epa.gov/waterresilience/awia-section-2013
    /// </summary>
    [Serializable]
    public class AwiaCompliance
        : IEntityWithCreationUserTracking<User>, IThingWithNotes, IThingWithDocuments
    {
        #region Constants

        public struct Display
        {
            public const string DATE_ACCEPTED = "Certification Due",
                                PUBLIC_WATER_SUPPLY = "PWSID";
        }

        #endregion

        #region Constructor

        public AwiaCompliance()
        {
            PublicWaterSupplies = new List<PublicWaterSupply>();
            AwiaComplianceNotes = new List<Note<AwiaCompliance>>();
            AwiaComplianceDocuments = new List<Document<AwiaCompliance>>();
        }

        #endregion

        #region Properties

        public virtual int Id { get; set; }

        public virtual State State { get; set; }

        public virtual OperatingCenter OperatingCenter { get; set; }

        public virtual AwiaComplianceCertificationType CertificationType { get; set; }

        [View("Entered By")]
        public virtual User CreatedBy { get; set; }

        public virtual User CertifiedBy { get; set; }

        [View(FormatStyle.Date)]
        public virtual DateTime DateSubmitted { get; set; }

        [View(Display.DATE_ACCEPTED, FormatStyle.Date)]
        public virtual DateTime DateAccepted { get; set; }

        [View(FormatStyle.Date)]
        public virtual DateTime RecertificationDue { get; set; }

        [View(Display.PUBLIC_WATER_SUPPLY)]
        public virtual IList<PublicWaterSupply> PublicWaterSupplies { get; set; }

        #region Documents

        public virtual IList<Document<AwiaCompliance>> AwiaComplianceDocuments { get; set; }

        public virtual IList<IDocumentLink> LinkedDocuments
        {
            get { return AwiaComplianceDocuments.Cast<IDocumentLink>().ToList(); }
        }

        #endregion

        #region Notes

        public virtual IList<Note<AwiaCompliance>> AwiaComplianceNotes { get; set; }

        public virtual IList<INoteLink> LinkedNotes
        {
            get { return AwiaComplianceNotes.Cast<INoteLink>().ToList(); }
        }

        public virtual string TableName => nameof(AwiaCompliance) + "s";

        #endregion

        #endregion
    }
}
