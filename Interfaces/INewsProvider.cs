namespace flyingApp
{
    public interface INewsProvider
    {
        string NewsAirport(Airport ai);
        string NewsCargoPlane(CargoPlane cp);
        string NewsPassengerPlane(PassengerPlane pp);
    }
}
