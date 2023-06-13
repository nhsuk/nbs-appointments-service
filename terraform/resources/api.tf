data "azurerm_subscription" "current" {}

resource "azurerm_resource_group" "nbs_appts_rg" {
  name     = "${var.application}-rg-${var.environment}-${var.loc}"
  location = var.location
  tags     = local.allTags
}

resource "azurerm_storage_account" "nbs_appts_stacc" {
  name                     = "${var.application_short}${var.environment}${var.loc}"
  resource_group_name      = azurerm_resource_group.nbs_appts_rg.name
  location                 = azurerm_resource_group.nbs_appts_rg.location
  account_tier             = "Standard"
  account_kind             = "BlobStorage"
  account_replication_type = "LRS"
}

resource "azurerm_storage_container" "nbs_appts_container" {
  name                  = "${var.application_short}${var.environment}${var.loc}"
  storage_account_name  = azurerm_storage_account.nbs_appts_stacc.name
  container_access_type = "blob"
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
    container_registry_use_managed_identity = false
    application_stack {
      docker_image     = "${var.docker_server_url}/${var.docker_image}"
      docker_image_tag = var.docker_image_tag
    }
  }

  app_settings = {    
    APPLICATIONINSIGHTS_CONNECTION_STRING = azurerm_application_insights.nbs_appts_app_insights.connection_string
    DOCKER_REGISTRY_SERVER_PASSWORD = var.docker_password
    DOCKER_REGISTRY_SERVER_URL = var.docker_server_url
    DOCKER_REGISTRY_SERVER_USERNAME = var.docker_username
    "DateTimeProvider__TimeZone" = "Europe/London"
    "DateTimeProvider__Type" = "system"
    "Qflow__AppBookingFlagId" = 5
    "Qflow__BaseUrl" = "<change me>"
    "Qflow__CallCentreBookingFlagId" = 1
    "Qflow__CallCentreEmailFlagId" = 3
    "Qflow__DefaultRescheduleReasonId" = 2
    "Qflow__UserId" = "<change me>"
    "SessionManager__ConnectionString" = azurerm_storage_account.nbs_appts_stacc.primary_blob_connection_string
    "SessionManager__ContainerName" = "${var.application_short}${var.environment}${var.loc}"
    "SessionManager__Type" = "AzureStorage"
    "ShowException" = true
  }

  identity {
    type = "SystemAssigned"
  }

  lifecycle {
    ignore_changes = [
      tags # Ignore changes to tags, avoiding loss of Azure link to App Insights 
    ]
  }
}

resource "azurerm_monitor_autoscale_setting" "nbs_appts_sp_autoscale" {
  count               = var.enable_autoscaling ? 1 : 0
  name                = " ${var.application_short}-scaling-${var.environment}-${var.loc}"
  resource_group_name = azurerm_resource_group.nbs_appts_rg.name
  location            = azurerm_resource_group.nbs_appts_rg.location
  target_resource_id  = azurerm_service_plan.nbs_appts_sp.id

  profile {
    name = "default"
    capacity {
      default = 1
      minimum = 1
      maximum = 10
    }

    rule {
      metric_trigger {
        metric_name        = "CpuPercentage"
        metric_resource_id = azurerm_service_plan.nbs_appts_sp.id
        time_grain         = "PT1M"
        statistic          = "Average"
        time_window        = "PT5M"
        time_aggregation   = "Average"
        operator           = "GreaterThan"
        threshold          = 70
      }

      scale_action {
        direction = "Increase"
        type      = "ChangeCount"
        value     = "1"
        cooldown  = "PT5M"
      }
    }

    rule {
      metric_trigger {
        metric_name        = "CpuPercentage"
        metric_resource_id = azurerm_service_plan.nbs_appts_sp.id
        time_grain         = "PT1M"
        statistic          = "Average"
        time_window        = "PT5M"
        time_aggregation   = "Average"
        operator           = "LessThan"
        threshold          = 30
      }

      scale_action {
        direction = "Decrease"
        type      = "ChangeCount"
        value     = "1"
        cooldown  = "PT5M"
      }
    }
  }
}


