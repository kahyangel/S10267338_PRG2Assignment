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
    abstract class Flight : IComparable<Flight>
    {
        public string FlightNumber { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public DateTime ExpectedTime { get; set; }
        public string Status { get; set; } = "On Time";

        public Flight() { }
        public Flight(string flightNumber, string origin, string destination, DateTime expectedTime)
        {
            FlightNumber = flightNumber;
            Origin = origin;
            Destination = destination;
            ExpectedTime = expectedTime;
        }

        public abstract double CalculateFees();

        public int CompareTo(Flight f)
        {
            return ExpectedTime.CompareTo(f.ExpectedTime);
        }

        public override string ToString()
        {
            return $"{FlightNumber}\t{Origin}\t{Destination}\t{ExpectedTime}";
        }
    }
}
