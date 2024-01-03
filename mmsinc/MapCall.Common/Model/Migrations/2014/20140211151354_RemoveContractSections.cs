using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20140211151354), Tags("Production")]
    public class RemoveContractSections : Migration
    {
        public override void Up()
        {
            Delete.Table("tblContract_SECTIONS");
        }

        public override void Down()
        {
            Execute.Sql(@"
                CREATE TABLE [dbo].[tblContract_SECTIONS](
	            [Contract_ID] [float] NULL,
	            [Contract_Section_Category] [nvarchar](255) NULL,
	            [ContractSectionID] [int] IDENTITY(1,1) NOT NULL,
	            [CurrentPageNumber] [int] NULL,
	            [ContractSection_Number] [float] NULL,
	            [ContractSection] [nvarchar](255) NULL,
	            [ContractSub_SectionName] [nvarchar](255) NULL,
	            [ContractSub_Section_Designation] [nvarchar](255) NULL,
	            [Current_Language] [ntext] NULL,
	            [Importance_Rating] [nvarchar](50) NULL,
	            [Impact_on_Absenteeism] [bit] NULL,
	            [Impact_on_Health_Safety] [bit] NULL,
	            [Impact_on_Managements_Rights] [bit] NULL,
	            [Impact_on_Operational_Efficiency] [bit] NULL,
	            [Impact_on_Overtime] [bit] NULL,
	            [Notes] [ntext] NULL,
	            CONSTRAINT [PK_tblContract_SECTIONS] PRIMARY KEY CLUSTERED ([ContractSectionID] ASC)
            ) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
            ALTER TABLE [dbo].[tblContract_SECTIONS] ADD  CONSTRAINT [DF_tblContract_SECTIONS_Impact_on_Absenteeism]  DEFAULT (0) FOR [Impact_on_Absenteeism]
            ALTER TABLE [dbo].[tblContract_SECTIONS] ADD  CONSTRAINT [DF_tblContract_SECTIONS_Impact_on_Health_Safety]  DEFAULT (0) FOR [Impact_on_Health_Safety]
            ALTER TABLE [dbo].[tblContract_SECTIONS] ADD  CONSTRAINT [DF_tblContract_SECTIONS_Impact_on_Managements_Rights]  DEFAULT (0) FOR [Impact_on_Managements_Rights]
            ALTER TABLE [dbo].[tblContract_SECTIONS] ADD  CONSTRAINT [DF_tblContract_SECTIONS_Impact_on_Operational_Efficiency]  DEFAULT (0) FOR [Impact_on_Operational_Efficiency]
            ALTER TABLE [dbo].[tblContract_SECTIONS] ADD  CONSTRAINT [DF_tblContract_SECTIONS_Impact_on_Overtime]  DEFAULT (0) FOR [Impact_on_Overtime]
            ");
        }
    }
}
