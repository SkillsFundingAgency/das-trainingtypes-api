## ⛔Never push sensitive information such as client id's, secrets or keys into repositories including in the README file⛔

# Course Types API

<img src="https://avatars.githubusercontent.com/u/9841374?s=200&v=4" align="right" alt="UK Government logo">

[![Build Status](https://dev.azure.com/sfa-gov-uk/Digital%20Apprenticeship%20Service/_apis/build/status/das-coursetypes-api?branchName=master)](https://dev.azure.com/sfa-gov-uk/Digital%20Apprenticeship%20Service/_build/latest?definitionId=_projectid_&branchName=master)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=_projectId_&metric=alert_status)](https://sonarcloud.io/dashboard?id=_projectId_)
[![Jira Project](https://img.shields.io/badge/Jira-Project-blue)](https://skillsfundingagency.atlassian.net/secure/RapidBoard.jspa?rapidView=564&projectKey=APPMAN)
[![Confluence Project](https://img.shields.io/badge/Confluence-Project-blue)](https://skillsfundingagency.atlassian.net/wiki/spaces/_pageurl_)
[![License](https://img.shields.io/badge/license-MIT-lightgrey.svg?longCache=true&style=flat-square)](https://en.wikipedia.org/wiki/MIT_License)

The Course Types API provides information about different types of courses in the apprenticeship service, including their duration, learner age requirements, and recognition of prior learning (RPL) requirements.

Key features:
1. Provides course duration information (minimum and maximum months)
2. Specifies learner age requirements (minimum and maximum age)
3. Details RPL requirements and off-the-job training minimum hours
4. Supports different course types (Apprenticeship, Foundation Apprenticeship)

## How It Works

The Course Types API is built using .NET Core and follows a clean architecture pattern with separate Domain, Application, and API layers. The API provides endpoints to retrieve information about different course types and their specific requirements.

The system is designed to be extensible, allowing new course types to be added by implementing the base `CourseType` class and providing the required information for that specific type.

## 🚀 Installation

### Pre-Requisites

* A clone of this repository
* .NET 8.0 SDK
* Visual Studio 2022 or later (or your preferred IDE)
* Azure subscription (for deployment)

### Config

This API uses the standard Apprenticeship Service configuration. All configuration can be found in the [das-employer-config repository](https://github.com/SkillsFundingAgency/das-employer-config).

Add an appsettings.Development.json file:
```json
{
    "Logging": {
      "LogLevel": {
        "Default": "Information",
        "Microsoft": "Warning",
        "Microsoft.Hosting.Lifetime": "Information"
      }
    },
    "ConfigurationStorageConnectionString": "UseDevelopmentStorage=true;",
    "ConfigNames": "SFA.DAS.CourseTypes.Api",
    "EnvironmentName": "LOCAL",
    "Version": "1.0",
    "APPINSIGHTS_INSTRUMENTATIONKEY": ""
}
```

## 🔗 External Dependencies

* No external dependencies required for local development

## Technologies

* .NET 8.0
* ASP.NET Core
* MediatR
* NUnit
* Moq
* FluentAssertions

## 🐛 Known Issues

* None currently known