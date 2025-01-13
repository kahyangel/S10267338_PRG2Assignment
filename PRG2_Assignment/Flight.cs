using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRG2_Assignment
{
    class Flight
    {
        public string FlightNumber { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public DateTime ExpectedTime { get; set; }
        public string Status { get; set; }

        public Flight() { }
        public Flight(string flightNumber, string origin, string destination, DateTime expectedTime, string status)
        {
            FlightNumber = flightNumber;
            Origin = origin;
            Destination = destination;
            ExpectedTime = expectedTime;
            Status = status;
        }

        public double CalculateFees()
        {

        }

        public override string ToString()
        {
            return "";
        }
    }
}
