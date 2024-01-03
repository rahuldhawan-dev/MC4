using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class TrainingContactHoursProgramCoordinator : EntityLookup
    {
        #region Private Members

        private TrainingContactHoursProgramCoordinatorDisplayItem _display;

        #endregion

        #region Properties

        public virtual Employee ProgramCoordinator { get; set; }

        public override string Description => (_display ?? (_display =
            new TrainingContactHoursProgramCoordinatorDisplayItem {
                ProgramCoordinator = ProgramCoordinator
            })).Display;

        #endregion
    }

    [Serializable]
    public class TrainingContactHoursProgramCoordinatorDisplayItem : DisplayItem<TrainingContactHoursProgramCoordinator>
    {
        public Employee ProgramCoordinator { get; set; }

        public override string Display => ProgramCoordinator.ToString();
    }
}
