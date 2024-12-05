/*
This SP update IsActive in table User
Created By :: Rekansh Patel
Created On :: 08/05/2018
*/	
CREATE PROCEDURE [dbo].[User_UpdateActive]
	@Id bigint,
	@IsActive bit, 
	@Email NVARCHAR(250), 
	@LastUpdateDateTime DATETIME
AS
BEGIN
	
	IF EXISTS(SELECT 1 FROM dbo.[User] U WHERE U.Id = @Id OR U.Email = @Email)
	BEGIN
		UPDATE [User] 
		SET [IsActive] = @IsActive,
			[LastUpdateDateTime] = @LastUpdateDateTime
		WHERE [Id] = @Id

		SELECT CONVERT(BIT, 1)
	END
	ELSE
		SELECT CONVERT(BIT, 0)
END




