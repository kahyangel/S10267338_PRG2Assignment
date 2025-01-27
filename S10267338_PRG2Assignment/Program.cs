//==========================================================
// Student Name : Wyse Lee Hong Yao (S10268134D)
// Student Name : Tang Kah Yan (S10267338C)
//==========================================================


using S10267338_PRG2Assignment;
using System;
using System.Diagnostics.Metrics;
using System.Globalization;
using System.Linq.Expressions;
using System.Text;
using System.Transactions;
using System.Xml.Serialization;

// Create Terminal object
Terminal terminal = new Terminal("Terminal 5");

// Basic Feature 1: Load files (airlines and boarding gates)
LoadAirlines(terminal);
LoadBoardingGate(terminal);

// Basic Feature 2: Load files (flights)
LoadFlights(terminal);

// Display all loaded objects from files
DisplayLoading(terminal);
Console.WriteLine();

// Main loop
while (true)
{
    DisplayMenu();

    // Create list of flights sorted by time
    List<Flight> sortedFlightList = new List<Flight>();
    int? option = null;

    try
    {
        Console.Write("Please select your option: ");
        option = Convert.ToInt32(Console.ReadLine());
    }
    catch (FormatException)
    {
        Console.WriteLine("Please enter a valid input");
        Console.WriteLine();
        continue;
    }

    // Basic Feature 3: List all flights with their basic information 
    if (option == 1)
    {
        Console.WriteLine();
        ListFlights(terminal);
    }

    // Basic Feature 4: List all boarding gates
    else if (option == 2)
    {
        ListBoardingGates(terminal);
    }

    // Basic Feature 5: Assign a boarding gate to a flight
    else if (option == 3)
    {
        // Display header
        Console.WriteLine("=============================================" +
            "\nAssign a Boarding Gate to a Flight" +
            "\n=============================================");

        // Prompt user for flight number of flight to be assigned to a boarding gate
        string? flightNum = null;
        Flight? flight = null;
        while (true)
        {
            try
            {
                Console.Write("Enter Flight Number: ");
                flightNum = Console.ReadLine().ToUpper();

                flight = terminal.Flights[flightNum];
                break;
            }
            catch (KeyNotFoundException)
            {
                Console.WriteLine("Please enter an existing flight number e.g. TR 123");
                Console.WriteLine();
            }
        }

        BoardingGate? gate = null;

        while (true)
        {
            // Prompt user for boarding gate that flight is going to be assigned to
            string? gateName = null;
            try
            {
                Console.Write("Enter Boarding Gate Name: ");
                gateName = Console.ReadLine().ToUpper();

                gate = terminal.BoardingGates[gateName];

                // Prompt user to enter another boarding gate if boarding gate entered not valid
                if (gate.Flight != null)
                {
                    Console.WriteLine("There is already an assigned flight at this boarding gate, please enter another boarding gate");
                    continue;
                }
                break;
            }
            catch (KeyNotFoundException)
            {
                Console.WriteLine("Please enter an existing boarding gate name e.g. A20");
                Console.WriteLine();
            }
        }

        DisplayFlightDetails(flight);
        DisplayBoardingGateDetails(gate);
        UpdateFlightStatus(flight);

        // Set the Flight property in gate object to the flight assigned to the gate
        gate.Flight = flight;

        Console.WriteLine($"Flight {flight.FlightNumber} has been assigned to Boarding Gate {gate.GateName}!");
    }
    // Basic Feature 6: Create a new flight
    else if (option == 4)
    {
        bool isCreating = true;
        while (isCreating)
        {
            string? flightNum = null;
            string? origin = null;
            string? destination = null;
            DateTime expectedTime;
            string? specialCode = null;

            while (true)
            {
                // Prompt user to enter flight details to create new flight object
                Console.Write("Enter Flight Number: ");
                flightNum = Console.ReadLine().ToUpper();

                if (IsValidCreateFlightNumber(flightNum))
                {
                    break;
                }
            }

            while (true)
            {
                Console.Write("Enter Origin: ");
                origin = Console.ReadLine();

                if (!IsValidFormatCountry(origin))
                {
                    Console.WriteLine("Invalid format. Please enter in the format: Country (XXX).");
                    Console.WriteLine();
                    continue;
                }
                break;
            }
            origin = char.ToUpper(origin[0]) + origin.Split(" ")[0].Substring(1).ToLower() + " " + origin.Split(" ")[1].ToUpper();

            while (true)
            {
                Console.Write("Enter Destination: ");
                destination = Console.ReadLine();

                if (!IsValidFormatCountry(destination))
                {
                    Console.WriteLine("Invalid format. Please enter in the format: Country (XXX).");
                    Console.WriteLine();
                    continue;
                }
                break;
            }
            destination = char.ToUpper(destination[0]) + destination.Split(" ")[0].Substring(1).ToLower() + " " + destination.Split(" ")[1].ToUpper();

            while (true)
            { 
                try
                {
                    Console.Write("Enter Expected Departure/Arrival Time (dd/mm/yyyy hh:mm): ");
                    expectedTime = Convert.ToDateTime(Console.ReadLine());
                    break;
                }
                catch (FormatException)
                {
                    Console.WriteLine("Please enter a valid date and time in the mentioned format e.g. 01/01/2025 00:00");
                    Console.WriteLine();
                    continue;
                }
            }

            while (true)
            {                
                Console.Write("Enter Special Request Code (CFFT/DDJB/LWTT/None): ");
                specialCode = Console.ReadLine().ToUpper();

                if (specialCode == "CFFT" || specialCode == "DDJB" || specialCode == "LWTT" || specialCode == "NONE")
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Please enter a valid code that is shown in brackets");
                    Console.WriteLine();
                }
            }

            if (specialCode == "NONE")
            {
                specialCode = "None";
            }

            // Create new flight object
            Flight? newFlight = CreateNewFlight(terminal, flightNum, origin, destination, expectedTime, specialCode);

            // Add the flight object into flight dictionary in terminal
            terminal.Flights[newFlight.FlightNumber] = newFlight;

            // Add flight details into flights.csv
            AppendFlightData(newFlight, specialCode);

            Console.WriteLine($"Flight {newFlight.FlightNumber} has been created successfully!\n");

            while (true)
            {
                // Prompt user if they want to create another flight
                Console.WriteLine("Would you like to create another flight? (Y/N)");
                string? choice = Console.ReadLine().ToUpper();

                if (choice == "N")
                {
                    isCreating = false;
                }
                else if (choice != "Y")
                {
                    Console.WriteLine("Please enter a valid choice");
                    continue;
                }
                break;
            }
        }
    }
    // Basic Feature 7: Display full flight details from an airline
    else if (option == 5)
    {
        // Displaying flight details of chosen airline
        Console.WriteLine();
        DisplayChosenAirlineFlightDetails(terminal);
        string flightNum;
        Flight? f = null;

        // Checking if flight number exists / is valid
        while (true)
        {
            Console.Write("Select a flight number: ");
            flightNum = Console.ReadLine().ToUpper();

            if (IsValidExistingFlightNumber(flightNum))
            {
                try
                {
                    f = terminal.Flights[flightNum];
                    break;
                }
                catch (KeyNotFoundException)
                {
                    Console.WriteLine("Please enter an existing flight code e.g. SQ");
                    Console.WriteLine();
                }
            }
        }

        // Retrieve airline object using flight
        Airline a = terminal.GetAirlineFromFlight(f);

        // Retrieve Flight Details (airline name, boarding gate, special request code)
        var flightDetails = RetrieveFlightDetails(flightNum, f);

        // Display the flight details
        Console.WriteLine();
        Console.WriteLine($"{"Flight Number",-16}{"Airline Name",-23}{"Origin",-23}{"Destination",-23}{"Expected Departure/Arrival Time",-35}{"Special Request Code",-23}Boarding Gate");
        Console.WriteLine($"{f.FlightNumber,-16}{a.Name,-23}{f.Origin,-23}{f.Destination,-23}{f.ExpectedTime,-35}{flightDetails.specialCode,-23}{flightDetails.boardingGateName}");
    }
    // Basic Feature 8: Modify flight details
    else if (option == 6)
    {
        // Displaying flight details of chosen airline & generating list of airline's flight numbers
        Console.WriteLine();

        string? flightNum = null;
        Flight? flight = null;
        // Prompt for flight number
        Console.WriteLine();

        while (true)
        {
            Console.Write("Choose an existing Flight to modify or delete: ");
            flightNum = Console.ReadLine().ToUpper();

            if (IsValidExistingFlightNumber(flightNum))
            {
                try
                {
                    flight = terminal.Flights[flightNum];
                    break;
                }
                catch (KeyNotFoundException)
                {
                    Console.WriteLine("Please enter an existing flight code e.g. SQ");
                    Console.WriteLine();
                }
            }
        }

        // Choose to Modify or Delete flight
        Console.WriteLine();
        Console.WriteLine("1. Modify Flight");
        Console.WriteLine("2. Delete Flight");
        Console.WriteLine();

        int choice;
        while (true)
        {
            try
            {
                Console.Write("Choose an option: ");
                choice = Convert.ToInt32(Console.ReadLine());
                if (choice != 1 && choice != 2)
                {
                    Console.WriteLine("Please enter either option 1 or 2.");
                    continue;
                }
                break;
            }
            catch (FormatException)
            {
                Console.WriteLine("Please enter a valid input.");
                Console.WriteLine();
                continue;
            }
            
        }

        // [1] Modify Flight Selection
        if (choice == 1)
        {
            Console.WriteLine();
            Console.WriteLine("1. Modify Basic Information");
            Console.WriteLine("2. Modify Status");
            Console.WriteLine("3. Modify Special Request Code");
            Console.WriteLine("4. Modify Boarding Gate");
            Console.WriteLine();

            int modificationOption;
            while (true)
            {
                try
                {
                    Console.Write("Choose an option: ");
                    modificationOption = Convert.ToInt32(Console.ReadLine());
                    if (choice != 1 && choice != 2 && choice != 3 && choice != 4)
                    {
                        Console.WriteLine("Please enter a valid option.");
                        continue;
                    }
                    break;
                }
                catch (FormatException)
                {
                    Console.WriteLine("Please enter a valid option.");
                    Console.WriteLine();
                    continue;
                }
            }

            // (1.1) Modify Flight Option
            if (modificationOption == 1)
            {
                string origin;
                while (true)
                {
                    Console.WriteLine();
                    Console.Write("Enter new Origin: ");
                    origin = Console.ReadLine();

                    if (!IsValidFormatCountry(origin))
                    {
                        Console.WriteLine("Invalid format. Please enter in the format: Country (XXX).");
                        Console.WriteLine();
                        continue;
                    }
                    break;
                }

                string destination;
                while (true)
                {
                    Console.Write("Enter new Destination: ");
                    destination = Console.ReadLine();

                    if (!IsValidFormatCountry(destination))
                    {
                        Console.WriteLine("Invalid format. Please enter in the format: Country (XXX).");
                        Console.WriteLine();
                        continue;
                    }
                    break;
                }

                DateTime expectedTime;
                while (true)
                {
                    try
                    {
                        Console.Write("Enter new Expected Departure/Arrival Time (dd/mm/yyyy hh:mm): ");
                        expectedTime = Convert.ToDateTime(Console.ReadLine());
                        break;
                    }
                    catch (FormatException)
                    {
                        Console.WriteLine("Please enter a valid date and time in the mentioned format e.g. 01/01/2025 00:00");
                        Console.WriteLine();
                        continue;
                    }
                }

                foreach (Flight f in terminal.Flights.Values)
                {
                    if (flightNum == f.FlightNumber)
                    {
                        f.Origin = origin;
                        f.Destination = destination;
                        f.ExpectedTime = expectedTime;
                    }
                }
                Console.WriteLine();
                Console.WriteLine("Flight updated!");
            }

            // (1.2) Modify new flight Status
            else if (modificationOption == 2)
            {
                UpdateFlightStatus(flight);
                Console.WriteLine();
                Console.WriteLine("Status updated!");
            }

            // (1.3) Modify new Special Request Code
            else if (modificationOption == 3)
            {
                // Prompt for new Special Request Code
                string specialCode;
                while (true)
                {
                    Console.WriteLine();
                    Console.Write("Enter new Special Request Code (CFFT/DDJB/LWTT/None): ");
                    specialCode = Console.ReadLine().ToUpper();

                    if (specialCode == "CFFT" || specialCode == "DDJB" || specialCode == "LWTT" || specialCode == "NONE")
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Please enter a valid code that is shown in brackets");
                        Console.WriteLine();
                    }
                }
                if (specialCode == "NONE")
                {
                    specialCode = "None";
                }

                // Change the Type of Flight
                terminal.Flights.Remove(flight.FlightNumber);
                terminal.Airlines[terminal.GetAirlineFromFlight(flight).Code].RemoveFlight(flight);
                if (specialCode == "CFFT")
                {
                    flight = new CFFTFlight(flight.FlightNumber, flight.Origin, flight.Destination, flight.ExpectedTime);
                }
                else if (specialCode == "DDJB")
                {
                    flight = new DDJBFlight(flight.FlightNumber, flight.Origin, flight.Destination, flight.ExpectedTime);
                }
                else if (specialCode == "LWTT")
                {
                    flight = new LWTTFlight(flight.FlightNumber, flight.Origin, flight.Destination, flight.ExpectedTime);
                }

                else if (specialCode == "None")
                {
                    flight = new NORMFlight(flight.FlightNumber, flight.Origin, flight.Destination, flight.ExpectedTime);
                }
                terminal.Flights[flight.FlightNumber] = flight;
                terminal.Airlines[terminal.GetAirlineFromFlight(flight).Code].RemoveFlight(flight);
                Console.WriteLine();
                Console.WriteLine("Special Request Code updated!");
            }

            // (1.4) Modify new Boarding Gate
            else if (modificationOption == 4)
            {
                string boardingGate;
                // Prompt for new Boarding Gate
                while (true)
                {
                    try
                    {
                        Console.WriteLine();
                        Console.Write("Enter new Boarding Gate: ");
                        boardingGate = Console.ReadLine().ToUpper();
                        BoardingGate bGate = terminal.BoardingGates[boardingGate];
                        break;
                    }
                    catch (KeyNotFoundException)
                    {
                        Console.WriteLine("Please enter a valid boarding gate.");
                        Console.WriteLine();
                    }
                }

                // Update the Flight Property in the BoardingGate object 
                foreach (BoardingGate g in terminal.BoardingGates.Values)
                {
                    if (g.GateName == boardingGate)
                    {
                        g.Flight = flight;
                    }
                }
                Console.WriteLine();
                Console.WriteLine("Boarding Gate updated!");
            }

            //(1.5) End Display of Flight Details
            //Basic Info: Origin + Destination + ExpectedTime
            Console.WriteLine();
            DisplayFlightDetails(flight);

            // Display flight's status
            Console.WriteLine($"Status: {flight.Status}");


            //Boarding Gate - (Assigned / Unassigned)
            bool flightFound = false;
            foreach (BoardingGate g in terminal.BoardingGates.Values)
            {
                if (g.Flight == flight)
                {
                    Console.WriteLine($"Boarding Gate: Assigned");
                    flightFound = true;
                    break;
                }
            }
            if (flightFound == false)
            {
                Console.WriteLine($"Boarding Gate: Unassigned");
            }
        }

        // [2] Delete Flight Selection
        else if (choice == 2)
        {
            // Confirmation
            Console.WriteLine();
            Console.Write("Are You Sure? (Y/N): ");
            string? confirmation = Console.ReadLine().ToUpper();

            if (confirmation == "Y")
            {
                terminal.Flights.Remove(flightNum);
                Airline airline = terminal.GetAirlineFromFlight(flight);
                terminal.Airlines[airline.Code].RemoveFlight(flight);
                Console.WriteLine();
                Console.WriteLine("Flight deleted!");
            }

            else if (confirmation != "N")
            {
                Console.WriteLine("Please enter a valid choice.");
                Console.WriteLine();
                continue;
            }


            //Displaying new updated Flight details (all flight specifications)
            Console.WriteLine();
            Console.WriteLine($"{"Flight Number",-16}{"Airline Name",-23}{"Origin",-23}{"Destination",-23}{"Expected Departure/Arrival Time",-35}{"Special Request Code",-23}Boarding Gate");
            foreach (Flight f in terminal.Flights.Values)
            {
                // Retrieve Flight Details (airline name, boarding gate, special request code)
                var flightDetails = RetrieveFlightDetails(flightNum, f);

                Console.WriteLine($"{f.FlightNumber,-16}{terminal.GetAirlineFromFlight(f).Name,-23}{f.Origin,-23}{f.Destination,-23}{f.ExpectedTime,-35}{flightDetails.specialCode,-23}{flightDetails.boardingGateName}");
            }
        }
    }
    // Basic Feature 9: Display scheduled flights in chronological order, with boarding gates assignments where applicable
    else if (option == 7)
    {
        // Display header
        Console.WriteLine($"{"Flight Number",-16}{"Airline Name",-21}{"Origin",-21}{"Destination",-19}{"Departure/Arrival Time",-24}{"Status",-12}{"Special Request Code",-24}Boarding Gate");

        // Create a list to store flight objects and sort by date/time
        foreach (Flight f in terminal.Flights.Values)
        {
            sortedFlightList.Add(f);
        }
        sortedFlightList.Sort();

        // Display flight details
        foreach (Flight f in sortedFlightList)
        {
            // Retrieve Flight Details (airline name, boarding gate, special request code)
            var flightDetails = RetrieveFlightDetails(f.FlightNumber, f);

            Console.Write($"{f.FlightNumber,-16}{terminal.GetAirlineFromFlight(f).Name,-21}{f.Origin,-21}{f.Destination,-19}{f.ExpectedTime,-24}{f.Status,-12}");
            if (flightDetails.specialCode == "")
            {
                Console.Write($"{"None",-24}");
            }
            else
            {
                Console.Write($"{flightDetails.specialCode,-24}");
            }

            if (flightDetails.boardingGateName == "")
            {
                Console.WriteLine("Unassigned");
            }
            else
            {
                Console.WriteLine(flightDetails.boardingGateName);
            }
        }
    }
    // Exits the program
    else if (option == 0)
    {
        Console.WriteLine("Goodbye!");
        break;
    }
    else
    {
        Console.WriteLine("Please enter a number from 0 to 9");
    }
    Console.WriteLine();
}


