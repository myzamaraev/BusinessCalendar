# BusinessCalendar

An easy way to store and manage business calendars to provide knowledge about workdays as infrastructure service for enterprise.

####  [Check out the demo video](https://drive.google.com/file/d/1sCv1SwkJi_u0Arteskc9g5_eV_IAXSd3/view?usp=sharing)

### Dependencies
Requires MongoDB as an external dependency

## Build and run service with docker-compose
Publishing ready-to use docker image is really complicated thing in terms of license compliance.
That's why you have to build the image by yourself. But it's really easy with the help of docker-compose.

1. Clone repo
2. Go to repo directory and run following command. It will create docker containers for both mongodb database and service itself.
   ```console
   docker compose -f Src/docker-compose.yml up --wait
   ``` 
3. Access UI dashboard with the following link http://host.docker.internal:4080, or alternatively http://localhost4080

## Usage

### .Net
Install nuget with client
```console
dotnet add package BusinessCalendar.Client --version 1.0.4
```

Check [demo](Src/BusinessCalendar.Demo) project for usage examples

### Other languages
No client libraries provided yet, feel free to contribute a port from .Net using provided API.
Swagger API documentation is available in the container http://host.docker.internal:4080/swagger/index.html

## Authentication

You can add authentication with the variety of identity providers through **OpenId Connect** protocol. 

Read [Configure Identity Provider](Custom_identity_provider.md) for further details and examples.

### Development configuration with Keycloak
Clone repo and use [docker-compose-auth.yml](Src/docker-compose-auth.yml) to run development configuration with [Keycloak](https://www.keycloak.org/)

Please note that it may take a little while for keycloak to be properly configured, check container logs in case it's not accessible after a minute at http://host.docker.internal:4082

Access an app using http://host.docker.internal:4080, as compose uses this domain for auth and CORS are not allowed with provided configuration.


#### preconfigured keycloak users
| Login       | Password   | application access
|-------------|------------|-------------------------
| admin       | admin      | Keycloak admin
| bc-user     | bc-user    | BusinessCalendar (read)
| bc-manager  | bc-manager | BusinessCalendar (write)

