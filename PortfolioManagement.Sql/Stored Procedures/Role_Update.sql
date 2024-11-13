/*
This SP update record in table Role
Created By :: Rekansh Patel
Created On :: 04/23/2017
*/	
CREATE PROCEDURE [dbo].[Role_Update]
	@Id smallint,
	@Name varchar(50),
	@IsPublic bit
AS
BEGIN
	IF NOT EXISTS(SELECT [Id] FROM [Role] WHERE [Name] = @Name AND [Id] != @Id)
	BEGIN
		UPDATE [Role] 
		SET [Name] = @Name,
			IsPublic = @IsPublic
		WHERE [Id] = @Id;
	END
	ELSE
		SET @Id = 0;
	
	SELECT @Id;
END

