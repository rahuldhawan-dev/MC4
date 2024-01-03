using System;
using System.Collections.Generic;
using System.Linq;
using MapCall.Common.Model.Entities.MapCall.Common.Model.Entities;
using MapCall.Common.Model.Mappings;
using MMSINC.Data;
using MMSINC.Metadata;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class Chemical : IEntity, IThingWithDocuments, IThingWithNotes
    {
        #region Constants

        public struct StringLengths
        {
            public const int SDS_HYPERLINK = 2048;
        }

        #endregion

        #region Properties

        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual ChemicalType ChemicalType { get; set; }
        public virtual string PartNumber { get; set; }
        public virtual decimal? PricePerPoundWet { get; set; }
        public virtual float? WetPoundsPerGal { get; set; }
        [View("Packaging")]
        public virtual PackagingType PackagingType { get; set; }
        public virtual float? PackagingQuantities { get; set; }
        public virtual string PackagingUnits { get; set; }
        public virtual string ChemicalSymbol { get; set; }
        public virtual string Appearance { get; set; }
        public virtual float? ChemicalConcentrationLiquid { get; set; }
        public virtual float? ConcentrationLbsPerGal { get; set; }
        public virtual float? SpecificGravityMin { get; set; }
        public virtual float? SpecificGravityMax { get; set; }
        public virtual float? RatioResidualProduction { get; set; }

        public virtual IList<StateOfMatter> ChemicalStates { get; set; } = new List<StateOfMatter>();

        [View("CAS Number")]
        public virtual string CasNumber { get; set; }

        [View("SDS Hyperlink")]
        public virtual string SdsHyperlink { get; set; }

        [View("Sub #")]
        public virtual int? SubNumber { get; set; }

        [View("DOT #")]
        public virtual int? DepartmentOfTransportationNumber { get; set; }

        [View("Purity"), BoolFormat("Pure", "Mixture")]
        public virtual bool? IsPure { get; set; }

        public virtual bool? TradeSecret { get; set; }

        [View("EPCRA-Only")]
        public virtual bool? EmergencyPlanningCommunityRightToKnowActOnly { get; set; }

        [View("Health Hazards")]
        public virtual IList<HealthHazard> HealthHazards { get; set; } = new List<HealthHazard>();

        [View("Physical Hazards")]
        public virtual IList<PhysicalHazard> PhysicalHazards { get; set; } = new List<PhysicalHazard>();

        public virtual IList<Document<Chemical>> Documents { get; set; } = new List<Document<Chemical>>();

        public virtual IList<Note<Chemical>> Notes { get; set; } = new List<Note<Chemical>>();

        public virtual IList<IDocumentLink> LinkedDocuments => Documents.Cast<IDocumentLink>().ToList();

        public virtual IList<INoteLink> LinkedNotes => Notes.Cast<INoteLink>().ToList();

        [View("Chemical Forms")]
        // Need to distinct this for chemical storage search, was getting dupe results and display values such as Gas,Gas,Gas
        public virtual string DisplayChemicalStates => string.Join(", ", ChemicalStates.Select(x => x.Description).Distinct());

        public virtual string TableName => nameof(Chemical) + "s";

        public virtual string SpecificGravity => $"{SpecificGravityMin} - {SpecificGravityMax}";
        
        public virtual bool ExtremelyHazardousChemical { get; set; }

        #endregion

        #region Public Methods

        public override string ToString() => new ChemicalDisplayItem {
            Name = Name, 
            ChemicalSymbol = ChemicalSymbol, 
            PartNumber = PartNumber
        }.Display;

        public virtual object ChemicalToJson() => new {
            Id,
            Name,
            ChemicalType = ChemicalType?.Description,
            PartNumber,
            PricePerPoundWet,
            WetPoundsPerGallon = WetPoundsPerGal,
            PackagingType = PackagingType?.Description,
            PackagingUnits,
            ChemicalForms = DisplayChemicalStates,
            Appearance,
            ChemicalConcentrationLiquid,
            ConcentrationPoundsPerGallon = ConcentrationLbsPerGal,
            SpecificGravityMin,
            SpecificGravityMax,
            RatioResidualProduction,
            SdsHyperlink,
            ExtremelyHazardousChemical
        };

        #endregion
    }
}
