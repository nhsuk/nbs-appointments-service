data "azurerm_container_registry" "container_registry" {
  resource_group_name = var.registry_group
  name                = var.registry_name
}

data "azurerm_subscription" "current" {}

resource "azurerm_resource_group" "nbs_appts_rg" {
  name     = "${var.application}-rg-${var.environment}-${var.loc}"
  location = var.location
  tags     = local.allTags
}

resource "azurerm_service_plan" "nbs_appts_sp" {
  name                = "${var.application}-sp-${var.environment}-${var.loc}"
  resource_group_name = azurerm_resource_group.nbs_appts_rg.name
  location            = azurerm_resource_group.nbs_appts_rg.location
  os_type             = "Linux"
  sku_name            = var.sku_name
  tags                = local.allTags
}

resource "azurerm_linux_web_app" "nbs_appts_app" {
  name                = "${var.application}-app-${var.environment}-${var.loc}"
  resource_group_name = azurerm_resource_group.nbs_appts_rg.name
  location            = azurerm_resource_group.nbs_appts_rg.location
  service_plan_id     = azurerm_service_plan.nbs_appts_sp.id
  https_only          = true

  site_config {
    minimum_tls_version                     = "1.2"
    container_registry_use_managed_identity = true
    application_stack {
      docker_image     = "${data.azurerm_container_registry.container_registry.login_server}/${var.docker_image}"
      docker_image_tag = var.docker_image_tag
    }
  }

  app_settings = {
    AppConfig = azurerm_app_configuration.nbs_appts_app_config.primary_read_key[0].connection_string
  }

  identity {
    type = "SystemAssigned"
  }

  depends_on = [
    azurerm_app_configuration.nbs_appts_app_config
  ]

  lifecycle {
    ignore_changes = [
      app_settings,
      tags # Ignore changes to tags, avoiding loss of Azure link to App Insights 
    ]
  }
}

resource "azurerm_role_assignment" "acrpull_role" {
  scope                = data.azurerm_container_registry.container_registry.id
  role_definition_name = "AcrPull"
  principal_id         = azurerm_linux_web_app.nbs_appts_app.identity.0.principal_id
}

resource "azurerm_role_assignment" "keyvault_secrets_user" {
  scope = azurerm_key_vault.nbs_appts_key_vault.id
  role_definition_name = "Key Vault Secrets User"
  principal_id = azurerm_linux_web_app.nbs_appts_app.identity.0.principal_id
}

resource "azurerm_portal_dashboard" "nbs_appts_dashboard" {
  name                = "${var.application}-dashboard-${var.environment}-${var.loc}"
  resource_group_name = azurerm_resource_group.nbs_appts_rg.name
  location            = azurerm_resource_group.nbs_appts_rg.location
  dashboard_properties = templatefile("dash.tpl",
    {
      sub_id     = data.azurerm_subscription.current.subscription_id
  })
}