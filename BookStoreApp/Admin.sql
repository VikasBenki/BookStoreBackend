Use BookStore;

----Creating table for the Admin ------

Create table Admin
(
AdminId int identity(1,1) primary key,
FullName Varchar(225)not null,
EmailId Varchar(225) not null,
Password varchar(225) not null,
MobileNumber BigInt not null,
Address varchar(max) not null
)

truncate table Admin;
Insert into Admin 
values('Admin', 'admin123@bookstore.com', 'Admin@123',8660683485, ' #52, WelcomeHome, InDream, AchieveGoal in reality');

------- Creating Stored procedure for the Login for Admin -----
create proc SP_Admin_Login
(
@EmailId varchar(225),
@Password Varchar(225)
)
As
Begin
		select * From Admin Where EmailId = @EmailId and Password = @Password;
end