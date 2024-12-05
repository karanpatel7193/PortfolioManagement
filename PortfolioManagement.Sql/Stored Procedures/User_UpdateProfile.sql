CREATE PROCEDURE [dbo].[User_UpdateProfile]
				@Id              BIGINT,
				@FirstName       nvarchar(50),
				@LastName        nvarchar(50),
				@Email           nvarchar(250),
				@Username        nvarchar(50)
	AS
	BEGIN
		IF EXISTS(SELECT 1 FROM [User] WHERE [Id] = @Id)
		BEGIN
			UPDATE [User]
			SET
				[FirstName] = @FirstName,
				[LastName] = @LastName,
				[Email] = @Email,
				[Username] = @Username
			WHERE [Id] = @Id;
		END
END