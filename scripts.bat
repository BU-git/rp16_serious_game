sqlcmd -S (localdb)\MSSQLLocalDB -Q "IF EXISTS(select * from sys.databases where name='RP16-SeriousGame') DROP DATABASE [RP16-SeriousGame]"
cd .\src\WebUI
dnx ef migrations add LatestStableMigration -p DAL
dnx ef database update -p DAL
cd .\..\..
dnu restore
exit()