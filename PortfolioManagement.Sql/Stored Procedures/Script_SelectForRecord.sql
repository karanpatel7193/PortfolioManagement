CREATE PROCEDURE [dbo].[Script_SelectForRecord]
	@Id smallint
AS
/***********************************************************************************************
	 NAME     :  Script_SelectForRecord
	 PURPOSE  :  This SP select perticular record from table Script for open page in edit / view mode.
	 REVISIONS:
	 Ver        Date				        Author              Description
	 ---------  ----------					---------------		-------------------------------
	 1.0        03/09/2024					Rekansh Patel             1. Initial Version.	 
************************************************************************************************/
BEGIN
	SELECT S.[Id], S.[Name], S.[BseCode], S.[NseCode], S.[ISINCode], S.[MoneyControlURL], S.[FetchURL], S.[IsFetch], S.[IsActive], S.[Price] ,S.[IndustryName], S.[FaceValue], S.[Group]
	FROM [Script] S
	WHERE S.[Id] = @Id;
END	
	