// Methods:
void LoadAirlines(Terminal t)
{
    using (StreamReader sr = new StreamReader("airlines.csv"))
    {
        // Reads the header of airlines.csv
        string? s = sr.ReadLine();

        // Reads the contents of airlines.csv and creates airlines objects
        while ((s = sr.ReadLine()) != null)
        {
            // Store each line from csv file into an array
            string[] airlineDetails = s.Split(",");

            string airlineName = airlineDetails[0];
            string airlineCode = airlineDetails[1];

            // Create airline object
            Airline airline = new Airline(airlineName, airlineCode);

            // Add airline object into airline dictionary as the value, with airline code as key
            t.AddAirline(airline);
        }
    }
}

void LoadBoardingGate(Terminal t)
{
    using (StreamReader sr = new StreamReader("boardinggates.csv"))
    {
        // Reads the header of boardinggates.csv
        string? s = sr.ReadLine();

        // Reads the contents of boardinggates.csv and creates boarding gate objects
        while ((s = sr.ReadLine()) != null)
        {
            // Store each line from csv file into an array
            string[] bgDetails = s.Split(",");

            string bgName = bgDetails[0];
            bool needDDJB = Convert.ToBoolean(bgDetails[1]);
            bool needCFFT = Convert.ToBoolean(bgDetails[2]);
            bool needLWTT = Convert.ToBoolean(bgDetails[3]);

            // Create boarding gate object
            BoardingGate boardingGate = new BoardingGate(bgName, needDDJB, needCFFT, needLWTT);

            // Add boarding gate object into boarding gate dictionary as the value, with boarding gate name as key
            t.AddBoardingGate(boardingGate);
        }
    }
}

