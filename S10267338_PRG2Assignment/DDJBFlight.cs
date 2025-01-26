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
    class DDJBFlight : Flight
    {
        //Property
        public double RequestFee { get; set; }

        //Constructor
        //public DDJBFlight() : base() { }
        public DDJBFlight(string flightNumber, string origin, string destination, DateTime expectedTime) : base(flightNumber, origin, destination, expectedTime)
        {
            RequestFee = 300;
        }

        //Methods
        public override double CalculateFees()
        {
            return RequestFee + base.CalculateFees();
        }

        public override string ToString()
        {
            return base.ToString() + $"Fees: ${CalculateFees():F2}";
        }
        
    }
}
