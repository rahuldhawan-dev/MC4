using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.Entities.Users;
using MMSINC.ClassExtensions.IListExtensions;
using MMSINC.Data;
using MMSINC.Data.ChangeTracking;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class DevelopmentProject
        : IEntityWithCreationUserTracking<User>, IValidatableObject, IThingWithDocuments, IThingWithNotes
    {
        #region Constants

        public struct StringLengths
        {
            public const int DEVELOPER_SERVICES_ID = 20,
                             WBS_NUMBER = 20,
                             STREET_NAME = 50,
                             CREATED_BY = 20;
        }

        #endregion

        #region Properties

        #region Table Columns

        public virtual int Id { get; set; }

        [StringLength(StringLengths.DEVELOPER_SERVICES_ID)]
        [DisplayName("Developer Services Reference Id")]
        public virtual string DeveloperServicesId { get; set; }

        [StringLength(StringLengths.WBS_NUMBER), Required]
        public virtual string WBSNumber { get; set; }

        [Required]
        public virtual string ProjectDescription { get; set; }

        [StringLength(StringLengths.STREET_NAME)]
        public virtual string StreetName { get; set; }

        public virtual DateTime? ForecastedInServiceDate { get; set; }
        public virtual DateTime? InServiceDate { get; set; }

        [Required, Range(0, 999)]
        public virtual int? DomesticWaterServices { get; set; }

        [Required, Range(0, 999)]
        public virtual int? FireServices { get; set; }

        [Required, Range(0, 999)]
        public virtual int? DomesticSanitaryServices { get; set; }

        #endregion

        #region References

        public virtual IList<DevelopmentProjectDocument> DevelopmentProjectDocuments { get; set; }
        public virtual IList<DevelopmentProjectNote> DevelopmentProjectNotes { get; set; }

        [Required]
        public virtual OperatingCenter OperatingCenter { get; set; }

        [Required]
        public virtual DevelopmentProjectCategory Category { get; set; }

        [Required]
        public virtual BusinessUnit BusinessUnit { get; set; }

        public virtual Employee ProjectManager { get; set; }

        [Required]
        public virtual PublicWaterSupply PublicWaterSupply { get; set; }

        public virtual Town Town { get; set; }

        [Required]
        public virtual User CreatedBy { get; set; }

        public virtual Coordinate Coordinate { get; set; }

        #endregion

        #region Logical Properties

        public virtual string TableName => nameof(DevelopmentProject) + "s";

        public virtual IList<INoteLink> LinkedNotes
        {
            get { return DevelopmentProjectNotes.Map(x => (INoteLink)x); }
        }

        public virtual IList<IDocumentLink> LinkedDocuments
        {
            get { return DevelopmentProjectDocuments.Map(x => (IDocumentLink)x); }
        }

        #endregion

        #endregion

        #region Constructors

        public DevelopmentProject()
        {
            DevelopmentProjectDocuments = new List<DevelopmentProjectDocument>();
            DevelopmentProjectNotes = new List<DevelopmentProjectNote>();
        }

        #endregion

        #region Exposed Methods

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return Enumerable.Empty<ValidationResult>();
        }

        #endregion
    }
}
