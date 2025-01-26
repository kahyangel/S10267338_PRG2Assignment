using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

//==========================================================
// Student Number : S10268134D
// Student Name : Wyse Lee Hong Yao
// Partner Name : Tang Kah Yan
//==========================================================

namespace S10267338_PRG2Assignment
{
    class Flight : IComparable<Flight>
    {
        public string FlightNumber { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public DateTime ExpectedTime { get; set; }
        public string Status { get; set; } = "Scheduled";

        //public Flight() { }
        public Flight(string flightNumber, string origin, string destination, DateTime expectedTime)
        {
            FlightNumber = flightNumber;
            Origin = origin;
            Destination = destination;
            ExpectedTime = expectedTime;
        }

        public virtual double CalculateFees()
        {
            double totalFee = 0;

            if (Origin == "Singapore (SIN)")
            {
                totalFee = 800;
            }
            else if (Destination == "Singapore (SIN)")
            {
                totalFee = 500;
            }

            return totalFee;
        }

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
