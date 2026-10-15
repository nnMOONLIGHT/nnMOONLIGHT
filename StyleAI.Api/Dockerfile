
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src


COPY StyleAI.Api/StyleAI.Api.csproj StyleAI.Api/
COPY StyleAI.Infrastructure/StyleAI.Infrastructure.csproj StyleAI.Infrastructure/
COPY StyleAI.Core/StyleAI.Core.csproj StyleAI.Core/
RUN dotnet restore StyleAI.Api/StyleAI.Api.csproj


COPY . .
WORKDIR /src/StyleAI.Api
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish ./

ENV ASPNETCORE_URLS=http://+:5000
EXPOSE 5000
ENTRYPOINT ["dotnet", "StyleAI.Api.dll"]
