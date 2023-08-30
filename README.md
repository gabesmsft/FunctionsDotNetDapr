# Dapr with Function App on Container Apps


[![Deploy To Azure](https://raw.githubusercontent.com/Azure/azure-quickstart-templates/master/1-CONTRIBUTION-GUIDE/images/deploytoazure.svg?sanitize=true)](https://portal.azure.com/#create/Microsoft.Template/uri/https%3A%2F%2Fraw.githubusercontent.com%2Fgabesmsft%2FFunctionsDotNetDapr%2Fmaster%2Fdeploy%2Fazuredeploy.json)  [![Visualize](https://raw.githubusercontent.com/Azure/azure-quickstart-templates/master/1-CONTRIBUTION-GUIDE/images/visualizebutton.svg?sanitize=true)](http://armviz.io/#/?load=https%3A%2F%2Fraw.githubusercontent.com%2Fgabesmsft%2FFunctionsDotNetDapr%2Fmaster%2Fdeploy%2Fazuredeploy.json)

This sample Azure Resource Manager template deploys a Function App on Container Apps, and several Dapr components.

This application is only for demonstration purposes and is not intended as a production application or as official instructions. Use discretion and best practices in your usage of this application and template.

## Prerequisites

- Deploy a Container App Environment. For now, it cannot be a workload profile-enabled Environment. You can use [this template](https://github.com/azureossd/Container-Apps/tree/master/ContainerAppEnvironment/deploy) to deploy a Container App Environment, but be sure to set the workloadProfileEnabled parameter to false.
- Optional: If you set the DisableDaprSecretComponent to false, the secrets component uses an Azure KeyVault. To authenticate to the Keyvault, you must use an AAD application because Managed Identity is currently not available for Function Apps on Container Apps. Create an AAD app registration and an AAD app registration secret, and note down the following, which you will need to enter when you run the template deployment:
  - The AAD app registration secret
  - The AAD tenant id
  - The AAD application id (client id)
  - The Enterprise application object id. This is **not** the app registration object id.

## Function App
This Function App does not use the default /api/ base route. Instead, the base route is /, which is configured in the host.json. In the Azure portal, the Function urls might show /api/, which you wil; need to remove the api/ portion of the URL to test the HTTP triggers.

Currently, Dapr triggers do not work in Function App on Container Apps. Dapr triggers should be able to work with Functions locally and in a regular AKS or non-PaaS Kubernetes environment.
Any references to triggering Dapr in the subsequent sections are not intended for testing within Function App on Container Apps.

## Functions

| Function name | Dapr component type(s) | HTTP method (if applicable) | HTTP route (if applicable) | Description |
| ------------- | ---------------------- | --------------------------- | -------------------------- | ----------- |
| DaprSubscribe | Pubsub trigger| GET | /dapr/subscribe | This Function is not intended for you to trigger. This Function is present to enable programmatic PubSub subscription. This Function will automatically run one time upon startup, so that the Pubsub consumer/trigger can listen for events. |
| HttpTriggerWithDaprPubSubAPIOutput | Pubsub output / publisher | POST | /mypubsubpublisher | Publishes a message to the pubsub component, which in turn should trigger |
| HttpTriggerWithDaprPubSubAPITrigger | Pubsub trigger / consumer | POST | /mypubsubconsumer | Triggers when a message is put in the pubsub component |
| HttpTriggerWithDaprBindingAPITriggerAndOutput | Binding trigger and output | POST | /mybinding1 | Listens for message in the mybinding1 component (a Service Bus queue), and when triggered, writes a message to the mybinding2 component (a Storage queue). You can also manually trigger this Function to put a message in the mybinding2 component. |
| HttpTriggerWithDaprBindingSDKOutput | Binding output | POST | /bindingviasdk | Writes a message to the mybinding1 component (a Service Bus queue), which in turn should trigger the HttpTriggerWithDaprBindingAPITriggerAndOutput Function. |
| HttpTriggerWithDaprSecretAPI | Secret output | GET | /mysecretviadaprapi | Gets a secret from the secret store, using the Dapr API. |
| HttpTriggerWithDaprSecretExtension | Secret output | GET | /daprsecretextension | Gets a secret from the secret store, using the Dapr Functions extension. |
| HttpTriggerWithDaprSecretSDK | Secret output | GET | /mysecretviadaprsdk | Gets a secret from the secret store, using the Dapr SDK. |
| HttpTriggerWithDaprServiceInvocationAPIfrontend | Service invocation (front end) | GET | /daprserviceinvocationviaapi| Makes an HTTP request to the HttpTriggerWithDaprServiceInvocationbackend via Dapr internally within the Environment rather than over the Internet. Returns the backend Function's response body via the HttpTriggerWithDaprServiceInvocationAPIfrontend request.  |
| HttpTriggerWithDaprServiceInvocationbackend | Service invocation (front end) | GET | /daprserviceinvocationbackend | A regular HTTP Trigger Function that returns a response body. The purpose of this Function is to test the HttpTriggerWithDaprServiceInvocationAPIfrontend Function's ability to make a request to it via Dapr service invocation. No need to manually trigger this Function, unless troubleshooting why HttpTriggerWithDaprServiceInvocationAPIfrontend is not receiving the expected response. |
| HttpTriggerWithDapStateOutputAPI | State output | GET | /writetostatestore | Writes the current date/time to the state store and returns the written value in the HTTP response. |
| HttpTriggerWithDapStateInputAPI | State input | GET | /readfromstatestore | Reads the latest value that HttpTriggerWithDapStateOutputAPI wrote to the state store. |

## Local testing
There are different ways to run Docker locally. This is not a definitive guide or otherwise intended as official instructions. Install software at your own risk. 
Assuming you are using a Windows machine:
Install Docker Desktop, Dapr, and .NET 6 SDK or later, if they are not already installed.

To run the application locally:
```
dapr run --app-id functionapp1 --dapr-http-port 3500 --app-port 7071 --components-path ./components -- func host start 
```

To publish a message to the pubsub:

```
curl -H "Content-Type: application/json" -X POST http://localhost:7071/mypubsubpublisher -d "{"status": "completed"}"
```

To put a message in the mybinding1 component:

```
curl -H "Content-Type: application/json" -X POST http://localhost:7071/bindingviasdk -d "{\"i like\":\"bacon\"}"
```
