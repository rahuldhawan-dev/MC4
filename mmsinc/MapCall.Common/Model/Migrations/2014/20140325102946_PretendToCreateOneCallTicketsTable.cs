using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20140325102946), Tags("Production")]
    public class PretendToCreateOneCallTicketsTable : Migration
    {
        public override void Up()
        {
            // This table already exists but isn't part of any of our dataimport
            // scripts and was created after the mcprod freeze. :shakes fist:

            Execute.Sql(@"
IF NOT EXISTS (SELECT 1 
           FROM INFORMATION_SCHEMA.TABLES 
           WHERE TABLE_TYPE='BASE TABLE' 
           AND TABLE_NAME='OneCallTickets') 
   CREATE TABLE [dbo].[OneCallTickets](
	[RequestNum] [nvarchar](18) NULL,
	[RequestNumOld] [nvarchar](18) NULL,
	[TicketFileName] [nvarchar](30) NULL,
	[CDC] [nvarchar](15) NULL,
	[SequenceNum] [smallint] NULL,
	[TransmitDate] [datetime] NULL,
	[TransmitTime] [nvarchar](8) NULL,
	[ReceiptDate] [datetime] NULL,
	[ReceiptTime] [nvarchar](8) NULL,
	[MessageType] [nvarchar](15) NULL,
	[MessageCode] [smallint] NULL,
	[County] [nvarchar](35) NULL,
	[Town] [nvarchar](35) NULL,
	[Subdivision] [nvarchar](55) NULL,
	[Street] [nvarchar](45) NULL,
	[NearIntersect] [nvarchar](40) NULL,
	[OtherIntersect] [nvarchar](40) NULL,
	[TypeOfWork] [nvarchar](60) NULL,
	[ExtentOfWork] [nvarchar](500) NULL,
	[MethodOfWork] [nvarchar](60) NULL,
	[Depth] [nvarchar](20) NULL,
	[StartDate] [datetime] NULL,
	[StartTime] [nvarchar](8) NULL,
	[Remarks] [nvarchar](500) NULL,
	[WorkFor] [nvarchar](55) NULL,
	[WorkForPh] [nvarchar](18) NULL,
	[WorkForContact] [nvarchar](35) NULL,
	[Caller] [nvarchar](35) NULL,
	[CallerPh] [nvarchar](18) NULL,
	[Excavator] [nvarchar](50) NULL,
	[ExcavatorPh] [nvarchar](18) NULL,
	[ExcavatorFax] [nvarchar](12) NULL,
	[ExcavatorContact] [nvarchar](35) NULL,
	[ExcavatorContactPh] [nvarchar](18) NULL,
	[ExcavatorCellPh] [nvarchar](12) NULL,
	[ExcavatorEmail] [nvarchar](50) NULL,
	[Grids] [nvarchar](500) NULL,
	[Comment] [nvarchar](500) NULL,
	[Flag1] [nvarchar](40) NULL,
	[CompleteBy] [nvarchar](35) NULL,
	[StatusOfMarkout] [nvarchar](35) NULL,
	[ReqNotified] [nvarchar](5) NULL,
	[Paint] [nvarchar](5) NULL,
	[Flag] [nvarchar](5) NULL,
	[Stake] [nvarchar](5) NULL,
	[Over500FT] [nvarchar](5) NULL,
	[Technique] [nvarchar](35) NULL,
	[CrewMarkout] [nvarchar](5) NULL,
	[StreetName] [nvarchar](50) NULL,
	[Area] [nvarchar](25) NULL,
	[RecvDateStr] [nvarchar](10) NULL,
	[CompleteDateStr] [nvarchar](10) NULL,
	[CompleteTimeStr] [nvarchar](5) NULL,
	[DueDate] [varchar](11) NULL,
	[StartDateStr] [nvarchar](10) NULL,
	[Viewed] [nvarchar](5) NULL,
	[State] [nvarchar](5) NULL,
	[ExcavatorAddress] [nvarchar](120) NULL,
	[TicketText] [varchar](4500) NULL,
	[CSMONum] [smallint] NULL,
	[CSMOTotalTime] [nvarchar](6) NULL,
	[CSMOUnableNum] [smallint] NULL,
	[Id] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_OneCallTickets] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]");
        }

        public override void Down()
        {
            // There is no down here. This table has existed for 6 months
            // and has a ton of data in it. I blame Alex.
        }
    }
}
