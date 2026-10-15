# Dockerfile for building and running the .NET API
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# copy csproj and restore as distinct layers
COPY StyleAI.Api/StyleAI.Api.csproj StyleAI.Api/
COPY StyleAI.Infrastructure/StyleAI.Infrastructure.csproj StyleAI.Infrastructure/
COPY StyleAI.Core/StyleAI.Core.csproj StyleAI.Core/
RUN dotnet restore StyleAI.Api/StyleAI.Api.csproj

# copy everything and build
COPY . .
WORKDIR /src/StyleAI.Api
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish ./

ENV ASPNETCORE_URLS=http://+:5000
EXPOSE 5000
ENTRYPOINT ["dotnet", "StyleAI.Api.dll"]
