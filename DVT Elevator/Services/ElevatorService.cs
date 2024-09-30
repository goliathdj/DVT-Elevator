using DVT_Elevator.Interfaces;
using DVT_Elevator.Models;
using Microsoft.Extensions.Logging;
using static System.Net.Mime.MediaTypeNames;
using System.Drawing;
using System.Runtime.InteropServices;
using System;
using System.Collections.Generic;
using DVT_Elevator.Enums;

namespace DVT_Elevator.Services
{
    /// <summary>
    /// this is the main elavator service which does the following 
    /// 
    /// 
    ///check queue to see what is in the queue and closest to this elevator
    ///once checked navigate direction to the floor in question
    ///on navigate do a check on the queue to see if there are any changes and if there is any item that is on the floor.next 
    ///we need a count up or down that increments every two seconds
    ///<<<<<<<<<<<------------->>>>>>>>>>>>
    ///once on the floor reached we do the following 
    ///open elevator 1 sec
    ///load people 5 sec
    ///close 1 sec
    ///increment/decrease passenger count
    ///max 20
    ///*****
    /// if max reached then only do dropoffs 
    ///<<<<<<<<<<<------------->>>>>>>>>>>>>
    /// </summary>
    public class ElevatorService : IElevatorService
    {
        private List<IDashboardServer> controlroomservers = new List<IDashboardServer>();
        private string ElevatorCurrentMessage { get; set; }
        private List<Elevator> Elevators = new List<Elevator>();

        private Elevator elevator { get; set; } = new Elevator() { ElevatorName = Guid.NewGuid().ToString().Split('-').OrderBy(x => x.Length).First(), State = ElevatorState.Stopped, MaxPassengerCount = 20 };


