## Configure Identity Provider for BusinessCalendar

You can use any desired identity provider which supports **OpenId Connect**

### Read access
any authenticated user has read access by default

### Write access
To grant the user write access you should provide `role` claim with `bc-manager` role inside an **id_token**
```json
"role": [
    "bc-manager"
  ],
```
