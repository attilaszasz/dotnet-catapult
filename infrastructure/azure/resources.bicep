param location string
param environmentName string
param tags object
param azureClientId string
@secure()
param azureClientSecret string

var abbrs = loadJsonContent('abbreviations.json')
var sanitizedEnvironmentName = replace(environmentName, '-', '')

resource acr 'Microsoft.ContainerRegistry/registries@2022-12-01' = {
  name: '${abbrs.containerRegistryRegistries}${sanitizedEnvironmentName}'
  location: location
  tags: tags
  identity: {
    type: 'SystemAssigned'
  }
  sku: {
    name: 'Basic'
  }
  properties: {
    adminUserEnabled: true
  }
}

resource redis 'Microsoft.Cache/redis@2022-06-01' = {
  name: '${abbrs.cacheRedis}${environmentName}'
  location: location
  tags: tags
  identity: {
    type: 'SystemAssigned'
  }
  properties: {
    minimumTlsVersion: '1.2'
    enableNonSslPort: false
    sku: {
      name: 'Basic'
      family: 'C'
      capacity: 0
    }
  }
}

resource keyvault 'Microsoft.KeyVault/vaults@2023-02-01' = {
  name: '${abbrs.keyVaultVaults}${environmentName}'
  location: location
  tags: tags
  properties: {
    sku: {
      family: 'A'
      name: 'standard'
    }
    tenantId: subscription().tenantId
    accessPolicies: []
    enabledForTemplateDeployment: true
    enableSoftDelete: true
    softDeleteRetentionInDays: 7
  }
}

resource storage 'Microsoft.Storage/storageAccounts@2022-09-01' = {
  name: '${abbrs.storageStorageAccounts}${sanitizedEnvironmentName}'
  location: location
  tags: tags
  identity: {
    type: 'SystemAssigned'
  }
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
  identity: {
    type: 'SystemAssigned'
  }
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

resource kvSecretstore 'Microsoft.App/managedEnvironments/daprComponents@2022-10-01' = {
  name: 'secretstore'
  parent: env
  properties: {
    componentType: 'secretstores.azure.keyvault'
    version: 'v1'
    metadata: [
      {
        name: 'vaultName'
        value: keyvault.name
      }
      {
        name: 'azureTenantId'
        value: subscription().tenantId
      }
      {
        name: 'azureClientId'
        value: azureClientId
      }
      {
        name: 'azureClientSecret'
        value: azureClientSecret
      }
    ]
    scopes: [
      'weatherforecast'
    ]
  }
}

resource redisStateStore 'Microsoft.App/managedEnvironments/daprComponents@2022-10-01' ={
  name: 'statestore'
  parent: env
  properties: {
    componentType: 'state.redis'
    version: 'v1'
    metadata: [
      {
        name: 'redisHost'
        value: redis.properties.hostName
      }
      {
        name: 'redisPassword'
        value: redis.listKeys().primaryKey
      }
      {
        name: 'enableTLS'
        value: 'true'
      }
    ]
    scopes: [
      'weatherforecast'
    ]
  }
}

resource containerapp 'Microsoft.App/containerApps@2022-10-01' = {
  name: '${abbrs.appContainerApps}weatherforecast'
  location: location
  identity: {
    type: 'SystemAssigned'
  }
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
        allowInsecure: false
      }
      dapr: {
        enabled: true
        appPort: 80
        appId: 'weatherforecast'
        appProtocol: 'http'
        enableApiLogging: true
      }
    }
    template: {
      containers: [
        {
          name: 'weatherforecast'
          image: '${acr.name}.azurecr.io/weatherforecast:latest'
          resources: {
            cpu: json('.25')
            memory: '.5Gi'
          }
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
