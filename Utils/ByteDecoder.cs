using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace flyingApp
{
    class ByteDecoder
    {
        private static Dictionary<string, Func<byte[], IBaseObject>> Factory = new Dictionary<
            string,
            Func<byte[], IBaseObject>
        >()
        {
            { "NAI", info => CreateAirport(info) },
            { "NCA", info => CreateCargo(info) },
            { "NPP", info => CreatePassengerPlane(info) },
            { "NCP", info => CreateCargoPlane(info) },
            { "NPA", info => CreatePassenger(info) },
            { "NFL", info => CreateFlight(info) },
            { "NCR", info => CreateCrew(info) }
        };

        public static IBaseObject ByteMessageDecode(byte[] mes)
        {
            using (MemoryStream mem = new MemoryStream(mes))
            using (BinaryReader reader = new BinaryReader(mem))
            {
                string ID = Encoding.ASCII.GetString(reader.ReadBytes(3));
                if (Factory.TryGetValue(ID, out var Method))
                {
                    return Method(mes);
                }
                else
                {
                    Console.WriteLine("Failed to find this key!");
                }
            }
            return null;
        }

        public static Crew CreateCrew(byte[] mes)
        {
            Crew crewMember = new Crew();
            crewMember.BuildFromBytes(mes);
            return crewMember;
        }

        public static Passenger CreatePassenger(byte[] mes)
        {
            Passenger passenger = new Passenger();
            passenger.BuildFromBytes(mes);
            return passenger;
        }

        public static Cargo CreateCargo(byte[] mes)
        {
            Cargo cargo = new Cargo();
            cargo.BuildFromBytes(mes);
            return cargo;
        }

        public static PassengerPlane CreatePassengerPlane(byte[] mes)
        {
            PassengerPlane pp = new PassengerPlane();
            pp.BuildFromBytes(mes);
            return pp;
        }

        public static CargoPlane CreateCargoPlane(byte[] mes)
        {
            CargoPlane cp = new CargoPlane();
            cp.BuildFromBytes(mes);
            return cp;
        }

        public static Airport CreateAirport(byte[] mes)
        {
            Airport ai = new Airport();
            ai.BuildFromBytes(mes);
            return ai;
        }

        public static Flight CreateFlight(byte[] mes)
        {
            Flight fl = new Flight();
            fl.BuildFromBytes(mes);
            return fl;
        }
    }
}
