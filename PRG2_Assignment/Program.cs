//==========================================================
// Student Name : Wyse Lee Hong Yao (S10268134D)
// Student Name : Tang Kah Yan (S10267338C)
//==========================================================

using PRG2_Assignment;

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

    }
    else if (option == 5)
    {

    }
    else if (option == 6)
    {

    }
    else if (option == 7)
    {

    }
    else if (option == 0)
    {

    }

    Console.WriteLine();
}



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
            else if (specialCode == "")
            {
                newFlight = new NORMFlight(flightNum, origin, destination, flightTime);
            }

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
        "\nFlight Number   Airline Name           Origin                 Destination            Expected Departure/Arrival Time");
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
    if (Convert.ToString(flight.GetType()) == "CFFTFlight")
    {
        Console.WriteLine($"Special Request Code: CFFT");
    }
    else if (Convert.ToString(flight.GetType()) == "DDJBFlight")
    {
        Console.WriteLine($"Special Request Code: DDJB");
    }
    else if (Convert.ToString(flight.GetType()) == "LWTTFlight")
    {
        Console.WriteLine($"Special Request Code: LWTT");
    }
    else if (Convert.ToString(flight.GetType()) == "NORMFlight")
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
