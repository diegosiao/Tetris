# Tetris .Net Core Library #

![.NET Core](https://github.com/diegosiao/Tetris/workflows/.NET%20Core/badge.svg) ![Nuget](https://github.com/diegosiao/Tetris/workflows/Nuget/badge.svg)

## Putting the building blocks of your application together

Tetris is a .NET Core based library to help you to build ASP.NET Core Web APIs.

Based on [CQRS](https://martinfowler.com/bliki/CQRS.html) - Command and Queries Responsibility Segregation - it helps you to build simple and scalable backend applications.

The Tetris library aims to provide out of the box support for your ASP.NET Core Web API application with: 
- Database Command and Query easy consumption
- Integrated .NET Component Model Validation
- JWT Token based authentication

⚠️ **IMPORTANT: this project is in development and should not be used in production enviroments.** ⚠️

## Nuget Package 

In Package Manager Console of you project, execute:

`Install-Package Tetris`

## Required Settings

Your application must declare the settings below before execution. 

You can override the default connection string for specific Queries and Commands using the 'ConnectionStringKey' property of 'TetrisProcedure' attribute.

Configuration Section | Key | Description
--- | --- | ---
ConnectionStrings | TetrisCommands | The connection string for read and write database commands
ConnectionStrings | TetrisQueries | The connection string for readonly database commands
AppSettings | TetrisEncryptionSecret | Secret to be used in simple encryption and decryption operations

## Getting Started

### Application setup

1. Reference the Tetris Nuget Package in your ASP.NET Core Web API application

2. Configure the Startup class

3. Set the required settings described above in the configuration file

### 'Per Operation' setup

1. Choose and classify a database procedure as a command (READ/WRITE) or query (READONLY);

2. Create a class that will represent the choosen database procedure and specify the 'TetrisProcedure' attribute and make it inherit from 'TetrisCommand' or 'TetrisQuery' class. Properties will act like parameters.

### Controller setup

1. Make your controller inherit 'TetrisApiController'

2. Declare your command or query representing class as an input parameter of any web action and execute it

## Who do I talk to about this?

Diego Morais - diegosiao@gmail.com

Grecio Beline - grecio@gmail.com
