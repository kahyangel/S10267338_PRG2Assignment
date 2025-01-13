using PRG2_Assignment;

// keep track of number of airlines, boarding gates, and flights
int airlineCount = 0;
int boardingGateCount = 0;
int flightCount = 0;

using (StreamReader sr = new StreamReader("airlines.csv"))
{
    // reads the header of flights.csv
    string? s = sr.ReadLine();

    // reads the contents of flights.csv
    while ((s = sr.ReadLine()) != null)
    {
        airlineCount++;
    }
}

using (StreamReader sr = new StreamReader("boardinggates.csv"))
{
    // reads the header of flights.csv
    string? s = sr.ReadLine();

    // reads the contents of flights.csv
    while ((s = sr.ReadLine()) != null)
    {
        boardingGateCount++;
    }
}

using (StreamReader sr = new StreamReader("flights.csv"))
{
    // reads the header of flights.csv
    string? s = sr.ReadLine();
    
    // reads the contents of flights.csv
    while ((s = sr.ReadLine()) != null)
    {
        flightCount++;
    }
}

Console.WriteLine("Loading Airlines..." +
    $"\n{airlineCount} Airlines Loaded!" +
    "\nLoading Boarding Gates..." +
    $"\n{boardingGateCount} Boarding Gates Loaded!" +
    "\nLoading Flights..." +
    $"\n{flightCount} Flights Loaded!");

// Main loop
while (true)
{
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
