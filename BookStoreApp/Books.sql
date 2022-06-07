use BookStore

Create table Books
(
BookId int identity(1,1) primary key,
Book_Name varchar(225) not null,
Author_Name Varchar(225) not null,
Book_Image varchar(225) not null,
Book_Detail varchar(225) not null,
Discount_Price float not null,
Actual_Price float not null,
Quantity int not null,
Rating float,
RatingCount int
)

----Creating procedure for the Adding Book----

Create Proc SP_Add_Book
(
@Book_Name varchar(225),
@Author_Name Varchar(225),
@Book_Image varchar(225),
@Book_Detail varchar(225),
@Discount_Price float,
@Actual_Price float,
@Quantity int,
@Rating float,
@RatingCount int,
@BookId int Output
)
As
Begin
	Insert into Books
	values (@Book_Name, @Author_Name, @Book_Image, @Book_Detail, @Discount_Price, @Actual_Price,
	@Quantity, @Rating, @RatingCount);
	set @BookId = SCOPE_IDENTITY()
	return @BookId;
End

select * from Books

------Creating procedure for Update Book ------------

Create Proc SP_Update_Book
(
@BookId int,
@Book_Name varchar(225),
@Author_Name Varchar(225),
@Book_Image varchar(225),
@Book_Detail varchar(225),
@Discount_Price float,
@Actual_Price float,
@Quantity int,
@Rating float,
@RatingCount int
)
As
Begin
 Update Books
 set Book_Name = @Book_Name,
 Author_Name=@Author_Name,
 Book_Image=@Book_Image,
 Book_Detail=@Book_Detail,
 Discount_Price = @Discount_Price,
 Actual_Price = @Actual_Price,
 Quantity = @Quantity,
 Rating= @Rating,
 RatingCount = @RatingCount where BookId = @BookId;
End

----------------------------------Creating procedure for Delete Book-----------
Create proc SP_Delete_Book
(
@BookId int
)
As
Begin
 delete from Books where BookId = @BookId ;
end

----------------------Creating procedure for the get all books -----------

create proc SP_GetAll_Books
AS
Begin
		Select * from Books;
End

---------------------- creating procedure fro the get book by Id ----

create proc SP_GetBook_ById
(
@BookId int
)
As
Begin
	select * from Books where BookId = @BookId;
End