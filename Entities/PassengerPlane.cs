using System.IO;
using System.Text;
using System.Text.Json;
using NetworkSourceSimulator;

namespace flyingApp
{
    public class PassengerPlane : IBaseObject, IReportable, IObserver
    {
        public ulong ObjectID { get; set; }
        public string ObjectType { get; set; } = "PassengerPlane";
        public string Serial { get; set; }
        public string Country { get; set; }
        public string Model { get; set; }
        public ushort FirstClassSize { get; set; }
        public ushort BusinessClassSize { get; set; }
        public ushort EconomClassSize { get; set; }

        public PassengerPlane() { }

        public PassengerPlane(
            ulong _id,
            string _serial,
            string _country,
            string _model,
            ushort _firstClass,
            ushort _businessClass,
            ushort _ecoClass
        )
        {
            ObjectID = _id;
            Serial = _serial;
            Country = _country;
            Model = _model;
            FirstClassSize = _firstClass;
            BusinessClassSize = _businessClass;
            EconomClassSize = _ecoClass;
        }

        public override string ToString()
        {
            return $"PP, Serial: {Serial}, Country: {Country}, Model: {Model}, First Class Size: {FirstClassSize}, Business Class Size: {BusinessClassSize}, Economy Class Size: {EconomClassSize}";
        }

        public string Accept(INewsProvider inp)
        {
            return inp.NewsPassengerPlane(this);
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
                FirstClassSize = BitConverter.ToUInt16(mes, 30 + modelLength);
                BusinessClassSize = BitConverter.ToUInt16(mes, 32 + modelLength);
                EconomClassSize = BitConverter.ToUInt16(mes, 34 + modelLength);
            }
        }

        public void Update(IDUpdateArgs id)
        {
            this.ObjectID = id.NewObjectID;
        }

        public void Update(PositionUpdateArgs pos) { }

        public void Update(ContactInfoUpdateArgs con) { }
    }
}