void LoadFlights(Terminal t)
{
    using (StreamReader sr = new StreamReader("flights.csv"))
    {
        // Reads the header of flights.csv
        string? s = sr.ReadLine();

        // Reads the contents of flights.csv and creates flight objects
        while ((s = sr.ReadLine()) != null)
        {
            // Store each line from csv file into an array
            string[] flightDetails = s.Split(",");

            string flightNum = flightDetails[0];
            string origin = flightDetails[1];
            string destination = flightDetails[2];
            DateTime flightTime = Convert.ToDateTime(flightDetails[3]);
            string specialCode = flightDetails[4];

            // Create flight object
            Flight? newFlight = CreateNewFlight(t, flightNum, origin, destination, flightTime, specialCode);

            // Add flight object into flight dictionary as the value, with flight number as key
            t.Flights[flightNum] = newFlight;

            // Add flight object into airline's flight dictionary as the value, with flight number as key
            Airline matchAirline = t.GetAirlineFromFlight(newFlight);
            matchAirline.AddFlight(newFlight);
        }
    }
}

void DisplayLoading(Terminal t)
{
    // Displays number of airlines, boarding gates, and flights loaded
    Console.WriteLine("Loading Airlines..." +
        $"\n{t.Airlines.Count} Airlines Loaded!" +
        "\nLoading Boarding Gates..." +
        $"\n{t.BoardingGates.Count} Boarding Gates Loaded!" +
        "\nLoading Flights..." +
        $"\n{t.Flights.Count} Flights Loaded!");
}

