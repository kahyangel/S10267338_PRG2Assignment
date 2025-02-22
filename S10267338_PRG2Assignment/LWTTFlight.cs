﻿using System;
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
    class LWTTFlight : Flight
    {
        //Property
        public double RequestFee { get; set; }

        //Constructor
        public LWTTFlight() : base() { }
        public LWTTFlight(string flightNumber, string origin, string destination, DateTime expectedTime) : base(flightNumber, origin, destination, expectedTime)
        {
            RequestFee = 500;
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
