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

namespace PRG2_Assignment
{
    class BoardingGate
    {
        public string GateName { get; set; }
        public bool SupportsCFFT { get; set; }
        public bool SupportsDDJB { get; set; }
        public Flight Flight { get; set; }

        public BoardingGate() { }
        public BoardingGate(string gateName, bool supportsCFFT, bool supportsDDJB, Flight flight)
        {
            GateName = gateName;
            SupportsCFFT = supportsCFFT;
            SupportsDDJB = supportsDDJB;
            Flight = flight;
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
