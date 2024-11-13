CREATE	PROCEDURE [dbo].[Script_Delete]
	@Id		smallint
AS
/***********************************************************************************************
	 NAME     :  Script_Delete
	 PURPOSE  :  This SP delete record from table Script
	 REVISIONS:
	 Ver        Date				        Author              Description
	 ---------  ----------					---------------		-------------------------------
	 1.0        03/09/2024					Rekansh Patel             1. Initial Version.	 
************************************************************************************************/
BEGIN
		DELETE FROM [Script] 
		WHERE [Id] = @Id;
END	