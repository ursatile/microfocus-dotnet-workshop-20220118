using Grpc.Net.Client;
using GrpcServer;

using var channel = GrpcChannel.ForAddress("https://localhost:7005");
var grpcClient = new Greeter.GreeterClient(channel);
Console.WriteLine("Connected to gRPC!");
var i = 0;
while (true) {
    Console.WriteLine("Press any key to make a request...");
    Console.ReadKey();
    var request = new HelloRequest {
        FirstName = "Microfocus",
        LastName = $" ({i++})",
        Lang = "en-GB",
        Friendliness = i % 3
    };

    var reply = grpcClient.SayHello(request);
    Console.WriteLine(reply.Message);
}
