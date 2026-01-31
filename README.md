# PRG2 Assignment

This project implements a **Flight Information Display System (FIDS)** designed to manage flight information at Terminal 5 of Changi Airport. The system utilizes **Object-Oriented Programming (OOP)** principles to efficiently display real-time flight details, assign boarding gates, and calculate terminal fees for arriving and departing flights.

## Project Overview

The Flight Information Display System (FIDS) provides real-time updates on flight statuses, assigned boarding gates, and other essential flight details, ensuring smooth operations at Terminal 5. The system can process incoming flight data, assign gates based on special requests, and calculate fees based on flight types and airport promotions.

### **Key Features:**
- **Flight Schedule Management**: Displays essential flight details including flight numbers, airline names, origins, destinations, expected departure/arrival times, and statuses (e.g., On Time, Delayed).
- **Boarding Gate Assignment**: Assigns boarding gates to flights based on their special request codes, with a limit of one gate assignment per flight per day.
- **Fee Calculation**: Calculates terminal fees for airlines based on the number of flights, special requests, and gate usage. Includes discounts for certain conditions.
- **Bulk Flight Processing**: Processes unassigned flights in bulk and assigns boarding gates automatically, ensuring efficiency in gate allocation.

## Technologies Used:
- **C#**: The primary language for this project.
- **Object-Oriented Programming (OOP)**: The system is built using OOP principles for structured and maintainable code.
- **CSV Files**: Flight, airline, and boarding gate data are loaded from CSV files (`airlines.csv`, `boardinggates.csv`, `flights.csv`).
