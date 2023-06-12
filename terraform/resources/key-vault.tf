data "azurerm_client_config" "current" {}

resource "azurerm_key_vault" "nbs_appts_key_vault" {
  name                       = "${var.application}kv${var.environment}${var.loc}"
  resource_group_name        = azurerm_resource_group.nbs_appts_rg.name
  location                   = azurerm_resource_group.nbs_appts_rg.location
  tenant_id                  = data.azurerm_client_config.current.tenant_id
  sku_name                   = "standard"
  soft_delete_retention_days = 7  

  access_policy {
    tenant_id = data.azurerm_client_config.current.tenant_id
    object_id = azurerm_linux_web_app.nbs_appts_app.identity.0.principal_id
    secret_permissions = [
      "Get"
    ]
  }

  access_policy {
    tenant_id = data.azurerm_client_config.current.tenant_id
    object_id = azurerm_linux_function_app.nbs_appts_alert_handler_func_app.identity.0.principal_id
    secret_permissions = [
      "Get"
    ]
  }

  access_policy {
    tenant_id = data.azurerm_client_config.current.tenant_id
    object_id = data.azurerm_client_config.current.object_id
    secret_permissions = [
      "Backup",
      "Delete",
      "Get",
      "List",
      "Purge",
      "Recover",
      "Restore",
      "Set",
    ]
  }
}

resource "azurerm_key_vault_secret" "kv_qflow_username" {
  name         = "Qflow--UserName"
  value        = "default"
  key_vault_id = azurerm_key_vault.nbs_appts_key_vault.id

  lifecycle {
    ignore_changes = [
      value
    ]
  }
}

resource "azurerm_key_vault_secret" "kv_qflow_password" {
  name         = "Qflow--Password"
  value        = "default"
  key_vault_id = azurerm_key_vault.nbs_appts_key_vault.id

  lifecycle {
    ignore_changes = [
      value
    ]
  }
}

resource "azurerm_key_vault_secret" "kv_nbs_api_key" {
  name         = "NbsApiKey"
  value        = "default"
  key_vault_id = azurerm_key_vault.nbs_appts_key_vault.id

  lifecycle {
    ignore_changes = [
      value
    ]
  }
}

resource "azurerm_key_vault_secret" "kv_alerts_slack_webhook_url" {
  name         = "Alerts--SlackWebhookUrl"
  value        = "default"
  key_vault_id = azurerm_key_vault.nbs_appts_key_vault.id

  lifecycle {
    ignore_changes = [
      value      
    ]
  }
}