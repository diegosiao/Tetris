# README #

Tetris is a C# framework to enable your code to be translated into database CRUD operations plus procedures and functions interchangeably among the officially supported databases: SQLServer, Oracle and MySQL.

The most important premise is: The meta code writen once can generate the target database object in **ALL** supported databases. It is an all or nothing approach.

Using what I called 'Database Agnostic Intermidiate Objects', you can build the procedures used by your application on C# code.

**Note: this project is in development and should not be used in production enviroments.**

### How to use Tetris in my project? ###

You can both:

* Via NuGet executing the command: **Install-Package Tetris** in the NuGet package manager console.

* Clone the repository and add a reference to Tetris.Common Visual Studio project in your application references.

### Contribution guidelines ###

Please keep in mind that only implementations that considers ALL supported databases will be accepted.

### Who do I talk to? ###

diegosiao@gmail.com