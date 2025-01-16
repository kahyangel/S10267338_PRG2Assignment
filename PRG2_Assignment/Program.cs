using PRG2_Assignment;

// Create dictionary to store flight objects with flight number as key
Dictionary<string, Flight> flightDict = new Dictionary<string, Flight>();

LoadFlights(flightDict);
DisplayLoading(airlineDict, boardingGateDict, flightDict);

// Main loop
while (true)
{
    DisplayLoading(airlineDict, boardingGateDict, flightDict);
    DisplayMenu();
    int option = Convert.ToInt32(Console.ReadLine());
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

void DisplayLoading(airlineDict, boardingGateDict, flightDict)
{
    // Loads number of airlines, boarding gates, and flights
    Console.WriteLine("Loading Airlines..." +
        $"\n{airlineDict.Count} Airlines Loaded!" +
        "\nLoading Boarding Gates..." +
        $"\n{boardingGateDict.Count} Boarding Gates Loaded!" +
        "\nLoading Flights..." +
        $"\n{flightDict.Count} Flights Loaded!");
}

void LoadFlights(Dictionary<string, Flight> fDict)
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

            fDict[flightNum] = newFlight;
        }
    }
}
