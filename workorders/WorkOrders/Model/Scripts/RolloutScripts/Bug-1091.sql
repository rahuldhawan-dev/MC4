
begin tran

update WorkOrders set PurposeID = 11 where PurposeID = 12

delete from WorkOrderPurposes where WorkOrderPurposeID = 12

SET IDENTITY_INSERT WorkOrderPurposes ON
insert into WorkOrderPurposes ([WorkOrderPurposeID], [Description]) values (13, 'Asset Record Control') 
SET IDENTITY_INSERT WorkOrderPurposes OFF

rollback tran