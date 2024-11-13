
CREATE FUNCTION [dbo].[fnSplit](@sText nvarchar(MAX), @sDelim nvarchar(10) = ' ')
RETURNS @retArray TABLE (idx int Primary Key, value nvarchar(MAX))
/*********************************************************************************************************************
This function parses a delimited string and returns it as an ID'd table.

Parameter Definition:
---------------------------------
@sText = Delimited string to be parsed.
@sDelim = Delimitation character used to seperate list ov values.

RETURNS:
---------------------------
Returns the table defined below:

Column 		Description
----------		------------------
idx		ID column for array
value		Value split from list.

*********************************************************************************************************************/
AS
BEGIN
DECLARE @idx int, @value nvarchar(MAX)

IF @sDelim = 'Space'
BEGIN
	SET @sDelim = ' '
END

SET @idx = 1
SET @sText = LTrim(RTrim(@sText))

WHILE CHARINDEX(@sDelim,@sText,0) <> 0
BEGIN
	SELECT
		@value=RTRIM(LTRIM(SUBSTRING(@sText,1,CHARINDEX(@sDelim,@sText,0)-1))),
		@sText=RTRIM(LTRIM(SUBSTRING(@sText,CHARINDEX(@sDelim,@sText,0)+LEN(@sDelim),LEN(@sText))))
 
	IF LEN(@value) > 0
	BEGIN
		INSERT @retArray (idx, value) VALUES (@idx, @value)
		SET @idx = @idx + 1
	END
END

IF LEN(@sText) > 0
BEGIN
	INSERT @retArray (idx, value) VALUES (@idx, @sText) -- Put the last item in
END

RETURN
END




