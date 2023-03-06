variable "application" {
  type    = string
  default = "nbs-appts"
}

variable "application_short" {
  type    = string
  default = "nbsappts"
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

variable "tags" {
  description = "A map of the tags to use for the resources that are deployed"
  type        = map(string)
  default = {
    product           = "covid19-booking"
    "cost code"       = "PO724/34"
    "created by"      = "Terraform"
    "created date"    = "2021-02-15"
    "product owner"   = "James Spirit"
    "requested by"    = "NBS"
    "service-product" = "National Booking Service"
    team              = "NBS"
  }
}
