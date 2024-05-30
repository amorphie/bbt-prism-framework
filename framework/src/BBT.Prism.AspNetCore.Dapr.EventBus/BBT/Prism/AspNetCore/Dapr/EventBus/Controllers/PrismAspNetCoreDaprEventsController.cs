using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using BBT.Prism.Dapr;
using BBT.Prism.EventBus.Dapr;
using Dapr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace BBT.Prism.AspNetCore.Dapr.EventBus.Controllers;

public sealed class PrismAspNetCoreDaprEventsController : Controller
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [HttpPost(PrismAspNetCoreDaprPubSubConsts.DaprEventCallbackUrl)]
    public async Task<IActionResult> EventAsync()
    {
        try
        {
            HttpContext.ValidateDaprAppApiToken();

            var daprSerializer = HttpContext.RequestServices.GetRequiredService<IDaprSerializer>();
            
            var body = (await JsonDocument.ParseAsync(HttpContext.Request.Body));

            var pubSubName = body.RootElement.GetProperty("pubsubname").GetString();
            var topic = body.RootElement.GetProperty("topic").GetString();
            var data = body.RootElement.GetProperty("data").GetRawText();
            if (pubSubName.IsNullOrWhiteSpace() || topic.IsNullOrWhiteSpace() || data.IsNullOrWhiteSpace())
            {
                // Logger<>.LogError("Invalid Dapr event request.");
                return BadRequest();
            }

            var distributedEventBus = HttpContext.RequestServices.GetRequiredService<DaprIntegrationEventBus>();

            if (IsDaprEventData(data))
            {
                var daprEventData = daprSerializer.Deserialize(data, typeof(PrismDaprEventData))
                    .As<PrismDaprEventData>();
                var eventData = daprSerializer.Deserialize(daprEventData.JsonData,
                    distributedEventBus.GetEventType(daprEventData.Topic));
                await distributedEventBus.TriggerHandlersAsync(distributedEventBus.GetEventType(daprEventData.Topic),
                    eventData, daprEventData.MessageId, daprEventData.CorrelationId);
            }
            else
            {
                var eventData = daprSerializer.Deserialize(data, distributedEventBus.GetEventType(topic!));
                await distributedEventBus.TriggerHandlersAsync(distributedEventBus.GetEventType(topic!), eventData);
            }
        }
        catch
        {
            // WARN: Stack issue on retries on error
            return Ok();
        }

        return Ok();
    }

    private bool IsDaprEventData(string data)
    {
        var document = JsonDocument.Parse(data);
        var objects = document.RootElement.EnumerateObject().ToList();
        return objects.Count == 5 &&
               objects.Any(x => x.Name.Equals("PubSubName", StringComparison.CurrentCultureIgnoreCase)) &&
               objects.Any(x => x.Name.Equals("Topic", StringComparison.CurrentCultureIgnoreCase)) &&
               objects.Any(x => x.Name.Equals("MessageId", StringComparison.CurrentCultureIgnoreCase)) &&
               objects.Any(x => x.Name.Equals("JsonData", StringComparison.CurrentCultureIgnoreCase)) &&
               objects.Any(x => x.Name.Equals("CorrelationId", StringComparison.CurrentCultureIgnoreCase));
    }
}