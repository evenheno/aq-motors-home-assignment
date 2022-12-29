using System.Text;
using Device;

AnomalyGenerator.StartThread();

var start = DateTime.Now;
var signalsSent = 0;
var sendSignals = true;
var minSPS = 500;
var http = new HttpClient();

var getSignalSine = (int frequency, int amplitude) =>
{
    var stringData = String.Format("0,{0},{1}",frequency,amplitude);
    var stringContent = new StringContent(stringData, Encoding.UTF8, "text/plain");
    return stringContent;
};

var getSignalState = (int state) =>
{
    var stringData = String.Format("1,{0}", state);
    var stringContent = new StringContent(stringData, Encoding.UTF8, "text/plain");
    return stringContent;
};

var sendSine = async  () =>
{
    var content = getSignalSine(100, Random.Shared.Next(0, 32));
    await http.PutAsync("http://localhost:5000/signal", content);
};

var sendState = async () =>
{
    var content = getSignalState(Random.Shared.Next(256, 4095));
    await http.PutAsync("http://localhost:5000/signal", content);
};

while (sendSignals)
{
    Console.WriteLine(String.Format("Sending signal No. {0}", signalsSent + 1));
    await sendSine();
    await sendState();
 
    var timeSpan = DateTime.Now.Subtract(start);
    if(timeSpan.TotalMilliseconds >= 1000)
    {
        //sendSignals = false;
        //break;
    }
    signalsSent++;
}

Console.WriteLine(String.Format("\nExecution Finished. {0} Signals sent to server", signalsSent));

if(signalsSent < minSPS)
{
    Console.WriteLine(String.Format("Low Performance. Minimum required SPS: {0}", minSPS));
}
else
{
    Console.WriteLine(String.Format("Good Performance (:", minSPS));
}