using MapCall.Common.Model.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using MapCall.Common.Model.Entities.Users;
using MapCall.SAP;

namespace MapCall.SAP.Model.Entities.Tests
{
    [TestClass()]
    public class SAPInspectionTests
    {
        #region Private Members

        private HydrantInspection hydrantInspection;
        private ValveInspection valveInspection;
        private BlowOffInspection blowOffInspection;
        private SewerMainCleaning sewerMainCleaning;

        #endregion

        #region Private Methods

        public HydrantInspection SetHydrantInspectionValues()
        {
            hydrantInspection = new HydrantInspection();
            hydrantInspection.Hydrant = new Hydrant {
                HydrantNumber = "HBH-96", SAPEquipmentId = 20004156,
                FunctionalLocation = new FunctionalLocation {Description = "NJOC-BH-HYDRT"}
            };
            hydrantInspection.HydrantInspectionType = new HydrantInspectionType {Description = "INSPECT/FLUSH"};
            hydrantInspection.DateInspected = Convert.ToDateTime("4/14/2016 12:16 PM");
            hydrantInspection.InspectedBy = new User {UserName = "casaburroc"};
            hydrantInspection.Remarks =
                "System inspections using fire hydrants will be performed in the datime. For each hydrant, the technician will fill out an inspection sheet listing the following  information:            a.Hydrant number on plan and hydrantb.Civic number closest to fire hydrant, and sector(e.g.Residential)c.Hydrant make, model and diameterd.Accessibility of fire hydrante.Accessibility to operating nut on hydrant supply pipe valvef.Description of supply pipe valve operating conditiong.Number and diameter of hose outletsh.Hose outlet and cap conditioni.Hose outlet thread pitchj.Flange height from groundk.Condition of paintl.Distance between water level inside hydrant and ground levelm.Hydrant depth, i.e.distance between boot and flangesn.Description of any interior damage for each part, for instance :• Defective retaining chains or cables• Defective operating sleeve• Cracked barrel• Defective bearing• Leaking gate - type disc• Twisted stemEnd of long text.";
            hydrantInspection.StaticPressure = 58;
            hydrantInspection.GPM = 960;
            hydrantInspection.MinutesFlowed = 5;
            hydrantInspection.ResidualChlorine = Convert.ToDecimal(0.92);
            hydrantInspection.TotalChlorine = 0;

            return hydrantInspection;
        }

        public ValveInspection SetValveInspectionValues()
        {
            valveInspection = new ValveInspection();
            //hydrantInspection.id = 720611;
            valveInspection.Valve = new Valve {
                ValveNumber = "VGT-4558", SAPEquipmentId = 10011768,
                FunctionalLocation = new FunctionalLocation {Description = "NJBC-GT-VALVE"}
            };
            valveInspection.DateInspected = Convert.ToDateTime("4/5/2013 1:04 PM");
            valveInspection.InspectedBy = new User {UserName = "murphymi"};
            valveInspection.Remarks = "";
            valveInspection.Inspected = true;
            valveInspection.PositionFound = new ValveNormalPosition {Description = "Open"};
            valveInspection.PositionLeft = new ValveNormalPosition {Description = "Open"};
            valveInspection.Valve.OpenDirection = new ValveOpenDirection {Description = "Left"};
            valveInspection.Turns = 0;

            return valveInspection;
        }

        public BlowOffInspection SetBlowOffInspectionValues()
        {
            blowOffInspection = new BlowOffInspection();
            blowOffInspection.Valve = new Valve {
                ValveNumber = "VRB-7", SAPEquipmentId = 10011769,
                FunctionalLocation = new FunctionalLocation {Description = "NJBC-GT-VALVE"},
                Turns = Convert.ToDecimal(0.25)
            };
            blowOffInspection.DateInspected = Convert.ToDateTime("2/22/2013 3:04 PM");
            blowOffInspection.InspectedBy = new User {UserName = "crawfordk"};
            blowOffInspection.HydrantInspectionType = new HydrantInspectionType {Description = "FLUSH"};
            blowOffInspection.Remarks = "";
            blowOffInspection.Valve.OpenDirection = new ValveOpenDirection {Description = "Left"};
            blowOffInspection.ResidualChlorine = 1;
            blowOffInspection.TotalChlorine = 1;
            blowOffInspection.GPM = 1;
            blowOffInspection.StaticPressure = 1;
            blowOffInspection.MinutesFlowed = 1;

            return blowOffInspection;
        }

