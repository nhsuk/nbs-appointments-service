data "azurerm_function_app_host_keys" "nbs_appts_funcapp" {
  name                = azurerm_function_app.nbs_appts_funcapp.name
  resource_group_name = azurerm_resource_group.nbs_appts_rg.name
}

resource "azurerm_resource_group" "nbs_appts_rg" {
  name     = "${var.application}-rg-${var.environment}-${var.loc}"
  location = "${var.location}"
  tags = local.allTags
}

resource "azurerm_storage_account" "nbs_appts_stacc" {
  name                     = "${var.application_short}stacc${var.environment}${var.loc}"
  resource_group_name      = azurerm_resource_group.nbs_appts_rg.name
  location                 = azurerm_resource_group.nbs_appts_rg.location
  account_tier             = "Standard"
  account_replication_type = "LRS"
  tags = local.allTags
}

resource "azurerm_app_service_plan" "nbs_appts_sp" {
  name                = "${var.application}-sp-${var.environment}-${var.loc}"
  resource_group_name = azurerm_resource_group.nbs_appts_rg.name
  location            = azurerm_resource_group.nbs_appts_rg.location
  kind                = "Linux"
  reserved            = true
  tags = local.allTags

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
  name                       = "${var.application}-funcapp-${var.environment}-${var.loc}"
  resource_group_name        = azurerm_resource_group.nbs_appts_rg.name
  location                   = azurerm_resource_group.nbs_appts_rg.location
  app_service_plan_id        = azurerm_app_service_plan.nbs_appts_sp.id
  storage_account_name       = azurerm_storage_account.nbs_appts_stacc.name
  storage_account_access_key = azurerm_storage_account.nbs_appts_stacc.primary_access_key
  os_type                    = "linux"
  version                    = "~4"
  tags = local.allTags
}

resource "azurerm_api_management" "nbs_appts_apim" {
  name                = "${var.application}-apim-${var.environment}-${var.loc}"
  resource_group_name = azurerm_resource_group.nbs_appts_rg.name
  location            = azurerm_resource_group.nbs_appts_rg.location
  publisher_name      = "Aire Logic"
  publisher_email     = "vincent.crowe@airelogic.com"

  sku_name = "Consumption_0"
  tags = local.allTags
}

resource "azurerm_api_management_backend" "nbs_appts_funcapp_backend" {
  name                = "${var.application}-funcapp-backend-${var.environment}-${var.loc}"
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
  name                = "${var.application}-api-${var.environment}-${var.loc}"
  resource_group_name = azurerm_resource_group.nbs_appts_rg.name
  api_management_name = azurerm_api_management.nbs_appts_apim.name
  revision            = "1"
  display_name        = "Appointments API"
  path                = "appts"
  protocols           = ["https"]  

  import {
    content_format = "swagger-json"
    content_value  = file("../../resources/swagger.json")
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
        <set-backend-service backend-id="${var.application}-funcapp-backend-${var.environment}-${var.loc}" />
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