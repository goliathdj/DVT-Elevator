using DVT_Elevator.Enums;
using DVT_Elevator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVT_Elevator.Interfaces
{
    public interface IElevatorService
    {
        Elevator CheckStatus();
        int GetCurrentFloor();

        Task StartAsync(CancellationToken cancellationToken);
        Task StopAsync(CancellationToken cancellationToken);



        //changed from observer to channel


        //void RegisterObserver(IDashboardServer observer);
        //// Remove or unregister an observer from the subject.
        //void RemoveObserver(IDashboardServer observer);
        //// Notify all registered observers when the state of the subject is changed.
        //void NotifyObservers();
    }


}
