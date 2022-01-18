using System;
using EasyNetQ;
using Autobarn.Messages;
using Microsoft.Extensions.Configuration;
using Autobarn.PricingEngine;
using System.IO;
using System.Threading.Tasks;
using Grpc.Net.Client;

namespace Autobarn.PricingClient {
    class Program {
        private static readonly IConfigurationRoot config = ReadConfiguration();

        private static Pricer.PricerClient grpcClient;

        static void Main(string[] args) {
            using var bus = RabbitHutch.CreateBus(config.GetConnectionString("AutobarnRabbitMQConnectionString"));
            var channel = GrpcChannel.ForAddress(config["AutobarnGrpcServerUrl"]);
            grpcClient = new Pricer.PricerClient(channel);
            const string SUBSCRIBER_ID = "Autobarn.PricingClient";
            bus.PubSub.Subscribe<NewVehicleMessage>(SUBSCRIBER_ID, HandleNewVehicleMessage);
            Console.WriteLine("Listening for NewVehicleMessages...");
            Console.ReadLine();
        }

        static async Task HandleNewVehicleMessage(NewVehicleMessage v) {
            var priceRequest = new PriceRequest {
                Color = v.Color,
                ModelName = v.ModelName,
                ManufacturerName = v.ManufacturerName,
                Year = v.Year
            };
            Console.WriteLine($"Calculating price for {v.ManufacturerName} {v.ModelName} ({v.Year}, {v.Color}...");
            var reply = await grpcClient.GetPriceAsync(priceRequest);
            Console.WriteLine($"{reply.Price} {reply.CurrencyCode}");
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