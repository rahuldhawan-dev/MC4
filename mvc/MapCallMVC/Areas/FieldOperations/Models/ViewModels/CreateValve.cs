using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using DataAnnotationsExtensions;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MMSINC.Authentication;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Utilities;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using IContainer = StructureMap.IContainer;

namespace MapCallMVC.Areas.FieldOperations.Models.ViewModels
{ 
    public class CreateValve : CreateValveBase
    {
        #region Constructors

        public CreateValve(IContainer container, AssetStatusNumberDuplicationValidator numberValidator) :
            base(container, numberValidator) { }

        #endregion

        #region Exposed Methods

        public override Valve MapToEntity(Valve entity)
        {
            SetInspectionFrequency(); // THIS MUST BE CALLED BEFORE base.MapToEntity(entity)
            base.MapToEntity(entity);
            return entity;
        }

        #endregion
    }
}