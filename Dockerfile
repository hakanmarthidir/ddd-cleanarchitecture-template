FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY . ./
RUN dotnet restore
RUN dotnet build -c Release

FROM build AS publish
RUN dotnet publish "./Api/Api.csproj" -c Release -o publish

FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=publish /src/publish .
ENTRYPOINT ["dotnet", "Api.dll"]


