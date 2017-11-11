using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace JobOrganizer
{
    class Program
    {
        static Event myEvent;

        // If modifying these scopes, delete your previously saved credentials
        // at ~/.credentials/calendar-dotnet-quickstart.json
        static string[] Scopes = { CalendarService.Scope.Calendar };
        static string ApplicationName = "Google Calendar API .NET Quickstart";

        static void Main(string[] args)
        {
            UserCredential credential;

            using (var stream =
                new FileStream("client_secret.json", FileMode.Open, FileAccess.Read))
            {
                string credPath = System.Environment.GetFolderPath(
                    System.Environment.SpecialFolder.Personal);
                credPath = Path.Combine(credPath, ".credentials/calendar-dotnet-quickstart.json");

                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
                Console.WriteLine("Credential file saved to: " + credPath);
            }

            // Create Google Calendar API service.
            var service = new CalendarService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            // Define parameters of request.
            EventsResource.ListRequest request = service.Events.List("primary");
            request.TimeMin = DateTime.Now;
            request.ShowDeleted = false;
            request.SingleEvents = true;
            request.MaxResults = 10;
            request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;

            // List events.
            Events events = request.Execute();
            Console.WriteLine("Upcoming events:");
            if (events.Items != null && events.Items.Count > 0)
            {
                foreach (var eventItem in events.Items)
                {
                    string when = eventItem.Start.DateTime.ToString();
                    if (String.IsNullOrEmpty(when))
                    {
                        when = eventItem.Start.Date;
                    }
                    Console.WriteLine("{0} ({1})", eventItem.Summary, when);
                }
            }
            else
            {
                Console.WriteLine("No upcoming events found.");
            }

            if (Console.ReadKey(true).Key == ConsoleKey.A)
            {
                CreateEvent();
                EventsResource.InsertRequest myRequest = service.Events.Insert(myEvent, "primary");
                myEvent = myRequest.Execute();
                Console.WriteLine("Pressed A and added new event: {0}", myEvent.HtmlLink);
            }

            if (Console.ReadKey(true).Key == ConsoleKey.Enter)
            {
                service.Events.Delete("primary", myEvent.Id).Execute();
                Console.WriteLine("Deleted event");
            }

            Console.Read();
        }

        private static void CreateEvent()
        {
            Event newEvent = new Event()
            {
                Summary = "Test Event",
                Location = "My Place",
                Description = "Testing adding an event",
                Start = new EventDateTime()
                {
                    //YY-MM-DD
                    DateTime = DateTime.Parse("2017-11-08T09:00:00"),
                    TimeZone = "Europe/Copenhagen",
                },
                End = new EventDateTime()
                {
                    DateTime = DateTime.Parse("2017-11-08T17:00:00"),
                    TimeZone = "Europe/Copenhagen"
                }
            };


            myEvent = newEvent;
        }
    }
}
