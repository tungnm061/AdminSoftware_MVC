DECLARE @GenerateProcsFor varchar(100)
--SET @GenerateProcsFor = 'Orders'
SET @GenerateProcsFor = 'Employee'
 
-- which database do we want to create the procs for?
-- Change both the USE and SET lines below to set the datbase name
-- to the required database.

DECLARE @DatabaseName varchar(100)
SET @DatabaseName = 'DION_SITE'
 
-- do we want the script to print out the CREATE PROC statements
-- or do we want to execute them to actually create the procs?
-- Assign a value of either 'Print' or 'Execute'
DECLARE @PrintOrExecute varchar(10)
--SET @PrintOrExecute = 'Execute'
SET @PrintOrExecute = 'Print'
 
 
-- Is there a table name prefix i.e. 'tbl_' which we don't want
-- to include in our stored proc names?
DECLARE @TablePrefix varchar(10)
SET @TablePrefix = ''

-- What schema do you want the stored procedures to be created under?
DECLARE @SchemaName varchar(20)
SET @SchemaName = 'dbo'
 
-- For our '_lst' and '_sel' procedures do we want to
-- do SELECT * or SELECT [ColumnName,]...
-- Assign a value of either 1 or 0
DECLARE @UseSelectWildCard bit
SET @UseSelectWildCard = 0
 
-- ##########################################################
/* END SETTING OF CONFIG VARIABLE
-- do not edit below this line */
-- ##########################################################
 
 
-- DECLARE CURSOR containing all columns from user defined tables
-- in the database
DECLARE TableCol Cursor FOR
SELECT c.TABLE_SCHEMA, c.TABLE_NAME, c.COLUMN_NAME, c.DATA_TYPE, c.CHARACTER_MAXIMUM_LENGTH
FROM INFORMATION_SCHEMA.Columns c INNER JOIN
      INFORMATION_SCHEMA.Tables t ON c.TABLE_NAME = t.TABLE_NAME
WHERE t.Table_Catalog = @DatabaseName
      AND t.TABLE_TYPE = 'BASE TABLE'
      AND t.TABLE_SCHEMA = @SchemaName
ORDER BY c.TABLE_NAME, c.ORDINAL_POSITION
 
-- Declare variables which will hold values from cursor rows
DECLARE @TableSchema varchar(100), @TableName varchar(100)
DECLARE @ColumnName varchar(100), @DataType varchar(30)
DECLARE @CharLength int
 
DECLARE @ColumnNameCleaned varchar(100)
 
-- Declare variables which will track what table we are
-- creating Stored Procs for
DECLARE @CurrentTable varchar(100)
DECLARE @FirstTable bit
DECLARE @FirstColumnName varchar(100)
DECLARE @FirstColumnDataType varchar(30)
DECLARE @ObjectName varchar(100) -- this is the tablename with the
                        -- specified tableprefix lopped off.
DECLARE @TablePrefixLength int
 
-- init vars
SET @CurrentTable = ''
SET @FirstTable = 1
SET @TablePrefixLength = Len(@TablePrefix)
 
-- Declare variables which will hold the queries we are building use unicode
-- data types so that can execute using sp_ExecuteSQL
DECLARE @LIST nvarchar(4000), @UPSERT nvarchar(4000)
DECLARE @SELECT nvarchar(4000), @INSERT nvarchar(4000), @INSERTVALUES nvarchar(4000)
DECLARE @UPDATE nvarchar(4000), @DELETE nvarchar(4000), @EXISTS nvarchar(200)
 
 
-- open the cursor
OPEN TableCol
 
-- get the first row of cursor into variables
FETCH NEXT FROM TableCol INTO @TableSchema, @TableName, @ColumnName, @DataType, @CharLength
 
