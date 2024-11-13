/*
This SP update record in table Menu
Created By :: Rekansh Patel
Created On :: 05/27/2017
*/
CREATE PROCEDURE [dbo].[Menu_Update]
	@Id int, 
	@Name varchar(50),
	@Description varchar(500) = NULL,  
	@ParentId int = NULL,
	@PageTitle varchar(50) = NULL,  
	@Icon varchar(250) = NULL ,
	@Routing varchar(250) = NULL,
	@OrderBy smallint = NULL,
	@IsMenu BIT = 1,
	@IsClient BIT = 0,
	@IsPublic BIT = 0
AS
BEGIN
	IF NOT EXISTS(SELECT [Id] FROM [Menu] WHERE [Name] = @Name AND ISNULL(ParentId,0) = ISNULL(@ParentId,0) AND IsClient = @IsClient AND IsPublic = @IsPublic AND [Id] != @Id )
	BEGIN
		UPDATE [Menu]
		SET [Name] = @Name,
			[Description] = @Description,
			ParentId = @ParentId,
			PageTitle = @PageTitle,
			Icon = @Icon,
			Routing = @Routing,
			OrderBy = @OrderBy,
			IsMenu = @IsMenu,
			IsClient = @IsClient,
			IsPublic = @IsPublic
		WHERE [Id] = @Id;
	END
	ELSE
		SET @Id = 0;
	
	SELECT @Id;  
END

