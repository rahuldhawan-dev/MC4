using System;
using System.Collections.Generic;
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

namespace MapCallMVC.Areas.Facilities.Models.ViewModels
{
    public class CreateBelowGroundHazard : BelowGroundHazardViewModel
    {
        #region Constructor

        public CreateBelowGroundHazard(IContainer container) : base(container) { }

        [AutoMap(MapDirections.None)]
        public string CoordinateCreateUrl { get; set; }

        public override void SetDefaults()
        {
            if (WorkOrder.HasValue)
            {
                var workOrderPopulatingBelowGroundHazard = _container.GetInstance<IRepository<WorkOrder>>().Find(WorkOrder.Value);

                if (workOrderPopulatingBelowGroundHazard != null)
                {
                    var coordinate = _container.GetInstance<IRepository<Coordinate>>().Save(new Coordinate
                    {
                        Latitude = Convert.ToDecimal(workOrderPopulatingBelowGroundHazard.Latitude),
                        Longitude = Convert.ToDecimal(workOrderPopulatingBelowGroundHazard.Longitude),
                        Icon = _container.GetInstance<IIconSetRepository>()
                                         .GetDefaultIconSet(_container.GetInstance<IRepository<MapIcon>>()).DefaultIcon
                    });
                    WorkOrder = workOrderPopulatingBelowGroundHazard.Id;
                    OperatingCenter = workOrderPopulatingBelowGroundHazard.OperatingCenter?.Id;
                    StreetNumber = int.Parse(workOrderPopulatingBelowGroundHazard.StreetNumber);
                    Street = workOrderPopulatingBelowGroundHazard.Street.Id;
                    Town = workOrderPopulatingBelowGroundHazard.Town?.Id;
                    TownSection = workOrderPopulatingBelowGroundHazard.TownSection?.Id;
                    CrossStreet = workOrderPopulatingBelowGroundHazard.NearestCrossStreet?.Id;
                    Coordinate = coordinate.Id;
                }
            }
        }

        #endregion
    }
}