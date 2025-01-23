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
    class Airline
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public Dictionary<string, Flight> Flights { get; set; } = new Dictionary<string, Flight>();

        public Airline() { }
        public Airline(string name, string code)
        {
            Name = name;
            Code = code;
        }

        public bool AddFlight(Flight f)
        {
            if (Flights.ContainsKey(f.FlightNumber))
            {
                return false;
            }
            Flights[f.FlightNumber] = f;
            return true;
        }

        //public double CalculateFees()
        //{

        //}

        public bool RemoveFlight(Flight f)
        {
            if (Flights.ContainsKey(f.FlightNumber))
            {
                Flights.Remove(f.FlightNumber);
                return true;
            }
            return false;
        }

        public override string ToString()
        {
            return "";
        }
    }
}
