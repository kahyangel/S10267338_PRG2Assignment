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

namespace PRG2_Assignment
{
    class CFFTFlight : Flight
    {
        //Property
        public double RequestFee { get; set; }

        //Constructor
        public CFFTFlight(): base() { }
        public CFFTFlight(string flightNumber, string origin, string destination, DateTime expectedTime) : base(flightNumber, origin, destination, expectedTime)
        {
            RequestFee = 150;
        }

        //Methods
        public override double CalculateFees()
        {
            double totalFee = 300 + RequestFee;
            if (Origin == "SIN")
            {
                totalFee += 800;
            }
            if (Destination == "SIN")
            {
                totalFee += 500;
            }
            return totalFee;
        }

        public override string ToString()
        {
            return $"Fees: ${CalculateFees():F2}";
        }
    }
}
