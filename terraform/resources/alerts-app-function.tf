resource "azurerm_resource_group" "nbs_appts_alert_handler_rg" {
  name     = "${var.application}-alert-handler-rg-${var.environment}-${var.loc}"
  location = var.location
  tags     = local.allTags
}

resource "azurerm_service_plan" "nbs_appts_alert_handler_sp" {
  name                = "${var.application}-alerts-sp-${var.environment}-${var.loc}"
  resource_group_name = azurerm_resource_group.nbs_appts_alert_handler_rg.name
  location            = azurerm_resource_group.nbs_appts_alert_handler_rg.location
  os_type             = "Linux"
  sku_name            = "Y1"
}

resource "azurerm_linux_function_app" "nbs_appts_alert_handler_func_app" {
  name                = "${var.application}-alerts-func-${var.environment}-${var.loc}"
  resource_group_name = azurerm_resource_group.nbs_appts_alert_handler_rg.name
  location            = azurerm_resource_group.nbs_appts_alert_handler_rg.location

  storage_account_name       = azurerm_storage_account.nbs_appts_alert_handler_stacc.name
  storage_account_access_key = azurerm_storage_account.nbs_appts_alert_handler_stacc.primary_access_key
  service_plan_id            = azurerm_service_plan.nbs_appts_alert_handler_sp.id

  site_config {}

  app_settings = {
    AppConfig = azurerm_app_configuration.nbs_appts_app_config.primary_read_key[0].connection_string
  }

  identity {
    type = "SystemAssigned"
  }
}

resource "azurerm_storage_account" "nbs_appts_alert_handler_stacc" {
  name                     = "${var.application_short}alerts${var.environment}${var.loc}"
  resource_group_name      = azurerm_resource_group.nbs_appts_alert_handler_rg.name
  location                 = azurerm_resource_group.nbs_appts_alert_handler_rg.location
  account_tier             = "Standard"
  account_replication_type = "LRS"
}

resource "azurerm_storage_container" "nbs_appts_alert_handler_container" {
  name                  = "${var.application_short}alerts${var.environment}${var.loc}"
  storage_account_name  = azurerm_storage_account.nbs_appts_alert_handler_stacc.name
  container_access_type = "private"
}

resource "azurerm_role_assignment" "alerts_func_kv_secrets_user_role" {
  scope = azurerm_key_vault.nbs_appts_key_vault.id
  role_definition_name = "Key Vault Secrets User"
  principal_id = azurerm_linux_function_app.nbs_appts_alert_handler_func_app.identity.0.principal_id
}