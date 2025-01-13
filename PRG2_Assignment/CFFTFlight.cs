using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRG2_Assignment
{
    class CFFTFlight : Flight
    {
        //Property
        public double RequestFee { get; set; }

        //Constructor
        public CFFTFlight() { }
        public CFFTFlight(string flightNumber, string origin, string destination, DateTime expectedTime, string status, double requestFee) : base(flightNumber, origin, destination, expectedTime, status)
        {
            RequestFee = requestFee;
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
