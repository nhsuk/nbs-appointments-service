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

namespace NBS.Appointments.Service.Monitoring
{
    public static partial class AzureAlertsHandler
    {
        [FunctionName("AzureAlertsHandler")]
        public static async Task<IActionResult> Run(
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

        private static Task SendSlackNotification(object notification)
        {
            var client = new HttpClient();
            var uri = "https://hooks.slack.com/services/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";
            return client.PostAsJsonAsync(uri, notification);
        }
    }
}
