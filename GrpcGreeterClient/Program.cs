using Grpc.Net.Client;
using Grpc.Core; 
using GrpcGreeter;

using var channel = GrpcChannel.ForAddress("https://localhost:7275");

var client = new Greeter.GreeterClient(channel);

var name = new Greeting {
    FirstName = "John",
    LastName = "Doe"
};

// unary streaming 

// var reply = await client.SayHelloAsync(new HelloRequest {
//     Name = greeting  // Pass Greeting object instead of string
// });

// server streaming 

// var request = new HelloManyTimesRequest() { Name = name}; 
// var response = client.SayHelloManyTimes(request);

// await foreach (var message in response.ResponseStream.ReadAllAsync()) { 
//     Console.WriteLine(response.ResponseStream.Current.Message);
//     await Task.Delay(200); 
// }

// client streaming 

var request = new LongHelloRequest() { Name = name }; 
var stream = client.LongHello(); 

foreach( int i in Enumerable.Range(1,10)) { 
    await stream.RequestStream.WriteAsync(request);
}

await stream.RequestStream.CompleteAsync(); 

var response = await stream.ResponseAsync;

Console.WriteLine(response.Message);




Console.WriteLine("Press any key to exit...");
Console.ReadKey();