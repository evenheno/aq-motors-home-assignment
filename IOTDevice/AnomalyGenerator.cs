using System.Text;

namespace Device
{
    public class AnomalyGenerator
    {
        private HttpClient _httpClient;
        private DateTime _nextGeneration;

        public AnomalyGenerator()
        {
            _httpClient = new HttpClient();
        }

        private StringContent _getSignalSine(int frequency, int amplitude)
        {
            var stringData = String.Format("0,{0},{1}", frequency, amplitude);
            var stringContent = new StringContent(stringData, Encoding.UTF8, "text/plain");
            return stringContent;
        }

        private StringContent _getSignalState(int state)
        {
            var stringData = String.Format("1,{0}", state);
            var stringContent = new StringContent(stringData, Encoding.UTF8, "text/plain");
            return stringContent;
        }

        private async Task sendSineAnomaly()
        {
            var content = _getSignalSine(100, Random.Shared.Next(33, 70));
            await _httpClient.PutAsync("http://localhost:5000/signal", content);
        }

        private async Task sendStateAnomaly()
        {
            var content = _getSignalState(Random.Shared.Next(4096, 10000));
            await _httpClient.PutAsync("http://localhost:5000/signal", content);
        }

        private void _markNextGeneration()
        {
            var now = DateTime.Now;
            _nextGeneration = now.AddSeconds(Random.Shared.Next(2, 5));
            TimeSpan diff = _nextGeneration.Subtract(now);
            Console.WriteLine("Next generation: " + diff.TotalSeconds);
        }

        public static void StartThread()
        {
            Console.WriteLine("Starting thread...");
            AnomalyGenerator anomalyGenerator = new AnomalyGenerator();
            var starter = new ParameterizedThreadStart(anomalyGenerator.Generate);
            Thread thread = new Thread(starter);
            thread.Start();
        }

        public async void Generate(Object? parameter)
        {
            _markNextGeneration();

            while (true)
            {
                var now = DateTime.Now;
                TimeSpan diff = _nextGeneration.Subtract(now);

                if (diff.TotalSeconds <= 0)
                {
                    Console.WriteLine("Sending anomaly...");
                    await sendSineAnomaly();
                    await sendStateAnomaly();
                    _markNextGeneration();
                }
            }
        }
    }
}