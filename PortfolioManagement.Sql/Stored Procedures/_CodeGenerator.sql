/*
This SP useful for generate 
Classes like Business, Entity, WebAPI, 
Page HTML, 
Angular code like controller, service, model 
store procedure like Insert, Update, Delete and Select.
Created By  : Rekansh Patel
Created Date: 7/3/2016

EXEC [dbo].[_CodeGenerator]
	@TableName = 'Users', --Actual database table name
	@TableShortName = 'U', --Table short name, if table name is UserRole then short name is UR
	@PKFieldName = 'Id', --Table primary key field name
	@AlreadyExistFields = 'UserName ShortName LongName', --Already exists check field list by space seperated while insert or update
	@IsTransactionApply = 1,
	@ClsTableName = 'User', --Class name except word cls and Business, Entity, Controller means if you enter User then Business > clsUserBusiness, Entity > clsUserEntity, model > UserModel etc...
	@AddLOVs = 'Organization UserRole Facility', --Singular table name list by space sperated for page add/edit mode
	@ListLOVs = 'Organization', --Singular table name list by space sperated for page list mode
	@MainColumns = 'Id UserName ShortName LongName', --Main fields list by space seperated for LOV
	@ParameterColumns = 'OrgId Id', --Field list by space seperated for create where part and parameter class
	@Namespace = 'Admin.Account', --Class namespace for generete namespace and routes
	@DevelopmentBy	= 'Rekansh Patel' --Developer name

*/
CREATE PROCEDURE [dbo].[_CodeGenerator]
	@TableName NVARCHAR(MAX),
	@TableShortName NVARCHAR(MAX) = NULL,
	@PKFieldName NVARCHAR(MAX),
	@AlreadyExistFields NVARCHAR(MAX) = NULL,
	@IsTransactionApply bit = false,
	@ClsTableName NVARCHAR(MAX) = NULL,
	@AddLOVs NVARCHAR(MAX) = NULL, --Singular table name
	@ListLOVs NVARCHAR(MAX) = NULL, --Singular table name
	@MainColumns NVARCHAR(MAX) = NULL,
	@ParameterColumns NVARCHAR(MAX) = NULL,
	@Namespace NVARCHAR(MAX) = NULL,
	@DevelopmentBy	VARCHAR(50) = 'Auto generated',
	@DevelopmentDateTime DATE = NULL
