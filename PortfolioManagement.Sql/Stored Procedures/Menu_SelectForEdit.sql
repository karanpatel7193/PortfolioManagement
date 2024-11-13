CREATE PROCEDURE [dbo].[Menu_SelectForEdit]
	@Id int
AS
/***********************************************************************************************
	 NAME     :  Menu_SelectForEdit
	 PURPOSE  :  This SP use for fill all LOV in Menu page for edit mode
	 REVISIONS:
	 Ver        Date				        Author              Description
	 ---------  ----------					---------------		-------------------------------
	 1.0        03/10/2018					Rekansh Patel             1. Initial Version.	 
************************************************************************************************/
BEGIN
	EXEC Menu_SelectForRecord @Id

	EXEC Menu_SelectForAdd
END

