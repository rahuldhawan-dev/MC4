using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Validation;
using StructureMap;

namespace MapCallMVC.Models.ViewModels
{
    public class TrainingContactHoursProgramCoordinatorViewModel : ViewModel<TrainingContactHoursProgramCoordinator>
    {
        #region Properties

        [DropDown, Required, EntityMustExist(typeof(Employee)), EntityMap]
        public int? ProgramCoordinator { get; set; }

        #endregion

        #region Constructors

        public TrainingContactHoursProgramCoordinatorViewModel(IContainer container) : base(container) {}

        #endregion
    }

    public class CreateTrainingContactHoursProgramCoordinator : TrainingContactHoursProgramCoordinatorViewModel
    {
        #region Constructors

        public CreateTrainingContactHoursProgramCoordinator(IContainer container) : base(container) { }

        #endregion
    }

    public class EditTrainingContactHoursProgramCoordinator : TrainingContactHoursProgramCoordinatorViewModel
    {
        public EditTrainingContactHoursProgramCoordinator(IContainer container) : base(container) { }
    }

    // There's nothing to search for?
    public class SearchTrainingContactHoursProgramCoordinator : SearchSet<TrainingContactHoursProgramCoordinator> { }
}