AS
BEGIN
	IF(@ClsTableName IS NULL)
		SET @ClsTableName = REPLACE(@TableName, 'tbl', '')
	
	IF(@TableShortName IS NULL)
		SET @TableShortName = SUBSTRING(@TableName, 1, 1)
	
	DECLARE @MainColumnsTable table(
		FieldName NVARCHAR(MAX)
	)
	INSERT INTO @MainColumnsTable(FieldName)
	SELECT t.value
	FROM dbo.fnSplit(@MainColumns, ' ') t
	
	DECLARE @ParameterColumnsTable table(
		FieldName NVARCHAR(MAX)
	)
	INSERT INTO @ParameterColumnsTable(FieldName)
	SELECT t.value
	FROM dbo.fnSplit(@ParameterColumns, ' ') t
	
	DECLARE @Route NVARCHAR(MAX) = REPLACE(@Namespace, '.','/')
	DECLARE @AddLOVsCount INT
	DECLARE @ListLOVsCount int
	SELECT @AddLOVsCount = count(1) FROM dbo.fnSplit(@AddLOVs, ' ') t
	SELECT @ListLOVsCount = count(1) FROM dbo.fnSplit(@ListLOVs, ' ') t

	DECLARE @Business_String NVARCHAR(MAX) = ''
	DECLARE @Entity_String NVARCHAR(MAX) = ''
	DECLARE @PKFieldDataType NVARCHAR(MAX) = ''
	DECLARE @PKNetFieldDataType NVARCHAR(MAX) = ''
	DECLARE @PKNetConvert NVARCHAR(MAX) = ''

	DECLARE @PublicProperty NVARCHAR(MAX) = ''
	DECLARE @SetDefaulValue NVARCHAR(MAX) = ''
	DECLARE @PublicMainProperty NVARCHAR(MAX) = ''
	DECLARE @SetMainDefaulValue NVARCHAR(MAX) = ''
	DECLARE @SpSelectMainFields NVARCHAR(MAX) = ''
	DECLARE @PublicParameterProperty NVARCHAR(MAX) = ''
	DECLARE @SetParameterDefaulValue NVARCHAR(MAX) = ''
	
	DECLARE @ColumnName NVARCHAR(MAX) = ''
	DECLARE @DataType NVARCHAR(MAX) = ''
	DECLARE @IsNullable bit
	DECLARE @DataSize NVARCHAR(MAX) = ''
	DECLARE @NetDataType NVARCHAR(MAX) = ''
	DECLARE @NetConvert NVARCHAR(MAX) = ''
	DECLARE @NetDefaultValue NVARCHAR(MAX) = ''

	DECLARE @InsertAddParameter NVARCHAR(MAX) = ''
	DECLARE @UpdateAddParameter NVARCHAR(MAX) = ''
	DECLARE @SelectAddParameter NVARCHAR(MAX) = ''
	DECLARE @SpSelectForParameter NVARCHAR(MAX) = ''
	DECLARE @SpSelectForParameterPass NVARCHAR(MAX) = ''
	DECLARE @SpSelectForParameterWhere NVARCHAR(MAX) = ''
	DECLARE @BusinessParameter NVARCHAR(MAX) = ''

	DECLARE @SpInsertString NVARCHAR(MAX) = ''
	DECLARE @AlreadyExistInsertSting NVARCHAR(MAX) = ''
	DECLARE @SpInsertParameter NVARCHAR(MAX) = ''
	DECLARE @SpInsertField NVARCHAR(MAX) = ''
	DECLARE @SpInsertValue NVARCHAR(MAX) = ''

	DECLARE @SpUpdateString NVARCHAR(MAX) = ''
	DECLARE @AlreadyExistUpdateSting NVARCHAR(MAX) = ''
	DECLARE @SpUpdateParameter NVARCHAR(MAX) = ''
	DECLARE @SpUpdateSet NVARCHAR(MAX) = ''

	DECLARE @SpDeleteString NVARCHAR(MAX) = ''

	DECLARE @SpSelectString NVARCHAR(MAX) = ''
	DECLARE @SpSelectForAddString NVARCHAR(MAX) = ''
	DECLARE @SpSelectForEditString NVARCHAR(MAX) = ''
	DECLARE @SpSelectForGridString NVARCHAR(MAX) = ''
	DECLARE @SpSelectForListString NVARCHAR(MAX) = ''
	DECLARE @SpSelectForLOVString NVARCHAR(MAX) = ''
	DECLARE @SpSelectForRecordString NVARCHAR(MAX) = ''

	DECLARE @SpSelectParameter NVARCHAR(MAX) = ''
	DECLARE @SpSelectParameterPass NVARCHAR(MAX) = ''
	DECLARE @SpSelectField NVARCHAR(MAX) = ''
	DECLARE @SpSelectWhere NVARCHAR(MAX) = ''
	
	DECLARE @MapDataColumns NVARCHAR(MAX) = ''

	DECLARE @AngularSetDefaulValue NVARCHAR(MAX) = ''
	DECLARE @AngularModelString NVARCHAR(MAX) = ''
	DECLARE @ModelDefination NVARCHAR(MAX) = ''
	DECLARE @ModelMainDefination NVARCHAR(MAX) = ''
	DECLARE @ModelParameter NVARCHAR(MAX) = ''
	
	DECLARE @WebAPIControllerString NVARCHAR(MAX) = ''

	DECLARE @AngularServiceString NVARCHAR(MAX) = ''
	DECLARE @AngularRouteString NVARCHAR(MAX) = ''
	DECLARE @AngularAddEditControllerString NVARCHAR(MAX) = ''
	DECLARE @AngularListControllerString NVARCHAR(MAX) = ''
	
	DECLARE @HtmlControlType NVARCHAR(MAX) = ''
	DECLARE @HtmlInputModeFields NVARCHAR(MAX) = ''
	DECLARE @HtmlSearchModeFields NVARCHAR(MAX) = ''
	DECLARE @HtmlGridFieldsHeader NVARCHAR(MAX) = ''
	DECLARE @HtmlGridFieldsDetail NVARCHAR(MAX) = ''
	DECLARE @AddEditHtmlString NVARCHAR(MAX) = ''
	DECLARE @ListHtmlString NVARCHAR(MAX) = ''
	
	DECLARE @Lov NVARCHAR(MAX)
	DECLARE @AddLovsEntity NVARCHAR(MAX) = ''
	DECLARE @AddLovsModel NVARCHAR(MAX) = ''
	DECLARE @AddEditLovDepRoute NVARCHAR(MAX) = ''
	DECLARE @AddLovSetAngular NVARCHAR(MAX) = ''
	DECLARE @EditLovSetAngular NVARCHAR(MAX) = ''
	DECLARE @AddEditLovDeclareAngular NVARCHAR(MAX) = ''

	DECLARE @ListLovsEntity NVARCHAR(MAX) = ''
	DECLARE @ListLovsModel NVARCHAR(MAX) = ''
	DECLARE @ListLovDepRoute NVARCHAR(MAX) = ''
	DECLARE @ListLovSetAngular NVARCHAR(MAX) = ''
	DECLARE @ListLovDeclareAngular NVARCHAR(MAX) = ''

	DECLARE @MapAddEntity NVARCHAR(MAX) = ''
	DECLARE @MapEditEntity NVARCHAR(MAX) = ''
	DECLARE @MapListEntity NVARCHAR(MAX) = ''
	declare @AddEditResultSet INT = 0
	declare @ListResultSet INT = 0

	DECLARE @AddLovsSPCall NVARCHAR(MAX) = ''
	DECLARE @ListLovsSPCall NVARCHAR(MAX) = ''
	
	DECLARE @Angular2DataType NVARCHAR(MAX) = ''
	DECLARE @PKAngular2FieldDataType NVARCHAR(MAX) = ''
	DECLARE @Angular2ModelFieldWithDefault NVARCHAR(MAX) = ''
	DECLARE @Angular2MainModelFieldWithDefault NVARCHAR(MAX) = ''
	DECLARE @Angular2ParameterModelFieldWithDefault NVARCHAR(MAX) = ''
	DECLARE @Angular2LOVImportModel NVARCHAR(MAX) = ''

	DECLARE @Angular2AddLOVModel NVARCHAR(MAX) = ''
	DECLARE @Angular2ListLOVModel NVARCHAR(MAX) = ''
	DECLARE @HtmlInputModeFields2 NVARCHAR(MAX) = ''
	DECLARE @HtmlSearchModeFields2 NVARCHAR(MAX) = ''
	DECLARE @HtmlGridFieldsHeader2 NVARCHAR(MAX) = ''
	DECLARE @HtmlGridFieldsDetail2 NVARCHAR(MAX) = ''

	DECLARE @Angular2ModuleString NVARCHAR(MAX) = ''
	DECLARE @Angular2ModelString NVARCHAR(MAX) = ''
	DECLARE @Angular2ServiceString NVARCHAR(MAX) = ''
	DECLARE @Angular2RouteString NVARCHAR(MAX) = ''
	DECLARE @Angular2ComponentString NVARCHAR(MAX) = ''
	DECLARE @Angular2FormHTMLString NVARCHAR(MAX) = ''
	DECLARE @Angular2FormComponentString NVARCHAR(MAX) = ''
	DECLARE @Angular2ListHTMLString NVARCHAR(MAX) = ''
	DECLARE @Angular2ListComponentString NVARCHAR(MAX) = ''

	IF(@AlreadyExistFields IS NOT NULL)
	BEGIN
		SELECT @AlreadyExistInsertSting = CASE @AlreadyExistInsertSting WHEN '' THEN 'SELECT [' + @PKFieldName + '] FROM [' + @TableName + '] WHERE [' + t.value + '] = @' + t.value 
			ELSE @AlreadyExistInsertSting + ' AND [' + t.value + '] = @' + t.value END
		FROM dbo.fnSplit(@AlreadyExistFields, ' ') t

		SET @AlreadyExistUpdateSting = @AlreadyExistInsertSting + ' AND [' + @PKFieldName + '] != @' + @PKFieldName 
	END

	DECLARE @CurColumn CURSOR
	SET @CurColumn = CURSOR FOR 
		SELECT c.name ColumnName, t.name DataType, c.IsNullable,
			CASE t.name WHEN 'nvarchar' THEN '(' + CASE c.length WHEN -1 THEN 'MAX' ELSE CONVERT(VARCHAR, c.length/2) END + ')'
				WHEN 'char' THEN '(' + CASE c.length WHEN -1 THEN 'MAX' ELSE CONVERT(VARCHAR, c.length) END + ')'
				WHEN 'varchar' THEN '(' + CASE c.length WHEN -1 THEN 'MAX' ELSE CONVERT(VARCHAR, c.length) END + ')'
				WHEN 'numeric' THEN '(' + CONVERT(VARCHAR, c.xprec) + ',' + CONVERT(VARCHAR, c.xscale) + ')'
				WHEN 'decimal' THEN '(' + CONVERT(VARCHAR, c.xprec) + ',' + CONVERT(VARCHAR, c.xscale) + ')'
				ELSE '' END DataSize
		FROM sysobjects o
			INNER JOIN syscolumns c ON o.id = c.id
			INNER JOIN systypes t ON c.xtype = t.xtype
		WHERE o.name = @TableName AND t.name != 'sysname'
		ORDER BY colid

	OPEN @CurColumn; 

	FETCH NEXT FROM @CurColumn INTO @ColumnName, @DataType, @IsNullable, @DataSize
	WHILE @@FETCH_STATUS = 0
	BEGIN
		SELECT @NetDataType = CASE @DataType
			WHEN 'tinyint' THEN 'int'
			WHEN 'smallint' THEN 'int'
			WHEN 'int' THEN 'int'
			WHEN 'bigint' THEN 'long'
			WHEN 'bit' THEN 'bool'
			WHEN 'nvarchar' THEN 'string'
			WHEN 'date' THEN 'DateTime'
			WHEN 'datetime' THEN 'DateTime'
			WHEN 'float' THEN 'double'
			WHEN 'numeric' THEN 'decimal'
			WHEN 'decimal' THEN 'decimal'
			WHEN 'char' THEN 'string'
			WHEN 'xml' THEN 'string'
			else 'string' end

		SELECT @NetConvert = CASE @DataType
			WHEN 'tinyint' THEN 'ToInt'
			WHEN 'smallint' THEN 'ToInt'
			WHEN 'int' THEN 'ToInt'
			WHEN 'bigint' THEN 'ToLong'
			WHEN 'bit' THEN 'ToBoolean'
			WHEN 'nvarchar' THEN 'ToString'
			WHEN 'datetime' THEN 'ToDateTime'
			WHEN 'date' THEN 'ToDateTime'
			WHEN 'float' THEN 'ToDouble'
			WHEN 'numeric' THEN 'ToDecimal'
			WHEN 'decimal' THEN 'ToDecimal'
			WHEN 'char' THEN 'ToString'
			WHEN 'xml' THEN 'ToString'
			else 'ToString' end

		SELECT @NetDefaultValue = CASE @DataType
			WHEN 'tinyint' THEN '0' --0
			WHEN 'smallint' THEN '0'
			WHEN 'int' THEN '0'
			WHEN 'bigint' THEN '0'
			WHEN 'bit' THEN 'false' --false
			WHEN 'nvarchar' THEN 'string.Empty'
			WHEN 'datetime' THEN 'DateTime.MinValue' --Todate ->Min Date
			WHEN 'date' THEN 'DateTime.MinValue' --Todate ->Min Date
			WHEN 'float' THEN '0'
			WHEN 'numeric' THEN '0'
			WHEN 'decimal' THEN '0'
			WHEN 'char' THEN 'string.Empty'
			WHEN 'xml' THEN 'string.Empty'
			else 'string.Empty' end

		SELECT @Angular2DataType = CASE @DataType
			WHEN 'tinyint' THEN 'number'
			WHEN 'smallint' THEN 'number'
			WHEN 'int' THEN 'number'
			WHEN 'bigint' THEN 'number'
			WHEN 'bit' THEN 'boolean'
			WHEN 'nvarchar' THEN 'string' 
			WHEN 'datetime' THEN 'Date'
			WHEN 'date' THEN 'Date'
			WHEN 'float' THEN 'number'
			WHEN 'numeric' THEN 'number'
			WHEN 'decimal' THEN 'number'
			WHEN 'char' THEN 'string'
			WHEN 'xml' THEN 'string'
			else 'string' end

		SELECT @AngularSetDefaulValue = CASE @DataType
			WHEN 'tinyint' THEN '0'
			WHEN 'smallint' THEN '0'
			WHEN 'int' THEN '0'
			WHEN 'bigint' THEN '0'
			WHEN 'bit' THEN 'false'
			WHEN 'nvarchar' THEN ''''''
			WHEN 'datetime' THEN 'new Date(0)'
			WHEN 'date' THEN 'new Date(0)'
			WHEN 'float' THEN '0'
			WHEN 'numeric' THEN '0'
			WHEN 'decimal' THEN '0'
			WHEN 'char' THEN ''''''
			WHEN 'xml' THEN ''''''
			else '''''' end
			
		SELECT @HtmlControlType = CASE @DataType
			WHEN 'tinyint' THEN 'number'
			WHEN 'smallint' THEN 'number'
			WHEN 'int' THEN 'number'
			WHEN 'bigint' THEN 'number'
			WHEN 'bit' THEN 'checkbox'
			WHEN 'nvarchar' THEN 'text'
			WHEN 'date' THEN 'date'
			WHEN 'datetime' THEN 'datetime'
			WHEN 'float' THEN 'number'
			WHEN 'numeric' THEN 'number'
			WHEN 'decimal' THEN 'number'
			WHEN 'char' THEN 'text'
			WHEN 'xml' THEN 'text'
			else 'text' end

		IF(@PKFieldName = @ColumnName)
		BEGIN
			SET @PKFieldDataType = @DataType
			SET @PKNetFieldDataType = @NetDataType
			SET @PKAngular2FieldDataType = @NetDataType
			SET @PKNetConvert = @NetConvert
		END

		IF EXISTS(SELECT * FROM @MainColumnsTable WHERE FieldName = @ColumnName)
		BEGIN
			SET @PublicMainProperty = @PublicMainProperty + '/// <summary>' + CHAR(13) + '/// Get & Set ' + dbo.fnUserFriendlyName(@ColumnName)  + CHAR(13) + '/// </summary>' + CHAR(13) + 'public ' + @NetDataType + ' ' + @ColumnName + ' { get; set; } ' + CHAR(13)  + CHAR(13) 
			SET @SetMainDefaulValue = @SetMainDefaulValue + @ColumnName + ' = ' + @NetDefaultValue + '; ' + CHAR(13) 
			SET @SpSelectMainFields = CASE @SpSelectMainFields WHEN '' THEN  @TableShortName + '.[' + @ColumnName + ']' else @SpSelectMainFields + ', ' + @TableShortName + '.[' + @ColumnName + ']' END
			SET @ModelMainDefination = CASE @ModelMainDefination WHEN '' THEN 'this.' + dbo.fnJavaScriptName(@ColumnName) + ' = ' + @AngularSetDefaulValue + ';' ELSE @ModelMainDefination + 'this.' + dbo.fnJavaScriptName(@ColumnName) + ' = ' + @AngularSetDefaulValue + ';' END + CHAR(13)
			SET @Angular2MainModelFieldWithDefault = CASE @Angular2MainModelFieldWithDefault WHEN '' THEN 'public ' + dbo.fnJavaScriptName(@ColumnName) + ': ' + @Angular2DataType + ' = ' + @AngularSetDefaulValue + ';' ELSE @Angular2MainModelFieldWithDefault + 'public ' + dbo.fnJavaScriptName(@ColumnName) + ': ' + @Angular2DataType + ' = ' + @AngularSetDefaulValue + ';' END + CHAR(13)
		END
		ELSE
		BEGIN
			SET @PublicProperty = @PublicProperty + '/// <summary>' + CHAR(13) + '/// Get & Set ' + dbo.fnUserFriendlyName(@ColumnName)  + CHAR(13) + '/// </summary>' + CHAR(13) + 'public ' + @NetDataType + ' ' + @ColumnName + ' { get; set; } ' + CHAR(13)  + CHAR(13) 
			SET @SetDefaulValue = @SetDefaulValue + @ColumnName + ' = ' + @NetDefaultValue + '; ' + CHAR(13) 
			SET @Angular2ModelFieldWithDefault = CASE @Angular2ModelFieldWithDefault WHEN '' THEN 'public ' + dbo.fnJavaScriptName(@ColumnName) + ': ' + @Angular2DataType + ' = ' + @AngularSetDefaulValue + ';' ELSE @Angular2ModelFieldWithDefault + 'public ' + dbo.fnJavaScriptName(@ColumnName) + ': ' + @Angular2DataType + ' = ' + @AngularSetDefaulValue + ';' END + CHAR(13)
		END

		IF EXISTS(SELECT * FROM @ParameterColumnsTable WHERE FieldName = @ColumnName)
		BEGIN
			SET @PublicParameterProperty = @PublicParameterProperty + '/// <summary>' + CHAR(13) + '/// Get & Set ' + dbo.fnUserFriendlyName(@ColumnName)  + CHAR(13) + '/// </summary>' + CHAR(13) + 'public ' + @NetDataType + ' ' + @ColumnName + ' { get; set; } ' + CHAR(13)  + CHAR(13) 
			SET @SetParameterDefaulValue = @SetParameterDefaulValue + @ColumnName + ' = ' + @NetDefaultValue + '; ' + CHAR(13) 

			SET @BusinessParameter = @BusinessParameter + 'if(' + dbo.fnJavaScriptName(@ClsTableName) + 'ParameterEntity.' + @ColumnName + ' != ' + @NetDefaultValue + ')' + CHAR(13) + 'sql.AddParameter("' + @ColumnName + '", ' + dbo.fnJavaScriptName(@ClsTableName) + 'ParameterEntity.' + @ColumnName + ');' + CHAR(13)
			
			SET @SpSelectForParameter = CASE @SpSelectForParameter WHEN '' THEN '@' + @ColumnName + ' ' + @DataType + @DataSize + CASE @IsNullable WHEN 1 THEN ' = NULL' ELSE '' END ELSE @SpSelectForParameter + ', ' + CHAR(13) + '@' + @ColumnName + ' ' + @DataType + @DataSize + CASE @IsNullable WHEN 1 THEN ' = NULL' ELSE '' END END
			SET @SpSelectForParameterPass = CASE @SpSelectForParameterPass WHEN ' ' THEN '@' + @ColumnName ELSE @SpSelectForParameterPass + ', @' + @ColumnName END
			SET @SpSelectForParameterWhere = CASE @SpSelectForParameterWhere WHEN '' THEN @TableShortName + '.[' + @ColumnName + ']' + ' = COALESCE(@' + @ColumnName + ', ' + @TableShortName + '.[' + @ColumnName + '])' ELSE @SpSelectForParameterWhere + CHAR(13) + ' AND ' + @TableShortName + '.[' + @ColumnName + '] = COALESCE(@' + @ColumnName + ', ' + @TableShortName + '.[' + @ColumnName + '])' END

			SET @ModelParameter = CASE @ModelParameter WHEN '' THEN 'this.' + dbo.fnJavaScriptName(@ColumnName) + ' = ' + @AngularSetDefaulValue + ';' ELSE @ModelParameter + 'this.' + dbo.fnJavaScriptName(@ColumnName) + ' = ' + @AngularSetDefaulValue + ';' END + CHAR(13)

			SET @Angular2ParameterModelFieldWithDefault = CASE @Angular2ParameterModelFieldWithDefault WHEN '' THEN 'public ' + dbo.fnJavaScriptName(@ColumnName) + ': ' + @Angular2DataType + ' = ' + @AngularSetDefaulValue + ';' ELSE @Angular2ParameterModelFieldWithDefault + 'public ' + dbo.fnJavaScriptName(@ColumnName) + ': ' + @Angular2DataType + ' = ' + @AngularSetDefaulValue + ';' END + CHAR(13)
		END

		if(@PKFieldName != @ColumnName)
		begin
			SET @InsertAddParameter = @InsertAddParameter + CASE @IsNullable WHEN 1 THEN 'if(' + dbo.fnJavaScriptName(@ClsTableName) + 'Entity.' + @ColumnName + ' != ' + @NetDefaultValue + ')' + CHAR(13) ELSE '' END + 'sql.AddParameter("' + @ColumnName + '", ' + dbo.fnJavaScriptName(@ClsTableName) + 'Entity.' + @ColumnName + ');' + CHAR(13)
			SET @SpInsertParameter = CASE @SpInsertParameter WHEN '' THEN '@' + @ColumnName + ' ' + @DataType + @DataSize + case @IsNullable WHEN 1 THEN ' = NULL' ELSE '' END ELSE @SpInsertParameter + ', ' + CHAR(13) + '@' + @ColumnName + ' ' + @DataType + @DataSize + CASE @IsNullable WHEN 1 THEN ' = NULL' ELSE '' END END
			SET @SpInsertField = CASE @SpInsertField WHEN '' THEN '[' + @ColumnName + ']' ELSE @SpInsertField + ', [' + @ColumnName + ']' END 
			SET @SpInsertValue = CASE @SpInsertValue WHEN '' THEN '@' + @ColumnName ELSE @SpInsertValue + ', ' + '@' + @ColumnName END
			SET @SpUpdateSet = CASE @SpUpdateSet WHEN '' THEN '[' + @ColumnName + '] = @' + @ColumnName else @SpUpdateSet + ', ' + CHAR(13) + '[' + @ColumnName + '] = @' + @ColumnName END
		end

		SET @SelectAddParameter = @SelectAddParameter + 'if(' + dbo.fnJavaScriptName(@ClsTableName) + 'Entity.' + @ColumnName + ' != ' + @NetDefaultValue + ')' + CHAR(13) + 'sql.AddParameter("' + @ColumnName + '", ' + dbo.fnJavaScriptName(@ClsTableName) + 'Entity.' + @ColumnName + ');' + CHAR(13)
		SET @SpSelectParameter = CASE @SpSelectParameter WHEN '' THEN '@' + @ColumnName + ' ' + @DataType + @DataSize + ' = NULL' ELSE @SpSelectParameter + ', ' + CHAR(13) + '@' + @ColumnName + ' ' + @DataType + @DataSize + ' = NULL' END
		SET @SpSelectParameterPass = CASE @SpSelectParameterPass WHEN '' THEN '@' + @ColumnName ELSE @SpSelectParameterPass + ', @' + @ColumnName END
		SET @SpSelectField = CASE @SpSelectField WHEN '' THEN @TableShortName + '.[' + @ColumnName + ']' ELSE @SpSelectField + ', ' + @TableShortName + '.[' + @ColumnName + ']' END
		SET @SpSelectWhere = CASE @SpSelectWhere WHEN '' THEN @TableShortName + '.[' + @ColumnName + ']' + ' = COALESCE(@' + @ColumnName + ', ' + @TableShortName + '.[' + @ColumnName + '])' else @SpSelectWhere + CHAR(13) + ' AND ' + @TableShortName + '.[' + @ColumnName + '] = COALESCE(@' + @ColumnName + ', ' + @TableShortName + '.[' + @ColumnName + '])' END

		SET @MapDataColumns = @MapDataColumns + 'case "' + @ColumnName + '":' + CHAR(13) + dbo.fnJavaScriptName(@ClsTableName) + 'Entity.' + @ColumnName + ' = MyConvert.' + @NetConvert + '(reader["' + @ColumnName + '"]);' + CHAR(13) + 'break;' + CHAR(13)
		
		SET @ModelDefination = CASE @ModelDefination WHEN '' THEN 'this.' + dbo.fnJavaScriptName(@ColumnName) + ' = ' + @AngularSetDefaulValue + ';' ELSE @ModelDefination + 'this.' + dbo.fnJavaScriptName(@ColumnName) + ' = ' + @AngularSetDefaulValue + ';' END + CHAR(13)

		SET @HtmlSearchModeFields = @HtmlSearchModeFields + N'
							<div class="form-group">
								<label for="inputSearch' + @ColumnName + '" class="col-sm-2 control-label">' + dbo.fnUserFriendlyName(@ColumnName) + '</label>
								<div class="col-sm-10">
									<input type="number" class="form-control" name="inputSearch' + @ColumnName + '" ng-model="' + dbo.fnJavaScriptName(@ClsTableName) + 'Search.' + dbo.fnJavaScriptName(@ColumnName) + '" placeholder="' + dbo.fnUserFriendlyName(@ColumnName) + '" />
								</div>
							</div>
		'
		SET @HtmlInputModeFields = @HtmlInputModeFields + N'
							<div class="form-group {{(' + dbo.fnJavaScriptName(@ClsTableName) + 'Form.$submitted && ' + dbo.fnJavaScriptName(@ClsTableName) + 'Form.input' + @ColumnName + '.$invalid) || (' + dbo.fnJavaScriptName(@ClsTableName) + 'Form.input' + @ColumnName + '.$touched && ' + dbo.fnJavaScriptName(@ClsTableName) + 'Form.input' + @ColumnName + '.$invalid) ? ''has-error'' : ''''}}">
								<label for="input' + @ColumnName + '" class="col-sm-2 control-label">' + dbo.fnUserFriendlyName(@ColumnName) + '</label>
								<div class="col-sm-10">
									<input type="' + @HtmlControlType + '" class="form-control" name="input' + @ColumnName + '" ng-model="' + dbo.fnJavaScriptName(@ClsTableName) + '.' + dbo.fnJavaScriptName(@ColumnName) + '" placeholder="' + dbo.fnUserFriendlyName(@ColumnName) + '" ng-required="true" />
									<div class="tooltip bottom fade in" ng-if="(' + dbo.fnJavaScriptName(@ClsTableName) + 'Form.$submitted && ' + dbo.fnJavaScriptName(@ClsTableName) + 'Form.input' + @ColumnName + '.$invalid) || (' + dbo.fnJavaScriptName(@ClsTableName) + 'Form.input' + @ColumnName + '.$touched && ' + dbo.fnJavaScriptName(@ClsTableName) + 'Form.input' + @ColumnName + '.$invalid)">
										<div class="tooltip-arrow"></div>
										<div class="tooltip-inner ng-binding" ng-if="' + dbo.fnJavaScriptName(@ClsTableName) + 'Form.input' + @ColumnName + '.$error.required">' + dbo.fnUserFriendlyName(@ColumnName) + ' is required.</div>
									</div>
								</div>
							</div>			
		'
		
		SET @HtmlGridFieldsHeader = @HtmlGridFieldsHeader + '<th ng-sort class="link" scope-sort-expression="' + dbo.fnJavaScriptName(@ClsTableName) + 'Search.SortExpression" scope-sort-direction="' + dbo.fnJavaScriptName(@ClsTableName) + 'Search.SortDirection" display-name="' + dbo.fnUserFriendlyName(@ColumnName) + '" sort-expression="' + @ColumnName + '" ng-click="sortRecord(''' + @ColumnName + ''');"></th>' + CHAR(13)
		SET @HtmlGridFieldsDetail = @HtmlGridFieldsDetail + '<td>{{'+ dbo.fnJavaScriptName(@ClsTableName) + 'List.' + dbo.fnJavaScriptName(@ColumnName) + '}}</td>' + CHAR(13)

		IF(@HtmlControlType = 'checkbox')
		BEGIN
			SET @HtmlSearchModeFields2 = @HtmlSearchModeFields2 + N'
                    <label class="custom-control custom-checkbox">
                        <input type="checkbox" class="custom-control-input" [(ngModel)]="' + dbo.fnJavaScriptName(@ClsTableName) + 'Parameter.' + dbo.fnJavaScriptName(@ColumnName) + '" id="inSearch#Language#' + @ColumnName + '" name="inSearch#Language#' + @ColumnName + '">
                        <span class="custom-control-indicator"></span>
                        <span class="custom-control-description">' + dbo.fnUserFriendlyName(@ColumnName) + '</span>
                    </label>
			'

			SET @HtmlInputModeFields2 = @HtmlInputModeFields2 + N'
                    <label class="custom-control custom-checkbox">
                        <input type="checkbox" class="custom-control-input" [(ngModel)]="' + dbo.fnJavaScriptName(@ClsTableName) + '.' + dbo.fnJavaScriptName(@ColumnName) + '" id="in#Language#' + @ColumnName + '" name="in#Language#' + @ColumnName + '">
                        <span class="custom-control-indicator"></span>
                        <span class="custom-control-description">' + dbo.fnUserFriendlyName(@ColumnName) + '</span>
                    </label>
			'
		END
		ELSE IF(@HtmlControlType = 'number')
		BEGIN
			SET @HtmlSearchModeFields2 = @HtmlSearchModeFields2 + N'
                    <div class="col-lg-4 col-md-4">
						<div class="form-group">
							<label for="in#Language#' + @ColumnName + '">' + dbo.fnUserFriendlyName(@ColumnName) + '</label>
							<div class="input-group-sm">
								<select class="form-control" [(ngModel)]="#Language#' + @ClsTableName + 'Parameter.' + dbo.fnJavaScriptName(@ColumnName) + '" id="inSearch#Language#' + @ColumnName + '" name="inSearch#Language#' + @ColumnName + '" >
                                    <option value="0">Select ' + dbo.fnUserFriendlyName(REPLACE(@ColumnName, 'Id', '')) + '</option>
									<option [value]="item.Id" *ngFor="let item of ' + REPLACE(@ColumnName, 'Id', '') + 's">{{item.Name}}</option>
								</select>
							</div>
						</div>
                    </div>
			'

			SET @HtmlInputModeFields2 = @HtmlInputModeFields2 + N'
						<div class="form-group">
							<label for="in#Language#' + @ColumnName + '">' + dbo.fnUserFriendlyName(@ColumnName) + '</label>
							<div class="input-group-sm">' + CASE @IsNullable WHEN 0 THEN '
								<select class="form-control" [(ngModel)]="' + dbo.fnJavaScriptName(@ClsTableName) + '.' + dbo.fnJavaScriptName(@ColumnName) + '" id="in#Language#' + @ColumnName + '" name="in#Language#' + @ColumnName + '" required [ngClass]="{''form-control-danger'': (#Language#' + @ClsTableName + 'Form.submitted && #Language#' + @ClsTableName + 'Form.form.controls.in#Language#' + @ColumnName + '?.invalid) || (#Language#' + @ClsTableName + 'Form.form.controls.in#Language#' + @ColumnName + '?.touched && #Language#' + @ClsTableName + 'Form.form.controls.in#Language#' + @ColumnName + '?.invalid)}" >
									<option [value]="item.Id" *ngFor="let item of ' + REPLACE(@ColumnName, 'Id', '') + 's">{{item.Name}}</option>
								</select>
								<div class="tooltip bottom fade in" *ngIf="(#Language#' + @ClsTableName + 'Form.submitted && #Language#' + @ClsTableName + 'Form.form.controls.in#Language#' + @ColumnName + '?.invalid) || (#Language#' + @ClsTableName + 'Form.form.controls.in#Language#' + @ColumnName + '?.touched && #Language#' + @ClsTableName + 'Form.form.controls.in#Language#' + @ColumnName + '?.invalid)">
									<div class="tooltip-arrow"></div>
									<div class="tooltip-inner ng-binding">' + dbo.fnUserFriendlyName(@ColumnName) + ' is required.</div>
								</div>' ELSE '
								<select class="form-control" [(ngModel)]="' + dbo.fnJavaScriptName(@ClsTableName) + '.' + dbo.fnJavaScriptName(@ColumnName) + '" id="in#Language#' + @ColumnName + '" name="in#Language#' + @ColumnName + '" >
									<option [value]="item.Id" *ngFor="let item of ' + REPLACE(@ColumnName, 'Id', '') + 's">{{item.Name}}</option>
								</select>' END + '
							</div>
						</div>
			'
		END
		ELSE 
		BEGIN
			SET @HtmlSearchModeFields2 = @HtmlSearchModeFields2 + N'
                    <div class="col-lg-4 col-md-4">
						<div class="form-group">
							<label for="in#Language#' + @ColumnName + '">' + dbo.fnUserFriendlyName(@ColumnName) + '</label>
							<div class="input-group-sm">
								<input type="' + @HtmlControlType + '" class="form-control" [(ngModel)]="' + dbo.fnJavaScriptName(@ClsTableName) + 'Parameter.' + dbo.fnJavaScriptName(@ColumnName) + '" id="inSearch#Language#' + @ColumnName + '" name="inSearch#Language#' + @ColumnName + '" placeholder="' + dbo.fnUserFriendlyName(@ColumnName) + '" />
							</div>
						</div>
                    </div>
			'

			SET @HtmlInputModeFields2 = @HtmlInputModeFields2 + N'
						<div class="form-group">
							<label for="in#Language#' + @ColumnName + '">' + dbo.fnUserFriendlyName(@ColumnName) + '</label>
							<div class="input-group-sm">' + CASE @IsNullable WHEN 0 THEN '
								<input type="' + @HtmlControlType + '" class="form-control" [(ngModel)]="' + dbo.fnJavaScriptName(@ClsTableName) + '.' + dbo.fnJavaScriptName(@ColumnName) + '" id="in#Language#' + @ColumnName + '" name="in#Language#' + @ColumnName + '" placeholder="' + dbo.fnUserFriendlyName(@ColumnName) + '" required [ngClass]="{''form-control-danger'': (#Language#' + @ClsTableName + 'Form.submitted && #Language#' + @ClsTableName + 'Form.form.controls.in#Language#' + @ColumnName + '?.invalid) || (#Language#' + @ClsTableName + 'Form.form.controls.in#Language#' + @ColumnName + '?.touched && #Language#' + @ClsTableName + 'Form.form.controls.in#Language#' + @ColumnName + '?.invalid)}" />
								<div class="tooltip bottom fade in" *ngIf="(#Language#' + @ClsTableName + 'Form.submitted && #Language#' + @ClsTableName + 'Form.form.controls.in#Language#' + @ColumnName + '?.invalid) || (#Language#' + @ClsTableName + 'Form.form.controls.in#Language#' + @ColumnName + '?.touched && #Language#' + @ClsTableName + 'Form.form.controls.in#Language#' + @ColumnName + '?.invalid)">
									<div class="tooltip-arrow"></div>
									<div class="tooltip-inner ng-binding">' + dbo.fnUserFriendlyName(@ColumnName) + ' is required.</div>
								</div>' ELSE '
								<input type="' + @HtmlControlType + '" class="form-control" [(ngModel)]="' + dbo.fnJavaScriptName(@ClsTableName) + '.' + dbo.fnJavaScriptName(@ColumnName) + '" id="in#Language#' + @ColumnName + '" name="in#Language#' + @ColumnName + '" placeholder="' + dbo.fnUserFriendlyName(@ColumnName) + '" />' END + '
							</div>
						</div>
			'
		END      
		
		SET @HtmlGridFieldsHeader2 = @HtmlGridFieldsHeader2 + '
					<th (click)="Sort(''' + @ColumnName + ''')" class="sort-header">
                        ' + @ColumnName + '
                        <i class="fa" [ngClass]="{''fa-sort'': (' + dbo.fnJavaScriptName(@ClsTableName) + 'Parameter.sortExpression != ''' + @ColumnName + '''), ''fa-sort-asc'': (' + dbo.fnJavaScriptName(@ClsTableName) + 'Parameter.sortExpression == ''' + @ColumnName + ''' && ' + dbo.fnJavaScriptName(@ClsTableName) + 'Parameter.sortDirection == ''asc''), ''fa-sort-desc'': (' + dbo.fnJavaScriptName(@ClsTableName) + 'Parameter.SortExpression == ''' + @ColumnName + ''' && ' + dbo.fnJavaScriptName(@ClsTableName) + 'Parameter.sortDirection == ''desc'') }" aria-hidden="true"></i>
                    </th>
' 
		SET @HtmlGridFieldsDetail2 = @HtmlGridFieldsDetail2 + '<td>{{item.' + dbo.fnJavaScriptName(@ColumnName) + '}}</td>' + CHAR(13)

		FETCH NEXT FROM @CurColumn INTO @ColumnName, @DataType, @IsNullable, @DataSize
	END;
	CLOSE @CurColumn;
	DEALLOCATE @CurColumn;

	DECLARE @CurAddLovs CURSOR
	SET @CurAddLovs = CURSOR FOR 
		SELECT t.value
		FROM dbo.fnSplit(@AddLOVs, ' ') t

	OPEN @CurAddLovs; 

	FETCH NEXT FROM @CurAddLovs INTO @Lov
	WHILE @@FETCH_STATUS = 0
	BEGIN
		SET @AddLovsEntity = @AddLovsEntity + 'public List<' + @Lov + 'MainEntity> ' + @Lov + 's = new List<' + @Lov + 'MainEntity>();' + CHAR(13)
		
		SET @MapAddEntity = @MapAddEntity + 'case ' + CONVERT(VARCHAR, @AddEditResultSet) + ':' + CHAR(13) + dbo.fnJavaScriptName(@ClsTableName) + 'AddEntity.' + @Lov + 's.Add(await sql.MapDataDynamicallyAsync<' + @Lov + 'MainEntity>(reader));' + CHAR(13) + 'break;' + CHAR(13)

		SET @MapEditEntity = @MapEditEntity + 'case ' + CONVERT(VARCHAR, @AddEditResultSet + 1) + ':' + CHAR(13) + dbo.fnJavaScriptName(@ClsTableName) + 'EditEntity.' + @Lov + 's.Add(await sql.MapDataDynamicallyAsync<' + @Lov + 'MainEntity>(reader));' + CHAR(13) + 'break;' + CHAR(13)
		
		SET @AddLovsSPCall = @AddLovsSPCall + 'EXEC ' + @Lov + '_SelectForLOV ' + @SpSelectForParameterPass + CHAR(13)

		SET @AddLovsModel = @AddLovsModel + 'this.' + dbo.fnJavaScriptName(@Lov) + 's = [];' + CHAR(13)

		SET @AddEditLovDepRoute = @AddEditLovDepRoute + '''Areas/' + @Route + '/' + @Lov + '/' + @Lov + '.Service.js?v='' + version,' + CHAR(13)

		SET @AddLovSetAngular = @AddLovSetAngular + '$scope.' + dbo.fnJavaScriptName(@Lov) + 'ConfigData.data = ' + dbo.fnJavaScriptName(@ClsTableName) + 'AddModel.' + dbo.fnJavaScriptName(@Lov) + 's;' + CHAR(13)
		SET @EditLovSetAngular = @EditLovSetAngular + '$scope.' + dbo.fnJavaScriptName(@Lov) + 'ConfigData.data = ' + dbo.fnJavaScriptName(@ClsTableName) + 'EditModel.' + dbo.fnJavaScriptName(@Lov) + 's;' + CHAR(13)
		SET @AddEditLovDeclareAngular = @AddEditLovDeclareAngular + '$scope.' + dbo.fnJavaScriptName(@Lov) + 'ConfigData = {' + CHAR(13) + 'data: []' + CHAR(13) + '};' + CHAR(13)

		SET @Angular2AddLOVModel = @Angular2AddLOVModel + 'public '+ dbo.fnJavaScriptName(@Lov) + 's: ' + @Lov + 'MainModel[] = [];' + CHAR(13)
		SET @Angular2LOVImportModel = @Angular2LOVImportModel + 'import { ' + @Lov + 'MainModel } from ''../'  + LOWER(@Lov)  + '/' + LOWER(@Lov) + '.model'';' + CHAR(13)
		
		SET @AddEditResultSet = @AddEditResultSet + 1
		FETCH NEXT FROM @CurAddLovs INTO @Lov
	END;
	CLOSE @CurAddLovs;
	DEALLOCATE @CurAddLovs;
	
	DECLARE @CurListLovs CURSOR
	SET @CurListLovs = CURSOR FOR 
		SELECT t.value
		FROM dbo.fnSplit(@ListLOVs, ' ') t

	OPEN @CurListLovs; 

	FETCH NEXT FROM @CurListLovs INTO @Lov
	WHILE @@FETCH_STATUS = 0
	BEGIN
		SET @ListLovsEntity = @ListLovsEntity + 'public List<' + @Lov + 'MainEntity> ' + @Lov + 's = new List<' + @Lov + 'MainEntity>();' + CHAR(13)
		
		SET @MapListEntity = @MapListEntity + 'case ' + CONVERT(VARCHAR, @ListResultSet) + ':' + CHAR(13) + dbo.fnJavaScriptName(@ClsTableName) + 'ListEntity.' + @Lov + 's.Add(await sql.MapDataDynamicallyAsync<' + @Lov + 'MainEntity>(reader));' + CHAR(13) + 'break;' + CHAR(13)
		
		SET @ListLovsSPCall = @ListLovsSPCall + 'EXEC ' + @Lov + '_SelectForLOV ' + @SpSelectForParameterPass + CHAR(13)

		SET @ListLovsModel = @ListLovsModel + 'this.' + dbo.fnJavaScriptName(@Lov) + 's = [];' + CHAR(13)

		SET @ListLovDepRoute = @ListLovDepRoute + '''Areas/' + @Route + '/' + @Lov + '/' + @Lov + '.Service.js?v='' + version,' + CHAR(13)

		SET @ListLovSetAngular = @ListLovSetAngular + '$scope.' + dbo.fnJavaScriptName(@Lov) + 'ConfigData.data = ' + dbo.fnJavaScriptName(@ClsTableName) + 'ListModel.' + dbo.fnJavaScriptName(@Lov) + 's;' + CHAR(13)
		SET @ListLovDeclareAngular = @ListLovDeclareAngular + '$scope.' + dbo.fnJavaScriptName(@Lov) + 'ConfigData = {' + CHAR(13) + 'data: []' + CHAR(13) + '};' + CHAR(13)

		SET @Angular2ListLOVModel = @Angular2ListLOVModel + 'public '+ dbo.fnJavaScriptName(@Lov) + 's: ' + @Lov + 'MainModel[] = [];' + CHAR(13)
		SET @Angular2LOVImportModel = @Angular2LOVImportModel + 'import { ' + @Lov + 'MainModel } from ''../'  + LOWER(@Lov)  + '/' + LOWER(@Lov) + '.model'';' + CHAR(13)

		SET @ListResultSet = @ListResultSet + 1
		FETCH NEXT FROM @CurListLovs INTO @Lov
	END;
	CLOSE @CurListLovs;
	DEALLOCATE @CurListLovs;

	SET @UpdateAddParameter = 'sql.AddParameter("' + @PKFieldName + '", ' + dbo.fnJavaScriptName(@ClsTableName) + 'Entity.' + @PKFieldName + ');' + CHAR(13) + @InsertAddParameter 
	SET @SpUpdateParameter = '@' + @PKFieldName + ' ' + @PKFieldDataType + ',' + CHAR(13) + @SpInsertParameter 

	IF(@DevelopmentDateTime IS NULL)
		SET @DevelopmentDateTime = GETDATE();

	SET @Entity_String = N'
	/// <summary>
	/// This class having main entities of table '+ @TableName + '
	/// Created By :: '+ @DevelopmentBy + '
	/// Created On :: '+ CONVERT(VARCHAR(10), @DevelopmentDateTime, 101) + '
	/// </summary>
	public class '+ @ClsTableName + 'MainEntity
{
	#region Constructor
	/// <summary>
	/// This construction is set properties default value based on its data type in table.
	/// </summary>
	public '+ @ClsTableName + 'MainEntity()
	{
		SetDefaulValue();
	}
	#endregion

	#region Public Properties
	' + @PublicMainProperty + '
	#endregion

	#region Private Methods
	/// <summary>
	/// This function is set properties default value based on its data type in table.
	/// </summary>
	private void SetDefaulValue()
	{
		' + @SetMainDefaulValue + '
	}
	#endregion
}

	/// <summary>
	/// This class having entities of table '+ @TableName + '
	/// Created By :: '+ @DevelopmentBy + '
	/// Created On :: '+ CONVERT(VARCHAR(10), @DevelopmentDateTime, 101) + '
	/// </summary>
	public class '+ @ClsTableName + 'Entity : '+ @ClsTableName + 'MainEntity
{
	#region Constructor
	/// <summary>
	/// This construction is set properties default value based on its data type in table.
	/// </summary>
	public '+ @ClsTableName + 'Entity()
	{
		SetDefaulValue();
	}
	#endregion

	#region Public Properties
	' + @PublicProperty + '
	#endregion

	#region Private Methods
	/// <summary>
	/// This function is set properties default value based on its data type in table.
	/// </summary>
	private void SetDefaulValue()
	{
		' + @SetDefaulValue + '
	}
	#endregion
}

    public class '+ @ClsTableName + 'AddEntity
    {
        ' + @AddLovsEntity + '
    }

    public class '+ @ClsTableName + 'EditEntity : '+ @ClsTableName + 'AddEntity
    {
        public '+ @ClsTableName + 'Entity '+ @ClsTableName + ' = new '+ @ClsTableName + 'Entity();
    }

    public class '+ @ClsTableName + 'GridEntity
    {
        public List<'+ @ClsTableName + 'Entity> '+ @ClsTableName + 's = new List<'+ @ClsTableName + 'Entity>();
        public int TotalRecords { get; set; }
    }

    public class '+ @ClsTableName + 'ListEntity : '+ @ClsTableName + 'GridEntity
    {
        ' + @ListLovsEntity + '
    }

    public class '+ @ClsTableName + 'ParameterEntity : PagingSortingEntity
    {
        #region Constructor
        /// <summary>
        /// This construction is set properties default value based on its data type in table.
        /// </summary>
        public '+ @ClsTableName + 'ParameterEntity()
        {
            SetDefaulValue();
        }
        #endregion

		#region Public Properties
		' + @PublicParameterProperty + '
		#endregion

		#region Private Methods
		/// <summary>
		/// This function is set properties default value based on its data type in table.
		/// </summary>
		private void SetDefaulValue()
		{
			' + @SetParameterDefaulValue + '
		}
		#endregion
    }
		'

	SET @Business_String = N'/// <summary>
	/// This class having crud operation function of table '+ @TableName + '
	/// Created By :: '+ @DevelopmentBy + '
	/// Created On :: '+ CONVERT(VARCHAR(10), @DevelopmentDateTime, 101) + '
	/// </summary>
	public class '+ @ClsTableName + 'Business : AbstractCommonBusiness<'+ @ClsTableName + 'Entity, '+ @ClsTableName + 'MainEntity, '+ @ClsTableName + 'AddEntity, '+ @ClsTableName + 'EditEntity, '+ @ClsTableName + 'GridEntity, '+ @ClsTableName + 'ListEntity, '+ @ClsTableName + 'ParameterEntity, ' + @PKNetFieldDataType + '>
{
        ISql sql;

	#region Constructor
	/// <summary>
	/// This construction is set properties default value based on its data type in table.
	/// </summary>
	public '+ @ClsTableName + 'Business(IConfiguration config) : base(config)
	{
            sql = CreateSqlInstance();
	}
	#endregion

        #region Public Override Methods
    /// <summary>
    /// This function return map reader table field to Entity of ' + @TableName + '.
    /// </summary>
    /// <returns>Entity</returns>
        public override '+ @ClsTableName + 'Entity MapData(IDataReader reader)
        {
            '+ @ClsTableName + 'Entity '+ dbo.fnJavaScriptName(@ClsTableName) + 'Entity = new '+ @ClsTableName + 'Entity();
            for (int i = 0; i < reader.FieldCount; i++)
            {
                switch (reader.GetName(i))
                {
                    ' + @MapDataColumns + '
				}
            }
            return '+ dbo.fnJavaScriptName(@ClsTableName) + 'Entity;
        }

        /// <summary>
        /// This function return all columns values for particular ' + @TableName + ' record
        /// </summary>
        /// <param name="' + @PKFieldName + '">Particular Record</param>
        /// <returns>Entity</returns>
        public override async Task<'+ @ClsTableName + 'Entity> SelectForRecord(' + @PKNetFieldDataType + ' ' + dbo.fnJavaScriptName(@PKFieldName) + ')
        {
            sql.AddParameter("' + @PKFieldName + '", ' + dbo.fnJavaScriptName(@PKFieldName) + ');
            return await sql.ExecuteRecordAsync<'+ @ClsTableName + 'Entity>("' + @TableName + '_SelectForRecord", CommandType.StoredProcedure);
        }

        /// <summary>
        /// This function returns main informations for bind ' + @ClsTableName + ' LOV
        /// </summary>
        /// <param name="' + dbo.fnJavaScriptName(@ClsTableName) + 'Entity">Filter criteria by Entity</param>
        /// <returns>Main Entitys</returns>
		public override async Task<List<' + @ClsTableName + 'MainEntity>> SelectForLOV(' + @ClsTableName + 'ParameterEntity ' + dbo.fnJavaScriptName(@ClsTableName) + 'ParameterEntity)
		{
            ' + @BusinessParameter + '
			return await sql.ExecuteListAsync<' + @ClsTableName + 'MainEntity>("' + @TableName + '_SelectForLOV", CommandType.StoredProcedure);
		}

        /// <summary>
        /// This function return all LOVs data for ' + @ClsTableName + ' page add mode
        /// </summary>
        /// <param name="' + dbo.fnJavaScriptName(@ClsTableName) + 'ParameterEntity">Parameter</param>
        /// <returns>Add modes all LOVs data</returns>
        public override async Task<' + @ClsTableName + 'AddEntity> SelectForAdd(' + @ClsTableName + 'ParameterEntity ' + dbo.fnJavaScriptName(@ClsTableName) + 'ParameterEntity)
        {
            ' + @ClsTableName + 'AddEntity ' + dbo.fnJavaScriptName(@ClsTableName) + 'AddEntity = new ' + @ClsTableName + 'AddEntity();
            ' + @BusinessParameter + '
            await sql.ExecuteEnumerableMultipleAsync<' + @ClsTableName + 'AddEntity>("' + @TableName + '_SelectForAdd", CommandType.StoredProcedure, ' + CONVERT(VARCHAR, @AddLOVsCount) + ', ' + dbo.fnJavaScriptName(@ClsTableName) + 'AddEntity, MapAddEntity);
            return ' + dbo.fnJavaScriptName(@ClsTableName) + 'AddEntity;
        }

        /// <summary>
        /// This function map data for ' + @ClsTableName + ' page add mode LOVs
        /// </summary>
        /// <param name="resultSet">Result Set No</param>
        /// <param name="' + dbo.fnJavaScriptName(@ClsTableName) + 'AddEntity">Add mode Entity for fill data</param>
        /// <param name="reader">Database reader</param>
        public override async Task MapAddEntity(int resultSet, ' + @ClsTableName + 'AddEntity ' + dbo.fnJavaScriptName(@ClsTableName) + 'AddEntity, IDataReader reader)
        {
            switch(resultSet)
            {
                ' + @MapAddEntity  + '
            }		
        }

        /// <summary>
        /// This function return all LOVs data and edit record information for ' + @ClsTableName + ' page edit mode
        /// </summary>
        /// <param name="' + dbo.fnJavaScriptName(@ClsTableName) + 'ParameterEntity">Parameter</param>
        /// <returns>Edit modes all LOVs data and edit record information</returns>
        public override async Task<' + @ClsTableName + 'EditEntity> SelectForEdit(' + @ClsTableName + 'ParameterEntity ' + dbo.fnJavaScriptName(@ClsTableName) + 'ParameterEntity)
        {
            ' + @ClsTableName + 'EditEntity ' + dbo.fnJavaScriptName(@ClsTableName) + 'EditEntity = new ' + @ClsTableName + 'EditEntity();
            ' + @BusinessParameter + '
            await sql.ExecuteEnumerableMultipleAsync<' + @ClsTableName + 'EditEntity>("' + @TableName + '_SelectForEdit", CommandType.StoredProcedure, ' + CONVERT(VARCHAR, @AddLOVsCount + 1) + ', ' + dbo.fnJavaScriptName(@ClsTableName) + 'EditEntity, MapEditEntity);
            return ' + dbo.fnJavaScriptName(@ClsTableName) + 'EditEntity;
        }

        /// <summary>
        /// This function map data for ' + @ClsTableName + ' page edit mode LOVs and edit record information
        /// </summary>
        /// <param name="resultSet">Result Set No</param>
        /// <param name="' + dbo.fnJavaScriptName(@ClsTableName) + 'EditEntity">Edit mode Entity for fill data and edit record information</param>
        /// <param name="reader">Database reader</param>
        public override async Task MapEditEntity(int resultSet, ' + @ClsTableName + 'EditEntity ' + dbo.fnJavaScriptName(@ClsTableName) + 'EditEntity, IDataReader reader)
        {
            switch (resultSet)
            {
                case 0:
                    ' + dbo.fnJavaScriptName(@ClsTableName) + 'EditEntity.' + @ClsTableName + ' = await sql.MapDataDynamicallyAsync<' + @ClsTableName + 'Entity>(reader);
                    break;
                ' + @MapEditEntity  + '
            }
        }

        /// <summary>
        /// This function returns ' + @ClsTableName + ' list page grid data.
        /// </summary>
        /// <param name="' + dbo.fnJavaScriptName(@ClsTableName) + 'ParameterEntity">Filter paramters</param>
        /// <returns>' + @ClsTableName + ' grid data</returns>
        public override async Task<' + @ClsTableName + 'GridEntity> SelectForGrid(' + @ClsTableName + 'ParameterEntity ' + dbo.fnJavaScriptName(@ClsTableName) + 'ParameterEntity)
        {
            ' + @ClsTableName + 'GridEntity ' + dbo.fnJavaScriptName(@ClsTableName) + 'GridEntity = new ' + @ClsTableName + 'GridEntity();
            ' + @BusinessParameter + '
            sql.AddParameter("SortExpression", ' + dbo.fnJavaScriptName(@ClsTableName) + 'ParameterEntity.SortExpression);
            sql.AddParameter("SortDirection", ' + dbo.fnJavaScriptName(@ClsTableName) + 'ParameterEntity.SortDirection);
            sql.AddParameter("PageIndex", ' + dbo.fnJavaScriptName(@ClsTableName) + 'ParameterEntity.PageIndex);
            sql.AddParameter("PageSize", ' + dbo.fnJavaScriptName(@ClsTableName) + 'ParameterEntity.PageSize);
            await sql.ExecuteEnumerableMultipleAsync<' + @ClsTableName + 'GridEntity>("' + @TableName + '_SelectForGrid", CommandType.StoredProcedure, 2, ' + dbo.fnJavaScriptName(@ClsTableName) + 'GridEntity, MapGridEntity);
            return ' + dbo.fnJavaScriptName(@ClsTableName) + 'GridEntity;
        }

        /// <summary>
        /// This function map data for ' + @ClsTableName + ' grid data
        /// </summary>
        /// <param name="resultSet">Result Set No</param>
        /// <param name="' + dbo.fnJavaScriptName(@ClsTableName) + 'GridEntity">' + @ClsTableName + ' grid data</param>
        /// <param name="reader">Database reader</param>
        public override async Task MapGridEntity(int resultSet, ' + @ClsTableName + 'GridEntity ' + dbo.fnJavaScriptName(@ClsTableName) + 'GridEntity, IDataReader reader)
        {
            switch (resultSet)
            {
                case 0:
                    ' + dbo.fnJavaScriptName(@ClsTableName) + 'GridEntity.' + @ClsTableName + 's.Add(await sql.MapDataDynamicallyAsync<' + @ClsTableName + 'Entity>(reader));
                    break;
                case 1:
                    ' + dbo.fnJavaScriptName(@ClsTableName) + 'GridEntity.TotalRecords = MyConvert.ToInt(reader["TotalRecords"]);
                    break;
            }
        }

        /// <summary>
        /// This function returns ' + @ClsTableName + ' list page grid data and LOV data
        /// </summary>
        /// <param name="' + dbo.fnJavaScriptName(@ClsTableName) + 'ParameterEntity">Filter paramters</param>
        /// <returns>' + @ClsTableName + ' grid data and LOV data</returns>
        public override async Task<' + @ClsTableName + 'ListEntity> SelectForList(' + @ClsTableName + 'ParameterEntity ' + dbo.fnJavaScriptName(@ClsTableName) + 'ParameterEntity)
        {
            ' + @ClsTableName + 'ListEntity ' + dbo.fnJavaScriptName(@ClsTableName) + 'ListEntity = new ' + @ClsTableName + 'ListEntity();
            ' + @BusinessParameter + '
            sql.AddParameter("SortExpression", ' + dbo.fnJavaScriptName(@ClsTableName) + 'ParameterEntity.SortExpression);
            sql.AddParameter("SortDirection", ' + dbo.fnJavaScriptName(@ClsTableName) + 'ParameterEntity.SortDirection);
            sql.AddParameter("PageIndex", ' + dbo.fnJavaScriptName(@ClsTableName) + 'ParameterEntity.PageIndex);
            sql.AddParameter("PageSize", ' + dbo.fnJavaScriptName(@ClsTableName) + 'ParameterEntity.PageSize);
            await sql.ExecuteEnumerableMultipleAsync<' + @ClsTableName + 'ListEntity>("' + @TableName + '_SelectForList", CommandType.StoredProcedure, ' + CONVERT(VARCHAR, @ListLOVsCount + 2) + ', ' + dbo.fnJavaScriptName(@ClsTableName) + 'ListEntity, MapListEntity);
            return ' + dbo.fnJavaScriptName(@ClsTableName) + 'ListEntity;
        }

        /// <summary>
        /// This function map data for ' + @ClsTableName + ' list page grid data and LOV data
        /// </summary>
        /// <param name="resultSet">Result Set No</param>
        /// <param name="' + dbo.fnJavaScriptName(@ClsTableName) + 'ListEntity">' + @ClsTableName + ' list page grid data and LOV data</param>
        /// <param name="reader">Database reader</param>
        public override async Task MapListEntity(int resultSet, ' + @ClsTableName + 'ListEntity ' + dbo.fnJavaScriptName(@ClsTableName) + 'ListEntity, IDataReader reader)
        {
            switch (resultSet)
            {
                ' + @MapListEntity + '
                case ' + CONVERT(VARCHAR, @ListLOVsCount) + ':
                    ' + dbo.fnJavaScriptName(@ClsTableName) + 'ListEntity.' + @ClsTableName + 's.Add(await sql.MapDataDynamicallyAsync<' + @ClsTableName + 'Entity>(reader));
                    break;
                case ' + CONVERT(VARCHAR, @ListLOVsCount + 1) + ':
                    ' + dbo.fnJavaScriptName(@ClsTableName) + 'ListEntity.TotalRecords = MyConvert.ToInt(reader["TotalRecords"]);
                    break;
            }
        }

		/// <summary>
		/// This function insert record in ' + @TableName + ' table.
		/// </summary>
		/// <returns>Identity / AlreadyExist = 0</returns>
		public override async Task<' + @PKNetFieldDataType + '> Insert(' + @ClsTableName + 'Entity ' + dbo.fnJavaScriptName(@ClsTableName) + 'Entity)
		{
			' + @InsertAddParameter + 'return MyConvert.' + @PKNetConvert + '(await sql.ExecuteScalarAsync("' + @TableName + '_Insert", CommandType.StoredProcedure));
		}

		/// <summary>
		/// This function update record in ' + @TableName + ' table.
		/// </summary>
		/// <returns>PrimaryKey Field Value / AlreadyExist = 0</returns>
		public override async Task<' + @PKNetFieldDataType + '> Update(' + @ClsTableName + 'Entity ' + dbo.fnJavaScriptName(@ClsTableName) + 'Entity)
		{
			' + @UpdateAddParameter + 'return MyConvert.' + @PKNetConvert + '(await sql.ExecuteScalarAsync("' + @TableName + '_Update", CommandType.StoredProcedure));
		}

		/// <summary>
		/// This function delete record from ' + @TableName + ' table.
		/// </summary>
		public override async Task Delete(' + @PKNetFieldDataType + ' ' + dbo.fnJavaScriptName(@PKFieldName) + ')
		{
			sql.AddParameter("' + @PKFieldName + '", ' + dbo.fnJavaScriptName(@PKFieldName) + ');
			await sql.ExecuteNonQueryAsync("' + @TableName + '_Delete", CommandType.StoredProcedure);
		}
		#endregion
}
			'

	SET @WebAPIControllerString = N'    /// <summary>
    /// This API use for ' + LOWER(@ClsTableName) + ' related operation like list, insert, update, delete ' + LOWER(@TableName) + ' from database etc.
    /// </summary>
	[Route("' + LOWER(@Route) + '/[controller]")]
    [ApiController]
    public class ' + @ClsTableName + 'Controller : ControllerBase, IPageController<' + @ClsTableName + 'Entity, ' + @ClsTableName + 'ParameterEntity, ' + @PKNetFieldDataType + '>
    {
        #region Interface public methods
        /// <summary>
        /// Get all columns information for particular ' + LOWER(dbo.fnUserFriendlyName(@TableName)) + ' record.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("getRecord/{' + dbo.fnJavaScriptName(@PKFieldName) + ':' + @PKNetFieldDataType +'}", Name = "' + LOWER(@Namespace + '.' + @ClsTableName) +'.record")]
        [AuthorizeAPI(pageName : "' + @ClsTableName + '", pageAccess : PageAccessValues.View)]
        public async Task<Response> GetForRecord(' + @PKNetFieldDataType +' ' + dbo.fnJavaScriptName(@PKFieldName) + ')
        {
            Response response;
            try
            {
                ' + @ClsTableName + 'Business ' + dbo.fnJavaScriptName(@ClsTableName) + 'Business = new ' + @ClsTableName + 'Business(Startup.Configuration);
                response = new Response(await ' + dbo.fnJavaScriptName(@ClsTableName) + 'Business.SelectForRecord(' + dbo.fnJavaScriptName(@PKFieldName) + '));
            }
            catch (Exception ex)
            {
				response = new Response(await ex.WriteLogFileAsync(), ex);
            }
            return response;
        }

        /// <summary>
        /// Get main columns informations for bind ' + LOWER(dbo.fnUserFriendlyName(@TableName)) + ' LOV
        /// </summary>
        /// <param name="' + dbo.fnJavaScriptName(@TableName) + 'ParameterEntity"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("getLovValue", Name = "' + LOWER(@Namespace + '.' + @ClsTableName) +'.lovValue")]
        [AuthorizeAPI(pageName : "' + @ClsTableName + '", pageAccess : PageAccessValues.IgnoreAccessValidation)]
        public async Task<Response> GetForLOV(' + @ClsTableName + 'ParameterEntity ' + dbo.fnJavaScriptName(@ClsTableName) + 'ParameterEntity)
        {
            Response response;
            try
            {
                ' + @ClsTableName + 'Business ' + dbo.fnJavaScriptName(@ClsTableName) + 'Business = new ' + @ClsTableName + 'Business(Startup.Configuration);
                response = new Response(await ' + dbo.fnJavaScriptName(@ClsTableName) + 'Business.SelectForLOV(' + dbo.fnJavaScriptName(@ClsTableName) + 'ParameterEntity));
            }
            catch (Exception ex)
            {
				response = new Response(await ex.WriteLogFileAsync(), ex);
            }
            return response;
        }

        /// <summary>
        /// Get ' + LOWER(dbo.fnUserFriendlyName(@TableName)) + '''s page all LOV data when ' + LOWER(dbo.fnUserFriendlyName(@TableName)) + ' page open in add mode.
        /// </summary>
        /// <param name="' + dbo.fnJavaScriptName(@TableName) + 'ParameterEntity"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("getAddMode", Name = "' + LOWER(@Namespace + '.' + @ClsTableName) +'.addMode")]
        [AuthorizeAPI(pageName : "' + @ClsTableName + '", pageAccess : PageAccessValues.Insert)]
        public async Task<Response> GetForAdd(' + @ClsTableName + 'ParameterEntity ' + dbo.fnJavaScriptName(@ClsTableName) + 'ParameterEntity)
        {
            Response response;
            try
            {
                ' + @ClsTableName + 'Business ' + dbo.fnJavaScriptName(@ClsTableName) + 'Business = new ' + @ClsTableName + 'Business(Startup.Configuration);
                response = new Response(await ' + dbo.fnJavaScriptName(@ClsTableName) + 'Business.SelectForAdd(' + dbo.fnJavaScriptName(@ClsTableName) + 'ParameterEntity));
            }
            catch (Exception ex)
            {
				response = new Response(await ex.WriteLogFileAsync(), ex);
            }
            return response;
        }

        /// <summary>
        /// Get ' + LOWER(dbo.fnUserFriendlyName(@TableName)) + '''s page all LOV data and all columns information for edit record when ' + LOWER(dbo.fnUserFriendlyName(@TableName)) + ' page open in edit mode.
        /// </summary>
        /// <param name="' + dbo.fnJavaScriptName(@TableName) + 'ParameterEntity"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("getEditMode", Name = "' + LOWER(@Namespace + '.' + @ClsTableName) +'.editMode")]
        [AuthorizeAPI(pageName : "' + @ClsTableName + '", pageAccess : PageAccessValues.Update)]
        public async Task<Response> GetForEdit(' + @ClsTableName + 'ParameterEntity ' + dbo.fnJavaScriptName(@ClsTableName) + 'ParameterEntity)
        {
            Response response;
            try
            {
                ' + @ClsTableName + 'Business ' + dbo.fnJavaScriptName(@ClsTableName) + 'Business = new ' + @ClsTableName + 'Business(Startup.Configuration);
                response = new Response(await ' + dbo.fnJavaScriptName(@ClsTableName) + 'Business.SelectForEdit(' + dbo.fnJavaScriptName(@ClsTableName) + 'ParameterEntity));
            }
            catch (Exception ex)
            {
				response = new Response(await ex.WriteLogFileAsync(), ex);
            }
            return response;
        }

        /// <summary>
        /// Get ' + LOWER(dbo.fnUserFriendlyName(@TableName)) + ' list for bind grid.
        /// </summary>
        /// <param name="' + dbo.fnJavaScriptName(@TableName) + 'ParameterEntity"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("getGridData", Name = "' + LOWER(@Namespace + '.' + @ClsTableName) +'.gridData")]
        [AuthorizeAPI(pageName : "' + @ClsTableName + '", pageAccess : PageAccessValues.View)]
        public async Task<Response> GetForGrid(' + @ClsTableName + 'ParameterEntity ' + dbo.fnJavaScriptName(@ClsTableName) + 'ParameterEntity)
        {
            Response response;
            try
            {
                ' + @ClsTableName + 'Business ' + dbo.fnJavaScriptName(@ClsTableName) + 'Business = new ' + @ClsTableName + 'Business(Startup.Configuration);
                response = new Response(await ' + dbo.fnJavaScriptName(@ClsTableName) + 'Business.SelectForGrid(' + dbo.fnJavaScriptName(@ClsTableName) + 'ParameterEntity));
            }
            catch (Exception ex)
            {
				response = new Response(await ex.WriteLogFileAsync(), ex);
            }
            return response;
        }

        /// <summary>
        /// Get ' + LOWER(dbo.fnUserFriendlyName(@TableName)) + '''s page all LOV data and grid result when ' + LOWER(dbo.fnUserFriendlyName(@TableName)) + ' page open in list mode.
        /// </summary>
        /// <param name="' + dbo.fnJavaScriptName(@TableName) + 'ParameterEntity"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("getListMode", Name = "' + LOWER(@Namespace + '.' + @ClsTableName) +'.listMode")]
        [AuthorizeAPI(pageName : "' + @ClsTableName + '", pageAccess : PageAccessValues.View)]
        public async Task<Response> GetForList(' + @ClsTableName + 'ParameterEntity ' + dbo.fnJavaScriptName(@ClsTableName) + 'ParameterEntity)
        {
            Response response;
            try
            {
                ' + @ClsTableName + 'Business ' + dbo.fnJavaScriptName(@ClsTableName) + 'Business = new ' + @ClsTableName + 'Business(Startup.Configuration);
                response = new Response(await ' + dbo.fnJavaScriptName(@ClsTableName) + 'Business.SelectForList(' + dbo.fnJavaScriptName(@ClsTableName) + 'ParameterEntity));
            }
            catch (Exception ex)
            {
				response = new Response(await ex.WriteLogFileAsync(), ex);
            }
            return response;
        }

        /// <summary>
        /// Insert record in ' + LOWER(dbo.fnUserFriendlyName(@TableName)) + ' table.
        /// </summary>
        /// <param name="' + dbo.fnJavaScriptName(@TableName) + 'Entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("insert", Name = "' + LOWER(@Namespace + '.' + @ClsTableName) +'.insert")]
        [AuthorizeAPI(pageName : "' + @ClsTableName + '", pageAccess : PageAccessValues.Insert)]
        public async Task<Response> Insert(' + @ClsTableName + 'Entity ' + dbo.fnJavaScriptName(@ClsTableName) + 'Entity)
        {
            Response response;
            try
            {
                ' + @ClsTableName + 'Business ' + dbo.fnJavaScriptName(@ClsTableName) + 'Business = new ' + @ClsTableName + 'Business(Startup.Configuration);
                response = new Response(await ' + dbo.fnJavaScriptName(@ClsTableName) + 'Business.Insert(' + dbo.fnJavaScriptName(@ClsTableName) + 'Entity));
            }
            catch (Exception ex)
            {
				response = new Response(await ex.WriteLogFileAsync(), ex);
            }
            return response;
        }

        /// <summary>
        /// Update record in ' + LOWER(dbo.fnUserFriendlyName(@TableName)) + ' table.
        /// </summary>
        /// <param name="' + dbo.fnJavaScriptName(@TableName) + 'Entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("update", Name = "' + LOWER(@Namespace + '.' + @ClsTableName) +'.update")]
        [AuthorizeAPI(pageName : "' + @ClsTableName + '", pageAccess : PageAccessValues.Update)]
        public async Task<Response> Update(' + @ClsTableName + 'Entity ' + dbo.fnJavaScriptName(@ClsTableName) + 'Entity)
        {
            Response response;
            try
            {
                ' + @ClsTableName + 'Business ' + dbo.fnJavaScriptName(@ClsTableName) + 'Business = new ' + @ClsTableName + 'Business(Startup.Configuration);
                response = new Response(await ' + dbo.fnJavaScriptName(@ClsTableName) + 'Business.Update(' + dbo.fnJavaScriptName(@ClsTableName) + 'Entity));
            }
            catch (Exception ex)
            {
				response = new Response(await ex.WriteLogFileAsync(), ex);
            }
            return response;
        }

        /// <summary>
        /// Delete record from ' + LOWER(dbo.fnUserFriendlyName(@TableName)) + ' table.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("delete/{' + dbo.fnJavaScriptName(@PKFieldName) + ':' + @PKNetFieldDataType +'}", Name = "' + LOWER(@Namespace + '.' + @ClsTableName) +'.delete")]
        [AuthorizeAPI(pageName : "' + @ClsTableName + '", pageAccess : PageAccessValues.Delete)]
        public async Task<Response> Delete(' + @PKNetFieldDataType +' ' + dbo.fnJavaScriptName(@PKFieldName) + ')
        {
            Response response;
            try
            {
                ' + @ClsTableName + 'Business ' + dbo.fnJavaScriptName(@ClsTableName) + 'Business = new ' + @ClsTableName + 'Business(Startup.Configuration);
                await ' + dbo.fnJavaScriptName(@ClsTableName) + 'Business.Delete(' + dbo.fnJavaScriptName(@PKFieldName) + ');
                response = new Response();
            }
            catch (Exception ex)
            {
				response = new Response(await ex.WriteLogFileAsync(), ex);
            }
            return response;
        }
        #endregion
	}
