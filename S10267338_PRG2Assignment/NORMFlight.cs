using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//==========================================================
// Student Number : S10267338C
// Student Name : Tang Kah Yan
// Partner Name : Wyse Lee Hong Yao
//==========================================================

namespace S10267338_PRG2Assignment
{
    class NORMFlight : Flight
    {
        //Constructor
        public NORMFlight() : base() { }
        public NORMFlight(string flightNumber, string origin, string destination, DateTime expectedTime) : base(flightNumber, origin, destination, expectedTime) { }

        //Methods
        public override double CalculateFees()
        {
            return base.CalculateFees() - 50;
        }
        

        public override string ToString()
        {
            return base.ToString() + $"Fees: ${CalculateFees():F2}";
        }
    }
}
