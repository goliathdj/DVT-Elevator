using DVT_Elevator;
using DVT_Elevator.Models;
using FluentAssertions;

namespace Elevator.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void RequestElevatorToFloor()
        {

            var controlRoom = new ControlRoom();
            Destination destination = new Destination() { OriginalFloor = 1, DestinationFloor = 10, PeopleCount = 1 };
            var result = controlRoom.RequestFloor(destination);
            controlRoom.Destinations.Should().NotBeNullOrEmpty();

        }
    }
}