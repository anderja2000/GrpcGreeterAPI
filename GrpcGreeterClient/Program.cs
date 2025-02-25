using Grpc.Net.Client;
using GrpcGreeter;

using var channel = GrpcChannel.ForAddress("https://localhost:7275");
var client = new Greeter.GreeterClient(channel);

var greeting = new Greeting {
    FirstName = "John",
    LastName = "Doe"
};

var reply = await client.SayHelloAsync(new HelloRequest {
    Name = greeting  // Pass Greeting object instead of string
});

Console.WriteLine($"Greeting: {reply.Message}");

Console.WriteLine("Press any key to exit...");
Console.ReadKey();