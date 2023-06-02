terraform {
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "~> 3.58.0"
    }
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
  docker_image = "test"
  docker_image_tag = "test"
}