/****** Object:  Table [dbo].[OrcomOrderCompletions]    Script Date: 01/13/2010 08:44:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrcomOrderCompletions]
(
	[OrcomOrderCompletionID] [int] IDENTITY(1,1) NOT NULL,
	[WorkOrderID] [int] NOT NULL,
	[CompletedByID] [int] NOT NULL,
	[CompletedOn] [datetime] NOT NULL,
	CONSTRAINT [PK_OrcomOrderCompletions] PRIMARY KEY CLUSTERED 
	(
		[OrcomOrderCompletionID] ASC
	) 
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[OrcomOrderCompletions]  WITH CHECK ADD  CONSTRAINT [FK_OrcomOrderCompletions_tblPermissions] FOREIGN KEY([CompletedByID])
REFERENCES [dbo].[tblPermissions] ([RecID])
GO
ALTER TABLE [dbo].[OrcomOrderCompletions] CHECK CONSTRAINT [FK_OrcomOrderCompletions_tblPermissions]
GO
ALTER TABLE [dbo].[OrcomOrderCompletions]  WITH CHECK ADD  CONSTRAINT [FK_OrcomOrderCompletions_WorkOrders] FOREIGN KEY([WorkOrderID])
REFERENCES [dbo].[WorkOrders] ([WorkOrderID])
GO
ALTER TABLE [dbo].[OrcomOrderCompletions] CHECK CONSTRAINT [FK_OrcomOrderCompletions_WorkOrders]
GO
GRANT ALL ON OrcomOrderCompletions TO MCUSER
GO
--Insert any order completed before 6/19/09.  402 is mcAdmin
insert into orcomordercompletions ( workorderid, completedbyid, completedon )
select workorderid, 402, GetDate() from workorders 
where dateCompleted < '2009-06-19'
GO