CREATE PROCEDURE [dbo].[PMS_Update]
	@Id int,
	@Name varchar(255),
	@IsActive bit,
	@Type varchar(100)
AS
BEGIN
	IF NOT EXISTS(SELECT [Id] FROM [PMS] WHERE [Name] = @Name AND [Id] != @Id)
	BEGIN
		UPDATE [PMS] 
		SET [Name] = @Name,
			[IsActive] = @IsActive,
			[Type] = @Type
		WHERE [Id] = @Id;
	END
	ELSE
		SET @Id = 0;
	
	SELECT @Id;
END