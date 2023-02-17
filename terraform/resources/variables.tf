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
  default = "dev"
}

variable "location" {
  type = string
  default = "uk south"
}

variable "loc" {
  type = string
  default = "uks"
}

variable "tags" {
  description = "A map of the tags to use for the resources that are deployed"
  type        = map(string)
  default     = { 
    product = "covid19-booking"
    "cost code" = "PO724/34"
    "created by" = "Terraform"
    "created date" = "2021-02-15"
    "product owner" = "James Spirit"
    "requested by" = "NBS"
    "service-product" = "National Booking Service"
    team = "NBS"
  }
}