-- loop through the rows of the cursor
WHILE @@FETCH_STATUS = 0 BEGIN
 
      SET @ColumnNameCleaned = Replace(@ColumnName, ' ', '')
 
      -- is this a new table?
      IF @TableName <> @CurrentTable BEGIN
           
            -- if is the end of the last table
            IF @CurrentTable <> '' BEGIN
                  IF @GenerateProcsFor = '' OR @GenerateProcsFor = @CurrentTable BEGIN
 
                        -- first add any syntax to end the statement
                       
                        -- _lst
                        SET @LIST = @List + Char(13) + 'FROM [' + @TableSchema + '].[' + @CurrentTable + '] WITH(NOLOCK)' + Char(13)
                        SET @LIST = @LIST + Char(13) + Char(13) + 'SET NOCOUNT OFF' + Char(13) + Char(13)
                        SET @LIST = @LIST + Char(13)
                       
                        -- _sel
                        SET @SELECT = @SELECT + Char(13) + 'FROM [' + @TableSchema + '].[' + @CurrentTable + '] WITH(NOLOCK)' + Char(13)
                        SET @SELECT = @SELECT + 'WHERE [' + @FirstColumnName + '] = @' + Replace(@FirstColumnName, ' ', '') + Char(13)
                        SET @SELECT = @SELECT + Char(13) + Char(13) + 'SET NOCOUNT OFF' + Char(13) + Char(13)
                        SET @SELECT = @SELECT + Char(13)
     
     
                        -- UPDATE (remove trailing comma and append the WHERE clause)
                        SET @UPDATE = SUBSTRING(@UPDATE, 0, LEN(@UPDATE)- 1) + Char(13) + Char(9) + 'WHERE [' + @FirstColumnName + '] = @' + Replace(@FirstColumnName, ' ', '') + Char(13)
                       
                        -- INSERT
                        SET @INSERT = SUBSTRING(@INSERT, 0, LEN(@INSERT) - 1) + Char(13) + Char(9) + ')' + Char(13)
                        SET @INSERTVALUES = SUBSTRING(@INSERTVALUES, 0, LEN(@INSERTVALUES) -1) + Char(13) + Char(9) + ')'
                        SET @INSERT = @INSERT + @INSERTVALUES
                       
                        SET @EXISTS = ''
						SET @EXISTS = @EXISTS + 'EXISTS (SELECT * FROM [' + @TableSchema + '].[' + @CurrentTable + '] WITH(NOLOCK) '
						SET @EXISTS = @EXISTS + 'WHERE [' + @FirstColumnName + '] = @' + Replace(@FirstColumnName, ' ', '') + ')'
                       
                        -- _ups
                        SET @UPSERT = @UPSERT + Char(13) + 'AS' + Char(13)
                        SET @UPSERT = @UPSERT + 'SET NOCOUNT ON' + Char(13)
                        IF @FirstColumnDataType IN ('int', 'bigint', 'smallint', 'tinyint', 'float', 'decimal')
                        BEGIN
                             SET @UPSERT = @UPSERT + 'IF @' + Replace(@FirstColumnName, ' ', '') + ' = 0 BEGIN' + Char(13)
                        END ELSE BEGIN
                             SET @UPSERT = @UPSERT + 'IF NOT ' + @EXISTS + ' BEGIN' + Char(13)  
                        END
                        SET @UPSERT = @UPSERT + ISNULL(@INSERT, '') + Char(13)
                        SET @UPSERT = @UPSERT + Char(9) + 'SELECT SCOPE_IDENTITY() As InsertedID' + Char(13)
                        SET @UPSERT = @UPSERT + 'END' + Char(13)
                        SET @UPSERT = @UPSERT + 'ELSE BEGIN' + Char(13)
                        SET @UPSERT = @UPSERT + ISNULL(@UPDATE, '') + Char(13)
                        SET @UPSERT = @UPSERT + 'END' + Char(13) + Char(13)
                        SET @UPSERT = @UPSERT + 'SET NOCOUNT OFF' + Char(13) + Char(13)
                        SET @UPSERT = @UPSERT + Char(13)
     
                        -- _del
                        -- delete proc completed already
     
                        -- --------------------------------------------------
                        -- now either print the SP definitions or
                        -- execute the statements to create the procs
                        -- --------------------------------------------------
                        IF @PrintOrExecute <> 'Execute' BEGIN
                             PRINT @LIST
                             PRINT @SELECT
                             PRINT @UPSERT
                             PRINT @DELETE
                        END ELSE BEGIN
                             EXEC sp_Executesql @LIST
                             EXEC sp_Executesql @SELECT
                             EXEC sp_Executesql @UPSERT
                             EXEC sp_Executesql @DELETE
                        END
                  END -- end @GenerateProcsFor = '' OR @GenerateProcsFor = @CurrentTable
            END
           
            -- update the value held in @CurrentTable
            SET @CurrentTable = @TableName
            SET @FirstColumnName = @ColumnName
            SET @FirstColumnDataType = @DataType
           
            IF @TablePrefixLength > 0 BEGIN
                  IF SUBSTRING(@CurrentTable, 1, @TablePrefixLength) = @TablePrefix BEGIN
                        --PRINT Char(13) + 'DEBUG: OBJ NAME: ' + RIGHT(@CurrentTable, LEN(@CurrentTable) - @TablePrefixLength)
                        SET @ObjectName = RIGHT(@CurrentTable, LEN(@CurrentTable) - @TablePrefixLength)
                  END ELSE BEGIN
                        SET @ObjectName = @CurrentTable
                  END
            END ELSE BEGIN
                  SET @ObjectName = @CurrentTable
            END
           
            IF @GenerateProcsFor = '' OR @GenerateProcsFor = @CurrentTable BEGIN
           
                  -- ----------------------------------------------------
                  -- now start building the procedures for the next table
                  -- ----------------------------------------------------
                 
                  -- _lst
                  SET @LIST = 'CREATE PROC [' + @SchemaName + '].[' + @ObjectName + '_GetAll]' + Char(13)
                  SET @LIST = @LIST + 'AS' + Char(13)
                  SET @LIST = @LIST + 'SET NOCOUNT ON' + Char(13)
                  IF @UseSelectWildcard = 1 BEGIN
                        SET @LIST = @LIST + Char(13) + 'SELECT * '
                  END
                  ELSE BEGIN
                        SET @LIST = @LIST + Char(13) + 'SELECT [' + @ColumnName + ']'
                  END
     
                  -- _sel
                  SET @SELECT = 'CREATE PROC [' + @SchemaName + '].[' + @ObjectName + '_GetById]' + Char(13)
                  SET @SELECT = @SELECT + Char(9) + '@' + @ColumnNameCleaned + ' ' + @DataType
                  IF @DataType IN ('varchar', 'nvarchar', 'char', 'nchar') BEGIN
                        SET @SELECT = @SELECT + '(' + CAST(@CharLength As varchar(10)) + ')'
                  END
                  SET @SELECT = @SELECT + Char(13) + 'AS' + Char(13)
                  SET @SELECT = @SELECT + 'SET NOCOUNT ON' + Char(13)
                  IF @UseSelectWildcard = 1 BEGIN
                        SET @SELECT = @SELECT + Char(13) + 'SELECT * '
                  END
                  ELSE BEGIN
                        SET @SELECT = @SELECT + Char(13) + 'SELECT [' + @ColumnName + ']'
                  END
     
                  -- _ups
                  SET @UPSERT = 'CREATE PROC [' + @SchemaName + '].[' + @ObjectName + '_Save]' + Char(13)
                             SET @UPSERT = @UPSERT + Char(13) + Char(9) + '@' + @ColumnNameCleaned + ' ' + @DataType
                  IF @DataType IN ('varchar', 'nvarchar', 'char', 'nchar') BEGIN
                        SET @UPSERT = @UPSERT + '(' + CAST(@CharLength As Varchar(10)) + ')'
                  END
     
                  -- UPDATE
                  SET @UPDATE = Char(9) + 'UPDATE ' + @TableName + ' SET ' + Char(13)
                 
                  -- INSERT -- don't add first column to insert if it is an
                  --         integer (assume autonumber)
                  SET @INSERT = Char(9) + 'INSERT INTO [' + @TableSchema + '].[' + @CurrentTable + '] (' + Char(13)
                  SET @INSERTVALUES = Char(9) + 'VALUES (' + Char(13)
                 
                  IF @FirstColumnDataType NOT IN ('int', 'bigint', 'smallint', 'tinyint')
                  BEGIN
                        SET @INSERT = @INSERT + Char(9) + Char(9) + '[' + @ColumnName + '],' + Char(13)
                        SET @INSERTVALUES = @INSERTVALUES + Char(9) + Char(9) + '@' + @ColumnNameCleaned + ',' + Char(13)
                  END
     
                  -- _del
                  SET @DELETE = 'CREATE PROC [' + @SchemaName + '].[' + @ObjectName + '_Delete]' + Char(13)
                  SET @DELETE = @DELETE + Char(9) + '@' + @ColumnNameCleaned + ' ' + @DataType
                  IF @DataType IN ('varchar', 'nvarchar', 'char', 'nchar') BEGIN
                        SET @DELETE = @DELETE + '(' + CAST(@CharLength As Varchar(10)) + ')'
                  END
                  SET @DELETE = @DELETE + Char(13) + 'AS' + Char(13)
                  SET @DELETE = @DELETE + 'SET NOCOUNT ON' + Char(13) + Char(13)
                  SET @DELETE = @DELETE + 'DELETE FROM [' + @TableSchema + '].[' + @CurrentTable + ']' + Char(13)
                  SET @DELETE = @DELETE + 'WHERE [' + @ColumnName + '] = @' + @ColumnNameCleaned + Char(13)
                  SET @DELETE = @DELETE + Char(13) + 'SET NOCOUNT OFF' + Char(13)
                  SET @DELETE = @DELETE + Char(13)
 
            END   -- end @GenerateProcsFor = '' OR @GenerateProcsFor = @CurrentTable
      END
      ELSE BEGIN
            IF @GenerateProcsFor = '' OR @GenerateProcsFor = @CurrentTable BEGIN
           
                  -- is the same table as the last row of the cursor
                  -- just append the column
                 
                  -- _lst
                  IF @UseSelectWildCard = 0 BEGIN
                        SET @LIST = @LIST + ', ' + Char(13) + Char(9) + '[' + @ColumnName + ']'
                  END
     
                  -- _sel
                  IF @UseSelectWildCard = 0 BEGIN
                        SET @SELECT = @SELECT + ', ' + Char(13) + Char(9) + '[' + @ColumnName + ']'
                  END
     
                  -- _ups
                  SET @UPSERT = @UPSERT + ',' + Char(13) + Char(9) + '@' + @ColumnNameCleaned + ' ' + @DataType
                  IF @DataType IN ('varchar', 'nvarchar', 'char', 'nchar') BEGIN
                        SET @UPSERT = @UPSERT + '(' + CAST(@CharLength As varchar(10)) + ')'
                  END
     
                  -- UPDATE
                  SET @UPDATE = @UPDATE + Char(9) + Char(9) + '[' + @ColumnName + '] = @' + @ColumnNameCleaned + ',' + Char(13)
     
                  -- INSERT
                  SET @INSERT = @INSERT + Char(9) + Char(9) + '[' + @ColumnName + '],' + Char(13)
                  SET @INSERTVALUES = @INSERTVALUES + Char(9) + Char(9) + '@' + @ColumnNameCleaned + ',' + Char(13)
     
                  -- _del
                  -- delete proc completed already
            END -- end @GenerateProcsFor = '' OR @GenerateProcsFor = @CurrentTable'
      END
 
      -- fetch next row of cursor into variables
      FETCH NEXT FROM TableCol INTO @TableSchema, @TableName, @ColumnName, @DataType, @CharLength
