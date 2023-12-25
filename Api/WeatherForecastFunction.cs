using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace API {
   public class WeatherForecastFunction {
      private readonly ILogger _logger;
      public WeatherForecastFunction(ILoggerFactory loggerFactory) {
         _logger = loggerFactory.CreateLogger<WeatherForecastFunction>();
      }

      [Function("WeatherForecast")]
      public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req) {
         var randomNumber = new Random();
         var temp = 0;

         var result = Enumerable.Range(1, 5).Select(index => new WeatherForecast {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = temp = randomNumber.Next(-20, 55),
            Summary = GetSummary(temp)
         }).ToArray();

         var response = req.CreateResponse(HttpStatusCode.OK);
         response.WriteAsJsonAsync(result);
         return response;
      }

      private string GetSummary(int temp) {
         var summary = "Mild";
         if (temp >= 32) {
            summary = "Hot";
         } else if (temp <= 16 && temp > 0) {
            summary = "Cold";
         } else if (temp <= 0) {
            summary = "Freezing";
         }
         return summary;
      }

      public class WeatherForecast {
         public DateOnly Date { get; set; }
         public int TemperatureC { get; set; }
         public string Summary { get; set; }
         public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
      }
   }
}
