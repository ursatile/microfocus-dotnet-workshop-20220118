using EasyNetQ;
using Messages;

var bus = RabbitHutch.CreateBus("amqps://lxsmehro:u_0Wh1RbOEQthm_8VDcFdTe7JsiLLsTm@whale.rmq.cloudamqp.com/lxsmehro");

// Set subscriberId to something unique
var subscriberId = Environment.MachineName;

bus.PubSub.Subscribe<Greeting>(subscriberId, g => {
    try {
        if (g.Message.Contains("5 ")) throw new Exception("Simulated exception");
        Console.WriteLine($"Received {g.Message}");
    } catch (Exception ex) {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"ERROR: {ex.Message}");
        Console.ForegroundColor = ConsoleColor.White;
        throw;
    }
});
Console.WriteLine("Listening for messages... press Enter to Exit.");
Console.ReadLine();