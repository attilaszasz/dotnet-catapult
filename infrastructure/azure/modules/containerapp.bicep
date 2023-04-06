param environmentName string
param image string
param location string
param containerAppName string
param ingress bool = false
param port int = 80

var tags = { 'azd-env-name': environmentName }
var abbrs = loadJsonContent('../abbreviations.json')
var sanitizedEnvironmentName = replace(environmentName, '-', '')

resource env 'Microsoft.App/managedEnvironments@2022-10-01' existing = {
  name: '${abbrs.appManagedEnvironments}${environmentName}'
}

resource acr 'Microsoft.ContainerRegistry/registries@2022-12-01' existing = {
  name: '${abbrs.containerRegistryRegistries}${sanitizedEnvironmentName}'
}

resource containerapp 'Microsoft.App/containerApps@2022-10-01' = {
  name: containerAppName
  location: location
  tags: union(tags, { 'azd-service-name': containerAppName })
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
        external: ingress
        targetPort: port
      }
    }
    template: {
      containers: []
      scale: {
        minReplicas: 1
        maxReplicas: 1
      }
    }
  }
}
