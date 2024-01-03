
begin tran

SET IDENTITY_INSERT WorkOrderPurposes ON
insert into WorkOrderPurposes ([WorkOrderPurposeID], [Description]) values (14, 'Seasonal') 
insert into WorkOrderPurposes ([WorkOrderPurposeID], [Description]) values (15, 'Demolition') 
insert into WorkOrderPurposes ([WorkOrderPurposeID], [Description]) values (16, 'BPU') 
SET IDENTITY_INSERT WorkOrderPurposes OFF

rollback tran