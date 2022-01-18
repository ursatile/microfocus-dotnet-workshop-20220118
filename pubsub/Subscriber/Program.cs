using EasyNetQ;
using Messages;

var bus = RabbitHutch.CreateBus("amqps://lxsmehro:u_0Wh1RbOEQthm_8VDcFdTe7JsiLLsTm@whale.rmq.cloudamqp.com/lxsmehro");

bus.PubSub.Subscribe<Greeting>("SUBSCRIBER_ID", g => {
    Console.WriteLine($"Received {g.Message}");
});
Console.WriteLine("Listening for messages... press Enter to Exit.");
Console.ReadLine();