        public SewerMainCleaning SetOpeningInspectionValues()
        {
            sewerMainCleaning = new SewerMainCleaning();
            sewerMainCleaning.Opening1 = new SewerOpening {
                OpeningNumber = "MLK-3387", SAPEquipmentId = 5007619,
                FunctionalLocation = new FunctionalLocation {Description = "NJMPW-MP-MAINS-0001"}
            };
            sewerMainCleaning.Opening1Condition = new OpeningCondition {Description = "BENCH WORK REQUIRED"};
            sewerMainCleaning.Opening1FrameAndCover = new OpeningFrameAndCover {Description = "HIGH"};

            sewerMainCleaning.Opening2 = new SewerOpening {
                OpeningNumber = "MLK-3386", SAPEquipmentId = 5007619,
                FunctionalLocation = new FunctionalLocation {Description = "NJMPW-MP-MAINS-0001"}
            };
            sewerMainCleaning.Opening2Condition = new OpeningCondition {Description = "INFILTRATION"};
            sewerMainCleaning.Opening2FrameAndCover = new OpeningFrameAndCover {Description = "LOW"};
            sewerMainCleaning.HydrantUsed = new Hydrant {HydrantNumber = "HOC-7"};
            sewerMainCleaning.TableNotes = "Notes text";
            sewerMainCleaning.InspectedDate = Convert.ToDateTime("5/5/2014 1:04 PM");
            sewerMainCleaning.CreatedBy = new User {UserName = "murphymi"};
            sewerMainCleaning.GallonsOfWaterUsed = 1;
            sewerMainCleaning.FootageOfMainInspected = 1;
            return sewerMainCleaning;
        }

        #endregion

        [TestInitialize]
        public void TestInitialize()
        {
            hydrantInspection = SetHydrantInspectionValues();
            valveInspection = SetValveInspectionValues();
            blowOffInspection = SetBlowOffInspectionValues();
            sewerMainCleaning = SetOpeningInspectionValues();
        }

        [TestMethod()]
        public void TestConstructorSetsPropertiesForHydrant()
        {
            var target = new SAPInspection(hydrantInspection);
            Assert.AreEqual("HYDRANT", target.AssetType?.ToUpper());
            Assert.AreEqual(hydrantInspection.HydrantInspectionType.Description, target.InspectionType);
            Assert.AreEqual(hydrantInspection.Hydrant.FunctionalLocation.Description, target.FunctionalLocation);
            Assert.AreEqual(hydrantInspection.Hydrant.SAPEquipmentNumber, target.EquipmentNo);
            Assert.AreEqual(hydrantInspection.DateInspected.Date.ToString("yyyyMMdd"), target.DateOfNotification);
            Assert.AreEqual(hydrantInspection.DateInspected.ToString("HHmmss"), target.NotificationTime);
            Assert.AreEqual(hydrantInspection.InspectedBy.UserName?.ToUpper(), target.ReportedBy);
            Assert.AreEqual(
                hydrantInspection.InspectedBy?.FullName?.ToUpper() + ":\n" + hydrantInspection.Remarks.ToUpper(),
                target.NotificationLongText);
            Assert.AreEqual(hydrantInspection.InspectedBy.UserName?.ToUpper(), target.TechInspectionBy);
            Assert.AreEqual(hydrantInspection.DateInspected.ToString("yyyyMMdd"), target.TechInspectionOn);
            Assert.AreEqual(hydrantInspection.HydrantInspectionType.Description.ToString(), target.InspectionType);
            Assert.AreEqual(hydrantInspection.DateInspected.ToString("yyyyMMdd"), target.StartDate);
            Assert.AreEqual(hydrantInspection.DateInspected.ToString("HHmmss"), target.StartTimeOfActivity);
            Assert.AreEqual(hydrantInspection.DateInspected.Date.ToString("yyyyMMdd"), target.EndDate);
            Assert.AreEqual(hydrantInspection.DateInspected.ToString("HHmmss"), target.EndTimeOfActivity);
            Assert.AreEqual(hydrantInspection.StaticPressure.ToString(), target.StaticPressure);
            Assert.AreEqual(hydrantInspection.GPM.ToString(), target.Flow1GPM);
            Assert.AreEqual(hydrantInspection.MinutesFlowed.ToString(), target.Flow1Minutes);
            Assert.AreEqual(hydrantInspection.ResidualChlorine.ToString(), target.ResidualChlorine);
            Assert.AreEqual(hydrantInspection.TotalChlorine.ToString(), target.TotalChlorine);
        }

