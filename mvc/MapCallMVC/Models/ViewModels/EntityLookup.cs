using System.ComponentModel.DataAnnotations;
using MMSINC.Data;
using StructureMap;

namespace MapCallMVC.Models.ViewModels
{
    public class EntityLookupViewModel<TEntity> : ViewModel<TEntity>
        where TEntity : class, IEntityLookup
    {
        #region Properties

        [Required, StringLength(EntityLookup.StringLengths.DESCRIPTION)]
        public virtual string Description { get; set; }

        #endregion

        #region Constructors

        public EntityLookupViewModel(IContainer container) : base(container) {}

        #endregion
    }
}