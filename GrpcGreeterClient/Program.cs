using Grpc.Net.Client;
using Grpc.Core;
using GrpcGreeter;
namespace GrpcGreeterClient;


class Program
{
    const string target = "https://localhost:7275";

    static async Task Main(string[] args)
    {

        using var channel = GrpcChannel.ForAddress(target);

        await channel.ConnectAsync().ContinueWith((task) =>
        {
            if (task.Status == TaskStatus.RanToCompletion)
            {
                Console.WriteLine("Connected to server");
            }
        });

        var client = new Greeter.GreeterClient(channel);


        // UnaryGreeting(client);
        // await ServerStreamingGreeting(client);
        // await ClientStreamingGreeting(client);
        await BidiStreamingGreeting(client);



        channel.ShutdownAsync().Wait();
        Console.ReadKey();

    }


    public static void UnaryGreeting(Greeter.GreeterClient client)
    {
        var name = new Greeting
        {
            FirstName = "John",
            LastName = "Doe"
        };

        var request = new HelloRequest() { Name = name };
        var response = client.SayHello(request);

        Console.WriteLine(response.Message);


    }

    public static async Task ServerStreamingGreeting(Greeter.GreeterClient client)
    {
        var name = new Greeting
        {
            FirstName = "John",
            LastName = "Doe"
        };

        var request = new HelloManyTimesRequest() { Name = name };
        var response = client.SayHelloManyTimes(request);

        await foreach (var message in response.ResponseStream.ReadAllAsync())
        {
            Console.WriteLine(response.ResponseStream.Current.Message);
            await Task.Delay(200);
        }


    }

    public static async Task ClientStreamingGreeting(Greeter.GreeterClient client)
    {
        var name = new Greeting
        {
            FirstName = "John",
            LastName = "Doe"
        };

        var request = new LongHelloRequest() { Name = name };
        var stream = client.LongHello();

        foreach (int i in Enumerable.Range(1, 10))
        {
            await stream.RequestStream.WriteAsync(request);
        }

        await stream.RequestStream.CompleteAsync();

        var response = await stream.ResponseAsync;

        Console.WriteLine(response.Message);

    }

    public static async Task BidiStreamingGreeting(Greeter.GreeterClient client)
    {
        var stream = client.GreetEveryone();

        var responseReaderTask = Task.Run(async () =>
        {
            while (await stream.ResponseStream.MoveNext())
            {
                Console.WriteLine($"Received: {stream.ResponseStream.Current.Message}");
            }
        });

        Greeting[] greetings =
        {
            new() { FirstName = "John", LastName = "Doe" },
            new() { FirstName = "Jeff", LastName = "Thompson" },
            new() { FirstName = "Sylvia", LastName = "Smith" },
            new() { FirstName = "Jane", LastName = "Smith" },
            new() { FirstName = "Harry", LastName = "Baker" },
        };

        foreach (var greeting in greetings)
        {
            await stream.RequestStream.WriteAsync(new GreetEveryoneRequest()
            {
                Name = greeting
            });
        }

        await stream.RequestStream.CompleteAsync();
        await responseReaderTask;


    }
}