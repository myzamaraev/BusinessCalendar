FROM node:lts-alpine as front-build
WORKDIR /app
COPY /dashboard/package*.json ./
RUN npm install
COPY /dashboard/ ./
RUN npm run build


FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
#Copy only project files and restore before build to allow restored packages caching when no changes in project files
COPY ["BusinessCalendar.WebAPI/BusinessCalendar.WebAPI.csproj", "BusinessCalendar.WebAPI/"]
COPY ["BusinessCalendar.Domain/BusinessCalendar.Domain.csproj", "BusinessCalendar.Domain/"]
COPY ["Storage/BusinessCalendar.MongoDb/BusinessCalendar.MongoDb.csproj", "Storage/BusinessCalendar.MongoDb/"]
COPY ["Storage/BusinessCalendar.Postgres/BusinessCalendar.Postgres.csproj", "Storage/BusinessCalendar.Postgres/"]
COPY ["Storage/BusinessCalendar.SqlMigrations/BusinessCalendar.SqlMigrations.csproj", "Storage/BusinessCalendar.SqlMigrations/"]
RUN dotnet restore "BusinessCalendar.WebAPI/BusinessCalendar.WebAPI.csproj" 
#Copy all the source files and run build
COPY . .
WORKDIR "/src/BusinessCalendar.WebAPI"
RUN dotnet build "BusinessCalendar.WebAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BusinessCalendar.WebAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY --from=front-build "app/dist" ./wwwroot
ENTRYPOINT ["dotnet", "BusinessCalendar.WebAPI.dll"]
