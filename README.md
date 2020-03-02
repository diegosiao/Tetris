# Tetris .Net Core Library #

![.NET Core](https://github.com/diegosiao/Tetris/workflows/.NET%20Core/badge.svg) ![Nuget](https://github.com/diegosiao/Tetris/workflows/Nuget/badge.svg)

## What is it for?

Tetris is a .NET Core based library to help you to build ASP.NET Core Web APIs.

Based on [CQRS](https://martinfowler.com/bliki/CQRS.html) - Command and Queries Responsibility Segregation - it helps you to build scalable backend applications.

⚠️ **Note: this project is in development and should not be used in production enviroments.** ⚠️

## Nuget Package 

In Package Manager Console of you project, execute:

`Install-Package Tetris`

## Required Settings

Configuration Section | Key | Description
--- | --- | --- | ---
AppSettings | Tetris_EncriptionKey | Secret to be used in simple encryption and decryption operations
ConnectionStrings | Tetris_CommandsConnectionString | File path containing version info, relative to repository root
ConnectionStrings | Tetris_QueriesConnectionString | Regex pattern to extract version info in a capturing group

## Who do I talk to about this? ##

Diego Morais - diegosiao@gmail.com
Grecio Beline - grecio@gmail.com
