using System;
using MapCall.Common.Model.Entities;
using MMSINC.Data;

namespace MapCall.Common.Model.ViewModels
{
    public class PremiseCoordinate : ThingWithFlattenedCoordinateBase { }

    public interface ISearchPremiseForMap : ISearchSet<PremiseCoordinate> { }
}
