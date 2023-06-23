# Development Information

## Running in docker

From the `./src` folder run 

```
docker compose up --build
```

This will build the api (in docker) and bring up 3 services. The mock-api (for simulating qflow responses), the azurite service (for simulating azure storage services) and the appointments api. The docker-compose file can be tweaked to alter how the services run, by default the api will be available via `http://localhost:4000`

If you want to run the service via an IDE in order to debug, you can run the two support service using

```
docker compose up mock-api azurite
```

You can then run the service via the IDE and attach a debugger

## Running the API tests

Bring up the service via 

```
docker compose up --build
```

Once the services are running open a terminal in the `./src/NBS.Appointments.Service.Api.Tests` folder and run

```
dotnet test
```
## Application Configuration and Key Vault 
The appointments service uses Azure App Configuration to store configuration values and Azure Key Vault to store secrets. The App Configuration accesses secrets stored in the Key Vault using Key Vault references.  

### Managing environmental config via Azure DevOps pipeline variables 
Environment-specific config values, as needed by Terraform, are defined as variables in a pipeline variable group with the TF_VAR_ prefix, these are mapped as environmental variables in the pipeline agent which will then map to the corresponding input variable in Terraform. The name of the variable (after the prefix) needs to match the name of the input variable in Terraform.

Below are the steps on how to create pipeline variables and map them to Terraform input variables for deployment. `qflow_user_id` is used as an example.

1. Create the pipeline variable in the variable group. The Appointment Service variable groups are named using the following scheme: `NBS Appts - <environment name>` 

Example pipeline variable definition using TF_VAR_:
```
TF_VAR_QFLOW_USER_ID = <value>
```

2. Define the input variable in `main.tf` for the environment. The variable needs to be defined in **capital letters** as Azure DevOps always transforms pipeline variables to uppercase environmental variables.
```
variable "QFLOW_USER_ID" {
  type = string
}
```
3. As the Appointment Service uses terraform modules, the variable value will need to be piped through to the module by adding it to the module declaration in `main.tf`.

```
module "api" {
  source                       = "../../resources"
  qflow_user_id                = var.QFLOW_USER_ID
}
```
4. Finally, the variable will need to be defined in `variables.tf` of the module itself   
```
variable "qflow_user_id" {
  type      = string
}
```

### Secret management via Azure DevOps pipeline secret variables
Secrets are defined as **secret variables** in a pipeline variable group. Secret variables are encrypted variables that you can use in pipelines without exposing their value (read official documentation [here](https://learn.microsoft.com/en-us/azure/devops/pipelines/process/set-secret-variables?view=azure-devops&tabs=yaml%2Cbash)). To make a variable secret, click the 'lock' icon when adding it to the variable group - the value will now be hidden. Azure Devops will mask the values of any secret variables in log output. 

As these variables are saved as secrets, it is not possible to pass these as environment variables using the TF_VAR_ prefix. Instead, these must be passed using command-line arguments. <span style="color:red">**Never directly pass secrets on the command line. Instead, map the secrets to an environment variable using additional command arguments in the pipeline. See step 2 below.**</span>.

Below are the steps on how to create secrets and map them to Terraform input variables for deployment. `qflow_password` is used as an example.

1. Create the pipeline variable in the variable group used in the release. The Appointment Service variable groups are named using the following scheme: `NBS Appts - <environment name>` Ensure the 'lock' icon is clicked to mark the variable as secret. Value will be hidden when locked. 
```
QFlowPassword = ********
```

2. Map the variable secret to an environmental variable in the 'Addition command arguments in the 'Terraform Apply' task.
```
-var="qflow_password=$(QFlowPassword)"
```

3. Define the variable in `main.tf` for the environment. The variable should be declared as sensitive, Terraform will then redact these values in the output of Terraform commands or log messages.
```
variable "qflow_password" {
  type      = string
  sensitive = true
}
```

4. As Appointment Service uses Terraform modules, the variable value will need to be piped through to the module by adding it to the module declaration.

```
module "api" {
  source                       = "../../resources"
  qflow_password               = var.qflow_password
}
```

5. Finally, the variable will need to be defined in `variables.tf` of the module itself. Again declare it as sensitive.   
```
variable "qflow_password" {
  type      = string
  sensitive = true
}
```

## Metrics Monitoring using Azure Dashboards
An Azure shared dashboard has been created as a single place to view key metrics for the appointments service across all deployed regions. Each environment will have a dashboard. The metrics chosen follow the RED pattern for monitoring applications with a micro-service architecture. 
* Rate: the number of requests the service is serving per second;
* Error: the number of failed requests per second;
* Duration: the amount of time it takes to process a request;

The dashboard is deployed using Terraform and is configured to be automatically deployed alongside the appointments service. Further information on managing a shared dashboard in Azure portal via Terraform can be found [here](https://registry.terraform.io/providers/hashicorp/azurerm/latest/docs/resources/portal_dashboard).  

To view the dashboard in Azure portal once deployed, follow these steps: 
1. In the Azure portal, select the menu at the top left of the screen.
2. Select Dashboard from the menu. Your default Azure dashboard will appear.
3. Select the arrow next to the dashboard name.
4. Select Browse all dashboards.
5. Search for `Appointment Service Metrics`.
6. Select the dashboard corresponding to the environment of interest.
