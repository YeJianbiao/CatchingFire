using System;
using Microsoft.Owin.Hosting;
using WebApi;

namespace WebApiHost
{
    class Program
    {
        private const string HostAddress = "http://localhost:1680";
        static void Main(string[] args)
        {
            //Utility.Helper.Console.Hide();

            WebApp.Start<Startup>(HostAddress);
           
            Console.Read();
        }

        
    }
    
}
