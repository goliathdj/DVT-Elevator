using DVT_Elevator.Interfaces;
using DVT_Elevator.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Channels;

namespace DVT_Elevator
{




    internal class Program
    {
        static void Main(string[] args)
        {


            var Elevatorcomms = Channel.CreateUnbounded<string>();

            using IHost host = Host.CreateDefaultBuilder(args)
                    .ConfigureServices((context, services) =>//services =>
                    {
                        services.AddSingleton<BuildingConfigurations>();
                        services.AddSingleton<ControlRoom>();
                        services.AddSingleton<ElevatorControlService>();
                        services.AddScoped<IElevatorService, ElevatorService>();
                        services.AddScoped<IPassengerRequestService, PassengerRequestService>();
                        services.AddScoped<IDashboardServer, DashboardRoom>();
                        services.AddHostedService<ElevatorControlService>();
                        services.AddHostedService<PassengerRequestService>();


                        services.AddSingleton(Elevatorcomms);

                    })
                    .Build();

            try
            {

                host.RunAsync();

            }
            catch (Exception ex)
            {

                throw;
            }



            Console.WriteLine("Press c to cancel...");
            Console.WriteLine("Press 'Q' to setup services... \n");
            Console.WriteLine("Press G to Get elevator Information...");



            while (true)
            {
                var keyPressed = Console.ReadKey().Key;

                if (keyPressed == ConsoleKey.C)
                {
                    Console.WriteLine("\nCancelling...");
                    host.StopAsync();
                    break;
                }
               else if (keyPressed == ConsoleKey.Q)
                {
                    
                    Console.WriteLine(" \nLoading configuration for building.");
                    using var scope = host.Services.CreateScope();
                    var services = scope.ServiceProvider;
                    try
                    {
                        var context = services.GetRequiredService<BuildingConfigurations>();
                        context.ConfigureBuilding();
                    }
                    catch (Exception ex)
                    {
                        var logger = services.GetRequiredService<ILogger<Program>>();
                        logger.LogError(ex, "An error occurred when loading configuration for building. {exceptionMessage}", ex.Message);
                    }
                }

                else if (keyPressed == ConsoleKey.G)
                {
                    // TODO: implement observer pattern to allow push messaging from the elevator service to notify the
                    Console.WriteLine(" \nLoading Elevator Information for the building.");
                    using var scope = host.Services.CreateScope();
                    var services = scope.ServiceProvider;
                    try
                    {
                        var context = services.GetRequiredService<IDashboardServer>();
                        context.SetListeningToElevatorsOn();
                    }
                    catch (Exception ex)
                    {
                        var logger = services.GetRequiredService<ILogger<Program>>();
                        logger.LogError(ex, "An error occurred when loading configuration for building. {exceptionMessage}", ex.Message);
                    }


                }
                else if (keyPressed == ConsoleKey.O)
                {
                    // TODO: implement observer pattern to allow push messaging from the elevator service to notify the
                    
                    using var scope = host.Services.CreateScope();
                    var services = scope.ServiceProvider;
                    try
                    {
                        var context = services.GetRequiredService<IDashboardServer>();
                        context.SetListeningToElevatorsOff();
                    }
                    catch (Exception ex)
                    {
                        var logger = services.GetRequiredService<ILogger<Program>>();
                        logger.LogError(ex, "An error occurred when loading configuration for building. {exceptionMessage}", ex.Message);
                    }


                }

                else
                {
                    Console.WriteLine($"\nYou pressed {keyPressed}...");
                }
            }

        }

        static void BuilderConfig(IConfiguration configuration)
        {


        }

        /// <summary>
        ///Display the real-Ɵme status of each elevator, including its current floor, direcƟon of movement,
        ///whether it's in moƟon or staƟonary, and the number of passengers it is carrying.
        /// </summary>
        void RealTimeElevatorStatus() { }




        /// <summary>
        ///Allow users to interact with the elevators through the console applicaƟon. Users should be able
        ///to call an elevator to a specific floor and indicate the number of passengers waiting on each floor. 
        /// </summary>
        void InteractiveElevatorControl() { }


    }





    /// <summary>
    /// if i have time i might add the below to just make it look fancy 
    /// 
    /// 
    /// 
    ///         ConsoleSpiner spin = new ConsoleSpiner();
    //Console.Write("Working....");
    //    while (true) 
    //    {
    //        spin.Turn();
    //    }
/// 
/// </summary>
public class ConsoleSpiner
    {
        int counter;
        public ConsoleSpiner()
        {
            counter = 0;
        }
        public void Turn()
        {
            counter++;
            switch (counter % 4)
            {
                case 0: Console.Write("/"); break;
                case 1: Console.Write("-"); break;
                case 2: Console.Write("\\"); break;
                case 3: Console.Write("|"); break;
            }
            Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
        }
    }


    class FloorRequester
    {

        public bool RequestElevator(int RequestedFloor)
        {

            return true;
        }


    }
}
