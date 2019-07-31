# Angrular 6 + Bootstrap 4 Migration
> - No jQuery.js, no popper.js, no bootstrap.js, no crap
> - use ng-bootstrap instead of bootsrap.js for all clicky functinality such as modals
> - ng2-smart-table is replaced by DataTable (@swimlane/ngx-datatable)
> - prod compilaiton supported (test with all new components and packages)
> - .csproj configured to test prod build when running in release mode 


## Running EmbilyServices  

### Option 1 Server and Client Separately  
in appsettings.Development.json set

RunAsProxy: true 

start ng server from PowerShell
``` 
cd ClientApp
ng serve 
```

then start .net server part in VS. For debugging F5 or just Ctrl-F5 without debugging 

### Option 2. Start with .NET 

in appsettings.Development.json set

RunAsProxy: false 

then start server part in VS, debugging F5 or just Ctrl-F5

> it is stil will start ng server but from .net 


## To Test Angular app in prod mode (prod build) 

### Option 1. Command line prod build/serve:

``` shell
cd ClientApp
ng serve --prod
```

also try with optimizer 

``` shell
ng build --prod --build-optimizer
```

that's it. 

>The first page should have NO db calls for faster load 


### Option 2. Just run .net project in Release mode 

it will launch release npm command and ng (angular cli) 'ng serve --prod --build-optimizer'

> It might take a few minutes for prod build to compile.

Build Note (from Angular 6 docs): 

The --prod meta-flag engages the following optimization features.

- Ahead-of-Time (AOT) Compilation: pre-compiles Angular component templates.
- Production mode: deploys the production environment which enables production mode.
- Bundling: concatenates your many application and library files into a few bundles.
- Minification: removes excess whitespace, comments, and optional tokens.
- Uglification: rewrites code to use short, cryptic variable and function names.
- Dead code elimination: removes unreferenced modules and much unused code.

it does not do only server-side-rendering 

# Helpers

## Create module example:

``` shell
ng g module Shared --module=app.module
```

## Pre-requisites 

1. .net core SDK 2.1 (https://www.microsoft.com/net/download/dotnet-core/2.1)
2. nodejs 8.11.3 LTS (https://nodejs.org/en/download/)
3. angular_cli 6 
```
npm install -g @angular/cli
```

