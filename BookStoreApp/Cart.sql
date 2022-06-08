use BookStore;

------------------ Creating a table for the Cart------------
Create Table Cart
(
CartId int identity(1,1) primary key,
Book_Quantity int default 1,
UserId int not null foreign key (UserId) references Users(UserId),
BookId int not null Foreign key (BookId) references Books(BookId)
)

select  *  From Cart
----------------- Creating a procedure for the Add to cart -----------


Create proc SP_Add_To_Cart
( @BookQuantity int,
@UserId int,
@BookId int
)
As
Begin
	insert into cart
	values ( @BookQuantity,@UserId, @BookId);
End

-------------------------Creating procedure for the Remove From Cart -----------------

Create proc SP_RemoveFrom_Cart
(
@CartId int
)
As
Begin
	delete from Cart where CartId = @CartId;
end

------------ Creating procedure for  the get all from cart -----------

create proc SP_GetAll_Cart
(
@UserId int
)
AS
Begin
	select
		c.CartId,
		c.BookId,
		c.UserId,
		c.Book_Quantity,
		b.Book_Name,
		b.Book_Image,
		b.Author_Name,
		b.Discount_Price,
		b.Actual_Price
		from Cart c
		inner join Books b
		on c.BookId = b.BookId
		where c.UserId = @UserId;
end

Exec SP_GetAll_Cart 1;

create proc SP_UpdateQty_InCart
(
@BookQuantity int,
@cartId int
)
As
Begin
	update Cart Set Book_Quantity = @BookQuantity where CartId = @CartId;
end