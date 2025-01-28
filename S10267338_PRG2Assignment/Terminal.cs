using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//==========================================================
// Student Number : S10268134D
// Student Name : Wyse Lee Hong Yao
// Partner Name : Tang Kah Yan
//==========================================================

namespace S10267338_PRG2Assignment
{
    class Terminal
    {
        public string TerminalName { get; set; }
        public Dictionary<string, Airline> Airlines { get; set; } = new Dictionary<string, Airline>();
        public Dictionary<string, Flight> Flights { get; set; } = new Dictionary<string, Flight>();
        public Dictionary<string, BoardingGate> BoardingGates { get; set; } = new Dictionary<string, BoardingGate>();
        public Dictionary<string, double> GateFees { get; set; } = new Dictionary<string, double>();

        public Terminal() { }
        public Terminal(string terminalName)
        {
            TerminalName = terminalName;
        }


        public bool AddAirline(Airline airline)
        {
            if (Airlines.ContainsKey(airline.Code))
            {
                return false;
            }
            Airlines[airline.Code] = airline;
            return true;
        }

        public bool AddBoardingGate(BoardingGate gate)
        {
            if (BoardingGates.ContainsKey(gate.GateName))
            {
                return false;
            }
            BoardingGates[gate.GateName] = gate;
            return true;
        }

        public Airline GetAirlineFromFlight(Flight flight)
        {
            string flightCode = flight.FlightNumber.Substring(0, 2);
            Airline? airline = null;

            foreach (KeyValuePair<string, Airline> kvp in Airlines)
            {
                if (kvp.Key == flightCode)
                {
                    airline = kvp.Value;
                }
            }
            return airline;
        }

        public BoardingGate GetBoardingGateFromFlight(Flight flight)
        {
            BoardingGate? bg = null;
            foreach (BoardingGate gate in BoardingGates.Values)
            {
                if (gate.Flight == flight)
                {
                    bg = gate;
                }
            }
            return bg;
        }

        public void PrintAirlineFees()
        {
            double airlineFee;
            double totalFees = 0;
            double discount = 0;
            int flightCount;
            bool moreThanFiveFlights;

            // Display header
            Console.WriteLine($"{"Airline Name", -23}{"Airline Fee ($)", -15}{"Discount ($)",-10}3% discount applied");

            // For each airline, retrieve all their flights
            foreach (Airline airline in Airlines.Values)
            {
                airlineFee = 0;
                flightCount = 0;
                moreThanFiveFlights = false;
                foreach (Flight flight in airline.Flights.Values)
                {
                    // Keeps track of number of flights
                    flightCount++;

                    // Calculates fee of airline before discounts
                    airlineFee += flight.CalculateFees() + GetBoardingGateFromFlight(flight).CalculateFees();

                    // Discount for every 3 flights
                    if (flightCount % 3 == 0 && flightCount > 0)
                    {
                        discount += 350;
                    }

                    // Discount if flight before 11am or after 9pm
                    if (flight.ExpectedTime.Hour < 11 || flight.ExpectedTime.Hour > 21)
                    {
                        discount += 110;
                    }

                    // Discount if origin of flight is dubai, bangkok or tokyo
                    if (flight.Origin.ToLower() == "dubai (dxb)" || flight.Origin.ToLower() == "bangkok (bkk)" || flight.Origin.ToLower() == "tokyo (nrt)")
                    {
                        discount += 25;
                    }

                    // Discount if flight has no special request code
                    if (flight is NORMFlight)
                    {
                        discount += 50;
                    }

                    // Discount if airline has more than 5 flights
                    if (flightCount > 5)
                    {
                        moreThanFiveFlights = true;
                    }
                }

                // Apply 3% discount where applicable before deduction from total fee of airline
                if (moreThanFiveFlights)
                {
                    airlineFee *= 0.97;
                }

                // Add airline fee with discount to total fees
                totalFees += airlineFee - discount;
                Console.WriteLine($"{airline.Name,-23}{airlineFee,-15}{discount,-10}{moreThanFiveFlights}");
            }

            Console.WriteLine();
            Console.WriteLine("Total fees after discount: $" + totalFees);
        }

        public override string ToString()
        {
            return $"Terminal name: {TerminalName}";
        }
    }
}
