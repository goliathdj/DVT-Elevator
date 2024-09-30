using DVT_Elevator.Interfaces;
using DVT_Elevator.Services;
using System.Threading.Channels;

namespace DVT_Elevator
{

    // TODO:Fix this to allow interactivity and hide show console messages from elevators 
    public class DashboardRoom : IDashboardServer
    {
        private ElevatorControlService elevatorControlService;
        public string MainDashboard = "DVT";
        private readonly Channel<string> elevatorcomms;
        private Timer? _timer = null;

        bool showElevatorReport = false;


        static object lockobject = new object();
        IServiceProvider ServiceProvider { get; set; }
        public DashboardRoom(Channel<string> channel)//IServiceProvider serviceProvider)
        {
            //ServiceProvider = serviceProvider;

            elevatorcomms = channel;
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
        public async void SetListeningToElevatorsOn()
        {

            showElevatorReport = true;
            await Task.Run(() =>
            {
                reportelevatorInfo();
            });

        }

        public async void SetListeningToElevatorsOff()
        {
            //TODO: change the thread and set reporting to off

            showElevatorReport = false;

        }


        async void reportelevatorInfo()
        {

            _timer?.Change(Timeout.Infinite, 0);
            while (showElevatorReport)
            {
                while (await elevatorcomms.Reader.WaitToReadAsync())
                {
                    while (elevatorcomms.Reader.TryRead(out string item))
                    {
                        Console.WriteLine(item);

                        if (!showElevatorReport)
                        {
                            return;
                        }
                    }
                }
            }

        }

    }
}