'

	SET @SpInsertString = N'	
CREATE PROCEDURE [dbo].[' + @TableName + '_Insert]
	' + @SpInsertParameter + '
AS
/***********************************************************************************************
	 NAME     :  ' + @TableName + '_Insert
	 PURPOSE  :  This SP insert record in table '+ @TableName + '.
	 REVISIONS:
	 Ver        Date				        Author              Description
	 ---------  ----------					---------------		-------------------------------
	 1.0        '+ CONVERT(VARCHAR(10), @DevelopmentDateTime, 101) + '					'+ @DevelopmentBy + '             1. Initial Version.	 
************************************************************************************************/
BEGIN' + CASE @IsTransactionApply WHEN 1 THEN '
	BEGIN TRANSACTION ' + @TableName + 'Insert
	BEGIN TRY ' ELSE '' END + '
		DECLARE @' + @PKFieldName + ' ' + @PKFieldDataType + '
		IF NOT EXISTS(' + @AlreadyExistInsertSting + ')
		BEGIN
			INSERT INTO [' + @TableName + '] (' + @SpInsertField + ') 
			VALUES (' + @SpInsertValue + ')
			SET @' + @PKFieldName + ' = SCOPE_IDENTITY();
		END
		ELSE
			SET @' + @PKFieldName + ' = 0;
	
		SELECT @' + @PKFieldName + ';'  + CASE @IsTransactionApply WHEN 1 THEN '
		IF @@TRANCOUNT > 0
			COMMIT TRANSACTION ' + @TableName + 'Insert
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
			ROLLBACK TRANSACTION ' + @TableName + 'Insert
		
		DECLARE @ErrorMessage NVARCHAR(4000), @ErrorSeverity INT, @ErrorState INT, @ErrorNumber INT
		SELECT @ErrorMessage = ERROR_MESSAGE(), @ErrorSeverity = ERROR_SEVERITY(), @ErrorState = ERROR_STATE(), @ErrorNumber = ERROR_NUMBER()
		RAISERROR  (@ErrorMessage,@ErrorSeverity,@ErrorState,@ErrorNumber)
	END CATCH' ELSE '' END + '
