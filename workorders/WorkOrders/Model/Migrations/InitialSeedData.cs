using System;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using FluentMigrator;

namespace WorkOrders.Model.Migrations
{
    [Migration(20130214111924), Tags("workorderstest")]
    public class InitialSeedData : Migration
    {
        public override void Up()
        {
            var scripts = new[] {
                @"WorkOrders\Model\Scripts\Common\04 ImportSiteData.sql",
                @"WorkOrders\Model\Scripts\Common\05 CreateStaticData.sql",
                @"WorkOrders\Model\Scripts\Common\06 CreateOperatingCenterData.sql",
                @"WorkOrders\Model\Scripts\Common\07 CreateSampleData.sql"
            };

            foreach (var script in scripts)
            {
                try
                {
                    Execute.Script(script);
                }
                catch (SqlException e)
                {
                    var errorRegex = new Regex("The error was(.+)(?:\r\n|\n)");
                    var theError = errorRegex.Match(e.Message).Groups[1].Value;
                    throw new Exception(String.Format("Error occurred executing script file '{0}'.{1}The error was:{2}", script, Environment.NewLine, theError), e);
                }
            }
        }

        public override void Down()
        {

        }
    }
}