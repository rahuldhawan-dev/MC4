using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapCallMVC.Areas.Engineering.Models.ViewModels.ArcFlash;
using StructureMap;

namespace MapCallImporter.ViewModels
{
    public class MyCreateArcFlashStudy : CreateArcFlashStudyBase
    {
        #region Constructors

        public MyCreateArcFlashStudy(IContainer container) : base(container) { }

        #endregion
    }
}
