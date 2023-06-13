resource "azurerm_app_configuration" "nbs_appts_app_config" {
  name                = "${var.application}-app-config-${var.environment}-${var.loc}"
  resource_group_name = azurerm_resource_group.nbs_appts_rg.name
  location            = azurerm_resource_group.nbs_appts_rg.location
  sku                 = "standard"
}

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
      "Get",
      "List"
    ]
  }

  access_policy {
    tenant_id = data.azurerm_client_config.current.tenant_id
    object_id = azurerm_linux_function_app.nbs_appts_alert_handler_func_app.identity.0.principal_id
    secret_permissions = [
      "Get",
      "List"
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
  name         = "qflowusername"
  value        = "default"
  key_vault_id = azurerm_key_vault.nbs_appts_key_vault.id

  lifecycle {
    ignore_changes = [
      value
    ]
  }
}

resource "azurerm_key_vault_secret" "kv_qflow_password" {
  name         = "qflowpassword"
  value        = "default"
  key_vault_id = azurerm_key_vault.nbs_appts_key_vault.id

  lifecycle {
    ignore_changes = [
      value
    ]
  }
}

resource "azurerm_key_vault_secret" "kv_nbs_api_key" {
  name         = "nbsapikey"
  value        = "default"
  key_vault_id = azurerm_key_vault.nbs_appts_key_vault.id

  lifecycle {
    ignore_changes = [
      value
    ]
  }
}

resource "azurerm_key_vault_secret" "kv_alerts_slack_webhook_url" {
  name         = "alertsslackwebhookurl"
  value        = "default"
  key_vault_id = azurerm_key_vault.nbs_appts_key_vault.id

  lifecycle {
    ignore_changes = [
      value
    ]
  }
}

resource "azurerm_app_configuration_key" "config_qflow_username" {
  configuration_store_id = azurerm_app_configuration.nbs_appts_app_config.id
  key                    = "Qflow:UserName"
  type                   = "vault"
  vault_key_reference    = azurerm_key_vault_secret.kv_qflow_username.versionless_id
}

resource "azurerm_app_configuration_key" "config_qflow_password" {
  configuration_store_id = azurerm_app_configuration.nbs_appts_app_config.id
  key                    = "Qflow:Password"
  type                   = "vault"
  vault_key_reference    = azurerm_key_vault_secret.kv_qflow_password.versionless_id
}

resource "azurerm_app_configuration_key" "config_nbs_api_key" {
  configuration_store_id = azurerm_app_configuration.nbs_appts_app_config.id
  key                    = "ApiKey"
  type                   = "vault"
  vault_key_reference    = azurerm_key_vault_secret.kv_nbs_api_key.versionless_id
}

resource "azurerm_app_configuration_key" "config_alerts_slack_webhook_url" {
  configuration_store_id = azurerm_app_configuration.nbs_appts_app_config.id
  key                    = "Alerts:SlackWebhookUrl"
  type                   = "vault"
  vault_key_reference    = azurerm_key_vault_secret.kv_alerts_slack_webhook_url.versionless_id
}

resource "azurerm_app_configuration_key" "config_qflow_url" {
  configuration_store_id = azurerm_app_configuration.nbs_appts_app_config.id
  key                    = "Qflow:BaseUrl"
  value                  = "default"

  lifecycle {
    ignore_changes = [
      value
    ]
  }
}

resource "azurerm_app_configuration_key" "config_datetimeprovider_type" {
  configuration_store_id = azurerm_app_configuration.nbs_appts_app_config.id
  key                    = "DateTimeProvider:Type"
  value                  = "system"

  lifecycle {
    ignore_changes = [
      value
    ]
  }
}

resource "azurerm_app_configuration_key" "config_datetimeprovider_timezone" {
  configuration_store_id = azurerm_app_configuration.nbs_appts_app_config.id
  key                    = "DateTimeProvider:TimeZone"
  value                  = "Europe/London"

  lifecycle {
    ignore_changes = [
      value
    ]
  }
}

resource "azurerm_app_configuration_key" "config_qflow_appbookingflagid" {
  configuration_store_id = azurerm_app_configuration.nbs_appts_app_config.id
  key                    = "Qflow:AppBookingFlagId"
  value                  = "5"

  lifecycle {
    ignore_changes = [
      value
    ]
  }
}

resource "azurerm_app_configuration_key" "config_qflow_callcentrebookingflagid" {
  configuration_store_id = azurerm_app_configuration.nbs_appts_app_config.id
  key                    = "Qflow:CallCentreBookingFlagId"
  value                  = "1"

  lifecycle {
    ignore_changes = [
      value
    ]
  }
}

resource "azurerm_app_configuration_key" "config_qflow_callcentreemailflagid" {
  configuration_store_id = azurerm_app_configuration.nbs_appts_app_config.id
  key                    = "Qflow:CallCentreEmailFlagId"
  value                  = "3"

  lifecycle {
    ignore_changes = [
      value
    ]
  }
}

resource "azurerm_app_configuration_key" "config_qflow_defaultreschedulereasonid" {
  configuration_store_id = azurerm_app_configuration.nbs_appts_app_config.id
  key                    = "Qflow:DefaultRescheduleReasonId"
  value                  = "2"

  lifecycle {
    ignore_changes = [
      value
    ]
  }
}

resource "azurerm_app_configuration_key" "config_qflow_userid" {
  configuration_store_id = azurerm_app_configuration.nbs_appts_app_config.id
  key                    = "Qflow:UserId"
  value                  = "0000"

  lifecycle {
    ignore_changes = [
      value
    ]
  }
}

resource "azurerm_app_configuration_key" "config_splunk_host" {
  configuration_store_id = azurerm_app_configuration.nbs_appts_app_config.id
  key                    = "Splunk:Host"
  value                  = "default"

  lifecycle {
    ignore_changes = [
      value
    ]
  }
}

resource "azurerm_app_configuration_key" "config_splunk_eventcollectortoken" {
  configuration_store_id = azurerm_app_configuration.nbs_appts_app_config.id
  key                    = "Splunk:EventCollectorToken"
  value                  = "00000000-0000-0000-0000-000000000000"

  lifecycle {
    ignore_changes = [
      value
    ]
  }
}

resource "azurerm_app_configuration_key" "config_sessionmanager_type" {
  configuration_store_id = azurerm_app_configuration.nbs_appts_app_config.id
  key                    = "SessionManager:Type"
  value                  = "AzureStorage"

  lifecycle {
    ignore_changes = [
      value
    ]
  }
}

resource "azurerm_app_configuration_key" "config_sessionmanager_blobendpoint" {
  configuration_store_id = azurerm_app_configuration.nbs_appts_app_config.id
  key                    = "SessionManager:BlobEndpoint"
  value                  = azurerm_storage_account.nbs_appts_stacc.primary_blob_endpoint

  lifecycle {
    ignore_changes = [
      value
    ]
  }
}

resource "azurerm_app_configuration_key" "config_sessionmanager_containername" {
  configuration_store_id = azurerm_app_configuration.nbs_appts_app_config.id
  key                    = "SessionManager:ContainerName"
  value                  = azurerm_storage_container.nbs_appts_container.name

  lifecycle {
    ignore_changes = [
      value
    ]
  }
}