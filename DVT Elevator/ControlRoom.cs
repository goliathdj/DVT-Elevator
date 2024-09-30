using DVT_Elevator.Enums;
using DVT_Elevator.Interfaces;
using DVT_Elevator.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DVT_Elevator
{
    public class ControlRoom
    {
        private List<Destination> Destinations { get; set; } = new List<Destination>();

        public ControlRoom()
        {
        }

        public bool RequestFloor(Destination destination)
        {
            if (destination.DestinationFloor > destination.OriginalFloor)
            {
                destination.Direction = ElevatorDirection.Up;
            }
            else
            {
                destination.Direction = ElevatorDirection.Down;
            }

            Destinations.Add(destination);
            return true;
        }

        public List<Destination> CheckFloorsForPickup(int currentFloor, ElevatorDirection elevatorDirection)
        {
            return Destinations.Where(x => x.OriginalFloor == currentFloor && x.Direction == elevatorDirection).OrderBy(x => x.OriginalFloor).ToList();
        }

        public bool RemoveFloorFromQueueList(int currentFloor, ElevatorDirection elevatorDirection)
        {

            var existingDestinations = Destinations.Where(x => x.OriginalFloor == currentFloor && x.Direction == elevatorDirection).ToList();
            foreach (Destination destination in existingDestinations)
            {
                Destinations.Remove(destination);
            }
            return true;
        }

        public Destination GetNextFloor(int currentFloor)
        {



            var currentListOfDestinations = Destinations.ToList();
            if (currentListOfDestinations is null || currentListOfDestinations.Count() < 1)
            {
                return new Destination();
            }

            Console.WriteLine("\n list count below =>" + currentListOfDestinations.Count());

            var FirstfloorCompare = currentListOfDestinations.Where(x => x.OriginalFloor <= currentFloor).OrderBy(x => x.OriginalFloor).FirstOrDefault();
            var SecondfloorCompare = currentListOfDestinations.Where(x => x.OriginalFloor >= currentFloor).OrderBy(x => x.OriginalFloor).FirstOrDefault();

            if (FirstfloorCompare is null)
            {
                return SecondfloorCompare;
            }
            if (SecondfloorCompare is null)
            {
                return FirstfloorCompare;
            }

            if ((currentFloor - FirstfloorCompare.OriginalFloor) > (SecondfloorCompare.OriginalFloor - currentFloor))
            {
                return SecondfloorCompare;
            }
            else
            {
                return FirstfloorCompare;
            }


        }


    }
}
