terraform {
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "~> 3.58.0"
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

variable "docker_image_tag" {
  type = string
}

module "api" {
  source = "../../resources"
  location = "uksouth"
  environment = "dev"
  loc = "uks"
  sku_name = "B1"
  docker_image_tag = var.docker_image_tag
}