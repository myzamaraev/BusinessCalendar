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
COPY ["BusinessCalendar.WebAPI/BusinessCalendar.WebAPI.csproj", "BusinessCalendar.WebAPI/"]
COPY ["BusinessCalendar.Domain/BusinessCalendar.Domain.csproj", "BusinessCalendar.Domain/"]
COPY ["BusinessCalendar.MongoDb/BusinessCalendar.MongoDb.csproj", "BusinessCalendar.MongoDb/"]
RUN dotnet restore "BusinessCalendar.WebAPI/BusinessCalendar.WebAPI.csproj"
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
