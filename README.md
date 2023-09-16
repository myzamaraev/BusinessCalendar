# BusinessCalendar

An easy way to store and manage business calendars to provide knowledge about workdays as infrastructure service for enterprise.

### Dependencies
Requires MongoDB as external dependency


## installation (docker compose)
1. Create `docker-compose.yml` file
    ```yaml
    version: "3.8"
    services:
      mongodb:
        image: mongo:6.0.4
        ports:
          - 27017:27017
        volumes:
          - mongo:/data/db
      business-calendar:
        image: "ghcr.io/myzamaraev/businesscalendar:latest"
        environment:
          MongoDB__ConnectionURI: "mongodb://mongodb:27017"
        ports:
          - "4080:80"
        depends_on:
          - mongodb
    volumes:
      mongo:
    ```
    - You can also provide the name of the database with optional `MongoDB__DatabaseName: "DesiredDbName"` environment variable
    - You can provide binging with any local port you want instead of `4080`
   
   
3. run services with `docker compose up` command
4. access the UI by the following link http://localhost:4081

## usage

### .Net
Install nuget with client.. (To be continued)
```console
dotnet add package BusinessCalendar.Client --version 1.0.1
```

Check [demo](https://github.com/myzamaraev/BusinessCalendar/tree/master/BusinessCalendar.Demo) project for usage example

### Other languages
No client libraries provided yet, but you can port it from .Net using provided API

## Authentication
You can add authentication with the variety of identity providers through **OpenId Connect** protocol. 

- Read [Configure Identity Provider](Custom_identity_provider.md) for further details and examples.
- Or clone repo and use [docker-compose.yml](Src/docker-compose.yml) to run development configuration with [Keycloak](https://www.keycloak.org/)