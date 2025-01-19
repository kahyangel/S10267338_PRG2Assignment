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

LoadAirlines(terminal);
LoadBoardingGate(terminal);
LoadFlights(terminal);
DisplayLoading(terminal);

// Main loop
while (true)
{
    DisplayMenu();
    
    int option = Convert.ToInt32(Console.ReadLine());
    if (option == 1)
    {
        Console.WriteLine();
        ListFlights(terminal);
    }
    else if (option == 2)
    {
        ListBoardingGates(terminal);
    }
    else if (option == 3)
    {
        Console.WriteLine("=============================================" +
            "\nAssign a Boarding Gate to a Flight" +
            "\n=============================================");

        Console.Write("Enter Flight Number: ");
        string? flightNum = Console.ReadLine();

        Flight flight = terminal.Flights[flightNum];
        BoardingGate? gate = null;

        while (true)
        {
            Console.Write("Enter Boarding Gate Name: ");
            string? gateName = Console.ReadLine();

            gate = terminal.BoardingGates[gateName];

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

        gate.Flight = flight;
        Console.WriteLine($"Flight {flight.FlightNumber} has been assigned to Boarding Gate {gate.GateName}!");
    }
    else if (option == 4)
    {
        bool isCreating = true;
        while (isCreating)
        {
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
            if (specialCode == "None")
            {
                specialCode = "";
            }

            Flight? newFlight = CreateNewFlight(terminal, flightNum, origin, destination, expectedTime, specialCode);

            terminal.Flights[newFlight.FlightNumber] = newFlight;
            AppendFlightData(newFlight, specialCode);
            Console.WriteLine($"Flight {newFlight.FlightNumber} has been created successfully!\n");

            while (true)
            {
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
    else if (option == 5)
    {
        // Displaying flight details of chosen airline
        Console.WriteLine();
        DisplayChosenAirlineFlightDetails(terminal);

        // Prompt user for flight number
        Console.WriteLine();
        Console.Write("Select a flight number: ");
        string? flightNumber = Console.ReadLine();

        foreach (Flight f in terminal.Flights.Values)
        {
            if (flightNumber == f.FlightNumber)
            {
                // Retrieve Flight Details (airline name, boarding gate, special request code)
                var flightDetails = RetrieveFlightDetails(flightNumber, f);

                // Display the flight details
                Console.WriteLine();
                Console.WriteLine($"{"Flight Number",-16}{"Airline Name",-23}{"Origin",-23}{"Destination",-23}{"Expected Departure/Arrival Time",-35}{"Special Request Code",-23}Boarding Gate");
                Console.WriteLine($"{f.FlightNumber,-16}{flightDetails.airlineName,-23}{f.Origin,-23}{f.Destination,-23}{f.ExpectedTime,-35}{flightDetails.specialCode,-23}{flightDetails.boardingGate}");
            }
        }
    }
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

            //Status - (Shceduled / Unscheduled)
            if (flight.Status != null)
            {
                Console.WriteLine("Status: Scheduled");
            }
            else
            {
                Console.WriteLine("Status: Unscheduled");
            }

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
        if (choice == 2)
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

                Console.WriteLine($"{f.FlightNumber,-16}{flightDetails.airlineName,-23}{f.Origin,-23}{f.Destination,-23}{f.ExpectedTime,-35}{flightDetails.specialCode,-23}{flightDetails.boardingGate}");
            }
        }
    }
    else if (option == 7)
    {

    }
    else if (option == 0)
    {
        Console.WriteLine("Goodbye!");
        break;
    }

    Console.WriteLine();
}

// Methods:
void DisplayMenu()
// Displays the menu and options
{
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

(string airlineName, string boardingGate, string specialCode) RetrieveFlightDetails(string flightNumber, Flight f)
{
    string airlineName = "";
    string fCode = f.FlightNumber.Split(" ")[0];
    foreach (Airline a in terminal.Airlines.Values)
    {
        if (fCode == a.Code)
        {
            airlineName = a.Name;
        }
    }

    // Retrieving the special code request
    string specialCode = "";
    if (Convert.ToString(f.GetType()) == "S10267338_PRG2Assignment.CFFTFlight")
    {
        specialCode = "CFFT";
    }
    else if (Convert.ToString(f.GetType()) == "S10267338_PRG2Assignment.DDJBFlight")
    {
        specialCode = "DDJBF";
    }
    else if (Convert.ToString(f.GetType()) == "S10267338_PRG2Assignment.LWTTFlight")
    {
        specialCode = "LWTT";
    }

    // Retrieving the boarding gate
    string boardingGate = "";
    foreach (BoardingGate gate in terminal.BoardingGates.Values)
    {
        if (f == gate.Flight)
        {
            boardingGate = gate.GateName;
        }
    }

    return (airlineName, boardingGate, specialCode);
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
            foreach (Flight f in t.Flights.Values)
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

void DisplayLoading(Terminal t)
{
    // Loads number of airlines, boarding gates, and flights
    Console.WriteLine("Loading Airlines..." +
        $"\n{t.Airlines.Count} Airlines Loaded!" +
        "\nLoading Boarding Gates..." +
        $"\n{t.BoardingGates.Count} Boarding Gates Loaded!" +
        "\nLoading Flights..." +
        $"\n{t.Flights.Count} Flights Loaded!");
}

Flight? CreateNewFlight(Terminal t, string flightNum, string origin, string destination, DateTime flightTime, string specialCode)
{
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
    else if (specialCode == "None")
    {
        newFlight = new NORMFlight(flightNum, origin, destination, flightTime);
    }

    return newFlight;
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
            string[] flightDetails = s.Split(",");

            string flightNum = flightDetails[0];
            string origin = flightDetails[1];
            string destination = flightDetails[2];
            DateTime flightTime = Convert.ToDateTime(flightDetails[3]);
            string specialCode = flightDetails[4];

            Flight? newFlight = CreateNewFlight(t, flightNum, origin, destination, flightTime, specialCode);

            t.Flights[flightNum] = newFlight;
        }
    }
}

void LoadAirlines(Terminal t)
{
    using (StreamReader sr = new StreamReader("airlines.csv"))
    {
        // Reads the header of flights.csv
        string? s = sr.ReadLine();

        // Reads the contents of flights.csv and creates flight objects
        while ((s = sr.ReadLine()) != null)
        {
            string[] airlineDetails = s.Split(",");
            string airlineName = airlineDetails[0];
            string airlineCode = airlineDetails[1];
            Airline airline = new Airline(airlineName, airlineCode);
            t.AddAirline(airline);
        }
    }
}

void LoadBoardingGate(Terminal t)
{
    using (StreamReader sr = new StreamReader("boardinggates.csv"))
    {
        // Reads the header of flights.csv
        string? s = sr.ReadLine();

        // Reads the contents of flights.csv and creates flight objects
        while ((s = sr.ReadLine()) != null)
        {
            string[] bgDetails = s.Split(",");
            string bgName = bgDetails[0];
            bool needDDJB = Convert.ToBoolean(bgDetails[1]);
            bool needCFFT = Convert.ToBoolean(bgDetails[2]);
            bool needLWTT = Convert.ToBoolean(bgDetails[3]);
            BoardingGate boardingGate = new BoardingGate(bgName, needDDJB, needCFFT, needLWTT);
            t.AddBoardingGate(boardingGate);
        }
    }
}

void ListFlights(Terminal t)
{
    Console.WriteLine("=============================================" +
        "\nList of Flights for Changi Airport Terminal 5" +
        "\n=============================================" +
        "\nFlight Number   Airline Name           Origin                 Destination            Expected  Departure/Arrival Time");
    foreach (Flight f in t.Flights.Values)
    {
        Console.WriteLine($"{f.FlightNumber, -16}{t.GetAirlineFromFlight(f).Name, -23}{f.Origin, -23}{f.Destination, -23}{f.ExpectedTime, -31}");
    }
}

void ListBoardingGates(Terminal t)
{
    Console.WriteLine("=============================================");
    Console.WriteLine("List of Boarding Gates for Changi Airport Terminal 5");
    Console.WriteLine("=============================================");
    Console.WriteLine($"{"Gate Name",-16}{"DDJB",-23}{"CFFT",-23}LWTT");
    foreach (BoardingGate bg in t.BoardingGates.Values)
    Console.WriteLine($"{bg.GateName,-16}{bg.SupportsDDJB, -23}{bg.SupportsCFFT, -23}{bg.SupportsLWTT}");
}

void DisplayFlightDetails(Flight flight)
{
    Console.WriteLine($"Flight Number: {flight.FlightNumber}");
    Console.WriteLine($"Origin: {flight.Origin}");
    Console.WriteLine($"Destination: {flight.Destination}");
    Console.WriteLine($"Expected Time: {flight.ExpectedTime}");

    if (Convert.ToString(flight.GetType()) == "S10267338_PRG2Assignment.CFFTFlight")
    {
        Console.WriteLine($"Special Request Code: CFFT");
    }
    else if (Convert.ToString(flight.GetType()) == "S10267338_PRG2Assignment.DDJBFlight")
    {
        Console.WriteLine($"Special Request Code: DDJB");
    }
    else if (Convert.ToString(flight.GetType()) == "S10267338_PRG2Assignment.LWTTFlight")
    {
        Console.WriteLine($"Special Request Code: LWTT");
    }
    else if (Convert.ToString(flight.GetType()) == "S10267338_PRG2Assignment.NORMFlight")
    {
        Console.WriteLine($"Special Request Code: None");
    }
}

void DisplayBoardingGateDetails(BoardingGate gate)
{
    Console.WriteLine("Boarding Gate Name: " + gate.GateName);
    Console.WriteLine("Supports DDJB: " + gate.SupportsDDJB);
    Console.WriteLine("Supports CFFT: " + gate.SupportsCFFT);
    Console.WriteLine("Supports LWTT: " + gate.SupportsLWTT);
}

void UpdateFlightStatus(Flight flight)
{
    Console.WriteLine("Would you like to update the status of the flight? (Y/N)");
    string? choice = Console.ReadLine();

    if (choice == "Y")
    {
        Console.WriteLine("1. Delayed");
        Console.WriteLine("2. Boarding");
        Console.WriteLine("3. On Time");

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

void AppendFlightData(Flight flight, string specialCode)
{
    using (StreamWriter sw = new StreamWriter("flights.csv", true))
    {
        string data = $"{flight.FlightNumber},{flight.Origin},{flight.Destination},{flight.ExpectedTime.ToString("h:mm tt")},{specialCode}";
        sw.WriteLine(data);
    }
}
