resource "azurerm_monitor_autoscale_setting"  {
  #count               = var.enable_autoscaling ? 1 : 0   
  #name                = "${var.application_short}${var.environment}${var.loc}"
  name                = "myAutoscaleSetting"
  enabled             = true
  resource_group_name = azurerm_resource_group.nbs_appts_rg.name
  location            = azurerm_resource_group.nbs_appts_rg.location
  target_resource_id  = azurerm_service_plan.nbs_appts_sp.id

  profile {
    name = "MyCPUProfile"

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

/*
{
    "location": "UK South",
    "tags": {},
    "properties": {
        "name": "nbs-appts-sp-perf-uks-Autoscale-152",
        "enabled": false,
        "targetResourceUri": "/subscriptions/07748954-52d6-46ce-95e6-2701bfc715b4/resourceGroups/nbs-appts-rg-perf-uks/providers/Microsoft.Web/serverfarms/nbs-appts-sp-perf-uks",
        "profiles": [
            {
                "name": "Auto created default scale condition",
                "capacity": {
                    "minimum": "1",
                    "maximum": "10",
                    "default": "1"
                },
                "rules": [
                    {
                        "scaleAction": {
                            "direction": "Increase",
                            "type": "ChangeCount",
                            "value": "1",
                            "cooldown": "PT5M"
                        },
                        "metricTrigger": {
                            "metricName": "CpuPercentage",
                            "metricNamespace": "microsoft.web/serverfarms",
                            "metricResourceUri": "/subscriptions/07748954-52d6-46ce-95e6-2701bfc715b4/resourceGroups/nbs-appts-rg-perf-uks/providers/Microsoft.Web/serverFarms/nbs-appts-sp-perf-uks",
                            "operator": "GreaterThan",
                            "statistic": "Average",
                            "threshold": 70,
                            "timeAggregation": "Average",
                            "timeGrain": "PT1M",
                            "timeWindow": "PT10M",
                            "Dimensions": [],
                            "dividePerInstance": false
                        }
                    }
                ]
            }
        ],
        "notifications": [],
        "targetResourceLocation": "UK South"
    },
    "id": "/subscriptions/07748954-52d6-46ce-95e6-2701bfc715b4/resourceGroups/nbs-appts-rg-perf-uks/providers/microsoft.insights/autoscalesettings/nbs-appts-sp-perf-uks-Autoscale-152",
    "name": "nbs-appts-sp-perf-uks-Autoscale-152",
    "type": "Microsoft.Insights/autoscaleSettings"
}*/
