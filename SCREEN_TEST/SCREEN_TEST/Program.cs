class Program
{
    static void Main(string[] args)
    {
        using (Engine game = new Engine())
        {
            if (game.CheckCacheStatus())
                game.Run();
            else
            {
                (new GameCrashHandler()).Run();
            }
        }
    }
}

