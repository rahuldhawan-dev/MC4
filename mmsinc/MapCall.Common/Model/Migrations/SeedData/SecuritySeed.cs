using System;

namespace MapCall.Common.Model.Migrations.SeedData
{
    /// <summary>
    /// Users, Roles, Operating Centers
    /// </summary>
    public class SecuritySeed : SeedBase
    {
        #region Private Methods

        private void InitApplications()
        {
            Action<int, string> add = ((applicationId, name) => {
                EnableIdentityInsert("Applications");
                SeedData.Insert.IntoTable("Applications").Row(new {
                    ApplicationID = applicationId,
                    Name = name
                });
                DisableIdentityInsert("Applications");
            });

            add(1, "Field Services");
            add(2, "Production");
            add(3, "Human Resources");
            add(4, "Operations");
            add(5, "Water Non Potable");
            add(6, "BPU");
            add(7, "Business Performance");
            add(8, "Management");
            add(9, "Customer");
            add(10, "Fleet Management");
            add(11, "Events");
            add(13, "Contractors");
            add(14, "Water Quality");
            add(16, "H2O");
            add(18, "Environmental");
        }

        private void InitModules()
        {
            Action<int, string> add = ((applicationId, name) => {
                SeedData.Insert.IntoTable("Modules").Row(new {
                    ApplicationID = applicationId,
                    Name = name
                });
            });

            // Field Services
            add(1, "Data Lookups");
            add(1, "Reports");
            add(1, "Services");
            add(1, "Hydrants");
            add(1, "Hydrant Inspections");
            add(1, "Valves");
            add(1, "Valve Inpsections");
            add(1, "Meter Change Outs");
            add(1, "Work Management");
            add(1, "Meters");
            add(1, "Images");
            add(1, "Projects");

            // Production
            add(2, "Production");

            // Human Resources
            add(3, "Employee");
            add(3, "Positions");
            add(3, "Position History");
            add(3, "Position Posting");
            add(3, "Union");
            add(3, "Contracts");
            add(3, "Sections");
            add(3, "Proposals");
            add(3, "Grievances");
            add(3, "Assets");
            add(3, "Environmental");
            add(3, "Expense Lines");
            add(3, "Facilities");
            add(3, "Sample Sites");
            add(3, "Lookups");
            add(3, "Admin");
            add(3, "Staffing Hours");

            // Operations
            add(4, "Health And Safety");
            add(4, "Training");
            add(4, "Management");
            add(4, "DistributionOnly");

            // Water Non Potable
            add(5, "Sewer");
            add(5, "Storm Water");

            // BPU
            add(6, "General");

            // Business Performance
            add(7, "General");

            // Management
            add(8, "General");

            // Customer
            add(9, "General");
            add(9, "Premises");

            // Fleet Management
            add(10, "General");

            // Events
            add(11, "Events");

            // Contractors
            add(13, "General");
            add(13, "Agreements");

            // Water Quality
            add(14, "General");

            // H2O
            add(16, "General");

            // Environmental
            add(18, "General");
        }

        private void OperatingCenters() { }

        #endregion

        #region Constructors

        public SecuritySeed(SeedData seedData) : base(seedData) { }

        #endregion

        #region Exposed Methods

        public override void Up()
        {
            //InitApplications();
            //InitModules();
        }

        #endregion
    }
}
