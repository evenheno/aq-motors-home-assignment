using CommonLib.DataModels;
using MongoDB.Bson;
using MongoDB.Driver;

namespace SignalServer
{
    public class DataManager
    {
        public IMongoCollection<dynamic> signals;
        private int _bufferLength;
        private static List<DTSignal> _dataQueue;
        private static AQLogger _logger;

        public DataManager()
        {
            //Create new logger
            _logger = new AQLogger("DataQueueManager");

            //Set buffer size for bulk inserts
            _bufferLength = 100;

            //Create new list instance
            _dataQueue = new List<DTSignal>();

            //Init database
            var connectionString = "mongodb+srv://dbuser-or:5BBDaTQVZ7nAqLnH@aqdb-dev.o8dbo2b.mongodb.net/home-assignment";
            var mongoClient = new MongoClient(connectionString);
            var mongoDB = mongoClient.GetDatabase("home-assignment");
            signals = mongoDB.GetCollection<dynamic>("signals");
        }

        public dynamic GetSignals()
        {
            var maxResults = 100;
            //Create empty filter (no need to filter query)
            var filter = new BsonDocument() { };
            //Fetch data and limit max results according to 'maxResults' variable
            var signals = this.signals.Find(filter).Limit(maxResults).ToList().ToArray();
            //Cast array and return results
            return signals;
        }

        /** Add signal to queue */
        public static void AddSignal(DTSignal signal)
        {
            _dataQueue.Add(signal);
        }

        /** Create new DataManager instance and start in a new thread */
        public static void StartThread()
        {
            _logger.Action("Creating new DataManager instance");
            DataManager dataManager = new DataManager();
            var starter = new ParameterizedThreadStart(dataManager.HandleQueue);
            Thread thread = new Thread(starter);
            _logger.Action("Starting queue handler thread");
            thread.Start(_dataQueue);
        }

        /** Take buffers and perform bulk inserts to database */
        public void HandleQueue(Object? parameter)
        {
            _logger.Success("Queue handler thread started");
            
            //Cast Object to List
            List<DTSignal> dataQueue = (List<DTSignal>)parameter!;

            while (true)
            {
                //Take buffer from data queue in the length specified
                var signalsBuffer = dataQueue.Take(_bufferLength);

                var signalsBufferLen = signalsBuffer.Count();

                //Check if buffer has items to process
                if (signalsBuffer != null && signalsBufferLen > 0)
                {
                    _logger.Action("Generating id's");
                    
                    //Generate new ID's for each signal
                    //TODO: Maybe ID Generation should not occur here..
                    foreach (var signal in signalsBuffer)
                    {
                        signal.id = ObjectId.GenerateNewId();
                    }
                    _logger.Action(String.Format("Inserting {0} signals to DB...", signalsBufferLen));
                    
                    //Insert current buffer into DB
                    signals.InsertMany(signalsBuffer);

                    _logger.Success(String.Format("{0} Signals inserted", signalsBufferLen));

                    //Remove items from the data queue
                    _logger.Action("Removing items from queue");
                    foreach (var signal in signalsBuffer)
                    {
                        dataQueue.Remove(signal);
                    }

                    var queueLength = dataQueue.Count();
                    _logger.Log(String.Format("{0} Signals remained to proccess",queueLength));
                    
                }
            }
        }
    }
}