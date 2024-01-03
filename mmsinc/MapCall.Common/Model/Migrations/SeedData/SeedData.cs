using System;
using FluentMigrator;

namespace MapCall.Common.Model.Migrations.SeedData
{
    [Profile("Development")]
    public class SeedData : Migration
    {
        #region Private Members

        private readonly Type[] _registrations = new[] {
            typeof(SecuritySeed)
        };

        #endregion

        public override void Up()
        {
            foreach (var reg in _registrations)
                ((SeedBase)Activator.CreateInstance(reg, new object[] {this})).Up();
        }

        public override void Down() { }
    }
}
