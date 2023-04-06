targetScope = 'subscription'

@minLength(1)
@maxLength(50)
@description('Name of the environment')
param environmentName string

@minLength(1)
@description('Location for all resources')
param location string

var tags = { 'azd-env-name': environmentName }

resource rg 'Microsoft.Resources/resourceGroups@2022-09-01' = {
  name: environmentName
  location: location
  tags: tags
}

module resources 'resources.bicep' = {
  name: 'resources'
  scope: resourceGroup(environmentName)
  params: {
    environmentName: environmentName
    location: location
    tags: tags
  }
}

output AZURE_LOCATION string = location
output AZURE_CONTAINER_REGISTRY_ENDPOINT string = resources.outputs.AZURE_CONTAINER_REGISTRY_ENDPOINT
