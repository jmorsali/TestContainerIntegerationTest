
# ToDoWebApi | ![.github/workflows/github.yml](https://github.com/AdemCatamak/ToDoWebApi/workflows/.github/workflows/github.yml/badge.svg?branch=master)

This project has been prepared to set an example for writing an integration test using [Test Containers](https://github.com/HofmeisterAn/dotnet-testcontainers).

You can access the article referencing this project on [Medium](https://medium.com/@ademcatamak/integration-test-with-net-core-and-docker-21b241f7372).

[Medium](https://medium.com/@ademcatamak/net-core-ve-docker-ile-entegrasyon-testi-4bf51c03246f) aracılığıyla bu projeyi referans alan yazıma ulaşabilirsiniz.

# Implementation Details

The project uses [Sql Server Db](https://www.microsoft.com/tr-tr/sql-server/sql-server-2019).

Communication with the database and creation of database tables are provided by [Entity Framework Core](https://github.com/dotnet/efcore).

[XUnit](https://github.com/xunit/xunit) library is used when tests are coded.

External components, such as the database required for the tests to run, are created on [Docker](https://www.docker.com/products/docker-desktop) platform via [Test Containers](https://github.com/HofmeisterAn/dotnet-testcontainers).

# Run

The project is developed with [.Net Core 3.1](https://dotnet.microsoft.com/download/dotnet-core/3.1). To compile and run, you must have the appropriate .NetCore SDK.

Sql Server database is needed for ToDoWebApi application to work. It will be sufficient to place the connection string belonging to the Sql Server database you have in the `SqlServerConnectionStr` field in `appsettings.json`.

# Test

The project is developed with [.Net Core 3.1](https://dotnet.microsoft.com/download/dotnet-core/3.1). To compile and run, you must have the appropriate .NetCore SDK. In addition, the database will be created as a container with the Docker tool. For this reason, the [Docker](https://www.docker.com/products/docker-desktop) tool must be installed on your computer in order for the tests to run.

ToDoWebApi.Test project provides access to the database through the port _9999_. Therefore, you must make sure that this port is not used in order to start the test process. Another method is to write a port value that you know is empty to the field `SqlServerConnectionStr` in the `appsettings.test.json` file.