﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRG2_Assignment
{
    class NORMFlight : Flight
    {
        //Constructor
        public NORMFlight() { }
        public NORMFlight(string flightNumber, string origin, string destination, DateTime expectedTime) : base(flightNumber, origin, destination, expectedTime) { }

        //Methods
        public override double CalculateFees()
        {
            double totalFee = 300;
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
