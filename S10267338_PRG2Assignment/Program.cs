//==========================================================
// Student Name : Wyse Lee Hong Yao (S10268134D)
// Student Name : Tang Kah Yan (S10267338C)
//==========================================================


using S10267338_PRG2Assignment;
using System;
using System.Globalization;
using System.Transactions;

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

    int option = Convert.ToInt32(Console.ReadLine());

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
        Console.Write("Enter Flight Number: ");
        string? flightNum = Console.ReadLine();

        Flight flight = terminal.Flights[flightNum];
        BoardingGate? gate = null;

        while (true)
        {
            // Prompt user for boarding gate that flight is going to be assigned to
            Console.Write("Enter Boarding Gate Name: ");
            string? gateName = Console.ReadLine();

            gate = terminal.BoardingGates[gateName];

            // Prompt user to enter another boarding gate if boarding gate entered not valid
            if (gate.Flight != null)
            {
                Console.WriteLine("There is already an assigned flight at this boarding gate, please enter another boarding gate");
                continue;
            }
            break;
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
            // Prompt user to enter flight details to create new flight object
            Console.Write("Enter Flight Mumber: ");
            string? flightNum = Console.ReadLine();

            Console.Write("Enter Origin: ");
            string? origin = Console.ReadLine();

            Console.Write("Enter Destination: ");
            string? destination = Console.ReadLine();

            Console.Write("Enter Expected Departure/Arrival Time (dd/mm/yyyy hh:mm): ");
            DateTime expectedTime = Convert.ToDateTime(Console.ReadLine());

            Console.Write("Enter Special Request Code (CFFT/DDJB/LWTT/None): ");
            string? specialCode = Console.ReadLine();
            //if (specialCode == "None")
            //{
            //    specialCode = "";
            //}

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
                string? choice = Console.ReadLine();

                if (choice == "N")
                {
                    isCreating = false;
                    break;
                }
                else if (choice == "Y")
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Please enter a valid choice");
                    continue;
                }
            }
        }
    }
    // Basic Feature 7: Display full flight details from an airline
    else if (option == 5)
    {
        // Displaying flight details of chosen airline
        Console.WriteLine();
        DisplayChosenAirlineFlightDetails(terminal);

        // Prompt user for flight number
        Console.WriteLine();
        Console.Write("Select a flight number: ");
        string? flightNumber = Console.ReadLine();

        // Retrieve flight object using flight number
        Flight f = terminal.Flights[flightNumber];

        // Retrieve airline object using flight
        Airline a = terminal.GetAirlineFromFlight(f);

        // Retrieve Flight Details (airline name, boarding gate, special request code)
        var flightDetails = RetrieveFlightDetails(flightNumber, f);

        // Display the flight details
        Console.WriteLine();
        Console.WriteLine($"{"Flight Number",-16}{"Airline Name",-23}{"Origin",-23}{"Destination",-23}{"Expected Departure/Arrival Time",-35}{"Special Request Code",-23}Boarding Gate");
        Console.WriteLine($"{f.FlightNumber,-16}{a.Name,-23}{f.Origin,-23}{f.Destination,-23}{f.ExpectedTime,-35}{flightDetails.specialCode,-23}{flightDetails.boardingGate}");
    }
    // Basic Feature 8: Modify flight details
    else if (option == 6)
    {
        // Displaying flight details of chosen airline
        Console.WriteLine();
        DisplayChosenAirlineFlightDetails(terminal);

        // Prompt for flight number
        Console.WriteLine();
        Console.Write("Choose an existing Flight to modify or delete: ");
        string? flightNum = Console.ReadLine();

        // Retrieve the corresponding flight object from the Flight dict
        Flight flight = terminal.Flights[flightNum];

        // Choose to Modify or Delete flight
        Console.WriteLine();
        Console.WriteLine("1. Modify Flight");
        Console.WriteLine("2. Delete Flight");
        Console.WriteLine();
        Console.Write("Choose an option: ");
        int choice = Convert.ToInt32(Console.ReadLine());

        // [1] Modify Flight Selection
        if (choice == 1)
        {
            Console.WriteLine();
            Console.WriteLine("1. Modify Basic Information");
            Console.WriteLine("2. Modify Status");
            Console.WriteLine("3. Modify Special Request Code");
            Console.WriteLine("4. Modify Boarding Gate");
            Console.WriteLine();
            Console.Write("Choose an option: ");
            int modificationOption = Convert.ToInt32(Console.ReadLine());

            // (1.1) Modify Flight Option
            if (modificationOption == 1)
            {
                Console.WriteLine();
                Console.Write("Enter new Origin: ");
                string? origin = Console.ReadLine();

                Console.Write("Enter new Destination: ");
                string? destination = Console.ReadLine();

                Console.Write("Enter new Expected Departure/Arrival Time (dd/mm/yyyy hh:mm): ");
                DateTime expectedTime = Convert.ToDateTime(Console.ReadLine());

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
                Console.WriteLine();
                Console.Write("Enter new Special Request Code (CFFT/DDJB/LWTT/None): ");
                string? specialCode = Console.ReadLine();

                // Change the Type of Flight
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
                Console.WriteLine();
                Console.WriteLine("Special Request Code updated!");
            }

            // (1.4) Modify new Boarding Gate
            else if (modificationOption == 4)
            {
                // Prompt for new Boarding Gate
                Console.WriteLine();
                Console.Write("Enter new Boarding Gate: ");
                string? boardingGate = Console.ReadLine();

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
            string? confirmation = Console.ReadLine();

            if (confirmation == "Y")
            {
                terminal.Flights.Remove(flightNum);
                Console.WriteLine();
                Console.WriteLine("Flight deleted!");
            }

            else if (confirmation == "N")
            {
                continue;
            }

            //Displaying new updated Flight details (all flight specifications)
            Console.WriteLine();
            Console.WriteLine($"{"Flight Number",-16}{"Airline Name",-23}{"Origin",-23}{"Destination",-23}{"Expected Departure/Arrival Time",-35}{"Special Request Code",-23}Boarding Gate");
            foreach (Flight f in terminal.Flights.Values)
            {
                // Retrieve Flight Details (airline name, boarding gate, special request code)
                var flightDetails = RetrieveFlightDetails(flightNum, f);

                Console.WriteLine($"{f.FlightNumber,-16}{terminal.GetAirlineFromFlight(f).Name,-23}{f.Origin,-23}{f.Destination,-23}{f.ExpectedTime,-35}{flightDetails.specialCode,-23}{flightDetails.boardingGate}");
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
                Console.Write($"{"None", -24}");
            }
            else
            {
                Console.Write($"{flightDetails.specialCode,-24}");
            }

            if (flightDetails.boardingGate == "")
            {
                Console.WriteLine("Unassigned");
            }
            else
            {
                Console.WriteLine(flightDetails.boardingGate);
            }
        }
    }
    else if (option == 0)
    {
        Console.WriteLine("Goodbye!");
        break;
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
    Console.Write("=============================================" +
        "\nWelcome to Changi Airport Terminal 5" +
        "\n=============================================" +
        "\n1. List All Flights" +
        "\n2. List Boarding Gates" +
        "\n3. Assign a Boarding Gate to a Flight" +
        "\n4. Create Flight" +
        "\n5. Display Airline Flights" +
        "\n6. Modify Flight Details" +
        "\n7. Display Flight Schedule" +
        "\n0. Exit\n" +
        "\nPlease select your option: ");
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
    // Prompt user if they want to change the status of the flight
    Console.WriteLine("Would you like to update the status of the flight? (Y/N)");
    string? choice = Console.ReadLine();

    if (choice == "Y")
    {
        Console.WriteLine("1. Delayed");
        Console.WriteLine("2. Boarding");
        Console.WriteLine("3. On Time");

        // Prompt user what status they want to set the flight to
        Console.Write("Please select the new status of the flight: ");
        int statusOption = Convert.ToInt32(Console.ReadLine());

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
    }
    else if (choice == "N")
    {
        flight.Status = "On Time";
    }
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
    Console.WriteLine($"{"Airline Code",-16}Airline Name");
    foreach (Airline a in t.Airlines.Values)
    {
        Console.WriteLine($"{a.Code,-16}{a.Name}");
    }
    // Prompt user for 2-letter airline code
    Console.WriteLine();
    Console.Write("Enter 2-letter Airline Code: ");
    string? code = Console.ReadLine();
    Console.WriteLine();
    foreach (Airline a in t.Airlines.Values)
    {
        if (code == a.Code)
        {
            Console.WriteLine("=============================================");
            Console.WriteLine($"List of Flights for {a.Name}");
            Console.WriteLine("=============================================");
            Console.WriteLine("Flight Number   Airline Name           Origin                 Destination            Expected Departure/Arrival Time");
            foreach (Flight f in a.Flights.Values)
            {
                // Retrieving code from Flight object's FlightNumber for comparison
                string fCode = f.FlightNumber.Split(" ")[0];
                if (code == fCode)
                {
                    Console.WriteLine($"{f.FlightNumber,-16}{a.Name,-23}{f.Origin,-23}{f.Destination,-23}{f.ExpectedTime,-31}");
                }
            }
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