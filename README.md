# Monik
Backend and client libraries to collect and process messages: logs, performance counters and keep-alive statuses. 

## Setup backend (Azure Cloud Service)
1. Prepare Service Bus namespace and queue
2. Prepare SQL Database and execute src/common/db.sql
3. Fill ServiceConfiguration files in MonikCloud project
4. Deploy service

## Client Use:
1. Add new nuget package source: https://www.myget.org/F/totopolis/
2. Install last package Monik.Client.Azure to your project
3. Sample:
```csharp
// Initialize
var azureSender = new AzureSender("[Service Bus connection string]", "[Queue name]");
M.Initialize(azureSender, "[Source name]", "[Source instance]");

// Send message
M.SecurityInfo("User John log to");
M.ApplicationError("Some error in application");

// Enable auto Keep-Alive (per 5 sec)
M.MainInstance.KeepAliveInterval = 5000;
M.MainInstance.AutoKeepAlive = true;
```