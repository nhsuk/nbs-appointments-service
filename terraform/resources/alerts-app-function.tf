resource "azurerm_resource_group" "nbs_alerthandler_rg" {
  name     = "${var.alert_handler_application}-rg-${var.environment}-${var.loc}"
  location = var.location
  tags     = local.allTags
}

resource "azurerm_service_plan" "nbs_alerthandler_sp" {
  name                = "${var.alert_handler_application}-sp-${var.environment}-${var.loc}"
  resource_group_name = azurerm_resource_group.nbs_alerthandler_rg.name
  location            = azurerm_resource_group.nbs_alerthandler_rg.location
  os_type             = "Linux"
  sku_name            = "Y1"
}

resource "azurerm_linux_function_app" "nbs_alerthandler_func" {
  name                = "${var.alert_handler_application}-func-${var.environment}-${var.loc}"
  resource_group_name = azurerm_resource_group.nbs_alerthandler_rg.name
  location            = azurerm_resource_group.nbs_alerthandler_rg.location

  storage_account_name       = azurerm_storage_account.nbs_alerthandler_strg.name
  storage_account_access_key = azurerm_storage_account.nbs_alerthandler_strg.primary_access_key
  service_plan_id            = azurerm_service_plan.nbs_alerthandler_sp.id

  site_config {}

  app_settings = {
    AppConfig = azurerm_app_configuration.nbs_appts_wac.primary_read_key[0].connection_string
  }

  identity {
    type = "SystemAssigned"
  }
}

resource "azurerm_storage_account" "nbs_alerthandler_strg" {
  name                     = "${var.alert_handler_application_short}strg${var.environment}${var.loc}"
  resource_group_name      = azurerm_resource_group.nbs_alerthandler_rg.name
  location                 = azurerm_resource_group.nbs_alerthandler_rg.location
  account_tier             = "Standard"
  account_replication_type = "LRS"
}

resource "azurerm_storage_container" "nbs_alerthandler_strgcont" {
  name                  = "${var.alert_handler_application_short}strgcont${var.environment}${var.loc}"
  storage_account_name  = azurerm_storage_account.nbs_alerthandler_strg.name
  container_access_type = "private"
}