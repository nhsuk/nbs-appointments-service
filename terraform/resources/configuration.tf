resource "azurerm_app_configuration" "nbs_appts_app_config" {
  name                = "${var.application}-app-config-${var.environment}-${var.loc}"
  resource_group_name = azurerm_resource_group.nbs_appts_rg.name
  location            = azurerm_resource_group.nbs_appts_rg.location
  sku = "standard"
}

data "azurerm_client_config" "current" {}

resource "azurerm_role_assignment" "appconf_dataowner" {
  scope                = azurerm_app_configuration.nbs_appts_app_config.id
  role_definition_name = "App Configuration Data Owner"
  principal_id         = data.azurerm_client_config.current.object_id
}

resource "azurerm_role_assignment" "keyvault_dataowner" {
  scope = azurerm_key_vault.nbs_appts_key_vault.id
  role_definition_name = "Key Vault Secrets Officer"
  principal_id = data.azurerm_client_config.current.object_id
}

resource "azurerm_key_vault" "nbs_appts_key_vault" {
  name                       = "${var.application}kv${var.environment}${var.loc}"
  resource_group_name        = azurerm_resource_group.nbs_appts_rg.name
  location                   = azurerm_resource_group.nbs_appts_rg.location
  tenant_id                  = data.azurerm_client_config.current.tenant_id
  sku_name                   = "standard"
  soft_delete_retention_days = 7
  enable_rbac_authorization  = true
}

resource "azurerm_key_vault_secret" "kv_qflow_username" {
  name         = "qflowusername"
  value        = "default"
  key_vault_id = azurerm_key_vault.nbs_appts_key_vault.id

  depends_on = [
    azurerm_role_assignment.keyvault_dataowner
  ]

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

  depends_on = [
    azurerm_role_assignment.keyvault_dataowner
  ]

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

  depends_on = [
    azurerm_role_assignment.keyvault_dataowner
  ]

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

  depends_on = [
    azurerm_role_assignment.appconf_dataowner
  ]
}

resource "azurerm_app_configuration_key" "config_qflow_password" {
  configuration_store_id = azurerm_app_configuration.nbs_appts_app_config.id
  key                    = "Qflow:Password"
  type                   = "vault"
  vault_key_reference    = azurerm_key_vault_secret.kv_qflow_password.versionless_id

  depends_on = [
    azurerm_role_assignment.appconf_dataowner
  ]
}

resource "azurerm_app_configuration_key" "config_nbs_api_key" {
  configuration_store_id = azurerm_app_configuration.nbs_appts_app_config.id
  key                    = "ApiKey"
  type                   = "vault"
  vault_key_reference    = azurerm_key_vault_secret.kv_nbs_api_key.versionless_id

  depends_on = [
    azurerm_role_assignment.appconf_dataowner
  ]
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

  depends_on = [
    azurerm_role_assignment.appconf_dataowner
  ]
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

  depends_on = [
    azurerm_role_assignment.appconf_dataowner
  ]
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

  depends_on = [
    azurerm_role_assignment.appconf_dataowner
  ]
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

  depends_on = [
    azurerm_role_assignment.appconf_dataowner
  ]
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

  depends_on = [
    azurerm_role_assignment.appconf_dataowner
  ]
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

  depends_on = [
    azurerm_role_assignment.appconf_dataowner
  ]
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

  depends_on = [
    azurerm_role_assignment.appconf_dataowner
  ]
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

  depends_on = [
    azurerm_role_assignment.appconf_dataowner
  ]
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

  depends_on = [
    azurerm_role_assignment.appconf_dataowner
  ]
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

  depends_on = [
    azurerm_role_assignment.appconf_dataowner
  ]
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

  depends_on = [
    azurerm_role_assignment.appconf_dataowner
  ]
}