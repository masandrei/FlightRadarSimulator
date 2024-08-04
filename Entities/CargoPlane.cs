using System.IO;
using System.Text;
using System.Text.Json;
using NetworkSourceSimulator;

namespace flyingApp
{
    public class CargoPlane : IBaseObject, IReportable, IObserver
    {
        public ulong ObjectID { get; set; }
        public string ObjectType { get; set; } = "CargoPlane";
        public string Serial { get; set; }
        public string Country { get; set; }
        public string Model { get; set; }
        public float MaxWeight { get; set; }

        public CargoPlane() { }

        public CargoPlane(
            ulong _id,
            string _serial,
            string _country,
            string _model,
            float _maxWeight
        )
        {
            ObjectID = _id;
            Serial = _serial;
            Country = _country;
            Model = _model;
            MaxWeight = _maxWeight;
        }

        public override string ToString()
        {
            return $"Cargo Plane, Serial: {Serial}, Country: {Country}, Model: {Model}, Max Weight: {MaxWeight} tons";
        }

        public string Accept(INewsProvider inp)
        {
            return inp.NewsCargoPlane(this);
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
                Serial = Encoding.ASCII.GetString(mes, 15, 10);
                Country = Encoding.ASCII.GetString(mes, 25, 3);
                ushort modelLength = BitConverter.ToUInt16(mes, 28);
                Model = Encoding.ASCII.GetString(mes, 30, modelLength);
                MaxWeight = BitConverter.ToSingle(mes, 30 + modelLength);
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
