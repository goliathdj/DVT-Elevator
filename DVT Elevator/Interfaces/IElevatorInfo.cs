namespace DVT_Elevator.Interfaces
{
    public interface IElevatorInfo
    {
        // Register an observer to the subject.
        void RegisterObserver(IDashboardServer observer);
        // Remove or unregister an observer from the subject.
        void RemoveObserver(IDashboardServer observer);
        // Notify all registered observers when the state of the subject is changed.
        void NotifyObservers();
    }




}