END	'
	
	SET @SpUpdateString = N'
CREATE PROCEDURE [dbo].[' + @TableName + '_Update]
	' + @SpUpdateParameter + '
AS
/***********************************************************************************************
	 NAME     :  ' + @TableName + '_Update
	 PURPOSE  :  This SP update record in table '+ @TableName + '.
	 REVISIONS:
	 Ver        Date				        Author              Description
	 ---------  ----------					---------------		-------------------------------
	 1.0        '+ CONVERT(VARCHAR(10), @DevelopmentDateTime, 101) + '					'+ @DevelopmentBy + '             1. Initial Version.	 
************************************************************************************************/
BEGIN' + CASE @IsTransactionApply WHEN 1 THEN '
	BEGIN TRANSACTION ' + @TableName + 'Update
	BEGIN TRY ' ELSE '' END + '
		IF NOT EXISTS(' + @AlreadyExistUpdateSting + ')
		BEGIN
			UPDATE [' + @TableName + '] 
			SET ' + @SpUpdateSet + '
			WHERE [' + @PKFieldName + '] = @' + @PKFieldName + ';
		END
		ELSE
			SET @' + @PKFieldName + ' = 0;
	
		SELECT @' + @PKFieldName + ';'  + CASE @IsTransactionApply WHEN 1 THEN '
		IF @@TRANCOUNT > 0
			COMMIT TRANSACTION ' + @TableName + 'Update
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
			ROLLBACK TRANSACTION ' + @TableName + 'Update
		
		DECLARE @ErrorMessage NVARCHAR(4000), @ErrorSeverity INT, @ErrorState INT, @ErrorNumber INT
		SELECT @ErrorMessage = ERROR_MESSAGE(), @ErrorSeverity = ERROR_SEVERITY(), @ErrorState = ERROR_STATE(), @ErrorNumber = ERROR_NUMBER()
		RAISERROR  (@ErrorMessage,@ErrorSeverity,@ErrorState,@ErrorNumber)
	END CATCH' ELSE '' END + '
