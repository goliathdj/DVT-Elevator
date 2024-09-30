using Ardalis.SmartEnum;

namespace DVT_Elevator.Enums
{
    public class ElevatorDirection : SmartEnum<ElevatorDirection>
    {

        public static readonly ElevatorDirection Up = new ElevatorDirection(nameof(Up), 1);
        public static readonly ElevatorDirection Down = new ElevatorDirection(nameof(Down), 2);

        //up and down
        public ElevatorDirection(string name, int value) : base(name, value)
        {
        }
    }
}
