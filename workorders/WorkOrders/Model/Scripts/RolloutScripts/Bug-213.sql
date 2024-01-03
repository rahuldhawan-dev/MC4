USE [McProd]
GO

UPDATE [WorkOrders]
SET
	[PriorityID] = CASE [PriorityID]
		WHEN (SELECT WorkOrderPriorityID FROM WorkOrderPriorities WHERE Description = 'Revenue Related')
			THEN (SELECT WorkOrderPriorityID FROM WorkOrderPriorities WHERE Description = 'High Priority')
		ELSE [PriorityID]
	END,
	[PurposeID] = CASE [PurposeID]
		WHEN (SELECT WorkOrderPurposeID FROM WorkOrderPurposes WHERE Description = 'Revenue')
			THEN (SELECT WorkOrderPurposeID FROM WorkOrderPurposes WHERE Description = 'Revenue 150-500')
		ELSE [PurposeID]
	END
WHERE [PriorityID] = (SELECT WorkOrderPriorityID FROM WorkOrderPriorities WHERE Description = 'Revenue Related')
OR [PurposeID] = (SELECT WorkOrderPurposeID FROM WorkOrderPurposes WHERE Description = 'Revenue');

DELETE FROM WorkOrderPriorities WHERE Description = 'Revenue Related';
DELETE FROM WorkOrderPurposes WHERE Description = 'Revenue';
