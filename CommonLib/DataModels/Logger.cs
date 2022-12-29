namespace CommonLib.DataModels
{
    public class AQLogger
    {
        public AQLogger(string name)
        {
            this._name = name;
        }

        private string _name;

        public void Action(string? message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write(DateTime.Now.ToString() + "  ");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write(this._name.PadRight(13));
            Console.ResetColor();
            Console.Write(" " + message + "...\n");
        }

        public void Log(string? message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write(DateTime.Now.ToString() + "  ");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write(this._name.PadRight(13));
            Console.ResetColor();
            Console.Write(" " + message + "\n");
        }
        public void Success(string message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write(DateTime.Now.ToString() + "  ");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write(this._name.PadRight(13));
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(" SUCCESS: " + message + "\n");
            Console.ResetColor();
        }
    }

}