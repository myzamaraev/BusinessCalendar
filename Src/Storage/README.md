# Adding new storage checklist

### Step 1: Implement storage layer library
- Register storage interface implementations
- For SQL database register fluent migrations
- Add DB health check

### Step 2: Register new storage layer in WebAPI project
- Add new value to DatabaseTypes and provide configuration file for new storage
- Call storage services and healthcheck registration
- add `COPY` statement for new project inside `DockerFile`

### Step 3: Provide docker-compose
- add compose file `compose-[storagename].yml` inside `Src` folder and include it into solution under `Docker` folder
- provide database container inside compose and bind it to port 4081
- for SQL storage it's required to create an empty database inside container and include it into connection string to allow migrations