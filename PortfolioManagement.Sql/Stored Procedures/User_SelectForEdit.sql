CREATE PROCEDURE [dbo].[User_SelectForEdit]
	@Id bigint
AS
/***********************************************************************************************
	 NAME     :  User_SelectForEdit
	 PURPOSE  :  This SP use for fill all LOV in User page for edit mode
	 REVISIONS:
	 Ver        Date				        Author              Description
	 ---------  ----------					---------------		-------------------------------
	 1.0        02/06/2018					Rekansh Patel             1. Initial Version.	 
************************************************************************************************/
BEGIN
	EXEC User_SelectForRecord @Id

	EXEC User_SelectForAdd 
END

