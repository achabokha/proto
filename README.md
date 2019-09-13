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

## For Mac
```bash
cd Server/ClientApp/
ionic cordova build ios
xed platforme/ios/
```

> see https://ionicframework.com/docs/v3/intro/deploying/ for details

If you are running Xcode 8, the code signing error will appear as a buildtime error, rather than as a pop-up:

To select the certificate to sign your app with, do the following:
1. Go to the ‘Project Editor’ by clicking the name or your project in the ‘Project Navigator’
2. Select the ‘General’ section
3. Update "Bundle Identifier" to something unique
4. Select the team associate with your signing certificate from the ‘Team’ dropdown in the ‘Signing’ section

Run and build in Xcode, this should launch simulator.


# Run on iPhone itself 

Connect your IPhone to Mac, in the XCode settings select your IPhone as run the run environment

This should deploy app to your phone before running it you need to trust certificate with which app was signed

![Xcode platform selection](readme/xcode.jpg?raw=true "Xcode platform selection")
![Pick your Iphone](readme/IphoneSelect.png?raw=true "Pick your Iphone")


1. Open the ‘Settings’ app on your iOS device
2. Go to ‘General > Device Management’. You’ll see the email address associated with the Apple ID or Apple Developer account you used to code sign your app.
3. Tap the email address
4. Tap ‘Trust <your_email>’:

# Database/EF Errors

if you run into SQL errors about missing fields, tables etc...

```bash
cd Models
dotnet ef database update init
dotent ef database update
```

This should reset DB to initial migration and after that apply all migrations sequentially 

# Integrating Google Sign-In

Full guide here https://developers.google.com/identity/sign-in/web/sign-in?refresh=1

1. Create Google API Console project and client ID 
  1.Find Configure A project in https://developers.google.com/identity/sign-in/web/sign-in
  2. Select Web Browser as your  app environment
  3. If you need to change or edit any params visit https://console.developers.google.com
  