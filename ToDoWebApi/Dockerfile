#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 443
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["ToDoWebApi/ToDoWebApi.csproj", "ToDoWebApi/"]
RUN dotnet restore "ToDoWebApi/ToDoWebApi.csproj"
COPY . .
WORKDIR "/src/ToDoWebApi"
RUN dotnet build "ToDoWebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ToDoWebApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ToDoWebApi.dll"]