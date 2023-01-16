# demo-todo-wpf-mvvm-efcore

A demonstration of a simple wpf todo app written using the mvvm toolkit that uses efcore with mssql

## Technologies

* [CommunityToolkit.Mvvm](https://learn.microsoft.com/en-us/dotnet/communitytoolkit/mvvm/)
	* Replacement for [MVVM Light](https://github.com/lbugnion/mvvmlight)
* Net 7
* EF Core 7
* Mssql database

## Projects

* TodoWpf - net7 Wpf app project with views, view models and models
* TodoDb - mssql database project containing the todo db table
* Todo.Database - net 7 library containing ef core db context and mappings

## Getting Started

1. Create the todo db by publishing the TodoDb project to localhost\sqlexpress or to localdb
	- Connection string will need to be manually changed in TodoDbContext.cs if not publishing to localhost\sqlexpress
2. Debug the TodoWpf Project in Visual Studio or Rider