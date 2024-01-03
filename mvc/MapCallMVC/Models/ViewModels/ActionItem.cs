using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MMSINC.Authentication;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using StructureMap;

namespace MapCallMVC.Models.ViewModels
{
    public class ActionItemModel : ViewModel<ActionItem>
    {
        #region Properties

        [Secured, DoesNotAutoMap]
        public int? State { get; set; }
        [DropDown, Required, EntityMap, EntityMustExist(typeof(ActionItemType))]
        public int? Type { get; set; }
        [RequiredWhen("Type", ComparisonType.EqualTo, MapCall.Common.Model.Entities.ActionItemType.Indices.NOT_LISTED)]
        public string NotListedType { get; set; }
        [DropDown("", "User", "GetActiveUsersByStateId", DependsOn = "State"), Required, EntityMap, EntityMustExist(typeof(User))]
        public int? ResponsibleOwner { get; set; }
        [Required, Multiline, View("Action Item")]
        public string Note { get; set; }
        [Required]
        public DateTime? TargetedCompletionDate { get; set; }
        public DateTime? DateCompleted { get; set; }
        
        #endregion

        #region Constructors

        public ActionItemModel(IContainer container) : base(container) { }

        #endregion
    }

    public class NewActionItem : ActionItemModel
    {
        #region Properties

        [Required,Secured]
        public virtual int? LinkedId { get; set; }
        [Required,Secured,DoesNotAutoMap]
        public virtual string TableName { get; set; }

        #endregion

        #region Constructor

        public NewActionItem(IContainer container) : base(container) { }

        #endregion

        #region ExposedMethods

        public override ActionItem MapToEntity(ActionItem entity)
        {
            entity = base.MapToEntity(entity);

            entity.CreatedBy = _container.GetInstance<IAuthenticationService<User>>().CurrentUser.UserName;
            entity.DataType = _container.GetInstance<IDataTypeRepository>().GetByTableName(TableName).First();

            return entity;
        }
        #endregion
    }

    public class EditActionItem : ActionItemModel
    {
        [Secured]
        public override int Id { get { return base.Id; } set { base.Id = value; } }

        [Required, DoesNotAutoMap("Used by controller action for redirect")]
        public string Url { get; set; }
        
        #region Constructor

        public EditActionItem(IContainer container) : base(container) { }

        #endregion
    }

    public class DeleteActionItem : ActionItemModel
    {
        [Secured]
        public override int Id { get { return base.Id; } set { base.Id = value; } }

        #region Constructor

        public DeleteActionItem(IContainer container) : base(container) { }

        #endregion
    }
}
