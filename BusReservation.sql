create database busreservation

use busreservation

create table Customer(
CustomerId int identity(10,1) not null,
EmailId varchar(25) primary key not null,
MobileNumber varchar(10) not null,
DateOfBirth date not null,
Password varchar(20) not null
)

drop table Customer
drop table Booking

create table Booking(
TicketId int identity(100,1) primary key not null,
CustomerId int foreign key references Customer(CustomerId) on delete cascade not null,
EmailId varchar(25),
MobileNumber varchar(10) not null,
BusName varchar(20) not null,
Start varchar(15) not null,
Destination varchar(15) not null,
DateOfJourney date not null,
Seats int not null,
Bookingstatus varchar(20) default 'On Progress',
Travelstatus varchar(20) default 'No'
)

drop table Booking

create table Admin
(AdminId int primary key not null,
Password varchar(15) not null)	

create table Buses
(BusId int identity(1000,1) primary key not null,
 BusName varchar(20) not null,
 Start varchar(20)not null,
 Destination varchar(20)not null,
 DateOfJourney date not null,
 Availability int default 75)


drop table Admin
drop table Buses

insert into Admin values(20,'Admin@20')
insert into Admin values(30,'Admin@30')
insert into Buses(BusName,Start, Destination, DateOfJourney) values('Bedrock','Chennai','Bangalore','2021-02-25')
insert into Buses(BusName,Start, Destination, DateOfJourney) values('Livin','Mysore','Hyderabad','2021-03-09')
insert into Buses(BusName,Start, Destination, DateOfJourney) values('Hotgear','Coimbatore','Hosur','2021-05-30')
insert into Buses (BusName,Start, Destination, DateOfJourney)values('Climax','Pune','Mumbai','2021-10-22')
insert into Buses (BusName,Start, Destination, DateOfJourney)values('RaajMalai','Chennai','Thanjavur','2021-03-21')
insert into Buses (BusName,Start, Destination, DateOfJourney)values('Yoguert','Bangalore','Hosur','2021-05-19')

insert into Customer values('raju@gmail.com','8596327426','1997-05-26','Raju@123')
insert into Customer values('ganesh@gmail.com','8596586126','1998-07-21','Ganesh@123')
insert into Customer values('juhan@gmail.com','7894561230','1986-10-14','juhan@234')
insert into Booking values(11,'ganesh@gmail.com','8596586126','Hotgear','Coimbatore','Hosur','2021-05-30',1)
insert into Booking values(12,'juhan@gmail.com','7894561230','Climax','Pune','Mumbai','2021-10-22',1)
insert into Booking values(10,'raju@gmail.com','8596327426','Bedrock','Chennai','Bangalore','2021-02-25',2)
insert into Booking values(10,'raju@gmail.com','8596327426','Climax','Pune','Mumbai','2021-10-22',1)
insert into Booking values(11,'ganesh@gmail.com','8521427413','Hotgear','Coimbatore','Hosur','2021-05-30',1)
insert into Booking values(11,'ganesh@gmail.com','8521427413','Livin','Mysore','Hyderabad','2021-03-09',3)
insert into Booking values(10,'raju@gmail.com','8596327426','Livin','Mysore','Hyderabad','2021-03-09',2)
insert into Booking(CustomerId,EmailId,MobileNumber,BusName,Start,Destination,DateOfJourney,Seats) values(29,'veeresh@gmail.com','8056977246','Bedrock','Chennai','Bangalore','2021-02-25',2)
insert into Booking(CustomerId,EmailId,MobileNumber,BusName,Start,Destination,DateOfJourney,Seats) values(31,'pradun@gmail.com','7894561238','Bedrock','Chennai','Bangalore','2021-02-25',2)
insert into Booking(CustomerId,EmailId,MobileNumber,BusName,Start,Destination,DateOfJourney,Seats) values(32,'rakesh@gmail.com','9647321851','Climax','Pune','Mumbai','2021-10-22',1)
insert into Booking(CustomerId,EmailId,MobileNumber,BusName,Start,Destination,DateOfJourney,Seats) values(36,'gouvtham@gmail.com','6380993131','Livin','Mysore','Hyderabad','2021-03-09',3)
insert into Booking(CustomerId,EmailId,MobileNumber,BusName,Start,Destination,DateOfJourney,Seats) values(29,'veeresh@gmail.com','8056977246','Bedrock','Chennai','Bangalore','2021-02-25',1)


truncate table Booking

go
create procedure GetBookingbyCid @Cid int
As
Begin
Select * from Booking where CustomerId=@Cid
end
 
exec GetBookingbyCid 32

go
create procedure GetBookbyCid @Cid int
As
Begin
Select * from Booking where CustomerId=@Cid
end

exec GetBookbyCid 32


drop proc GetBookingbyCid


go
create procedure GetBus @Start varchar(20),@Destination varchar(20),@DateOfJourney date
As
Begin
Select * from Buses where Start=@Start and Destination=@Destination and DateOfJourney=@DateOfJourney and Availability>0
end

exec GetBus 'Chennai','Bangalore','2021-02-25' 

go
create procedure CancelBooking @TicketId int,@CustomerId int
As
Begin
Select * from Booking where TicketId=@TicketId and CustomerId=@CustomerId
end

drop proc CancelTicket

exec CancelBooking 119,10
 
delete from Booking where CustomerId=10 and TicketId=120 

select * from Booking
select * from Buses
select * from Customer 

select * from Admin

delete from Booking where TicketId=130

go
create trigger availability_inc
on [dbo].[Booking]
for insert 
As
Begin
update b set Availability=Availability-i.Seats
from dbo.Buses b inner join inserted i
on b.BusName=i.BusName
end

drop trigger availability_inc
drop trigger availability_change

go
create trigger availability_change
on [dbo].[Booking]
for delete 
As
Begin
update b set Availability=Availability+d.Seats
from dbo.Buses b inner join deleted d
on b.BusName=d.BusName
end

go
create trigger busdelete
on [dbo].[Buses]
for delete
As
Begin
update b set Bookingstatus='Unavailable'
from dbo.Booking b inner join deleted d
on b.BusName=d.BusName
end

go
create trigger bookstat
on [dbo].[Booking]
for insert
As
Begin
update b set Bookingstatus='Booked',Travelstatus='No'
from dbo.Booking b inner join inserted i
on b.TicketId=i.TicketId
end

drop trigger bookstat

delete from Buses where BusName='Hotgear'
