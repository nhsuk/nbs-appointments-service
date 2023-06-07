resource "azurerm_resource_group" "nbs_appts_alerthandler_rg" {
  name     = "${var.application}-alerthandler-rg-${var.environment}-${var.loc}"
  location = var.location
  tags     = local.allTags
}

resource "azurerm_service_plan" "nbs_appts_alerthandler_sp" {
  name                = "${var.application}-alerthandler-sp-${var.environment}-${var.loc}"
  resource_group_name = azurerm_resource_group.nbs_appts_alerthandler_rg.name
  location            = azurerm_resource_group.nbs_appts_alerthandler_rg.location
  os_type             = "Linux"
  sku_name            = "Y1"
}

resource "azurerm_linux_function_app" "nbs_appts_alerthandler_func" {
  name                = "${var.application}-alerthandler-func-${var.environment}-${var.loc}"
  resource_group_name = azurerm_resource_group.nbs_appts_alerthandler_rg.name
  location            = azurerm_resource_group.nbs_appts_alerthandler_rg.location

  storage_account_name       = azurerm_storage_account.nbs_appts_alerthandler_strg.name
  storage_account_access_key = azurerm_storage_account.nbs_appts_alerthandler_strg.primary_access_key
  service_plan_id            = azurerm_service_plan.nbs_appts_alerthandler_sp.id

  site_config {}

  app_settings = {
    AppConfig = azurerm_app_configuration.nbs_appts_wac.primary_read_key[0].connection_string
  }

  identity {
    type = "SystemAssigned"
  }
}

resource "azurerm_storage_account" "nbs_appts_alerthandler_strg" {
  name                     = "${var.application_short}alerthandlerstrg${var.environment}${var.loc}"
  resource_group_name      = azurerm_resource_group.nbs_appts_alerthandler_rg.name
  location                 = azurerm_resource_group.nbs_appts_alerthandler_rg.location
  account_tier             = "Standard"
  account_replication_type = "LRS"
}

resource "azurerm_storage_container" "nbs_appts_alerthandler_strgcont" {
  name                  = "${var.application_short}alerthandlerstrgcont${var.environment}${var.loc}"
  storage_account_name  = azurerm_storage_account.nbs_appts_alerthandler_strg.name
  container_access_type = "private"
}

resource "azurerm_role_assignment" "alerthandler_keyvaultsecretsuser_role" {
  scope                = azurerm_key_vault.nbs_appts_kv.id
  role_definition_name = "Key Vault Secrets User"
  principal_id         = azurerm_linux_function_app.nbs_appts_alerthandler_func.identity.0.principal_id
}
