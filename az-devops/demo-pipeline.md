# Fichier de la pipeline pour partie CI (Continuous Integration) sur Azure DevOps

```yaml
# Starter pipeline
# Start with a minimal pipeline that you can customize to build and deploy your code.
# Add steps that build, run tests, deploy, and more:
# https://aka.ms/yaml

trigger:
- master

variables: 
  buildConfiguration: 'Release'
  solutionPath: '**/DogApi.sln'
  projectPath: '**/DogApi.csproj'

pool:
  name: cd-agp
  demands:
  - agent.name -equals WORKER

steps: 
- task: NuGetToolInstaller@1
  name: 'NugetToolInstaller'
  inputs:
    versionSpec: '6.x'
    checkLatest: true

- task: NuGetCommand@2
  name: 'NuGetRestore'
  inputs:
    command: 'restore'
    restoreSolution: '$(solutionPath)'
    feedsToUse: 'select'

- task: DotNetCoreCLI@2
  name: 'DotnetBuild'
  inputs:
    command: 'build'
    projects: '$(projectPath)'

- task: DotNetCoreCLI@2
  name: 'DotnetPublish'
  inputs:
    command: 'publish'
    publishWebProjects: true
    arguments: '--configuration $(buildConfiguration) --output $(Build.ArtifactStagingDirectory)'
    zipAfterPublish: true

- task: PublishBuildArtifacts@1
  name: 'PublishArtifactOnAzArtifacts'
  inputs:
    PathtoPublish: '$(Build.ArtifactStagingDirectory)'
    ArtifactName: 'dogapi-artifact'
    publishLocation: 'Container'
```