void DisplayMenu()
{
    // Displays the menu options
    Console.WriteLine("=============================================" +
        "\nWelcome to Changi Airport Terminal 5" +
        "\n=============================================" +
        "\n1. List All Flights" +
        "\n2. List Boarding Gates" +
        "\n3. Assign a Boarding Gate to a Flight" +
        "\n4. Create Flight" +
        "\n5. Display Airline Flights" +
        "\n6. Modify Flight Details" +
        "\n7. Display Flight Schedule" +
        "\n0. Exit\n");
}

void ListFlights(Terminal t)
{
    // Displays the header
    Console.WriteLine("=============================================" +
        "\nList of Flights for Changi Airport Terminal 5" +
        "\n=============================================" +
        "\nFlight Number   Airline Name           Origin                 Destination            Expected  Departure/Arrival Time");

    // Displays the basic information of all flights
    foreach (Flight f in t.Flights.Values)
    {
        Console.WriteLine($"{f.FlightNumber,-16}{t.GetAirlineFromFlight(f).Name,-23}{f.Origin,-23}{f.Destination,-23}{f.ExpectedTime,-31}");
    }
}

void ListBoardingGates(Terminal t)
{
    // Displays the header
    Console.WriteLine("=============================================");
    Console.WriteLine("List of Boarding Gates for Changi Airport Terminal 5");
    Console.WriteLine("=============================================");
    Console.WriteLine($"{"Gate Name",-16}{"DDJB",-23}{"CFFT",-23}{"LWTT", -23}Flight");

    // Displays the information of all boarding gates
    foreach (BoardingGate bg in t.BoardingGates.Values)
    {
        Console.Write($"{bg.GateName,-16}{bg.SupportsDDJB,-23}{bg.SupportsCFFT,-23}{bg.SupportsLWTT,-23}");

        // Displays "None" if boarding gate is not assigned a flight, otherwise display flight number of the assigned flight
        if (bg.Flight == null)
        {
            Console.WriteLine("None");
        }
        else
        {
            Console.WriteLine(bg.Flight.FlightNumber);
        }
    }
}

