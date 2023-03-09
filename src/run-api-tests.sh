#!/bin/bash

docker compose up --build -d
cd NBS.Appointments.Service.Api.Tests && dotnet test