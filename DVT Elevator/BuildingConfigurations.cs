using DVT_Elevator.Models;

namespace DVT_Elevator
{


    public class BuildingConfigurations
    {
        public bool StartService { get; set; } = true;

        public Building Building { get; private set; } = new Building();

        public void ConfigureBuilding()
        {
            try
            {
                Console.WriteLine("Welcome to the configuration of the DVT.");
                Console.WriteLine("Press 'R' to randomize settings. ");
                Console.WriteLine("Press 'M' to setup settings manually.");
                var keyPressed = Console.ReadKey().Key;

                if (keyPressed == ConsoleKey.R)
                {
                    RandomizeBuildingSettings();
                    return;
                }
                else if (keyPressed == ConsoleKey.M)
                {
                    SetBuildingSettingsManually();
                    return;
                }
                else
                {
                    Console.WriteLine("Invalid selection going to home screen.");
                }




            }
            catch (Exception)
            {

                throw;
            }

        }

        void RandomizeBuildingSettings()
        {

            Building = new Building() { AmountOfElevators = Random.Shared.Next(1, 5), AmountOfFloors = Random.Shared.Next(1, 100) };

            Console.WriteLine($" \n Building Settings have been randomized the building now has {Building.AmountOfFloors} floors and {Building.AmountOfElevators} elevators. ");

            Console.WriteLine("If you are happy with these settings press enter to save, or press any other key to Randomize once more. \n");
            if (Console.ReadKey().Key == ConsoleKey.Enter)
            {
                return;
            }
            else
            {
                RandomizeBuildingSettings();
            }
        }

        void SetBuildingSettingsManually()
        {

            setBuildingElevators();
            Console.WriteLine($" \n ");
            setBuildingFloor();

        }
        void setBuildingFloor()
        {
            try
            {
                Console.WriteLine($" \n Please enter the amount of floors");
                int floors = Convert.ToInt32(Console.ReadLine());
                if (floors > 1000)
                {
                    throw new OverflowException();
                }
                Building.AmountOfFloors = floors;

            }
            catch (FormatException)
            {
                Console.WriteLine($" \n Please enter numbers only");
                setBuildingFloor();

            }
            catch (OverflowException)
            {

                Console.WriteLine($" \n The Number entered exceed the logical limit. Please try once more.");
                setBuildingFloor();

            }
            catch (Exception)
            {
                Console.WriteLine($" \n A Fatal error occured Defaulting to Randomizing.");
                RandomizeBuildingSettings();
            }

        }
        void setBuildingElevators()
        {
            try
            {
                Console.WriteLine($" \n Please enter the amount of elevators");
                int elevatorcount = Convert.ToInt32(Console.ReadLine());
                if (elevatorcount > 100)
                {
                    throw new OverflowException();
                }
                Building.AmountOfElevators = elevatorcount;

            }
            catch (FormatException)
            {
                Console.WriteLine($" \n Please enter numbers only");
                setBuildingElevators();

            }
            catch (OverflowException)
            {

                Console.WriteLine($" \n The Number entered exceed the logical limit. Please try once more.");
                setBuildingElevators();

            }
            catch (Exception)
            {
                Console.WriteLine($" \n A Fatal error occured defaulting to randomizing.");
                RandomizeBuildingSettings();
            }

        }
    }


}
