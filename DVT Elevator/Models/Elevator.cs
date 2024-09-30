using DVT_Elevator.Enums;

namespace DVT_Elevator.Models
{
    public class Elevator
    {

        public int ElevatorID { get; set; }
        public string ElevatorName { get; set; }
        public int CurrentElevatorFloor { get; set; }
        public int CurrentPassengerCount { get; set; }
        public int MaxPassengerCount { get; set; }
        public ElevatorDirection Direction { get; set; }
        public ElevatorState State { get; set; }
        public List<Destination> Destinations { get; set; } = new List<Destination> ();

    }
}
