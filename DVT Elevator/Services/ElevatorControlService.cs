using DVT_Elevator.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DVT_Elevator.Services
{
    /*
     * 3. MulƟple Floors and Elevators Support
            Design the applicaƟon to accommodate buildings with mulƟple floors and mulƟple elevators.
            Ensure that elevators can efficiently move between different floors.
     * 4. Efficient Elevator Dispatching
            Implement an algorithm that efficiently directs the nearest available elevator to respond to an
            elevator request. Minimize wait Ɵmes for passengers and opƟmize elevator usage.
     * 5. Passenger Limit Handling
            Consider the maximum passenger limit for each elevator. Prevent the elevator from becoming
            overloaded and handle scenarios where addiƟonal elevators might be required.
     * 6. ConsideraƟon for Different Elevator Types
            Although the challenge focuses on passenger elevators, consider the existence of other elevator
            types, such as high-speed elevators, glass elevators, and freight elevators. Plan the applicaƟon's
            architecture to accommodate future extension for these types.
     * 7. Real-Time OperaƟon
            Ensure that the console applicaƟon operates in real-Ɵme, providing immediate responses to user
            interacƟons and accurately reflecƟng elevator movements and status.
     * 
     * 
     */





    /// <summary>
    /// This service is always active however we can use the console to set it to working status 
    /// </summary>

    public class ElevatorControlService : BackgroundService
    {
        private readonly ILogger<ElevatorControlService> _logger;
        private readonly BuildingConfigurations _configurations;
        public readonly List<IElevatorService> _Elavators = new List<IElevatorService>();
        private readonly IElevatorService _Elavator ;


        public ElevatorControlService(ILogger<ElevatorControlService> logger, BuildingConfigurations configurations,IElevatorService elevatorService)
        {
            try
            {
                _logger = logger;
                _configurations = configurations;
                _Elavator = elevatorService;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {

                if (_configurations.StartService)
                {
                    AddElevatorServices();
                    _logger.LogInformation("Elevator Moving at: {time}", DateTimeOffset.Now); 
                    
                    _configurations.StartService = false;
                    await Task.Delay(5000, stoppingToken);
                  await  StartAllElavatorsAsync(stoppingToken);
                }

                _logger.LogInformation("Service is active and listening : {time}", DateTimeOffset.Now);

                await Task.Delay(10000, stoppingToken);
            }
        }


        public async Task StartAllElavatorsAsync(CancellationToken cancellationToken)
        {
            var tasks = _Elavators.Select(ws => ws.StartAsync(cancellationToken));
            await Task.WhenAll(tasks);
        }

        public async Task StopAllElavatorsAsync(CancellationToken cancellationToken)
        {
            var tasks = _Elavators.Select(ws => ws.StopAsync(cancellationToken));
            await Task.WhenAll(tasks);
        }
        public void AddElevatorServices()
        {

            if (_configurations.Building.AmountOfElevators<1)
            {
                Console.WriteLine("\n There is no elevators listed in the settings automatically assuming Service not configured => Running Auto Setup.");
                _configurations.ConfigureBuilding();
                
            }

            Console.WriteLine($"\n Loading {_configurations.Building.AmountOfElevators} elevators on the system.");
            for (int i = 0; i < _configurations.Building.AmountOfElevators; i++)
            {
                _Elavators.Add(_Elavator);
            }

        }

    }


}
