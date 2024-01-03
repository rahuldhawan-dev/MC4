using System.Linq;
using MapCallScheduler.JobHelpers.GIS;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallScheduler.Tests.JobHelpers.GIS
{
    [TestClass]
    public class GISFileParserTest
    {
        public const string SAMPLE_INPUT_TEMPLATE = @"{{{{
  ""DataType"": ""{0}_DATA"",
  ""SourceSystem"": ""GIS"",
  ""{1}"": [
    {{0}}
  ]
}}}}";

        public static readonly string HYDRANT_SAMPLE_INPUT_TEMPLATE =
                                          string.Format(SAMPLE_INPUT_TEMPLATE, "HYDRANT", "Hydrants"),
                                      VALVE_SAMPLE_INPUT_TEMPLATE =
                                          string.Format(SAMPLE_INPUT_TEMPLATE, "VALVE", "Valves"),
                                      SEWER_OPENING_SAMPLE_INPUT_TEMPLATE =
                                          string.Format(SAMPLE_INPUT_TEMPLATE, "SEWER_OPENING", "SewerOpenings"),
                                      SERVICE_SAMPLE_INPUT_TEMPLATE =
                                          string.Format(SAMPLE_INPUT_TEMPLATE, "SERVICE", "Services");

        private GISFileParser _target;

        [TestInitialize]
        public void TestInitialize()
        {
            _target = new GISFileParser();
        }

        [TestMethod]
        public void TestParseHydrantsParsesHydrants()
        {
            var result = _target.ParseHydrants(string.Format(HYDRANT_SAMPLE_INPUT_TEMPLATE,
                @"{""Latitude"": 39.539669, ""Id"": 205277, ""Longitude"": -86.085767}, {""Latitude"": 39.541238, ""Id"": 205287, ""Longitude"": -86.088781}, {""Latitude"": 39.53965, ""Id"": 205278, ""Longitude"": -86.087019}"))
                                .ToArray();

            Assert.AreEqual(205277, result[0].Id);
            Assert.AreEqual(39.539669m, result[0].Latitude);
            Assert.AreEqual(-86.085767m, result[0].Longitude);

            Assert.AreEqual(205287, result[1].Id);
            Assert.AreEqual(39.541238m, result[1].Latitude);
            Assert.AreEqual(-86.088781m, result[1].Longitude);

            Assert.AreEqual(205278, result[2].Id);
            Assert.AreEqual(39.53965m, result[2].Latitude);
            Assert.AreEqual(-86.087019m, result[2].Longitude);
        }

        [TestMethod]
        public void TestParseValvesParsesValves()
        {
            var result = _target.ParseValves(string.Format(VALVE_SAMPLE_INPUT_TEMPLATE,
                                     @"{""Latitude"": 39.539669, ""Id"": 205277, ""Longitude"": -86.085767}, {""Latitude"": 39.541238, ""Id"": 205287, ""Longitude"": -86.088781}, {""Latitude"": 39.53965, ""Id"": 205278, ""Longitude"": -86.087019}"))
                                .ToArray();

            Assert.AreEqual(205277, result[0].Id);
            Assert.AreEqual(39.539669m, result[0].Latitude);
            Assert.AreEqual(-86.085767m, result[0].Longitude);

            Assert.AreEqual(205287, result[1].Id);
            Assert.AreEqual(39.541238m, result[1].Latitude);
            Assert.AreEqual(-86.088781m, result[1].Longitude);

            Assert.AreEqual(205278, result[2].Id);
            Assert.AreEqual(39.53965m, result[2].Latitude);
            Assert.AreEqual(-86.087019m, result[2].Longitude);
        }

        [TestMethod]
        public void TestParseSewerOpeningsParsesSewerOpenings()
        {
            var result = _target.ParseSewerOpenings(string.Format(SEWER_OPENING_SAMPLE_INPUT_TEMPLATE,
                                     @"{""Latitude"": 39.539669, ""Id"": 205277, ""Longitude"": -86.085767}, {""Latitude"": 39.541238, ""Id"": 205287, ""Longitude"": -86.088781}, {""Latitude"": 39.53965, ""Id"": 205278, ""Longitude"": -86.087019}"))
                                .ToArray();

            Assert.AreEqual(205277, result[0].Id);
            Assert.AreEqual(39.539669m, result[0].Latitude);
            Assert.AreEqual(-86.085767m, result[0].Longitude);

            Assert.AreEqual(205287, result[1].Id);
            Assert.AreEqual(39.541238m, result[1].Latitude);
            Assert.AreEqual(-86.088781m, result[1].Longitude);

            Assert.AreEqual(205278, result[2].Id);
            Assert.AreEqual(39.53965m, result[2].Latitude);
            Assert.AreEqual(-86.087019m, result[2].Longitude);
        }

        [TestMethod]
        public void TestParseServicesParsesServices()
        {
            var result = _target.ParseServices(string.Format(SERVICE_SAMPLE_INPUT_TEMPLATE,
                                     @"{""Latitude"": 39.539669, ""Id"": 205277, ""Longitude"": -86.085767}, {""Latitude"": 39.541238, ""Id"": 205287, ""Longitude"": -86.088781}, {""Latitude"": 39.53965, ""Id"": 205278, ""Longitude"": -86.087019}"))
                                .ToArray();

            Assert.AreEqual(205277, result[0].Id);
            Assert.AreEqual(39.539669m, result[0].Latitude);
            Assert.AreEqual(-86.085767m, result[0].Longitude);

            Assert.AreEqual(205287, result[1].Id);
            Assert.AreEqual(39.541238m, result[1].Latitude);
            Assert.AreEqual(-86.088781m, result[1].Longitude);

            Assert.AreEqual(205278, result[2].Id);
            Assert.AreEqual(39.53965m, result[2].Latitude);
            Assert.AreEqual(-86.087019m, result[2].Longitude);
        }
    }
}