        [TestMethod()]
        public void TestConstructorSetsPropertiesForValve()
        {
            var target = new SAPInspection(valveInspection);
            Assert.AreEqual("STREET VALVE", target.AssetType);
            Assert.AreEqual(valveInspection.Valve.FunctionalLocation.Description, target.FunctionalLocation);
            Assert.AreEqual(valveInspection.Valve.SAPEquipmentNumber, target.EquipmentNo);
            Assert.AreEqual(valveInspection.DateInspected.Date.ToString("yyyyMMdd"), target.DateOfNotification);
            Assert.AreEqual(valveInspection.DateInspected.ToString("HHmmss"), target.NotificationTime);
            Assert.AreEqual(valveInspection.InspectedBy?.UserName?.ToUpper(), target.ReportedBy);
            Assert.AreEqual(valveInspection.InspectedBy?.FullName?.ToUpper() + ":\n" + valveInspection.Remarks,
                target.NotificationLongText);
            Assert.AreEqual(valveInspection.InspectedBy.UserName?.ToUpper(), target.TechInspectionBy);
            Assert.AreEqual(valveInspection.DateInspected.ToString("yyyyMMdd"), target.TechInspectionOn);
            Assert.AreEqual(valveInspection.DateInspected.ToString("yyyyMMdd"), target.StartDate);
            Assert.AreEqual(valveInspection.DateInspected.ToString("HHmmss"), target.StartTimeOfActivity);
            Assert.AreEqual(valveInspection.DateInspected.Date.ToString("yyyyMMdd"), target.EndDate);
            Assert.AreEqual(valveInspection.DateInspected.ToString("HHmmss"), target.EndTimeOfActivity);
            Assert.AreEqual(valveInspection.Inspected == true ? "YES" : "NO", target.Operated);
            Assert.AreEqual(valveInspection.PositionFound.Description.ToUpper(), target.PositionFound);
            Assert.AreEqual(valveInspection.PositionLeft.Description.ToUpper(), target.PositionLeft);
            Assert.AreEqual(valveInspection.Valve.OpenDirection.Description.ToUpper(), target.OpenDirection);
            Assert.AreEqual(valveInspection.Turns.ToString(), target.noTurnsOperated);
        }

        [TestMethod()]
        public void TestConstructorSetsPropertiesForBlowOff()
        {
            var target = new SAPInspection(blowOffInspection);
            Assert.AreEqual("BLOW-OFF VALVE", target.AssetType);
            Assert.AreEqual(blowOffInspection.HydrantInspectionType?.Description, target.InspectionType);
            Assert.AreEqual(blowOffInspection.Valve.FunctionalLocation.Description, target.FunctionalLocation);
            Assert.AreEqual(blowOffInspection.Valve.SAPEquipmentNumber, target.EquipmentNo);
            Assert.AreEqual(blowOffInspection.DateInspected.Date.ToString("yyyyMMdd"), target.DateOfNotification);
            Assert.AreEqual(blowOffInspection.DateInspected.ToString("HHmmss"), target.NotificationTime);
            Assert.AreEqual(blowOffInspection.InspectedBy?.UserName?.ToUpper(), target.ReportedBy);
            Assert.AreEqual(blowOffInspection.InspectedBy?.FullName?.ToUpper() + ":\n" + blowOffInspection.Remarks,
                target.NotificationLongText);
            Assert.AreEqual(blowOffInspection.InspectedBy.UserName?.ToUpper(), target.TechInspectionBy);
            Assert.AreEqual(blowOffInspection.DateInspected.ToString("yyyyMMdd"), target.TechInspectionOn);
            Assert.AreEqual(blowOffInspection.DateInspected.ToString("yyyyMMdd"), target.StartDate);
            Assert.AreEqual(blowOffInspection.DateInspected.ToString("HHmmss"), target.StartTimeOfActivity);
            Assert.AreEqual(blowOffInspection.DateInspected.Date.ToString("yyyyMMdd"), target.EndDate);
            Assert.AreEqual(blowOffInspection.DateInspected.ToString("HHmmss"), target.EndTimeOfActivity);
            Assert.AreEqual(blowOffInspection.Valve.OpenDirection.Description.ToUpper(), target.OpenDirection);
            Assert.AreEqual(blowOffInspection.ResidualChlorine?.ToString(), target.ResidualChlorine?.ToString());
            Assert.AreEqual(blowOffInspection.Valve?.Turns?.ToString(), target.noTurnsOperated?.ToString());
            Assert.AreEqual(blowOffInspection.TotalChlorine?.ToString(), target.TotalChlorine?.ToString());
            Assert.AreEqual(blowOffInspection.StaticPressure?.ToString(), target.StaticPressure?.ToString());
            Assert.AreEqual(blowOffInspection.GPM?.ToString(), target.Flow1GPM?.ToString());
            Assert.AreEqual(blowOffInspection.MinutesFlowed?.ToString(), target.Flow1Minutes?.ToString());
        }

