using System;
using EasyNetQ;
using Autobarn.Messages;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Autobarn.AuditLog {
    class Program {
        private static readonly IConfigurationRoot config = ReadConfiguration();

        static void Main(string[] args) {
            using var bus = RabbitHutch.CreateBus(config.GetConnectionString("AutobarnRabbitMQConnectionString"));
            const string SUBSCRIBER_ID = "Autobarn.AuditLog";
            bus.PubSub.Subscribe<NewVehicleMessage>(SUBSCRIBER_ID, HandleNewVehicleMessage);
            Console.WriteLine("Listening for NewVehicleMessages...");
            Console.ReadLine();
        }

        static void HandleNewVehicleMessage(NewVehicleMessage v) {
            var csv = $"{v.Registration},{v.Color},{v.Year},{v.ModelName},{v.ManufacturerName},{v.ListedAt:O}";
            Console.WriteLine(csv);
        }

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