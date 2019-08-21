# Proto - Foundry Row

# Development Environment Setup

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

## Run on Mac

Make sure 

```bash
dotnet --version 
```

shows 2.2 

if not, install 2.2 
> in September we shell move to 3.0

in VS Code hit F5

it should restore .net and npm packages, compile and run the server. 

now run the client app... 

```bash
cd Server/ClientApp
npm start
```

the server proxy localhost:5000 to localhost:4200 for the angular client.

# Run on iPhone Simulator

Andrej's will write it up...

# Run on iPhone itself 

Andrej's will write it up...

