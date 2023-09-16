# Secure BusinessCalendar with OpenId Connect

You can use any desired identity provider with **OpenId Connect** support

## Configure Identity Provider for BusinessCalendar

Check desired identity provider documentation to find out how to create new client, and enrich tokens with claims

### Read access
any authenticated user has read access by default

### Write access
To grant write access to user you should provide `role` claim with `bc-manager` role inside an **id_token** or through UserInfo endpoint
```json
"role": [
    "bc-manager"
  ],
```

### Troubleshooting
1. Check the authority address accessing it's configuration from browser,
   e.g. for the authority address http://host.docker.internal:4082/realms/business-calendar-demo
   configuration should be accessible through url http://host.docker.internal:4082/realms/business-calendar-demo/.well-known/openid-configuration.
   Check your identity provider settings in case you don't receive the json configuration.
2. The authority address should be accessible from both inside the docker and from browser on client machine

## Configure BusinessCalendar with docker-compose

1. create `docker-compose.yml` file
    ```yaml
    version: "3.8"
    services:
      web-api-prod-image:
        image: "ghcr.io/myzamaraev/businesscalendar:latest"
        environment:
          MongoDB__ConnectionURI: "mongodb://mongodb:27017"
          Auth__UseOpenIdConnectAuth: true 
          Auth__SessionCookieLifetimeMinutes: 60 #Optional, 60 minutes is default value
          OpenIdConnect__Authority: ${OpenIdConnect_Authority}
          OpenIdConnect__ClientId: ${OpenIdConnect_ClientId}
          OpenIdConnect__ClientSecret: ${OpenIdConnect_ClientSecret}
        ports:
          - "4081:80"
        depends_on:
          - mongodb
      
      mongodb:
        image: mongo:6.0.4
        ports:
          - 27017:27017
        volumes:
          - mongo:/data/db
    volumes:
      mongo:
    ```
2. define OpenIdConnect environment variables, the options are:
   - replace with the appropriate string values, 
   - create `.env` file in the same directory
       ```yaml
      OpenIdConnect_Authority: "http://host.docker.internal:4082/realms/business-calendar-demo"
      OpenIdConnect_ClientId: "business-calendar"
      OpenIdConnect_ClientSecret: "H2Wzx1NhB21NT85NNDsbGobuf9RpOAVe"
       ```



