create table Customers(
	Id int not null identity(1,1) primary key,
	Name varchar(120) not null,
	Phone varchar(120) not null,
	Email varchar(120) not null,
	Creation datetime not null default(getdate())
);
GO

create table CustomersBanned(
	Id int not null identity(1, 1) primary key,
	CustomerId int,
	Creation datetime not null default(getdate())
);
GO

select * from Customers;
select * from CustomersBanned;