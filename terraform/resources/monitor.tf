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
    name          = "Team Jabby Member"
    email_address = "kim.crowe4@nhs.net; vincent.crowe1@nhs.net; paul.lewis43@nhs.net"
  }
}

resource "azurerm_monitor_metric_alert" "nbs_appts_app_http_401_alert" {
  name = "${var.application}-http-401-alert-${var.environment}-${var.loc}"
  resource_group_name = azurerm_resource_group.nbs_appts_rg.name
  scopes = [azurerm_linux_web_app.nbs_appts_app.id]
  description = "Alert will be triggered when http 401 error count is greater than 0"

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