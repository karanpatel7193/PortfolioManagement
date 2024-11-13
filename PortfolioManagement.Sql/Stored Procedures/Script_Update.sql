CREATE PROCEDURE [dbo].[Script_Update]
	@Id smallint,
	@Name varchar(30), 
	@BseCode numeric(6,0), 
	@NseCode varchar(30), 
	@ISINCode varchar(30), 
	@MoneyControlURL varchar(MAX), 
	@FetchURL varchar(MAX), 
	@IsFetch bit, 
	@IsActive bit, 
	@IndustryName varchar(MAX), 
	@FaceValue int, 
	@Group varchar(10)
AS
/***********************************************************************************************
	 NAME     :  Script_Update
	 PURPOSE  :  This SP update record in table Script.
	 REVISIONS:
	 Ver        Date				        Author              Description
	 ---------  ----------					---------------		-------------------------------
	 1.0        03/09/2024					Rekansh Patel             1. Initial Version.	 
************************************************************************************************/
BEGIN
		IF NOT EXISTS(SELECT [Id] FROM [Script] WHERE [Name] = @Name AND [BseCode] = @BseCode AND [NseCode] = @NseCode AND [ISINCode] = @ISINCode AND [Id] != @Id)
		BEGIN
			UPDATE [Script] 
			SET [Name] = @Name, 
				[BseCode] = @BseCode, 
				[NseCode] = @NseCode, 
				[ISINCode] = @ISINCode, 
				[MoneyControlURL] = @MoneyControlURL, 
				[FetchURL] = @FetchURL, 
				[IsFetch] = @IsFetch, 
				[IsActive] = @IsActive, 
				[IndustryName] = @IndustryName,
				[FaceValue] = @FaceValue,
				[Group] = @Group

			WHERE [Id] = @Id;
		END
		ELSE
			SET @Id = 0;
	
		SELECT @Id;
END	