END	'
	
	SET @SpDeleteString = N'
CREATE PROCEDURE [dbo].[' + @TableName + '_Delete]
	@' + @PKFieldName + ' ' + @PKFieldDataType + '
AS
/***********************************************************************************************
	 NAME     :  ' + @TableName + '_Delete
	 PURPOSE  :  This SP delete record from table '+ @TableName + '
	 REVISIONS:
	 Ver        Date				        Author              Description
	 ---------  ----------					---------------		-------------------------------
	 1.0        '+ CONVERT(VARCHAR(10), @DevelopmentDateTime, 101) + '					'+ @DevelopmentBy + '             1. Initial Version.	 
************************************************************************************************/
BEGIN' + CASE @IsTransactionApply WHEN 1 THEN '
	BEGIN TRANSACTION ' + @TableName + 'Delete
	BEGIN TRY ' ELSE '' END + '
		DELETE FROM [' + @TableName + '] 
		WHERE [' + @PKFieldName + '] = @' + @PKFieldName + ';'  + CASE @IsTransactionApply WHEN 1 THEN '
		IF @@TRANCOUNT > 0
			COMMIT TRANSACTION ' + @TableName + 'Delete
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
			ROLLBACK TRANSACTION ' + @TableName + 'Delete
		
		DECLARE @ErrorMessage NVARCHAR(4000), @ErrorSeverity INT, @ErrorState INT, @ErrorNumber INT
		SELECT @ErrorMessage = ERROR_MESSAGE(), @ErrorSeverity = ERROR_SEVERITY(), @ErrorState = ERROR_STATE(), @ErrorNumber = ERROR_NUMBER()
		RAISERROR  (@ErrorMessage,@ErrorSeverity,@ErrorState,@ErrorNumber)
	END CATCH' ELSE '' END + '
END	'

	SET @SpSelectString = N'
CREATE PROCEDURE [dbo].[' + @TableName + '_Select]
	' + @SpSelectParameter + ' 
AS
/***********************************************************************************************
	 NAME     :  ' + @TableName + '_Select
	 PURPOSE  :  This SP select records from table '+ @TableName + '
	 REVISIONS:
	 Ver        Date				        Author              Description
	 ---------  ----------					---------------		-------------------------------
	 1.0        '+ CONVERT(VARCHAR(10), @DevelopmentDateTime, 101) + '					'+ @DevelopmentBy + '             1. Initial Version.	 
************************************************************************************************/
BEGIN
	SELECT ' + @SpSelectField + ' 
	FROM [' + @TableName + '] ' + @TableShortName + '
	WHERE ' + @SpSelectWhere + ' 
END	'

	SET @SpSelectForAddString = N'
CREATE PROCEDURE [dbo].['+ @TableName + '_SelectForAdd]
	' + @SpSelectForParameter + '
AS
/***********************************************************************************************
	 NAME     :  ' + @TableName + '_SelectForAdd
	 PURPOSE  :  This SP use for fill all LOV in '+ @TableName + ' page for add mode
	 REVISIONS:
	 Ver        Date				        Author              Description
	 ---------  ----------					---------------		-------------------------------
	 1.0        '+ CONVERT(VARCHAR(10), @DevelopmentDateTime, 101) + '					'+ @DevelopmentBy + '             1. Initial Version.	 
************************************************************************************************/
BEGIN
	' + @AddLovsSPCall + '
END		'

	SET @SpSelectForEditString = N'
CREATE PROCEDURE [dbo].['+ @TableName + '_SelectForEdit]
	' + @SpSelectForParameter + '
AS
/***********************************************************************************************
	 NAME     :  ' + @TableName + '_SelectForEdit
	 PURPOSE  :  This SP use for fill all LOV in '+ @TableName + ' page for edit mode
	 REVISIONS:
	 Ver        Date				        Author              Description
	 ---------  ----------					---------------		-------------------------------
	 1.0        '+ CONVERT(VARCHAR(10), @DevelopmentDateTime, 101) + '					'+ @DevelopmentBy + '             1. Initial Version.	 
************************************************************************************************/
BEGIN
	EXEC '+ @TableName + '_SelectForRecord @' + @PKFieldName + '

	EXEC '+ @TableName + '_SelectForAdd ' + @SpSelectForParameterPass + '
END	 '

	SET @SpSelectForGridString = N'
CREATE PROCEDURE [dbo].['+ @TableName + '_SelectForGrid]
	' + @SpSelectForParameter + ', 
	@SortExpression varchar(50),
	@SortDirection varchar(5),
	@PageIndex int,
	@PageSize int
