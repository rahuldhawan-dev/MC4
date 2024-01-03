using MapCall.Common.Model.Entities;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MapCallMVC.Areas.Facilities.Models.ViewModels
{
    public class EditCommunityRightToKnow : CommunityRightToKnowViewModel
    {
        public EditCommunityRightToKnow(IContainer container) : base(container) { }
    }
}