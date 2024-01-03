using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Model.ViewModels;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities.ObjectMapping;
using StructureMap;

namespace MapCallMVC.Models.ViewModels
{
    public class CreateVideoLink : ViewModel<Video>
    {
        #region Fields

        private SproutVideo _video;

        #endregion

        #region Properties

        [Required, Secured]
        public int? LinkedId { get; set; }

        [Required, Secured, DoesNotAutoMap("Used to get DataType.")]
        public string TableName { get; set; }

        [Required, StringLength(Video.MAX_SPROUT_VIDEO_ID_LENGTH)]
        public string SproutVideoId { get; set; }

        #endregion

        #region Consructors

        public CreateVideoLink(IContainer container) : base(container) { }

        #endregion

        #region Properties

        public override Video MapToEntity(Video entity)
        {
            base.MapToEntity(entity);
            entity.Title = _video.Title;
            entity.DataType = _container.GetInstance<IDataTypeRepository>().GetByTableName(TableName).First();
            return entity;
        }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var repo = _container.GetInstance<IVideoRepository>();
            _video = repo.FindSproutVideo(SproutVideoId.Trim());
            if (_video == null)
            {
                yield return new ValidationResult("Unable to find video " + SproutVideoId, new[] { "VideoId" });
            }
        }
        #endregion
    }
}