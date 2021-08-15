
## Creating a Model for an Existing Database in Entity Framework Core
Use Scaffold-DbContext to create a model based on your existing database. The following parameters can be specified with Scaffold-DbContext in Package Manager Console:

Scaffold-DbContext [-Connection] [-Provider] [-OutputDir] [-Context] [-Schemas>] [-Tables>] 
                    [-DataAnnotations] [-Force] [-Project] [-StartupProject] [<CommonParameters>]
In Visual Studio, select menu Tools -> NuGet Package Manger -> Package Manger Console and run the following command:

Scaffold-DbContext "Server=.\SQLExpress;Database=weatherApp;Trusted_Connection=True;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models