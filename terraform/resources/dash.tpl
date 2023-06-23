{
    "lenses": {
        "0": {
            "order": 0,
            "parts": {
                "0": {
                    "position": {
                        "x": 0,
                        "y": 0,
                        "colSpan": 5,
                        "rowSpan": 1
                    },
                    "metadata": {
                        "inputs": [],
                        "type": "Extension/HubsExtension/PartType/MarkdownPart",
                        "settings": {
                            "content": {
                                "content": "# Rate",
                                "title": "",
                                "subtitle": "",
                                "markdownSource": 1,
                                "markdownUri": ""
                            }
                        }
                    }
                },
                "1": {
                    "position": {
                        "x": 5,
                        "y": 0,
                        "colSpan": 5,
                        "rowSpan": 1
                    },
                    "metadata": {
                        "inputs": [],
                        "type": "Extension/HubsExtension/PartType/MarkdownPart",
                        "settings": {
                            "content": {
                                "content": "# Errors",
                                "title": "",
                                "subtitle": "",
                                "markdownSource": 1,
                                "markdownUri": ""
                            }
                        }
                    }
                },
                "2": {
                    "position": {
                        "x": 10,
                        "y": 0,
                        "colSpan": 5,
                        "rowSpan": 1
                    },
                    "metadata": {
                        "inputs": [],
                        "type": "Extension/HubsExtension/PartType/MarkdownPart",
                        "settings": {
                            "content": {
                                "content": "# Duration",
                                "title": "",
                                "subtitle": "",
                                "markdownSource": 1,
                                "markdownUri": ""
                            }
                        }
                    }
                },
                "3": {
                    "position": {
                        "x": 0,
                        "y": 1,
                        "colSpan": 5,
                        "rowSpan": 4
                    },
                    "metadata": {
                        "inputs": [
                            {
                                "name": "sharedTimeRange",
                                "isOptional": true
                            },
                            {
                                "name": "options",
                                "value": {
                                    "chart": {
                                        "metrics": [
                                            {
                                                "resourceMetadata": {
                                                    "id": "/subscriptions/${sub_id}/resourceGroups/${application}-rg-${environment}-uks/providers/Microsoft.Web/sites/${application}-wa-${environment}-uks"
                                                },
                                                "name": "Requests",
                                                "aggregationType": 1,
                                                "namespace": "microsoft.web/sites",
                                                "metricVisualization": {
                                                    "displayName": "Requests"
                                                }
                                            }
                                        ],
                                        "title": "Sum Requests for ${application}-${environment}-uks",
                                        "titleKind": 1,
                                        "visualization": {
                                            "chartType": 2,
                                            "legendVisualization": {
                                                "isVisible": true,
                                                "position": 2,
                                                "hideSubtitle": false
                                            },
                                            "axisVisualization": {
                                                "x": {
                                                    "isVisible": true,
                                                    "axisType": 2
                                                },
                                                "y": {
                                                    "isVisible": true,
                                                    "axisType": 1
                                                }
                                            }
                                        },
                                        "timespan": {
                                            "relative": {
                                                "duration": 86400000
                                            },
                                            "showUTCTime": false,
                                            "grain": 1
                                        }
                                    }
                                },
                                "isOptional": true
                            }
                        ],
                        "type": "Extension/HubsExtension/PartType/MonitorChartPart",
                        "settings": {
                            "content": {
                                "options": {
                                    "chart": {
                                        "metrics": [
                                            {
                                                "resourceMetadata": {
                                                    "id": "/subscriptions/${sub_id}/resourceGroups/${application}-rg-${environment}-uks/providers/Microsoft.Web/sites/${application}-wa-${environment}-uks"
                                                },
                                                "name": "Requests",
                                                "aggregationType": 1,
                                                "namespace": "microsoft.web/sites",
                                                "metricVisualization": {
                                                    "displayName": "Requests"
                                                }
                                            }
                                        ],
                                        "title": "All requests (UKS)",
                                        "titleKind": 2,
                                        "visualization": {
                                            "chartType": 2,
                                            "legendVisualization": {
                                                "isVisible": true,
                                                "position": 2,
                                                "hideSubtitle": false
                                            },
                                            "axisVisualization": {
                                                "x": {
                                                    "isVisible": true,
                                                    "axisType": 2
                                                },
                                                "y": {
                                                    "isVisible": true,
                                                    "axisType": 1
                                                }
                                            },
                                            "disablePinning": true
                                        }
                                    }
                                }
                            }
                        }
                    }
                },
                "4": {
                    "position": {
                        "x": 5,
                        "y": 1,
                        "colSpan": 5,
                        "rowSpan": 4
                    },
                    "metadata": {
                        "inputs": [
                            {
                                "name": "sharedTimeRange",
                                "isOptional": true
                            },
                            {
                                "name": "options",
                                "value": {
                                    "chart": {
                                        "metrics": [
                                            {
                                                "resourceMetadata": {
                                                    "id": "/subscriptions/${sub_id}/resourceGroups/${application}-rg-${environment}-uks/providers/Microsoft.Web/sites/${application}-wa-${environment}-uks"
                                                },
                                                "name": "Http4xx",
                                                "aggregationType": 1,
                                                "namespace": "microsoft.web/sites",
                                                "metricVisualization": {
                                                    "displayName": "Http 4xx"
                                                }
                                            },
                                            {
                                                "resourceMetadata": {
                                                    "id": "/subscriptions/${sub_id}/resourceGroups/${application}-rg-${environment}-uks/providers/Microsoft.Web/sites/${application}-wa-${environment}-uks"
                                                },
                                                "name": "Http5xx",
                                                "aggregationType": 1,
                                                "namespace": "microsoft.web/sites",
                                                "metricVisualization": {
                                                    "displayName": "Http Server Errors"
                                                }
                                            }
                                        ],
                                        "title": "Http 4xx and Http 5xx errors (uks)",
                                        "titleKind": 2,
                                        "visualization": {
                                            "chartType": 2,
                                            "legendVisualization": {
                                                "isVisible": true,
                                                "position": 2,
                                                "hideSubtitle": false
                                            },
                                            "axisVisualization": {
                                                "x": {
                                                    "isVisible": true,
                                                    "axisType": 2
                                                },
                                                "y": {
                                                    "isVisible": true,
                                                    "axisType": 1
                                                }
                                            }
                                        },
                                        "timespan": {
                                            "relative": {
                                                "duration": 86400000
                                            },
                                            "showUTCTime": false,
                                            "grain": 1
                                        }
                                    }
                                },
                                "isOptional": true
                            }
                        ],
                        "type": "Extension/HubsExtension/PartType/MonitorChartPart",
                        "settings": {
                            "content": {
                                "options": {
                                    "chart": {
                                        "metrics": [
                                            {
                                                "resourceMetadata": {
                                                    "id": "/subscriptions/${sub_id}/resourceGroups/${application}-rg-${environment}-uks/providers/Microsoft.Web/sites/${application}-wa-${environment}-uks"
                                                },
                                                "name": "Http4xx",
                                                "aggregationType": 1,
                                                "namespace": "microsoft.web/sites",
                                                "metricVisualization": {
                                                    "displayName": "Http 4xx"
                                                }
                                            },
                                            {
                                                "resourceMetadata": {
                                                    "id": "/subscriptions/${sub_id}/resourceGroups/${application}-rg-${environment}-uks/providers/Microsoft.Web/sites/${application}-wa-${environment}-uks"
                                                },
                                                "name": "Http5xx",
                                                "aggregationType": 1,
                                                "namespace": "microsoft.web/sites",
                                                "metricVisualization": {
                                                    "displayName": "Http Server Errors"
                                                }
                                            }
                                        ],
                                        "title": "Http 4xx and Http 5xx errors (UKS)",
                                        "titleKind": 2,
                                        "visualization": {
                                            "chartType": 2,
                                            "legendVisualization": {
                                                "isVisible": true,
                                                "position": 2,
                                                "hideSubtitle": false
                                            },
                                            "axisVisualization": {
                                                "x": {
                                                    "isVisible": true,
                                                    "axisType": 2
                                                },
                                                "y": {
                                                    "isVisible": true,
                                                    "axisType": 1
                                                }
                                            },
                                            "disablePinning": true
                                        }
                                    }
                                }
                            }
                        }
                    }
                },
                "5": {
                    "position": {
                        "x": 10,
                        "y": 1,
                        "colSpan": 5,
                        "rowSpan": 4
                    },
                    "metadata": {
                        "inputs": [
                            {
                                "name": "sharedTimeRange",
                                "isOptional": true
                            },
                            {
                                "name": "options",
                                "value": {
                                    "chart": {
                                        "metrics": [
                                            {
                                                "resourceMetadata": {
                                                    "id": "/subscriptions/${sub_id}/resourceGroups/${application}-rg-${environment}-uks/providers/Microsoft.Web/sites/${application}-wa-${environment}-uks"
                                                },
                                                "name": "HttpResponseTime",
                                                "aggregationType": 4,
                                                "namespace": "microsoft.web/sites",
                                                "metricVisualization": {
                                                    "displayName": "Response Time"
                                                }
                                            }
                                        ],
                                        "title": "Http 4xx and Http 5xx errors (uks)",
                                        "titleKind": 2,
                                        "visualization": {
                                            "chartType": 2,
                                            "legendVisualization": {
                                                "isVisible": true,
                                                "position": 2,
                                                "hideSubtitle": false
                                            },
                                            "axisVisualization": {
                                                "x": {
                                                    "isVisible": true,
                                                    "axisType": 2
                                                },
                                                "y": {
                                                    "isVisible": true,
                                                    "axisType": 1
                                                }
                                            }
                                        },
                                        "timespan": {
                                            "relative": {
                                                "duration": 86400000
                                            },
                                            "showUTCTime": false,
                                            "grain": 1
                                        }
                                    }
                                },
                                "isOptional": true
                            }
                        ],
                        "type": "Extension/HubsExtension/PartType/MonitorChartPart",
                        "settings": {
                            "content": {
                                "options": {
                                    "chart": {
                                        "metrics": [
                                            {
                                                "resourceMetadata": {
                                                    "id": "/subscriptions/${sub_id}/resourceGroups/${application}-rg-${environment}-uks/providers/Microsoft.Web/sites/${application}-wa-${environment}-uks"
                                                },
                                                "name": "HttpResponseTime",
                                                "aggregationType": 4,
                                                "namespace": "microsoft.web/sites",
                                                "metricVisualization": {
                                                    "displayName": "Response Time"
                                                }
                                            }
                                        ],
                                        "title": "Average response time (UKS)",
                                        "titleKind": 2,
                                        "visualization": {
                                            "chartType": 2,
                                            "legendVisualization": {
                                                "isVisible": true,
                                                "position": 2,
                                                "hideSubtitle": false
                                            },
                                            "axisVisualization": {
                                                "x": {
                                                    "isVisible": true,
                                                    "axisType": 2
                                                },
                                                "y": {
                                                    "isVisible": true,
                                                    "axisType": 1
                                                }
                                            },
                                            "disablePinning": true
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    },
    "metadata": {
        "model": {
            "timeRange": {
                "value": {
                    "relative": {
                        "duration": 24,
                        "timeUnit": 1
                    }
                },
                "type": "MsPortalFx.Composition.Configuration.ValueTypes.TimeRange"
            },
            "filterLocale": {
                "value": "en-us"
            },
            "filters": {
                "value": {
                    "MsPortalFx_TimeRange": {
                        "model": {
                            "format": "utc",
                            "granularity": "auto",
                            "relative": "24h"
                        },
                        "displayCache": {
                            "name": "UTC Time",
                            "value": "Past 24 hours"
                        },
                        "filteredPartIds": [
                            "StartboardPart-MonitorChartPart-5cbc3d72-600d-4392-8352-6360d2b010df",
                            "StartboardPart-MonitorChartPart-5cbc3d72-600d-4392-8352-6360d2b01117",
                            "StartboardPart-MonitorChartPart-5cbc3d72-600d-4392-8352-6360d2b01130"
                        ]
                    }
                }
            }
        }
    }
}