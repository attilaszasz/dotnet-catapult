# DotNet Catapult

## Prerequisites

- Visual Studio 2022, .net 7 SDK
- [Dapr minimal](guides/install-dapr.md) installed on local environment

## Set up local development environment

Set environment variable `CATAPULT_DAPR_FOLDER` to point to the `<ProjectFolder>\infrastructure\local\dapr\components` folder.


Example using powershell (*run as administrator*):

```powershell
[Environment]::SetEnvironmentVariable('CATAPULT_DAPR_FOLDER', "C:\Work\Projects\dotnet-catapult\infrastructure\local\dapr\components", "Machine")
```

Restart Visual Studio if it was running.  

Create a `secrets.json` file *outside* of the repository folder. Make sure [secretstore.yaml](../infrastructure/local/dapr/components/secretstore.yaml) is pointing to it.

secrets.json example:
```json
{
    "OpenWeatherAPIToken": "[your token]",
    "Caching": {
        "Local": "false",
        "Shared": "true"
    }
}
```