resource "azurerm_log_analytics_workspace" "nbs_appts_analytics_workspace" {
  name                = "${var.application}-log-analytics-${var.environment}-${var.loc}"
  resource_group_name = azurerm_resource_group.nbs_appts_rg.name
  location            = azurerm_resource_group.nbs_appts_rg.location
  sku                 = "PerGB2018"
  retention_in_days   = 30
}

resource "azurerm_application_insights" "nbs_appts_app_insights" {
  name                = "${var.application}-ai-${var.environment}-${var.loc}"
  resource_group_name = azurerm_resource_group.nbs_appts_rg.name
  location            = azurerm_resource_group.nbs_appts_rg.location
  workspace_id        = azurerm_log_analytics_workspace.nbs_appts_analytics_workspace.id
  application_type    = "web"
  retention_in_days   = 30
}

resource "azurerm_service_plan" "nbs_appts_alert_handler_sp" {
  name                = "${var.application}-alerts-sp-${var.environment}-${var.loc}"
  resource_group_name = azurerm_resource_group.nbs_appts_rg.name
  location            = azurerm_resource_group.nbs_appts_rg.location
  os_type             = "Linux"
  sku_name            = "Y1"
}

resource "azurerm_linux_function_app" "nbs_appts_alert_handler_func_app" {
  name                = "${var.application}-alerts-func-${var.environment}-${var.loc}"
  resource_group_name = azurerm_resource_group.nbs_appts_rg.name
  location            = azurerm_resource_group.nbs_appts_rg.location

  storage_account_name       = azurerm_storage_account.nbs_appts_stacc.name
  storage_account_access_key = azurerm_storage_account.nbs_appts_stacc.primary_access_key
  service_plan_id            = azurerm_service_plan.nbs_appts_alert_handler_sp.id

  site_config {}

  app_settings = {
    AppConfig = azurerm_app_configuration.nbs_appts_app_config.primary_read_key[0].connection_string
  }

  identity {
    type = "SystemAssigned"
  }
}

resource "azurerm_role_assignment" "alerts_func_kv_secrets_user_role" {
  scope = azurerm_key_vault.nbs_appts_key_vault.id
  role_definition_name = "Key Vault Secrets User"
  principal_id = azurerm_linux_function_app.nbs_appts_alert_handler_func_app.identity.0.principal_id
}

resource "azurerm_portal_dashboard" "nbs_appts_dashboard" {
  name                = "${var.application}-dashboard-${var.environment}-${var.loc}"
  resource_group_name = azurerm_resource_group.nbs_appts_rg.name
  location            = azurerm_resource_group.nbs_appts_rg.location
  tags = {
    hidden-title = "Appointment Service Metrics for ${var.environment}"
  }
  dashboard_properties = templatefile("../../resources/dash.tpl",
    {
      sub_id     = data.azurerm_subscription.current.subscription_id
      application = var.application
      environment = var.environment
  })
}

resource "azurerm_monitor_action_group" "nbs_appts_app_alert_action_group" {
  name                = "${var.application}-alert-action-group-${var.environment}-${var.loc}"
  resource_group_name = azurerm_resource_group.nbs_appts_rg.name
  short_name          = "${var.environment}-${var.loc}"

  email_receiver {
    name          = "Kim Crowe"
    email_address = "kim.crowe4@nhs.net"
  }  

  azure_function_receiver {
    name                     = "${var.application}-slack-webhook"
    function_app_resource_id = azurerm_linux_function_app.nbs_appts_alert_handler_func_app.id
    function_name            = azurerm_linux_function_app.nbs_appts_alert_handler_func_app.name
    http_trigger_url         = "https://${azurerm_linux_function_app.nbs_appts_alert_handler_func_app.default_hostname}/api/${azurerm_linux_function_app.nbs_appts_alert_handler_func_app.name}"
    use_common_alert_schema  = true
  }
}

resource "azurerm_monitor_metric_alert" "nbs_appts_app_http_401_alert" {
  name = "${var.application}-http-401-alert-${var.environment}-${var.loc}"
  resource_group_name = azurerm_resource_group.nbs_appts_rg.name
  scopes = [azurerm_linux_web_app.nbs_appts_app.id]
  description = "Alert will be triggered when http 401 error count is greater than 0"
	
	severity = 2
	frequency = "PT5M"
	window_size = "PT15M"

  criteria {
    metric_namespace = "Microsoft.Web/sites"
    metric_name      = "Http401"
    aggregation      = "Total"
    operator         = "GreaterThan"
    threshold        = 0
  }

  action {
    action_group_id = azurerm_monitor_action_group.nbs_appts_app_alert_action_group.id
  }
}