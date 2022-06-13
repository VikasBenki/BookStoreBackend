
use BookStore;


------ Creating Table for Wishlist ------------
create table WishList
(
WishListId int identity(1,1) primary key,
UserId int not null foreign key(UserId) references Users(UserId),
BookId int not null Foreign key(BookId) references Books(BookId)
)
---- Creating procedure for Add to WishList -------

alter proc SP_AddTo_WishList
(
@BookId int,
@UserId int
)
As
Begin 
		insert into WishList
		Values (@UserId,@BookId);
end

---------------Creating procedure for the Deletting from wishList ---------------------------------

create proc SP_Remove_FromWishList
(
@WishListId int
)
AS
Begin
		delete from WishList where WishListId = @WishListId;
end

select * from Users;
select * from WishList
select * from Books

------- Creating procedure for the  Get all from WishList ------------------

create proc SP_GetAll_FromWishList
(
@UserId int
) 
As
Begin
	select
		w.WishListId,
		w.BookId,
		w.UserId,
		b.Book_Name,
		b.Book_Image,
		b.Author_Name,
		b.Discount_Price,
		b.Actual_Price
		from WishList w
		inner join Books b
		on w.BookId = b.BookId
		where w.UserId =@UserId;
end

truncate table wishlist