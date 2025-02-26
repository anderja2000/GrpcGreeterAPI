# In this exercise, your goal is to implement a ComputeAverage RPC Client Streaming API in a CalculatorService:

- The function takes a `stream of Request message` that has `one integer`, and returns a `Response with a double` that represents the `computed average`

- Remember to first `implement the service definition` in a .`proto file`, alongside the RPC messages

- Implement the `Server code first`

- Test the server code by `implementing the Client`

## Example:

The client will send a `stream of numbers (1,2,3,4)` and the server will `respond with (2.5)`, because `(1+2+3+4)/4 = 2.5`

