# Guard Clauses

Every migration script should include a guard clause that will stop the migration script making changes if it doesn't need to be run. It is dangerous to assume that a script will only be run in the right preconditions. If you don't include a guard clause, the migration script might fail or create unexpected changes in the database.

## To check that a table exists
    IF EXISTS ( SELECT  1
                FROM    information_schema.Tables
                WHERE   table_schema = 'MySchema'
                        AND TABLE_NAME = 'MyTableName' )
        PRINT 'the table exists' 
    ELSE
        PRINT 'The table isn''t there'

## To check that a view exists
    IF EXISTS ( SELECT * FROM sys.views WHERE name = 'MyViewName' )
        PRINT 'the view exists' 
    ELSE
        PRINT 'The view isn''t there'


## To check that a function exists
    IF EXISTS ( SELECT  1
                FROM    information_schema.Routines
                WHERE   ROUTINE_NAME = 'MyFunctionName'
                        AND ROUTINE_TYPE = 'FUNCTION'
                        AND ROUTINE_SCHEMA = 'MySchema' )
        PRINT 'the function exists' 
    ELSE
        PRINT 'The function isn''t there'

## To check that a procedure exists
    IF EXISTS ( SELECT  1
                FROM    information_schema.Routines
                WHERE   ROUTINE_NAME = 'MyProcedureName'
                        AND ROUTINE_TYPE = 'PROCEDURE'
                        AND ROUTINE_SCHEMA = 'MySchema' )
        PRINT 'the procedure exists' 
    ELSE
        PRINT 'The procedure isn''t there'

## To check that a particular column exists in a table exists
    IF EXISTS ( SELECT  1
                FROM    information_schema.COLUMNS
                WHERE   table_schema = 'MySchema'
                        AND TABLE_NAME = 'MyTableName'
                        AND column_Name = 'MyColumnName' )
        PRINT 'the colmun exists' 
    ELSE
        PRINT 'The column isn''t there'

## To check whether a column has any sort of constraint
    IF EXISTS ( SELECT  1
                FROM    information_schema.CONSTRAINT_column_USAGE
                WHERE   table_schema = 'MySchema'
                        AND TABLE_NAME = 'MyTableName'
                        AND column_Name = 'MyColumnName' )
        PRINT 'there is a constraint on the column' 
    ELSE
        PRINT 'no constraint'

## To check that a column has a check constraint
    IF EXISTS ( SELECT  1
                FROM    information_schema.CONSTRAINT_column_USAGE CCU
                        INNER JOIN information_schema.CHECK_CONSTRAINTS CC ON CC.constraint_name = CCU.constraint_NAME
                WHERE   table_schema = 'MySchema'
                        AND TABLE_NAME = 'MyTableNames'
                        AND column_Name = 'MyColumnName' )
        PRINT 'there is a check constraint on the column' 
    ELSE
        PRINT 'no check constraint'

## To check that a column has a foreign key constraint
    IF EXISTS ( SELECT  1
                FROM    information_schema.CONSTRAINT_column_USAGE CCU
                        INNER JOIN information_schema.REFERENTIAL_CONSTRAINTS CC ON CC.constraint_name = CCU.constraint_NAME
                WHERE   table_schema = 'MySchema'
                        AND TABLE_NAME = 'MyTableName'
                        AND column_Name = 'MyColumnName' )
        PRINT 'there is a referential constraint on the column' 
    ELSE
        PRINT 'no referential constraint'

## To check that a column participates in a primary key
    IF EXISTS ( SELECT  1
                FROM    information_schema.CONSTRAINT_column_USAGE CCU
                        INNER JOIN information_schema.TABLE_CONSTRAINTS CC ON CC.constraint_name = CCU.constraint_NAME
                WHERE   CCU.table_schema = 'MySchema'
                        AND CCU.TABLE_NAME = 'MyTableName'
                        AND column_Name = 'MyColumnName'
                        AND constraint_Type = 'PRIMARY KEY' )
        PRINT 'the column is involved in a primary key' 
    ELSE
        PRINT 'not involved in a primary key'

## To check if an index exists on a table
    IF EXISTS ( SELECT  1
                FROM    sys.indexes 
                WHERE   name='YourIndexName'
                        AND object_id = OBJECT_ID('YourTableName'))
        PRINT 'the column is involved in a primary key' 
    ELSE
        PRINT 'not involved in a primary key'

## To check if a user defined type exists
    IF EXISTS ( SELECT  1
                FROM    sys.types 
                WHERE   is_user_defined = 1
                	AND name='YourTypeName')
        PRINT 'the type exists' 
    ELSE
        PRINT 'the type does not exist'
        
Reference: [Using Migration Scripts in Database Deployments](https://www.simple-talk.com/sql/database-administration/using-migration-scripts-in-database-deployments/)
