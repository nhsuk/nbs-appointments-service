variable "application" {
  type    = string
  default = "nbs-appts"
}

variable "application_short" {
  type    = string
  default = "nbsappts"
}

variable "alert_handler_application" {
  type    = string
  default = "nbs-alerts"
}

variable "alert_handler_application_short" {
  type    = string
  default = "nbsalerts"
}

variable "environment" {
  type = string
}

variable "location" {
  type = string
}

variable "loc" {
  type = string
}

variable "sku_name" {
  type = string
}

variable "registry_group" {
  type    = string
  default = "covid19-booking-rg-dev-uks"
}

variable "registry_name" {
  type    = string
  default = "nbsimages"
}

variable "docker_image" {
  type    = string
  default = "appointments-service"
}

variable "docker_image_tag" {
  type = string
}

variable "docker_server_url" {
  type    = string
  default = "nbsimages.azurecr.io"
}

variable "docker_username" {
  type = string
}

variable "docker_password" {
  type = string
}

variable "enable_autoscaling" {
  type    = bool
  default = false
}

variable "container_registry_login_server" {
  type    = string
  default = "nbsimages.azurecr.io"
}

variable "container_registry_id" {
  type    = string
  default = "/subscriptions/07748954-52d6-46ce-95e6-2701bfc715b4/resourceGroups/covid19-booking-rg-dev-uks/providers/Microsoft.ContainerRegistry/registries/nbsimages"
}

variable "tags" {
  description = "A map of the tags to use for the resources that are deployed"
  type        = map(string)
  default = {
    "product"         = "covid19-booking"
    "cost code"       = "PO724/34"
    "created by"      = "Terraform"
    "created date"    = "2021-02-15"
    "product owner"   = "James Spirit"
    "requested by"    = "NBS"
    "service-product" = "National Booking Service"
    "team"            = "NBS"
  }
}
