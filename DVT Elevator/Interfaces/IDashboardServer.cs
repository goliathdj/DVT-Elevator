namespace DVT_Elevator.Interfaces
{
    public interface IDashboardServer
    {
        void ListenToElevators();

        // Receive Notification from Elevator
        void Update(string message);
    }




}
