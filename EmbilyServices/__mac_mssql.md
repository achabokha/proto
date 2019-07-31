
# Get latest docker image for MS SQL

sudo docker pull mcr.microsoft.com/mssql/server:2017-latest

# Create Docker Container named sql1

sudo docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=P@55w0rd" -p 1433:1433 --name sql1 -d mcr.microsoft.com/mssql/server:2017-latest

# Start bash shell inside sql1 container

sudo docker exec -it sql1 "bash"

# Run SQL cmd

/opt/mssql-tools/bin/sqlcmd -S localhost -U SA -P "P@55w0rd"