void DisplayFlightDetails(Flight flight)
{
    // Display flight information and special code (if any)
    Console.WriteLine($"Flight Number: {flight.FlightNumber}");
    Console.WriteLine($"Origin: {flight.Origin}");
    Console.WriteLine($"Destination: {flight.Destination}");
    Console.WriteLine($"Expected Time: {flight.ExpectedTime}");

    if (flight is CFFTFlight)
    {
        Console.WriteLine($"Special Request Code: CFFT");
    }
    else if (flight is DDJBFlight)
    {
        Console.WriteLine($"Special Request Code: DDJB");
    }
    else if (flight is LWTTFlight)
    {
        Console.WriteLine($"Special Request Code: LWTT");
    }
    else if (flight is NORMFlight)
    {
        Console.WriteLine($"Special Request Code: None");
    }
}

void DisplayBoardingGateDetails(BoardingGate gate)
{
    // Display gate name and what special request code it supports
    Console.WriteLine("Boarding Gate Name: " + gate.GateName);
    Console.WriteLine("Supports DDJB: " + gate.SupportsDDJB);
    Console.WriteLine("Supports CFFT: " + gate.SupportsCFFT);
    Console.WriteLine("Supports LWTT: " + gate.SupportsLWTT);
}

void UpdateFlightStatus(Flight flight)
{
    bool notUpdated = true;
    // Prompt user if they want to change the status of the flight
    while (notUpdated)
    {
        Console.WriteLine("Would you like to update the status of the flight? (Y/N)");
        string? choice = Console.ReadLine().ToUpper();

        if (choice == "Y")
        {
            Console.WriteLine("1. Delayed");
            Console.WriteLine("2. Boarding");
            Console.WriteLine("3. On Time");
            while (true)
            {
                int? statusOption = null;
                try
                {
                    // Prompt user what status they want to set the flight to
                    Console.Write("Please select the new status of the flight (1-3): ");
                    statusOption = Convert.ToInt32(Console.ReadLine());

                    if (statusOption == 1)
                    {
                        flight.Status = "Delayed";
                    }
                    else if (statusOption == 2)
                    {
                        flight.Status = "Boarding";
                    }
                    else if (statusOption == 3)
                    {
                        flight.Status = "On Time";
                    }
                    else
                    {
                        Console.WriteLine("Please enter a number from 1 to 3");
                        continue;
                    }

                    notUpdated = false;
                    break;
                }
                catch (FormatException)
                {
                    Console.WriteLine("Please enter a valid input");
                }
            }
        }
        else if (choice == "N")
        {
            flight.Status = "On Time";
            notUpdated = false;
        }
        else
        {
            Console.WriteLine("Enter a valid choice");
            Console.WriteLine();
        }
    }
}

