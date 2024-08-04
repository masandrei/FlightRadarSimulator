using System.IO;
using System.Text;
using System.Text.Json;
using NetworkSourceSimulator;

namespace flyingApp
{
    class Crew : IBaseObject, IObserver
    {
        public ulong ObjectID { get; set; }
        public string ObjectType { get; set; } = "Crew";
        public string Name { get; set; }
        public ulong Age { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public ushort Practice { get; set; }
        public string Role { get; set; }

        public Crew() { }

        public Crew(
            ulong _id,
            string _name,
            ulong _age,
            string _phone,
            string _email,
            ushort _pract,
            string _role
        )
        {
            ObjectID = _id;
            Name = _name;
            Age = _age;
            Phone = _phone;
            Email = _email;
            Practice = _pract;
            Role = _role;
        }

        public override string ToString()
        {
            return $"Crew, Name: {Name}, Age: {Age}, Phone: {Phone}, Email: {Email}, Practice: {Practice}, Role: {Role}";
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
                Practice = BitConverter.ToUInt16(mes, 33 + nameLength + emailLength);
                Role = Encoding.ASCII.GetString(mes, 35 + nameLength + emailLength, 1);
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
