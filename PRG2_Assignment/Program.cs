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
    Console.WriteLine("Flight Number   Airline Name           Origin                 Destination            Expected Departure/Arrival Time");
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
    Console.WriteLine($"{"Gate Name",-16}{ "DDJB",-23}{ "CFFT",-23}LWTT");
    foreach (BoardingGate bg in t.BoardingGates.Values)
    Console.WriteLine($"{bg.GateName,-16}{bg.SupportsDDJB, -23}{bg.SupportsCFFT, -23}{bg.SupportsLWTT}");
}
