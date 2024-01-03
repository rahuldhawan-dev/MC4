using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MMSINC.Authentication;
using MMSINC.Data;
using StructureMap;

namespace Contractors.Models.ViewModels
{
    public abstract class CrewViewModel : ViewModel<Crew>
    {
        #region Properties

        [Required, StringLength(15)]
        public string Description { get; set; }
        [Required]
        public decimal Availability { get; set; }

        #endregion

        #region Constructors

        public CrewViewModel(IContainer container) : base(container) { }

        #endregion
    }

    public class EditCrew : CrewViewModel
    {
        #region Constructors

        public EditCrew(IContainer container) : base(container) {}

        #endregion
    }

    public class CreateCrew : CrewViewModel
    {
        #region Constructors

        public CreateCrew(IContainer container) : base(container) {}

        #endregion

        #region Methods

        public override Crew MapToEntity(Crew entity)
        {
            entity = base.MapToEntity(entity);
            entity.Contractor = _container
                .GetInstance<IAuthenticationService<ContractorUser>>()
                .CurrentUser.Contractor;
            return entity;
        }

        #endregion
    }
}