END
 
-- ----------------
-- clean up cursor
-- ----------------
CLOSE TableCol
DEALLOCATE TableCol
 
-- ------------------------------------------------
-- repeat the block of code from within the cursor
-- So that the last table has its procs completed
-- and printed / executed
-- ------------------------------------------------
 
-- if is the end of the last table
IF @CurrentTable <> '' BEGIN
      IF @GenerateProcsFor = '' OR @GenerateProcsFor = @CurrentTable BEGIN
 
            -- first add any syntax to end the statement
           
            -- _lst
            SET @LIST = @List + Char(13) + 'FROM ' + @CurrentTable + Char(13)
            SET @LIST = @LIST + Char(13) + Char(13) + 'SET NOCOUNT OFF' + Char(13)
            SET @LIST = @LIST + Char(13)
           
            -- _sel
            SET @SELECT = @SELECT + Char(13) + 'FROM ' + @CurrentTable + Char(13)
            SET @SELECT = @SELECT + 'WHERE [' + @FirstColumnName + '] = @' + Replace(@FirstColumnName, ' ', '') + Char(13)
            SET @SELECT = @SELECT + Char(13) + Char(13) + 'SET NOCOUNT OFF' + Char(13)
            SET @SELECT = @SELECT + Char(13)
 
 
            -- UPDATE (remove trailing comma and append the WHERE clause)
            SET @UPDATE = SUBSTRING(@UPDATE, 0, LEN(@UPDATE)- 1) + Char(13) + Char(9) + 'WHERE [' + @FirstColumnName + '] = @' + Replace(@FirstColumnName, ' ', '') + Char(13)
           
            -- INSERT
            SET @INSERT = SUBSTRING(@INSERT, 0, LEN(@INSERT) - 1) + Char(13) + Char(9) + ')' + Char(13)
            SET @INSERTVALUES = SUBSTRING(@INSERTVALUES, 0, LEN(@INSERTVALUES) -1) + Char(13) + Char(9) + ')'
            SET @INSERT = @INSERT + @INSERTVALUES

            SET @EXISTS = ''
			SET @EXISTS = @EXISTS + 'EXISTS (SELECT * FROM [' + @TableSchema + '].[' + @CurrentTable + '] WITH(NOLOCK) '
			SET @EXISTS = @EXISTS + 'WHERE [' + @FirstColumnName + '] = @' + Replace(@FirstColumnName, ' ', '') + ')'
           
            -- _ups
            SET @UPSERT = @UPSERT + Char(13) + 'AS' + Char(13)
            SET @UPSERT = @UPSERT + 'SET NOCOUNT ON' + Char(13)
            IF @FirstColumnDataType IN ('int', 'bigint', 'smallint', 'tinyint', 'float', 'decimal')
            BEGIN
                 SET @UPSERT = @UPSERT + 'IF @' + Replace(@FirstColumnName, ' ', '') + ' = 0 BEGIN' + Char(13)
            END ELSE BEGIN
                 SET @UPSERT = @UPSERT + 'IF NOT ' + @EXISTS + ' BEGIN' + Char(13)  
            END
            SET @UPSERT = @UPSERT + ISNULL(@INSERT, '') + Char(13)
            SET @UPSERT = @UPSERT + Char(9) + 'SELECT SCOPE_IDENTITY() As InsertedID' + Char(13)
            SET @UPSERT = @UPSERT + 'END' + Char(13)
            SET @UPSERT = @UPSERT + 'ELSE BEGIN' + Char(13)
            SET @UPSERT = @UPSERT + ISNULL(@UPDATE, '') + Char(13)
            SET @UPSERT = @UPSERT + 'END' + Char(13) + Char(13)
            SET @UPSERT = @UPSERT + 'SET NOCOUNT OFF' + Char(13)
            SET @UPSERT = @UPSERT + Char(13)
 
            -- _del
            -- delete proc completed already
 
            -- --------------------------------------------------
            -- now either print the SP definitions or
            -- execute the statements to create the procs
            -- --------------------------------------------------
            IF @PrintOrExecute <> 'Execute' BEGIN
                  PRINT @LIST
                  PRINT @SELECT
                  PRINT @UPSERT
                  PRINT @DELETE
            END ELSE BEGIN
                  EXEC sp_Executesql @LIST
                  EXEC sp_Executesql @SELECT
                  EXEC sp_Executesql @UPSERT
                  EXEC sp_Executesql @DELETE
            END
      END -- end @GenerateProcsFor = '' OR @GenerateProcsFor = @CurrentTable
END