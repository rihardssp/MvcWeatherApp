using DataAccessLayer.Contexts;
using DataAccessLayer.Repositories;
using System;
using System.Linq;

namespace BatchQueue
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            using var context = new WeatherContext();
            var rep = new RepositoryWrapper(context);
            var entryCount = rep.Location.FindAll().Count();

            Console.WriteLine($"Entry count in DB: {entryCount}");

            Console.ReadKey();
        }
    }
}
