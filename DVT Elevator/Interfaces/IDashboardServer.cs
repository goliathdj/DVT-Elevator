namespace DVT_Elevator.Interfaces
{
    public interface IDashboardServer
    {
        void SetListeningToElevatorsOff();
        void SetListeningToElevatorsOn();

        // Receive Notification from Elevator
        void Update(string message);
    }




}
