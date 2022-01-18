// See https://aka.ms/new-console-template for more information

using EasyNetQ;
using Messages;

var bus = RabbitHutch.CreateBus("amqps://lxsmehro:u_0Wh1RbOEQthm_8VDcFdTe7JsiLLsTm@whale.rmq.cloudamqp.com/lxsmehro");

while (true) {
    // Console.WriteLine("Press any key to publish a message");
    // Console.ReadKey(false);
    var greeting = new Greeting();
    bus.PubSub.Publish(greeting);
    Console.WriteLine($"Published {greeting.Message}");
    Thread.Sleep(TimeSpan.FromSeconds(1));
}

