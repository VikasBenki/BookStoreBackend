use BookStore;		

---- creating the table for the Adress ---------------

create table Adress
(
AddressId int identity(1,1) primary key,
Address Varchar(225) not null,
City varchar(225) not null,
State varchar(225) not null,
AdTypeId int not null foreign key (AdTypeId) references AddressType(AdTypeId),
UserId int not null foreign key (UserId) references Users(UserId)
)

--------------------- Creating table for AddressType ----------------------------

Create table AddressType
(
AdTypeId int identity(1,1) primary key,
AddressType Varchar(225)
)

insert into AddressType values
('Home'),
('Office'),
('Other');

select * from Adress
-------creating procedure for the Add Address -------

create proc SP_Add_Address
(
@Address varchar(225),
@City varchar(225),
@State varchar(225),
@AdTypeId Int,
@UserId Int
)
As
Begin
		insert into Adress
		values(@Address,@City,@State,@AdTypeId,@UserId);
end

------Creating procedure for the Delete-----------

create proc SP_Delete_Address
(
@AddressId int,
@UserId int
)
As
Begin
		delete from Adress where AddressId = @AddressId and UserId =@UserId;
end

------------------------creating proc for the Update Adress ----------------------

alter proc SP_Update_Address
(
@AddressId int,
@Address varchar,
@City varchar,
@State varchar,
@AdTypeId int,
@UserId int
)
as
Begin
	if Exists(select * from AddressTyPe where AdtypeId =@AdTypeId)
	Begin
		Update Adress set Address =@Address, City =@City, State =@state, AdTypeId =@AdTypeId
		where AddressId =@AddressId and UserId =@UserId;
	end
	else
	begin
		select 2
	end
end


select * from Adress
truncate table Adress

------------------------------- stored procedure for get all adress ------------------
create proc SP_Get_AllAddress
(
	@UserId int
)
as
BEGIN 
	select * from Adress where UserId = @UserId;
END 

