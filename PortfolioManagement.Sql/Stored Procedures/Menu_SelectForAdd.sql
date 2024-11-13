CREATE PROCEDURE [dbo].[Menu_SelectForAdd]
AS
/***********************************************************************************************
	 NAME     :  Menu_SelectForAdd
	 PURPOSE  :  This SP use for fill all LOV in Menu page for add mode
	 REVISIONS:
	 Ver        Date				        Author              Description
	 ---------  ----------					---------------		-------------------------------
	 1.0        03/10/2018					Rekansh Patel             1. Initial Version.	 
************************************************************************************************/
BEGIN
	EXEC Menu_SelectParent
END

