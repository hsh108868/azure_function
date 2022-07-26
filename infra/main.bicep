// 파라미터, 배리어블, 리소스, 아웃풋 네가지가 가장 중요하다 리소스는 무조건 있어야 함

var rg = 'rg-${name}-${env}-${loc}'
var fncappname = 'rg-${name}-${env}-${loc}'

param name string
param env string = 'dev'
param loc string = 'krc'
param location string = resourceGroup().location

output rn string = rg

//리소스는 원하는 만큼 넣을 수 잇다.

resource csplan 'Microsoft.Web/sites@2021-03-01' = {
  name: 'csplan-${name}-${loc}'
  location: location
  kind: functionapp
  sku: {
    name: 'Y1'
    tier: 'Dynamic'
    size: 'Y1'
    family: 'Y'
    capacity: 0
  }
  properties: {
    reserved: true
  }
}
resource fncapp 'Microsoft.Web/sites@2016-08-01' = {
  name: fncappname
  location: location
  properties: {
    serverFarmId: csplan.id
  }
}
