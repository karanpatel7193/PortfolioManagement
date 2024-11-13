CREATE PROCEDURE [dbo].[PMS_Insert]
	@Name varchar(255),
	@IsActive BIT ,
	@Type varchar(100)
AS
BEGIN
	DECLARE @Id smallint
	IF NOT EXISTS(SELECT [Id] FROM [PMS] WHERE [Name] = @Name)
	BEGIN
		INSERT INTO [PMS] ([Name], [IsActive], P.[Type]) 
		VALUES (@Name, @IsActive, @Type)
		SET @Id = SCOPE_IDENTITY();
	END
	ELSE
		SET @Id = 0;
	
	SELECT @Id;
END
