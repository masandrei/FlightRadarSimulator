using System.IO;
using System.Text;
using System.Text.Json;
using NetworkSourceSimulator;

namespace flyingApp
{
    public class Airport : IBaseObject, IReportable, IObserver
    {
        public ulong ObjectID { get; set; }
        public string ObjectType { get; set; } = "Airport";
        public string Name { get; set; }
        public string Code { get; set; }
        public float Longitude { get; set; }
        public float Latitude { get; set; }
        public float AMSL { get; set; }
        public string Country { get; set; }
        private static Dictionary<ulong, Airport> AirportId = new Dictionary<ulong, Airport>();

        public static Airport? getAirportById(ulong key)
        {
            if (AirportId.TryGetValue(key, out Airport? ai))
            {
                return ai;
            }
            else
            {
                Console.WriteLine("No such airport!");
                return null;
            }
        }

        public Airport() { }

        public Airport(
            ulong _id,
            string _name,
            string _code,
            float _long,
            float _lat,
            float _amsl,
            string _country
        )
        {
            ObjectID = _id;
            Name = _name;
            Code = _code;
            Longitude = _long;
            Latitude = _lat;
            AMSL = _amsl;
            Country = _country;
            AirportId.TryAdd(ObjectID, this);
        }

        public override string ToString()
        {
            return $"Airport Name: {Name}, Code: {Code}, Longitude: {Longitude}, Latitude: {Latitude}, AMSL: {AMSL}, Country: {Country}";
        }

        public string Accept(INewsProvider inp)
        {
            return inp.NewsAirport(this);
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
                ushort nameLength = BitConverter.ToUInt16(mes, 15);
                Name = Encoding.ASCII.GetString(mes, 17, nameLength);
                Code = Encoding.ASCII.GetString(mes, 17 + nameLength, 3);
                Longitude = BitConverter.ToSingle(mes, 20 + nameLength);
                Latitude = BitConverter.ToSingle(mes, 24 + nameLength);
                AMSL = BitConverter.ToSingle(mes, 28 + nameLength);
                Country = Encoding.ASCII.GetString(mes, 32 + nameLength, 3);
                AirportId.TryAdd(ObjectID, this);
            }
        }

        public void Update(IDUpdateArgs id)
        {
            this.ObjectID = id.NewObjectID;
        }

        public void Update(PositionUpdateArgs pos)
        {

        }

        public void Update(ContactInfoUpdateArgs con)
        {

        }
    }
}
