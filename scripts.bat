sqlcmd -S (localdb)\MSSQLLocalDB -Q "IF EXISTS(select * from sys.databases where name='RP16-SeriousGame') DROP DATABASE [RP16-SeriousGame]"
dnu restore
exit()