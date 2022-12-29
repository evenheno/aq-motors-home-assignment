using CommonLib.DataModels;
using MongoDB.Bson.Serialization;
using SignalServer;

var logger = new AQLogger("Program");
var app = WebApplication.CreateBuilder(args).Build();

BsonClassMap.RegisterClassMap<DTSine>(cm => { cm.AutoMap(); });
BsonClassMap.RegisterClassMap<DTState>(cm => { cm.AutoMap(); });
BsonClassMap.RegisterClassMap<DTSignal>(cm => { cm.AutoMap(); });

//Create new DataManager instance
var dataManager = new DataManager();

//Add middleware to loq all requests and add response headers
app.Use(async (context, next) =>
{
    var req = context.Request;
    logger.Log(String.Format("{0} Request: {1}", req.Method, req.Path));

    //Add headers to response to support crosss-origin calls etc...
    context.Response.Headers.Add("Access-Control-Allow-Headers", new[] { "Origin, X-Requested-With, Content-Type, Accept, Athorization, ActualUserOrImpersonatedUserSamAccount, IsImpersonatedUser" });
    context.Response.Headers.Add("Access-Control-Allow-Methods", new[] { "GET, POST, PUT, DELETE, OPTIONS" });
    context.Response.Headers.Add("Access-Control-Allow-Origin", "*");

    if (context.Request.Method == HttpMethod.Options.Method)
    {
        context.Response.StatusCode = 200;
        await context.Response.WriteAsync("OK");
        return;
    }

    await next.Invoke(context);
});

//Add "/signal" PUT Method to receive and store signals
app.MapPut("/signal", async (HttpContext context) =>
{
    var request = context.Request;
    var body = request.Body;
    var reader = new StreamReader(body);
    var input = await reader.ReadToEndAsync();

    /*Receive integer array instead of JSON, for max performance.
      Should consider maybe using binary data for more performance*/
    var intArray = input.Split(',').Select(Int32.Parse).ToArray();
    var signalType = intArray[0];
    switch (signalType)
    {
        case 0: //Signal type 0 (Sine)
            var frequency = intArray[1];
            var amplitude = intArray[2];
            var dtSine = new DTSine()
            {
                Frequency = frequency,
                Amplitude = amplitude
            };
            DataManager.AddSignal(dtSine);
            logger.Success(String.Format("SINE Signal Recieved"));
            break;
        case 1: //Signal type 1 (State)
            var state = intArray[1];
            logger.Success(String.Format("STATE Signal Recieved"));
            var dtState = new DTState()
            {
                State = state
            };
            DataManager.AddSignal(dtState);
            break;
        default:
            throw new Exception(String.Format("Invalid Signal Type: '{0}'", signalType));
    }

});

//Add "/signals" GET Method to fetch latest 100 signals
app.MapGet("/signals", () =>
{
    logger.Action("Fetching signals from database");
    var results = dataManager.GetSignals();
    return results;
});

logger.Log("Starting Data Manager...");
DataManager.StartThread();

logger.Log("Starting web application...");
app.Run();

