using Ardalis.SmartEnum;

namespace DVT_Elevator.Enums
{


    public class ElevatorState : SmartEnum<ElevatorState>
    {

        public static readonly ElevatorState Stopped = new ElevatorState(nameof(Stopped), 1);
        public static readonly ElevatorState Idle = new ElevatorState(nameof(Idle), 2);
        public static readonly ElevatorState Moving = new ElevatorState(nameof(Moving), 3);


        //up and down
        public ElevatorState(string name, int value) : base(name, value)
        {
        }
    }
}
