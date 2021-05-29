# Setup IBuySpy

I'm using the tailspin IBuySpy to get updated on some skills like Angular etc

This codeplex link _probably_ doesnt work, but the source archive is in this repo  
<https://archive.codeplex.com/?p=tailspinspyworks>

## TLDR;

- [Database setup](#database-setup)
- [Setup TailSpin proj](#setup-tailspin-proj)
- [Hit F5](#hit-f5)

## Database Setup

**I'm using a really bad password - literally the one from the online tut for Docker SqlServer.  
_This is insanely bad practice._**

### Install Docker Desktop

- https://community.chocolatey.org/packages/docker-desktop

```powershell
choco install wsl2 -y
choco install docker-desktop -y
choco install sql-server-management-studio -y
```

### Setup SqlServer

I'm using a docker instance using linux containers - lots faster than windows!

- <https://docs.microsoft.com/en-us/sql/linux/quickstart-install-connect-docker?view=sql-server-ver15&pivots=cs1-powershell>

```powershell
docker pull mcr.microsoft.com/mssql/server
docker run --rm --name SQLServer -h SQLServer -d -p 1433:1433 -e "SA_PASSWORD=Password_01" -e "ACCEPT_EULA=Y" -v C:/GIT/Data:/Data mcr.microsoft.com/mssql/server
```

### Setup TailspinIbuySpy Databases

1. Copy the mdf/ldf files to your docker shared volume folder  
   i.e. `C:/GIT/Data` in the instance above
2. Open SqlServer Management Studio with `localhost` as the server.  
   Or You can use `.`, but it just seems messier in connection strings
3. Attach DB files to a new DB  
   ```sql
   USE [master]
   GO
   CREATE DATABASE [ASPNETDB] ON 
   ( FILENAME = N'/Data/aspnetdb.mdf' ),
   ( FILENAME = N'/Data/aspnetdb_log.ldf' )
    FOR ATTACH
   GO
   
   USE [master]
   GO
   CREATE DATABASE [Commerce] ON 
   ( FILENAME = N'/Data/Commerce.mdf' ),
   ( FILENAME = N'/Data/Commerce_log.ldf' )
    FOR ATTACH
   GO
   ```

## Setup TailSpin proj

Create a blank solution and add the IBuySpy project to it

Update DB Connections in `web.config`  

```xml
<connectionStrings>
  <add name="ApplicationServices" connectionString="Data Source=localhost;Initial    Catalog=ASPNETDB;User ID=sa;Password=Password_01;Connect Timeout=30;"    providerName="System.Data.SqlClient" />
  <add name="CommerceEntities" connectionString="metadata=res://*/Data_Access.EDM_Commerce.   csdl|res://*/Data_Access.EDM_Commerce.ssdl|res://*/Data_Access.EDM_Commerce.msl;   provider=System.Data.SqlClient;provider connection string=&quot;Data Source=localhost;   Initial Catalog=Commerce;User ID=sa;Password=Password_01;Connect Timeout=30;&quot;"    providerName="System.Data.EntityClient" />
</connectionStrings>
```

Hunt down AjaxControlToolkit

```powershell
Install-Package AjaxControlToolkit -Version 4.1.50731
```

## Hit F5

It seems to work!
