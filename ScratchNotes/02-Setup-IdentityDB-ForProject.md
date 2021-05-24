# MyIBuySpy

This is to run a VisualStudio Angular project with Identity Authenticaton on In-Memory Sqlite.

It's using the solution from the previous scratch notes

## TLDR

- [Setup Angular Project](#setup-angular-project)
- [Setup Identity Sqlite](#setup-identity-sqlite)
- [Setup DbUp](#setup-dbup)
- [Scripting Migrations](#scripting-migrations)
- [Testing](#testing)

## Setup Angular Project

Just add the bog standard Aspnet Angular project in Visual Studio, with standard individual accounts for authenticaton

## Setup Identity Sqlite

NB: SqliteManager doesnt seem to like the db... will investigate later

1. In the csproj file, replace the `MS.EFCore.SqlServer` package reference with `Microsoft.EntityFrameworkCore.Sqlite`
2. Replace the appsettings DefaultConnection with `Data Source=Sharable;Mode=Memory;Cache=Shared`
3. Install Sqlite
   ```powershell
    Install-Package Microsoft.EntityFrameworkCore.Sqlite
   ```
4. Inside Startup.cs > ConfigureServices
   ```csharp
   // InMemory Sqlite needs to be kept open or its deleted
   string connectionString = Configuration.GetConnectionString("DefaultConnection");
   var keepAliveConnection = new SqliteConnection(connectionString);
   keepAliveConnection.Open();

   // Put DbUp bit here
      
   // DbContext needs the keep-alive connection
   services.AddDbContext<ApplicationDbContext>(options => 
     options.UseSqlite(keepAliveConnection)
     );
   ```

## Setup DbUp

Note: The MS docs recommend _against_ using their migration to deploy to prod, so lets use DbUp

- <https://dbup.readthedocs.io/en/latest/>

1. Install DbUp - this includes `DbUp-core`
   ```powershell
    Install-Package DbUp-SQLite
   ```
2. Inside Startup.cs > ConfigureServices
   ```csharp
   // Setup connection string bit from above

   // DbUp install/upgrade DB on startup
   //EnsureDatabase.For.SQLiteDatabase(connectionString);
   var sharedConnection = new SharedConnection(keepAliveConnection);
   var upgrader =
    DeployChanges.To
    .SQLiteDatabase(sharedConnection)
    .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
    .LogToConsole()
    .Build();
   
   var result = upgrader.PerformUpgrade();
   
   if (!result.Successful)
   {
     throw result.Error;
   }

   // AddDbContext bit from above

   ```

## Scripting Migrations

- <https://docs.microsoft.com/en-us/dotnet/standard/data/sqlite/connection-strings>
- <https://www.devart.com/dotconnect/sqlite/docs/Identity-3-Tutorial.html>
- [EntityFrameworkCore Migrations reference](https://docs.microsoft.com/en-us/ef/core/cli/powershell)
- [Sqlite inmemory](https://stackoverflow.com/questions/56319638/entityframeworkcore-sqlite-in-memory-db-tables-are-not-created)

1. In the Solution Explorer open the Data\Migrations folder and delete all its content.
2. Execute the following command in the Package Manager Console:
   ```powershell
     Add-Migration CreateIdentitySchema -OutputDir Data\Migrations
   ```
3. Execute the following command in the Package Manager Console:
   ```powershell
   Script-Migration -Out AngUI\Data\Scripts\001_CreateIdentitySchema.sql
   ```
4. Make sure all the sripts are embedded resources:
   ```xml
   <ItemGroup>
    <EmbeddedResource Include="Data\Scripts\*.*" />
   </ItemGroup>
   ```

## Testing

- Hit F5 to run
- Register a user and it shouldnt get a 'No DB' error
- log in - Yay!
