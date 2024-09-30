using DVT_Elevator.Interfaces;
using DVT_Elevator.Services;

namespace DVT_Elevator
{

    // TODO:Fix this to allow interactivity and hide show console messages from elevators 
    public class DashboardRoom : IDashboardServer
    {
        private ElevatorControlService elevatorControlService;
        public  string MainDashboard = "DVT";
        IServiceProvider ServiceProvider { get; set; }
        public DashboardRoom( IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;

        }


        public void Update(string message)
        {
            Console.WriteLine(message);
        }
        public void AddSubscriber(IElevatorInfo elevator)
        {
            elevator.RegisterObserver(this);
        }
        //Removing the ControlRoom from the elevator
        public void RemoveSubscriber(IElevatorInfo elevator)
        {
            elevator.RemoveObserver(this);
        }

        // TODO: List the elevators below and subcribe to thier messages 
        public void ListenToElevators()
        {
            



         





        }

    }
}
