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
