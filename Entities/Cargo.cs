using System.IO;
using System.Text;
using System.Text.Json;
using NetworkSourceSimulator;

namespace flyingApp
{
    class Cargo : IBaseObject, IObserver
    {
        public ulong ObjectID { get; set; }
        public string ObjectType { get; set; } = "Cargo";
        public float Weight { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }

        public Cargo() { }

        public Cargo(ulong _id, float _weight, string _code, string _description)
        {
            ObjectID = _id;
            ObjectType = "Cargo";
            Weight = _weight;
            Code = _code;
            Description = _description;
        }

        public override string ToString()
        {
            return $"Cargo, Weight: {Weight} tons, Code: {Code}, Description: {Description}";
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
                Weight = BitConverter.ToSingle(mes, 15);
                Code = Encoding.ASCII.GetString(mes, 19, 6);
                ushort descriptionLength = BitConverter.ToUInt16(mes, 25);
                Description = Encoding.ASCII.GetString(mes, 27, descriptionLength);
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
