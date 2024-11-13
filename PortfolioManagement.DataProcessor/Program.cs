using Microsoft.Extensions.Configuration;
using PortfolioManagement.DataProcessor.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace PortfolioManagement.DataProcessor
{
    public class Program 
    {

        public static IConfigurationRoot Configuration;

        static void Main(string[] args)
        {
            try
            {
                Console.OutputEncoding = Encoding.UTF8;
                var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

                Configuration = builder.Build();
                
                Log.Write("Start data processor...");

                List<int> minutes = AppSettings.Minutes;
                int randomMin = AppSettings.RandomMin;
                int randomMax = AppSettings.RandomMax;
                Random random = new Random();

                while (true) {
                    int currentMinute = DateTime.UtcNow.Minute;
                    if (minutes.Any(x => x == currentMinute)) 
                    {
                        int randomMilisecond = random.Next(randomMin, randomMax)*60000;
                        Thread.Sleep(randomMilisecond);
                        
                        Log.Write("Start process...");
                        Processor processor = new Processor();
                        processor.Start();
                        Log.Write("End process...");
                    }
                    Thread.Sleep(60000);
                }
                Log.Write("End data processor...");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in data processor...");
                ex.WriteLogFile();
            }
        }

    }

}