        [TestMethod()]
        public void TestConstructorSetsPropertiesForOpening()
        {
            var target = new SAPInspection(sewerMainCleaning);
            Assert.AreEqual("OPENING", target.AssetType);

            string opening1Number,
                   opening1Condition,
                   opening1FrameAndCover,
                   opening2Number,
                   opening2Condition,
                   opening2FrameAndCover,
                   hydrantUsed,
                   notes,
                   notificationLongText;

            opening1Number = sewerMainCleaning.Opening1 != null
                ? "Opening 1: " + sewerMainCleaning.Opening1?.OpeningNumber.ToUpper()
                : string.Empty;
            opening1Condition = sewerMainCleaning.Opening1Condition != null
                ? "Opening 1 Condition:" + sewerMainCleaning.Opening1Condition?.Description.ToUpper()
                : string.Empty;
            opening1FrameAndCover = sewerMainCleaning.Opening1FrameAndCover != null
                ? "Opening 1 Frame And Cover:" + sewerMainCleaning.Opening1FrameAndCover?.Description.ToUpper()
                : string.Empty;
            opening2Number = sewerMainCleaning.Opening2 != null
                ? "Opening 2: " + sewerMainCleaning.Opening2?.OpeningNumber.ToUpper()
                : string.Empty;
            opening2Condition = sewerMainCleaning.Opening2Condition != null
                ? "Opening 2 Condition: " + sewerMainCleaning.Opening2Condition?.Description.ToUpper()
                : string.Empty;
            opening2FrameAndCover = sewerMainCleaning.Opening2FrameAndCover != null
                ? "Opening 2 Frame And Cover: " + sewerMainCleaning.Opening2FrameAndCover?.Description.ToUpper()
                : string.Empty;
            hydrantUsed = sewerMainCleaning.HydrantUsed != null
                ? "Hydrant Used: " + sewerMainCleaning.HydrantUsed.HydrantNumber.ToUpper()
                : string.Empty;
            notes = sewerMainCleaning.TableNotes != null
                ? "Notes: " + sewerMainCleaning.TableNotes.ToUpper()
                : string.Empty;

            notificationLongText = "CREATED BY : " + sewerMainCleaning.CreatedBy?.FullName?.ToUpper() + "\n" +
                                   opening1Number + "\n" + opening1Condition + "\n" + opening1FrameAndCover + "\n" +
                                   opening2Number + "\n" + opening2Condition + "\n" + opening2FrameAndCover + "\n" +
                                   hydrantUsed + "\n" + notes;

            Assert.AreEqual(sewerMainCleaning.Opening1.FunctionalLocation.Description, target.FunctionalLocation);
            Assert.AreEqual(sewerMainCleaning.Opening1.SAPEquipmentNumber, target.EquipmentNo);
            Assert.AreEqual(sewerMainCleaning.InspectedDate?.Date.ToString("yyyyMMdd"), target.DateOfNotification);
            Assert.AreEqual(sewerMainCleaning.InspectedDate?.ToString("HHmmss"), target.NotificationTime);
            Assert.AreEqual(sewerMainCleaning.CreatedBy.UserName?.ToUpper(), target.ReportedBy);
            Assert.AreEqual(sewerMainCleaning.CreatedBy.UserName?.ToUpper(), target.TechInspectionBy);
            Assert.AreEqual(sewerMainCleaning.InspectedDate?.ToString("yyyyMMdd"), target.TechInspectionOn);
            Assert.AreEqual(sewerMainCleaning.InspectedDate?.ToString("yyyyMMdd"), target.StartDate);
            Assert.AreEqual(sewerMainCleaning.InspectedDate?.ToString("HHmmss"), target.StartTimeOfActivity);
            Assert.AreEqual(sewerMainCleaning.InspectedDate?.Date.ToString("yyyyMMdd"), target.EndDate);
            Assert.AreEqual(sewerMainCleaning.InspectedDate?.ToString("HHmmss"), target.EndTimeOfActivity);
            Assert.AreEqual(sewerMainCleaning.GallonsOfWaterUsed.ToString(), target.FlushWaterVolume.ToString());
            Assert.AreEqual(sewerMainCleaning.FootageOfMainInspected.ToString(), target.FootageOfMainCleaned.ToString());
            Assert.AreEqual(notificationLongText, target.NotificationLongText);
        }

