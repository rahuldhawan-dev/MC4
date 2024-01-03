using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities;
using MMSINC.Validation;
using StructureMap;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MapCallMVC.Areas.Facilities.Models.ViewModels
{
    public class CreateCommunityRightToKnow : CommunityRightToKnowViewModel
    {
        #region Constructors
        public CreateCommunityRightToKnow(IContainer container) : base(container) { }

        #endregion

        #region Exposed Methods

        public override CommunityRightToKnow MapToEntity(CommunityRightToKnow entity)
        {
            var communityRightToKnow = base.MapToEntity(entity);
            if (!SubmissionDate.HasValue)
            {
                communityRightToKnow.SubmissionDate = _container.GetInstance<IDateTimeProvider>().GetCurrentDate();
            }
            return communityRightToKnow;
        }

        #endregion
    }
}