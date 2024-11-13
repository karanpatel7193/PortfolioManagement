CREATE PROCEDURE [dbo].[Index_UpdateFiiDii]
	@Date date, 
	@FII float, 
	@DII float
AS
/***********************************************************************************************
	 NAME     :  Index_UpdateFiiDii
	 PURPOSE  :  This SP update Fii & Dii data in index table.
	 REVISIONS:
	 Ver        Date				        Author              Description
	 ---------  ----------					---------------		-------------------------------
	 1.0        11/22/2020					Rekansh Patel             1. Initial Version.	 
************************************************************************************************/
BEGIN
	UPDATE	[Index] 
	SET		[FII]				= @FII, 
			[DII]				= @DII
	WHERE	[Date]				= @Date;
END