        [TestMethod()]
        public void TestConstructorDoesNotFailWithNullsForHydrant()
        {
            hydrantInspection = new HydrantInspection();
            var target = new SAPInspection(hydrantInspection);
            Assert.AreEqual("HYDRANT", target.AssetType);

            Assert.AreEqual(hydrantInspection.HydrantInspectionType?.Description, target.InspectionType);
            Assert.AreEqual(hydrantInspection.Hydrant?.FunctionalLocation.Description, target.FunctionalLocation);
            Assert.AreEqual(hydrantInspection.Hydrant?.SAPEquipmentNumber, target.EquipmentNo);
            Assert.AreEqual(
                hydrantInspection.DateInspected != null
                    ? hydrantInspection.DateInspected.Date.ToString("yyyyMMdd")
                    : null, target.DateOfNotification);
            Assert.AreEqual(
                hydrantInspection.DateInspected != null ? hydrantInspection.DateInspected.ToString("HHmmss") : null,
                target.NotificationTime);
            Assert.AreEqual(hydrantInspection.InspectedBy?.UserName?.ToUpper(), target.ReportedBy);
            Assert.AreEqual(hydrantInspection.InspectedBy?.FullName?.ToUpper() + ":\n" + hydrantInspection.Remarks,
                target.NotificationLongText);
            Assert.AreEqual(hydrantInspection.InspectedBy?.UserName?.ToUpper(), target.TechInspectionBy);
            Assert.AreEqual(
                hydrantInspection.DateInspected != null ? hydrantInspection.DateInspected.ToString("yyyyMMdd") : null,
                target.TechInspectionOn);
            Assert.AreEqual(hydrantInspection.HydrantInspectionType?.Description.ToString(), target.InspectionType);
            Assert.AreEqual(
                hydrantInspection.DateInspected != null ? hydrantInspection.DateInspected.ToString("yyyyMMdd") : null,
                target.StartDate);
            Assert.AreEqual(
                hydrantInspection.DateInspected != null ? hydrantInspection.DateInspected.ToString("HHmmss") : null,
                target.StartTimeOfActivity);
            Assert.AreEqual(
                hydrantInspection.DateInspected != null
                    ? hydrantInspection.DateInspected.Date.ToString("yyyyMMdd")
                    : null, target.EndDate);
            Assert.AreEqual(
                hydrantInspection.DateInspected != null ? hydrantInspection.DateInspected.ToString("HHmmss") : null,
                target.EndTimeOfActivity);
            Assert.AreEqual(hydrantInspection.StaticPressure?.ToString(), target.StaticPressure);
            Assert.AreEqual(hydrantInspection.GPM?.ToString(), target.Flow1GPM);
            Assert.AreEqual(hydrantInspection.MinutesFlowed?.ToString(), target.Flow1Minutes);
            Assert.AreEqual(hydrantInspection.ResidualChlorine?.ToString(), target.ResidualChlorine);
            Assert.AreEqual(hydrantInspection.TotalChlorine?.ToString(), target.TotalChlorine);
        }

