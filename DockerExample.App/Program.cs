using System;
using DockerExample.App.Services;

namespace DockerExample.App
{
    class Program
    {
        static void Main(string[] args)
        {
            var messagingService = new MessagingService();
            Console.WriteLine(messagingService.GetMessage());
            Console.ReadLine();
        }
    }
}
