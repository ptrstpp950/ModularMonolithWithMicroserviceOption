## How to create new module
Go to template dir:
```
cd template
```

Install it with
```
dotnet new -i .
```
To reinstall template uninstall it first with:
```
dotnet new -u [Full path to template]
```

Create a module with:
```
mkdir ModuleName
cd ModuleName
dotnet new bootloader.module -n ModuleName
```
Now add it manually to SLN and optionally to bootloader project.