terraform {
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "~> 3.58.0"
    }
  }

  backend "azurerm" {
    resource_group_name  = "covid19-booking-rg-tfstate-stag-uks"
    storage_account_name = "covidbookstagtf"
    container_name       = "tfstate"
    key                  = "stag.tfstate"
  }

  required_version = ">= 1.1.0"
}

provider "azurerm" {
  features {}
}

variable "docker_image_tag" {
  type = string
}

module "api" {
  source             = "../../resources"
  location           = "uksouth"
  environment        = "stag"
  loc                = "uks"
  sku_name           = "S1"
  enable_autoscaling = true
  docker_image_tag   = var.docker_image_tag
  docker_username    = var.docker_username
  docker_password    = var.docker_password
}


