data "azurerm_function_app_host_keys" "nbs_alerthandler_func_app_host_keys" {
  name                = azurerm_linux_function_app.nbs_alerthandler_func.name
  resource_group_name = azurerm_resource_group.nbs_alerthandler_rg.name

  depends_on = [
    azurerm_linux_function_app.nbs_alerthandler_func
  ]
}

resource "azurerm_log_analytics_workspace" "nbs_appts_log" {
  name                = "${var.application}-log-${var.environment}-${var.loc}"
  resource_group_name = azurerm_resource_group.nbs_appts_rg.name
  location            = azurerm_resource_group.nbs_appts_rg.location
  sku                 = "PerGB2018"
  retention_in_days   = 30
}

resource "azurerm_application_insights" "nbs_appts_ai" {
  name                = "${var.application}-ai-${var.environment}-${var.loc}"
  resource_group_name = azurerm_resource_group.nbs_appts_rg.name
  location            = azurerm_resource_group.nbs_appts_rg.location
  workspace_id        = azurerm_log_analytics_workspace.nbs_appts_log.id
  application_type    = "web"
  retention_in_days   = 30
}

resource "azurerm_portal_dashboard" "nbs_appts_dash" {
  name                = "${var.application}-dash-${var.environment}-${var.loc}"
  resource_group_name = azurerm_resource_group.nbs_appts_rg.name
  location            = azurerm_resource_group.nbs_appts_rg.location
  tags = {
    hidden-title = "Appointment Service Metrics for ${var.environment}"
  }
  dashboard_properties = templatefile("../../resources/dash.tpl",
    {
      sub_id      = data.azurerm_subscription.current.subscription_id
      application = var.application
      environment = var.environment
  })
}

resource "azurerm_monitor_action_group" "nbs_appts_ag" {
  name                = "${var.application}-ag-${var.environment}-${var.loc}"
  resource_group_name = azurerm_resource_group.nbs_appts_rg.name
  short_name          = "${var.environment}-${var.loc}"

  azure_function_receiver {
    name                     = "${var.application}-slack-webhook"
    function_app_resource_id = azurerm_linux_function_app.nbs_alerthandler_func.id
    function_name            = azurerm_linux_function_app.nbs_alerthandler_func.name
    http_trigger_url         = "https://${azurerm_linux_function_app.nbs_alerthandler_func.default_hostname}/api/AzureAlertsHandler?code=${data.azurerm_function_app_host_keys.nbs_alerthandler_func_app_host_keys.default_function_key}"
    use_common_alert_schema  = true
  }
}

resource "azurerm_monitor_metric_alert" "nbs_appts_http_401_alert" {
  name                = "${var.application}-http-401-alert-${var.environment}-${var.loc}"
  resource_group_name = azurerm_resource_group.nbs_appts_rg.name
  scopes              = [azurerm_linux_web_app.nbs_appts_wa.id]
  description         = "Alert will be triggered when http 401 error count is greater than 0"

  severity    = 2
  frequency   = "PT5M"
  window_size = "PT15M"

  criteria {
    metric_namespace = "Microsoft.Web/sites"
    metric_name      = "Http401"
    aggregation      = "Total"
    operator         = "GreaterThan"
    threshold        = 0
  }

  action {
    action_group_id = azurerm_monitor_action_group.nbs_appts_ag.id
  }
}
