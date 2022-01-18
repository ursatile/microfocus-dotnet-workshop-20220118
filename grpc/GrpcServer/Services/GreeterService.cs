using Grpc.Core;
using GrpcServer;

namespace GrpcServer.Services;

public class GreeterService : Greeter.GreeterBase {
    private readonly ILogger<GreeterService> _logger;
    public GreeterService(ILogger<GreeterService> logger) {
        _logger = logger;
    }

    public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context) {
        var name = request.FirstName + " " + request.LastName;
        var message = GetMessage(name, request.Friendliness);
        return Task.FromResult(new HelloReply {
            Message = message
        });
    }

    private string GetMessage(string name, int friendliness) {
        switch (friendliness) {
            case 2: return ($"Yay! It's {name}! How lovely!");
            case 1: return ($"Oh, it is {name}");
            default: return ($"Oh dear, it's {name}");
        }
    }
}
