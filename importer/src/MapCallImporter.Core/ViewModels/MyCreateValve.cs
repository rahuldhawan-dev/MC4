using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using MMSINC.Data.NHibernate;
using MMSINC.Utilities.ObjectMapping;
using StructureMap;

namespace MapCallImporter.ViewModels
{
    /// <summary>
    /// We're inheriting from CreateValveBase here because we do not want the logic
    /// for inheriting inspection frequencies that lives in CreateValve and EditValve view models.
    /// </summary>
    /// <seealso cref="MapCallMVC.Areas.FieldOperations.Models.ViewModels.CreateValveBase" />
    public class MyCreateValve : CreateValveBase
    {
        #region Properties

        public int? SAPEquipmentId { get; set; }
        public int? ObjectID { get; set; }

        [DoesNotAutoMap]
        public string ValveNumber { get; set; }

        #endregion

        #region Constructors

        public MyCreateValve(IContainer container, AssetStatusNumberDuplicationValidator numberDuplicationValidator) : base(container, numberDuplicationValidator) {}

        #endregion

        #region Private Methods

        protected override bool ValveNumberIsUniqueToOperatingCenter(IRepository<Valve> valveRepository, OperatingCenter operatingCenter, ValveNumber number)
        {
            return !valveRepository.FindByOperatingCenterAndValveNumber(operatingCenter, ValveNumber).Any();
        }

        #endregion

        #region Exposed Methods

        public override IEnumerable<ValidationResult> ValidateValveSuffixForFoundValve()
        {
            return Enumerable.Empty<ValidationResult>();
        }

        #endregion
    }
}