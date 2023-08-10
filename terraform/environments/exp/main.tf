terraform {
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "~> 3.61.0"
    }
  }

  backend "azurerm" {
    resource_group_name  = "nbs-appts-rg-exp-uks"
    storage_account_name = "apptstfexpuks"
    container_name       = "tfstate"
    key                  = "exp.tfstate"
  }

  required_version = ">= 1.1.0"
}

provider "azurerm" {
  features {
    app_configuration {
      recover_soft_deleted = false
    }
    key_vault {
      recover_soft_deleted_key_vaults = false
    }
  }
}

variable "docker_image_tag" {
  type = string
}

module "api" {
  source           = "../../resources"
  location         = "uksouth"
  environment      = "exp"
  loc              = "uks"
  sku_name         = "B1"
  docker_image_tag = var.docker_image_tag
}