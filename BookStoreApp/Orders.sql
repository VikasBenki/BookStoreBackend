use BookStore;

Create table Orders
(
OrderId int identity (1,1) primary key,
Order_Date Date Not null,
Books_Qty int not null,
Order_Price float not null,
Actual_Price float Not null,
BookId int not null foreign key (BookId) references Books (BookId),
UserId int not null foreign key (UserId) references Users(UserId),
AddressId int not null foreign key (AddressId) references Adress (AddressId)
)

select * from Orders As Orders
Select * From Books;
Select * from Adress;
select * from cart
-------------- creating procedure for the Add Orders- ----------------

Create proc SP_Add_Orders
(
@BookId int,
@UserId int,
@AddressId int
)
As
declare @OrderPrice float;
declare @ActualPrice float;
declare @BookQuantity int;
Begin TRY
		if(Exists(select * from Adress where AddressId = @AddressId))
		Begin
			Begin Transaction
					select @BookQuantity = Book_Quantity From Cart where BookId =@BookId and UserId =@UserId;
					set @OrderPrice = (select Discount_Price from Books where BookId = @BookId);
					set @ActualPrice = (select Actual_Price From Books where BookId = @BookId);

					If((Select Quantity From Books where BookId =@BookId)>=@BookQuantity)
					Begin
							insert into Orders
							Values(GETDATE(), @BookQuantity, @OrderPrice * @BookQuantity, @ActualPrice * @BookQuantity, @BookId, @UserId,@AddressId);

							update Books Set Quantity = Quantity - @BookQuantity where BookId = @BookId;

							delete From Cart where BookId = @BookId and UserId =@UserId;
					End
					else
					Begin
							select 2;
					end
				Commit Transaction
			end
		ELSE
			begin
			select 3
			end
END TRY
BEGIN CATCH
ROLLBACK TRANSACTION
END CATCH


--------- creating procedure for the get all orders ---------

Create proc SP_GetAll_Orders
(
@UserId int
)
As
Begin
	select 
		b.Book_Image,
		b.Author_Name,
		o.Order_Price,
		o.Order_Date,
		o.Books_Qty,
		o.OrderId,
		o.UserId,
		o.AddressId
	From Orders o
	Inner Join Books b
	On O.BookId = b.BookId
	where o.UserId =@UserId;
End
