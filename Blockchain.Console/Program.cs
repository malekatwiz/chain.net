namespace Blockchain.Console
{
    using Microsoft.Extensions.Logging;
    using Serilog;
    using Serilog.Core;
    using System;
    using System.Threading.Tasks;

    public class Program
    {
        async static Task Main(string[] args)
        {
            var loggerConfig = new LoggerConfiguration();
            loggerConfig.WriteTo.Console();

            var logger = loggerConfig.CreateLogger();
            var node = new Node.NodeCore("node-1", logger);
            node.Mine();
            await Task.Delay(100);

            node.Mine();
            //while (true)
            //{
            //    node.Mine();
            //    Task.Delay(1000).GetAwaiter().GetResult();
            //}
            
            Console.ReadLine();
        }
    }
}
