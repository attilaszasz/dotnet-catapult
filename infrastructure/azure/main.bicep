targetScope = 'subscription'

@minLength(1)
@maxLength(50)
@description('Name of the environment')
param environmentName string

param location string = resourceGroup().location

param azureClientId string
@secure()
param azureClientSecret string

var tags = { 'azd-env-name': environmentName }

resource rg 'Microsoft.Resources/resourceGroups@2022-09-01' = {
  name: environmentName
  location: location
  tags: tags
}

module resources 'resources.bicep' = {
  name: 'resources'
  scope: rg
  params: {
    environmentName: environmentName
    location: location
    tags: tags
    azureClientId: azureClientId
    azureClientSecret: azureClientSecret
  }
}

output AZURE_LOCATION string = location
output AZURE_CONTAINER_REGISTRY_ENDPOINT string = resources.outputs.AZURE_CONTAINER_REGISTRY_ENDPOINT
