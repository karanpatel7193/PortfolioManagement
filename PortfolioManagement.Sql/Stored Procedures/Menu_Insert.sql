/*  
This SP insert record in table Menu  
Created By :: Rekansh Patel  
Created On :: 05/27/2017  
*/
CREATE PROCEDURE [dbo].[Menu_Insert]
	@Name varchar(50),  
	@Description varchar(500) = NULL,  
	@InternalCall bit = 0 , 
	@ParentId int = NULL,
	@PageTitle varchar(50) = NULL,  
	@Icon varchar(250) = NULL,
	@Routing varchar(250) = NULL,
	@OrderBy smallint = NULL,
	@IsMenu BIT = 1,
	@IsClient BIT = 0,
	@IsPublic BIT = 0
AS  
BEGIN
	DECLARE @Id int
	IF NOT EXISTS(SELECT [Id] FROM [Menu] WHERE [Name] = @Name AND ISNULL(ParentId,0) = ISNULL(@ParentId,0) AND IsClient = @IsClient AND IsPublic = @IsPublic)
	BEGIN
		INSERT INTO [Menu] ([Name], [Description], ParentId, PageTitle, Icon, Routing, OrderBy, IsMenu, IsClient, IsPublic)
		VALUES (@Name, @Description, @ParentId, @PageTitle, @Icon, @Routing, @OrderBy, @IsMenu, @IsClient, @IsPublic)

		SET @Id = SCOPE_IDENTITY();
	END
	ELSE
		SET @Id = 0;

	IF (@InternalCall = 0)
		SELECT @Id;  
END

