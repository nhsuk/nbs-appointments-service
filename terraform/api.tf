terraform {
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "~> 3.0.2"
    }
  }

  required_version = ">= 1.1.0"
}

provider "azurerm" {
  features {}
}

data "azurerm_function_app_host_keys" "nbs_appts_funcapp" {
  name                = azurerm_function_app.nbs_appts_funcapp.name
  resource_group_name = azurerm_resource_group.nbs_appts_rg.name
}

resource "azurerm_resource_group" "nbs_appts_rg" {
  name     = "nbs-appts-rg-dev"
  location = "UK South"
}

resource "azurerm_storage_account" "nbs_appts_stacc" {
  name                     = "nbsapptsstaccdev"
  resource_group_name      = azurerm_resource_group.nbs_appts_rg.name
  location                 = azurerm_resource_group.nbs_appts_rg.location
  account_tier             = "Standard"
  account_replication_type = "LRS"
}

resource "azurerm_app_service_plan" "nbs_appts_sp" {
  name                = "nbs-appts-sp-dev"
  resource_group_name = azurerm_resource_group.nbs_appts_rg.name
  location            = azurerm_resource_group.nbs_appts_rg.location
  kind                = "Linux"
  reserved            = true

  sku {
    tier = "Dynamic"
    size = "Y1"
  }

  lifecycle {
    ignore_changes = [
      kind
    ]
  }
}

resource "azurerm_function_app" "nbs_appts_funcapp" {
  name                       = "nbs-appts-funcapp-dev"
  resource_group_name        = azurerm_resource_group.nbs_appts_rg.name
  location                   = azurerm_resource_group.nbs_appts_rg.location
  app_service_plan_id        = azurerm_app_service_plan.nbs_appts_sp.id
  storage_account_name       = azurerm_storage_account.nbs_appts_stacc.name
  storage_account_access_key = azurerm_storage_account.nbs_appts_stacc.primary_access_key
  os_type                    = "linux"
  version                    = "~4"
}

resource "azurerm_api_management" "nbs_appts_apim" {
  name                = "nbs-appts-apim-dev"
  resource_group_name = azurerm_resource_group.nbs_appts_rg.name
  location            = azurerm_resource_group.nbs_appts_rg.location
  publisher_name      = "Aire Logic"
  publisher_email     = "vincent.crowe@airelogic.com"

  sku_name = "Consumption_0"
}

resource "azurerm_api_management_backend" "nbs_appts_funcapp_backend" {
  name                = "nbs-appts-funcapp-backend-dev"
  resource_group_name = azurerm_resource_group.nbs_appts_rg.name
  api_management_name = azurerm_api_management.nbs_appts_apim.name
  protocol            = "http"
  url                 = "https://${azurerm_function_app.nbs_appts_funcapp.name}.azurewebsites.net/api/"
  credentials {
    header = {
      "x-functions-key" = "${data.azurerm_function_app_host_keys.nbs_appts_funcapp.default_function_key}"
    }
  }
}

resource "azurerm_api_management_api" "nbs_appts_api" {
  name                = "nbs-appts-api-dev"
  resource_group_name = azurerm_resource_group.nbs_appts_rg.name
  api_management_name = azurerm_api_management.nbs_appts_apim.name
  revision            = "1"
  display_name        = "Appointments API"
  path                = "appts"
  protocols           = ["https"]

  import {
    content_format = "swagger-json"
    content_value  = file("swagger.json")
  }
}

resource "azurerm_api_management_api_policy" "nbs_appts_api_pol" {
  api_name            = azurerm_api_management_api.nbs_appts_api.name
  resource_group_name = azurerm_resource_group.nbs_appts_rg.name
  api_management_name = azurerm_api_management.nbs_appts_apim.name

  xml_content = <<XML
<policies>
    <inbound>
        <base />
        <set-backend-service backend-id="nbs-appts-funcapp-backend-dev" />
    </inbound>
    <backend>
        <base />
    </backend>
    <outbound>
        <base />
    </outbound>
    <on-error>
        <base />
    </on-error>
</policies>
XML
}