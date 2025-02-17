
using Microsoft.Extensions.Logging;
using MRS.Application;

namespace Program
{
    class Program
    {


        static void Main(string[] args)
        {

            // LoggerFactory
            using var loggerFactory = LoggerFactory.Create(builder =>/*configure loggerfactory !!*/
            {
                builder
                    .AddConsole()/*write logs in console */
                        .SetMinimumLevel(LogLevel.Information);/*only more important logs than information will be loged like warning or error or Critical*/
            });

            var logger = loggerFactory.CreateLogger<MessageRouterApplication>(); /*creates a new instance of a logger for messageapplication*/
            var messageapplication = new MessageRouterApplication(logger);/*passes logger as dependency to newly created application*/



            Console.ReadLine();
        }
    }
}


