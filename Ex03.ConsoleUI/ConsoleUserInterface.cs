using Ex02.ConsoleUtils;
using Ex03.GarageLogic;
using System;
using System.Collections.Generic;

namespace Ex03.ConsoleUI
{

    public class ConsoleUserInterface
    {
        private readonly Garage r_Garage;
        private bool m_ProgramExit;
        private AddCarToGarage addCarToGarage;
        public ConsoleUserInterface()
        {
            this.addCarToGarage = new AddCarToGarage();
            this.r_Garage = new Garage();
            this.m_ProgramExit = false;
        }

        public enum eMainMenuOptions
        {
            AddVehicleToGarage = 1,
            ListVehiclesByStatus,
            ChangeVehicleStatus,
            AddTiresAirPressure,
            AddFuef,
            ChargeBattery,
            ShowVehicleInfo,
            ExitProgram,
        }


        private static void printMainMenu()
        {

            Console.BackgroundColor = ConsoleColor.Blue;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Garage management program");
            Console.ResetColor();


            const string k_Menu = @"
Please enter your choise (1-8)
-------------------------------
1) Add new vehicle to the garage.
2) List all vehicles in the garage group by status.
3) Change vehicle status.
4) Add tires air pressure.
5) Fill fuel.
6) Charge electric car battery.
7) Show vehicle details by license number.
8) Exit program.";

            Console.WriteLine(k_Menu);
        }

        private int parseIntegerToStringValidation(string i_StringInput)
        {
            int userIntegerInput;
            int numberOfEnumItems = Enum.GetNames(typeof(eMainMenuOptions)).Length;

            if (int.TryParse(i_StringInput, out userIntegerInput) == false)
            {
                throw new FormatException();
            }
            else if (userIntegerInput < 1 || userIntegerInput > numberOfEnumItems)
            {
                throw new ValueOutOfRangeException(1, numberOfEnumItems);
            }

            return userIntegerInput;
        }

        private void mainMenuOptions(int i_UserInputSelection)
        {
            switch ((eMainMenuOptions)i_UserInputSelection)
            {
                case eMainMenuOptions.AddVehicleToGarage:
                    this.addNewVehicleToGarage();
                    break;
                case eMainMenuOptions.ListVehiclesByStatus:
                    this.printVehicleGroupByStatus();
                    break;
                case eMainMenuOptions.ChangeVehicleStatus:
                    this.getStatusChangeFromUser();
                    break;
                case eMainMenuOptions.AddTiresAirPressure:
                    this.getNewAirPressureFromUser();
                    break;
                case eMainMenuOptions.AddFuef:
                    this.getNewFuelRateFromUser();
                    break;
                case eMainMenuOptions.ChargeBattery:
                    this.getBatteryChargeFromUser();
                    break;
                case eMainMenuOptions.ShowVehicleInfo:
                    this.printAllDetails();
                    break;
                case eMainMenuOptions.ExitProgram:
                    this.programExit();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(i_UserInputSelection), i_UserInputSelection, null);
            }
        }

        public void Start()
        {
            while (m_ProgramExit == false)
            {
                try
                {
                    printMainMenu();
                    Console.Write("Your choice: ");
                    string userChoiseInput = Console.ReadLine();
                    int userInput = parseIntegerToStringValidation(userChoiseInput);
                    mainMenuOptions(userInput);
                }
                catch (FormatException)
                {
                    Screen.Clear();
                    Console.WriteLine("{0}", Environment.NewLine);
                    Console.WriteLine("->[Only numbers are acceptable,please enter number]<-");
                    Console.WriteLine("{0}", Environment.NewLine);
                }
                catch (ValueOutOfRangeException ex)
                {
                    Screen.Clear();
                    Console.WriteLine("{0}", Environment.NewLine);
                    Console.WriteLine("{0}", ex.Message);
                    Console.WriteLine("{0}", Environment.NewLine);
                }
            }
        }



        private Vehicle makeNewVehicle(string i_CarModel, string i_LicenseNumber, string i_WheelManufacturer)
        {
            Vehicle.eVehicleType newVehicleType = this.getVehicleTypeFronUser();
            Engine.eTypeOfEngine typeOfEngine;

            if (newVehicleType == Vehicle.eVehicleType.Truck)
            {
                typeOfEngine = Engine.eTypeOfEngine.Fuel;
            }
            else
            {
                typeOfEngine = getEngineTypeFromUser();
            }

            Vehicle vehicleToAdd = addCarToGarage.AddVehicle(i_CarModel, i_LicenseNumber, i_WheelManufacturer, typeOfEngine, newVehicleType);
            this.getVehicleDetailsByVehicle(vehicleToAdd);

            return vehicleToAdd;
        }

        private void getVehicleDetailsByVehicle(Vehicle i_NewVehicle)
        {
            getCurrentAirPressureFromUser(i_NewVehicle);
            switch (i_NewVehicle)
            {
                case Truck truck:
                    getDangerousMaterialsFromUser(truck);
                    getMaximumCarryWeightFromUser(truck);
                    break;
                case Car car:
                    getNumOfDoorsFromUser(car);
                    getVehicleColorFromUser(car);
                    break;
                case Motorcycle moto:
                    getLicenseTypeFromUser((Motorcycle)i_NewVehicle);
                    getEngineVolumeFromUser((Motorcycle)i_NewVehicle);
                    break;
            }
        }

        private void getCurrentAirPressureFromUser(Vehicle i_Vehicle)
        {
            Screen.Clear();
            int numberOfWheels = 1;
            bool flag = false;

            foreach (Wheel wheel in i_Vehicle.Wheels)
            {
                flag = false;
                while (!flag)
                {
                    try
                    {
                        Console.WriteLine("Enter air pressure for wheel number {0}:", numberOfWheels);
                        bool inputValidationCheck = float.TryParse(Console.ReadLine(), out float airToAdd);
                        if (inputValidationCheck == false)
                        {
                            throw new FormatException();
                        }

                        wheel.AddAirPressure(airToAdd);
                        flag = true;
                        numberOfWheels++;
                    }
                    catch (FormatException)
                    {
                        Screen.Clear();
                        Console.WriteLine("{0}", Environment.NewLine);
                        Console.WriteLine("->[Only numbers are acceptable,please enter number]<-");
                        Console.WriteLine("{0}", Environment.NewLine);
                    }
                    catch (ValueOutOfRangeException ex)
                    {
                        Screen.Clear();
                        Console.WriteLine("{0}", Environment.NewLine);
                        Console.WriteLine("{0}", ex.Message);
                        Console.WriteLine("{0}", Environment.NewLine);
                    }
                }

            }

        }
        private void addNewVehicleToGarage()
        {
            Vehicle newVehicleToAdd;
            string clientName;
            string clientPhone;
            string carModel;
            string wheelManufacture;
            string licenseNumber;


            if (isVehicleInTheGarage(r_Garage, out licenseNumber))
            {
                Screen.Clear();
                Console.WriteLine("[Vehicle #{0} is allready in garage,status changed to 'In repairs']{1}", licenseNumber, Environment.NewLine);
                r_Garage.ChangeVehicleStatus(licenseNumber, Garage.eStatus.InRepairs);
            }
            else
            {
                Screen.Clear();
                Console.WriteLine("Enter client's name");
                clientName = Console.ReadLine();
                Screen.Clear();
                Console.WriteLine("Enter vehicle model");
                carModel = Console.ReadLine();
                Screen.Clear();
                Console.WriteLine("Enter client's phone");
                clientPhone = Console.ReadLine();
                Screen.Clear();
                Console.WriteLine("Please enter the wheel's manufacturer");
                wheelManufacture = Console.ReadLine();

                newVehicleToAdd = this.makeNewVehicle(carModel, licenseNumber, wheelManufacture);
                r_Garage.AddNewVehicleToGarage(newVehicleToAdd, clientName, clientPhone);
            }
        }


        private void getEngineVolumeFromUser(Motorcycle i_Motorcycle)
        {
            bool check = false;
            Screen.Clear();

            while (!check)
            {
                try
                {
                    Console.WriteLine("Enter vehicle's engine volume:");
                    bool inputValidation = int.TryParse(Console.ReadLine(), out int inputInteger);
                    if (inputValidation == false)
                    {
                        throw new FormatException();
                    }

                    i_Motorcycle.EngineVolume = inputInteger;
                    check = true;
                    Screen.Clear();
                }
                catch (FormatException)
                {
                    Screen.Clear();
                    Console.WriteLine("{0}", Environment.NewLine);
                    Console.WriteLine("->[Only numbers are acceptable,please enter number]<-");
                    Console.WriteLine("{0}", Environment.NewLine);
                }
            }
        }


        private void getMaximumCarryWeightFromUser(Truck i_Truck)
        {
            bool check = false;
            Screen.Clear();

            while (!check)
            {
                try
                {
                    Console.WriteLine("Enter maximum carry weight: ");
                    bool inputValid = float.TryParse(Console.ReadLine(), out float inputFromUser);
                    if (inputValid == false)
                    {
                        throw new FormatException();
                    }

                    i_Truck.MaximumCarryWeight = inputFromUser;
                    check = true;
                }
                catch (FormatException)
                {
                    Screen.Clear();
                    Console.WriteLine("{0}", Environment.NewLine);
                    Console.WriteLine("->[Only numbers are acceptable,please enter number]<-");
                    Console.WriteLine("{0}", Environment.NewLine);
                }
            }


        }

        private void getDangerousMaterialsFromUser(Truck i_Truck)
        {
            bool inputFlag = false;
            Screen.Clear();

            while (!inputFlag)
            {
                try
                {
                    Console.WriteLine("Vehicle carrying dangerous material?{0}1) Yes{0}2) No", Environment.NewLine);
                    bool isNumber = int.TryParse(Console.ReadLine(), out int userChoice);
                    if (isNumber == false)
                    {
                        throw new FormatException();
                    }
                    if (userChoice <= 0 || userChoice > 2)
                    {
                        throw new ValueOutOfRangeException(1, 2);
                    }
                    if (userChoice == 1)
                        i_Truck.CarryingDangerousMaterials = true;
                    else
                        i_Truck.CarryingDangerousMaterials = false;

                    inputFlag = true;

                }
                catch (FormatException)
                {
                    Screen.Clear();
                    Console.WriteLine("{0}", Environment.NewLine);
                    Console.WriteLine("->[Only numbers are acceptable,please enter number]<-");
                    Console.WriteLine("{0}", Environment.NewLine);
                }
                catch (ValueOutOfRangeException ex)
                {
                    Screen.Clear();
                    Console.WriteLine("{0}", Environment.NewLine);
                    Console.WriteLine("{0}", ex.Message);
                    Console.WriteLine("{0}", Environment.NewLine);
                }
            }
        }

        private void getVehicleColorFromUser(Car i_Car)

        {
            bool check = false;
            Screen.Clear();

            while (!check)
            {
                try
                {
                    Console.WriteLine("Select vehicle color:{0}1) Red{0}2) Silver{0}3) White{0}4) Black", Environment.NewLine);
                    bool inputValid = int.TryParse(Console.ReadLine(), out int userInput);
                    if (inputValid == false)
                    {
                        throw new FormatException();
                    }
                    if (userInput <= 0 || userInput > 4)
                    {
                        throw new ValueOutOfRangeException(1, 4);
                    }

                    i_Car.CarColor = (Car.eColor)userInput;
                    check = true;
                }
                catch (FormatException)
                {
                    Screen.Clear();
                    Console.WriteLine("{0}", Environment.NewLine);
                    Console.WriteLine("->[Only numbers are acceptable,please enter number]<-");
                    Console.WriteLine("{0}", Environment.NewLine);
                }
                catch (ValueOutOfRangeException ex)
                {
                    Screen.Clear();
                    Console.WriteLine("{0}", Environment.NewLine);
                    Console.WriteLine("{0}", ex.Message);
                    Console.WriteLine("{0}", Environment.NewLine);
                }
            }

            Screen.Clear();
        }
        private void getLicenseTypeFromUser(Motorcycle i_Motorcycle)
        {
            bool check = false;
            Screen.Clear();

            while (!check)
            {
                try
                {
                    Console.WriteLine("Select vehicle's license type:{0}1) A{0}2) B1{0}3) AA{0}4) BB", Environment.NewLine);
                    bool inputValid = int.TryParse(Console.ReadLine(), out int userInputSelection);
                    if (inputValid == false)
                    {
                        throw new FormatException();
                    }
                    if (userInputSelection > 4 || userInputSelection <= 0)
                    {
                        throw new ValueOutOfRangeException(1, 4);
                    }
                    i_Motorcycle.LicenseType = (Motorcycle.eLicenseType)userInputSelection;
                    check = true;
                }
                catch (FormatException)
                {
                    Screen.Clear();
                    Console.WriteLine("{0}", Environment.NewLine);
                    Console.WriteLine("->[Only numbers are acceptable,please enter number]<-");
                    Console.WriteLine("{0}", Environment.NewLine);
                }
                catch (ValueOutOfRangeException ex)
                {
                    Screen.Clear();
                    Console.WriteLine("{0}", Environment.NewLine);
                    Console.WriteLine("{0}", ex.Message);
                    Console.WriteLine("{0}", Environment.NewLine);
                }
            }

        }


        private void getNumOfDoorsFromUser(Car i_Car)
        {
            bool check = false;
            Screen.Clear();

            while (!check)
            {
                try
                {
                    Console.WriteLine("Enter number of doors (2 - 5):");
                    bool inputValid = int.TryParse(Console.ReadLine(), out int userInput);
                    if (inputValid == false)
                    {
                        throw new FormatException();
                    }
                    if (userInput < 2 || userInput > 5)
                    {
                        throw new ValueOutOfRangeException(2, 5);
                    }
                    check = true;
                    i_Car.NumbersOfDoors = (Car.eNumOfDoors)userInput;

                }
                catch (FormatException)
                {
                    Screen.Clear();
                    Console.WriteLine("{0}", Environment.NewLine);
                    Console.WriteLine("->[Only numbers are acceptable,please enter number]<-");
                    Console.WriteLine("{0}", Environment.NewLine);
                }
                catch (ValueOutOfRangeException ex)
                {
                    Screen.Clear();
                    Console.WriteLine("{0}", Environment.NewLine);
                    Console.WriteLine("{0}", ex.Message);
                    Console.WriteLine("{0}", Environment.NewLine);
                }
            }
        }


        private Vehicle.eVehicleType getVehicleTypeFronUser()
        {
            bool check = false;
            Screen.Clear();
            Vehicle.eVehicleType vehicleType = Vehicle.eVehicleType.Car;

            while (!check)
            {
                try
                {
                    Console.WriteLine("Select vehicle type:{0}1) Car{0}2) Truck{0}3) Motorcycle", Environment.NewLine);
                    bool inputValid = int.TryParse(Console.ReadLine(), out int input);
                    if (inputValid == false)
                    {
                        throw new FormatException();
                    }
                    if (input <= 0 || input > 3)
                    {
                        throw new ValueOutOfRangeException(0, 3);
                    }

                    if (input == 1)
                        vehicleType = Vehicle.eVehicleType.Car;
                    else if (input == 2)
                        vehicleType = Vehicle.eVehicleType.Truck;
                    else
                        vehicleType = Vehicle.eVehicleType.Motorcycle;

                    check = true;
                }
                catch (FormatException)
                {
                    Screen.Clear();
                    Console.WriteLine("{0}", Environment.NewLine);
                    Console.WriteLine("->[Only numbers are acceptable,please enter number]<-");
                    Console.WriteLine("{0}", Environment.NewLine);
                }
                catch (ValueOutOfRangeException ex)
                {
                    Screen.Clear();
                    Console.WriteLine("{0}", Environment.NewLine);
                    Console.WriteLine("{0}", ex.Message);
                    Console.WriteLine("{0}", Environment.NewLine);
                }
            }


            return vehicleType;

        }

        private bool isVehicleInTheGarage(Garage i_Garage, out string i_LicenseNumberToCheck)
        {
            Screen.Clear();
            bool isInTheGarage = false;
            Console.WriteLine("Please enter vehicle's license number: ");
            i_LicenseNumberToCheck = Console.ReadLine();
            if (i_Garage.IsExistsInGarage(i_LicenseNumberToCheck) != null)
            {
                isInTheGarage = true;
            }

            return isInTheGarage;
        }

        private void printVehicleGroupByStatus()
        {
            List<string> listOfPaidStatus = r_Garage.GetLicensesListByStatus(Garage.eStatus.Paid);
            List<string> listOfInRepairsStatus = r_Garage.GetLicensesListByStatus(Garage.eStatus.InRepairs);
            List<string> listOfRepairsStatus = r_Garage.GetLicensesListByStatus(Garage.eStatus.Repaired);
            bool check = false;

            Screen.Clear();

            while (!check)
            {
                try
                {
                    Console.WriteLine("Please select status:{0}1)Cars in repairs{0}2)Repaired{0}3)Paid ", Environment.NewLine);
                    bool isInputValid = int.TryParse(Console.ReadLine(), out int UserSelection);
                    if (isInputValid == false)
                    {
                        throw new FormatException();
                    }
                    if (UserSelection <= 0 || UserSelection > 3)
                    {
                        throw new ValueOutOfRangeException(1, 3);
                    }

                    check = true;

                    if (UserSelection == 1)
                    {
                        Screen.Clear();
                        Console.WriteLine("Vehicles with 'In repairs' stutus:");
                        foreach (string vehicleLicenseNumber in listOfInRepairsStatus)
                        {
                            Console.WriteLine("License number - {0}", vehicleLicenseNumber);
                        }
                    }
                    else if (UserSelection == 2)
                    {
                        Screen.Clear();
                        Console.WriteLine("Vehicles with 'Repaired' stutus: ");
                        foreach (string vehicleLicenseNumber in listOfRepairsStatus)
                        {
                            Console.WriteLine("License number - {0}", vehicleLicenseNumber);
                        }
                    }
                    else
                    {
                        Screen.Clear();
                        Console.WriteLine("Vehicles with 'Paid' stutus: ");
                        foreach (string vehicleLicenseNumber in listOfPaidStatus)
                        {
                            Console.WriteLine("License number - {0}", vehicleLicenseNumber);
                        }

                    }
                }
                catch (FormatException)
                {
                    Screen.Clear();
                    Console.WriteLine("{0}", Environment.NewLine);
                    Console.WriteLine("->[Only numbers are acceptable,please enter number]<-");
                    Console.WriteLine("{0}", Environment.NewLine);
                }
                catch (ValueOutOfRangeException ex)
                {
                    Screen.Clear();
                    Console.WriteLine("{0}", Environment.NewLine);
                    Console.WriteLine("{0}", ex.Message);
                    Console.WriteLine("{0}", Environment.NewLine);
                }
            }
        }

        private Engine.eTypeOfEngine getEngineTypeFromUser()
        {
            bool inputFlag = false;
            Screen.Clear();
            Engine.eTypeOfEngine engineType = Engine.eTypeOfEngine.Fuel;

            while (!inputFlag)
            {
                try
                {
                    Console.WriteLine("Select engine type :{0}1) Fuel engine{0}2) Electric engine", Environment.NewLine);
                    bool inputValid = int.TryParse(Console.ReadLine(), out int UserInput);
                    if (inputValid == false)
                    {
                        throw new FormatException();
                    }
                    if (UserInput <= 0 || UserInput > 2)
                    {
                        throw new ValueOutOfRangeException(2, 0);
                    }

                    if (UserInput == 2)
                    {
                        engineType = Engine.eTypeOfEngine.Battery;
                    }
                    else
                    {
                        engineType = Engine.eTypeOfEngine.Fuel;
                    }
                    inputFlag = true;

                }
                catch (FormatException)
                {
                    Screen.Clear();
                    Console.WriteLine("{0}", Environment.NewLine);
                    Console.WriteLine("->[Only numbers are acceptable,please enter number]<-");
                    Console.WriteLine("{0}", Environment.NewLine);
                }
                catch (ValueOutOfRangeException ex)
                {
                    Screen.Clear();
                    Console.WriteLine("{0}", Environment.NewLine);
                    Console.WriteLine("{0}", ex.Message);
                    Console.WriteLine("{0}", Environment.NewLine);
                }
            }

            return engineType;
        }

        private void getStatusChangeFromUser()
        {
            Screen.Clear();
            Console.WriteLine("Enter vehicle's license number:");
            string vehiclesLicenseNumberToChange = Console.ReadLine();
            Console.WriteLine("Please select the new status:{0}1) In repairs {0}2) Repaired {0}3) Paid", Environment.NewLine);
            Garage.eStatus newVehicleStatus = Garage.eStatus.InRepairs;
            int newStatusSelection = int.Parse(Console.ReadLine());
            try
            {
                if (newStatusSelection == 1)
                {
                    newVehicleStatus = Garage.eStatus.InRepairs;

                }
                else if (newStatusSelection == 2)
                {
                    newVehicleStatus = Garage.eStatus.Repaired;
                }
                else if (newStatusSelection == 3)
                {

                    newVehicleStatus = Garage.eStatus.Paid;
                }
                else
                {
                    throw new ArgumentException();
                }

                r_Garage.ChangeVehicleStatus(vehiclesLicenseNumberToChange, newVehicleStatus);
                Screen.Clear();
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
                getStatusChangeFromUser();
            }
            catch (MissingVehicleException ex)
            {
                Screen.Clear();
                Console.WriteLine("{0}", Environment.NewLine);
                Console.WriteLine("{0}", ex.Message);
                Console.WriteLine("{0}", Environment.NewLine);
            }
        }

        private void getNewAirPressureFromUser()
        {
            Screen.Clear();
            Console.WriteLine("Enter vehicle's license number:");
            string licenseNumber = Console.ReadLine();

            try
            {
                r_Garage.AddAirPressure(licenseNumber);
            }
            catch (MissingVehicleException ex)
            {
                Screen.Clear();
                Console.WriteLine("{0}", Environment.NewLine);
                Console.WriteLine("{0}", ex.Message);
                Console.WriteLine("{0}", Environment.NewLine);

            }

        }


        private Fuel.eFuelType getFuelTypeFromUser()
        {
            Console.WriteLine("Select vehicle's fuel type:{0}1) Octan98 {0}2) Octan96 {0}3) Octan95{0}4) Soler", Environment.NewLine);
            bool inputValid = int.TryParse(Console.ReadLine(), out int userSelection);

            if (inputValid == false)
            {
                throw new FormatException();
            }

            if (userSelection < 1 || userSelection > 4)
            {
                throw new ValueOutOfRangeException(4, 1);
            }
            Fuel.eFuelType fuelType = Fuel.eFuelType.Octan95;
            switch (userSelection)
            {
                case 1:
                    fuelType = Fuel.eFuelType.Octan98;
                    break;
                case 2:
                    fuelType = Fuel.eFuelType.Octan96;
                    break;
                case 3:
                    fuelType = Fuel.eFuelType.Octan95;
                    break;
                case 4:
                    fuelType = Fuel.eFuelType.Soler;
                    break;
            }
            return fuelType;

        }
        private void getBatteryChargeFromUser()
        {
            float hoursToCharge = 0;
            Screen.Clear();
            Console.WriteLine("Enter vehicle's license number:");
            string licenseNumberToAdd = Console.ReadLine();

            try
            {
                Console.WriteLine("Enter how many charge hours to add: ");
                bool inpuValid = float.TryParse(Console.ReadLine(), out hoursToCharge);
                if (inpuValid == false)
                {
                    throw new FormatException();
                }
                r_Garage.ChargeVehicle(licenseNumberToAdd, hoursToCharge);
                Screen.Clear();
                Console.WriteLine("{0} hours Charged successfully to Vehicle #{1}", hoursToCharge.ToString(), licenseNumberToAdd, hoursToCharge);

            }
            catch (FormatException)
            {
                Screen.Clear();
                Console.WriteLine("{0}", Environment.NewLine);
                Console.WriteLine("->[Only numbers are acceptable,please enter number]<-");
                Console.WriteLine("{0}", Environment.NewLine);

            }
            catch (MissingVehicleException ex)
            {
                Screen.Clear();
                Console.WriteLine("{0}", Environment.NewLine);
                Console.WriteLine("{0}", ex.Message);
                Console.WriteLine("{0}", Environment.NewLine);

            }
            catch (EngineTypeException ex)
            {
                Screen.Clear();
                Console.WriteLine("{0}", Environment.NewLine);
                Console.WriteLine("{0}", ex.Message);
                Console.WriteLine("{0}", Environment.NewLine);

            }



        }

        private void printAllDetails()
        {
            Screen.Clear();
            try
            {
                Console.WriteLine("Please enter vehicle's license number:");
                string licenseNumber = Console.ReadLine();

                Console.WriteLine(r_Garage.GetVehicleDetails(licenseNumber));
            }
            catch (MissingVehicleException ex)
            {
                Screen.Clear();
                Console.WriteLine("{0}", Environment.NewLine);
                Console.WriteLine(ex.Message);
            }

        }

        private void programExit()
        {
            m_ProgramExit = true;
        }

        private void getNewFuelRateFromUser()
        {
            float fuelToAdd = 0;
            Screen.Clear();
            Console.WriteLine("Enter vehicle's license number:");
            string licenseNumberToAddFuel = Console.ReadLine();
            Fuel.eFuelType fuelType = getFuelTypeFromUser();
            Console.WriteLine("Please enter amount of fuel to add:");
            bool inputValid = float.TryParse(Console.ReadLine(), out fuelToAdd);
            if (inputValid == false)
            {
                throw new FormatException();
            }

            try
            {
                r_Garage.AddFuel(licenseNumberToAddFuel, fuelType, fuelToAdd);
                Screen.Clear();
                Console.WriteLine("{2} {0} Added successfully to Vehicle #{1} ", fuelType.ToString(), licenseNumberToAddFuel, fuelToAdd);

            }
            catch (MissingVehicleException ex)
            {
                Screen.Clear();
                Console.WriteLine("{0}", Environment.NewLine);
                Console.WriteLine("{0}", ex.Message);
                Console.WriteLine("{0}", Environment.NewLine);

            }
            catch (EngineTypeException ex)
            {
                Screen.Clear();
                Console.WriteLine("{0}", Environment.NewLine);
                Console.WriteLine("{0}", ex.Message);
                Console.WriteLine("{0}", Environment.NewLine);

            }
            catch (FuelTypeException ex)
            {
                Screen.Clear();
                Console.WriteLine("{0}", Environment.NewLine);
                Console.WriteLine("{0}", ex.Message);
                Console.WriteLine("{0}", Environment.NewLine);

            }



        }

    }
}
