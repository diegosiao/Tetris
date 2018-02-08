# README #

Tetris is a .NET based framework to enable C# code to be 'transpiled' into database scripts, procedures and functions interchangeably among the officially supported databases: **SQLServer, Oracle and MySQL.**

The most important premise is: The meta code writen once can generate the target database object in **ALL** supported databases. It is an all or nothing approach.

Using what I called 'Database Agnostic Intermidiate Objects', you can build the procedures used by your application on C# code.

**Note: this project is in development and should not be used in production enviroments.**

### How to use Tetris in my .NET project? ###

You can both:

* Add via NuGet executing the command: **Install-Package Tetris** in NuGet Package Manager console.

* Clone the repository and add a reference to Tetris.Common Visual Studio project in your application references.

### Contribution guidelines ###

Please keep in mind that only implementations that considers ALL supported databases will be accepted.

### Who do I talk to? ###

diegosiao@gmail.com
