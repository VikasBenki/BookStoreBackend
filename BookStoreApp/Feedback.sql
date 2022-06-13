Use BookStore;

Create table Feedback
(
FeedbackId int identity(1,1) primary key,
Rating float not null,
Comment varchar(max),
BookId int not null Foreign key (BookId) references Books(BookId),
UserId int not null foreign key (UserId) references Users(UserId)
)

--------------------------- Stored procedure for the Addfeedback --------------------
create proc SP_Add_Feedback
(
@Rating float,
@Comment varchar(max),
@BookId int,
@UserId int
)
As
declare @AvgRating float;
BEGIN 
		IF(EXISTS(SELECT * FROM Feedback where BookId = @BookId and UserId = @UserId))
		select 1;
		Else
		begin try
			BEGIN TRANSACTION
				insert into Feedback
				values (@Rating,@Comment,@BookId, @UserId);
				set @AvgRating =(select Avg(Rating) from Feedback where BookId =@BookId);
				Update Books set Rating = @AvgRating, RatingCount = (RatingCount+1) where BookId =@BookId;
			Commit Transaction
		End Try

		Begin catch
		 RollBack Transaction
		 End Catch
End


--------------------- Create procedure for the  Get all feedback ----------
Create proc SP_GetAll_Feedback
( 
@BookId int
)
As
Begin
	 Select 
	 f.FeedbackId,
	 f.UserId,
	 f.BookId,
	 f.Comment,
	 f.Rating,
	 u.Full_Name
	 From Users u
	 Inner join Feedback f
	 on f.UserId = U.UserId where BookId =@BookId;
End