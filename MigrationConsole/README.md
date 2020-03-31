﻿# FTDNA.Database.[DATABASE] Projects.
.
Projects that are named FTDNA.Database.DATABASE (where DATABASE is the name of a database)
are used to manage the SQL migration scripts for the associated database.  Such projects are intended
to support incremental migration scripts that will be sequentially applied during deployment to the associated database.

A special case is the FTDNA.Database project which is used to manage the SQL migration scripts for the FTDNA database.

Each project specifies a target database (via the app setting "DBConnectionString") and defines a console application that will identify .sql files in the project's .\Migrations folder that have not been applied to the database and then execute these files in ascending order by name (i.e., same order as displayed in Visual Studio).  Scripts with "skip!" or "donotrun" in the name will be ignored.

Migrations are created using the New-Migrations command in the Package Manager Console.  By default, migration files are placed in a subfolder corresponding to the current GIT branch.

Migrations are applied manually using the Start-Migrations command from the Package Manager Console and automatically at deployment time.

After migrations are run, the affected database object(s) (Tables, Views, Procedures, Functions, Synonyms) will be scripted (using SMO) and saved to the .\Definitions folder for version control and code review.

This application uses [DbUp](http://dbup.github.io/) under the hood.

Building upon DbUp, a Package Manager Console module is available (source is located in the nuget package FTDNA.Database.Scripter) which allows easy control of the migrations from the PMC.  This script uses the GIT command and assumes that it is available on the default system path.

## Update 03/10/2017

All the database projects use wildcard includes/excludes for migration files and they follow the conventions: 
* Include: Migrations\branch_name\migration_name.sql
* Exclude: Migrations\released_to_production\branch_name\migration_name.sql
This means you should not push changes to the csproj files. See New-Migration command below for details on how to create migrations. The command will create the folders and files that follow the above conventions.

## Demo Video

[https://drive.google.com/a/genebygene.com/file/d/0B2fVTrUVnXvbbkVtM29yTnRCVms/view?usp=sharing](https://drive.google.com/a/genebygene.com/file/d/0B2fVTrUVnXvbbkVtM29yTnRCVms/view?usp=sharing)

## Package Manager Console Commands

*    New-Migration [-Name filename] [-JournalOnly] [-ProjectName projectName] 
*    Start-Migrations [-ProjectName projectName] [-WhatIf]
*    Start-DatabaseScript 
*    Set-ConnectionStringDatabase -Name

IMPORTANT NOTES: 
- Install the GIT command line tools and add the path to GIT.EXE to the default system path.
- If ProjectName parameter is omitted the Package Manager Console's default project is used.
- The GIT command must be executable from the command line via the default PATH.  On Windows, the GIT command is installed with the GitHub Desktop Client in the user's %APPDATA%\\GitHub\\PortableGit\_* directory.  Add this directory to the PATH environment variable.  The GIT command is also available in the GIT command line tools available [here](https://git-scm.com/downloads).
- If ProjectName parameter is omitted the Package Manager Console's default project is used.

_Examples_:

*   

         New-Migration [GitBranchName] -ProjectName [VSProjectName] #Creates YYMMDDHHMMSS_[GitBranchName].sql
*    

         New-Migration -Name NewTables -ProjectName [VSProjectName] #Creates YYMMDDHHMMSS_\[GitBranchName]\_NewTables.sql

*
         Remove-Migration "xxxx_test.sql" #Remove the migration file from the location Migrations\current_branch\

*
         New-Migration "test" #Create migration in folder Migrations\current_branch with name xxxx_test.sql, the developer will have to manually reload the database project to see the file as included in the project.
         
*    
         New-Migration "test" -WithReload #Create migration in folder Migrations\current_branch with name xxxx_test.sql and prompts to reload the project.
         
*    
         New-Migration -JournalOnly -ProjectName [VSProjectName] #Create migration and records it as executed.
*    
         Start-Migrations -ProjectName [VSProjectName] #Runs all pending migrations

*    
         Start-Migrations -ProjectName [VSProjectName] -WhatIf #Runs all pending migrations in "what if" mode

*    
         Start-Migrations -Force -CurrentBranchOnly #Runs all migrations for current branch even if the migration was run before or not

*    
         Start-Migrations -Force -Branch [BranchName] #Runs all migrations for the specified branch only even if the migration was run before or not

*    
         Start-Migrations -Force -ScriptFile [FileName] #Runs the specified migration file even if was run before

*    
         Start-DatabaseScript #Scripts database definitions to the \Definitions folder

*    
         Set-ConnectionStringDatabase "FTDNA_FeatureY" #Changes DBConnectionString in app app/web.config files to use FTDNA_FeatureY database

## Edit and run .sql files in Visual Studio setup:
 
*    Right click .sql file
*    Click "Open With"
*    Select "Microsoft SQL Server Data Tools, T-SQL Editor" in the list
*    Click "Set as Default"

## Configure a new project for database migrations
1. Name the project using the naming convention defined above.
2. Configure the target database by defining DBConnectionString in the appropriate App.Config file (development, qa, etc.).
2. Open Package Manager Console.
2. Set Package Manager Console's "Default project" using the dropdown selector. 
3. Run "Install-Package SolutionScripts" command.
4. Run "Install-Package DbUp" command.
4. Run "Update-SolutionScripts" command.

## SQL Formatting

All SQL scripts should be formatted using SQL Prompt, with the [FTDNA SQL Formatting Style.sqlpromptstyle](https://github.com/genebygene/familytreedna.com/tree/master/FTDNA.Database/FTDNA SQL Formatting Style.sqlpromptstyle) template.

Ideally, when preparing to change a stored procedure or function, the script should be formatted first, then committed.  This way, logic changes will be visible in the diff of the second commit.

## Misc
If, when running a migration, you get an error that looks like the following:
```
System.IO.FileNotFoundException: Could not load file or assembly 'Microsoft.SqlServer.SqlClrProvider, Version=11.0.0.0, 
Culture=neutral, PublicKeyToken=89845dcd8080cc91' or one of its dependencies. The system cannot find the file specified.
File name: 'Microsoft.SqlServer.SqlClrProvider, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91' 
...
```
 install SharedManagementObjects.msi from G:\Software\SQLServerTools
