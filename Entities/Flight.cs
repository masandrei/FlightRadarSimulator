using System.Globalization;
using System.IO;
using System.Text;
using System.Text.Json;
using FlightTrackerGUI;
using NetworkSourceSimulator;

namespace flyingApp
{
    public class Flight : IBaseObject, IObserver
    {
        public ulong ObjectID { get; set; }
        public string ObjectType { get; set; } = "Flight";
        public Airport OriginID { get; set; }
        public Airport TargetID { get; set; }
        public DateTime TakeoffTime { get; set; }
        public DateTime LandingTime { get; set; }
        public float Longitude { get; set; }
        public float Latitude { get; set; }
        public float AMSL { get; set; }
        public ulong PlaneID { get; set; }
        public ulong[] CrewID { get; set; }
        public ulong[] LoadID { get; set; }

        public Flight() { }

        public Flight(
            ulong id,
            ulong originID,
            ulong targetID,
            string takeoffTime,
            string landingTime,
            float longitude,
            float latitude,
            float amsl,
            ulong planeID,
            ulong[] crewID,
            ulong[] loadID
        )
        {
            ObjectID = id;
            OriginID = Airport.getAirportById(originID);
            TargetID = Airport.getAirportById(targetID);
            TakeoffTime = DateTime.ParseExact(takeoffTime, "HH:mm", CultureInfo.InvariantCulture);
            LandingTime = DateTime.ParseExact(landingTime, "HH:mm", CultureInfo.InvariantCulture);
            Latitude =
                OriginID.Latitude
                + (TargetID.Latitude - OriginID.Latitude)
                    / (FlightRadar.getTotalSecondsDifference(TakeoffTime, LandingTime))
                    * (FlightRadar.getTotalSecondsDifference(TakeoffTime, DateTime.Now));
            Longitude =
                OriginID.Longitude
                + (TargetID.Longitude - OriginID.Longitude)
                    / (FlightRadar.getTotalSecondsDifference(TakeoffTime, LandingTime))
                    * (FlightRadar.getTotalSecondsDifference(TakeoffTime, DateTime.Now));
            AMSL = amsl;
            PlaneID = planeID;
            CrewID = crewID;
            LoadID = loadID;
        }

        public override string ToString()
        {
            return $"Flight, Origin ID: {OriginID.ObjectID}, Target ID: {TargetID.ObjectID}, Takeoff Time: {TakeoffTime}, Landing Time: {LandingTime}, Longitude: {Longitude}, Latitude: {Latitude}, AMSL: {AMSL}, Plane ID: {PlaneID}, Crew IDs: [{string.Join(", ", CrewID)}], Load IDs: [{string.Join(", ", LoadID)}]";
        }

        public string ToJson()
        {
            return JsonSerializer.Serialize(this);
        }

        public void BuildFromBytes(byte[] mes)
        {
            using (MemoryStream mem = new MemoryStream(mes))
            using (BinaryReader reader = new BinaryReader(mem))
            {
                string ClassType = Encoding.ASCII.GetString(mes, 0, 3);
                ObjectID = BitConverter.ToUInt64(mes, 7);
                OriginID = Airport.getAirportById(BitConverter.ToUInt64(mes, 15));
                TargetID = Airport.getAirportById(BitConverter.ToUInt64(mes, 23));
                TakeoffTime = new DateTime(BitConverter.ToInt64(mes, 31) * 10000, DateTimeKind.Utc);
                LandingTime = new DateTime(BitConverter.ToInt64(mes, 39) * 10000, DateTimeKind.Utc);
                PlaneID = BitConverter.ToUInt64(mes, 47);
                int crewNum = BitConverter.ToUInt16(mes, 55);
                CrewID = new ulong[crewNum];
                for (int i = 0; i < crewNum; i++)
                {
                    CrewID[i] = BitConverter.ToUInt64(mes, 57 + i * 8);
                }

                int loadNum = BitConverter.ToUInt16(mes, 57 + crewNum * 8);
                LoadID = new ulong[loadNum];
                for (int i = 0; i < loadNum; i++)
                {
                    LoadID[i] = BitConverter.ToUInt64(mes, 59 + crewNum * 8 + i * 8);
                }
            }
        }

        public void Update(IDUpdateArgs id)
        {
            //Console.WriteLine($"Changing ID from {this.ObjectID}, {id.NewObjectID}");
            this.ObjectID = id.NewObjectID;
        }

        public virtual void Update(PositionUpdateArgs pos)
        {
            // Console.WriteLine(
            //     $"ID: {this.ObjectID}, Latitude:{this.Latitude}, Longitude:{this.Longitude}"
            // );
            this.Longitude = pos.Longitude;
            this.Latitude = pos.Latitude;
            this.Movement();
            AMSL = pos.AMSL;
            // Console.WriteLine(
            //     $"ID: {this.ObjectID}, Latitude:{this.Latitude}, Longitude:{this.Longitude}"
            // );
        }

        public void Update(ContactInfoUpdateArgs con)
        {
            //Console.WriteLine("No such parameter");
        }

        public virtual void Movement()
        {
            (float, float) InitPosTuple = (this.Longitude, this.Latitude);
            (float, float) EndPosTuple = (this.TargetID.Longitude, this.TargetID.Latitude);
            this.Longitude =
                this.Longitude
                + (EndPosTuple.Item1 - InitPosTuple.Item1)
                    / (FlightRadar.getTotalSecondsDifference(this.TakeoffTime, this.LandingTime));
            this.Latitude =
                this.Latitude
                + (EndPosTuple.Item2 - InitPosTuple.Item2)
                    / (FlightRadar.getTotalSecondsDifference(this.TakeoffTime, this.LandingTime));
        }

        public double CalcRotation()
        {
            double num = Math.Atan2(
                this.TargetID.Longitude - this.Longitude,
                this.TargetID.Latitude - this.Latitude
            );

            return num;
        }

        public virtual FlightGUI ConvertFlight()
        {
            return new FlightGUI
            {
                ID = this.ObjectID,
                WorldPosition = new WorldPosition(this.Latitude, this.Longitude),
                MapCoordRotation = this.CalcRotation()
            };
        }
    }
}
