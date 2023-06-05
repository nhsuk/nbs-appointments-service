using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace NBS.Appointments.Service.Monitoring;

public class AzureAlertsHandler
{
    private readonly Options _options;
    public AzureAlertsHandler(IOptions<Options> options)
    {
        _options = options.Value;
    }

    [FunctionName("AzureAlertsHandler")]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]
        HttpRequest alertRequest,
        ILogger log)
    {
        var requestBody = await new StreamReader(alertRequest.Body).ReadToEndAsync();

        var alertData = JsonSerializer.Deserialize<AlertData>(requestBody);

        if (alertData.Data.Essentials.MonitorCondition == "Fired")
        {
            var alertName = alertData.Data.Essentials.AlertRule;
            var alertTarget = alertData.Data.Essentials.AlertTargetIDs[0];
            var alertTargetResourceName = alertTarget.Split("/").Last();
            var metric = alertData.Data.AlertContext.Condition.ConditionDetails[0].MetricName;
            var alertOperator = alertData.Data.AlertContext.Condition.ConditionDetails[0].Operator;
            var threshold = alertData.Data.AlertContext.Condition.ConditionDetails[0].Threshold;
            var notification = new
            {
                blocks = new[]
                {
                    new
                    {
                        type = "section",
                        text = new
                        {
                            type = "mrkdwn",
                            text = ":alert: *Alert fired* :alert:"
                        }
                    },
                    new
                    {
                        type = "section",
                        text = new
                        {
                            type = "mrkdwn",
                            text = $"*{alertName} has fired*! \n\n_{metric} {alertOperator} {threshold}_ "
                        }
                    },
                    new
                    {
                        type = "section",
                        text = new
                        {
                            type = "mrkdwn",
                            text =
                                $"View all alerts for {alertTargetResourceName} <https://portal.azure.com/#@nhschoices.net/resource{alertTarget}/alerts|here>"
                        }
                    }
                }
            };

            try
            {
                await SendSlackNotification(notification);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex);
            }
        }

        return new OkResult();
    }

    private Task SendSlackNotification(object notification)
    {
        var client = new HttpClient();
        var uri = _options.SlackWebhookUrl;
        return client.PostAsJsonAsync(uri, notification);
    }

    public class Options
    {
        public string SlackWebhookUrl { get; set; }
    }
}
