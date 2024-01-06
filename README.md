# OnlineShop
Online Shop



# Data Migration for Order.Api
Select "Order.Api" from startup project.

Open PackageManagerConsole and select "02-Application\Order.Infrastructure" project for create migration.
PackageManagerConsole : 02-Application\Order.Infrastructure

Run below commands in PackageManagerConsole

Add-Migration InitialCreate -Context OrderDbContext
Update-Database -Context OrderDbContext


Add-Migration InitialCreate -Context IntegrationEventLogContext
Update-Database -Context IntegrationEventLogContext


To connect sqlserver use "SQL Server Object Explorer" from view menu in visual studio. 
SSMS Connection
Server Name     : 127.0.0.1,5433
Authentication  : SQL Server Authentication
User Name       : sa
Password        : Pass@word


## To Run Order Api

Select "Docker-Compose" from startup project. And hit F5