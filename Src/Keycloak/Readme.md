## exporting realm with users

1. Enter to the container bash terminal ```docker exec -it business-calendar-keycloak-1 bash```
2. run command ```/opt/keycloak/bin/kc.sh export --dir /opt/keycloak/data/export --realm /business-calendar-demo --users realm_file```
3. to copy exported file to host machine 
   1. cd to the desired directory
   2. run command on host machine ```docker cp business-calendar-keycloak-1:/opt/keycloak/data/export/business-calendar-demo-realm.json ./keycloakbusiness-calendar-demo-realm.json```