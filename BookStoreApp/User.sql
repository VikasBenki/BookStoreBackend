-----------------Creating database for BookStore-------------------

Create database BookStore;

use BookStore;

------------Creating tables for Users------------------

Create table Users
( UserId int identity(1,1) Primary key,
Full_Name Varchar(225) not null,
Email_Id Varchar(225) not null unique,
Password varchar(225) not null,
Mobile_Number bigint not null
)

select * from Users

insert Users values ('vikas', 'vikasms@gmail.com', 'vikas143#', 8553335583)
-------Stored Procedure for Users ----------

Create procedure SP_User_Registration
(
@FullName varchar(255),
@EmailId varchar(255),
@Password Varchar(255),
@MobileNumber Bigint
)
as
Begin
		insert Users
		values (@FullName, @EmailId, @Password, @MobileNumber) 
end
-------------------------
alter proc SP_User_Login
(
@EmailId varchar(225)
)
AS
Begin
	Select * from Users where Email_Id = @EmailId 
end


-------------------create procedure for ForgetPassword--------------

create proc SP_User_ForgotPassword
(
@EmailId varchar(225)
)
As
Begin
select * from Users where Email_Id = @EmailId
end

----------------------------------------------------------

Create proc SP_User_ResetPassword
(@EmailId varchar(225),
@Password varchar(225)
)
As
Begin
	Update Users
	set Password = @Password where Email_Id = @EmailId
End