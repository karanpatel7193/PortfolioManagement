/*
This SP update record in table User
Created By :: Rekansh Patel
Created On :: 04/23/2017
*/	
CREATE PROCEDURE [dbo].[User_Update]
	@Id bigint,
	@FirstName nvarchar(50), 
	@MiddleName nvarchar(50) = NULL, 
	@LastName nvarchar(50), 
	@RoleId smallint, 
	@Gender int,
	@Email nvarchar(250),
	@PhoneNumber VARCHAR(15) =NULL, 
	@Username nvarchar(50), 
	@Password varchar(max), 
	@PasswordSalt varchar(max),
	@IsActive BIT,
	@LastUpdateDateTime DATETIME,
	@ParentUserId BIGINT = NULL
AS
BEGIN
	IF NOT EXISTS(SELECT [Id] FROM [User] WHERE [FirstName] = @FirstName AND [LastName] = @LastName AND [Gender] = @Gender AND ([Username] = @Username OR [Email] = @Email) AND [Id] != @Id)
	BEGIN
		UPDATE [User] 
		SET [FirstName] = @FirstName, 
			[MiddleName] = @MiddleName, 
			[LastName] = @LastName, 
			[RoleId] = @RoleId, 
			[Email] = @Email, 
			[Gender] = @Gender,
			[PhoneNumber] = @PhoneNumber,
			[Username] = @Username, 
			[Password] = @Password, 
			[PasswordSalt] = @PasswordSalt,
			IsActive = @IsActive,
			LastUpdateDateTime = @LastUpdateDateTime
		WHERE [Id] = @Id;
	END
	ELSE
		SET @Id = 0;
	
	SELECT @Id;
END