        private readonly ControlRoom ControlRoom;
        // TODO:Change this config below to be configurable via appsettings.json
        private readonly BuildingConfigurations _configurations;
        private Timer? _timer = null;
        private readonly ILogger<ElevatorService> _logger;
        public ElevatorService(ILogger<ElevatorService> logger, ControlRoom controlRoom, BuildingConfigurations configurations)
        {
            _logger = logger;
            ControlRoom = controlRoom;
            _configurations = configurations;
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {

            // TODO:Change this back to LogElevatorinformation 
            _logger.LogInformation($"{elevator.ElevatorName}: has started running.");


            //Spicing code up by starting elevator on random floor 
            elevator.CurrentElevatorFloor = Random.Shared.Next(1, _configurations.Building.AmountOfFloors);

            _timer = new Timer(DoWork, null, TimeSpan.Zero,
                TimeSpan.FromSeconds(1));
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            // TODO:Change this back to LogElevatorinformation 
            _logger.LogInformation("This Elevator Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;

        }

        private async void DoWork(object? state)
        {


            //stop timer so that it pauses for now
            _timer?.Change(Timeout.Infinite, 0);
            //keep running the below loop 
            SetDirection();
            while (true)
            {

                Console.WriteLine($"current passenger count =>{elevator.CurrentPassengerCount}");


                if (elevator.State == ElevatorState.Stopped && elevator.CurrentPassengerCount < 1)
                {
                    SetDirection();
                    return;
                }



                if (elevator.Direction == ElevatorDirection.Up)
                {
                    await MoveUpElevator();
                }
                else
                {
                    await MoveDownElevatorAsync();
                }

            }

        }


        public void Dispose()
        {
            _timer?.Dispose();
        }
        private bool StartElevator()
        {

            return true;
        }
        private bool StopElevator()
        {

            return true;
        }
        private async Task<bool> MoveUpElevator()
        {
            int destinationfloor;

            if (elevator.Destinations.Count == 0)
            {
                destinationfloor = _configurations.Building.AmountOfFloors;
            }
            else
                destinationfloor = elevator.Destinations.OrderByDescending(x => x.DestinationFloor).FirstOrDefault().DestinationFloor;


            while (elevator.CurrentElevatorFloor <= destinationfloor)
            {
                //check if the floor we are on has passengers for the floor










                if (elevator.Destinations.Where(x => x.DestinationFloor == elevator.CurrentElevatorFloor).Count() > 0 ||
                    ControlRoom.CheckFloorsForPickup(elevator.CurrentElevatorFloor, ElevatorDirection.Up).Select(x => x.PeopleCount).Sum() > 0)
                {
                    await OpenDoorElevator(ElevatorDirection.Up);
                    //will need to change below logic to check if there is no one in the elevator and if the destinations are no more.
                    if (elevator.Destinations.Count() > 0)
                    {
                        destinationfloor = elevator.Destinations.OrderByDescending(x => x.DestinationFloor).FirstOrDefault().DestinationFloor;
                    }
                }
                _logger.LogInformation($"Elevator currently going to {elevator.CurrentElevatorFloor}");
                elevator.CurrentElevatorFloor = elevator.CurrentElevatorFloor + 1;
                _logger.LogInformation($"Elevator final destination {destinationfloor}");

                await Task.Delay(2000);
            }
            SetDirection();
            return true;
        }
        private async Task<bool> MoveDownElevatorAsync()
        {



            while (elevator.CurrentElevatorFloor >= 1)
            {
                //check if the floor we are on has passengers for the floor
                if (elevator.Destinations.Where(x => x.DestinationFloor == elevator.CurrentElevatorFloor).Count() > 0 ||
                    ControlRoom.CheckFloorsForPickup(elevator.CurrentElevatorFloor, ElevatorDirection.Up).Select(x => x.PeopleCount).Sum() > 0)
                {
                    await OpenDoorElevator(ElevatorDirection.Down);

                }
                _logger.LogInformation($"Elevator currently going to {elevator.CurrentElevatorFloor}");
                elevator.CurrentElevatorFloor = elevator.CurrentElevatorFloor - 1;
                await Task.Delay(2000);
            }
            SetDirection();
            return true;
        }

        private async Task ValidateFloorToOpen(ElevatorDirection direction)
        {
            var currentElevatorDestinations = elevator.Destinations.Where(x => x.DestinationFloor == elevator.CurrentElevatorFloor).ToList();
            if (elevator.Destinations.Where(x => x.DestinationFloor == elevator.CurrentElevatorFloor).Count() > 0)
            {
                OpenDoorElevator(direction);
            }

        }

        private async Task OpenDoorElevator(ElevatorDirection direction)
        {

            _logger.LogInformation($"Opening Floor on => {elevator.CurrentElevatorFloor}:\n Passenger count =>{elevator.CurrentPassengerCount}");
            var currentElevatorDestinations = elevator.Destinations.Where(x => x.DestinationFloor == elevator.CurrentElevatorFloor).ToList();
            foreach (Destination destination in currentElevatorDestinations)
            {
                elevator.Destinations.Remove(destination);
            }
            //Pause simulating offload of users
            await Task.Delay(2000);

            var NextFloorPickup = ControlRoom.CheckFloorsForPickup(elevator.CurrentElevatorFloor, direction);
            if (NextFloorPickup.Select(x => x.PeopleCount).Sum() > 0 && (elevator.CurrentPassengerCount + NextFloorPickup.Select(x => x.PeopleCount).Sum()) < elevator.MaxPassengerCount)
            {
                //Load destinations
                elevator.Destinations.AddRange(ControlRoom.CheckFloorsForPickup(elevator.CurrentElevatorFloor, direction));

                ControlRoom.RemoveFloorFromQueueList(elevator.CurrentElevatorFloor, direction);

                await Task.Delay(2000);
            }
            await CloseDoorElevator();

        }
        private async Task CloseDoorElevator()
        {

            elevator.CurrentPassengerCount = elevator.Destinations.Select(x => x.PeopleCount).Sum();
            _logger.LogInformation($"Closing Floor on => {elevator.CurrentElevatorFloor}:\n Passenger count =>{elevator.CurrentPassengerCount}");
            await Task.Delay(2000);

        }
        private void SetDirection()
        {
            Destination floor = new Destination();
            floor = ControlRoom.GetNextFloor(elevator.CurrentElevatorFloor);

            if (floor.PeopleCount < 1)
            {
                elevator.State = ElevatorState.Stopped;
            }
            else if (floor.OriginalFloor > elevator.CurrentElevatorFloor)
            {
                elevator.Direction = ElevatorDirection.Up;
                elevator.State = ElevatorState.Moving;
            }
            else
            {
                elevator.Direction = ElevatorDirection.Down;
                elevator.State = ElevatorState.Moving;
            }

        }
        public int GetCurrentFloor()
        {
            return elevator.CurrentElevatorFloor;
        }
        public Elevator CheckStatus()
        {
            return elevator;
        }



        /// <summary>
        /// use this to send messages to all the listeners
        /// </summary>
        /// <param name="Message">this is the message to be posted to any subs</param>
        void _logger.LogInformation(string Message)
        {
            ElevatorCurrentMessage = Message;
            NotifyObservers();
        }

        public void RegisterObserver(IDashboardServer ContolRoomserver)
        {
            Console.WriteLine("Observer Added : " + ((DashboardRoom)ContolRoomserver).MainDashboard);
            controlroomservers.Add(ContolRoomserver);
        }
        public void RemoveObserver(IDashboardServer ContolRoomserver)
        {
            Console.WriteLine("Observer Removed : " + ((DashboardRoom)ContolRoomserver).MainDashboard);
            controlroomservers.Remove(ContolRoomserver);
        }

        public void NotifyObservers()
        {
            Console.WriteLine();
            foreach (IDashboardServer observer in controlroomservers)
            {
                //By Calling the Update method, we are sending notifications to observers
                observer.Update(ElevatorCurrentMessage);
            }
        }


        public void AddElevatorService(Elevator elevator)
        {

            Elevators.Add(elevator);
        }

    }

}
