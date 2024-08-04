using System.Collections.Generic;
using System.Text.Json;

namespace flyingApp
{
    public class Factory
    {
        private static Dictionary<string, Func<string[], IBaseObject>> dict = new Dictionary<
            string,
            Func<string[], IBaseObject>
        >
        {
            { "AI", info => CreateAirport(info) },
            { "CA", info => CreateCargo(info) },
            { "PP", info => CreatePP(info) },
            { "CP", info => CreateCP(info) },
            { "P", info => CreatePassenger(info) },
            { "FL", info => CreateFlight(info) },
            { "C", info => CreateCrew(info) }
        };
        private static Dictionary<string, IBaseObject> defaultDict = new Dictionary<
            string,
            IBaseObject
        >
        {
            { "Airport", new Airport() },
            { "Cargo", new Cargo() },
            { "PassengerPlane", new PassengerPlane() },
            { "CargoPlane", new CargoPlane() },
            { "Passenger", new Passenger() },
            { "Flight", new Flight() },
            { "Crew", new Crew() }
        };

        public static IBaseObject getElement(string[] info)
        {
            if (dict.TryGetValue(info[0], out Func<string[], IBaseObject> result))
            {
                try
                {
                    return result(info);
                }
                catch (Exception ex)
                {
                    throw new Exception("Failed to parse data" + ex.Message);
                }
            }
            else
            {
                throw new Exception("Can't find this key!");
            }
        }

        public static IBaseObject getDefaultElement(string _class)
        {
            if (defaultDict.TryGetValue(_class, out IBaseObject result))
            {
                return result;
            }
            else
            {
                throw new Exception("Can't find this key!");
            }
        }

        private static Airport CreateAirport(string[] data)
        {
            ulong id;
            Parser.ParseUInt64(data[1], "Airport ID", out id);
            string name = data[2];
            string city = data[3];
            float latitude,
                longitude,
                altitude;
            Parser.ParseSingle(data[4], "latitude", out latitude);
            Parser.ParseSingle(data[5], "longitude", out longitude);
            Parser.ParseSingle(data[6], "altitude", out altitude);

            string country = data[7];

            return new Airport(id, name, city, latitude, longitude, altitude, country);
        }

        private static Flight CreateFlight(string[] info)
        {
            string[] numbersStrings = info[9].Replace("[", "").Replace("]", "").Split(";");
            ulong[] crew;
            Parser.TryParseArray(numbersStrings, "Crew Array", out crew);

            numbersStrings = info[10].Replace("[", "").Replace("]", "").Split(";");
            ulong[] load;
            Parser.TryParseArray(numbersStrings, "Load Array", out load);

            ulong flightId;
            Parser.ParseUInt64(info[1], "flightId", out flightId);
            ulong departureAirportId;
            Parser.ParseUInt64(info[2], "departureAirportId", out departureAirportId);
            ulong arrivalAirportId;
            Parser.ParseUInt64(info[3], "arrivalAirportId", out arrivalAirportId);
            string takeoffTime = info[4];
            string landingTime = info[5];
            float longitude;
            Parser.ParseSingle(info[6], "longitude", out longitude);
            float latitude;
            Parser.ParseSingle(info[7], "latitude", out latitude);
            float AMSL;
            Parser.ParseSingle(info[8], "AMSL", out AMSL);
            ulong aircraftId;
            Parser.ParseUInt64(info[9], "aircraftId", out aircraftId);

            return new Flight(
                flightId,
                departureAirportId,
                arrivalAirportId,
                takeoffTime,
                landingTime,
                longitude,
                latitude,
                AMSL,
                aircraftId,
                crew,
                load
            );
        }

        private static PassengerPlane CreatePP(string[] info)
        {
            ulong id;
            Parser.ParseUInt64(info[1], "Plane ID", out id);
            string serial = info[2];
            string country = info[3];
            string model = info[4];
            ushort firstClassSize,
                businessClassSize,
                economClassSize;
            Parser.ParseUInt16(info[5], "first class size", out firstClassSize);
            Parser.ParseUInt16(info[6], "business class size", out businessClassSize);
            Parser.ParseUInt16(info[7], "economy class size", out economClassSize);

            return new PassengerPlane(
                id,
                serial,
                country,
                model,
                firstClassSize,
                businessClassSize,
                economClassSize
            );
        }

        private static CargoPlane CreateCP(string[] info)
        {
            ulong id;
            Parser.ParseUInt64(info[1], "Plane ID", out id);
            string serial = info[2];
            string country = info[3];
            string model = info[4];
            float maxWeight;
            Parser.ParseSingle(info[5], "max weight", out maxWeight);
            return new CargoPlane(id, serial, country, model, maxWeight);
        }

        private static Crew CreateCrew(string[] info)
        {
            ulong id;
            Parser.ParseUInt64(info[1], "Crew ID", out id);
            string name = info[2];
            ulong age;
            Parser.ParseUInt64(info[3], "age", out age);
            string phone = info[4];
            string email = info[5];
            ushort practice;
            Parser.ParseUInt16(info[6], "practice", out practice);
            string role = info[7];

            return new Crew(id, name, age, phone, email, practice, role);
        }

        private static Passenger CreatePassenger(string[] info)
        {
            ulong id;
            Parser.ParseUInt64(info[1], "Passenger ID", out id);
            string name = info[2];
            ulong age;
            Parser.ParseUInt64(info[3], "age", out age);
            string phone = info[4];
            string email = info[5];
            string passengerClass = info[6];
            ulong miles;
            Parser.ParseUInt64(info[7], "miles", out miles);

            return new Passenger(id, name, age, phone, email, passengerClass, miles);
        }

        private static Cargo CreateCargo(string[] info)
        {
            ulong id;
            Parser.ParseUInt64(info[1], "Cargo ID", out id);
            float weight;
            Parser.ParseSingle(info[2], "weight", out weight);
            string code = info[3];
            string description = info[4];

            return new Cargo(id, weight, code, description);
        }
    }

    public static class Parser
    {
        public static void ParseUInt64(string input, string parsedValueDesc, out ulong output)
        {
            if (!UInt64.TryParse(input, out output))
            {
                throw new FormatException($"Failed to parse {parsedValueDesc}.");
            }
        }

        public static void ParseSingle(string input, string parsedValueDesc, out float output)
        {
            if (!float.TryParse(input, out output))
            {
                throw new FormatException($"Failed to parse {parsedValueDesc}.");
            }
        }

        public static void ParseUInt16(string input, string parsedValueDesc, out ushort output)
        {
            if (!UInt16.TryParse(input, out output))
            {
                throw new FormatException($"Failed to parse {parsedValueDesc}.");
            }
        }

        public static void TryParseArray(
            string[] inputArray,
            string parseValueDesc,
            out ulong[]? outputArray
        )
        {
            outputArray = null;
            List<ulong> parsedNumbers = new List<ulong>();

            foreach (string s in inputArray)
            {
                ulong number;
                if (ulong.TryParse(s, out number))
                {
                    parsedNumbers.Add(number);
                }
                else
                {
                    throw new FormatException($"Failed to parse {parseValueDesc}.");
                }
            }

            outputArray = parsedNumbers.ToArray();
        }
    }
}