        [TestMethod()]
        public void TestConstructorDoesNotFailWithNullsForValve()
        {
            valveInspection = new ValveInspection();
            var target = new SAPInspection(valveInspection);
            Assert.AreEqual("STREET VALVE", target.AssetType);

            Assert.AreEqual(valveInspection.Valve?.FunctionalLocation.Description, target.FunctionalLocation);
            Assert.AreEqual(valveInspection.Valve?.SAPEquipmentNumber, target.EquipmentNo);
            Assert.AreEqual(
                valveInspection.DateInspected != null ? valveInspection.DateInspected.Date.ToString("yyyyMMdd") : null,
                target.DateOfNotification);
            Assert.AreEqual(
                valveInspection.DateInspected != null ? valveInspection.DateInspected.ToString("HHmmss") : null,
                target.NotificationTime);
            Assert.AreEqual(valveInspection.InspectedBy?.UserName?.ToUpper(), target.ReportedBy);
            Assert.AreEqual(valveInspection.InspectedBy?.FullName.ToUpper() + ":\n" + valveInspection.Remarks,
                target.NotificationLongText);
            Assert.AreEqual(valveInspection.InspectedBy?.UserName?.ToUpper(), target.TechInspectionBy);
            Assert.AreEqual(
                valveInspection.DateInspected != null ? valveInspection.DateInspected.ToString("yyyyMMdd") : null,
                target.TechInspectionOn);
            Assert.AreEqual(
                valveInspection.DateInspected != null ? valveInspection.DateInspected.ToString("yyyyMMdd") : null,
                target.StartDate);
            Assert.AreEqual(
                valveInspection.DateInspected != null ? valveInspection.DateInspected.ToString("HHmmss") : null,
                target.StartTimeOfActivity);
            Assert.AreEqual(
                valveInspection.DateInspected != null ? valveInspection.DateInspected.Date.ToString("yyyyMMdd") : null,
                target.EndDate);
            Assert.AreEqual(
                valveInspection.DateInspected != null ? valveInspection.DateInspected.ToString("HHmmss") : null,
                target.EndTimeOfActivity);
            Assert.AreEqual(valveInspection.Inspected == true ? "YES" : "NO", target.Operated);
            Assert.AreEqual(valveInspection.PositionFound?.Description.ToUpper(), target.PositionFound);
            Assert.AreEqual(valveInspection.PositionLeft?.Description.ToUpper(), target.PositionLeft);
            Assert.AreEqual(valveInspection.Valve?.OpenDirection.Description.ToUpper(), target.OpenDirection);
            Assert.AreEqual(valveInspection.Turns?.ToString(), target.noTurnsOperated);
        }

        [TestMethod()]
        public void TestConstructorDoesNotFailWithNullsForBlowOff()
        {
            blowOffInspection = new BlowOffInspection();
            var target = new SAPInspection(blowOffInspection);
            Assert.AreEqual("BLOW-OFF VALVE", target.AssetType);
            Assert.AreEqual(blowOffInspection.HydrantInspectionType?.Description, target.InspectionType);
            Assert.AreEqual(blowOffInspection.Valve?.FunctionalLocation.Description, target.FunctionalLocation);
            Assert.AreEqual(blowOffInspection.Valve?.SAPEquipmentNumber, target.EquipmentNo);
            Assert.AreEqual(
                blowOffInspection.DateInspected != null
                    ? blowOffInspection.DateInspected.Date.ToString("yyyyMMdd")
                    : null, target.DateOfNotification);
            Assert.AreEqual(
                blowOffInspection.DateInspected != null ? blowOffInspection.DateInspected.ToString("HHmmss") : null,
                target.NotificationTime);
            Assert.AreEqual(blowOffInspection.InspectedBy?.UserName.ToUpper(), target.ReportedBy);
            Assert.AreEqual(
                blowOffInspection.InspectedBy?.FullName?.ToUpper() + ":\n" + blowOffInspection.Remarks?.ToString(),
                target.NotificationLongText);
            Assert.AreEqual(blowOffInspection.InspectedBy?.UserName.ToUpper(), target.TechInspectionBy);
            Assert.AreEqual(
                blowOffInspection.DateInspected != null ? blowOffInspection.DateInspected.ToString("yyyyMMdd") : null,
                target.TechInspectionOn);
            Assert.AreEqual(
                blowOffInspection.DateInspected != null ? blowOffInspection.DateInspected.ToString("yyyyMMdd") : null,
                target.StartDate);
            Assert.AreEqual(
                blowOffInspection.DateInspected != null ? blowOffInspection.DateInspected.ToString("HHmmss") : null,
                target.StartTimeOfActivity);
            Assert.AreEqual(
                blowOffInspection.DateInspected != null
                    ? blowOffInspection.DateInspected.Date.ToString("yyyyMMdd")
                    : null, target.EndDate);
            Assert.AreEqual(
                blowOffInspection.DateInspected != null ? blowOffInspection.DateInspected.ToString("HHmmss") : null,
                target.EndTimeOfActivity);
            Assert.AreEqual(blowOffInspection.Valve?.OpenDirection.Description.ToUpper(), target.OpenDirection);
            Assert.AreEqual(blowOffInspection.ResidualChlorine?.ToString(), target.ResidualChlorine?.ToString());
            Assert.AreEqual(blowOffInspection.Valve?.Turns?.ToString(), target.noTurnsOperated?.ToString());
            Assert.AreEqual(blowOffInspection.TotalChlorine?.ToString(), target.TotalChlorine?.ToString());
            Assert.AreEqual(blowOffInspection.StaticPressure?.ToString(), target.StaticPressure?.ToString());
            Assert.AreEqual(blowOffInspection.GPM?.ToString(), target.Flow1GPM?.ToString());
            Assert.AreEqual(blowOffInspection.MinutesFlowed?.ToString(), target.Flow1Minutes?.ToString());
        }

