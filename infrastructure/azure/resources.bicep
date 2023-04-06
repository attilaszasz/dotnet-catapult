param location string
param environmentName string
param tags object
var abbrs = loadJsonContent('abbreviations.json')
var sanitizedEnvironmentName = replace(environmentName, '-', '')

resource acr 'Microsoft.ContainerRegistry/registries@2022-12-01' = {
  name: '${abbrs.containerRegistryRegistries}${sanitizedEnvironmentName}'
  location: location
  tags: tags
  sku: {
    name: 'Basic'
  }
  properties: {
    adminUserEnabled: true
  }
}

resource logs 'Microsoft.OperationalInsights/workspaces@2022-10-01' = {
  name: '${abbrs.operationalInsightsWorkspaces}${environmentName}'
  location: location
  tags: tags
  properties: any({
    retentionDays: 30
    features: {
      searchVersion: 1
    }
    sku: {
      name: 'PerGB2018'
    }
  })
}

resource env 'Microsoft.App/managedEnvironments@2022-10-01' = {
  name: '${abbrs.appManagedEnvironments}${environmentName}'
  location: location
  tags: tags
  properties: {
    appLogsConfiguration: {
      destination: 'log-analytics'
      logAnalyticsConfiguration: {
        customerId: logs.properties.customerId
        sharedKey: logs.listKeys().primarySharedKey
      }
    }
  }
}

output AZURE_CONTAINER_REGISTRY_ENDPOINT string = acr.properties.loginServer
