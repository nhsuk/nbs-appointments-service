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

variable "nbsapikey" {
  type      = string
  sensitive = true
}

variable "qflowusername" {
  type      = string
  sensitive = true
}

variable "qflowpassword" {
  type      = string
  sensitive = true
}

variable "alertsslackwebhookurl" {
  type      = string
  sensitive = true
}

module "api" {
  source                = "../../resources"
  location              = "uksouth"
  environment           = "exp"
  loc                   = "uks"
  sku_name              = "B1"
  docker_image_tag      = var.docker_image_tag
  docker_username       = var.docker_username
  docker_password       = var.docker_password
  nbsapikey             = var.nbsapikey
  qflowusername         = var.qflowusername
  qflowpassword         = var.qflowpassword
  alertsslackwebhookurl = var.alertsslackwebhookurl
}