using System;
using System.Collections.Generic;
using System.Globalization;
using FlightTrackerGUI;
using Mapsui.Projections;

namespace flyingApp
{
    class FlightRadar
    {
        public static async void CreateRadar(List<IBaseObject> list)
        {
            List<Flight> FlightsList = new List<Flight>();
            Thread thread = new Thread(async () =>
            {
                PeriodicTimer timer = new PeriodicTimer(TimeSpan.FromSeconds(1));
                while (await timer.WaitForNextTickAsync())
                {
                    FlightsList.Clear();
                    foreach (IBaseObject obj in list)
                    {
                        if (obj is Flight fl && IsFlightActive(fl))
                            FlightsList.Add(fl);
                    }
                    UpdateFlightPositions(FlightsList);
                }
            });
            thread.Start();
            Runner.Run();
        }

        private static void UpdateFlightPositions(List<Flight> flights)
        {
            foreach (Flight flight in flights)
            {
                flight.Movement();
            }
            FlightsGUIData temp = ConvertData(flights);
            Runner.UpdateGUI(temp);
        }

        public static float getTotalSecondsDifference(DateTime TimeInit, DateTime TimeEnd)
        {
            if (TimeEnd <= TimeInit)
            {
                TimeEnd = TimeEnd.AddDays(1);
            }
            float TimeDiff = (float)(TimeEnd.Subtract(TimeInit)).TotalSeconds;
            return TimeDiff;
        }

        private static bool IsFlightActive(Flight flight)
        {
            DateTime TimeInit = flight.TakeoffTime;
            DateTime TimeEnd = flight.LandingTime;
            if (TimeEnd < TimeInit)
            {
                TimeEnd = TimeEnd.AddDays(1);
            }
            return DateTime.Now <= TimeEnd && DateTime.Now >= TimeInit;
        }

        private static FlightsGUIData ConvertData(List<Flight> flights)
        {
            List<FlightGUI> list = new List<FlightGUI>();
            foreach (Flight flight in flights)
            {
                list.Add(flight.ConvertFlight());
            }

            return new FlightsGUIData(list);
        }
    }
}