bool IsValidCreateFlightNumber(string flightNum)
{
    string[] fNumArray = flightNum.Split(' ');
    string fCode = fNumArray[0];
    int? fEndNum = null;
    try { fEndNum = Convert.ToInt32(fNumArray[1]); }
    catch (FormatException)
    {
        Console.WriteLine("Please enter a 3 digit number after the flight code");
        Console.WriteLine();
        return false;
    }
    catch (IndexOutOfRangeException)
    {
        Console.WriteLine("Please enter a valid flight number e.g. SQ 999");
        Console.WriteLine();
        return false;
    }

    if (fNumArray.Count() != 2 || fEndNum < 100 || fEndNum > 999)
    {
        Console.WriteLine("Please enter a valid flight number e.g. SQ 999");
        Console.WriteLine();
        return false;
    }
    else if (!terminal.Airlines.ContainsKey(fCode))
    {
        Console.WriteLine("Please enter a valid flight code e.g. SQ");
        Console.WriteLine();
        return false;
    }
    else if (terminal.Flights.ContainsKey(flightNum))
    {
        ListFlights(terminal);
        Console.WriteLine("This flight number already exists, please enter a new flight number that is not the same as the above flights");
        Console.WriteLine();
        return false;
    }
    return true;
}

bool IsValidExistingFlightNumber(string flightNum)
{
    string[] fNumArray = flightNum.Split(' ');
    string fCode = fNumArray[0];
    int? fEndNum = null;
    try { fEndNum = Convert.ToInt32(fNumArray[1]); }
    catch (FormatException)
    {
        Console.WriteLine("Please enter a 3 digit number after the flight code");
        Console.WriteLine();
        return false;
    }
    catch (IndexOutOfRangeException)
    {
        Console.WriteLine("Please enter a valid flight number e.g. SQ 999");
        Console.WriteLine();
        return false;
    }

    if (fNumArray.Count() != 2 || fEndNum < 100 || fEndNum > 999)
    {
        Console.WriteLine("Please enter a valid flight number e.g. SQ 999");
        Console.WriteLine();
        return false;
    }
    else if (!terminal.Airlines.ContainsKey(fCode))
    {
        Console.WriteLine("Please enter a valid flight code e.g. SQ");
        Console.WriteLine();
        return false;
    }
    else if (!terminal.Flights.ContainsKey(flightNum))
    {
        Console.WriteLine("Please enter an existing flight number");
        Console.WriteLine();
        return false;
    }
    return true;
}