AS
/***********************************************************************************************
	 NAME     :  ' + @TableName + '_SelectForGrid
	 PURPOSE  :  This SP select records from table '+ @TableName + ' for bind ' + @TableName + ' page grid.
	 REVISIONS:
	 Ver        Date				        Author              Description
	 ---------  ----------					---------------		-------------------------------
	 1.0        '+ CONVERT(VARCHAR(10), @DevelopmentDateTime, 101) + '					'+ @DevelopmentBy + '             1. Initial Version.	 
************************************************************************************************/
BEGIN
	DECLARE @SqlQuery NVARCHAR(MAX)

	SET @SqlQuery = N''
	SELECT ' + @SpSelectField + '
	FROM ['+ @TableName + '] ' + @TableShortName + '
	WHERE  ' + @SpSelectForParameterWhere + '
	ORDER BY '' + @SortExpression + '' '' + @SortDirection + ''
	OFFSET ((@PageIndex - 1) * @PageSize) ROWS
	FETCH NEXT @PageSize ROWS ONLY
	''

	EXEC sp_executesql @SqlQuery, N''' + @SpSelectForParameter + ', @PageIndex int, @PageSize int'', ' + @SpSelectForParameterPass + ', @PageIndex, @PageSize
	
	SELECT COUNT(1) AS TotalRecords
	FROM ['+ @TableName + '] ' + @TableShortName + '
	WHERE  ' + @SpSelectForParameterWhere + '

END		
	'

	SET @SpSelectForListString = N'
CREATE PROCEDURE [dbo].['+ @TableName + '_SelectForList]
	' + @SpSelectForParameter + ',
	@SortExpression varchar(50),
	@SortDirection varchar(5),
	@PageIndex int,
	@PageSize int
AS
/***********************************************************************************************
	 NAME     :  ' + @TableName + '_SelectForList
	 PURPOSE  :  This SP use for fill all LOV and list grid in '+ @TableName + ' list page
	 REVISIONS:
	 Ver        Date				        Author              Description
	 ---------  ----------					---------------		-------------------------------
	 1.0        '+ CONVERT(VARCHAR(10), @DevelopmentDateTime, 101) + '					'+ @DevelopmentBy + '             1. Initial Version.	 
************************************************************************************************/
BEGIN
	' + @ListLovsSPCall + '

	EXEC '+ @TableName + '_SelectForGrid ' + @SpSelectForParameterPass + ', @SortExpression, @SortDirection, @PageIndex, @PageSize
END	
	'

	SET @SpSelectForLOVString = N'
CREATE PROCEDURE [dbo].['+ @TableName + '_SelectForLOV]
	' + @SpSelectForParameter + '
AS
/***********************************************************************************************
	 NAME     :  ' + @TableName + '_SelectForLOV
	 PURPOSE  :  This SP select records from table '+ @TableName + ' for fill LOV
	 REVISIONS:
	 Ver        Date				        Author              Description
	 ---------  ----------					---------------		-------------------------------
	 1.0        '+ CONVERT(VARCHAR(10), @DevelopmentDateTime, 101) + '					'+ @DevelopmentBy + '             1. Initial Version.	 
************************************************************************************************/
BEGIN
	SELECT ' + @SpSelectMainFields + '
	FROM [' + @TableName + '] ' + @TableShortName + '
	WHERE ' + @SpSelectForParameterWhere + '
END		
	'

	SET @SpSelectForRecordString = N'
CREATE PROCEDURE [dbo].['+ @TableName + '_SelectForRecord]
	@' + @PKFieldName + ' ' + @PKFieldDataType + '
AS
/***********************************************************************************************
	 NAME     :  ' + @TableName + '_SelectForRecord
	 PURPOSE  :  This SP select perticular record from table '+ @TableName + ' for open page in edit / view mode.
	 REVISIONS:
	 Ver        Date				        Author              Description
	 ---------  ----------					---------------		-------------------------------
	 1.0        '+ CONVERT(VARCHAR(10), @DevelopmentDateTime, 101) + '					'+ @DevelopmentBy + '             1. Initial Version.	 
************************************************************************************************/
BEGIN
	SELECT ' + @SpSelectField + '
	FROM ['+ @TableName + '] ' + @TableShortName + '
	WHERE ' + @TableShortName + '.[' + @PKFieldName + '] = @' + @PKFieldName + ';
END	
	'

	SET @AngularModelString = N'function ' + @ClsTableName + 'MainModel() {
'+ @ModelMainDefination +' }

function ' + @ClsTableName + 'Model() {
'+ @ModelDefination +' }

function ' + @ClsTableName + 'AddModel() {
    ' + @AddLovsModel + '
}

function ' + @ClsTableName + 'EditModel() {
    this.' + dbo.fnJavaScriptName(@ClsTableName) + ' = new ' + @ClsTableName + 'Model();
    ' + @AddLovsModel + '
}

function ' + @ClsTableName + 'GridModel() {
    this.totalRecords = 0;
    this.' + dbo.fnJavaScriptName(@ClsTableName) + 's = [];
}

function ' + @ClsTableName + 'ListModel() {
    this.totalRecords = 0;
    this.' + dbo.fnJavaScriptName(@ClsTableName) + 's = [];
    ' + @ListLovsModel + '
}

function ' + @ClsTableName + 'ParameterModel() {
    ' + @ModelParameter + ' 

    this.sortExpression = '''';
    this.sortDirection = '''';
    this.pageIndex = 1;
    this.pageSize = 10;
    this.totalRecords = 100;
}
'

	SET @AngularServiceString = N'angular.module(''app'').service(''' + dbo.fnJavaScriptName(@ClsTableName) + 'Service'', [''baseService'', function (baseService) {
    //#region main service
    this.getRecord = function (' + dbo.fnJavaScriptName(@PKFieldName) + ') {
        return baseService.httpGet(''' + LOWER(@Route + '/' + @ClsTableName) + '/getRecord?' + dbo.fnJavaScriptName(@PKFieldName) + '='' + ' + dbo.fnJavaScriptName(@PKFieldName) + ');
    }

    this.getLovValue = function (' + dbo.fnJavaScriptName(@ClsTableName) + 'Entity) {
        return baseService.httpPost(''' + LOWER(@Route + '/' + @ClsTableName) + '/getLovValue'', ' + dbo.fnJavaScriptName(@ClsTableName) + 'Entity);
    }

    this.getAddMode = function (' + dbo.fnJavaScriptName(@ClsTableName) + 'ParameterEntity) {
        return baseService.httpPost(''' + LOWER(@Route + '/' + @ClsTableName) + '/getAddMode'', ' + dbo.fnJavaScriptName(@ClsTableName) + 'ParameterEntity);
    }

    this.getEditMode = function (' + dbo.fnJavaScriptName(@ClsTableName) + 'ParameterEntity) {
        return baseService.httpPost(''' + LOWER(@Route + '/' + @ClsTableName) + '/getEditMode'', ' + dbo.fnJavaScriptName(@ClsTableName) + 'ParameterEntity);
    }

    this.getGridData = function (' + dbo.fnJavaScriptName(@ClsTableName) + 'ParameterEntity) {
        return baseService.httpPost(''' + LOWER(@Route + '/' + @ClsTableName) + '/getGridData'', ' + dbo.fnJavaScriptName(@ClsTableName) + 'ParameterEntity);
    }

    this.getListMode = function (' + dbo.fnJavaScriptName(@ClsTableName) + 'ParameterEntity) {
        return baseService.httpPost(''' + LOWER(@Route + '/' + @ClsTableName) + '/getListMode'', ' + dbo.fnJavaScriptName(@ClsTableName) + 'ParameterEntity);
    }

    this.save = function (' + @ClsTableName + 'Entity) {
        if (' + dbo.fnJavaScriptName(@ClsTableName) + 'Entity.' + @PKFieldName + ' > 0)
            return baseService.httpPost(''' + LOWER(@Route + '/' + @ClsTableName) + '/update'', ' + dbo.fnJavaScriptName(@ClsTableName) + 'Entity);
        else
            return baseService.httpPost(''' + LOWER(@Route + '/' + @ClsTableName) + '/insert'', ' + dbo.fnJavaScriptName(@ClsTableName) + 'Entity);
    }

    this.delete = function (' + dbo.fnJavaScriptName(@PKFieldName) + ') {
        return baseService.httpPost(''' + LOWER(@Route + '/' + @ClsTableName) + '/delete?' + dbo.fnJavaScriptName(@PKFieldName) + '='' + ' + dbo.fnJavaScriptName(@PKFieldName) + ');
    }
    //#endregion
}]);'

	SET @AngularRouteString = N'angular.module(''app'').config([''$stateProvider'', ''$urlRouterProvider'', ''$ocLazyLoadProvider'', function ($stateProvider, $urlRouterProvider, $ocLazyLoadProvider) {
    $stateProvider
          .state(''' + LOWER(@Namespace + '.' + @ClsTableName) + '.list'', {
              url: ''/list'',
              views: {
                  '''': {
                      templateUrl: ''Areas/' + @Route + '/' + @ClsTableName + '/' + @ClsTableName + 'List.html?v='' + version,
                      controller: ''' + dbo.fnJavaScriptName(@ClsTableName) + 'ListController'',
                      resolve: {
                          deps: [''$ocLazyLoad'', function ($ocLazyLoad) {
                              return $ocLazyLoad.load(
                                {
                                    files: [
                                    ' + @ListLovDepRoute + '''Areas/' + @Route + '/' + @ClsTableName + '/' + @ClsTableName + '.Model.js?v='' + version,
                                    ''Areas/' + @Route + '/' + @ClsTableName + '/' + @ClsTableName + 'List.Controller.js?v='' + version,
                                    ''Areas/' + @Route + '/' + @ClsTableName + '/' + @ClsTableName + '.Service.js?v='' + version
                                    ]
                                }
                              );
                          }]
                      }
                  }
              }
          })
        .state(''' + LOWER(@Namespace + '.' + @ClsTableName) + '.add'', {
            url: ''/add'',
            views: {
                '''': {
                    templateUrl: ''Areas/' + @Route + '/' + @ClsTableName + '/' + @ClsTableName + 'AddEdit.html?v='' + version,
                    controller: ''' + dbo.fnJavaScriptName(@ClsTableName) + 'AddEditController'',
                    resolve: {
                        deps: [''$ocLazyLoad'', function ($ocLazyLoad) {
                            return $ocLazyLoad.load({
                                files: [
                                    ' + @AddEditLovDepRoute + '''Areas/' + @Route + '/' + @ClsTableName + '/' + @ClsTableName + '.Model.js?v='' + version,
                                    ''Areas/' + @Route + '/' + @ClsTableName + '/' + @ClsTableName + 'AddEdit.Controller.js?v='' + version,
                                    ''Areas/' + @Route + '/' + @ClsTableName + '/' + @ClsTableName + '.Service.js?v='' + version
                                ]
                            });
                        }]
                    }
                }
            }
        })
        .state(''' + LOWER(@Namespace + '.' + @ClsTableName) + '.edit'', {
            url: ''/edit/:id'',
            params: { ''id'': null },
            views: {
                '''': {
                    templateUrl: ''Areas/' + @Route + '/' + @ClsTableName + '/' + @ClsTableName + 'AddEdit.html?v='' + version,
                    controller: ''' + dbo.fnJavaScriptName(@ClsTableName) + 'AddEditController'',
                    resolve: {
                        deps: [''$ocLazyLoad'', function ($ocLazyLoad) {
                            return $ocLazyLoad.load({
                                files: [
                                    ' + @AddEditLovDepRoute + '''Areas/' + @Route + '/' + @ClsTableName + '/' + @ClsTableName + '.Model.js?v='' + version,
                                    ''Areas/' + @Route + '/' + @ClsTableName + '/' + @ClsTableName + 'AddEdit.Controller.js?v='' + version,
                                    ''Areas/' + @Route + '/' + @ClsTableName + '/' + @ClsTableName + '.Service.js?v='' + version
                                ]
                            });
                        }]
                    }
                }
            }
        })
}]);
'
	
	SET @AngularAddEditControllerString = N'/// <reference path="../../../../Scripts/_references.js" />
