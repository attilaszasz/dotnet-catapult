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

resource storage 'Microsoft.Storage/storageAccounts@2022-09-01' = {
  name: '${abbrs.storageStorageAccounts}${sanitizedEnvironmentName}'
  location: location
  tags: tags
  sku: {
    name: 'Standard_LRS'
  }
  kind: 'StorageV2'
  properties: {
    minimumTlsVersion: 'TLS1_2'
    allowBlobPublicAccess: false
    networkAcls: {
      bypass: 'AzureServices'
      defaultAction: 'Allow'
    }
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

resource ai 'Microsoft.Insights/components@2020-02-02' = {
  name: '${abbrs.insightsComponents}${environmentName}'
  location: location
  tags: tags
  kind: 'web'
  properties: {
    Application_Type: 'web'
    WorkspaceResourceId: logs.id
  }
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

resource containerapp 'Microsoft.App/containerApps@2022-10-01' = {
  name: '${abbrs.appContainerApps}${environmentName}'
  location: location
  tags: union(tags, { 'azd-service-name': '${abbrs.appContainerApps}${environmentName}' })
  properties: {
    managedEnvironmentId: env.id
    configuration: {
      activeRevisionsMode: 'single'
      secrets: [
        {
          name: 'container-registry-password'
          value: acr.listCredentials().passwords[0].value
        }
      ]
      registries: [
        {
          server: '${acr.name}.azurecr.io'
          username: acr.name
          passwordSecretRef: 'container-registry-password'
        }
      ]
      ingress: {
        external: true
        targetPort: 80
      }
    }
    template: {
      containers: [
        {
          image: 'weatherforecastservice-api:latest'
          name: 'weatherforecast'
          env: [
            {
              name: 'ASPNETCORE_ENVIRONMENT'
              value: 'Development'
            }
          ]
        }
      ]
      scale: {
        minReplicas: 1
        maxReplicas: 1
      }
    }
  }
}

output AZURE_CONTAINER_REGISTRY_ENDPOINT string = acr.properties.loginServer
