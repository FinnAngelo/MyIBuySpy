# MyIBuySpy

I'm using the tailspin IBuySpy to get updated on some skills like Angular etc

## TLDR;

- [Setup TailspinIbuySpy](#setup-tailspinibuyspy)
- [Setup Angular Project](#setup-angular-project)

## Setup Angular Project

Walkthru

- Create Github project
- Open in VS
- Create blank solution
- Add aspnet site, basic auth, docker, razor compile
- Install docker-desktop
- Setup Sqlite
- Setup TailspinSpyWorks with Docker SqlServer
- Setup Areas > Commerce
 - Add API Controller with actions, using EntityFramework
 - 

## Integration Testing



## Setup Commerce DB

Links

- <https://docs.microsoft.com/en-us/ef/core/managing-schemas/scaffolding?tabs=dotnet-core-cli>
- <https://docs.microsoft.com/en-us/ef/core/cli/dotnet>
- <https://docs.microsoft.com/en-us/ef/core/cli/powershell>


### Add Commerce Area

- Add Folder called `Areas`
- Rt-click and add Areas called `Commerce`
- Add to Startup class:
  ```csharp
  app.UseEndpoints(endpoints =>
  {
    endpoints.MapControllerRoute(
      name : "areas",
      pattern : "{area:exists}/{controller=Home}/{action=Index}/{id?}"
    );
  });
  ```

Add new CommerceConnection to connectionstrings in appsettings.json:  
```json
"CommerceConnection": "Data Source=localhost;Initial Catalog=Commerce;User ID=sa;Password=Password_01;Connect Timeout=30;",
```

### Scaffold CommerceDbContext

```powershell
Install-Package Microsoft.EntityFrameworkCore.Design
Install-Package Microsoft.EntityFrameworkCore.Sqlite
Install-Package Microsoft.EntityFrameworkCore.SqlServer

# Because aspnet can use connectionstring - note is SqlServer because scaffolding from existing
Scaffold-DbContext -Connection "name=CommerceConnection" -Provider Microsoft.EntityFrameworkCore.SqlServer -Context CommerceDbContext -ContextDir Areas\Commerce\Data -OutputDir Areas\Commerce\Models
```

Add API Controllers with actions, using EntityFramework

- Yeah; just rightclick the folder and add
- The views are a bit messier as no primary key
  - I want to get these to be EFCore queries so that Sqlite can use them too

## Setup Identity Sqlite

NB: SqliteManager doesnt seem to like the db... will investigate later

### Setup DbUp

- <https://dbup.readthedocs.io/en/latest/>

1. Replace the `MS.EFCore.SqlServer` package with `Microsoft.EntityFrameworkCore.Sqlite`
2. Replace the appsettings DefaultConnection with `Data Source=Sharable;Mode=Memory;Cache=Shared`
3. Install DbUp
    ```powershell
    Install-Package DbUp-SQLite
    ```
4. Inside Startup.cs > ConfigureServices
	```csharp
	// InMemory Sqlite needs to be kept open or its deleted
	string connectionString = Configuration.GetConnectionString("DefaultConnection");
	var keepAliveConnection = new SqliteConnection(connectionString);
	keepAliveConnection.Open();

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

	// DbContext (duh!) for Identity
	services.AddDbContext<ApplicationDbContext>(options => 
		options.UseSqlite(keepAliveConnection)
		);
	```

Scripting Migrations

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

## Install Docker Desktop

- https://community.chocolatey.org/packages/docker-desktop

```powershell
choco install wsl2 -y
choco install docker-desktop -y
```

# Setup Angular Project

Just the bog standard Angular setup in Visual Studio, with standard individual accounts for authenticaton

## Setup so uses Sqlite