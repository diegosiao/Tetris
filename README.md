# Tetris .Net Core Library #

![.NET Core](https://github.com/diegosiao/Tetris/workflows/.NET%20Core/badge.svg) ![Nuget](https://github.com/diegosiao/Tetris/workflows/Nuget/badge.svg)

## Putting the building blocks of your application together

Tetris is a .NET Core based library to help you to build ASP.NET Core Web APIs.

Based on [CQRS](https://martinfowler.com/bliki/CQRS.html) - Command and Queries Responsibility Segregation - it helps you to build simple and scalable backend applications.

The Tetris library aims to provide out of the box support for your ASP.NET Core Web API application with: 
- Database Command and Query easy consumption
- Integrated .NET Component Model Validation
- JWT Token based authentication

⚠️ **Note: this project is in development and should not be used in production enviroments.** ⚠️

## Nuget Package 

In Package Manager Console of you project, execute:

`Install-Package Tetris`

## Required Settings

Your application must declare the settings below before execution. 

You can override the default connection string for specific Queries and Commands using the 'ConnectionStringKey' property of 'TetrisProcedure' attribute.

Configuration Section | Key | Description
--- | --- | ---
ConnectionStrings | Tetris_CommandsConnectionString | The connection string for read and write database commands
ConnectionStrings | Tetris_QueriesConnectionString | The connection string for readonly database commands
AppSettings | Tetris_EncriptionKey | Secret to be used in simple encryption and decryption operations

## Who do I talk to about this?

Diego Morais - diegosiao@gmail.com
Grecio Beline - grecio@gmail.com
