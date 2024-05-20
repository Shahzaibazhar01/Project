create database WDMS
go
use WDMS
go
create table BUYER(
BID int not null identity(1,1) primary key,
BName varchar(50),
BAddress varchar(100), 
BCNIC bigint,
BPhoneNumber bigint
)
go
create table ORDERS(
OID int not null identity(1,1) primary key,
OTotal int,
ODate datetime,
BuyerID int foreign key references Buyer (BID)
)
go
create table Supplier(
SID int not null identity(1,1) primary key,
SName varchar(50),
SAddress varchar(100),
SCNIC bigint,
SPhoneNumber bigint
)
go
create table Product(
PID int not null identity(1,1) primary key,
PPrice int,
DeliveryCharges int,
SupplierID int foreign key references Supplier(SID),
)
go
create table OrderDetail(
ODID int not null identity(1,1) primary key,
OPrice int,
OrderID int foreign key references Orders (OID),
ProductID int foreign key references Product(PID)
)
go
create table PaymentMode(
PMID int not null identity(1,1) primary key,
PMName varchar(50)
)
go
create table Payment(
PYID int not null identity(1,1) primary key,
InstallTime int,
OrderID int foreign key references Orders(OID),
PaymentModeID int foreign key references PaymentMode(PMID),
PDateTime datetime
)
go
create table Installment(
IID int not null identity(1,1) primary key,
Ipay int,
PaymentID int foreign key references Payment(PYID)
)
go
create proc sp_Install @time int, @PaymentID int, @pay int
as
WHILE ( @time > 0)
Begin
	insert into Installment values(@pay,@PaymentID)

insert into PaymentMode values('Card'),('Cash')
select * from UserRole
DBCC CHECKIDENT ('[OrderDetail]', RESEED, 0)
DELETE FROM OrderDetail
DBCC CHECKIDENT ('[Orders]', RESEED, 0)
DELETE FROM Orders
DBCC CHECKIDENT ('[Payment]', RESEED, 0)
DELETE FROM Payment
DBCC CHECKIDENT ('[PaymentDetails]', RESEED, 0)
DELETE FROM Payment
DBCC CHECKIDENT ('[Product]', RESEED, 2)
DELETE FROM Product WHERE PID > 2

DBCC CHECKIDENT ('[Users]', RESEED, 0)
DELETE FROM Users