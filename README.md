# BosquesNalcahue Web API
_You can find the doc for the API here:_ https://app.eraser.io/workspace/dx4U2YKrkqLNF9nEiOfO?origin=share

## Summary
Web API created  ﻿with ﻿ASP.NET Core 8.0 as part of a larger demo project for a client. You can find the other components in the system in here:
- .NET MAUI Mobile App => https://github.com/DamianQS7/ForestalCasablancaApp
- Angular 18 SPA => https://github.com/DamianQS7/BosquesNalcahue_WebPortal

![image](https://github.com/user-attachments/assets/969183b1-0044-4707-8b9b-84a3b26085b4)



The API is designed to centralize data from a Mobile App and serve as the backbone for several key components and services in a larger system. 
This demo showcases the API's role in achieving the client's desired functionality, with four main components integrated into the system:

1. **Reports API**: Exposes endpoints to perform all the CRUD operations on the Reports (the main resource of this API):
- Get a Report by ID (GET)
- Get all the Reports (GET)
- Create a Report (POST)
- Delete a Report (DELETE)
- Update a Report (PUT)
2. **Analytics API:** Exposes endpoints to retrieve computed data based on the information contained on the reports; So far only a count metric for product type has been implemented:
- Count all the reports of a specific period of time (GET)
- Count all the reports in a specific month (GET)
- Monthly breakdown of the counts (GET)
3. **Identity Management API:** Exposes endpoints for the users to authenticate themselves:
- Login (POST)
- Register (POST)
- Refresh JWT (POST)
4. **Blob Storage API:** Exposes endpoints to manage files in an Azure Blob Storage's container:
- Upload a file (POST)
- Delete a file (DELETE)
- Get a _Shared Access Signature_ _(SAS)_ token to access a file (GET)

## Implementation details

- Deployed to Azure Web App using Github Actions.
- Data persistance with MongoDB Atlas:
    - LINQ and Builders approaches to interact with the database.
    - Aggregation Pipelines using LINQ approach.
    - EF Core driver for MongoDB (Only for the Identity Databas
- Repository Pattern
- Dependency Injection
- Extension methods for object mapping.
- Validations using FluentValidation and Custom Middleware.
- Auth Implementation:
    - Uses ASP.NET Core Identity with MongoDB and the EF Core Driver, to handle users account creation.
    - JWT and Refresh Tokens ⇒ Without using a third-party service; Custom method to sign and verify the tokens.
    - API Key and AuthorizationFilters implemented at Controller’s action methods level.
    - Authorization Policies.
- CORS Enabled.