bool IsValidFormatCountry(string country)
{
    // Check if input is null or empty
    if (string.IsNullOrEmpty(country))
    {
        return false;
    }

    // Check if input contains a space and parentheses
    int openBracketIndex = country.IndexOf('(');
    int closeBracketIndex = country.IndexOf(')');

    // Validate the format
    if (openBracketIndex > 0 && closeBracketIndex == country.Length - 1 && closeBracketIndex > openBracketIndex + 1)
    {
        string countryName = country.Substring(0, openBracketIndex).Trim();
        countryName = char.ToUpper(countryName[0]) + countryName.Substring(1).ToLower();

        string countryCode = country.Substring(openBracketIndex + 1, closeBracketIndex - openBracketIndex - 1).ToUpper();

        // Ensure country name is not empty and country code is exactly 3 uppercase letters
        if (!string.IsNullOrWhiteSpace(countryName) && countryCode.Length == 3)
        {
            return true;
        }
    }

    return false;
}

Flight? CreateNewFlight(Terminal t, string flightNum, string origin, string destination, DateTime flightTime, string specialCode)
{
    // Creates new flight using flight details
    Flight? newFlight = null;

    if (specialCode == "DDJB")
    {
        newFlight = new DDJBFlight(flightNum, origin, destination, flightTime);
    }
    else if (specialCode == "CFFT")
    {
        newFlight = new CFFTFlight(flightNum, origin, destination, flightTime);
    }
    else if (specialCode == "LWTT")
    {
        newFlight = new LWTTFlight(flightNum, origin, destination, flightTime);
    }
    else if (specialCode == "None" || specialCode == "")
    {
        newFlight = new NORMFlight(flightNum, origin, destination, flightTime);
    }

    return newFlight;
}

