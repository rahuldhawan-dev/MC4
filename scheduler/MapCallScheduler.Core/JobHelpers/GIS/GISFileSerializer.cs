using System.Collections.Generic;
using System.Linq;
using MapCall.Common.Model.Entities;
using Newtonsoft.Json;

namespace MapCallScheduler.JobHelpers.GIS
{
    public class GISFileSerializer : IGISFileSerializer
    {
        #region Constants

        #endregion

        #region Exposed Methods

        public string Serialize(IQueryable<Hydrant> coll, Formatting formatting = Formatting.None)
        {
            // this select filters the selection list in sql and reduces additional db lookups
            var selected = coll.Select(h => new Hydrant {
                Id = h.Id,
                HydrantBilling = h.HydrantBilling,
                HydrantNumber = h.HydrantNumber,
                DateInstalled = h.DateInstalled,
                Status = h.Status,
                HydrantManufacturer = h.HydrantManufacturer,
                FireDistrict = h.FireDistrict,
                Route = h.Route,
                SAPEquipmentId = h.SAPEquipmentId,
                Stop = h.Stop,
                UpdatedAt = h.UpdatedAt,
                UpdatedBy = h.UpdatedBy,
                WorkOrders = h.WorkOrders,
                OperatingCenter = h.OperatingCenter,
                Town = h.Town,
                FunctionalLocation = h.FunctionalLocation
            });

            // this select simplifies the list finding the most recent
            // work order with the correct work description
            var simplified = selected.Select(h => Models.Hydrant.FromDbRecord(h)).ToList();

            return JsonConvert.SerializeObject(new Models.MapCallSyncMessage(simplified),
                new JsonSerializerSettings {
                    Formatting = formatting,
                    DateFormatString = "o"
                });
        }

        public string Serialize(IQueryable<Valve> coll, Formatting formatting = Formatting.None)
        {
            // this select filters the selection list in sql and reduces additional db lookups
            var selected = coll.Select(v => new Valve {
                Id = v.Id,
                ValveControls = v.ValveControls,
                ValveSize = v.ValveSize,
                DateInstalled = v.DateInstalled,
                Status = v.Status,
                ValveMake = v.ValveMake,
                NormalPosition = v.NormalPosition,
                OpenDirection = v.OpenDirection,
                Route = v.Route,
                SAPEquipmentId = v.SAPEquipmentId,
                Stop = v.Stop,
                Turns = v.Turns,
                ValveNumber = v.ValveNumber,
                ValveType = v.ValveType,
                UpdatedAt = v.UpdatedAt,
                UpdatedBy = v.UpdatedBy,
                WorkOrders = v.WorkOrders,
                OperatingCenter = v.OperatingCenter,
                Town = v.Town,
                FunctionalLocation = v.FunctionalLocation
            });

            // this select simplifies the list finding the most recent
            // work order with the correct work description
            var simplified = selected.Select(v => Models.Valve.FromDbRecord(v)).ToList();

            return JsonConvert.SerializeObject(new Models.MapCallSyncMessage(simplified),
                new JsonSerializerSettings {
                    Formatting = formatting,
                    DateFormatString = "o"
                });
        }

        public string Serialize(IQueryable<SewerOpening> coll, Formatting formatting = Formatting.None)
        {
            // this select filters the selection list in sql and reduces additional db lookups
            var selected = coll.Select(so => new SewerOpening {
                Id = so.Id,
                DepthToInvert = so.DepthToInvert,
                DateInstalled = so.DateInstalled,
                Status = so.Status,
                SewerOpeningMaterial = so.SewerOpeningMaterial,
                RimElevation = so.RimElevation,
                Route = so.Route,
                SAPEquipmentId = so.SAPEquipmentId,
                Stop = so.Stop,
                OpeningNumber = so.OpeningNumber,
                SewerOpeningType = so.SewerOpeningType,
                UpdatedAt = so.UpdatedAt,
                UpdatedBy = so.UpdatedBy,
                WorkOrders = so.WorkOrders,
                OperatingCenter = so.OperatingCenter,
                Town = so.Town,
                FunctionalLocation = so.FunctionalLocation
            });

            // this select simplifies the list finding the most recent
            // work order with the correct work description
            var simplified = selected.Select(o => Models.SewerOpening.FromDbRecord(o)).ToList();

            return JsonConvert.SerializeObject(new Models.MapCallSyncMessage(simplified),
                new JsonSerializerSettings {
                    Formatting = formatting,
                    DateFormatString = "o"
                });
        }

        public string Serialize(
            IQueryable<MostRecentlyInstalledService> coll,
            Formatting formatting = Formatting.None)
        {
            IList<Models.Service> mapped = coll.Select(s => Models.Service.FromDbRecord(s))
                                               .ToList();

            return JsonConvert.SerializeObject(new Models.MapCallSyncMessage(mapped),
                new JsonSerializerSettings {
                    Formatting = formatting,
                    DateFormatString = "o"
                });
        }

        public string Serialize(IQueryable<AsBuiltImage> coll, Formatting formatting = Formatting.None)
        {
            IList<Models.AsBuiltImage> mapped = coll.Select(i => Models.AsBuiltImage.FromDbRecord(i))
                                                    .ToList();

            return JsonConvert.SerializeObject(new Models.MapCallSyncMessage(mapped),
                new JsonSerializerSettings {
                    Formatting = formatting,
                    DateFormatString = "o"
                });
        }
        
        #endregion
    }
}
