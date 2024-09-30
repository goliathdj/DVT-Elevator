using DVT_Elevator.Interfaces;
using DVT_Elevator.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Drawing;

namespace DVT_Elevator.Services
{
    /// <summary>
    /// This Class simulates normal operation of business.
    /// this makes random calls essentially simulating users on different floors pressing elavator buttons 
    /// </summary>
    public class PassengerRequestService : BackgroundService, IPassengerRequestService
    {
        private readonly ControlRoom ControlRoom;
        private Timer? _timer = null;
        private readonly BuildingConfigurations _configurations;

        private readonly ILogger<PassengerRequestService> _logger;
        public PassengerRequestService(ControlRoom controlRoom, ILogger<PassengerRequestService> logger, BuildingConfigurations buildingConfigurations)
        {
            ControlRoom = controlRoom;
            _logger = logger;
            _configurations = buildingConfigurations;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {


            _timer = new Timer(SimulateOtherPassengerRequests, null, TimeSpan.Zero,
                TimeSpan.FromSeconds(5));

            return Task.CompletedTask;

        }

        void SimulateOtherPassengerRequests(object? state)
        {

            //check if configured then make random  calls throughout the building
            if (_configurations.Building.AmountOfFloors > 0)
            {

                Destination destination = new Destination()
                {
                    DestinationFloor = Random.Shared.Next(1, _configurations.Building.AmountOfFloors),
                    OriginalFloor = Random.Shared.Next(1, _configurations.Building.AmountOfFloors),
                    PeopleCount = Random.Shared.Next(1, 5)
                };

                ControlRoom.RequestFloor(destination);
                _logger.LogInformation("New Destination added");
            }


        }

    }


}
