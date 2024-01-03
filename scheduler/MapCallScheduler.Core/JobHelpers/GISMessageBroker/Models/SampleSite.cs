using System;
using System.ComponentModel.DataAnnotations;

namespace MapCallScheduler.JobHelpers.GISMessageBroker.Models
{
    public class SampleSite
    {
        #region Properties

        [Required]
        public int Id { get; set; }

        public Coordinate Coordinate { get; set; }
        public Street Street { get; set; }
        public Town Town { get; set; }
        public Facility Facility { get; set; }
        public PublicWaterSupply PublicWaterSupply { get; set; }
        public OperatingCenter OperatingCenter { get; set; }
        public State State { get; set; }
        public ServiceMaterial CustomerPlumbingMaterial { get; set; }
        public SampleSiteStatus Status { get; set; }
        public SampleSiteAvailability Availability { get; set; }
        public SampleSiteCollectionType CollectionType { get; set; }
        public SampleSiteLocationType LocationType { get; set; }
        public int? ParentSiteId { get; set; }
        public string StreetNumber { get; set; }
        public string Zip { get; set; }
        public string CommonSiteName { get; set; }
        public string LocationNameDescription { get; set; }
        public string PremiseNumber { get; set; }
        public bool? LeadCopperSite { get; set; }
        public string AgencyId { get; set; }
        public string LimsFacilityId { get; set; }
        public string LimsSiteId { get; set; }
        public string LimsPrimaryStationCode { get; set; }
        public int? LimsSequenceNumber { get; set; }
        public SampleSiteProfile LimsProfile { get; set; }
        public bool? CustomerParticipationConfirmed { get; set; }
        public bool? IsAlternateSite { get; set; }
        public bool? IsComplianceSampleSite { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerHomePhone { get; set; }
        public virtual bool? IsLimsLocation { get; set; }
        public bool BactiSite { get; set; }
        public DateTime? ValidatedAt { get; set; }
        public virtual Employee ValidatedBy { get; set; }
        public virtual SampleSiteValidationStatus ValidationStatus { get; set; }
        public virtual SampleSiteInactivationReason InactivationReason { get; set; }

        #endregion

        #region Exposed Methods

        public static SampleSite FromDbRecord(MapCall.Common.Model.Entities.SampleSite site)
        {
            // Why is this assignments instead of one massive initializer? easier to debug when
            // something goes wrong, we'll get an exactly line number, instead of the line number
            // of the initializer.

            var sampleSite = new SampleSite();
            sampleSite.Coordinate = site.Coordinate == null
                ? null
                : new Coordinate {
                    Latitude = site.Coordinate.Latitude,
                    Longitude = site.Coordinate.Longitude
                };
            sampleSite.StreetNumber = site.StreetNumber;
            sampleSite.Street = site.Street == null 
                ? null 
                : new Street {
                    Id = site.Street.Id,
                    Prefix = site.Street.Prefix == null 
                        ? null 
                        : new StreetPrefix {
                            Id = site.Street.Prefix.Id,
                            Description = site.Street.Prefix.Description
                        },
                    Suffix = site.Street.Suffix == null 
                        ? null 
                        : new StreetSuffix {
                            Id = site.Street.Suffix.Id,
                            Description = site.Street.Suffix.Description
                        },
                    Name = site.Street.Name
                };
            sampleSite.Town = site.Town == null
                ? null
                : new Town {
                    Id = site.Town.Id,
                    FullName = site.Town.FullName
                };
            sampleSite.Zip = site.ZipCode;
            sampleSite.CommonSiteName = site.CommonSiteName;
            sampleSite.LocationNameDescription = site.LocationNameDescription;
            sampleSite.Id = site.Id;
            sampleSite.PremiseNumber = site.Premise?.PremiseNumber;
            sampleSite.Facility = site.Facility == null
                ? null
                : new Facility {
                    Id = site.Facility.Id,
                    FacilityName = site.Facility.FacilityName
                };
            sampleSite.PublicWaterSupply = site.PublicWaterSupply == null
                ? null
                : new PublicWaterSupply {
                    Id = site.PublicWaterSupply.Id,
                    Identifier = site.PublicWaterSupply.Identifier,
                    Status = site.PublicWaterSupply.Status == null
                        ? null
                        : new PublicWaterSupplyStatus {
                            Id = site.PublicWaterSupply.Status.Id,
                            Description = site.PublicWaterSupply.Status.Description
                        }
                };
            sampleSite.OperatingCenter = site.OperatingCenter == null
                ? null
                : new OperatingCenter {
                    Id = site.OperatingCenter.Id,
                    OperatingCenterCode = site.OperatingCenter.OperatingCenterCode,
                    OperatingCenterName = site.OperatingCenter.OperatingCenterName
                };
            sampleSite.State = site.State == null
                ? null
                : new State {
                    Id = site.State.Id,
                    Abbreviation = site.State.Abbreviation
                };
            sampleSite.CustomerPlumbingMaterial = site.CustomerPlumbingMaterial == null
                ? null
                : new ServiceMaterial {
                    Id = site.CustomerPlumbingMaterial.Id,
                    Description = site.CustomerPlumbingMaterial.Description
                };
            sampleSite.LeadCopperSite = site.LeadCopperSite;
            sampleSite.Status = site.Status == null
                ? null
                : new SampleSiteStatus {
                    Id = site.Status.Id,
                    Description = site.Status.Description
                };
            sampleSite.Availability = site.Availability == null
                ? null
                : new SampleSiteAvailability {
                    Id = site.Availability.Id,
                    Description = site.Availability.Description
                };
            sampleSite.CollectionType = site.CollectionType == null
                ? null
                : new SampleSiteCollectionType {
                    Id = site.CollectionType.Id,
                    Description = site.CollectionType.Description
                };
            sampleSite.LocationType = site.LocationType == null
                ? null
                : new SampleSiteLocationType {
                    Id = site.LocationType.Id,
                    Description = site.LocationType.Description
                };
            sampleSite.ParentSiteId = site.ParentSite?.Id;
            sampleSite.AgencyId = site.AgencyId;
            sampleSite.CustomerParticipationConfirmed = site.CustomerParticipationConfirmed;
            sampleSite.IsAlternateSite = site.IsAlternateSite;
            sampleSite.IsComplianceSampleSite = site.IsComplianceSampleSite;
            sampleSite.CustomerName = site.CustomerName;
            sampleSite.CustomerEmail = site.CustomerEmail;
            sampleSite.CustomerHomePhone = site.CustomerHomePhone;
            sampleSite.IsLimsLocation = site.IsLimsLocation;
            sampleSite.LimsFacilityId = site.LimsFacilityId;
            sampleSite.LimsPrimaryStationCode = site.LimsPrimaryStationCode;
            sampleSite.LimsSiteId = site.LimsSiteId;
            sampleSite.LimsSequenceNumber = site.LimsSequenceNumber;
            sampleSite.LimsProfile = site.SampleSiteProfile == null
                ? null
                : new SampleSiteProfile {
                    Id = site.SampleSiteProfile.Id,
                    Name = site.SampleSiteProfile.Name,
                    Number = site.SampleSiteProfile.Number,
                    SampleSiteProfileAnalysisType = new SampleSiteProfileAnalysisType {
                        Id = site.SampleSiteProfile.SampleSiteProfileAnalysisType.Id,
                        Description = site.SampleSiteProfile.SampleSiteProfileAnalysisType.Description,
                    },
                    PublicWaterSupply = new PublicWaterSupply {
                        Id = site.SampleSiteProfile.PublicWaterSupply.Id,
                        Identifier = site.SampleSiteProfile.PublicWaterSupply.Identifier,
                        Status = site.SampleSiteProfile.PublicWaterSupply.Status == null
                            ? null
                            : new PublicWaterSupplyStatus {
                                Id = site.SampleSiteProfile.PublicWaterSupply.Status.Id,
                                Description = site.SampleSiteProfile.PublicWaterSupply.Status.Description
                            }
                    }
                };
            sampleSite.BactiSite = site.BactiSite;
            sampleSite.ValidatedAt = site.ValidatedAt?.ToUniversalTime();
            sampleSite.ValidatedBy = site.ValidatedBy == null
                ? null
                : new Employee {
                    Id = site.ValidatedBy.Id,
                    EmployeeId = site.ValidatedBy.EmployeeId,
                    FirstName = site.ValidatedBy.FirstName,
                    MiddleName = site.ValidatedBy.MiddleName,
                    LastName = site.ValidatedBy.LastName,
                    EmailAddress = site.ValidatedBy.EmailAddress
                };
            sampleSite.ValidationStatus = site.SampleSiteValidationStatus == null
                ? null
                : new SampleSiteValidationStatus {
                    Id = site.SampleSiteValidationStatus.Id,
                    Description = site.SampleSiteValidationStatus.Description
                };
            sampleSite.InactivationReason = site.SampleSiteInactivationReason == null
                ? null
                : new SampleSiteInactivationReason {
                    Id = site.SampleSiteInactivationReason.Id,
                    Description = site.SampleSiteInactivationReason.Description
                };
            return sampleSite;
        }

        #endregion
    }
}
