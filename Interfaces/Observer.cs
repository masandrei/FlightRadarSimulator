using NetworkSourceSimulator;
namespace flyingApp
{
    public interface IObserver
    {
        void Update(IDUpdateArgs args);
        void Update(PositionUpdateArgs args);
        void Update(ContactInfoUpdateArgs args);
    }
}
