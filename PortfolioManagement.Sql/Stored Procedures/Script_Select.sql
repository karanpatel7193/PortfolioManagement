CREATE PROCEDURE [dbo].[Script_Select]
	@Id smallint = NULL, 
	@Name varchar(30) = NULL, 
	@BseCode numeric(6,0) = NULL, 
	@NseCode varchar(30) = NULL, 
	@IsFetch bit = NULL, 
	@IsActive bit = NULL
AS
/***********************************************************************************************
	 NAME     :  Script_Select
	 PURPOSE  :  This SP select records from table Script
	 REVISIONS:
	 Ver        Date				        Author              Description
	 ---------  ----------					---------------		-------------------------------
	 1.0        03/09/2024					Rekansh Patel             1. Initial Version.	 
************************************************************************************************/
BEGIN
	SELECT S.[Id], S.[Name], S.[BseCode], S.[NseCode], S.[ISINCode], S.[MoneyControlURL], S.[FetchURL], S.[IsFetch], S.[IsActive], S.[Price], S.[IndustryName], S.[FaceValue], S.[Group]
	FROM [Script] S
	WHERE S.[Id] = COALESCE(@Id, S.[Id])
		 AND S.[Name] = COALESCE(@Name, S.[Name])
		 AND S.[BseCode] = COALESCE(@BseCode, S.[BseCode])
		 AND S.[NseCode] = COALESCE(@NseCode, S.[NseCode])
		 AND S.[IsFetch] = COALESCE(@IsFetch, S.[IsFetch])
		 AND S.[IsActive] = COALESCE(@IsActive, S.[IsActive])
END	