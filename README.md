# BusinessCalendar

An easy way to store and manage business calendars to provide knowledge about workdays as infrastructure service for enterprise.

### Dependencies
Requires MongoDB as external dependency


## installation (docker compose)
1. Create `docker-compose.yml` file
    ```yaml
    version: 3
    services:
      mongodb:
        image: mongo:6.0.4
        ports:
          - 27018:27017
        volumes:
          - mongo:/data/db
      business-calendar:
        image: "ghcr.io/myzamaraev/businesscalendar:0.1"
        environment:
          MongoDB__ConnectionURI: "mongodb://mongodb:27017"
        ports:
          - 4081:80
    volumes:
      mongo:
    ```
    - You can also provide the name of the database with optional `MongoDB__DatabaseName: "DesiredDbName"` environment variable
    - You can provide binging with any local port you want instead of `4081`
   

3. run services with `docker compose up` command
4. access the UI by the following link http://localhost:4081

## usage

### .Net
Install nuget with client.. (To be continued)

Check [demo](https://github.com/myzamaraev/BusinessCalendar/tree/master/BusinessCalendar.Demo) project for usage example

### Other languages
No client libraries provided yet, but you can port it from .Net using provided API


