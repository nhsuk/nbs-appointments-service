FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /App

# Copy everything
COPY ./NBS.Appointments.Service ./NBS.Appointments.Service
COPY ./NBS.Appointments.Service.Core ./NBS.Appointments.Service.Core

WORKDIR /App/NBS.Appointments.Service
# Restore as distinct layers
RUN dotnet restore
# Build and publish a release
RUN dotnet publish -c Release -o ../out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /App
COPY --from=build-env /App/out .
ENTRYPOINT ["dotnet", "NBS.Appointments.Service.dll"]