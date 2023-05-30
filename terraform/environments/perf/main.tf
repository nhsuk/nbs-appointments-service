terraform {
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "~> 3.0.2"
    }
  }

  backend "azurerm" {
    resource_group_name  = "covid19-booking-rg-tfstate-dev-uks"
    storage_account_name = "nbsapptsdevtf"
    container_name       = "tfstate"
    key                  = "perf.tfstate"
  }

  required_version = ">= 1.1.0"
}

provider "azurerm" {
  features {}
}

variable "docker_image_tag" {
  type = string
}

enable_autoscaling = true

module "api" {
  source           = "../../resources"
  location         = "uksouth"
  environment      = "perf"
  loc              = "uks"
  sku_name         = "S1"
  docker_image_tag = var.docker_image_tag
}