void AppendFlightData(Flight flight, string specialCode)
{
    // Adds flight details into flights.csv
    using (StreamWriter sw = new StreamWriter("flights.csv", true))
    {
        string data = $"{flight.FlightNumber},{flight.Origin},{flight.Destination},{flight.ExpectedTime.ToString("h:mm tt")},{specialCode}";
        sw.WriteLine(data);
    }
}

void DisplayChosenAirlineFlightDetails(Terminal t)
{
    string? code = null;
    Airline? airline = null;

    Console.WriteLine($"{"Airline Code",-16}Airline Name");
    foreach (Airline a in t.Airlines.Values)
    {
        Console.WriteLine($"{a.Code,-16}{a.Name}");
    }

    // Prompt user for 2-letter airline code
    Console.WriteLine();

    while (true)
    {
        try
        {
            Console.Write("Enter 2-letter Airline Code: ");
            code = Console.ReadLine().ToUpper();

            airline = t.Airlines[code];
            break;
        }
        catch (KeyNotFoundException)
        {
            Console.WriteLine("Please enter an airline code e.g. SQ");
            Console.WriteLine();
        }
    }

    Console.WriteLine();

    Console.WriteLine("=============================================");
    Console.WriteLine($"List of Flights for {airline.Name}");
    Console.WriteLine("=============================================");
    Console.WriteLine("Flight Number   Airline Name           Origin                 Destination            Expected Departure/Arrival Time");

    foreach (Flight f in airline.Flights.Values)
    {
        // Retrieving code from Flight object's FlightNumber for comparison
        string fCode = f.FlightNumber.Split(" ")[0];
        if (code == fCode)
        {
            Console.WriteLine($"{f.FlightNumber,-16}{airline.Name,-23}{f.Origin,-23}{f.Destination,-23}{f.ExpectedTime,-31}");
        }
    }
}

(string boardingGateName, string specialCode) RetrieveFlightDetails(string flightNumber, Flight f)
{
    // Retrieving the special code request
    string specialCode = "";
    if (f is CFFTFlight)
    {
        specialCode = "CFFT";
    }
    else if (f is DDJBFlight)
    {
        specialCode = "DDJB";
    }
    else if (f is LWTTFlight)
    {
        specialCode = "LWTT";
    }

    // Retrieving the boarding gate name
    string boardingGateName = "";
    foreach (BoardingGate gate in terminal.BoardingGates.Values)
    {
        if (f == gate.Flight)
        {
            boardingGateName = gate.GateName;
        }
    }

    return (boardingGateName, specialCode);
}