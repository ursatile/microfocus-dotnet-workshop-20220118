using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.IO;
using System.Threading.Tasks;
using Autobarn.Messages;
using EasyNetQ;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Autobarn.Notifier {
    class Program {

        private static readonly IConfigurationRoot config = ReadConfiguration();
        private static IBus bus;
        private static HubConnection hub;


        static async Task Main(string[] args) {
            JsonConvert.DefaultSettings = JsonSettings;
            hub = new HubConnectionBuilder()
                .WithUrl(config["AutobarnSignalRServerUrl"])
                .Build();
            await hub.StartAsync();
            Console.WriteLine("Connected to SignalR!");
            bus = RabbitHutch.CreateBus(config.GetConnectionString("AutobarnRabbitMQConnectionString"));
            const string SUBSCRIBER_ID = "Autobarn.Notifier";
            bus.PubSub.Subscribe<NewVehiclePriceMessage>(SUBSCRIBER_ID, HandleNewVehiclePriceMessage);
            Console.WriteLine("Listening for NewVehiclePriceMessages...");
            Console.ReadLine();
        }

        static async Task HandleNewVehiclePriceMessage(NewVehiclePriceMessage m) {
            var json = JsonConvert.SerializeObject(m);
            Console.WriteLine($"Sending JSON to SignalR: {json}");
            await hub.SendAsync("NotifyWebUsers", "Autobarn.Notifier", json);
            Console.WriteLine("Sent!");
        }

        private static JsonSerializerSettings JsonSettings() =>
            new JsonSerializerSettings {
                ContractResolver = new DefaultContractResolver {
                    NamingStrategy = new CamelCaseNamingStrategy()
                }
            };
        private static IConfigurationRoot ReadConfiguration() {
            var basePath = Directory.GetParent(AppContext.BaseDirectory).FullName;
            return new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json")
                .AddJsonFile("appsettings.Development.json")
                .AddEnvironmentVariables()
                .Build();
        }
    }

}
