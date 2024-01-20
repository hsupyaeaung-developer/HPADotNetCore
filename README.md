

Scaffold-DbContext "Server=DESKTOP-HDNAOSB\SQL2022;Database=TestDb;User ID=sa;Password=sa@123;Trusted_Connection=True;Trust Server Certificate=True;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models -Context AppDbContext -Tables Tbl_Blog

https://github.com/reactiveui/refit
https://reactiveui.github.io/refit/
https://www.ezzylearning.net/wp-content/uploads/ASP.NET-Core-Service-Lifetime-Infographic.png
https://www.c-sharpcorner.com/UploadFile/85ed7a/dependency-injection-in-C-Sharp/
https://datatables.net/


![Alt text](https://www.ezzylearning.net/wp-content/uploads/ASP.NET-Core-Service-Lifetime-Infographic.png)

https://learn.microsoft.com/en-us/ef/core/providers/?tabs=dotnet-core-cli
https://javabydeveloper.com/wp-content/uploads/2020/02/ORM-object-relational-mapping.png
https://stackoverflow.com/questions/74451415/asp-net-core-7-0-error-on-login-the-certificate-chain-was-issued-by-an-authorit

.NET     => https://www.nuget.org/
Frontend => https://www.npmjs.com/
Flutter  => https://pub.dev/
Java     => https://mvnrepository.com/

Pull
Commit (local commit)
Pull (server update or not?)

```
- Console App

- Ado.Net
- Dapper 
- EF
RepoDB

- Postman

- Asp.Net Core Web Api (Rest Api) 
	- EF
	- Ado.Net
	- Dapper 

- Api Call [Console]
	- HttpClient
	- RestClient
	Refit

Asp.Net Core MVC
	Ado.Net
	EF
	Dapper 
Api Call [MVC]
	HttpClient
	RestClient
	Refit
Minimal Api
Text Logging
Db Logging

Chart [ApexChart, ChartJs, HighCharts, CanvasJS]

SignalR - (Insert Data => UpdateChart, Chat Message)
UI Design
Blazor CRUD [Server, WASM]
Deploy WASM
Deploy on IIS

Middleware For MVC

GraphQL
gRPC
```

https://dillinger.io/

https://www.javatpoint.com/ado-net-tutorial
> ADO.NET is a module of .Net Framework which is used to establish connection between application and data sources. Data sources can be such as SQL Server and XML. ADO.NET consists of classes that can be used to connect, retrieve, insert and delete data.

https://www.microsoft.com/en-us/sql-server/sql-server-downloads

https://medium.com/@buttertechn/qa-testing-what-is-dev-sit-uat-prod-ac97965ce4f

DEV  — Development [Software developer]
SIT  — System Integration Test [Software developer and QA engineer]
UAT  — User Acceptance Test [Client]
PROD — Production [Public user]

```
select @@version

--CRUD
-- Create, Read, Update, Delete
-- READ
select * from tbl_blog


INSERT INTO [dbo].[Tbl_Blog]
           ([Blog_Title]
           ,[Blog_Author]
           ,[Blog_Content])
     VALUES
           ('test title'
           ,'test author'
           ,'test author')

select * from tbl_blog where Blog_Id = 1

UPDATE [dbo].[Tbl_Blog]
   SET [Blog_Title] = 'test title 1'
      ,[Blog_Author] = 'test author 2'
      ,[Blog_Content] = 'test author 3'
 WHERE Blog_Id = 1

DELETE FROM [dbo].[Tbl_Blog]
      WHERE Blog_Id = 1

-- Product = 1, Price = 1000
-- 100
-- Voucher 
-- Soft Delete

-- 1
-- 
-- 3
-- 4

select 1000 * 100

-- 2
```