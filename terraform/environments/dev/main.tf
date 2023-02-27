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
    key                  = "terraform.tfstate"
  }

  required_version = ">= 1.1.0"
}

provider "azurerm" {
  features {}
}

module "api" {
  source = "../../resources"
  location = "uksouth"
  environment = "dev"
  loc = "uks"
  sku_name = "B1"
}