angular.module(''app'').controller(''' + dbo.fnJavaScriptName(@ClsTableName) + 'AddEditController'', [''$scope'', ''$rootScope'', ''$state'', ''' + dbo.fnJavaScriptName(@ClsTableName) + 'Service'', ''Session'', function ($scope, $rootScope, $state, ' + dbo.fnJavaScriptName(@ClsTableName) + 'Service, Session) {
    //#region submit or cancel record
    $scope.submitRecord = function () {
        if ($scope.' + dbo.fnJavaScriptName(@ClsTableName) + 'Form.$invalid) {
            myToastr.warning(''Invalid input'');
            return;
        }
        if ($scope.accessModel.canInsert || $scope.accessModel.canUpdate) {
            ' + dbo.fnJavaScriptName(@ClsTableName) + 'Service.save($scope.' + dbo.fnJavaScriptName(@ClsTableName) + ').then(function (response) {
                if (response == 0) {
                    myToastr.info("' + dbo.fnUserFriendlyName(@ClsTableName) + ' already exist.");
                    return;
                }
                else {
                    myToastr.success("' + dbo.fnUserFriendlyName(@ClsTableName) + ' submit successfully.");
                }
                $scope.cancelRecord();
            });
        }
    };

    $scope.cancelRecord = function () {
        $state.go(''' + LOWER(@Namespace + '.' + @ClsTableName) + '.list'');
    };
    //#endregion

    //#region LOV
    //#endregion

    //#region set page UI
    $scope.setPageAddEditMode = function () {
        if ($state.current.name == ''' + LOWER(@Namespace + '.' + @ClsTableName) + '.add'')
            $scope.setPageAddMode();
        else if ($state.current.name == ''' + LOWER(@Namespace + '.' + @ClsTableName) + '.edit'')
            $scope.setPageEditMode();
    }

    $scope.setPageAddMode = function () {
        $scope.mode = ''Add'';

        var ' + dbo.fnJavaScriptName(@ClsTableName) + 'ParameterModel = new ' + @ClsTableName + 'ParameterModel();
        ' + dbo.fnJavaScriptName(@ClsTableName) + 'ParameterModel.orgId = $rootScope.user.orgId;
        ' + dbo.fnJavaScriptName(@ClsTableName) + 'ParameterModel.userId = $rootScope.user.userId;

        ' + dbo.fnJavaScriptName(@ClsTableName) + 'Service.getAddMode(' + dbo.fnJavaScriptName(@ClsTableName) + 'ParameterModel).then(function (response) {
            var ' + dbo.fnJavaScriptName(@ClsTableName) + 'AddModel = new ' + @ClsTableName + 'AddModel();
            ' + dbo.fnJavaScriptName(@ClsTableName) + 'AddModel = response;

            $scope.' + dbo.fnJavaScriptName(@ClsTableName) + ' = new ' + @ClsTableName + 'Model();
            ' + @AddLovSetAngular + '
        });
    }

    $scope.setPageEditMode = function () {
        $scope.mode = ''Edit'';

        var ' + dbo.fnJavaScriptName(@ClsTableName) + 'ParameterModel = new ' + @ClsTableName + 'ParameterModel();
        ' + dbo.fnJavaScriptName(@ClsTableName) + 'ParameterModel.orgId = $rootScope.user.orgId;
        ' + dbo.fnJavaScriptName(@ClsTableName) + 'ParameterModel.userId = $rootScope.user.userId;
        ' + dbo.fnJavaScriptName(@ClsTableName) + 'ParameterModel.' + @PKFieldName + ' = $state.params.id;

        ' + dbo.fnJavaScriptName(@ClsTableName) + 'Service.getEditMode(' + dbo.fnJavaScriptName(@ClsTableName) + 'ParameterModel).then(function (response) {
            var ' + dbo.fnJavaScriptName(@ClsTableName) + 'EditModel = new ' + @ClsTableName + 'EditModel();
            ' + dbo.fnJavaScriptName(@ClsTableName) + 'EditModel = response;

            $scope.' + dbo.fnJavaScriptName(@ClsTableName) + ' = ' + dbo.fnJavaScriptName(@ClsTableName) + 'EditModel.' + @ClsTableName + ';
            ' + @EditLovSetAngular + '
        });
    }

    $scope.setLOVConfiguration = function () {
        ' + @AddEditLovDeclareAngular + '
    }

    $scope.initializePage = function () {
        $scope.accessModel = Session.getAccess();

        $scope.setLOVConfiguration();

        $scope.setPageAddEditMode();
    }
    $scope.initializePage();
    //#endregion
}]);
'

	SET @AngularListControllerString = N'/// <reference path="../../../../Scripts/_references.js" />
angular.module(''app'').controller(''' + dbo.fnJavaScriptName(@ClsTableName) + 'ListController'', [''$scope'', ''$state'', ''' + dbo.fnJavaScriptName(@ClsTableName) + 'Service'', ''Session'', function ($scope, $state, ' + dbo.fnJavaScriptName(@ClsTableName) + 'Service, Session) {
    //#region operation
    $scope.addNewRecord = function () {
        if ($scope.accessModel.canInsert) {
            $state.go(''' + LOWER(@Namespace + '.' + @ClsTableName) + '.add'');
        }
    };

    $scope.editRecord = function (id) {
        if ($scope.accessModel.canUpdate) {
            $state.go(''' + LOWER(@Namespace + '.' + @ClsTableName) + '.edit'', { ''id'': id });
        }
    };

    $scope.deleteRecord = function (id) {
        if ($scope.accessModel.canDelete) {
            if (confirm("Do you want to delete reecord?")) {
                ' + dbo.fnJavaScriptName(@ClsTableName) + 'Service.delete(id).then(function (response) {
                    myToastr.success("' + dbo.fnUserFriendlyName(@ClsTableName) + ' delete successfully.");
                    $scope.searchRecord();
                });
            }
        }
    };
    //#endregion

    //#region search
    $scope.searchRecord = function () {
        ' + dbo.fnJavaScriptName(@ClsTableName) + 'Service.getGridData($scope.' + dbo.fnJavaScriptName(@ClsTableName) + 'Search).then(function (response) {
            var ' + dbo.fnJavaScriptName(@ClsTableName) + 'GridModel = new ' + @ClsTableName + 'GridModel();
            ' + dbo.fnJavaScriptName(@ClsTableName) + 'GridModel = response;
            $scope.' + dbo.fnJavaScriptName(@ClsTableName) + 's = ' + dbo.fnJavaScriptName(@ClsTableName) + 'GridModel.' + dbo.fnJavaScriptName(@ClsTableName) + 's;
            $scope.' + dbo.fnJavaScriptName(@ClsTableName) + 'Search.totalRecords = ' + dbo.fnJavaScriptName(@ClsTableName) + 'GridModel.totalRecords;
        });
    }

    $scope.searchCancel = function () {
        $scope.' + dbo.fnJavaScriptName(@ClsTableName) + 'Search = new ' + @ClsTableName + 'ParameterModel();

        $scope.' + dbo.fnJavaScriptName(@ClsTableName) + 'Search.sortExpression = ''' + @PKFieldName + ''';
        $scope.' + dbo.fnJavaScriptName(@ClsTableName) + 'Search.sortDirection = ''asc'';
        $scope.' + dbo.fnJavaScriptName(@ClsTableName) + 'Search.pageIndex = 1;
        $scope.' + dbo.fnJavaScriptName(@ClsTableName) + 'Search.pageSize = 10;

		$scope.searchRecord();
    };

    $scope.sortRecord = function (sortExpression) {
        if (sortExpression == $scope.' + dbo.fnJavaScriptName(@ClsTableName) + 'Search.sortExpression) {
            $scope.' + dbo.fnJavaScriptName(@ClsTableName) + 'Search.SortDirection == ''asc'' ? $scope.' + dbo.fnJavaScriptName(@ClsTableName) + 'Search.sortDirection = ''desc'' : $scope.' + dbo.fnJavaScriptName(@ClsTableName) + 'Search.sortDirection = ''asc'';
        }
        else {
            $scope.' + dbo.fnJavaScriptName(@ClsTableName) + 'Search.sortExpression = sortExpression;
            $scope.' + dbo.fnJavaScriptName(@ClsTableName) + 'Search.sortDirection = ''asc'';
        }
        $scope.searchRecord();
    };
    $scope.pageChanged = function () {
        $scope.searchRecord();
    };
    //#endregion

    //#region UI
    $scope.setPageListMode = function () {
        ' + dbo.fnJavaScriptName(@ClsTableName) + 'Service.getListMode($scope.' + dbo.fnJavaScriptName(@ClsTableName) + 'Search).then(function (response) {
            var ' + dbo.fnJavaScriptName(@ClsTableName) + 'ListModel = new ' + @ClsTableName + 'ListModel();
            ' + dbo.fnJavaScriptName(@ClsTableName) + 'ListModel = response;
            ' + @ListLovSetAngular + '
            $scope.' + dbo.fnJavaScriptName(@ClsTableName) + 's = ' + dbo.fnJavaScriptName(@ClsTableName) + 'ListModel.' + dbo.fnJavaScriptName(@ClsTableName) + 's;
            $scope.' + dbo.fnJavaScriptName(@ClsTableName) + 'Search.totalRecords = ' + dbo.fnJavaScriptName(@ClsTableName) + 'ListModel.totalRecords;
        });
    }

    $scope.showHideSearchCriteria = function () {
        if ($scope.showSearchCriteria) {
            $scope.showSearchCriteria = false;
            $scope.showSearchCriteriaImage = ''chevron-down'';
        }
        else {
            $scope.showSearchCriteria = true;
            $scope.showSearchCriteriaImage = ''chevron-up'';
        }
    }

    $scope.setLOVConfiguration = function () {
        ' + @ListLovDeclareAngular + '
    }

    $scope.initializePage = function () {
        $scope.accessModel = Session.getAccess();

        $scope.setLOVConfiguration();

        $scope.showSearchCriteria = false;

        $scope.searchCancel();

        $scope.setPageListMode();

        $scope.showHideSearchCriteria();
    }
    $scope.initializePage();
    //#endregion
}]);
'

	SET @AddEditHtmlString = N'
    <section class="content">
        <form class="form-horizontal" name="' + dbo.fnJavaScriptName(@ClsTableName) + 'Form" novalidate>
            <div class="row">
                <div class="col-xs-12">
                    <div class="box">
                        <div class="box-header with-border">
                            <h3 class="box-title">' + dbo.fnUserFriendlyName(@ClsTableName) + ' {{mode}}</h3>
                        </div>
                        <div class="box-body">
' + @HtmlInputModeFields + '
                        </div>
                        <div class="box-footer">
                            <button type="button" ng-click="cancelRecord();" class="btn btn-default">Cancel</button>
                            <button type="submit" ng-click="submitRecord();" class="btn btn-info pull-right" ng-disabled="!(accessModel.canInsert || accessModel.canUpdate)" title="{{accessModel.canInsert || accessModel.canUpdate ? ''Click here to subit record.'' : ''You have not not add or edit rights.''}}">Submit</button>
                        </div>
                    </div>
                </div>
            </div>
        </form>
    </section>
'

	SET @ListHtmlString = N'
    <section class="content">
        <form class="form-horizontal" name="' + dbo.fnJavaScriptName(@ClsTableName) + 'SearchForm" novalidate>
            <div class="row">
                <div class="col-xs-12">
                    <div class="box">
                        <div class="box-header with-border">
                            <h3 class="box-title">' + dbo.fnUserFriendlyName(@ClsTableName) + ' Search Criteria</h3>
                            <div class="box-tools">
                                <div class="input-group input-group-sm" style="width: 40px;">
                                    <div class="input-group-btn">
                                        <button type="button" ng-click="showHideSearchCriteria();" class="btn btn-default"><i class="{{''fa fa-'' + showSearchCriteriaImage}}"></i></button>
                                        <button type="button" ng-click="addNewRecord();" class="btn btn-default" ng-disabled="!accessModel.canInsert" title="{{accessModel.canInsert ? ''Click here to add new record.'' : ''You have not add rights.''}}"><i class="fa fa-plus"></i></button>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="box-body">
                            ' + @HtmlSearchModeFields + '
                        </div>
                        <div class="box-footer" ng-show="showSearchCriteria">
                            <button type="submit" ng-click="searchCancel();" class="btn btn-default">Reset</button>
                            <button type="submit" ng-click="searchRecord();" class="btn btn-info pull-right">Submit</button>
                        </div>
                    </div>
                </div>
                <div class="col-xs-12">
                    <div class="box">
                        <div class="box-header with-border">
                            <h3 class="box-title">' + dbo.fnUserFriendlyName(@ClsTableName) + ' List</h3>
                       </div>
                        <div class="box-body table-responsive">
                            <table class="table table-bordered">
                                <tbody>
                                    <tr>
                                        ' + @HtmlGridFieldsHeader + '
                                        <th style="width:50px;text-align:center">Edit</th>
                                        <th style="width:50px;text-align:center">Delete</th>
                                    </tr>
                                    <tr ng-repeat="' + dbo.fnJavaScriptName(@ClsTableName) + 'List in ' + @ClsTableName + 's">
                                        ' + @HtmlGridFieldsDetail + '
                                        <td style="width:50px;text-align:center">
                                            <button type="button" class="btn btn-default" ng-click="editRecord(' + dbo.fnJavaScriptName(@ClsTableName) + 'List.Id);" ng-disabled="!accessModel.canUpdate" title="{{accessModel.canUpdate ? ''Click here to edit this record.'' : ''You have not edit rights.''}}"><i class="fa fa-pencil"></i></button>
                                        </td>
                                        <td style="width:50px;text-align:center">
                                            <button type="button" class="btn btn-default" ng-click="deleteRecord(' + dbo.fnJavaScriptName(@ClsTableName) + 'List.Id);" ng-disabled="!accessModel.canDelete" title="{{accessModel.canDelete ? ''Click here to delete this record.'' : ''You have not delete rights.''}}"><i class="fa fa-remove"></i></button>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                            <div class="box-tools">
                                <SELECT ng-change="pageChanged();" ng-model="' + dbo.fnJavaScriptName(@ClsTableName) + 'Search.PageSize">
                                    <option value="10">10</option>
                                    <option value="20">20</option>
                                    <option value="25">25</option>
                                    <option value="50">50</option>
                                    <option value="100">100</option>
                                    <option value="150">150</option>
                                    <option value="200">200</option>
                                </select>
                                <ul uib-pagination items-per-page="' + dbo.fnJavaScriptName(@ClsTableName) + 'Search.pageSize" ng-change="pageChanged();" total-items="' + dbo.fnJavaScriptName(@ClsTableName) + 'Search.totalRecords" ng-model="' + dbo.fnJavaScriptName(@ClsTableName) + 'Search.pageIndex" max-size="3" class="pagination-sm no-margin pull-right" boundary-link-numbers="true"></ul>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </form>
    </section>
	'

	SET @Angular2ModuleString = '
import { NgModule } from ''@angular/core'';
import { FormsModule } from ''@angular/forms'';
import { CommonModule } from ''@angular/common'';
import { ColumnResizerModule } from ''../../common-component/column-resizer/column-resizer.module'';
import { ' + @ClsTableName + 'ListComponent } from ''./' + LOWER(@ClsTableName) + '-list.component'';
import { ' + @ClsTableName + 'FormComponent } from ''./' + LOWER(@ClsTableName) + '-form.component'';
import { ' + @ClsTableName + 'Route } from ''./' + LOWER(@ClsTableName) + '.route'';
import { ' + @ClsTableName + 'Component } from ''./' + LOWER(@ClsTableName) + '.component'';
import { ' + @ClsTableName + 'Service } from ''./' + LOWER(@ClsTableName) + '.service'';

@NgModule({
    imports: [
        FormsModule,
        CommonModule,
		ColumnResizerModule,
        ' + @ClsTableName + 'Route
    ],
    declarations: [
        ' + @ClsTableName + 'Component,
        ' + @ClsTableName + 'ListComponent,
        ' + @ClsTableName + 'FormComponent
    ],
    providers: [
        ' + @ClsTableName + 'Service
    ]
})
export class ' + @ClsTableName + 'Module { }
'

	SET @Angular2ModelString = '
import { PagingSortingModel } from ''../../../models/pagingsorting.model'';

' + @Angular2LOVImportModel + '            

export class ' + @ClsTableName + 'MainModel {
' + @Angular2MainModelFieldWithDefault + '            
}

export class ' + @ClsTableName + 'Model extends ' + @ClsTableName + 'MainModel {
' + @Angular2ModelFieldWithDefault + '            
}

export class ' + @ClsTableName + 'AddModel {
' + @Angular2AddLOVModel + '            
}

export class ' + @ClsTableName + 'EditModel extends ' + @ClsTableName + 'AddModel{
    public ' + dbo.fnJavaScriptName(@ClsTableName) + ': ' + @ClsTableName + 'Model;
}

export class ' + @ClsTableName + 'GridModel {
    public ' + dbo.fnJavaScriptName(@ClsTableName) + 's: ' + @ClsTableName + 'Model[] = [];
    public totalRecords: number = 0;
}

export class ' + @ClsTableName + 'ListModel extends ' + @ClsTableName + 'GridModel {
' + @Angular2ListLOVModel + '            
}

export class ' + @ClsTableName + 'ParameterModel extends PagingSortingModel {
' + @Angular2ParameterModelFieldWithDefault + '            
}
'

	SET @Angular2ServiceString = '
import { Injectable } from ''@angular/core'';
import { Observable } from ''rxjs'';
import { Observable } from ''rxjs/Rx'';
import { map } from ''rxjs/operators'';
import { HttpService } from ''../../common/services/http.service'';
import { ' + @ClsTableName + 'Model, ' + @ClsTableName + 'MainModel, ' + @ClsTableName + 'ParameterModel, ' + @ClsTableName + 'GridModel, ' + @ClsTableName + 'AddModel, ' + @ClsTableName + 'EditModel, ' + @ClsTableName + 'ListModel } from ''./' + LOWER(@ClsTableName) + '.model'';

@Injectable()
export class ' + @ClsTableName + 'Service {
    constructor(private http: BaseService) {
    }

    public getRecord(id: number): Observable<' + @ClsTableName + 'Model> {
        return this.http.get(''' + LOWER(@Route) + '/' + LOWER(@ClsTableName) + '/getRecord/'' + id).pipe(
            map((response: ' + @ClsTableName + 'Model) => {
                return response;
            }),
        );
    }

    public getForLOV(' + dbo.fnJavaScriptName(@ClsTableName) + 'Parameter: ' + @ClsTableName + 'ParameterModel): Observable<' + @ClsTableName + 'MainModel[]> {
        return this.http.post(''' + LOWER(@Route) + '/' + LOWER(@ClsTableName) + '/getLovValue'', ' + dbo.fnJavaScriptName(@ClsTableName) + 'Parameter).pipe(
            map((response: ' + @ClsTableName + 'MainModel[]) => {
                return response;
            }),
        );
    }

	' + CASE WHEN @AddLOVs = '' THEN '' ELSE '
    public getAddMode(' + dbo.fnJavaScriptName(@ClsTableName) + 'Parameter: ' + @ClsTableName + 'ParameterModel): Observable<' + @ClsTableName + 'AddModel> {
        return this.http.post(''' + LOWER(@Route) + '/' + LOWER(@ClsTableName) + '/getAddMode'', ' + dbo.fnJavaScriptName(@ClsTableName) + 'Parameter).pipe(
            map((response: ' + @ClsTableName + 'AddModel) => {
                return response;
            }),
        );
    }

    public getEditMode(' + dbo.fnJavaScriptName(@ClsTableName) + 'Parameter: ' + @ClsTableName + 'ParameterModel): Observable<' + @ClsTableName + 'EditModel> {
        return this.http.post(''' + LOWER(@Route) + '/' + LOWER(@ClsTableName) + '/getEditMode'', ' + dbo.fnJavaScriptName(@ClsTableName) + 'Parameter).pipe(
            map((response: ' + @ClsTableName + 'EditModel) => {
                return response;
            }),
        );
    }
	' END + '

    public getForGrid(' + dbo.fnJavaScriptName(@ClsTableName) + 'Parameter: ' + @ClsTableName + 'ParameterModel): Observable<' + @ClsTableName + 'GridModel> {
        return this.http.post(''' + LOWER(@Route) + '/' + LOWER(@ClsTableName) + '/getGridData'', ' + dbo.fnJavaScriptName(@ClsTableName) + 'Parameter).pipe(
            map((response: ' + @ClsTableName + 'GridModel) => {
                return response;
            }),
        );
    }

	' + CASE WHEN @ListLOVs = '' THEN '' ELSE '
    public getListMode(' + dbo.fnJavaScriptName(@ClsTableName) + 'Parameter: ' + @ClsTableName + 'ParameterModel): Observable<' + @ClsTableName + 'ListModel> {
        return this.http.post(''' + LOWER(@Route) + '/' + LOWER(@ClsTableName) + '/getListMode'', ' + dbo.fnJavaScriptName(@ClsTableName) + 'Parameter).pipe(
            map((response: ' + @ClsTableName + 'ListModel) => {
                return response;
            }),
        );
    }
	' END + '

    public save(' + dbo.fnJavaScriptName(@ClsTableName) + ': ' + @ClsTableName + 'Model): Observable<number> {
        if (' + dbo.fnJavaScriptName(@ClsTableName) + '.id === 0)
            return this.http.post(''' + LOWER(@Route) + '/' + LOWER(@ClsTableName) + '/insert'', ' + dbo.fnJavaScriptName(@ClsTableName) + ').pipe(
                map((response: number) => {
                    return response;
                }),
            );
        else
            return this.http.post(''' + LOWER(@Route) + '/' + LOWER(@ClsTableName) + '/update'', ' + dbo.fnJavaScriptName(@ClsTableName) + ').pipe(
                map((response: number) => {
                    return response;
                }),
            );
    }

    public delete(id: number): Observable<void> {
        return this.http.post(''' + LOWER(@Route) + '/' + LOWER(@ClsTableName) + '/delete/'' + id, null).pipe(
            map((response: void) => {
                return response;
            }),
        );
    }
}
'

	SET @Angular2RouteString = '
import { NgModule } from ''@angular/core'';
import { ExtraOptions, RouterModule, Routes } from ''@angular/router'';
import { ' + @ClsTableName + 'ListComponent } from ''./' + LOWER(@ClsTableName) + '-list.component'';
import { ' + @ClsTableName + 'FormComponent } from ''./' + LOWER(@ClsTableName) + '-form.component'';
import { ' + @ClsTableName + 'Component } from ''./' + LOWER(@ClsTableName) + '.component'';

const routes: Routes = [{
    path: '''',
    component: ' + @ClsTableName + 'Component,
    children: [{
        path: '''',
        redirectTo: ''list'',
    },
    {
        path: ''list'',
        component: ' + @ClsTableName + 'ListComponent,
    },
    {
        path: ''add'',
        component: ' + @ClsTableName + 'FormComponent,
    },
    {
        path: ''edit/:id'',
        component: ' + @ClsTableName + 'FormComponent,
    },
	],
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class ' + @ClsTableName + 'Route {
}
'

	SET @Angular2ComponentString = '
import { Component } from ''@angular/core'';

@Component({
  selector: ''' + LOWER(@ClsTableName) + ''',
  template: `
    <router-outlet></router-outlet>
  `,
})
export class ' + @ClsTableName + 'Component {
}
'

	SET @Angular2FormHTMLString = '
<div class="row">
    <div class="col-lg-12">
        <form #' + dbo.fnJavaScriptName(@ClsTableName) + 'Form="ngForm" novalidate>
            <nb-card>
                <nb-card-header>
                    <span>{{mode}} ' + dbo.fnUserFriendlyName(@ClsTableName) + '</span>
                </nb-card-header>
                <nb-card-body>
' + REPLACE(@HtmlInputModeFields2, '#Language#', '') + '
                </nb-card-body>
                <nb-card-footer>
                    <div>
                        <button type="submit" class="btn btn-success btn-tn" (click)="save(' + dbo.fnJavaScriptName(@ClsTableName) + 'Form.valid)">Save</button>
                        <button type="submit" class="btn btn-secondary btn-tn" (click)="cancel()">Cancel</button>
                    </div>
                </nb-card-footer>
            </nb-card>
        </form>
    </div>
</div>
'

	SET @Angular2FormComponentString = '
import { Component, OnInit, OnDestroy } from ''@angular/core'';
import { Router, ActivatedRoute, UrlSegment } from ''@angular/router'';
import { ' + @ClsTableName + 'Service } from ''./' + LOWER(@ClsTableName) + '.service'';
import { ' + @ClsTableName + 'Model, ' + @ClsTableName + 'AddModel, ' + @ClsTableName + 'EditModel, ' + @ClsTableName + 'ParameterModel } from ''./' + LOWER(@ClsTableName) + '.model'';
import { SessionService } from ''../../../services/session.service'';
import { AccessModel } from ''../../../models/access.model'';
import { MyToasterService } from ''../../../services/my-toaster.service'';

@Component({
    selector: ''' + LOWER(@ClsTableName) + '-form'',
    templateUrl: ''./' + LOWER(@ClsTableName) + '-form.component.html'',
    styles: [`
    nb-card {
      transform: translate3d(0, 0, 0);
    }
  `],
})
export class ' + @ClsTableName + 'FormComponent implements OnInit, OnDestroy {
    public access: AccessModel = new AccessModel();
    public ' + dbo.fnJavaScriptName(@ClsTableName) + ': ' + @ClsTableName + 'Model = new ' + @ClsTableName + 'Model();
    public ' + dbo.fnJavaScriptName(@ClsTableName) + 's: ' + @ClsTableName + 'Model[] = [];
	public ' + dbo.fnJavaScriptName(@ClsTableName) + 'Add: ' + @ClsTableName + 'AddModel = new ' + @ClsTableName + 'AddModel();
	public ' + dbo.fnJavaScriptName(@ClsTableName) + 'Edit: ' + @ClsTableName + 'EditModel = new ' + @ClsTableName + 'EditModel();
    public ' + dbo.fnJavaScriptName(@ClsTableName) + 'Parameter: ' + @ClsTableName + 'ParameterModel = new ' + @ClsTableName + 'ParameterModel();
	' + REPLACE(ISNULL(STUFF(( SELECT  '' + value FROM  ( SELECT xs.value FROM (SELECT ' public ' + dbo.fnJavaScriptName(value) +  's: ' + value +  'MainModel[] = [];' AS value FROM dbo.fnSplit(@AddLOVs, ' ')) AS XS ) x FOR XML PATH('') ), 1, 1, ''), ''), ';', ';' + CHAR(13))  + '

    public hasAccess: boolean = false;
    public mode: string;
    public id: number;
    private sub: any;

    constructor(private ' + dbo.fnJavaScriptName(@ClsTableName) + 'Service: ' + @ClsTableName + 'Service,
        private sessionService: SessionService,
        private router: Router,
        private route: ActivatedRoute,
        private toastr: MyToasterService) {

    }

    ngOnInit() {
        this.getRouteData();
    }

    ngOnDestroy() {
        this.sub.unsubscribe();
    }

    public getRouteData(): void {
        this.sub = this.route.params.subscribe(params => {
            const segments: UrlSegment[] = this.route.snapshot.url;
            if (segments.toString().toLowerCase() === ''add'')
                this.id = 0;
            else
                this.id = +params[''id'']; // (+) converts string ''id'' to a number
            this.setPageMode();
        });
    }

    public clearModels(): void {
        this.' + dbo.fnJavaScriptName(@ClsTableName) + ' = new ' + @ClsTableName + 'Model();
    }

    public setPageMode(): void {
        if (this.id === undefined || this.id === 0)
            this.setPageAddMode();
        else
            this.setPageEditMode();

        if (this.hasAccess) {
        }
    }

    public setPageAddMode(): void {
        if (!this.access.CanInsert) {
            this.toastr.warning(''You do not have add access of this page.'');
            return;
        }
        this.hasAccess = true;
        this.mode = ''Add'';

		' + CASE WHEN @AddLOVs = '' THEN '' ELSE '
		this.' + dbo.fnJavaScriptName(@ClsTableName) + 'Service.getAddMode(this.' + dbo.fnJavaScriptName(@ClsTableName) + 'Parameter).subscribe(data => {
            this.' + dbo.fnJavaScriptName(@ClsTableName) + 'Add = data;
			' + REPLACE(ISNULL(STUFF(( SELECT  '' + value FROM  ( SELECT xs.value FROM (SELECT ' this.' + dbo.fnJavaScriptName(value) +  's = this.' + dbo.fnJavaScriptName(@ClsTableName) + 'Add.' + dbo.fnJavaScriptName(value) +  's;' AS value FROM dbo.fnSplit(@AddLOVs, ' ')) AS XS ) x FOR XML PATH('') ), 1, 1, ''), ''), ';', ';' + CHAR(13))  + '
        });
		' END  + '
        this.clearModels();
    }

    public setPageEditMode(): void {
        if (!this.access.canUpdate) {
            this.toastr.warning(''You do not have update access of this page.'');
            return;
        }
        this.hasAccess = true;
        this.mode = ''Edit'';

		' + CASE WHEN @AddLOVs = '' THEN '
        this.' + dbo.fnJavaScriptName(@ClsTableName) + 'Service.getRecord(this.id).subscribe(data => {
            this.' + dbo.fnJavaScriptName(@ClsTableName) + ' = data;
        });
		' ELSE '
        this.' + dbo.fnJavaScriptName(@ClsTableName) + 'Parameter.id = this.id;
		this.' + dbo.fnJavaScriptName(@ClsTableName) + 'Service.getEditMode(this.' + dbo.fnJavaScriptName(@ClsTableName) + 'Parameter).subscribe(data => {
            this.' + dbo.fnJavaScriptName(@ClsTableName) + 'Edit = data;
            this.' + dbo.fnJavaScriptName(@ClsTableName) + ' = this.' + dbo.fnJavaScriptName(@ClsTableName) + 'Edit.' + dbo.fnJavaScriptName(@ClsTableName) + ';
			' + REPLACE(ISNULL(STUFF(( SELECT  '' + value FROM  ( SELECT xs.value FROM (SELECT ' this.' + dbo.fnJavaScriptName(value) +  's = this.' + dbo.fnJavaScriptName(@ClsTableName) + 'Edit.' + dbo.fnJavaScriptName(value) +  's;' AS value FROM dbo.fnSplit(@AddLOVs, ' ')) AS XS ) x FOR XML PATH('') ), 1, 1, ''), ''), ';', ';' + CHAR(13))  + '
        });
		' END  + '
    }

    public save(isFormValid: boolean): void {
        if (isFormValid) {
            if (!this.access.canInsert && !this.access.canUpdate) {
                this.toastr.warning(''You do not have add or edit access of this page.'');
                return;
            }

            this.' + dbo.fnJavaScriptName(@ClsTableName) + 'Service.save(this.' + dbo.fnJavaScriptName(@ClsTableName) + ').subscribe(data => {
                if (data === 0)
                    this.toastr.warning(''Record is already exist.'');
                else if (data > 0) {
                    this.toastr.success(''Record submitted successfully.'');
                    this.cancel();
                }
            });
        } else {
            this.toastr.warning(''Please provide valid input.'');
        }
    }
	
	public cancel(): void {
		this.router.navigate([''/' + LOWER(@Route) + '/' + LOWER(@ClsTableName) + '/list'']);
	}
}'	

	SET @Angular2ListHTMLString = '
<nb-card>
    <nb-card-header>
        ' + dbo.fnUserFriendlyName(@ClsTableName) + 's
        <button (click)="add()" class="btn btn-icon btn-tnx pull-right" [ngClass]="{''btn-outline-success'':access.canInsert, ''btn-outline-secondary'':!access.canInsert}" [disabled]="!access.canInsert" title="{{access.canInsert ? ''Click here to add new record.'' : ''You do not have add access of this page.''}}"><i class="fa fa-plus"></i></button>
        <div class="input-group pull-right header-button" style="width:250px;">
            <input type="text" class="form-control" [(ngModel)]="' + dbo.fnJavaScriptName(@ClsTableName) + 'Parameter.name" placeholder="Search here..." />
            <span class="input-group-btn">
                <button (click)="search()" class="btn btn-icon btn-tnx"><i class="fa fa-search"></i></button>
            </span>
        </div>
    </nb-card-header>
    <nb-card-body>
		<div class="row">
			<div class="col-lg-12">
				<form #Search' + @ClsTableName + 'Form="ngForm" novalidate>
					<nb-card>
						<nb-card-header>
							<span>Search ' + dbo.fnUserFriendlyName(@ClsTableName) + '</span>
						</nb-card-header>
						<nb-card-body>
		' + REPLACE(@HtmlSearchModeFields2, '#Language#', '') + '
						</nb-card-body>
						<nb-card-footer>
							<div>
								<button type="submit" class="btn btn-success btn-tn" (click)="search()">Search</button>
								<button type="submit" class="btn btn-secondary btn-tn" (click)="reset()">Reset</button>
							</div>
						</nb-card-footer>
					</nb-card>
				</form>
			</div>
		</div>
		<hr />
        <table class="table table-bordered sort">
            <thead>
                <tr column-resizer [id]="''' + dbo.fnJavaScriptName(@ClsTableName) + '-list''">
					' + @HtmlGridFieldsHeader2 + '
                    <th style="width:90px;">Actions</th>
                </tr>
            </thead>
            <tbody>
                <tr *ngFor="let item of ' + dbo.fnJavaScriptName(@ClsTableName) + 'Grid?.' + dbo.fnJavaScriptName(@ClsTableName) + 's;" (click)="' + dbo.fnJavaScriptName(@ClsTableName) + 'Parameter.id == item.id;" [ngClass]="{''selected-row'': (' + dbo.fnJavaScriptName(@ClsTableName) + 'Parameter.id == item.id)}" >
					' + @HtmlGridFieldsDetail2 + '
                    <td>
                        <button class="btn btn-icon btn-tnx" (click)="edit(item.id)" [ngClass]="{''btn-outline-primary'':access.canUpdate, ''btn-outline-secondary'':!access.canUpdate}" [disabled]="!access.canUpdate" title="{{access.canUpdate ? ''Click here to edit this record.'' : ''You do not have edit access of this page.''}}"><i class="fa fa-pencil"></i></button>
                        <button class="btn btn-icon btn-tnx" (click)="delete(item.id)" [ngClass]="{''btn-outline-danger'':access.canDelete, ''btn-outline-secondary'':!access.canDelete}" [disabled]="!access.canDelete" title="{{access.canDelete ? ''Click here to delete this record.'' : ''You do not have delete access of this page.''}}"><i class="fa fa-trash"></i></button>
                    </td>
                </tr>
            </tbody>
        </table>
    </nb-card-body>
    <nb-card-footer>
        <div class="pull-left card-header-controls">
            <select [(ngModel)]="' + dbo.fnJavaScriptName(@ClsTableName) + 'Parameter.pageSize" (change)="search()" class="form-control">
                <option value="5">5</option>
                <option value="10">10</option>
                <option value="20">20</option>
                <option value="30">30</option>
                <option value="40">40</option>
                <option value="50">50</option>
                <option value="100">100</option>
                <option value="200">200</option>
            </select>
        </div>
		<div class="pull-left total-records">Total Record(s) <span>{{' + dbo.fnJavaScriptName(@ClsTableName) + 'Grid.totalRecords}}</span></div>
        <div class="pull-right">
            <ngb-pagination [collectionSize]="' + dbo.fnJavaScriptName(@ClsTableName) + 'Grid.totalRecords" [pageSize]="' + dbo.fnJavaScriptName(@ClsTableName) + 'Parameter.pageSize" [(page)]="' + dbo.fnJavaScriptName(@ClsTableName) + 'Parameter.pageIndex" [maxSize]="5" [rotate]="true" [ellipses]="false" [boundaryLinks]="true" (pageChange)="search()"></ngb-pagination>
        </div>
    </nb-card-footer>
</nb-card>
'

	SET @Angular2ListComponentString = '
import { Component, OnInit } from ''@angular/core'';
import { Router } from ''@angular/router'';
import { ' + @ClsTableName + 'Service } from ''./' + LOWER(@ClsTableName) + '.service'';
import { ' + @ClsTableName + 'ParameterModel, ' + @ClsTableName + 'GridModel, ' + @ClsTableName + 'ListModel, ' + @ClsTableName + 'Model } from ''./' + LOWER(@ClsTableName) + '.model'';
import { SessionService } from ''../../../services/session.service'';
import { StateParamService } from ''../../../services/stateparam.service'';
import { AccessModel } from ''../../../models/access.model'';
import { MyToasterService } from ''../../../services/my-toaster.service'';

@Component({
    selector: ''' + LOWER(@ClsTableName) + '-list'',
    templateUrl: ''./' + LOWER(@ClsTableName) + '-list.component.html'',
    styles: [`
    nb-card {
      transform: translate3d(0, 0, 0);
    }
  `],
})
export class ' + @ClsTableName + 'ListComponent implements OnInit {
    public access: AccessModel = new AccessModel();
    public ' + dbo.fnJavaScriptName(@ClsTableName) + 'Parameter: ' + @ClsTableName + 'ParameterModel = new ' + @ClsTableName + 'ParameterModel();
    public ' + dbo.fnJavaScriptName(@ClsTableName) + 'Grid: ' + @ClsTableName + 'GridModel = new ' + @ClsTableName + 'GridModel();
    public ' + dbo.fnJavaScriptName(@ClsTableName) + 'List: ' + @ClsTableName + 'ListModel = new ' + @ClsTableName + 'ListModel();
	' + REPLACE(ISNULL(STUFF(( SELECT  '' + value FROM  ( SELECT xs.value FROM (SELECT ' public ' + dbo.fnJavaScriptName(value) +  's: ' + value +  'MainModel[] = [];' AS value FROM dbo.fnSplit(@ListLOVs, ' ')) AS XS ) x FOR XML PATH('') ), 1, 1, ''), ''), ';', ';' + CHAR(13))  + '

    constructor(private ' + dbo.fnJavaScriptName(@ClsTableName) + 'Service: ' + @ClsTableName + 'Service,
        private sessionService: SessionService,
		private stateParam: StateParamService,
        private router: Router,
        private toastr: MyToasterService) {
		this.setAccess();
    }

    ngOnInit() {
        this.setPageListMode();
    }

    public reset(): void {
		this.' + dbo.fnJavaScriptName(@ClsTableName) + 'Parameter = new ' + @ClsTableName + 'ParameterModel();
		this.' + dbo.fnJavaScriptName(@ClsTableName) + 'Parameter.sortExpression = ''Id'';		
		this.' + dbo.fnJavaScriptName(@ClsTableName) + 'Parameter.sortDirection = ''asc'';		
        this.search();
	}

    public search(): void {
        if (!this.access.canView) {
            this.toastr.warning(''You do not have view access of this page.'');
            return;
        }

        this.' + dbo.fnJavaScriptName(@ClsTableName) + 'Service.getForGrid(this.' + dbo.fnJavaScriptName(@ClsTableName) + 'Parameter).subscribe(data => {
            this.' + dbo.fnJavaScriptName(@ClsTableName) + 'Grid = data;
        });
    }

    public sort(sortExpression: string): void {
        if (sortExpression === this.' + dbo.fnJavaScriptName(@ClsTableName) + 'Parameter.sortExpression) {
            this.' + dbo.fnJavaScriptName(@ClsTableName) + 'Parameter.sortDirection = this.' + dbo.fnJavaScriptName(@ClsTableName) + 'Parameter.sortDirection === ''asc'' ? ''desc'' : ''asc'';
        }
        else {
            this.' + dbo.fnJavaScriptName(@ClsTableName) + 'Parameter.sortExpression = sortExpression;
            this.' + dbo.fnJavaScriptName(@ClsTableName) + 'Parameter.sortDirection = ''asc'';
        }
        this.search();
    }

    public add(): void {
        if (!this.access.canInsert) {
            this.toastr.warning(''You do not have add access of this page.'');
            return;
        }
		
		this.stateParam.push(''' + dbo.fnJavaScriptName(@ClsTableName) + 'Parameter'', this.' + dbo.fnJavaScriptName(@ClsTableName) + 'Parameter);
        this.router.navigate([''/' + LOWER(@Route) + '/' + LOWER(@ClsTableName) + '/add'']);
    }

    public edit(id: number): void {
        if (!this.access.canUpdate) {
            this.toastr.warning(''You do not have edit access of this page.'');
            return;
        }

		this.stateParam.push(''' + dbo.fnJavaScriptName(@ClsTableName) + 'Parameter'', this.' + dbo.fnJavaScriptName(@ClsTableName) + 'Parameter);
        this.router.navigate([''/' + LOWER(@Route) + '/' + LOWER(@ClsTableName) + '/edit'', id]);
    }

    public delete(id: number): void {
        if (!this.access.canDelete) {
            this.toastr.warning(''You do not have delete access of this page.'');
            return;
        }

        if (window.confirm(''Are you sure you want to delete?'')) {
            this.' + dbo.fnJavaScriptName(@ClsTableName) + 'Service.delete(id).subscribe(data => {
                this.toastr.success(''Record deleted successfully.'');
                this.search();
            });
        }
    }

    public setPageListMode(): void {
        if (!this.access.canView) {
            this.toastr.warning(''You do not have view access of this page.'');
            return;
        }

        this.' + dbo.fnJavaScriptName(@ClsTableName) + 'Parameter.sortExpression = ''Id'';
		this.setParameterByStateParam();
		' + CASE WHEN @ListLOVs = '' THEN '
        this.search();
		' ELSE '
        this.' + dbo.fnJavaScriptName(@ClsTableName) + 'Service.getListMode(this.' + dbo.fnJavaScriptName(@ClsTableName) + 'Parameter).subscribe(data => {
            this.' + dbo.fnJavaScriptName(@ClsTableName) + 'List = data;
            this.' + dbo.fnJavaScriptName(@ClsTableName) + 'Grid.' + dbo.fnJavaScriptName(@ClsTableName) + 's = this.' + dbo.fnJavaScriptName(@ClsTableName) + 'List.' + dbo.fnJavaScriptName(@ClsTableName) + 's;
            this.' + dbo.fnJavaScriptName(@ClsTableName) + 'Grid.totalRecords = this.' + dbo.fnJavaScriptName(@ClsTableName) + 'List.totalRecords;
			' + REPLACE(ISNULL(STUFF(( SELECT  '' + value FROM  ( SELECT xs.value FROM (SELECT ' this.' + dbo.fnJavaScriptName(value) +  's = this.' + dbo.fnJavaScriptName(@ClsTableName) + 'List.' + dbo.fnJavaScriptName(value) +  's;' AS value FROM dbo.fnSplit(@ListLOVs, ' ')) AS XS ) x FOR XML PATH('') ), 1, 1, ''), ''), ';', ';' + CHAR(13))  + '
        });
		' END + '
    }
	
	public setParameterByStateParam(): void {
		let params = this.stateParam.popTill(''' + dbo.fnJavaScriptName(@ClsTableName) + 'Parameter'').params;
		if(params != '''') {
			this.' + dbo.fnJavaScriptName(@ClsTableName) + 'Parameter = params;
            this.' + dbo.fnJavaScriptName(@ClsTableName) + 'Grid.totalRecords = +params.totalRecords;
		}
	}
	
	public setAccess(): void {
		const currentUrl = this.router.url.substring(0, this.router.url.indexOf(''/list''));
		this.access = this.sessionService.getAccess(currentUrl);
	}
}
'
    SELECT @TableName + '_Insert.sql' [Insert SP File Name]
	SELECT @SpInsertString AS [Insert SP]
    SELECT @TableName + '_Update.sql' [Update SP File Name]
	SELECT @SpUpdateString AS [Update SP]
    SELECT @TableName + '_Delet.sql' [Delete SP File Name]
	SELECT @SpDeleteString AS [Delete SP]
    SELECT @TableName + '_Select.sql' [Select SP File Name]
	SELECT @SpSelectString AS [Select SP]
    SELECT @TableName + '_SelectForAdd.sql' [Select For Add SP File Name]
	SELECT @SpSelectForAddString AS [Select For Add SP]
    SELECT @TableName + '_SelectForEdit.sql' [Select For Edit SP File Name]
	SELECT @SpSelectForEditString AS [Select For Edit SP]
    SELECT @TableName + '_SelectForGrid.sql' [Select For Grid SP File Name]
	SELECT @SpSelectForGridString AS [Select For Grid SP]
    SELECT @TableName + '_SelectForList.sql' [Select For List SP File Name]
	SELECT @SpSelectForListString AS [Select For List SP]
    SELECT @TableName + '_SelectForLOV.sql' [Select For LOV SP File Name]
	SELECT @SpSelectForLOVString AS [Select For LOV SP]
    SELECT @TableName + '_SelectForRecord.sql' [Select For Record SP File Name]
	SELECT @SpSelectForRecordString AS [Select For Record SP]

    SELECT @TableName + 'Entity.cs' [Entity Class File Name]
	SELECT @Entity_String AS Entity
    SELECT @TableName + 'Business.cs' [Business Class File Name]
	SELECT @Business_String AS Business
    SELECT @TableName + 'Controller.cs' [Api Controller Name]
	SELECT @WebAPIControllerString AS [Web API Controller]

	--SELECT @AngularModelString AS [Angular Model]
	--SELECT @AngularServiceString AS [Angular Service]
	--SELECT @AngularRouteString AS [Angular Route]
	--SELECT @AngularAddEditControllerString AS [Angular Add-Edit Controller]
	--SELECT @AngularListControllerString AS [Angular List Controller]
	--SELECT @AddEditHtmlString AS [Add Edit HTML]
	--SELECT @ListHtmlString AS [List HTML]

    SELECT LOWER(@TableName) + '.module.ts' [Angular 2 Module File Name]
	SELECT @Angular2ModuleString AS [Angular 2 Module]
    SELECT LOWER(@TableName) + '.model.ts' [Angular 2 Model File Name]
	SELECT @Angular2ModelString AS [Angular 2 Model]
    SELECT LOWER(@TableName) + '.service.ts' [Angular 2 service File Name]
	SELECT @Angular2ServiceString AS [Angular 2 Service]
    SELECT LOWER(@TableName) + '.route.ts' [Angular 2 Route File Name]
	SELECT @Angular2RouteString AS [Angular 2 Route]
    SELECT LOWER(@TableName) + '.component.ts' [Angular 2 Component File Name]
	SELECT @Angular2ComponentString AS [Angular 2 Component]
    SELECT LOWER(@TableName) + '-form.component.html' [Angular 2 Form Component HTML File Name]
	SELECT @Angular2FormHTMLString AS [Angular 2 Form HTML]
    SELECT LOWER(@TableName) + '-form.component.ts' [Angular 2 Form Component TS File Name]
	SELECT @Angular2FormComponentString AS [Angular 2 Form Component]
    SELECT LOWER(@TableName) + '-list.component.html' [Angular 2 List Component HTML File Name]
	SELECT @Angular2ListHTMLString AS [Angular 2 List HTML]
    SELECT LOWER(@TableName) + '-list.component.ts' [Angular 2 List Component TS File Name]
	SELECT @Angular2ListComponentString AS [Angular 2 List Component]
END
