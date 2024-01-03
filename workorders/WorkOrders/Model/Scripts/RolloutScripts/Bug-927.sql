use McProd
go;

alter table [WorkOrders]
drop column [CustomerAlert];

alter table [WorkOrders]
add [AlertID] varchar(20) null;
