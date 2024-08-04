using System.IO;
using System.Text;
using System.Text.Json;
using NetworkSourceSimulator;

namespace flyingApp
{
    public class Passenger : IBaseObject, IObserver
    {
        public ulong ObjectID { get; set; }
        public string ObjectType { get; set; } = "Passenger";
        public string Name { get; set; }
        public ulong Age { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Class { get; set; }
        public ulong Miles { get; set; }

        public Passenger() { }

        public Passenger(
            ulong _id,
            string _name,
            ulong _age,
            string _phone,
            string _email,
            string _class,
            ulong _miles
        )
        {
            ObjectID = _id;
            Name = _name;
            Age = _age;
            Phone = _phone;
            Email = _email;
            Miles = _miles;
            Class = _class;
        }

        public override string ToString()
        {
            return $"Passenger, Name: {Name}, Age: {Age}, Phone: {Phone}, Email: {Email}, Class: {Class}, Miles: {Miles}";
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
                Age = BitConverter.ToUInt16(mes, 17 + nameLength);
                Phone = Encoding.ASCII.GetString(mes, 19 + nameLength, 12);
                ushort emailLength = BitConverter.ToUInt16(mes, 31 + nameLength);
                Email = Encoding.ASCII.GetString(mes, 33 + nameLength, emailLength);
                Class = Encoding.ASCII.GetString(mes, 33 + nameLength + emailLength, 1);
                Miles = BitConverter.ToUInt64(mes, 34 + nameLength + emailLength);
            }
        }

        public void Update(IDUpdateArgs id)
        {
            //Console.WriteLine($"Changing ID from {this.ObjectID}, {id.NewObjectID}");
            this.ObjectID = id.NewObjectID;
        }

        public void Update(PositionUpdateArgs pos)
        {
            //Console.WriteLine("No such parameter");
        }

        public void Update(ContactInfoUpdateArgs con)
        {
            this.Phone = con.PhoneNumber;
            this.Email = con.EmailAddress;
        }
    }
}