        [TestMethod()]
        public void TestConstructorDoesNotFailWithNullsForOpening()
        {
            sewerMainCleaning = new SewerMainCleaning();

            var target = new SAPInspection(sewerMainCleaning);

            string opening1Number,
                   opening1Condition,
                   opening1FrameAndCover,
                   opening2Number,
                   opening2Condition,
                   opening2FrameAndCover,
                   hydrantUsed,
                   notes,
                   notificationLongText;

            opening1Number = sewerMainCleaning.Opening1 != null
                ? "Opening 1: " + sewerMainCleaning.Opening1?.OpeningNumber.ToUpper()
                : string.Empty;
            opening1Condition = sewerMainCleaning.Opening1Condition != null
                ? "Opening 1 Condition:" + sewerMainCleaning.Opening1Condition?.Description.ToUpper()
                : string.Empty;
            opening1FrameAndCover = sewerMainCleaning.Opening1FrameAndCover != null
                ? "Opening 1 Frame And Cover:" + sewerMainCleaning.Opening1FrameAndCover?.Description.ToUpper()
                : string.Empty;
            opening2Number = sewerMainCleaning.Opening2 != null
                ? "Opening 2: " + sewerMainCleaning.Opening2?.OpeningNumber.ToUpper()
                : string.Empty;
            opening2Condition = sewerMainCleaning.Opening2Condition != null
                ? "Opening 2 Condition: " + sewerMainCleaning.Opening2Condition?.Description.ToUpper()
                : string.Empty;
            opening2FrameAndCover = sewerMainCleaning.Opening2FrameAndCover != null
                ? "Opening 2 Frame And Cover: " + sewerMainCleaning.Opening2FrameAndCover?.Description.ToUpper()
                : string.Empty;
            hydrantUsed = sewerMainCleaning.HydrantUsed != null
                ? "Hydrant Used: " + sewerMainCleaning.HydrantUsed.HydrantNumber.ToUpper()
                : string.Empty;
            notes = sewerMainCleaning.TableNotes != null
                ? "Notes: " + sewerMainCleaning.TableNotes.ToUpper()
                : string.Empty;

            notificationLongText = "CREATED BY : " + sewerMainCleaning.CreatedBy?.FullName?.ToUpper() + "\n" +
                                   opening1Number + "\n" + opening1Condition + "\n" + opening1FrameAndCover + "\n" +
                                   opening2Number + "\n" + opening2Condition + "\n" + opening2FrameAndCover + "\n" +
                                   hydrantUsed + "\n" + notes;

            Assert.AreEqual("OPENING", target.AssetType);
            Assert.AreEqual(sewerMainCleaning.Opening1?.FunctionalLocation.Description, target.FunctionalLocation);
            Assert.AreEqual(sewerMainCleaning.Opening1?.SAPEquipmentNumber, target.EquipmentNo);
            Assert.AreEqual(sewerMainCleaning.InspectedDate?.Date.ToString("yyyyMMdd"), target.DateOfNotification);
            Assert.AreEqual(sewerMainCleaning.InspectedDate?.ToString("HHmmss"), target.NotificationTime);
            Assert.AreEqual(sewerMainCleaning.CreatedBy?.UserName.ToUpper(), target.ReportedBy);
            Assert.AreEqual(sewerMainCleaning.CreatedBy?.UserName.ToUpper(), target.TechInspectionBy);
            Assert.AreEqual(sewerMainCleaning.InspectedDate?.ToString("yyyyMMdd"), target.TechInspectionOn);
            Assert.AreEqual(sewerMainCleaning.InspectedDate?.ToString("yyyyMMdd"), target.StartDate);
            Assert.AreEqual(sewerMainCleaning.InspectedDate?.ToString("HHmmss"), target.StartTimeOfActivity);
            Assert.AreEqual(sewerMainCleaning.InspectedDate?.Date.ToString("yyyyMMdd"), target.EndDate);
            Assert.AreEqual(sewerMainCleaning.InspectedDate?.ToString("HHmmss"), target.EndTimeOfActivity);
            Assert.AreEqual(sewerMainCleaning.GallonsOfWaterUsed?.ToString(), target.FlushWaterVolume?.ToString());
            Assert.AreEqual(sewerMainCleaning.FootageOfMainInspected?.ToString(),
                target.FootageOfMainCleaned?.ToString());
            Assert.AreEqual(notificationLongText, target.NotificationLongText);
        }

