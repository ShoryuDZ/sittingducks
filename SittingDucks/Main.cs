using AppKit;

namespace SittingDucks
{
    static class MainClass
    {
        static void Main(string[] args)
        {
            NSApplication.Init();
            NSApplication.Main(args);
        }

        public static void doSomeThing()
        {
            System.Console.WriteLine("Thing completed");
        }
    }
}
