using System;
using System.Collections.Generic;
using System.Globalization;

namespace flyingApp
{
    public static class Setter
    {
        public static Dictionary<string, Action<IBaseObject, string>>? GetSetter(string type)
        {
            if (ModelSetter.TryGetValue(type, out var dict))
            {
                return dict;
            }
            else
                return null;
        }

        private static ulong ParseULong(string value)
        {
            return ulong.TryParse(value, out var result) ? result : default;
        }

        private static ushort ParseUShort(string value)
        {
            return ushort.TryParse(value, out var result) ? result : default;
        }

        private static float ParseFloat(string value)
        {
            return float.TryParse(
                value,
                NumberStyles.Float,
                CultureInfo.InvariantCulture,
                out var result
            )
                ? result
                : default;
        }

        private static Dictionary<string, Action<IBaseObject, string>> FlightProperties =
            new Dictionary<string, Action<IBaseObject, string>>()
            {
                { "ObjectID", (obj, value) => ((Flight)obj).ObjectID = ParseULong(value) },
                {
                    "OriginID",
                    (obj, value) =>
                        ((Flight)obj).OriginID = Airport.getAirportById(ParseULong(value))
                },
                {
                    "TargetID",
                    (obj, value) =>
                        ((Flight)obj).TargetID = Airport.getAirportById(ParseULong(value))
                },
                {
                    "TakeoffTime",
                    (obj, value) =>
                        ((Flight)obj).TakeoffTime = DateTime.TryParseExact(
                            value,
                            "HH:mm",
                            CultureInfo.InvariantCulture,
                            DateTimeStyles.None,
                            out DateTime takeoffTime
                        )
                            ? takeoffTime
                            : DateTime.Now
                },
                {
                    "LandingTime",
                    (obj, value) =>
                        ((Flight)obj).LandingTime = DateTime.TryParseExact(
                            value,
                            "HH:mm",
                            CultureInfo.InvariantCulture,
                            DateTimeStyles.None,
                            out DateTime landingTime
                        )
                            ? landingTime
                            : DateTime.Now
                },
                { "WorldPosition.Lat", (obj, value) => ((Flight)obj).Latitude = ParseFloat(value) },
                {
                    "WorldPosition.Long",
                    (obj, value) => ((Flight)obj).Longitude = ParseFloat(value)
                },
                { "AMSL", (obj, value) => ((Flight)obj).AMSL = ParseFloat(value) },
                { "PlaneID", (obj, value) => ((Flight)obj).PlaneID = ParseULong(value) }
            };

        private static Dictionary<string, Action<IBaseObject, string>> AirportProperties =
            new Dictionary<string, Action<IBaseObject, string>>()
            {
                { "ObjectID", (obj, value) => ((Airport)obj).ObjectID = ParseULong(value) },
                { "Name", (obj, value) => ((Airport)obj).Name = value },
                { "Code", (obj, value) => ((Airport)obj).Code = value },
                {
                    "WorldPosition.Lat",
                    (obj, value) => ((Airport)obj).Latitude = ParseFloat(value)
                },
                {
                    "WorldPosition.Long",
                    (obj, value) => ((Airport)obj).Longitude = ParseFloat(value)
                },
                { "AMSL", (obj, value) => ((Airport)obj).AMSL = ParseFloat(value) },
                { "Country", (obj, value) => ((Airport)obj).Country = value }
            };

        private static Dictionary<string, Action<IBaseObject, string>> CargoProperties =
            new Dictionary<string, Action<IBaseObject, string>>()
            {
                { "ObjectID", (obj, value) => ((Cargo)obj).ObjectID = ParseULong(value) },
                { "Weight", (obj, value) => ((Cargo)obj).Weight = ParseFloat(value) },
                { "Code", (obj, value) => ((Cargo)obj).Code = value },
                { "Description", (obj, value) => ((Cargo)obj).Description = value }
            };

        private static Dictionary<string, Action<IBaseObject, string>> CargoPlaneProperties =
            new Dictionary<string, Action<IBaseObject, string>>()
            {
                { "ObjectID", (obj, value) => ((CargoPlane)obj).ObjectID = ParseULong(value) },
                { "Serial", (obj, value) => ((CargoPlane)obj).Serial = value },
                { "Country", (obj, value) => ((CargoPlane)obj).Country = value },
                { "Model", (obj, value) => ((CargoPlane)obj).Model = value },
                { "MaxWeight", (obj, value) => ((CargoPlane)obj).MaxWeight = ParseFloat(value) }
            };

        private static Dictionary<string, Action<IBaseObject, string>> CrewProperties =
            new Dictionary<string, Action<IBaseObject, string>>()
            {
                { "ObjectID", (obj, value) => ((Crew)obj).ObjectID = ParseULong(value) },
                { "Name", (obj, value) => ((Crew)obj).Name = value },
                { "Age", (obj, value) => ((Crew)obj).Age = ParseULong(value) },
                { "Phone", (obj, value) => ((Crew)obj).Phone = value },
                { "Email", (obj, value) => ((Crew)obj).Email = value },
                { "Practice", (obj, value) => ((Crew)obj).Practice = ParseUShort(value) },
                { "Role", (obj, value) => ((Crew)obj).Role = value }
            };

        private static Dictionary<string, Action<IBaseObject, string>> PassengerProperties =
            new Dictionary<string, Action<IBaseObject, string>>()
            {
                { "ObjectID", (obj, value) => ((Passenger)obj).ObjectID = ParseULong(value) },
                { "Name", (obj, value) => ((Passenger)obj).Name = value },
                { "Age", (obj, value) => ((Passenger)obj).Age = ParseULong(value) },
                { "Phone", (obj, value) => ((Passenger)obj).Phone = value },
                { "Email", (obj, value) => ((Passenger)obj).Email = value },
                { "Class", (obj, value) => ((Passenger)obj).Class = value },
                { "Miles", (obj, value) => ((Passenger)obj).Miles = ParseULong(value) }
            };

        private static Dictionary<string, Action<IBaseObject, string>> PassengerPlaneProperties =
            new Dictionary<string, Action<IBaseObject, string>>()
            {
                { "ObjectID", (obj, value) => ((PassengerPlane)obj).ObjectID = ParseULong(value) },
                { "Serial", (obj, value) => ((PassengerPlane)obj).Serial = value },
                { "Country", (obj, value) => ((PassengerPlane)obj).Country = value },
                { "Model", (obj, value) => ((PassengerPlane)obj).Model = value },
                {
                    "FirstClassSize",
                    (obj, value) => ((PassengerPlane)obj).FirstClassSize = ParseUShort(value)
                },
                {
                    "BusinessClassSize",
                    (obj, value) => ((PassengerPlane)obj).BusinessClassSize = ParseUShort(value)
                },
                {
                    "EconomyClassSize",
                    (obj, value) => ((PassengerPlane)obj).EconomClassSize = ParseUShort(value)
                }
            };

        private static Dictionary<
            string,
            Dictionary<string, Action<IBaseObject, string>>
        > ModelSetter = new Dictionary<string, Dictionary<string, Action<IBaseObject, string>>>()
        {
            { "Flight", FlightProperties },
            { "Airport", AirportProperties },
            { "Cargo", CargoProperties },
            { "CargoPlane", CargoPlaneProperties },
            { "Crew", CrewProperties },
            { "Passenger", PassengerProperties },
            { "PassengerPlane", PassengerPlaneProperties }
        };
    }
}
