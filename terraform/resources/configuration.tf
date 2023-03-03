resource "azurerm_app_configuration" "nbs_appts_app_config" {
  name                = "${var.application}-app-config-${var.environment}-${var.loc}"
  resource_group_name = azurerm_resource_group.nbs_appts_rg.name
  location            = azurerm_resource_group.nbs_appts_rg.location
}

data "azurerm_client_config" "current" {}

resource "azurerm_role_assignment" "appconf_dataowner" {
  scope                = azurerm_app_configuration.nbs_appts_app_config.id
  role_definition_name = "App Configuration Data Owner"
  principal_id         = data.azurerm_client_config.current.object_id
}

resource "azurerm_key_vault" "nbs_appts_key_vault" {
  name                       = "${var.application}-key-vault-${var.environment}-${var.loc}"
  resource_group_name        = azurerm_resource_group.nbs_appts_rg.name
  location                   = azurerm_resource_group.nbs_appts_rg.location
  tenant_id                  = data.azurerm_client_config.current.tenant_id
  sku_name                   = "standard"
  soft_delete_retention_days = 7

  access_policy {
    tenant_id = data.azurerm_client_config.current.tenant_id
    object_id = data.azurerm_client_config.current.object_id

    key_permissions = [
      "Create",
      "Get",
    ]

    secret_permissions = [
      "Set",
      "Get",
      "Delete",
      "Purge",
      "Recover"
    ]
  }
}

resource "azurerm_key_vault_secret" "kv_qflow_username" {
  name         = "qflowusername"
  value        = "default"
  key_vault_id = azurerm_key_vault.nbs_appts_key_vault.id
}

resource "azurerm_key_vault_secret" "kv_qflow_password" {
  name         = "qflowpassword"
  value        = "default"
  key_vault_id = azurerm_key_vault.nbs_appts_key_vault.id
}

resource "azurerm_app_configuration_key" "config_qflow_username" {
  configuration_store_id = azurerm_app_configuration.nbs_appts_app_config.id
  key                    = "Qflow:UserName"
  type                   = "vault"
  label                  = "Qflow UserName"
  vault_key_reference    = azurerm_key_vault_secret.kv_qflow_username.versionless_id

  depends_on = [
    azurerm_role_assignment.appconf_dataowner
  ]
}

resource "azurerm_app_configuration_key" "config_qflow_password" {
  configuration_store_id = azurerm_app_configuration.nbs_appts_app_config.id
  key                    = "Qflow:Password"
  type                   = "vault"
  label                  = "Qflow Password"
  vault_key_reference    = azurerm_key_vault_secret.kv_qflow_password.versionless_id

  depends_on = [
    azurerm_role_assignment.appconf_dataowner
  ]
}

resource "azurerm_app_configuration_key" "config_qflow_url" {
  configuration_store_id = azurerm_app_configuration.nbs_appts_app_config.id
  key                    = "Qflow:BaseUrl"
  label                  = "Qflow Url"
  value                  = "default"

  depends_on = [
    azurerm_role_assignment.appconf_dataowner
  ]
}