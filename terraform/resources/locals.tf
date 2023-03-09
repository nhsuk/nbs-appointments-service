locals {
  allTags = merge(var.tags, { environment = var.environment })
}
