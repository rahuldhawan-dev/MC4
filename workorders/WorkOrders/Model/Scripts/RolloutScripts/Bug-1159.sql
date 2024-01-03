begin tran

alter table [WorkOrders] add  [UpdatedMobileGIS] bit NULL

rollback tran