using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.Mappings;
using MMSINC.Data;
using Newtonsoft.Json;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class FacilityProcess : IThingWithNotes, IThingWithDocuments, IEntityLookup, IThingWithVideos
    {
        #region Properties

        public virtual int Id { get; set; }
        public virtual string Description => Process.Description;

        [JsonIgnore]
        public virtual Facility Facility { get; set; }

        public virtual Process Process { get; set; }

        [JsonIgnore]
        public virtual IList<FacilityProcessStep> FacilityProcessSteps { get; set; }

        public virtual string FacilityProcessDescription { get; set; }

        #region Notes/Docs

        public virtual string TableName => FacilityProcessMap.TABLE_NAME;

        public virtual IList<FacilityProcessDocument> FacilityProcessDocuments { get; set; }
        public virtual IList<FacilityProcessNote> FacilityProcessNotes { get; set; }

        public virtual IList<IDocumentLink> LinkedDocuments => FacilityProcessDocuments.Cast<IDocumentLink>().ToList();

        public virtual IList<INoteLink> LinkedNotes => FacilityProcessNotes.Cast<INoteLink>().ToList();

        public virtual IList<FacilityProcessVideo> Videos { get; set; }

        public virtual IEnumerable<IVideoLink> LinkedVideos => Videos;

        #endregion

        #endregion

        #region COnstructor

        public FacilityProcess()
        {
            Videos = new List<FacilityProcessVideo>();
        }

        #endregion

        #region Public Methods

        public override string ToString()
        {
            return Description;
        }

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return Enumerable.Empty<ValidationResult>();
        }

        #endregion
    }
}
