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
