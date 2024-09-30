using DVT_Elevator.Enums;
using DVT_Elevator.Interfaces;
using System;

namespace DVT_Elevator.Models
{

    public class Destination
    {
        public int DestinationFloor { get; set; }
        public int OriginalFloor { get; set; }
        public int PeopleCount { get; set; }
        public ElevatorDirection Direction { get; set; }
    }

}
