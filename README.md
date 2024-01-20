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

Select "Docker-Compose" from startup project and hit F5
After run docker container, stop visual studio. 
Select "Order.Api" from startup project and hit F5 to run project or debug test methods.




# Data Migration for Product.Api And Customer.Api
Select "Product.Api" from startup project.

Open PackageManagerConsole and select "02-Application\Product.Infrastructure" project for create migration.
PackageManagerConsole : 02-Application\Product.Infrastructure

Run below commands in PackageManagerConsole

Add-Migration InitialCreate -Context ProductDbContext
Update-Database -Context ProductDbContext


Add-Migration InitialCreate -Context IntegrationEventLogContext
Update-Database -Context IntegrationEventLogContext


To connect sqlserver use "SQL Server Object Explorer" from view menu in visual studio. 
SSMS Connection
Server Name     : 127.0.0.1,5433
Authentication  : SQL Server Authentication
User Name       : sa
Password        : Pass@word


## To Run Product Api

Select "Docker-Compose" from startup project and hit F5
After run docker container, stop visual studio. 
Select "Product.Api" from startup project and hit F5 to run project or debug test methods.




# Data Migration for Customer.Api
Select "Customer.Api" from startup project.

Open PackageManagerConsole and select "02-Application\Customer.Infrastructure" project for create migration.
PackageManagerConsole : 02-Application\Customer.Infrastructure

Run below commands in PackageManagerConsole

Add-Migration InitialCreate -Context CustomerDbContext
Update-Database -Context CustomerDbContext


Add-Migration InitialCreate -Context IntegrationEventLogContext
Update-Database -Context IntegrationEventLogContext


To connect sqlserver use "SQL Server Object Explorer" from view menu in visual studio. 
SSMS Connection
Server Name     : 127.0.0.1,5433
Authentication  : SQL Server Authentication
User Name       : sa
Password        : Pass@word


## To Run Customer Api

Select "Docker-Compose" from startup project and hit F5
After run docker container, stop visual studio. 
Select "Customer.Api" from startup project and hit F5 to run project or debug test methods.