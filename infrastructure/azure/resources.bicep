param location string
param name string
param tags object
var abbrs = loadJsonContent('abbreviations.json')

resource acr 'Microsoft.ContainerRegistry/registries@2022-12-01' = {
  name: '${abbrs.containerRegistryRegistries}${name}'
  location: location
  tags: tags
  sku: {
    name: 'Basic'
  }
  properties: {
    adminUserEnabled: true
  }
}

output AZURE_CONTAINER_REGISTRY_ENDPOINT string = acr.properties.loginServer
