using Grpc.Core;
using GrpcGreeter;

namespace GrpcGreeter.Services;

public class GreeterService : Greeter.GreeterBase
{
    private readonly ILogger<GreeterService> _logger;
    public GreeterService(ILogger<GreeterService> logger)
    {
        _logger = logger;
    }

    public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
    {
        return Task.FromResult(new HelloReply
        {
            Message = $"Hello {request.Name.FirstName} {request.Name.LastName}"
        });
    }

    public override async Task SayHelloManyTimes(HelloManyTimesRequest request, IServerStreamWriter<HelloManyTimesReply> responseStream, ServerCallContext context)
    {

        Console.WriteLine("The server has recieve the request");
        Console.WriteLine(request.ToString());
        string result = String.Format("hello {0} {1}", request.Name.FirstName, request.Name.LastName);

        foreach (int i in Enumerable.Range(0, 10))
        {
            await responseStream.WriteAsync(new HelloManyTimesReply { Message = result }); 
        }

    }

}