        [TestMethod]
        public void TestInspectionRecordCreateWithNullsDoesNot()
        {
            SAPInspection SAPInspection = new SAPInspection();

            var target = SAPInspection.InspectionRecordCreate();

            Assert.AreEqual(target[0].ServiceObjectURL, SAPInspection.ServiceObjectURL);
            Assert.AreEqual(target[0].AssetType, SAPInspection.AssetType);
            Assert.AreEqual(target[0].NotificationType, SAPInspection.NotificationType);
            //NotificationNo, SAPInspection.NotificationNo);
            Assert.AreEqual(target[0].ShortText, SAPInspection.ShortText);
            Assert.AreEqual(target[0].FunctionalLocation, SAPInspection.FunctionalLocation);
            Assert.AreEqual(target[0].EquipmentNo, SAPInspection.EquipmentNo);
            Assert.AreEqual(target[0].CodeGroup, SAPInspection.CodeGroup);
            Assert.AreEqual(target[0].Coding, SAPInspection.Coding);
            Assert.AreEqual(target[0].PlanningPlant, SAPInspection.PlanningPlant);
            Assert.AreEqual(target[0].PlannerGroup, SAPInspection.PlannerGroup);
            Assert.AreEqual(target[0].DateOfNotification, SAPInspection.DateOfNotification);
            Assert.AreEqual(target[0].NotificationTime, SAPInspection.NotificationTime);
            Assert.AreEqual(target[0].ReportedBy, SAPInspection.ReportedBy);
            Assert.AreEqual(target[0].NotificationLongText, SAPInspection.NotificationLongText?.ToString());
            Assert.AreEqual(target[0].Priority, SAPInspection.Priority);
            Assert.AreEqual(target[0].TechInspectionBy, SAPInspection.TechInspectionBy);
            Assert.AreEqual(target[0].TechInspectionOn, SAPInspection.TechInspectionOn);
            Assert.AreEqual(target[0].InspectionType, SAPInspection.InspectionType);
            Assert.AreEqual(target[0].Operated, SAPInspection.Operated);
            Assert.AreEqual(target[0].PositionFound, SAPInspection.PositionFound);
            Assert.AreEqual(target[0].PositionLeft, SAPInspection.PositionLeft);
            Assert.AreEqual(target[0].OpenDirection, SAPInspection.OpenDirection);
            Assert.AreEqual(target[0].CodeType, SAPInspection.CodeType);
            Assert.AreEqual(target[0].RootCutter, SAPInspection.RootCutter);
            Assert.AreEqual(target[0].ActivityText, SAPInspection.ActivityText);
            Assert.AreEqual(target[0].StartDate, SAPInspection.StartDate);
            Assert.AreEqual(target[0].StartTimeOfActivity, SAPInspection.StartTimeOfActivity);
            Assert.AreEqual(target[0].EndDate, SAPInspection.EndDate);
            Assert.AreEqual(target[0].EndTimeOfActivity, SAPInspection.EndTimeOfActivity);
            Assert.AreEqual(target[0].StaticPressure,
                SAPInspection.StaticPressure != "0" ? SAPInspection.StaticPressure : null);
            Assert.AreEqual(target[0].Flow1GPM, SAPInspection.Flow1GPM != "0" ? SAPInspection.Flow1GPM : null);
            Assert.AreEqual(target[0].Flow1Minutes,
                SAPInspection.Flow1Minutes != "0" ? SAPInspection.Flow1Minutes : null);
            Assert.AreEqual(target[0].ResidualChlorine,
                SAPInspection.ResidualChlorine != "0" ? SAPInspection.ResidualChlorine : null);
            Assert.AreEqual(target[0].TotalChlorine,
                SAPInspection.TotalChlorine != "0" ? SAPInspection.TotalChlorine : null);
            Assert.AreEqual(target[0].noTurnsOperated,
                SAPInspection.noTurnsOperated != "0" ? SAPInspection.noTurnsOperated : null);
            Assert.AreEqual(target[0].FlushWaterVolume,
                SAPInspection.FlushWaterVolume != "0" ? SAPInspection.FlushWaterVolume : null);
            Assert.AreEqual(target[0].FootageOfMainCleaned,
                SAPInspection.FootageOfMainCleaned != "0" ? SAPInspection.FootageOfMainCleaned : null);
        }
    }
}
