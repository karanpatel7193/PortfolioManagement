/*
This SP insert record in table Role
Created By :: Rekansh Patel
Created On :: 04/23/2017
*/	
CREATE PROCEDURE [dbo].[Role_Insert]
	@Name varchar(50),
	@IsPublic BIT 
AS
BEGIN
	DECLARE @Id smallint
	IF NOT EXISTS(SELECT [Id] FROM [Role] WHERE [Name] = @Name)
	BEGIN
		INSERT INTO [Role] ([Name], IsPublic) 
		VALUES (@Name, @IsPublic)
		SET @Id = SCOPE_IDENTITY();
	END
	ELSE
		SET @Id = 0;
	
	SELECT @Id;
END

