# Embily

The application compose of:
1. Embily Services - customer's portal
2. Embily Admin - administrative portal
3. MS SQL Database EmbilyDBLocal

### Setup 

Embily Services

```bash
$ cd EmbilyServices\ClientApp
$ npm install 
```

Embily Admin

```bash
$ cd EmbilyAdmin\ClientApp
$ npm install 
```

### Run 

##### Embily Services

1. Open Embily.sln with Visual Studio 2017
2. Setup EmbilyServices as a start up project.
3. hit Ctrl-F5
4. From Powershell or Cmd: $ cd EmbilyServices\ClientApp
5. Run angural cli: $ ng build --watch
 

##### Embily Admin

1. Set EmbilyAdmin as a start project 
2. Ctrl-F5

Note: it will start angular cli to compile angular code. 

### Notes 

1. The database, (localdb)\MSSQLocalDB\EmbilyDBLocal should be created and seeded automatically 
on the first run. 
2. Credentials EmbilyServices: achabokha@hotmail.com / Pa$$w0rd!
3. Credentials EmbilyAdmin: admin@embily.com / Pa$$w0rd!