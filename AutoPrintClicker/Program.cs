using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AutoPrintClicker
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Canon IP 7200 Auto Clicker");
            Console.WriteLine($"version: {Assembly.GetExecutingAssembly().GetName().Version}");

            Console.WriteLine("==================================");
            Console.WriteLine("Initializing AutoClicker");

            var cancellationTokenSource = new CancellationTokenSource();
            var sleepDuration = TimeSpan.FromSeconds(2);
            var autoClicker = new AutoClicker(cancellationTokenSource.Token, sleepDuration);

            Console.WriteLine("==================================");
            Console.WriteLine("Write `exit` to close the application...");

            var autoClickTask = autoClicker.Watch();

            while (true)
            {
                var input = Console.ReadLine()?.Trim()?.ToLower();
                if (!string.IsNullOrWhiteSpace(input) && input.Equals("exit"))
                {
                    cancellationTokenSource.Cancel();
                    Task.WaitAll(autoClickTask);

                    break;
                }

            }
        }
    }
}