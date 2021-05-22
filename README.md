# MyIBuySpy

I'm using the tailspin IBuySpy to get updated on some skills like Angular etc

This codeplex link _probably_ doesnt work, but the source archive is in this repo  
<https://archive.codeplex.com/?p=tailspinspyworks>

## TLDR;

- [Setup TailspinIbuySpy](#setup-tailspinibuyspy)
- [Setup Angular Project](#setup-angular-project)

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