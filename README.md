# MyIBuySpy

I'm using the tailspin IBuySpy to get updated on some skills like Angular etc

This codeplex link _probably_ doesnt work, but the source archive is in this repo  
<https://archive.codeplex.com/?p=tailspinspyworks>

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

### Setup Sqlite

NB: SqliteManager doesnt seem to like the db... will investigate later

Setup DbUp

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

### Scaffold IBuySpy Commercve database

Links

- <https://docs.microsoft.com/en-us/ef/core/managing-schemas/scaffolding?tabs=dotnet-core-cli>
- <https://docs.microsoft.com/en-us/ef/core/cli/dotnet>
- <https://docs.microsoft.com/en-us/ef/core/cli/powershell>

Add new Commerce project with `FinnAngelo.MyIBuySpy.Commerce` assemblyname and namespace

```powershell

Add-Package Microsoft.EntityFrameworkCore.Design
Add-Package Microsoft.EntityFrameworkCore.Sqlite
Add-Package Microsoft.EntityFrameworkCore.SqlServer

Scaffold-DbContext "Data Source=localhost;Initial Catalog=Commerce;User ID=sa;Password=Password_01;Connect Timeout=30;" `
-Context CommerceDbContext `
-ContextDir Data -OutputDir Models `
Microsoft.EntityFrameworkCore.SqlServer

dotnet ef dbcontext scaffold "Data Source=localhost;Initial Catalog=Commerce;User ID=sa;Password=Password_01;Connect Timeout=30;" Microsoft.EntityFrameworkCore.SqlServer --context-dir Data --output-dir Models --namespace FinnAngelo.MyIBuySpy.AngUI

# Then move the files into the AngUI project and do the `Add-Migration` and `Script-Migration` for sqlite
```


## Install Docker Desktop

- https://community.chocolatey.org/packages/docker-desktop

```powershell
choco install wsl2 -y
choco install docker-desktop -y
```


## Setup TailspinIbuySpy

### Setup SqlServer

I'm using a docker instance

```powershell
Enable-WindowsOptionalFeature -Online -FeatureName $("Microsoft-Hyper-V", "Containers") -All
docker pull microsoft/mssql-server-windows-developer
docker run --rm --name SQLServer -d -p 1433:1433 -e sa_password=Password_01 -e ACCEPT_EULA=Y -v C:/GIT/Data:C:/Data microsoft/mssql-server-windows-developer
```

Attach DB files to a new DB

```sql
USE [master]
GO
CREATE DATABASE ASPNETDB ON 
( FILENAME = N'C:\data\aspnetdb.mdf' ),
( FILENAME = N'C:\data\aspnetdb_log.ldf' )
 FOR ATTACH
GO

USE [master]
GO
CREATE DATABASE Commerce ON 
( FILENAME = N'C:\data\Commerce.mdf' ),
( FILENAME = N'C:\data\Commerce_log.ldf' )
 FOR ATTACH
GO
```

Update DB Connections in `web.config`

```xml
<connectionStrings>
  <add name="ApplicationServices" connectionString="Data Source=localhost;Initial Catalog=ASPNETDB;User ID=sa;Password=Password_01;Connect Timeout=30;" providerName="System.Data.SqlClient" />
  <add name="CommerceEntities" connectionString="metadata=res://*/Data_Access.EDM_Commerce.csdl|res://*/Data_Access.EDM_Commerce.ssdl|res://*/Data_Access.EDM_Commerce.msl;provider=System.Data.SqlClient;provider connection string=&quot;Data Source=localhost;Initial Catalog=Commerce;User ID=sa;Password=Password_01;Connect Timeout=30;&quot;" providerName="System.Data.EntityClient" />
</connectionStrings>
```

Hunt down AjaxControlToolkit

```powershell
Install-Package AjaxControlToolkit -Version 4.1.50731
```

# Setup Angular Project

Just the bog standard Angular setup in Visual Studio, with standard individual accounts for authenticaton

## Setup so uses Sqlite