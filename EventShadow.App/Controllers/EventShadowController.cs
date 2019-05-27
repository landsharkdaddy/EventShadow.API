using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EventShadow.App.Models;
using System.Net.Http;
using Newtonsoft.Json;

namespace EventShadow.App.Controllers
{
    public class EventShadowController : Controller
    {
        
        public async Task<IActionResult> Index(int eventID = 1)
        {
            HttpClient client = new HttpClient();
            DeviceViewModel deviceModel = new DeviceViewModel();

            try
            {
                // URI to get the event information
                // localhost SQL Server
                var eventURIString = string.Format("https://localhost:44337/api/EventShadow/GetEventShadowEvents/{0}", eventID);
                // Production SQL Server
                //var eventuriString = String.Format("https://dal-staging.integer.com/Integer/SentimentAPI/api/Sentiments/GetSentimentByDateRange/{0}/{1}/{2}", startDate.ToString("MM-dd-yyyy"), endDate.ToString("MM-dd-yyyy"), deviceId);

                HttpResponseMessage eventResponce = await client.GetAsync(eventURIString);
                eventResponce.EnsureSuccessStatusCode();
                string eventResponseBody = await eventResponce.Content.ReadAsStringAsync();

                var events = JsonConvert.DeserializeObject<List<Event>>(eventResponseBody);
                if (events.Any())
                {

                    deviceModel.eventInfo = events[0];
                    // need to get all the DevicesFound for each EventShadowDevice at the event
                    //deviceModel.eventInfo.id = events[0].id;
                    //deviceModel.eventInfo.eventName = events[0].eventName;
                    //deviceModel.eventInfo.startDate = events[0].startDate;
                    //deviceModel.eventInfo.endDate = events[0].endDate;
                    //deviceModel.eventInfo.market = events[0].market;
                    //deviceModel.eventInfo.address1 = events[0].address1;
                    //deviceModel.eventInfo.address2 = events[0].address2;
                    //deviceModel.eventInfo.city = events[0].city;
                    //deviceModel.eventInfo.state = events[0].state;
                    //deviceModel.eventInfo.zip = events[0].zip;

                }

                // URI to get the list of devices for the event
                // Localhost sql server
                var uriString = string.Format("https://localhost:44337/api/EventShadow/GetDevicesByEvent/{0}", eventID);
                // Production sql server
                //var uriString = String.Format("https://dal-staging.integer.com/Integer/SentimentAPI/api/Sentiments/GetSentimentByDateRange/{0}/{1}/{2}", startDate.ToString("MM-dd-yyyy"), endDate.ToString("MM-dd-yyyy"), deviceId);                                                          
                HttpResponseMessage response = await client.GetAsync(uriString);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                deviceModel.devicesFound = JsonConvert.DeserializeObject<List<Device>>(responseBody);
                // Above three lines can be replaced with new helper method below
                // string responseBody = await client.GetStringAsync(uri);
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
            }

            // Need to call dispose on the HttpClient object
            // when done using it, so the app doesn't leak resources
            client.Dispose();
            return View(deviceModel);
        }
    }
}