# Developer Reference

## Full rebuild of BusinessCalendar container
This set of commands guarantees full rebuild of the image
```console
docker compose -f compose-mongo.yml build --no-cache
docker compose -f compose-mongo.yml up --wait
```

## Change storage type
All storage services in compose use the same port 4081. 

To successfully start with another database we should stop the current one to release port number  
```console
docker compose -f compose-mongo.yml stop
docker compose -f compose-postgres.yml up --wait
```

## Debug locally on Windows
1. Start **BusinessCalendar.WebAPI** in Debug mode on http port
2. Change the port number in `dashboard/vue.config.js` to proxy requests to local API
3. Build and run SPA
    ```
   cd dashboard
   npm install
   npm audit fix
   npm run serve
   ```
4. access SPA on `http://localhost:8080`