using MMSINC.ClassExtensions.DateTimeExtensions;
using MMSINC.Metadata;
using MMSINC.Utilities;
using StructureMap.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using MMSINC.Utilities.Excel;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class OperatorLicense : 
        IThingWithDocuments, IThingWithOperatingCenter, IThingWithNotes, IThingWithEmployee
    {
        #region Constants

        public struct StringLengths
        {
            public const int LICENSE_LEVEL = 30,
                             LICENSE_SUB_LEVEL = 100,
                             LICENSE_NUMBER = 12;
        }

        public readonly struct DisplayNames
        {
            public const string LICENSE_LEVEL = "License Level/Class",
                                LICENSE_SUB_LEVEL = "License Sub-level/Sub-class",
                                EMPLOYEE_STATUS = "Employee Status";
        }

        #endregion

        #region Private Members

        [NonSerialized] private IDateTimeProvider _dateTimeProvider;

        #endregion

        #region Properties

        #region Table Properties

        public virtual int Id { get; set; }

        [View(DisplayNames.LICENSE_LEVEL)]
        public virtual string LicenseLevel { get; set; }

        [View(DisplayNames.LICENSE_SUB_LEVEL)]
        public virtual string LicenseSubLevel { get; set; }

        public virtual string LicenseNumber { get; set; }

        [View(FormatStyle.Date)]
        public virtual DateTime ValidationDate { get; set; }

        [View(FormatStyle.Date)]
        public virtual DateTime ExpirationDate { get; set; }

        public virtual bool LicensedOperatorOfRecord { get; set; }

        #endregion

        #region References

        public virtual OperatingCenter OperatingCenter { get; set; }
        public virtual Employee Employee { get; set; }
        public virtual State State { get; set; }
        public virtual OperatorLicenseType OperatorLicenseType { get; set; }

        #endregion

        #region Logical Properties

        [DoesNotExport]
        public virtual string TableName => nameof(OperatorLicense) + "s";

        [DoesNotExport]
        public virtual string RecordUrl { get; set; }

        public virtual IList<Document<OperatorLicense>> Documents { get; set; } = new List<Document<OperatorLicense>>();
        public virtual IList<Note<OperatorLicense>> Notes { get; set; } = new List<Note<OperatorLicense>>();

        public virtual IList<PublicWaterSupplyLicensedOperator> PublicWaterSupplies { get; set; } =
            new List<PublicWaterSupplyLicensedOperator>();

        public virtual IList<WasteWaterSystem> WasteWaterSystems { get; set; } =
            new List<WasteWaterSystem>();

        public virtual IList<IDocumentLink> LinkedDocuments => Documents.Cast<IDocumentLink>().ToList();
        public virtual IList<INoteLink> LinkedNotes => Notes.Cast<INoteLink>().ToList();

        // license normally expires at the end of the month so if the expiration date is 4/30 and it's 4/30/20 - the license should still be valid
        public virtual bool Expired => _dateTimeProvider.GetCurrentDate().BeginningOfDay() > ExpirationDate.Date;

        #endregion

        #region Injected Properties

        [SetterProperty]
        public virtual IDateTimeProvider DateTimeProvider
        {
            set => _dateTimeProvider = value;
        }

        #endregion

        #endregion

        #region Exposed Methods

        public override string ToString()
        {
            return $"Operator License for {Employee} with expiration: {ExpirationDate}";
        }

        public virtual bool IsValidAndNotExpiredForDate(DateTime date)
        {
            date = date.Date;
            return ValidationDate.Date <= date && date <= ExpirationDate.Date;
        }

        #endregion
    }
}
