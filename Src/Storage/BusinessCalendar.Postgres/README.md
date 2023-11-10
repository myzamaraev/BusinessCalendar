# Storage layer for PostgreSQL

Uses [npgsql](https://npgsql.org) to run ADO.Net queries and [dapper](https://github.com/DapperLib/Dapper) micro ORM.

As the data model for Calendar is not suitable for relational storage the decisions were made:
- to store separate id fields (to query them easily) and use them in composite primary key
- store whole CompactCalendar object as json in a text field
- use service-level serialization to not encounter immutable fields deserialization problems

### P.S.
Entity framework was considered as excessive, as it doesn't provide an opportunities to map
domain model to proper database representation:
- It's almost impossible to transform composite Id from subtype to separate fields. 
- System.Text.Json serializer used in postgres for jsonb field requires public setters on model, 
  what I find not a good thing for DDD approach

