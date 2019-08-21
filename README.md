# Proto


## Setup MS SQL on Mac 

1. Install Docker container 

https://hub.docker.com/_/microsoft-mssql-server

2. Run it 

```bash 
docker run -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=P@55w0rd;' -p 1433:1433 -d mcr.microsoft.com/mssql/server:2017-CU8-ubuntu
```
> `P@55w0rd;` this all thing the password including `;`

> you can also see docker container running via Docker->Kitematic (look up for docker image in upper-right Mac menu)

3. Get VS Code plug-in for MS SQL 

4. Create databaser 

``` sql
CREATE DATABASE FoundryRowDBLocalMac;
```

> useful: check the list of databases 

```sql
SELECT name, database_id, create_date FROM sys.databa
ses;
```  

