# com-danliris-service-finance-accounting

[![codecov](https://codecov.io/gh/danliris/com-danliris-service-finance-accounting/branch/dev/graph/badge.svg)](https://codecov.io/gh/danliris/com-danliris-service-finance-accounting) [![Build Status](https://travis-ci.com/danliris/com-danliris-service-finance-accounting.svg?branch=dev)](https://travis-ci.com/danliris/com-danliris-service-finance-accounting) [![Maintainability](https://api.codeclimate.com/v1/badges/937e40c506282348a416/maintainability)](https://codeclimate.com/github/danliris/com-danliris-service-finance-accounting/maintainability)


DanLiris Application is a enterprise project that aims to manage the business processes of a textile factory, PT. DanLiris.
This application is a microservices application consisting of services based on .NET Core and Aurelia Js which part of  NodeJS Frontend Framework. This application show how to implement microservice architecture principles. com-danliris-service-finance-accounting repository is part of service that will serve  finance accounting business activity.

## Prerequisites
* Windows, Mac or Linux
* [Visual Studio Code](https://code.visualstudio.com/) or [Visual Studio](https://visualstudio.microsoft.com/vs/whatsnew/)
* [IIS Web Server](https://www.iis.net/) 
* [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
* [.NET Core SDK](https://www.microsoft.com/net/download/core#/current) (v2.0.9,  SDK 2.1.202, ASP.NET Core Runtime 2.0.9 )


## Getting Started

- Fork the repository and then clone the repository using command  `git clone https://github/YOUR-USERNAME/com-danliris-service-finance-accounting.git`  checkout the `dev` branch.


### Command Line

- Install the latest version of the .NET Core SDK from this page <https://www.microsoft.com/net/download/core>
- Next, navigate to root project or wherever your folder is on the command line in administrator mode.
- Create empty database.
- Setting connection to database using Connection Strings in appsettings.json. Your appsettings.json look like this:

```
{
  "Logging": {
    "IncludeScopes": false,
    "Debug": {
      "LogLevel": {
        "Default": "Warning"
      }
    },
    "Console": {
      "LogLevel": {
        "Default": "Warning"
      }
    }
  },

  "ConnectionStrings": {
    "DefaultConnection": "Server=YourDbServer;Database=com-danliris-service-finance-accounting;Trusted_Connection=True;MultipleActiveResultSets=true",
  },
  "ClientId": "your ClientId",
  "Secret": "Your Secret",
  "ASPNETCORE_ENVIRONMENT": "Development"
}
```
and  Your appsettings.Developtment.json look like this :
```
{
  "Logging": {
    "IncludeScopes": false,
    "LogLevel": {
      "Default": "Debug",
      "System": "Information",
      "Microsoft": "Information"
    }
  }
}
```
- Make sure port application has no conflict, setting port application in launchSettings.json
```
com-danliris-service-finance-accounting
 ┣ Com.Danliris.Service.Finance.Accounting.WebApi
    ┗ Properties
       ┗ launchSettings.json
```

file launchSettings.json look like this :
```
{
  "iisSettings": {
    "windowsAuthentication": false,
    "anonymousAuthentication": true,
    "iisExpress": {
      "applicationUrl": "http://localhost:52409/",
      "sslPort": 0
    }
  },
  "profiles": {
    "IIS Express": {
      "commandName": "IISExpress",
      "launchBrowser": true,
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      }
    },
    "Com.Danliris.Service.Finance.Accounting.WebApi": {
      "commandName": "Project",
      "launchBrowser": true,
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      },
      "applicationUrl": "http://localhost:5000"
    }
  }
} 
```
- Call `dotnet run`.
- Then open the `http://localhost:52409` URL in your browser.

### Visual Studio

- Download Visual Studio 2019 (any edition) from https://www.visualstudio.com/downloads/ .
- Open `Com.Danliris.Service.Finance.Accounting.sln` and wait for Visual Studio to restore all Nuget packages.
- Create empty database.
- Setting connection to database using Connection Strings in appsettings.json. Your appsettings.json look like this:

```
{
  "Logging": {
    "IncludeScopes": false,
    "Debug": {
      "LogLevel": {
        "Default": "Warning"
      }
    },
    "Console": {
      "LogLevel": {
        "Default": "Warning"
      }
    }
  },

  "ConnectionStrings": {
    "DefaultConnection": "Server=YourDbServer;Database=your_parent_database;Trusted_Connection=True;MultipleActiveResultSets=true",
  },
  "ClientId": "your ClientId",
  "Secret": "Your Secret",
  "ASPNETCORE_ENVIRONMENT": "Development"
}
```
and  Your appsettings.Developtment.json look like this :
```
{
  "Logging": {
    "IncludeScopes": false,
    "LogLevel": {
      "Default": "Debug",
      "System": "Information",
      "Microsoft": "Information"
    }
  }
}
```
- Make sure port application has no conflict, setting port application in launchSettings.json.
```
com-danliris-service-finance-accounting
 ┣ Com.Danliris.Service.Finance.Accounting.WebApi
    ┗ Properties
       ┗ launchSettings.json
```
file launchSettings.json look like this :
```
{
  "iisSettings": {
    "windowsAuthentication": false,
    "anonymousAuthentication": true,
    "iisExpress": {
      "applicationUrl": "http://localhost:52409/",
      "sslPort": 0
    }
  },
  "profiles": {
    "IIS Express": {
      "commandName": "IISExpress",
      "launchBrowser": true,
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      }
    },
    "Com.Danliris.Service.Finance.Accounting.WebApi": {
      "commandName": "Project",
      "launchBrowser": true,
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      },
      "applicationUrl": "http://localhost:5000"
    }
  }
} 
```

- Ensure `Com.Danliris.Service.Finance.Accounting.WebApi` is the startup project and run it and the browser will launched in new tab http://localhost:5000/swagger/index.html


### Run Unit Tests in Visual Studio 
1. You can run all test suite, specific test suite or specific test case on test explorer.
2. Choose Tab Menu **Test** to select differnt menu test.
3. Select **Run All Test** or press (Ctrl + R, A ) to run all test suite.
4. Select **Test Explorer** or press (Ctrl + E, T ) to determine  test suite to run specifically.
5. Select **Analyze Code Coverage For All Test** to generate code coverage. 


## Knows More Details
### Root directory and description

```
com-danliris-service-finance-accounting
 ┣ Com.Danliris.Service.Finance.Accounting.Lib
 ┣ Com.Danliris.Service.Finance.Accounting.Test
 ┣ Com.Danliris.Service.Finance.Accounting.WebApi
 ┣ TestResults
 ┣ .codecov.yml
 ┣ .gitignore
 ┣ .travis.yml
 ┣ Com.Danliris.Service.Finance.Accounting.sln
 ┗ README.md
 ```

**1. Com.Danliris.Service.Finance.Accounting.Lib**

This folder consists of various libraries, domain Models, View Models, and Business Logic.The Model and View Models represents the data structure. Business Logic has responsibility  to organize, prepare, manipulate, and organize data. The tasks are include entering data into databases, updating data, deleting data, and so on. The model carries out its work based on instructions from the controller.


AutoMapperProfiles:

- Colecction class to setup mapping data 

BusinessLogic

- Colecction of classes to prepare, manipulate, and organize data, including CRUD (Create, Read, Update, Delete ) on database.

Models:

- The Model is a collection of objects that Representation of data structure which hold the application data and it may contain the associated business logic.

ViewModels

- The View Model refers to the objects which hold the data that needs to be shown to the user.The View Model is related to the presentation layer of our application. They are defined based on how the data is presented to the user rather than how they are stored.

ModelConfigs

- Collection of classes to setup entity model  that will be used in EF framework to generate schema database.

Migrations

- Collection of classes that generated by EF framework  to setup database and the tables.


PdfTemplates

- Collection of classes to generate report in pdf format.


Helpers 

- Collection of helper classes that frequently used in various cases. 


Utilities

- Collection of classes that frequently used as utility in various class. 

Services

- Collection of classes and interfaces to validation and authentication user.


The folder tree in this folder is:

```
com-danliris-service-finance-accounting
 ┣ Com.Danliris.Service.Finance.Accounting.Lib
 ┃ ┣ AutoMapperProfiles
 ┃ ┃ ┣ DailyBankTransaction
 ┃ ┃ ┣ DownPayment
 ┃ ┃ ┣ JournalTransaction
 ┃ ┃ ┣ LockTransaction
 ┃ ┃ ┣ Master
 ┃ ┃ ┣ Memo
 ┃ ┃ ┣ PaymentDispositionNote
 ┃ ┃ ┣ PurchasingDispositionExpedition
 ┃ ┃ ┣ RealizationVBNonPO
 ┃ ┃ ┣ RealizationVBWithPO
 ┃ ┃ ┣ SalesReceipt
 ┃ ┃ ┣ VBNonPORequest
 ┃ ┃ ┗ VBWithPORequest
 ┃ ┣ bin
 ┃ ┃ ┗ Debug
 ┃ ┃ ┃ ┗ netcoreapp2.0
 ┃ ┣ BusinessLogic
 ┃ ┃ ┣ Interfaces
 ┃ ┃ ┃ ┣ CashierApproval
 ┃ ┃ ┃ ┣ ClearaceVB
 ┃ ┃ ┃ ┣ CreditBalance
 ┃ ┃ ┃ ┣ CreditorAccount
 ┃ ┃ ┃ ┣ DailyBankTransaction
 ┃ ┃ ┃ ┣ JournalTransaction
 ┃ ┃ ┃ ┣ LockTransaction
 ┃ ┃ ┃ ┣ Master
 ┃ ┃ ┃ ┣ PaymentDispositionNote
 ┃ ┃ ┃ ┣ PaymentDispositionNotVerifiedReport
 ┃ ┃ ┃ ┣ PurchasingDispositionExpedition
 ┃ ┃ ┃ ┣ SalesReceipt
 ┃ ┃ ┃ ┣ VBExpeditionRealizationReport
 ┃ ┃ ┃ ┣ VBRequestAll
 ┃ ┃ ┃ ┗ VBStatusReport
 ┃ ┃ ┣ Services
 ┃ ┃ ┃ ┣ CashierApproval
 ┃ ┃ ┃ ┣ ClearaceVB
 ┃ ┃ ┃ ┣ CreditBalance
 ┃ ┃ ┃ ┣ CreditorAccount
 ┃ ┃ ┃ ┣ DailyBankTransaction
 ┃ ┃ ┃ ┣ DownPayment
 ┃ ┃ ┃ ┣ JournalTransaction
 ┃ ┃ ┃ ┣ LockTransaction
 ┃ ┃ ┃ ┣ Master
 ┃ ┃ ┃ ┣ Memo
 ┃ ┃ ┃ ┣ OthersExpenditureProofDocument
 ┃ ┃ ┃ ┣ PaymentDispositionNote
 ┃ ┃ ┃ ┣ PaymentDispositionNotVerifiedReport
 ┃ ┃ ┃ ┣ PurchasingDispositionExpedition
 ┃ ┃ ┃ ┣ RealizationVBNonPO
 ┃ ┃ ┃ ┣ RealizationVBWIthPO
 ┃ ┃ ┃ ┣ SalesReceipt
 ┃ ┃ ┃ ┣ VBExpeditionRealizationReport
 ┃ ┃ ┃ ┣ VbNonPORequest
 ┃ ┃ ┃ ┣ VBRequestAll
 ┃ ┃ ┃ ┣ VBStatusReport
 ┃ ┃ ┃ ┣ VBVerification
 ┃ ┃ ┃ ┗ VbWIthPORequest
 ┃ ┃ ┗ VBRealizationDocumentExpedition
 ┃ ┣ Enums
 ┃ ┃ ┣ Expedition
 ┃ ┃ ┣ JournalTransaction
 ┃ ┃ ┗ Master
 ┃ ┣ Helpers
 ┃ ┣ Migrations
 ┃ ┣ Models
 ┃ ┃ ┣ CreditorAccount
 ┃ ┃ ┣ DailyBankTransaction
 ┃ ┃ ┣ DownPayment
 ┃ ┃ ┣ JournalTransaction
 ┃ ┃ ┣ LockTransaction
 ┃ ┃ ┣ Master
 ┃ ┃ ┃ ┗ COA
 ┃ ┃ ┣ Memo
 ┃ ┃ ┣ OthersExpenditureProofDocument
 ┃ ┃ ┣ PaymentDispositionNote
 ┃ ┃ ┣ PurchasingDispositionExpedition
 ┃ ┃ ┣ RealizationVB
 ┃ ┃ ┣ SalesReceipt
 ┃ ┃ ┣ VbNonPORequest
 ┃ ┃ ┗ VBRealizationDocumentExpedition
 ┃ ┣ obj
 ┃ ┃ ┣ Debug
 ┃ ┃ ┃ ┗ netcoreapp2.0
 ┃ ┣ PDFTemplates
 ┃ ┣ Services
 ┃ ┃ ┣ BasicUploadCsvService
 ┃ ┃ ┣ HttpClientService
 ┃ ┃ ┣ IdentityService
 ┃ ┃ ┗ ValidateService
 ┃ ┣ Utilities
 ┃ ┣ ViewModels
 ┃ ┃ ┣ CashierApproval
 ┃ ┃ ┣ ClearaceVB
 ┃ ┃ ┣ CreditBalance
 ┃ ┃ ┣ CreditorAccount
 ┃ ┃ ┣ DailyBankTransaction
 ┃ ┃ ┣ DownPayment
 ┃ ┃ ┣ IntegrationViewModel
 ┃ ┃ ┣ JournalTransaction
 ┃ ┃ ┣ LockTransaction
 ┃ ┃ ┣ Master
 ┃ ┃ ┃ ┗ COA
 ┃ ┃ ┣ Memo
 ┃ ┃ ┣ NewIntegrationViewModel
 ┃ ┃ ┣ OthersExpenditureProofDocumentViewModels
 ┃ ┃ ┣ PaymentDispositionNoteViewModel
 ┃ ┃ ┣ PaymentDispositionNotVerifiedReportViewModel
 ┃ ┃ ┣ PurchasingDispositionAcceptance
 ┃ ┃ ┣ PurchasingDispositionExpedition
 ┃ ┃ ┣ PurchasingDispositionReport
 ┃ ┃ ┣ PurchasingDispositionVerification
 ┃ ┃ ┣ RealizationVBNonPO
 ┃ ┃ ┣ RealizationVBWIthPO
 ┃ ┃ ┣ SalesReceipt
 ┃ ┃ ┣ VBExpeditionRealizationReport
 ┃ ┃ ┣ VbNonPORequest
 ┃ ┃ ┣ VBRequestAll
 ┃ ┃ ┣ VBStatusReport
 ┃ ┃ ┣ VBVerification
 ┃ ┃ ┗ VbWithPORequest
 ┃ ┣ Com.Danliris.Service.Finance.Accounting.Lib.csproj
 ┃ ┣ FinanceDbContext.cs
 ┃ ┗ FinanceLibClassDiagram.cd

 ```


**2. Com.Danliris.Service.Finance.Accounting.WebApi**

This folder consists of controller API. The controller has responsibility to processing data and  HTTP requests and then send it to a web page. All responses from the HTTP requests API are formatted as JSON (JavaScript Object Notation) objects containing information related to the request, and any status.

The folder tree in this folder is:

```
com-danliris-service-finance-accounting
 ┣ Com.Danliris.Service.Finance.Accounting.WebApi
 ┃ ┣ bin
 ┃ ┃ ┗ Debug
 ┃ ┃ ┃ ┗ netcoreapp2.0
 ┃ ┃ ┃ ┃ ┣ Properties
 ┃ ┣ Controllers
 ┃ ┃ ┗ v1
 ┃ ┃ ┃ ┣ CashierApproval
 ┃ ┃ ┃ ┣ ClearaceVB
 ┃ ┃ ┃ ┣ CreditBalance
 ┃ ┃ ┃ ┣ CreditorAccount
 ┃ ┃ ┃ ┣ DailyBankTransaction
 ┃ ┃ ┃ ┣ DownPayment
 ┃ ┃ ┃ ┣ JournalTransaction
 ┃ ┃ ┃ ┣ LockTransaction
 ┃ ┃ ┃ ┣ Master
 ┃ ┃ ┃ ┣ Memo
 ┃ ┃ ┃ ┣ OthersExpenditureProofDocument
 ┃ ┃ ┃ ┣ PaymentDispositionNote
 ┃ ┃ ┃ ┣ PaymentDispositionNotVerifiedReport
 ┃ ┃ ┃ ┣ PurchasingDispositionAcceptance
 ┃ ┃ ┃ ┣ PurchasingDispositionExpedition
 ┃ ┃ ┃ ┣ PurchasingDispositionReport
 ┃ ┃ ┃ ┣ PurchasingDispositionVerification
 ┃ ┃ ┃ ┣ RealizationVBNonPO
 ┃ ┃ ┃ ┣ RealizationVBWIthPO
 ┃ ┃ ┃ ┣ SalesReceipt
 ┃ ┃ ┃ ┣ VBExpeditionRealizationReport
 ┃ ┃ ┃ ┣ VBNonPORequest
 ┃ ┃ ┃ ┣ VBRequestAll
 ┃ ┃ ┃ ┣ VBStatusReport
 ┃ ┃ ┃ ┣ VBVerification
 ┃ ┃ ┃ ┣ VBWIthPORequest
 ┃ ┣ obj
 ┃ ┃ ┣ Debug
 ┃ ┃ ┃ ┗ netcoreapp2.0
 ┃ ┣ Properties
 ┃ ┣ Utilities
 ┃ ┣ Com.Danliris.Service.Finance.Accounting.WebApi.csproj
 ┃ ┣ Com.Danliris.Service.Finance.Accounting.WebApi.csproj.user
 ┃ ┣ Program.cs
 ┃ ┗ Startup.cs
 ```

**3. Com.Danliris.Service.Finance.Accounting.Test**

This folder is collection of classes to run code testing. The automation type testing used in this app is  a unit testing with using moq and xunit libraries.

DataUtils:

- Colecction class to seed data as data input in unit test 

The folder tree in this folder is:

```
com-danliris-service-finance-accounting
 ┣ Com.Danliris.Service.Finance.Accounting.Test
 ┃ ┣ bin
 ┃ ┃ ┗ Debug
 ┃ ┃ ┃ ┗ netcoreapp2.0
 ┃ ┃ ┃ ┃ ┣ Properties
 ┃ ┣ Controllers
 ┃ ┃ ┣ CashierApproval
 ┃ ┃ ┣ ClearaceVB
 ┃ ┃ ┣ CreditBalance
 ┃ ┃ ┣ CreditorAccount
 ┃ ┃ ┣ DailyBankTransaction
 ┃ ┃ ┣ DownPayment
 ┃ ┃ ┣ JournalTransaction
 ┃ ┃ ┣ LockTransaction
 ┃ ┃ ┣ Masters
 ┃ ┃ ┣ Memo
 ┃ ┃ ┣ OthersExpenditureProofDocument
 ┃ ┃ ┣ PaymentDispositionNote
 ┃ ┃ ┣ PaymentDispositionNotVerifiedReport
 ┃ ┃ ┣ PurchasingDIspositionAcceptance
 ┃ ┃ ┣ PurchasingDispositionExpedition
 ┃ ┃ ┣ PurchasingDispositionReport
 ┃ ┃ ┣ PurchasingDIspositionVerification
 ┃ ┃ ┣ RealizationVBNonPO
 ┃ ┃ ┣ RealizationVBWIthPO
 ┃ ┃ ┣ SalesReceipt
 ┃ ┃ ┣ VBExpeditionRealizationReport
 ┃ ┃ ┣ VbNonPORequest
 ┃ ┃ ┣ VBRealizationDocumentExpedition
 ┃ ┃ ┣ VBRequestAll
 ┃ ┃ ┣ VBStatusReport
 ┃ ┃ ┣ VbVerification
 ┃ ┃ ┗ VbWithPORequest
 ┃ ┣ DataUtils
 ┃ ┃ ┣ CashierApproval
 ┃ ┃ ┣ ClearaceVB
 ┃ ┃ ┣ CreditorAccount
 ┃ ┃ ┣ DailyBankTransaction
 ┃ ┃ ┣ DownPayment
 ┃ ┃ ┣ JournalTransaction
 ┃ ┃ ┣ LockTransaction
 ┃ ┃ ┣ Masters
 ┃ ┃ ┃ ┗ COADataUtils
 ┃ ┃ ┣ Memo
 ┃ ┃ ┣ NewIntegrationDataUtils
 ┃ ┃ ┣ PaymentDispositionNote
 ┃ ┃ ┣ PurchasingDispositionExpedition
 ┃ ┃ ┣ RealizationVBNonPO
 ┃ ┃ ┣ RealizationVBWIthPO
 ┃ ┃ ┣ SalesReceipt
 ┃ ┃ ┣ VBExpeditionRealizationReport
 ┃ ┃ ┣ VbNonPORequest
 ┃ ┃ ┣ VBRealizationDocumentExpedition
 ┃ ┃ ┣ VBRequestAll
 ┃ ┃ ┣ VBStatusReport
 ┃ ┃ ┣ VbVerification
 ┃ ┃ ┗ VbWithPORequest
 ┃ ┣ Enums
 ┃ ┣ Helpers
 ┃ ┣ obj
 ┃ ┃ ┣ Debug
 ┃ ┃ ┃ ┗ netcoreapp2.0
 ┃ ┣ Services
 ┃ ┃ ┣ CashierApproval
 ┃ ┃ ┣ ClearaceVB
 ┃ ┃ ┣ CreditBalance
 ┃ ┃ ┣ CreditorAccount
 ┃ ┃ ┣ DailyBankTransaction
 ┃ ┃ ┣ DownPayment
 ┃ ┃ ┣ JournalTransaction
 ┃ ┃ ┣ LockTransaction
 ┃ ┃ ┣ Masters
 ┃ ┃ ┃ ┗ COATest
 ┃ ┃ ┣ Memo
 ┃ ┃ ┣ OthersExpenditureProofDocument
 ┃ ┃ ┃ ┣ Helper
 ┃ ┃ ┣ PaymentDispositionNote
 ┃ ┃ ┣ PaymentDispositionNotVerifiedReport
 ┃ ┃ ┣ PurchasingDispositionExpedition
 ┃ ┃ ┣ RealizationVBNonPO
 ┃ ┃ ┣ RealizationVBWIthPO
 ┃ ┃ ┣ SalesReceipt
 ┃ ┃ ┣ VBExpeditionRealizationReport
 ┃ ┃ ┣ VbNonPORequest
 ┃ ┃ ┣ VBRealizationDocumentExpedition
 ┃ ┃ ┣ VBRequestAll
 ┃ ┃ ┣ VBStatusReport
 ┃ ┃ ┣ VbVerification
 ┃ ┃ ┗ VbWithPORequest
 ┃ ┣ ServicesLib
 ┃ ┃ ┣ HttpClientService
 ┃ ┃ ┗ ValidateService
 ┃ ┣ Utilities
 ┃ ┣ Utils
 ┃ ┣ ViewModels
 ┃ ┃ ┣ ClearaceVB
 ┃ ┃ ┣ CreditorAccount
 ┃ ┃ ┣ DownPayment
 ┃ ┃ ┣ NewIntegrationViewModel
 ┃ ┃ ┣ PaymentDispositionNoteViewModel
 ┃ ┃ ┣ VBExpeditionRealizationReport
 ┃ ┃ ┣ VBRealizationDocumentExpedition
 ┃ ┃ ┣ VBRequestAll
 ┃ ┃ ┗ VBStatusReport
 ┃ ┗ Com.Danliris.Service.Finance.Accounting.Test.csproj
```

**TestResults**

- Collections of files generated by the system for purposes of unit test code coverage.


**FinanceDbContext.cs**

This file contain context class that derives from DbContext in entity framework. DbContext is an important class in Entity Framework API. It is a bridge between domain or entity classes and the database. DbContext and context class  is the primary class that is responsible for interacting with the database.


**File Program.cs**

Important class that contains the entry point to the application. The file has the Main() method used to run the application and it is used to create an instance of WebHostBuilder for creating a host for the application. The Startup class to be used by the application is specified in the Main method.

**File Startup.cs**

This file contains Startup class. The Startup class configures services and the app's request pipeline.Optionally includes a ConfigureServices method to configure the app's services. A service is a reusable component that provides app functionality. Services are registered in ConfigureServices and consumed across the app via dependency injection (DI) or ApplicationServices.This class also Includes a Configure method to create the app's request processing pipeline.

**File .codecov.yml**

This file is used to configure code coverage in unit tests.

**File .travis.yml**

Travis CI (continuous integration) is configured by adding a file named .travis.yml. This file in a YAML format text file, located in root directory of the repository. This file specifies the programming language used, the desired building and testing environment (including dependencies which must be installed before the software can be built and tested), and various other parameters.

**Com.Danliris.Service.Finance.Accounting.sln**

File .sln is extention for *solution* aka file solution for .Net Core, this file is used to manage all project by code editor.

 ### Validation
Data validation using **IValidatableObject**