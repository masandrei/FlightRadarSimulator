using System;
using System.Text.Json;

namespace flyingApp
{
    public interface IBaseObject
    {
        ulong ObjectID { get; set; }
        string ObjectType { get; set; }
        string ToString();
        string ToJson();
        void BuildFromBytes(byte[] mes);
    }
}
