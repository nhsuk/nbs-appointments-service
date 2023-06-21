terraform {
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "~> 3.58.0"
    }
  }

  backend "azurerm" {
    resource_group_name  = "nbs-appts-rg-exp-uks"
    storage_account_name = "apptstfdevuks"
    container_name       = "tfstate"
    key                  = "exp.tfstate"
  }

  required_version = ">= 1.1.0"
}

provider "azurerm" {
  features {}
}

variable "docker_image_tag" {
  type = string
}

variable "docker_username" {
  type      = string
  sensitive = true
}

variable "docker_password" {
  type      = string
  sensitive = true
}

variable "NBS_API_KEY" {
  type      = string
  sensitive = true
}

variable "qflow_username" {
  type      = string
  sensitive = true
}

variable "qflow_password" {
  type      = string
  sensitive = true
}

variable "alerts_slack_webhook_url" {
  type      = string
  sensitive = true
}

variable "splunk_host" {
  type      = string
  sensitive = true
}

variable "splunk_event_collector_token" {
  type      = string
  sensitive = true
}

variable "QFLOW_BASE_URL" {
  type = string
}

variable "QFLOW_USER_ID" {
  type = string
}

module "api" {
  source                       = "../../resources"
  location                     = "uksouth"
  environment                  = "exp"
  loc                          = "uks"
  sku_name                     = "B1"
  docker_image_tag             = var.docker_image_tag
  docker_username              = var.docker_username
  docker_password              = var.docker_password
  nbs_api_key                  = var.NBS_API_KEY
  qflow_username               = var.qflow_username
  qflow_password               = var.qflow_password
  alerts_slack_webhook_url     = var.alerts_slack_webhook_url
  splunk_host                  = var.splunk_host
  splunk_event_collector_token = var.splunk_event_collector_token
  qflow_base_url               = var.QFLOW_BASE_URL
  qflow_user_id                = var.QFLOW_USER_ID
}